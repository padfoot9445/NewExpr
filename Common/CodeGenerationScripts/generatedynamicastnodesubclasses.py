import yaml
from pathlib import Path
from typing import Literal, cast
from codegenframework import *
from sys import argv

HEADER_NAME: Literal["dynamicastnode subclasses"] = "dynamicastnode subclasses"
CHILDREN: Literal["children"] = "children"
HAS_DATA: Literal["has data"] = "has data"
VALID_DATA_TYPES: Literal["valid data types"] = "valid data types"
HAS_ADDITIONAL_DVF: Literal["has additional data validation function"] = "has additional data validation function"
DATA_VALIDATION_FUNCTION: Literal["data validation function"] = "data validation function"
NAME: Literal["name"] = "name"
IS_OPTIONAL: Literal["is optional"] = "is optional"
CHECK_DATA_TYPE: Literal["check data type"] = "check data type"
PARENT: Literal["parent"] = "parent"

CLASSES: Literal["classes"] = "classes"
ENUM_TYPE: Literal["enum type"] = "enum type"
ANNOTATION_TYPE: Literal["annotation type"] = "annotation type"
BASE_CLASS_NAME: Literal["base class name"] = "base class name"

USINGS: list[str] = ["Common.Tokens", "Common.AST", "SmallLang.IR.Metadata"]
NAMESPACE: str = "SmallLang.IR.AST.Generated"
childtype = dict[str, str | bool]
classtype = \
dict[ #dictionary representing an individual class
        str, #key
            list[childtype] | #children
            bool | #has data and has additional dvf
            list[str] | #valid data types
            str | Literal[""] #data validation function and class name

        
    ]
classestype = \
list[ #list containing all the classes
    classtype
]

f"""expected structure:

    {HEADER_NAME}:
        {CLASSES}:
            -
                {NAME}: [class name]
                {PARENT}: [Parent Name]
                {CHILDREN}:
                    -
                        {NAME}: [child name]
                        {IS_OPTIONAL}: [yes | no]
                {HAS_DATA}: [yes | no]
                {CHECK_DATA_TYPE}: [yes | no]
                {VALID_DATA_TYPES}:
                    - [data type]
                {HAS_ADDITIONAL_DVF}: [yes | no]
                {DATA_VALIDATION_FUNCTION}: |
                    [valid c# BODY (everything between the curly brackets). Should probably just be a return [Expression]]
        {ENUM_TYPE}: [Enum Type Name]
        {ANNOTATION_TYPE}: [Annotation Type Name]
    """

def generate_dynamicastnode_subclass(subclass: classtype, enum_type: str) -> str:

    ADATA: Literal["AData"] = "AData"
    DVF_NAME: Literal["AdditionalDataValidationFunction"] = "AdditionalDataValidationFunction"
    DATACHECKER_NAME: Literal["DataChecker"] = "DataChecker"

    def get_children_properties(children: list[childtype]):
        for child in children:
            name = cast(str, child[NAME])
            is_optional = cast(bool, child[IS_OPTIONAL])
            yield code_property(name=name, type=(name + ("?" if is_optional else "")), access_modifier=AccessModifiers.Public, setter_access_modifier=AccessModifiers.Private)

    def get_parameters(self: classtype):
        if self[HAS_DATA]:
            yield f"IToken {ADATA}"
        for child in cast(list[childtype], self[CHILDREN]):
            if child[IS_OPTIONAL]:
                yield f"{child[NAME]}? {child[NAME]}"
            else:
                yield f"{child[NAME]} {child[NAME]}"

    def get_delegate_ctor_arguments(self: classtype, enum_type: str):
        yield ("Data" if self[HAS_DATA] else "null") # data
        yield f"[{", ".join(cast(str, child[NAME]) for child in cast(list[childtype], self[CHILDREN]))}]" #children list
        yield f"{enum_type}.{self[NAME]}" #node type enum

    def get_ctor_content(child_names: list[str], has_data: bool, has_dvf: bool, check_data_type: bool):
        if has_data:
            yield f"Data = {ADATA};"
        
        if has_dvf:
            yield f"if(!{DVF_NAME}()) throw new Exception(\"Invalid data submitted\");"

        if check_data_type:
            yield f"if(!{DATACHECKER_NAME}()) throw new Exception(\"Invalid data type submitted\");"
        
        for child_name in child_names:
            yield f"this.{child_name} = {child_name}"

    children: list[childtype] = cast(list[childtype], subclass[CHILDREN])
    content = [i for i in get_children_properties(children)]
    has_data: bool = cast(bool, subclass[HAS_DATA])
    class_name = cast(str, subclass[NAME])
    check_data_type = cast(bool, subclass[CHECK_DATA_TYPE])
    has_dvf = cast(bool, subclass[HAS_ADDITIONAL_DVF])

    if has_data:
        content.append(
            code_property("Data", "IToken", access_modifier="public new", getter_access_modifier=AccessModifiers.Public, setter_access_modifier=AccessModifiers.Private)
        )

    content.append(
        f"public bool HasData => {"true" if has_data else "false"};"
    )

    if check_data_type:
        content.append(code_method(
            method_name=DATACHECKER_NAME,
            content=[
                f"if(Data.TT == TokenType.{TTName}) return true;"
                for TTName in cast(list[str], subclass[VALID_DATA_TYPES])
            ] + ["return false;"],
            return_type="bool"
        ))
    
    if has_dvf:
        content.append(code_method(
            DVF_NAME,
            [cast(str, subclass[DATA_VALIDATION_FUNCTION])],
            "bool"
        ))

    return code_class(
                name=class_name,
                content = content,
                ctors=[
                    code_ctor(
                        class_name=class_name,
                        content = list(get_ctor_content(
                            child_names =[cast(str, i[NAME]) for i in cast(list[childtype], subclass[CHILDREN])],
                            has_data = has_data,
                            has_dvf = has_dvf,
                            check_data_type = check_data_type
                        )),
                        parameters=list(get_parameters(subclass)),
                        access_modifier=AccessModifiers.Public,
                        delegated_ctor="base",
                        delegated_ctor_arguments=list(get_delegate_ctor_arguments(subclass, enum_type))
                    )
                ],
                parents=[cast(str,subclass[PARENT])]
            )


def write_header(dst: Any):
    write_block(code_using_statements(USINGS), dst)
    write_block(f"namespace {NAMESPACE};", dst)

def generate_dynamicastnode_subclasses(config_path: str | Path, output_directory: str | Path):
    output_directory = Path(output_directory)

    with open(config_path) as config_file:
        config: Any = yaml.load(config_file, Loader=yaml.Loader)[HEADER_NAME]
        subclasses: classestype = config[CLASSES]
        base_class_name = config[BASE_CLASS_NAME]

    assert isinstance(subclasses, list)

    generic_base_type = f"DynamicASTNode<{config[ENUM_TYPE]}, {config[ANNOTATION_TYPE]}>"
    with open(str(output_directory/f"{base_class_name}.cs"), "w") as file:
        write_header(file)
        write_block(
            code_class(
                name = base_class_name, 
                content = [], 
                modifiers = [AccessModifiers.Public, "record"], 
                primary_ctor =[
                    "IToken? Data", 
                    f"List<{generic_base_type}> Children",
                    f"{config[ENUM_TYPE]} NodeType"
                ],
                parents = [f"{generic_base_type}(Data, Children, NodeType)"]
            ),
            file
        )
    for subclass in subclasses:
        with open(str(output_directory/f"{subclass[NAME]}.cs"), "w") as file:
            write_header(file)
            write_block(generate_dynamicastnode_subclass(subclass, config[ENUM_TYPE]), file)


if __name__ == "__main__":
    config_path = argv[1]
    output_dir = argv[2]
    generate_dynamicastnode_subclasses(config_path, output_dir)

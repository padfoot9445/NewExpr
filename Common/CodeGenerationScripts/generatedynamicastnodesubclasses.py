import yaml
from pathlib import Path
from typing import Literal, cast
from codegenframework import *
from sys import argv
import glob
import os

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
IS_MULTIPLE: Literal["is multiple"] = "is multiple"

CLASSES: Literal["classes"] = "classes"
ENUM_TYPE: Literal["enum type"] = "enum type"
ANNOTATION_TYPE: Literal["annotation type"] = "annotation type"
BASE_CLASS_NAME: Literal["base class name"] = "base class name"
BASE_CLASSES: Literal["base classes"] = "base classes"

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
        {BASE_CLASSES}:
            -
                {NAME}: [baseclassname]
                {PARENT}: [Parent of base class]
        {CLASSES}:
            -
                {NAME}: [class name]
                {PARENT}: [Parent Name]
                {CHILDREN}:
                    -
                        {NAME}: [child name]
                        {IS_OPTIONAL}: [yes | no]
                        {IS_MULTIPLE}: [yes | no]
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
        names: dict[str, int] = {}
        for child in children:
            raw_name = cast(str, child[NAME])
            type =  raw_name + "Node"
            name = get_name(raw_name, names)
    
            is_optional = cast(bool, child[IS_OPTIONAL])
            type = (type + ("?" if is_optional else ""))
            if child[IS_MULTIPLE]:
                type = f"IEnumerable<{type}>"
            yield code_property(
                name=name, 
                type=type, access_modifier=AccessModifiers.Public, setter_access_modifier=AccessModifiers.Private)

    def get_parameters(self: classtype):
        names: dict[str, int] = {}

        if self[HAS_DATA]:
            yield f"IToken {ADATA}"
        for child in cast(list[childtype], self[CHILDREN]):
            if child[IS_OPTIONAL]:
                type = f"{child[NAME]}Node?"
            else:
                type = f"{child[NAME]}Node"
            
            yield f"{type} {get_name(cast(str,child[NAME]), names)}"
    def get_name(name: str, names: dict[str, int]):
        if name not in names:
            names[name] = 0
            return name
        else:
            names[name] += 1
            return name + str(names[name])
    def get_delegate_ctor_arguments(self: classtype, enum_type: str):

        names: dict[str, int] = {}

        yield ("Data" if self[HAS_DATA] else "null") # data
        yield f"[{", ".join(get_name(cast(str, child[NAME]), names) for child in cast(list[childtype], self[CHILDREN]))}]" #children list
        yield f"{enum_type}.{self[NAME]}" #node type enum

    def get_ctor_content(child_names: list[str], has_data: bool, has_dvf: bool, check_data_type: bool):
        names: dict[str, int] = {}
        if has_data:
            yield f"Data = {ADATA};"
        
        if has_dvf:
            yield f"if(!{DVF_NAME}()) throw new Exception(\"Invalid data submitted\");"

        if check_data_type:
            yield f"if(!{DATACHECKER_NAME}()) throw new Exception(\"Invalid data type submitted\");"
        
        for child_name in child_names:
            new_name = get_name(child_name, names)
            yield f"this.{new_name} = {new_name};"

    children: list[childtype] = cast(list[childtype], subclass[CHILDREN])
    content = [i for i in get_children_properties(children)]
    has_data: bool = cast(bool, subclass[HAS_DATA])
    class_name = cast(str, subclass[NAME]) + "Node"
    check_data_type = cast(bool, subclass[CHECK_DATA_TYPE])
    has_dvf = cast(bool, subclass[HAS_ADDITIONAL_DVF])

    if has_data:
        content.append(
            code_property("Data", "IToken", access_modifier="public new", setter_access_modifier=AccessModifiers.Private)
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
                parents=[cast(str,subclass[PARENT])],
                modifiers=[AccessModifiers.Public, "record"]
            )

def base_class(base_class_name: str, config:Any, base_base_type: str):
    return code_class(
                name = base_class_name, 
                content = [], 
                modifiers = [AccessModifiers.Public, "record"], 
                primary_ctor =[
                    "IToken? Data", 
                    f"List<{base_base_type}> Children",
                    f"{config[ENUM_TYPE]} NodeType"
                ],
                parents = [f"{base_base_type}(Data, Children, NodeType)"]
            )

def write_header(dst: Any):
    write_block(code_using_statements(USINGS), dst)
    write_block(f"namespace {NAMESPACE};", dst)

def generate_dynamicastnode_subclasses(config_path: str | Path, output_directory: str | Path):
    output_directory = Path(output_directory)

    with open(config_path) as config_file:
        config: Any = yaml.load(config_file, Loader=yaml.Loader)[HEADER_NAME]
        subclasses: classestype = config[CLASSES]

    assert isinstance(subclasses, list)

    generic_base_type = f"DynamicASTNode<{config[ENUM_TYPE]}, {config[ANNOTATION_TYPE]}>"
    for _base_class in config[BASE_CLASSES]:
        with open(str(output_directory/f"{_base_class[NAME]}.cs"), "w") as file:
            write_header(file)
            write_block(
                base_class(
                    _base_class[NAME] + "Node",
                    config,
                    _base_class[PARENT] if _base_class[PARENT] is not False else generic_base_type
                ),
                file
            )
    for subclass in subclasses:
        with open(str(output_directory/f"{subclass[NAME]}Node.cs"), "w") as file:
            write_header(file)
            write_block(generate_dynamicastnode_subclass(subclass, config[ENUM_TYPE]), file)


if __name__ == "__main__":
    config_path = argv[1]
    output_dir = argv[2]
    
    files = glob.glob(str(Path(output_dir)/"*"))
    for file in files:
        os.remove(file)
    
    generate_dynamicastnode_subclasses(config_path, output_dir)


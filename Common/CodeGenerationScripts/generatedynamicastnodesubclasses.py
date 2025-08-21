from pathlib import Path
from typing import Literal, cast
from codegenframework import *
from initializer import *

HAS_DATA: Literal["has data"] = "has data"
VALID_DATA_TYPES: Literal["valid data types"] = "valid data types"
HAS_ADDITIONAL_DVF: Literal["has additional data validation function"] = "has additional data validation function"
DATA_VALIDATION_FUNCTION: Literal["data validation function"] = "data validation function"

IS_OPTIONAL: Literal["is optional"] = "is optional"
CHECK_DATA_TYPE: Literal["check data type"] = "check data type"
PARENT: Literal["parent"] = "parent"
IS_MULTIPLE: Literal["is multiple"] = "is multiple"

CLASSES: Literal["classes"] = "classes"
ENUM_TYPE: Literal["enum type"] = "enum type"
ANNOTATION_TYPE: Literal["annotation type"] = "annotation type"
BASE_CLASS_NAME: Literal["base class name"] = "base class name"
BASE_CLASSES: Literal["base classes"] = "base classes"

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

    [section_key]:
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
            if child[IS_MULTIPLE]:
                type = f"IEnumerable<{type}>"
            
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

        yield (ADATA if self[HAS_DATA] else "null") # data
        yield f"GetList([{", ".join(
            f"{".." if child[IS_MULTIPLE] else ""}{get_name(cast(str, child[NAME]), names)}"
            for child in cast(list[childtype], self[CHILDREN]))}])" #children list
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

    content.append(
        code_method(
            method_name="GetList",
            return_type="List<SmallLangNode>",
            parameters=[
                "IEnumerable<SmallLangNode?> Nodes"
            ],
            access_modifier="private static",
            content=[
                "return Nodes.Where(x => x is not null).Select(x => x!).ToList();"
            ]
        )
    )

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
                parents=[f"{subclass[PARENT]}Node"],
                modifiers=[AccessModifiers.Public, "record"]
            )

def base_class(base_class_name: str, config:Any, base_base_type: str):
    ChildrenType = f"List<SmallLangNode>"
    return code_class(
                name = base_class_name,
                content = [
                    code_property(
                        name = "Children",
                        type = ChildrenType,
                        access_modifier = "public new"
                        )
                ], 
                modifiers = [AccessModifiers.Public, "record"], 
                ctors=[
                    code_ctor(
                        class_name=base_class_name, 
                        content = [
                            "this.Children = Children;"
                        ],
                        access_modifier=AccessModifiers.Public,
                        parameters= [
                            "IToken? Data", 
                            f"{ChildrenType} Children",
                            f"{config[ENUM_TYPE]} NodeType"
                        ],
                        delegated_ctor="base",
                        delegated_ctor_arguments=[
                            "Data",
                            f"Children.Select(x => ({base_base_type})x).ToList()",
                            "NodeType"
                        ]
                    )
                ],
                parents = [f"{base_base_type}"]
            )


def generate_dynamicastnode_subclasses(output_directory: Path, raw_config: Any):
    

    subclasses: classestype = raw_config[CLASSES]

    assert isinstance(subclasses, list)

    generic_base_type = f"DynamicASTNode<{raw_config[ENUM_TYPE]}, {raw_config[ANNOTATION_TYPE]}>"
    for _base_class in raw_config[BASE_CLASSES]:
        with open(str(output_directory/f"{_base_class[NAME]}Node.cs"), "w") as file:
            write_header(raw_config, file)
            write_block(
                base_class(
                    _base_class[NAME] + "Node",
                    raw_config,
                    (_base_class[PARENT] + "Node" if _base_class[PARENT] is not False else generic_base_type)
                ),
                file
            )
    for subclass in subclasses:
        with open(str(output_directory/f"{subclass[NAME]}Node.cs"), "w") as file:
            write_header(raw_config, file)
            write_block(generate_dynamicastnode_subclass(subclass, raw_config[ENUM_TYPE]), file)


if __name__ == "__main__":
    
    _, output_dir, _, raw_config, _ = initialize()
    
    generate_dynamicastnode_subclasses(output_dir, raw_config)


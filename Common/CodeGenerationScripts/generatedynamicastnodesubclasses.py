import yaml
from pathlib import Path
from typing import Literal, cast
from codegenframework import *

HEADER_NAME: Literal["dynamicastnode subclasses"] = "dynamicastnode subclasses"
CHILDREN: Literal["children"] = "children"
HAS_DATA: Literal["has data"] = "has data"
VALID_DATA_TYPES: Literal["valid data types"] = "valid data types"
HAS_ADDITIONAL_DVF: Literal["has additional data validation function"] = "has additional data validation function"
DATA_VALIDATION_FUNCTION: Literal["data validation function"] = "data validation function"
NAME: Literal["name"] = "name"
IS_OPTIONAL: Literal["is optional"] = "is optional"
CHECK_DATA_TYPE: Literal["check data type"] = "check data type"

CLASSES: Literal["classes"] = "classes"
ENUM_TYPE: Literal["enum type"] = "enum type"
ANNOTATION_TYPE: Literal["annotation type"] = "annotation type"

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

def generate_dynamicastnode_subclass(subclass: classtype) -> str:

    def get_children_properties(children: list[childtype]):
        for child in children:
            name = cast(str, child[NAME])
            is_optional = cast(bool, child[IS_OPTIONAL])
            yield code_property(name=name, type=(name + ("?" if is_optional else "")), access_modifier=AccessModifiers.Public, setter_access_modifier=AccessModifiers.Private)

    def get_parameters(self: classtype):
        if self[HAS_DATA]:
            yield "IToken Data"
        for child in cast(list[childtype], self[CHILDREN]):
            if child[IS_OPTIONAL]:
                yield f"{child[NAME]}? {child[NAME]}"
            else:
                yield f"{child[NAME]} {child[NAME]}"

    def get_delegate_ctor_arguments(self: classtype):
        yield ("Data" if self[HAS_DATA] else "null") # data
        yield f"[{", ".join(cast(str, child[NAME]) for child in cast(list[childtype], self[CHILDREN]))}]" #children list
        yield f"{self[ENUM_TYPE]}.{self[NAME]}" #node type enum


    children: list[childtype] = cast(list[childtype], subclass[CHILDREN])
    content = [i for i in get_children_properties(children)]
    has_data: bool = cast(bool, subclass[HAS_DATA])
    class_name = cast(str, subclass[NAME])

    if has_data:
        content.append(
            code_property("Data", "IToken", access_modifier="public new", getter_access_modifier=AccessModifiers.Public, setter_access_modifier=AccessModifiers.Private)
        )

    content.append(
        f"public bool HasData => {"true" if has_data else "false"};"
    )

    return "\n".join(
        [
            "namespace SmallLang.IR.AST.Generated;",
            code_class(
                name=class_name,
                content = content,
                ctors=[
                    code_ctor(
                        class_name=class_name,
                        content=[],
                        parameters=list(get_parameters(subclass)),
                        access_modifier=AccessModifiers.Public,
                        delegated_ctor="base",
                        delegated_ctor_arguments=list(get_delegate_ctor_arguments(subclass))
                    )
                ],
                parents=[f"DynamicASTNode<{subclass[ENUM_TYPE]}, {subclass[ANNOTATION_TYPE]}>"]
            )
        ]
    )

def generate_dynamicastnode_subclasses(config_path: str, output_directory: str):
    with open(config_path) as config_file:
        subclasses: classestype = yaml.load(config_file, Loader=yaml.Loader)[HEADER_NAME]

    assert isinstance(subclasses, list)
    for subclass in subclasses:
        with open(Path(output_directory)/cast(str,(subclass[NAME])), "w") as file:
            print(generate_dynamicastnode_subclass(subclass), file=file)




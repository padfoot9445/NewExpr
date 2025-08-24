from initializer import *
from codegenframework import *
from attribute_interface_generator import get_attribute_property, get_interface_name_from_attribute

from collections import Counter, defaultdict
from collections.abc import Iterator, Callable
from typing import cast


names_used: defaultdict[str, defaultdict[str, int]] = defaultdict(lambda : defaultdict(int))
def number_name(x: str, subnode: Any) -> str:
    names = Counter(i[NAME] for i in subnode["children"])
    if names[x] == 1: return x
    else:
        names_used[subnode[NAME]][x] += 1
        return f"{x}{names_used[subnode[NAME]][x]}"


def get_children(children: list[dict[str, str | bool]], suffix: str) -> Iterator[tuple[Any,...]]: #type, name, is_optional, is_multiple
    for child in children:
        yield f"{child["type"]}{suffix}", child[NAME], child["is optional"], child["is multiple"]

if __name__ == "__main__":
    _, output_directory, _, raw_config, config = initialize()
    suffix = raw_config["node-type suffix"]

    #write subnodes
    for subnode in config:
        name = subnode[NAME] + suffix
        attributes = cast(list[dict[str, str]], subnode["attributes"])
        attribute_names = set(i["name"] for i in attributes)
        parents = subnode["parents"]


        for parent in parents:
            for attribute in raw_config["marker interface attributes"][parent]:
                if attribute["name"] in attribute_names:
                    continue
                else:
                    attributes.append(attribute)

        data = ("IToken", "Data") if subnode["has data"] else None
        children = [
            (f"{"List<" if is_multiple else ""}{type}{"?" if is_optional else ""}{">" if is_multiple else ""}", 
            f"{number_name(name, subnode)}")
            
            for type, name, is_optional, is_multiple in get_children(subnode["children"], suffix)
        ]

        ctor_children = children if data is None else [data] + children

        get_assignment: Callable[[str], str] = lambda numbered_child_name: f"this.{numbered_child_name} = {numbered_child_name};"

        class_content = [
            f"public {child_type} {child_name} {{ get; init; }}" for child_type, child_name in children
        ] + [
            code_method(
                method_name="DataChecker",
                content=[
                    f"if(Data.TT == TokenType.{valid_tt}) return true;"
                    for valid_tt in subnode["valid data types"]
                ] + [
                    "return false;" if subnode["has data"] and subnode["check data type"] else "return true;"
                ],
                return_type="bool",
                access_modifier=AccessModifiers.Private
            )
        ] + [
            "protected override IEnumerable<ISmallLangNode?> Children { get; set; }"
        ] + [
            get_attribute_property(attribute) for attribute in attributes
        ]

        
        if data is not None:
            class_content.append(
                f"public {data[0]} {data[1]} {{ get; init; }}"
            )

        with open(output_directory/f"{name}.cs", "w") as file:
            write_header(raw_config, file)
            write_block(
                code_class(
                    name=name,
                    content=class_content,
                    ctors=[
                        code_ctor(
                            name,
                            content=[
                                get_assignment(child_name)
                                for _, child_name in ctor_children
                            ] + [
                                f"if (!DataChecker(){"|| !DataValidationFunction()" if subnode["has additional data validation function"] else ""}) throw new Exception($\"{name}: Invalid data type submitted. {"Was {Data}. " if subnode["has data"] else ""}\");",
                                f"Children = [{", ".join(child_name if not child_type.startswith("List<") else f"..{child_name}" for child_type, child_name in children)}];"
                            ],
                            parameters= [
                                f"{child_type} {child_name}"
                                for child_type, child_name in ctor_children
                            ],
                            access_modifier=AccessModifiers.Public
                        )
                    ],
                    modifiers=[
                        "public",
                        "partial" if subnode["has additional data validation function"] else "",
                        "record"
                    ],
                    parents=[f"{i}{suffix}" for i in parents] + [get_interface_name_from_attribute(attribute) for attribute in attributes]
                )
                , file)

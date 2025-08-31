from initializer import *
from codegenframework import *
from attribute_group_generator import join_two_parent_groups
from attribute_interface_generator import get_interface_name

if __name__ == "__main__":
    _, output_directory, _, raw_config, config = initialize()
    suffix = raw_config["node-type suffix"]

    #write interfaces
    for interface in config:
        name = interface[NAME] + suffix
        parents = interface["parents"]
        attributes = interface["attributes"]
        with open(output_directory/f"{name}.cs", "w") as file:
            write_header(raw_config, file)
            write_block(
                code_block(
                    name=name,
                    keyword="interface",
                    content=[
                        "IToken Data { get; }" if interface["has data"] else ""
                    ],
                    affixes=[":", join_two_parent_groups((i + suffix for i in parents), (get_interface_name(i["name"]) for i in attributes))],
                    modifiers=[AccessModifiers.Public]
                ),
                file
            )
    
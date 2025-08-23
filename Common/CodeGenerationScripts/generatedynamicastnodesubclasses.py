from initializer import *
from codegenframework import *
from typing import Callable


if __name__ == "__main__":
    _, output_directory, _, raw_config, config = initialize()
    suffix = raw_config["node-type suffix"]




    
    get_interface_name: Callable[[str], str] = lambda x: f"IHasAttribute{x}"

    #write interfaces for attributes
    for attribute in config["attributes"]:
        attribute_name = attribute["name"]
        attribute_type = attribute["type"]
        interface_name = get_interface_name(attribute_name)

        with open(output_directory/f"{interface_name}.cs", "w") as file:
            write_header(raw_config, file)
            write_block(
                code_block(
                    name=interface_name,
                    keyword="interface",
                    content=[
                        f"public {attribute_type}? {attribute_name} {{ get; }}"
                    ],
                    prefix=["public"]
                ),
                file
            )
    
    #write interfaces for attribute groups
    for attribute_group in config["attribute groups"]:
        name = f"IHasAttributes_{"_".join(attribute for attribute in attribute_group)}"
        with open(output_directory/f"{name}.cs", "w") as file:
            write_header(raw_config, file)
            write_block(
                code_block(
                    name=name,
                    keyword="interface",
                    content=[],
                    affixes=[":", ", ".join(f"{get_interface_name(attribute)}" for attribute in attribute_group)]
                ),
                file
            )
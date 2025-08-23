from initializer import *
from codegenframework import *
from attribute_interface_generator import get_interface_name


if __name__ == "__main__":
    _, output_directory, _, raw_config, config = initialize()
    #write interfaces for attribute groups
    for attribute_group in config:
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
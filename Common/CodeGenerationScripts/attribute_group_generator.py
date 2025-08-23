from initializer import *
from codegenframework import *
from attribute_interface_generator import get_interface_name


if __name__ == "__main__":
    _, output_directory, _, raw_config, config = initialize()
    #write interfaces for attribute groups
    for attribute_group in config:
        name = f"IHasAttributes_{"_".join(attribute for attribute in attribute_group["attributes"] + attribute_group["marker interfaces"])}"
        joined_markers = ", ".join(attribute_group["marker interfaces"])
        joined_attributes = ", ".join(get_interface_name(i) for i in attribute_group["attributes"])
        both_joined = joined_markers + (", " if len(joined_markers) > 0 and len(joined_attributes) > 0 else "") + joined_attributes
        with open(output_directory/f"{name}.cs", "w") as file:
            write_header(raw_config, file)
            write_block(
                code_block(
                    name=name,
                    keyword="interface",
                    content=[],
                    affixes=[":", both_joined],
                    modifiers=[AccessModifiers.Public]
                ),
                file
            )
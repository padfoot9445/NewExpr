from initializer import *
from codegenframework import *
from collections.abc import Callable

get_interface_name: Callable[[str], str] = lambda x: f"IHasAttribute{x}"
get_interface_name_from_attribute: Callable[[dict[str, str]], str] = lambda x: get_interface_name(x["name"])
get_attribute_property: Callable[[dict[str, str]], str] = lambda attribute: f"public {attribute["type"]}? {attribute["name"]} {{ get; }}"
if __name__ == "__main__":
    _, output_directory, _, raw_config, config = initialize()

    #write interfaces for attributes
    for attribute in config:
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
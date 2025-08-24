from initializer import *
from codegenframework import *
from attribute_interface_generator import get_interface_name
from collections.abc import Callable, Iterable

join_two_parent_groups: Callable[[Iterable[str], Iterable[str]], str] = lambda j, k: (lambda x, y: ", ".join(x) + (", " if len(x) > 0 and len(y) > 0 else "") + ", ".join(y))(list(j), list(k))

if __name__ == "__main__":
    _, output_directory, _, raw_config, config = initialize()
    #write interfaces for attribute groups
    for attribute_group in config:
        name = f"IHasAttributes_{"_".join(attribute for attribute in attribute_group["attributes"] + attribute_group["marker interfaces"])}"
        both_joined = join_two_parent_groups(attribute_group["marker interfaces"], (get_interface_name(i) for i in attribute_group["attributes"]))
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
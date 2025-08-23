from initializer import *
from codegenframework import *

if __name__ == "__main__":
    _, output_directory, _, raw_config, config = initialize()
    suffix = raw_config["node-type suffix"]

    #write interfaces
    for interface in config:
        name = interface[NAME] + suffix
        parents = interface["parents"]
        with open(output_directory/f"{name}.cs", "w") as file:
            write_header(raw_config, file)
            write_block(
                code_block(
                    name=name,
                    keyword="interface",
                    content=[

                    ],
                    affixes=[":", ", ".join(i + suffix for i in parents)],
                    modifiers=[AccessModifiers.Public]
                ),
                file
            )
    
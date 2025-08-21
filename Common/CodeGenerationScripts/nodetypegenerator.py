from generatedynamicastnodesubclasses import write_header
from codegenframework import *
from typing import Callable
from initializer import *

if __name__ == "__main__":
    
    config_path, output_dir, _, raw_config, config = initialize()
    name = raw_config[NAME]
    with open(output_dir, "w") as file:
        get_string: Callable[[dict[str, str]], str] = lambda i: f"{list(i.keys())[0]}, //{list(i.values())[0]}"
        write_header(raw_config, file)
        write_block(
            code_block(
                name = name,
                keyword = "enum",
                content = [get_string(i) for i in config],
                modifiers = [AccessModifiers.Public],
            ),
            file
        )

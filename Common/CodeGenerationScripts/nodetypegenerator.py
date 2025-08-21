import yaml
import sys
from generatedynamicastnodesubclasses import write_header
from codegenframework import *
from chunktree import DATA
from typing import Callable

NAME: str = "name"

print("test1")
if __name__ == "__main__":
    print("test2")
    config_path = sys.argv[1]
    dst_path = sys.argv[2]
    section_key: str = sys.argv[3]
    with open(config_path) as file:
        config = yaml.load(file, yaml.Loader)[section_key]
        name = config[NAME]
        data: list[dict[str, str]] = config[DATA]
    with open(dst_path, "w") as file:
        get_string: Callable[[dict[str, str]], str] = lambda i: f"{list(i.keys())[0]}, //{list(i.values())[0]}"
        write_header(config, file)
        write_block(
            code_block(
                name = name,
                keyword = "enum",
                content = [get_string(i) for i in data],
                modifiers = [AccessModifiers.Public],
            ),
            file
        )

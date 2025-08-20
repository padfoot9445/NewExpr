from typing import Literal, cast, Any
import yaml
from sys import argv
from pathlib import Path
from codegenframework import *

SECTION_KEY: Literal["chunktree classes"] = "chunktree classes"
NAME: Literal["name"] = 'name'
CHILDREN: Literal["children"] = "children"

def header():
    yield "namespace SmallLang.IR.LinearIR.Generated;"
def generate_classes(config_path: str | Path, output_directory: str | Path):
    output_directory = Path(output_directory)

    with open(config_path) as config_file:
        config: Any = yaml.load(config_file, Loader=yaml.Loader)[SECTION_KEY]
    

    for ChunkType in config:
        assert isinstance(ChunkType, dict)
        
        with open(str(output_directory/cast(str,ChunkType[NAME])) + ".cs", "w") as file:
            
            children_names: list[str] = cast(list[str],ChunkType[CHILDREN])

            write_block("\n".join(header()),file)
            write_block(
                code_class(
                    name = cast(str, ChunkType[NAME]) + "TreeChunk",
                    content = [
                        f"protected override IEnumerable<BaseTreeChunk> Children => [{", ".join(children_names)}];",
                    ],
                    parents=[
                        "BaseTreeChunk(Self)"
                    ],
                    primary_ctor=[
                        "Chunk Self"
                    ] + [
                        f"BaseTreeChunk {name}"
                        for name in children_names
                    ],
                    modifiers = ["public", "record"]
                ),
                file
            )
if __name__ == "__main__":
    config_path = argv[1]
    output_dir = argv[2]
    generate_classes(config_path, output_dir)
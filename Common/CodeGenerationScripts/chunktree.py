from typing import cast, Any
from pathlib import Path
from codegenframework import *
from initializer import *


def generate_classes(config_path: str | Path, output_directory: str | Path, section_key: str, raw_config: Any, config: Any):
    output_directory = Path(output_directory)

    

    for ChunkType in config:
        assert isinstance(ChunkType, dict)
        
        with open(str(output_directory/cast(str,ChunkType[NAME])) + ".cs", "w") as file:
            
            children_names: list[str] = cast(list[str],ChunkType[CHILDREN])

            # write_block("\n".join(header()),file)
            write_header(raw_config, file)
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
    
    config_path, output_dir, section_key, raw_config, config = initialize()
    generate_classes(config_path, output_dir, section_key, raw_config, config)
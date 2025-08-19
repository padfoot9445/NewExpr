from typing import Any
from enum import Enum
TAB: str = "    "
class Keywords(Enum):
    Class = "class"
    Where = "where"
def code_block(name: str, keyword: str | Keywords, content: list[str], prefix: list[str] = [], affixes: list[str] = [], modifiers: list[str] = []) -> str:
    return f"{" ".join(modifiers)} {"".join(prefix)} {keyword if isinstance(keyword, str) else keyword.value} {name} {" ".join(affixes)}\n{{\n{TAB}{f"\n{TAB}".join(content)}\n}}"
def write_block(block: str, out: Any):
    print(block, file=out)
def code_class(name: str, content: list[str], modifiers: list[str] = [], primary: list[str] = [], parents: list[str] = [], constraints: list[str] = []) -> str:
    return code_block(name, keyword=Keywords.Class, content=content, modifiers=modifiers, affixes=
        [
            f"({", ".join(primary)})",
            ":",
            ", ".join(parents),
            "\n".join(constraints)
        ]
    )

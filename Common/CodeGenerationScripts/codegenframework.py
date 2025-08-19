from typing import Any, cast
from enum import Enum
TAB: str = "    "
class Keywords(Enum):
    Class = "class"
    Where = "where"
class AccessModifiers(Enum):
    Public = "public"
    Internal = "internal"
    Protected = "protected"
    Private = "private"
    PrivateProtected = Private + Protected
    Empty = ""
def get_value(possible_enum: str | Enum) -> str: return possible_enum if isinstance(possible_enum, str) else possible_enum.value
def optional_str(condition: Any | None, string: str | None) -> str: return cast(str, string) if condition is not None else ""
def code_block(name: str, keyword: str | Keywords, content: list[str], prefix: list[str] = [], affixes: list[str] = [], modifiers: list[str] = []) -> str:
    return f"{" ".join(modifiers)} {"".join(prefix)} {keyword if isinstance(keyword, str) else keyword.value} {name} {" ".join(affixes)}\n{{\n{TAB}{f"\n{TAB}".join(content)}\n}}"
def write_block(block: str, out: Any):
    print(block, file=out)
def code_class(name: str, content: list[str], modifiers: list[str] = [], primary_ctor: list[str] = [], parents: list[str] = [], constraints: list[str] = []) -> str:
    return code_block(name, keyword=Keywords.Class, content=content, modifiers=modifiers, affixes=
        [
            f"({", ".join(primary_ctor)})",
            ":",
            ", ".join(parents),
            "\n".join(constraints)
        ]
    )
def code_property(name: str, type: str, access_modifier: str | AccessModifiers = AccessModifiers.Private, getter_access_modifier: str | AccessModifiers = AccessModifiers.Empty, setter_access_modifier: str | AccessModifiers = AccessModifiers.Empty, getter_body: str | None = None, setter_body: str | None = None, initializing_expression: str | None = None) -> str:
    return " ".join([
        get_value(access_modifier),
        type,
        name,
        "{",
        get_value(getter_access_modifier),
        "get",
        optional_str(getter_body, f"=> {getter_body}"),
        ";",
        get_value(setter_access_modifier),
        "set",
        optional_str(setter_body, f"=> {setter_body}"),
        "}",
        optional_str(initializing_expression, "="),
        optional_str(initializing_expression, initializing_expression),
        ";"
    ])

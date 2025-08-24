import os
from pathlib import Path
from typing import Callable

def generate_emitting_functions(amount_of_generics: int = 5) -> None:
    module_path = Path(os.path.dirname(__file__))

    emitting_functions_file_path = Path(module_path / "generated.CodeGenerator__emittingfunctions.cs")

    with open(emitting_functions_file_path, "w") as emitting_functions_file:
        output = (
            "using System.Numerics;\n"
            "using SmallLang.IR.LinearIR;\n"
            "using Common.LinearIR;\n"
            "namespace SmallLang.CodeGen.Frontend;\n"
            "\n"
            "public partial class CodeGenerator\n"
            "{\n"
        )

        get_generic_variable_name: Callable[[str], str] = lambda i: ("N" + i[1:])

        for current_generic_index in range(1, amount_of_generics + 1):
            generics: list[str] = [f"T{i}" for i in range(current_generic_index)]
            parameters: str = ", ".join([
                "Opcode op",
                *[
                    f"NumberWrapper<{i}, BackingNumberType> {get_generic_variable_name(i)}"
                    for i
                    in generics
                ]
            ])
            arguments: str = ", ".join([
                "op",
                *[
                    get_generic_variable_name(i)
                    for i
                    in generics
                ]
            ])

            output += (
                f"    internal void Emit<{', '.join(generics)}> ({parameters})\n"
                f"    {'\n    '.join(
                    f"where {generic} : IBinaryInteger<{generic}>, IMinMaxValue<{generic}>"
                    for generic
                    in generics
                )}\n"
                f"        => Emit({arguments});\n"
            )

        output += "}"

        # print(f"Generated code for {emitting_functions_file_path}:\n----")
        # print(output)
        # print("----")

        emitting_functions_file.write(output)

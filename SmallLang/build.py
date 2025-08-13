import subprocess
from typing import Callable
from GlobalUsings import add_global_usings_to_cs_projects
from Codegen.Frontend.emittingfunctions import generate_emitting_functions

if __name__ == "__main__":
    code_generators: list[Callable[[], None]] = [
        add_global_usings_to_cs_projects,
        generate_emitting_functions
    ]

    for code_generator in code_generators:
        code_generator()

    subprocess.run(["dotnet", "build"], shell=True)

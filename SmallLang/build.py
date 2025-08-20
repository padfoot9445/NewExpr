import subprocess
from typing import Callable
from GlobalUsings import add_global_usings_to_cs_projects
from Codegen.Frontend.emittingfunctions import generate_emitting_functions
from pathlib import Path
import os
import sys

smalllang = Path(os.path.dirname(__file__))

if __name__ == "__main__":
    code_generators: list[Callable[[], None]] = [
        add_global_usings_to_cs_projects,
        generate_emitting_functions
    ]

    commands: list[list[str]] = [
        [sys.executable, str(smalllang/"../Common/CodeGenerationScripts/generatedynamicastnodesubclasses.py"), str(smalllang/"config.yaml"), str(smalllang/"IR"/"AST"/"Generated")],
        ["dotnet", "build"]
    ]

    for code_generator in code_generators:
        code_generator()

    for command in commands:
        subprocess.run(command, check=True)

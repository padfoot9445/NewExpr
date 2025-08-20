import subprocess
from typing import Callable
from GlobalUsings import add_global_usings_to_cs_projects
from Codegen.Frontend.emittingfunctions import generate_emitting_functions
from pathlib import Path
import os
import sys

smalllang = Path(os.path.dirname(__file__))
common_codegenscripts = smalllang/"../Common/CodeGenerationScripts"
yaml_path = smalllang/"config.yaml"
if __name__ == "__main__":
    code_generators: list[Callable[[], None]] = [
        add_global_usings_to_cs_projects,
        generate_emitting_functions
    ]

    commands: list[list[str | Path]] = [
        [sys.executable, common_codegenscripts/"generatedynamicastnodesubclasses.py", yaml_path, smalllang/"IR"/"AST"/"Generated"],
        [sys.executable, common_codegenscripts/"chunktree.py", yaml_path, smalllang/"IR"/"LinearIR"/"Generated"],
    ]
    build: list[list[str | Path]] = [
        ["dotnet", "restore"],
        ["dotnet", "format", "--no-restore",],
        ["dotnet", "build", "--no-restore"]
    ]
    if not "--no-build" in sys.argv:
        commands += build
    for code_generator in code_generators:
        code_generator()

    for command in commands:
        command = [str(i) for i in command]
        subprocess.run(command, check=True)

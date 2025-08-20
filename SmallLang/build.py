import subprocess
from typing import Callable
from GlobalUsings import add_global_usings_to_cs_projects
from Codegen.Frontend.emittingfunctions import generate_emitting_functions
from pathlib import Path
import os
import sys

smalllang = Path(os.path.dirname(__file__))
common_codegenscripts = smalllang/"../Common/CodeGenerationScripts"
yaml_path = smalllang/"NodeTypeGeneratorConfig.yaml"
if __name__ == "__main__":
    code_generators: list[Callable[[], None]] = [
        add_global_usings_to_cs_projects,
        generate_emitting_functions
    ]

    
    fmt: tuple[list[str | Path], str] = (["dotnet", "format", "--no-restore"], "Formatting succeeded")
    commands: list[tuple[list[str | Path], str]] = [
        ([sys.executable, common_codegenscripts/"generatedynamicastnodesubclasses.py", yaml_path, smalllang/"IR"/"AST"/"Generated"], "Dynamic astnode subclasses generation succeeded"),
        ([sys.executable, common_codegenscripts/"chunktree.py", yaml_path, smalllang/"IR"/"LinearIR"/"Generated"], "Chunktrees generation succeeded"),
        ([sys.executable, common_codegenscripts/"nodetypegenerator.py", yaml_path, smalllang/"IR/AST/Generated/ImportantASTNodeType.cs"], "ImportantASTNodeType generation succeeded"),

        (["dotnet", "restore"], "Restored")
    ]
    build: list[tuple[list[str | Path], str]] = [
        (["dotnet", "build", "--no-restore"], "Build succeeded")
    ]
    from time import time
    for code_generator in code_generators:
        code_generator()

    if not "--no-build" in sys.argv:
        commands += build
    if "--whitespace" in sys.argv:
        fmt[0].append("whitespace")
    if not "--no-format" in sys.argv:
        commands.append(fmt)
    
    with open("log.tmp", "w") as file: file.write("")
    for command, msg in commands:
        t = time()
        command = [str(i) for i in command]
        with open("log.tmp", "a") as file:
            subprocess.run(command, check=True, stdout=file)
        print(f"BUILD: {msg} in {round(time() - t, 2)}s")
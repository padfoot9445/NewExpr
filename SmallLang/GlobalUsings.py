import os
import sys
from pathlib import Path


def add_global_usings_to_cs_projects() -> None:
    module_path = Path(os.path.dirname(__file__))

    for directory in os.listdir(module_path):
        directory = Path(module_path / directory)

        if os.path.isfile(directory):
            continue

        cs_project_paths: list[str] = [
            path
            for path
            in os.listdir(directory)
            if path.endswith(".csproj")
        ]


        if len(cs_project_paths) > 1:
            print(f"Too many .csproj files in {directory} ({len(cs_project_paths)})")
            sys.exit(1)
        elif len(cs_project_paths) == 0:
            print(f"No .csproj files in {directory}. Continuing.")
            continue
        print(f"Processing {directory}.")
        cs_project_path: str = cs_project_paths[0]

        with open(Path(directory / "GlobalUsings.cs"), "w") as global_usings_file:
            with open(Path(directory / cs_project_path), "r") as cs_project_file:
                reference_ir_in_cs_project_file: bool = "IR.csproj" in cs_project_file.read()

                global_usings_file.write(
                    f"global using BackingNumberType = byte;\n"
                    f"global using OpcodeBackingType = uint;\n"
                    "global using Node = Common.AST.DynamicASTNode<SmallLang.IR.AST.ImportantASTNodeType, SmallLang.IR.Metadata.Attributes>;\n" if reference_ir_in_cs_project_file else ""
                )

import os
from pathlib import Path
from initializer import *

def add_global_usings_to_cs_projects(aliases: dict[str, str]) -> None:
    module_path = Path(os.getcwd())

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
            raise Exception(f"Too many .csproj files in {directory} ({len(cs_project_paths)})")
        elif len(cs_project_paths) == 0:
            # print(f"No .csproj files in {directory}. Continuing.")
            continue
        # print(f"Processing {directory}.")

        with open(Path(directory / "generated.GlobalUsings.cs"), "w") as global_usings_file:
            for k, v in aliases.items():
                print(f"global using {k} = {v};", file=global_usings_file)
            print("wrote", directory / "generated.GlobalUsings.cs")

if __name__ == "__main__":
    _, _, _, _, config = initialize()
    add_global_usings_to_cs_projects(config["aliases"])


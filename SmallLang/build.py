import subprocess
from typing import Callable, Any
from GlobalUsings import add_global_usings_to_cs_projects
from Codegen.Frontend.emittingfunctions import generate_emitting_functions
from pathlib import Path
import os
import sys
import yaml
from time import time


Command = tuple[list[str | Path], str]
working_directory = Path(os.path.dirname(__file__))


custom_code_generators: list[Callable[[], None]] = [
    add_global_usings_to_cs_projects,
    generate_emitting_functions
]

if __name__ == "__main__":
    #get build steps:
    build_steps: list[Command] = []

    with open(working_directory/"config.yaml") as file_path:
        config = yaml.load(file_path, yaml.Loader)
        file_paths = config["files"]
        code_generation_scripts_directory = working_directory/Path(config["generators relative path"])
    
    for file_path in file_paths:
        with open(working_directory/file_path) as file:
            current_file_dict: dict[str, Any] = yaml.load(file, yaml.Loader)
            for step_name in current_file_dict.keys():
                generator = current_file_dict[step_name]["generator"]
                dst = current_file_dict[step_name]["dst"]
                display_name = current_file_dict[step_name]["display name"]
                build_steps.append((
                    [sys.executable, code_generation_scripts_directory/generator, working_directory/file_path, working_directory/dst],
                    display_name
                ))


    fmt_command: Command = (["dotnet", "format", "--no-restore"], "Dotnet-Format")
    restore_command: Command = (["dotnet", "restore"], "Dotnet-Restore")
    build_command: Command = (["dotnet", "build", "--no-restore"], "Dotnet-Build")

    #add unconditional additional build steps
    build_steps.append(restore_command)


    #handle commmand line arguments
    flags: list[tuple[str, Callable[[], None], bool]] = [ #flag to look out for, action to take if flag, [bool]: desired existence (so if True then we take the step if the flag exists, and if false we take the step if the flag does not exist)
        ("--no-build", lambda: build_steps.append(build_command), False),
        ("--whitespace", lambda: fmt_command[0].append("whitespace"), True),
        ("--no-format", lambda: build_steps.append(fmt_command), False)
    ]

    for flag, action, desired in flags:
        if (flag in sys.argv) == desired:
            action()


    #make build_steps string only
    for i in build_steps:
        for j in range(len(i[0])):
            i[0][j] = str(i[0][j])

    #run local generators
    for code_generator in custom_code_generators:
        code_generator()


    #set up log-file
    log_file_path = working_directory/"log.tmp"
    with open(log_file_path, "w") as file_path:
        file_path.write("")
    

    total_time = time()
    #run build steps
    for command, msg in build_steps:
        t = time()
        command = [str(i) for i in command]
        with open(log_file_path, "a") as file_path:
            subprocess.run(command, check=True, stdout=file_path)
        print(f"BUILD: {msg} succeeded in {round(time() - t, 2)}s")
    print(f"BUILD: Build Steps executed in {round(time() - total_time,2)}s")
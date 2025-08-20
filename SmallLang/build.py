import subprocess
from typing import Callable, Any
from GlobalUsings import add_global_usings_to_cs_projects
from Codegen.Frontend.emittingfunctions import generate_emitting_functions
from pathlib import Path
import os
import sys
import yaml
from time import time, sleep
import glob
from threading import Thread

Command = tuple[list[str | Path], str]
working_directory = Path(os.path.dirname(__file__))

TIME_ROUND: int = 2
BOLD = "\033[1m"
END = '\033[0m'
SUCCEED = f"\033[92m\033[1msucceeded{END} in"
FAIL = f"\033[31m{BOLD}failed{END} in"
BUILD = f"\033[90m\033[1mBUILD{END}"

custom_code_generators: list[Callable[[], None]] = [
    add_global_usings_to_cs_projects,
    generate_emitting_functions
]

def custom_run(command: Any, path:Any, out: list[bool]):
    try:
        subprocess.run(command, check=True, stdout=path)
    except subprocess.CalledProcessError:
        out[0] = False
    else:
        out[0] = True

def time_command(command: list[str], path: Any, name: str):
    
    splitname = name.split(" ")
    name = f"\033[1m{splitname[0]}{END}{(" " + " ".join(splitname[1:])) if len(splitname) > 1 else ""}"
    
    out = [True]

    work_thread = Thread(target=custom_run, args=[command, path, out])
    work_thread.start()

    start_time = time()
    time_str = f"(0.0s)"
    sys.stdout.write(f"{BUILD}: {name} {" " * len(SUCCEED)} {time_str}"); sys.stdout.flush()

    while True:
        sleep(0.01)
        sys.stdout.write("\b" * len(time_str))
        time_str = f"({round(time() - start_time , TIME_ROUND)}s)"
        sys.stdout.write(time_str); sys.stdout.flush()
        if not work_thread.is_alive(): break


    sys.stdout.write("\b" * (len(SUCCEED) + len(time_str) + 1)) #+1 to account for the space between succeed and time_str
    time_str = f"({round(time() - start_time, TIME_ROUND)}s)"
    sys.stdout.write(f"{SUCCEED if out[0] else FAIL} {time_str} {" " * 20}"); print() #flush and newline
    assert not work_thread.is_alive()
    return out[0]


    


if __name__ == "__main__":
    #get generator-dst-directories to remove
    dst_directories: list[Path] = []

    #get build steps:
    build_steps: list[Command] = []

    with open(working_directory/"config.yaml") as file_path:
        config = yaml.load(file_path, yaml.Loader)
        configurations_path = working_directory/config["configuration directory"]
        file_paths = config["files"]
        code_generation_scripts_directory = working_directory/Path(config["generators relative path"])
    
    for file_path in file_paths:
        with open(configurations_path/file_path) as file:
            current_file_dict: dict[str, Any] = yaml.load(file, yaml.Loader)
            for step_name in current_file_dict.keys():
                generator = current_file_dict[step_name]["generator"]
                dst = current_file_dict[step_name]["dst"]
                display_name = current_file_dict[step_name]["display name"]
                build_steps.append((
                    [sys.executable, code_generation_scripts_directory/generator, configurations_path/file_path, working_directory/dst],
                    display_name
                ))

                #append dst to dst_directories to facilitate removal
                
                if Path.is_dir(working_directory/dst):
                    dst_directories.append(working_directory/dst)


    fmt_command: Command = (["dotnet", "format", "--no-restore"], "Dotnet-Format")
    restore_command: Command = (["dotnet", "restore"], "Dotnet-Restore")
    build_command: Command = (["dotnet", "build", "--no-restore"], "Dotnet-Build")


    dotnet_build_steps: list[Command] = []

    #add unconditional additional build steps
    build_steps.append(restore_command)


    #handle commmand line arguments
    flags: list[tuple[str, Callable[[], None], bool]] = [ #flag to look out for, action to take if flag, [bool]: desired existence (so if True then we take the step if the flag exists, and if false we take the step if the flag does not exist)
        ("--no-build", lambda: dotnet_build_steps.append(build_command), False),
        ("--whitespace", lambda: fmt_command[0].append("whitespace"), True),
        ("--no-format", lambda: dotnet_build_steps.append(fmt_command), False)
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

    #delete old files
    del_time = time()
    for dst_dir in dst_directories:
        for file in glob.glob(str(dst_dir/"*")):
            os.remove(file)

    #run build steps
    success = True

    #run non-dotnet steps
    for command, msg in build_steps:
        command = [str(i) for i in command]
        with open(log_file_path, "a") as file_path:
            success = time_command(command, file_path, msg) and success

    for command, msg in dotnet_build_steps:
        command = [str(i) for i in command]
        with open(log_file_path, "a") as file_path:
            success = time_command(command, file_path, msg) and success
            if not success: break

    build_code = "\033[092m\033[1m" if success else f"\033[31m{BOLD}"
    print(f"{build_code}BUILD: Build {"succeeded" if success else "failed"} in {round(time() - total_time, TIME_ROUND)}s{END}")
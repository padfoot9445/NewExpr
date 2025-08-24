import subprocess
from typing import Callable, Any, cast
from pathlib import Path
import os
import sys
import yaml
from time import time, sleep
from threading import Thread
import shutil

Command = tuple[list[str | Path], str, bool] #(Command, Name, Execute?)

TIME_ROUND: int = 2
BOLD = "\033[1m"
END = '\033[0m'
GREEN = '\033[92m'
RED = '\033[31m'
SUCCEED = f"{GREEN}{BOLD}succeeded{END} in"
FAIL = f"{RED}{BOLD}failed{END} in"
HEADERCODE = "\033[90m\033[1m"
BUILD = f"{HEADERCODE}BUILD{END}"
YELLOW = '\033[93m'

def mutate_command(src: list[Command], srcindex: int, val: list[str | Path] | str | bool = False, mutindex: int=2):
    src[srcindex] = cast(Command, tuple(v if mutindex != i else val for i, v in enumerate(src[srcindex])))

def custom_run(command: Any, path:Any, out: list[bool]):
    try:
        subprocess.run(command, check=True, stdout=path)
    except subprocess.CalledProcessError:
        out[0] = False
    else:
        out[0] = True

def time_thread(name: str, work_function: Callable[..., Any], *args: Any, **kwargs: Any):
    splitname = name.split(" ")
    name = f"\033[1m{splitname[0]}{END}{(" " + " ".join(splitname[1:])) if len(splitname) > 1 else ""}"
    
    out = [True]

    work_thread = Thread(target=work_function, args=[*list(args), out], kwargs=kwargs)
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

def time_command(command: list[str], stdout_destination: Any, msg: str):
    return time_thread(msg,
        custom_run, command, stdout_destination)

def make_dir(dst: Path):
    if os.path.isdir(dst): return
    elif os.path.split(dst)[-1] in ["NUL", "NUL:"]: return

    if dst.is_file() or len(os.path.splitext(dst)[1]) > 0:
        dst = dst.parent.resolve()
    
    if os.path.isdir(dst): return
    
    os.mkdir(dst)
    print(f"{YELLOW}{BOLD}INFO{END}:  {YELLOW}{BOLD}\033[4:5m{dst}{END}")
    

def delete_files(clean: bool, out: list[bool]):
    try:
        for dirpath, _, filenames in os.walk(working_directory):
            for i in filenames:
                if "generated" in [j.lower() for j in dirpath.split(os.sep)] or i.split(".")[0].lower() == "generated":
                    os.remove(Path(dirpath)/i)
            if clean and os.path.split(dirpath)[-1].lower() == "generated":
                shutil.rmtree(Path(dirpath).resolve())
    except Exception as e:
        out[0] = False
        print(f"{RED}{BOLD}INFO{END}:  {RED}{BOLD}{e}{END}")
            

if __name__ == "__main__":

    working_directory = Path(sys.argv[1] if len(sys.argv) >= 2 and not sys.argv[1].startswith("-") else os.getcwd()).resolve()



    with open(working_directory/"config.yaml") as file_path:
        config = yaml.load(file_path, yaml.Loader)
        configurations_path = working_directory/config["configuration directory"]
        file_paths = config["files"]
        code_generation_scripts_directory = working_directory/Path(config["generators relative path"])
    

    fmt_command: Command = (["dotnet", "format", "--no-restore"], "Dotnet-Format", True)
    restore_command: Command = (["dotnet", "restore"], "Dotnet-Restore", True)
    build_command: Command = (["dotnet", "build", "--no-restore"], "Dotnet-Build", True)


    dotnet_build_steps: list[Command] = [
        restore_command,
        build_command,
        fmt_command
    ]

    do_clean_flag = False
    def do_clean():
        global do_clean_flag
        do_clean_flag = True
    
    #handle commmand line arguments
    flags: list[tuple[str, Callable[[], Any], bool]] = [ #flag to look out for, action to take if flag, [bool]: desired existence (so if True then we take the step if the flag exists, and if false we take the step if the flag does not exist)
        ("--no-build", lambda: mutate_command(dotnet_build_steps, 1), True),
        ("--whitespace", lambda: fmt_command[0].append("whitespace"), True),
        ("--no-format", lambda: mutate_command(dotnet_build_steps, 2), True),
        ("--no-dotnet", lambda: [mutate_command(dotnet_build_steps, i) for i in range(3)], True),
        ("--clean", do_clean, True)
    ]

    for flag, action, desired in flags:
        if (flag in sys.argv) == desired:
            action()


    #delete old files
    delete_out = [True]
    time_thread("Delete-Generated-Files", delete_files, do_clean_flag)


    #get build steps:
    build_steps: list[Command] = []
    for file_path in file_paths:
        file_path = configurations_path/file_path
        with open(file_path) as file:
            current_file_dict: dict[str, Any] = yaml.load(file, yaml.Loader)
            for step_name in current_file_dict.keys():
                generator = code_generation_scripts_directory/current_file_dict[step_name]["generator"]
                dst = working_directory/current_file_dict[step_name]["dst"]
                display_name = current_file_dict[step_name]["display name"]
                build_steps.append((
                    [sys.executable, generator, file_path, dst, step_name, working_directory],
                    display_name,
                    (not current_file_dict[step_name].get("ignore", False))
                ))

                #make dst if necessary
                make_dir(dst)
                with open("tmp.tmp", "w") as f:
                    print(f"making dir {dst}", file=f)



    #make build_steps string only
    for i in build_steps:
        for j in range(len(i[0])):
            i[0][j] = str(i[0][j])


    #set up log-file
    log_file_path = working_directory/"log.tmp"
    with open(log_file_path, "w") as file_path:
        file_path.write("")
    

    total_time = time()

    total_steps = len(dotnet_build_steps) + len(build_steps) + 1
    steps_taken = total_steps
    ignored = 0

    skip: Callable[[str], None] = lambda name: print(f"{HEADERCODE}INFO{END}:  {BOLD}{name} {YELLOW}ignored{END} in (0.0s)")

    
    #run build steps
    success = True
    
    #run non-dotnet steps
    for command, msg, execute in build_steps:
        if not execute:
            skip(msg); ignored += 1; continue
        command = [str(i) for i in command]
        with open(log_file_path, "a") as file_path:
            success = time_command(command, file_path, msg) and success
            if not success:
                steps_taken -= 1

    for command, msg, execute in dotnet_build_steps:
        if not execute:
            skip(msg); ignored += 1; continue

        if not success:
            steps_taken -= 1
            break
        command = [str(i) for i in command]
        with open(log_file_path, "a") as file_path:
            success = time_command(command, file_path, msg) and success
            if not success:
                time_command(command, sys.stdout, msg)


    build_code = "\033[092m\033[1m" if success else f"\033[31m{BOLD}"
    color = (GREEN if success else RED) + BOLD
    if success:
        print(f"{BOLD}{color}INFO{END}:  {color}{steps_taken - ignored}/{total_steps - ignored} build steps {BOLD}{f"succeeded" if success else f"failed"}{END}{color} in {round(time() - total_time, TIME_ROUND)}s{END}")
    else:
        print(f"{BOLD}{color}INFO{END}:  {color}{total_steps - steps_taken}/{total_steps - ignored} build steps {BOLD}{f"succeeded" if success else f"failed"}{END}{color} in {round(time() - total_time, TIME_ROUND)}s{END}")
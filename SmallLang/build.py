import subprocess
CodeGeneratorPaths: list[str] = \
[
    r"C:\Users\User\coding\nostars\Expa\NewExpr\SmallLang\GlobalUsings.py",
    r"C:\Users\User\coding\nostars\Expa\NewExpr\SmallLang\Codegen\Frontend\emittingfunctions.py"
]
def run_process(cmd: list[str]) -> None:
    subprocess.run(cmd, shell=True)
for i in CodeGeneratorPaths:
    run_process(["python3", i])
run_process(["dotnet", "build"])

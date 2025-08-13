import os
CWD = r"C:\Users\User\coding\nostars\Expa\NewExpr\SmallLang"
os.chdir(CWD)

for directory in os.listdir(os.getcwd()):
    if os.path.isfile(directory): continue
    csprojs: list[str] = [i for i in os.listdir(os.path.join(os.getcwd(),directory)) if os.path.splitext(i)[-1].lower() == ".csproj"]
    assert len(csprojs) <= 1
    if len(csprojs) == 0:
        continue
    with open(f"./{directory}/GlobalUsings.cs", "w") as file:
        csproj: str = csprojs[0]
        with open(f"{directory}/{csproj}", "r") as csfile:
            reference_ir:bool = "IR.csproj\"" in csfile.read()
        file.write(
            """
global using BackingNumberType = byte;
global using OpcodeBackingType = uint;
""" +
("global using Node = Common.AST.DynamicASTNode<SmallLang.IR.AST.ImportantASTNodeType, SmallLang.IR.Metadata.Attributes>;" if reference_ir else ""
)        )
Generics = list[str]
Generic = str
Code = str
PATH: str = r"C:\Users\User\coding\nostars\Expa\NewExpr\SmallLang\Codegen\Frontend\CodeGenerator__emittingfunctions.cs"
TAB = "    "
AMOUNT: int = 5
BackingNumberType = "BackingNumberType"
def get_generics(n: int) -> Generics:
    return [f"T{i}" for i in range(n)]
def get_constraint(generic: Generic) -> Code:
    return f"where {generic} : IBinaryInteger<{generic}>, IMinMaxValue<{generic}>"
def get_variable_name(i: Generic) -> Code:
    return "N" + i[1:]
def get_param(i: Generic) -> Code:
    return f"NumberWrapper<{i}, {BackingNumberType}> {get_variable_name(i)}"
def get_params(generics: Generics):
    return ", ".join(
        ["Opcode op"] +
        [
            get_param(i) for i in generics
        ]
    )
def get_arguments(generics: Generics) -> Code:
    return ", ".join(
        ["op"] +
        [
            get_variable_name(i) for i in generics
        ]
    )

with open(PATH, "w") as file:
    print(
        """
using System.Numerics;
using SmallLang.LinearIR;
using Common.LinearIR;
namespace SmallLang.CodeGen.Frontend;

public partial class CodeGenerator
{
"""
    , file=file) #header
    for j in range(1, AMOUNT + 1):
        generics = get_generics(j)
        print(f"{TAB}void Emit<{", ".join(generics)}>({get_params(generics)})", file=file)
        print(TAB + f"\n{TAB}".join(get_constraint(i) for i in generics), file=file)
        print(TAB * 2 + f"=> Emit({get_arguments(generics)});",file=file)
    print("}",file=file)

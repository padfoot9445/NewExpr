using System.Diagnostics;
using System.Text.Json;
using Common.Metadata;

namespace SmallLang.IR.Metadata;

partial class Functions
{
    public Functions()
    {
    }

    public void RegisterFunction(FunctionSignature<BackingNumberType, GenericSmallLangType> Signature)
    {
        if (RegisteredFunctions.Any(x => x.Name == Signature.Name))
        {
            if (!(RegisteredFunctions.Where(x => x.Name == Signature.Name).Select(x => x == Signature).All(x => x)))
            {
                throw new ArgumentOutOfRangeException(
                    string.Join('\n',
                        RegisteredFunctions.Where(x => x.Name == Signature.Name && (x != Signature))
                            .Select(x => x.ToString())) + $"Had the same name as {Signature} but were not the same.");
            }

            return;
        }

        RegisteredFunctions.Add(Signature);
    }

    public HashSet<FunctionSignature<BackingNumberType, GenericSmallLangType>> RegisteredFunctions { get; } = new();

    public FunctionSignature<BackingNumberType, GenericSmallLangType> GetSignature(string Name)
    {
        return RegisteredFunctions.Single(x => x.Name == Name);
    }

    public FunctionSignature<BackingNumberType, GenericSmallLangType> GetSignature(FunctionID<BackingNumberType> ID)
    {
        return RegisteredFunctions.Single(x => x.ID == ID);
    }
}
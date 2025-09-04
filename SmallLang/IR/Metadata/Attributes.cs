using Common.AST;
using Common.Metadata;

namespace SmallLang.IR.Metadata;

public record class Attributes(
    List<GenericSmallLangType>? DeclArgumentTypes = null,
    FunctionID<BackingNumberType>? FunctionID = null,
    GenericSmallLangType? TypeOfExpression = null,
    GenericSmallLangType? TypeLiteralType = null,
    VariableName? VariableName = null,
    VariableModifiers? VarMods = null,
    uint? SizeOfVariable = null,
    Scope? VariablesInScope = null,
    LoopGUID? LoopGUID = null,
    LoopGUID? GUIDOfLoopLabel = null,
    GenericSmallLangType? GreatestCommonType = null,
    GenericSmallLangType? ExpectedReturnType = null
) : IMetadata
{
    public Attributes() : this(DeclArgumentTypes: null)
    {
    }

    public void ForcedMerge(IMetadata other, bool PrioritizeOther = false)
    {
        throw new NotImplementedException();
    }

    public bool IsEquivalentTo(IMetadata other)
    {
        throw new NotImplementedException();
    }

    public void Merge(IMetadata other)
    {
        throw new NotImplementedException();
    }
}
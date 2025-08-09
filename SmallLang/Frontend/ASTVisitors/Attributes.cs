using Common.AST;
using Common.Metadata;
using SmallLang.Metadata;

namespace SmallLang.Frontend.ASTVisitors;

public record class Attributes(List<SmallLangType>? DeclArgumentTypes = null, FunctionID<BackingNumberType>? FunctionID = null, SmallLangType? TypeOfExpression = null, SmallLangType? TypeLiteralType = null, VariableName? VariableName = null, VariableModifiers? VarMods = null, uint? SizeOfVariable = null, Scope? VariablesInScope = null, LoopGUID? LoopGUID = null) : IMetadata
{
    public Attributes() : this(DeclArgumentTypes: null) { }
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
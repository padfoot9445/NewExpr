using Common.LinearIR;
using SmallLang.LinearIR;

namespace SmallLang.Metadata;

public record class FunctionID(uint BackingValue) : GenericNumberWrapper<uint>(BackingValue)
{

}
using SmallLang.Backend;

namespace SmallLangTest.BackendComponentTests;
class FunctionCallDriverMock : CodeGenVisitor
{
    public FunctionCallDriverMock(bool RetToRegister)
    {
        OutputToRegister = RetToRegister;
    }
    public override uint? DestinationRegister { get => 1; set => base.DestinationRegister = value; }
}
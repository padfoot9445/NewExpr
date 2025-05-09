using SmallLang.Backend;

namespace SmallLangTest.BackendComponentTests;
class PrimaryTestVisitorMock : CodeGenVisitor
{
    public PrimaryTestVisitorMock(bool RetToRegister)
    {
        OutputToRegister = RetToRegister;
    }
    public override uint? DestinationRegister { get => 1; set => base.DestinationRegister = value; }
}
using SmallLang.IR.AST.Generated;
using SmallLang.IR.Metadata;

namespace SmallLangTest;

[TestFixture]
public class ASTTests
{
    [Test]
    [CancelAfter(5000)]
    public void GetHashCode__Node_With_VariableNames_Changed__Returns_Different()
    {
        var node = ParserTest.Parse("int x = 1;");

        var initial = node.GetHashCode();

        (node.Statements[0] as DeclarationNode)!.VariableName = new VariableName("JD:LKSF");

        Assert.That(initial, Is.Not.EqualTo(node.GetHashCode()));
    }
}
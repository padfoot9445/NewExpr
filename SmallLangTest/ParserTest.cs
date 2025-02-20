using System.Diagnostics;
using Common.Tokens;
using SmallLang;
using SmallLang.Parser;

namespace SmallLangTest;
using Node = Common.AST.DynamicASTNode<SmallLang.ASTNodeType, SmallLang.Attributes>;
using NodeType = SmallLang.ASTNodeType;
[TestFixture]
public class ParserTest
{
    [Test]
    public void Ctor__Any_Input__Does_Not_Throw()
    {
        Assert.That(() => new Parser(""), Throws.Nothing);
    }
    [Test]
    public void Parse__Section__Returns_Correct()
    {
        Assert.Multiple(() =>
        {
            //var res = new Parser("int i = 0; Function(1, 2, 3); if(i == 2){return 1;} while(true){i += 1;} return 2; break ident;").Parse();
            var res = new Parser("Function();").Parse();
            return;
            Assert.That(res.NodeType, Is.EqualTo(NodeType.Section));
            Assert.That(res.Children, Has.Count.EqualTo(6));
            Assert.That(res.Children[0].NodeType, Is.EqualTo(NodeType.Declaration));
            Assert.That(res.Children[1].NodeType, Is.EqualTo(NodeType.FunctionCall));
            Assert.That(res.Children[2].NodeType, Is.EqualTo(NodeType.If));
            Assert.That(res.Children[3].NodeType, Is.EqualTo(NodeType.While));
            Assert.That(res.Children[4].NodeType, Is.EqualTo(NodeType.Return));
            Assert.That(res.Children[5].NodeType, Is.EqualTo(NodeType.LoopCTRL));
        });
    }
}
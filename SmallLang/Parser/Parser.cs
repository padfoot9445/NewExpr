namespace SmallLang.Parser;

using System.Text;
using Common.AST;
using Common.Tokens;
using sly.buildresult;
using sly.parser;
using sly.parser.generator;
using LYParser = sly.parser.Parser<Common.Tokens.TokenType, Common.AST.DynamicASTNode<ASTNodeType, Attributes>>;
using NodeType = Common.AST.DynamicASTNode<ASTNodeType, Attributes>;
using OutNodeType = Common.AST.DynamicASTNode<ImportantASTNodeType, Attributes>;
public class Parser
{
    private LYParser LyParser;
    string input;
    public Parser(string input)
    {
        this.input = input;
        LyParser = GetParser();
    }
    private LYParser GetParser()
    {
        var def = new SmallLangParser();
        BuildResult<LYParser> parserResult = new ParserBuilder<TokenType, NodeType>().BuildParser(def, ParserType.EBNF_LL_RECURSIVE_DESCENT, "NTSection"); //also NTSCExpr for tests
        if (parserResult.IsOk)
        {
            // everythin'fine : we have a configured parser
            return parserResult.Result;
        }
        else
        {
            // something's wrong
            foreach (var error in parserResult.Errors)
            {
                Console.WriteLine($"{error.Code} : {error.Message}");
            }
            throw new Exception(string.Join('\n', parserResult.Errors.Select(x => x.Message)));
        }
    }
    public OutNodeType Parse()
    {
        var r = LyParser.Parse(input);
        if (!r.IsError && r.Result != null && r.Result is NodeType)
        {
            return Map(r.Result);
        }
        else
        {
            StringBuilder sb = new(r.Errors.Count * 25);
            if (r.Errors != null && r.Errors.Any())
            {
                // display errors
                r.Errors.ForEach(error => { sb.Append(error.ErrorMessage); Console.WriteLine(error.ErrorMessage); });
            }
            throw new Exception(sb.ToString());
        }
    }
    static OutNodeType Map(NodeType node)
    {
        return new OutNodeType(node.Data, node.Children.Select(Map).ToList(), node.NodeType.ToImportant());
    }
}
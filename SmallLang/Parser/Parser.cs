using System.Text;
using Common.Tokens;
using sly.buildresult;
using sly.parser;
using sly.parser.generator;
using SmallLang.IR.AST;

namespace SmallLang.Parser;

using LYParser = Parser<TokenType, ISmallLangNode>;
using NodeType = ISmallLangNode;

public class Parser
{
    private readonly LYParser LyParser;
    private readonly string input;

    public Parser(string input)
    {
        this.input = input;
        LyParser = GetParser();
    }

    private LYParser GetParser()
    {
        var def = new SmallLangParser();
        BuildResult<LYParser> parserResult =
            new ParserBuilder<TokenType, NodeType>().BuildParser(def, ParserType.EBNF_LL_RECURSIVE_DESCENT,
                "NTSection"); //also NTSCExpr for tests
        if (parserResult.IsOk)
            // everythin'fine : we have a configured parser
            return parserResult.Result;

        // something's wrong
        foreach (var error in parserResult.Errors) Console.WriteLine($"{error.Code} : {error.Message}");
        throw new Exception(string.Join('\n', parserResult.Errors.Select(x => x.Message)));
    }

    public T Parse<T>()
    {
        var r = LyParser.Parse(input);
        if (!r.IsError && r.Result != null && r.Result is NodeType) return (T)r.Result;

        StringBuilder sb = new(r.Errors.Count * 25);
        if (r.Errors != null && r.Errors.Any())
            // display errors
            r.Errors.ForEach(error =>
            {
                sb.Append(error.ErrorMessage);
                Console.WriteLine(error.ErrorMessage);
            });
        throw new Exception(sb.ToString());
    }

    private static T Map<T>(NodeType node)
    {
        throw new NotImplementedException();
        // return new OutNodeType(node.Data, node.Children.Select(Map).ToList(), node.NodeType.ToImportant());
    }
}
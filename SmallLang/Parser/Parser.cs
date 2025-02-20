namespace SmallLang.Parser;

using Common.Tokens;
using sly.buildresult;
using sly.parser.generator;
using NodeType = Common.AST.DynamicASTNode<SmallLang.ASTNodeType, SmallLang.Attributes>;
public class Parser(string input)
{
    private Parser<TokenType, NodeType> GetParser()
    {
        var def = new SmallLangParser();
        BuildResult<Parser<TokenType, NodeType>> ParserResult = ParserBuilder<TokenType, NodeType>.BuildParser(def,
                                                                            ParserType.EBNF_LL_RECURSIVE_DESCENT,
                                                                            "Section");
        if (parserResult.IsOk)
        {
            // everythin'fine : we have a configured parser
            parser = parserResult.Result;
        }
        else
        {
            // something's wrong
            foreach (var error in parserResult.Errors)
            {
                Console.WriteLine($"{error.Code} : {error.Message}");
            }
        }
    }
    public NodeType Lex()
    {

    }
}
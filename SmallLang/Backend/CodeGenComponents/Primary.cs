using System.Diagnostics;
using System.Numerics;
using Common.AST;
using Common.Tokens;
using SmallLang.LinearIR;
using SmallLang.Metadata;

namespace SmallLang.Backend.CodeGenComponents;

class Primary : BaseCodeGenComponent
{
    public Primary(CodeGenVisitor driver) : base(driver)
    {
    }
    uint? LoadString(IToken data)
    {
        Debug.Assert(data.TT == TokenType.String && data.Literal is not null);
        char[] Chars = data.Literal.ToCharArray().Skip(1).SkipLast(1).ToArray();
        List<uint> write = [(uint)Chars.Length];
        write.AddRange(BytesToUInt(Chars.Select(x => (byte)x).ToArray()));

        var Ptr = WriteStaticData(StringTypeCode, write.ToArray());
        return LoadValue(Ptr);
    }
    uint? LoadNumber(IToken data, uint? ExpectedType)
    {

        if (ExpectedType is not uint Type) throw new Exception($"Cannot load number {data} as type null");
        return LoadValue(
            SwitchOnType(Type,
                () => new NotImplementedException(),
                (FloatTypeCode, (() => BitConverter.GetBytes(float.Parse(data.Lexeme)))),
                (IntTypeCode, (() => BitConverter.GetBytes(int.Parse(data.Lexeme)))),
                (DoubleTypeCode, (() => BitConverter.GetBytes(double.Parse(data.Lexeme)))),
                (LongTypeCode, (() => BitConverter.GetBytes(long.Parse(data.Lexeme)))),
                (LongintTypeCode, (() => throw new NotImplementedException())),
                (NumberTypeCode, (() => throw new NotImplementedException())),
                (ByteTypeCode, (() => [byte.Parse(data.Lexeme)])),
                (CharTypeCode, (() => data.Literal.Length != 3 ? throw new Exception($"Expected char length to be 3 ( 1 + two quotes), got {data.Literal.Length}") : data.Literal.Select(x => (byte)x).Skip(1).SkipLast(1).ToArray()))
            )
        );
    }
    uint? LoadBoolean(IToken data, bool Boolean) => LoadValue(Boolean ? uint.MaxValue : 0);
    uint? LoadVariable(IToken data)
    {
        return Driver.OutputToRegister ? Driver.VariableNameToRegister[data.Lexeme] : LoadValue(Driver.VariableNameToRegister[data.Lexeme]);
    }
    public override void GenerateCode(DynamicASTNode<ImportantASTNodeType, Attributes>? parent, DynamicASTNode<ImportantASTNodeType, Attributes> self)
    {
        Debug.Assert(self.Data is not null);
        uint? ResultLocation = self.Data.TT switch
        {
            TokenType.String => self.Attributes.TypeOfExpression == CharTypeCode ? LoadNumber(self.Data, CharTypeCode) : LoadString(self.Data),
            TokenType.Number => LoadNumber(self.Data, self.Attributes.TypeOfExpression),
            TokenType.TrueLiteral => LoadBoolean(self.Data, true),
            TokenType.FalseLiteral => LoadBoolean(self.Data, false),
            TokenType.Identifier => LoadVariable(self.Data),
            _ => throw new Exception($"Unexpected {self.Data.TT} in data slot of Primary"),
        };
        if (ResultLocation is not null) Driver.OutputRegisters = [(uint)ResultLocation];
    }
}
namespace MEXP.Parser;
public interface ITypeProvider
{
    public uint? GetTypeFromIdentifierLiteral(string Lexeme);
    public uint? StoreIdentifierType(string Lexeme, uint Type);
    public uint? BinOpResultantType(uint Type1, uint Type2);
    public uint GetTypeFromTypeDenotingIdentifier(string Lexeme);
    public bool CanBeAssignedTo(uint TypeRecieving, uint ExprType);
    public bool CanBeDeclaredTo(uint TypeRecieving, uint ExprType);
    public bool CanBeDeclaredTo(uint? typeDenotedByIdentifier, uint? ExprType);
    public uint GetTypeFromNumberLiteral(string Lexeme);
}
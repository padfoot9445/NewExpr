namespace Parser;
class TypeProvider
{
    public uint? GetTypeFromIdentifierLiteral(string Lexeme)
    {
        throw new NotImplementedException();
    }
    public uint? StoreIdentifierType(string Lexeme, uint Type)
    {
        throw new NotImplementedException();
    }
    public uint? BinOpResultantType(uint Type1, uint Type2)
    {
        //returns null if types cannot operate 
        throw new NotImplementedException();
    }
    public uint GetTypeFromTypeDenotingIdentifier(string Lexeme)
    {
        // TODO: implement this method
        throw new NotImplementedException();
        //should return the type denoted by the type, or throw an exception if it does not exist
    }
    public bool CanBeAssignedTo(uint TypeRecieving, uint ExprType)
    {
        throw new NotImplementedException();
    }
    public bool CanBeDeclaredTo(uint TypeRecieving, uint ExprType)
    {
        return true;
    }

    public bool CanBeDeclaredTo(uint? typeDenotedByIdentifier, uint? ExprType) => CanBeDeclaredTo((uint)typeDenotedByIdentifier!, (uint)ExprType!);
    public uint GetTypeFromNumberLiteral(string Lexeme)
    {
        // TODO: implement this method
        throw new NotImplementedException();
        //should return the lowest prec possible to avoid type clashes brought on by number literals, and we just make sure to generate the appropiate load/parsing code
    }

}
namespace MEXP.Parser;
public class TypeProvider : ITypeProvider
{
    Dictionary<string, uint> IdentToType = new();
    Dictionary<string, uint> ExistingTypeDenotingIdentifiers = new();
    uint TypeID = 1;
    //"int" | "float" | "double" | "number" | "long" | "longint" | "byte";
    public readonly uint IntTypeCode;
    public readonly uint FloatTypeCode;
    public readonly uint DoubleTypeCode;
    public readonly uint NumberTypeCode;
    public readonly uint LongTypeCode;
    public readonly uint LongIntTypeCode;
    public readonly uint ByteTypeCode;
    List<uint> DefaultTypesHierarchy;
    public TypeProvider()
    {
        IntTypeCode = GetTypeFromTypeDenotingIdentifier("int");
        FloatTypeCode = GetTypeFromTypeDenotingIdentifier("float");
        DoubleTypeCode = GetTypeFromTypeDenotingIdentifier("double");
        NumberTypeCode = GetTypeFromTypeDenotingIdentifier("number");
        LongTypeCode = GetTypeFromTypeDenotingIdentifier("long");
        LongIntTypeCode = GetTypeFromTypeDenotingIdentifier("longint");
        ByteTypeCode = GetTypeFromTypeDenotingIdentifier("byte");
        DefaultTypesHierarchy = [ByteTypeCode, IntTypeCode, LongTypeCode, LongIntTypeCode, FloatTypeCode, DoubleTypeCode, NumberTypeCode];
    }
    public uint? GetTypeFromIdentifierLiteral(string Lexeme)
    {
        if (Lexeme == "_")
        {
            return 0;
        }
        if (IdentToType.TryGetValue(Lexeme, out uint Type))
        {
            return Type;
        }
        return null;
    }
    public uint? StoreIdentifierType(string Lexeme, uint Type)
    {
        IdentToType[Lexeme] = Type;
        return Type;
    }
    public uint? BinOpResultantType(uint Type1, uint Type2)
    {
        //returns null if types cannot operate 
        return DefaultTypesHierarchy[Math.Max(DefaultTypesHierarchy.IndexOf(Type1), DefaultTypesHierarchy.IndexOf(Type2))];
    }
    public uint GetTypeFromTypeDenotingIdentifier(string Lexeme)
    {
        if (ExistingTypeDenotingIdentifiers.TryGetValue(Lexeme, out uint Type))
        {
            return Type;
        }
        ExistingTypeDenotingIdentifiers[Lexeme] = TypeID++;
        return GetTypeFromTypeDenotingIdentifier(Lexeme);
        //should return the type denoted by the type, or throw an exception if it does not exist
    }
    public bool CanBeAssignedTo(uint TypeRecieving, uint ExprType)
    {
        if (TypeRecieving == 0) return true;
        return DefaultTypesHierarchy.IndexOf(TypeRecieving) > DefaultTypesHierarchy.IndexOf(ExprType);
    }
    public bool CanBeDeclaredTo(uint TypeRecieving, uint ExprType)
    {
        return true;
    }

    public bool CanBeDeclaredTo(uint? typeDenotedByIdentifier, uint? ExprType) => CanBeDeclaredTo((uint)typeDenotedByIdentifier!, (uint)ExprType!);
    public uint GetTypeFromNumberLiteral(string Lexeme)
    {
        if (Lexeme.Contains('.'))
        {
            return GetTypeFromFloatLiteral(Lexeme.Length);
        }
        else
        {
            return GetTypeFromIntLiteral(Lexeme.Length);
        }
        //should return the lowest prec possible to avoid type clashes brought on by number literals, and we just make sure to generate the appropiate load/parsing code
    }
    public uint GetTypeFromIntLiteral(int Number)
    {
        return Number switch
        {
            (<= 2) => ByteTypeCode,
            (> 2) and (<= 9) => IntTypeCode,
            (> 9) and (<= 18) => LongTypeCode,
            _ => LongIntTypeCode
        };
    }
    public uint GetTypeFromFloatLiteral(int Length)
    {
        return NumberTypeCode;
    }
}
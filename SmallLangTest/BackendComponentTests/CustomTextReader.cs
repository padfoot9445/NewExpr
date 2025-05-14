namespace SmallLangTest.BackendComponentTests;
public class CustomTextReader(Stack<string> Strings) : TextReader
{
    public override string? ReadLine()
    {
        return Strings.Pop();
    }
}
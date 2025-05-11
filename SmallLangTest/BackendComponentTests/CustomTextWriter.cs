using System.Text;

namespace SmallLangTest.BackendComponentTests;
class CustomTextWriter : TextWriter
{
    TextWriter inner = Console.Out;
    public readonly StringBuilder outStore = new();
    public override void WriteLine(string? Value)
    {
        outStore.AppendLine(Value);
    }
    public override Encoding Encoding => inner.Encoding;
}
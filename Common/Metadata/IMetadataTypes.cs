namespace Common.Metadata;

public interface IMetadataTypes<TSelf> where TSelf : IMetadataTypes<TSelf>
{
    public bool CanDeclareTo(TSelf other);
    public bool ImplicitCast(TSelf other);
}
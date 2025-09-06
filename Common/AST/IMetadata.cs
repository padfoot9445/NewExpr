namespace Common.AST;

public interface IMetadata
{
    /// <summary>
    ///     Merges attributes from other with self. If there is a conflict, throw. It is likely that other must be the same
    ///     type as self.
    /// </summary>
    /// <param name="other"></param>
    public void Merge(IMetadata other);

    /// <summary>
    ///     Similar to Merge but does not throw on conflict. By default prioritizes self, but depends on PrioritizeOther.
    /// </summary>
    /// <param name="other"></param>
    /// <param name="PrioritizeOther"> Whether to Prioritize Attributes from the other.</param>
    public void ForcedMerge(IMetadata other, bool PrioritizeOther = false);
}
using Common.AST;

namespace ASTTest;
struct AnnotationContainerMock : IMetadata
{
    public AnnotationContainerMock() { }
    public AnnotationContainerMock(int i)
    {
        num = i;
    }
    public readonly int num = 0;
    public void ForcedMerge(IMetadata other, bool PrioritizeOther = false)
    {

    }

    public bool IsEquivalentTo(IMetadata other)
    {
        if (other is AnnotationContainerMock mock)
        {
            return num == mock.num;
        }
        return false;
    }

    public void Merge(IMetadata other)
    {

    }
}
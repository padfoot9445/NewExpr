using Common.AST;

namespace ASTTest;
struct AnnotationContainerMock : IMetadata
{
    public AnnotationContainerMock() { }

    public void ForcedMerge(IMetadata other, bool PrioritizeOther = false)
    {

    }

    public void Merge(IMetadata other)
    {

    }
}
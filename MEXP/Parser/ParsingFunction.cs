namespace MEXP.Parser;
using Common.AST;
delegate bool ParsingFunction(out AnnotatedNode<Annotations>? Node);
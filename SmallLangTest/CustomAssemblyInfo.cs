using System.Runtime.CompilerServices;

[assembly: Parallelizable(scope: ParallelScope.Children)]
[assembly: InternalsVisibleTo("RunStuff")]
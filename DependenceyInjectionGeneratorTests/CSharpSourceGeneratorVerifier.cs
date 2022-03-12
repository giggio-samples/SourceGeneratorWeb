using System.Collections.Immutable;
using System.IO;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;

namespace DependenceyInjectionGeneratorTests
{
    public class CSharpSourceGeneratorTest<TSourceGenerator> : CSharpSourceGeneratorTest<TSourceGenerator, XUnitVerifier> where TSourceGenerator : ISourceGenerator, new()
    {
        public CSharpSourceGeneratorTest(OutputKind outputKind)
        {
            ReferenceAssemblies = new ReferenceAssemblies("net6.0",
                                                          new PackageIdentity("Microsoft.NetCore.App.Ref", "6.0.0"),
                                                          Path.Combine("ref", "net6.0"))
                .AddPackages(ImmutableArray.Create(new PackageIdentity("Microsoft.AspNetCore.App.Ref", "6.0.0")));
            TestState.OutputKind = outputKind;
        }
    }
}

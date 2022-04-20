using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using System.Collections.Immutable;

namespace DependenceyInjectionGeneratorTests;

public class CSharpSourceGeneratorTest<TSourceGenerator> : CSharpSourceGeneratorTest<TSourceGenerator, XUnitVerifier> where TSourceGenerator : ISourceGenerator, new()
{
    public CSharpSourceGeneratorTest(OutputKind outputKind)
    {
        ReferenceAssemblies = ReferenceAssemblies.Net.Net60
            .AddPackages(ImmutableArray.Create(new PackageIdentity("Microsoft.AspNetCore.App.Ref", "6.0.0")));
        TestState.OutputKind = outputKind;
    }
}

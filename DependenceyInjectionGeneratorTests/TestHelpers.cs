using DependencyInjectionGenerator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;

namespace DependenceyInjectionGeneratorTests;

public static class TestHelpers
{
    public static readonly ReferenceAssemblies Net60WithAspNet = new ReferenceAssemblies("net6.0", new("Microsoft.NETCore.App.Ref", "6.0.0"), @"ref\net6.0")
            .WithPackages(ImmutableArray.Create(new PackageIdentity("Microsoft.AspNetCore.App.Ref", "6.0.0")));

    public static CSharpSourceGeneratorTest<Generator, XUnitVerifier> GetTestRunner(string source, OutputKind outputKind, params (string, SourceText)[] expectedGeneratedCode)
    {
        var testRunner = new CSharpSourceGeneratorTest<Generator, XUnitVerifier>()
        {
            TestState =
            {
                Sources = { source }
            }
        };
        testRunner.TestState.GeneratedSources.AddRange(expectedGeneratedCode);
        testRunner.TestState.ReferenceAssemblies = Net60WithAspNet;
        testRunner.TestState.OutputKind = outputKind;
        return testRunner;
    }
}

using System.Collections.Immutable;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;

namespace DependenceyInjectionGeneratorTests
{
    public class CSharpSourceGeneratorTest<TSourceGenerator> : CSharpSourceGeneratorTest<TSourceGenerator, XUnitVerifier> where TSourceGenerator : ISourceGenerator, new()
    {
        private readonly OutputKind outputKind;

        public CSharpSourceGeneratorTest(OutputKind outputKind)
        {
            ReferenceAssemblies = new ReferenceAssemblies("net6.0",
                                                          new PackageIdentity("Microsoft.NetCore.App.Ref", "6.0.0"),
                                                          Path.Combine("ref", "net6.0"))
                .AddPackages(ImmutableArray.Create(new PackageIdentity("Microsoft.AspNetCore.App.Ref", "6.0.0")));
            this.outputKind = outputKind;
        }

        protected override CompilationOptions CreateCompilationOptions() =>
            new CSharpCompilationOptions(outputKind, allowUnsafe: false);

        protected override ParseOptions CreateParseOptions() =>
            ((CSharpParseOptions)base.CreateParseOptions()).WithLanguageVersion(LanguageVersion.Default);

        protected override Project ApplyCompilationOptions(Project project)
        {
            project = base.ApplyCompilationOptions(project);
            if (project.CompilationOptions == null)
                project = project.WithCompilationOptions(CreateCompilationOptions());
            var modifiedCompilationOptions = project.CompilationOptions!.WithOutputKind(outputKind); // all this work just to change the output kind
            var solution = project.Solution.WithProjectCompilationOptions(project.Id, modifiedCompilationOptions);
            return solution.GetProject(project.Id)!;
        }
    }
}

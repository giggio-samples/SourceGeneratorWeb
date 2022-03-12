using Microsoft.CodeAnalysis.CSharp;
using Shouldly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit.Abstractions;

namespace DependenceyInjectionGeneratorTests
{
    public abstract class BaseTest
    {
        private readonly ITestOutputHelper output;
        private readonly OutputKind outputKind;

        public BaseTest(ITestOutputHelper output, OutputKind outputKind)
        {
            this.output = output ?? throw new ArgumentNullException(nameof(output));
            this.outputKind = outputKind;
        }

        protected (string, string) GetGeneratedOutput(string source)
        {
            var outputCompilation = CreateCompilation(source);
            var trees = outputCompilation.SyntaxTrees.Reverse().Take(2).Reverse().ToList();
            foreach (var tree in trees)
            {
                output.WriteLine(Path.GetFileName(tree.FilePath) + ":");
                output.WriteLine(tree.ToString());
            }
            return (trees.First().ToString(), trees[1].ToString());
        }

        protected List<string> GetAllGeneratedOutput(string source)
        {
            var outputCompilation = CreateCompilation(source);
            var trees = outputCompilation.SyntaxTrees.Reverse().Take(2).Reverse().ToList();
            foreach (var tree in trees)
            {
                output.WriteLine(Path.GetFileName(tree.FilePath) + ":");
                output.WriteLine(tree.ToString());
            }
            return trees.Select(t => t.ToString()).ToList();
        }

        protected Compilation CreateCompilation(string source)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(source);

            var references = new List<MetadataReference>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                if (!assembly.IsDynamic && !string.IsNullOrWhiteSpace(assembly.Location))
                    references.Add(MetadataReference.CreateFromFile(assembly.Location));
            references.Add(MetadataReference.CreateFromFile(typeof(Microsoft.AspNetCore.Builder.WebApplication).Assembly.Location));

            var compilation = CSharpCompilation.Create("foo",
                                                       new SyntaxTree[] { syntaxTree },
                                                       references,
                                                       new CSharpCompilationOptions(outputKind));

            var generator = new IncrementalGenerator(); // switch for "Generator" to test the non incremental generator

            var driver = CSharpGeneratorDriver.Create(generator);
            driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var generateDiagnostics);

            var compileDiagnostics = outputCompilation.GetDiagnostics();
            compileDiagnostics.Any(d => d.Severity == DiagnosticSeverity.Error).ShouldBeFalse("Failed: " + compileDiagnostics.FirstOrDefault()?.GetMessage());

            generateDiagnostics.Any(d => d.Severity == DiagnosticSeverity.Error).ShouldBeFalse("Failed: " + generateDiagnostics.FirstOrDefault()?.GetMessage());
            return outputCompilation;
        }
    }
}

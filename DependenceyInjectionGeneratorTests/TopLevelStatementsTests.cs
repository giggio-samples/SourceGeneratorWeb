using Microsoft.CodeAnalysis.Text;
using System.Text;
using System.Threading.Tasks;

namespace DependenceyInjectionGeneratorTests;

public class TopLevelStatementsTests
{
    [Fact]
    public async Task GeneratedCodeWithoutServicesWork()
    {
        var source = @"
using Microsoft.AspNetCore.Builder;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddServicesToDI();
";
        const string expectedAttributeCode = @"// <auto-generated />
[System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
internal class AddServiceAttribute : System.Attribute
{
}
";
        const string expectedExtensionCode = @"// <auto-generated />
using Microsoft.Extensions.DependencyInjection;

public static class GeneratedServicesExtension
{
    public static void AddServicesToDI(this IServiceCollection services)
    {
    }
}
";
        await new CSharpSourceGeneratorTest<Generator>(OutputKind.ConsoleApplication)
        {
            TestState =
            {
                Sources = { source },
                GeneratedSources =
                {
                    (typeof(Generator), "AddService.Generated.cs", SourceText.From(expectedAttributeCode, Encoding.UTF8, SourceHashAlgorithm.Sha1)),
                    (typeof(Generator), "GeneratedServicesExtension.Generated.cs", SourceText.From(expectedExtensionCode, Encoding.UTF8, SourceHashAlgorithm.Sha1)),
                },
            },
        }.RunAsync();
    }
}
using Microsoft.AspNetCore.Mvc;
using SourceGeneratorWeb.Models;
using System.Diagnostics;

namespace SourceGeneratorWeb.Controllers;

public class HomeController : Controller
{
    private readonly ExampleService exampleService;

    public HomeController(ExampleService exampleService) => this.exampleService = exampleService;

    public IActionResult Index() => View((object)exampleService.GetValue());

    public IActionResult Privacy() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}

using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using System.Diagnostics;

//Link used for kill process:
//https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.process.kill?view=net-11.0

namespace Bison.PlaywrightTests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class Tests : PageTest
{

    private Process _serverProcess;

    [SetUp]
    public async Task Init()
    {
        // Custom utility class - not part of Playwright
        _serverProcess = await EndToEndUtil.StartServer();
    }


    [TearDown]
    public async Task Cleanup()
    {
        _serverProcess.Kill(entireProcessTree: true); //stops the server after the test have run 
        _serverProcess.Dispose();
    }

    [Test]
    public async Task HomepageHasBisonInTitleAndLoginLinkWorks()
    {
        await Page.GotoAsync("http://localhost:5273");

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("Bison"));

        // create a locator
        var Login = Page.GetByRole(AriaRole.Link, new() { Name = "Login" });

        // Expect an attribute "to be strictly equal" to the value.
        await Expect(Login).ToHaveAttributeAsync("href", "/Identity/Account/Login");

        // Click the Login link.
        await Login.ClickAsync();

        // Expects the URL to contain Login.
        await Expect(Page).ToHaveURLAsync(new Regex(".*Login"));
    }

    [Test]
    public async Task RegisterLinkWorks()
    {
        await Page.GotoAsync("http://localhost:5273");

        // create a locator
        var Register = Page.GetByRole(AriaRole.Link, new() { Name = "Register" });

        // Expect an attribute "to be strictly equal" to the value
        await Expect(Register).ToHaveAttributeAsync("href", "/Identity/Account/Register");

        // Click the Login link
        await Register.ClickAsync();

        // Expects the URL to contain Login
        await Expect(Page).ToHaveURLAsync(new Regex(".*Register"));
    }

    [Test]
    public async Task PublicObservationsAreVisibleWhenNotLoggedIn()
    {
        await Page.GotoAsync("http://localhost:5273");

        // Expect an attribute "to be strictly equal" to the value
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Public Timeline" })).ToBeVisibleAsync();

    }

    [Test]
    public async Task UserTimelineIsAccessibleAndObservationBoxIsNotVisibleWhenNotLoggedIn()
    {
        await Page.GotoAsync("http://localhost:5273");

        // Click the author's link
        await Page.GetByRole(AriaRole.Link, new() { Name = "Nathan Sirmon" }).First.ClickAsync();

        // Click on the heading
        await Page.GetByRole(AriaRole.Heading, new() { Name = "Nathan Sirmon's Observations" }).ClickAsync();

        // Expect that the user's timeline is visible
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Nathan Sirmon's Observations" })).ToBeVisibleAsync();

        // Expect that the observation box is not visible
        await Expect(Page.GetByRole(AriaRole.Textbox)).Not.ToBeVisibleAsync();
    }
}

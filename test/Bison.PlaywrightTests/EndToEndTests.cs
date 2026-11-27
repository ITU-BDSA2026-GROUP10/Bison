using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace Bison.PlaywrightTests;

[TestFixture]
public class EndToEndTests : PageTest
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
        _serverProcess.Kill(entireProcessTree: true);
        _serverProcess.Dispose();
    }

    [Test]
    public async Task UserCanCreateObservationWithCorrectAuthor()
    {
        await Page.GotoAsync("http://localhost:5273");

        // Check that the My Observations link is not visible before login
        await Expect(Page.GetByRole(AriaRole.Link, new() { Name = "My Observations" })).Not.ToBeVisibleAsync();

        // Click the Register link
        await Page.GetByRole(AriaRole.Link, new() { Name = "Register" }).ClickAsync();

        // Click on the email field
        await Page.GetByPlaceholder("name@example.com").ClickAsync();

        //Fill out the email
        await Page.GetByPlaceholder("name@example.com").FillAsync("test@example.com");

        // Click on the Password field
        await Page.GetByLabel("Password", new() { Exact = true }).ClickAsync();

        // Fill out the password
        await Page.GetByLabel("Password", new() { Exact = true }).FillAsync("test123");

        // Click on Confirm Password field
        await Page.GetByLabel("Confirm Password").ClickAsync();

        // Fill out the password again
        await Page.GetByLabel("Confirm Password").FillAsync("test123");

        // Click Register button
        await Page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();

        // Click on link to confirm registration
        await Page.GetByRole(AriaRole.Link, new() { Name = "Click here to confirm your" }).ClickAsync();

        // Click the Login link
        await Page.GetByRole(AriaRole.Link, new() { Name = "Login" }).ClickAsync();

        // Click on the email field
        await Page.GetByPlaceholder("name@example.com").ClickAsync();

        //Fill out the email
        await Page.GetByPlaceholder("name@example.com").FillAsync("test@example.com");

        // Click on the Password field
        await Page.GetByPlaceholder("password").ClickAsync();

        // Fill out the password
        await Page.GetByPlaceholder("password").FillAsync("test123");

        // Click Login button
        await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();

        // Check that the My Observations link is visible after login
        await Expect(Page.GetByRole(AriaRole.Link, new() { Name = "My Observations" })).ToBeVisibleAsync();


        // Click the My Observations link
        await Page.GetByRole(AriaRole.Link, new() { Name = "My Observations" }).ClickAsync();

        // Check that theo observation box is visible 
        await Expect(Page.GetByRole(AriaRole.Textbox)).ToBeVisibleAsync();

        // Click on the observations box 
        await Page.GetByRole(AriaRole.Textbox).ClickAsync();

        // Write a observation
        await Page.GetByRole(AriaRole.Textbox).FillAsync("This is a test observation");

        // Click on the Post button
        await Page.GetByRole(AriaRole.Button, new() { Name = "Post" }).ClickAsync();

        // Click the Public Observations link
        await Page.GetByRole(AriaRole.Link, new() { Name = "Public Observations" }).ClickAsync();

        // Check that the observation is visible with the correct author
        await Expect(Page.Locator("li").Filter(new() { HasText = "test@example.com This is a test observation" })).ToBeVisibleAsync();


        // Click the author's link
        //await Page.GetByRole(AriaRole.Link, new() { Name = "test@example.com" }).ClickAsync();
    }
}
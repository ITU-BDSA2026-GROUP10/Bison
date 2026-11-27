using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace Bison.PlaywrightTests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class Tests : PageTest
{
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
}

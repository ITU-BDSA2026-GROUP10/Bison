global using Xunit;

using System.CommandLine;
using System.Net;
using System.Net.Http;

public class EndtoEndTest
{
    [Fact] 
    public async Task ProgramExistsCorrectlyAfterCommand ()
    {
        var rootCommand = new RootCommand();

        var exitCode = await rootCommand.InvokeAsync("observe Heron DR Byen");
        Assert.Equal(0, exitCode);
    }
}
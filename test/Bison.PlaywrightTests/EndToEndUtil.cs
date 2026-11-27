using System.Diagnostics;

namespace Bison.PlaywrightTests;

//used these links: 
//https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.processstartinfo?view=net-10.0 
//https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.processstartinfo.workingdirectory?view=net-10.0
//https://learn.microsoft.com/en-us/dotnet/api/system.io.directorynotfoundexception?view=net-10.0
//https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task.delay?view=net-11.0

public class EndToEndUtil
{
    public static async Task<Process> StartServer()
    {
        string path = Directory.GetCurrentDirectory();
        Console.WriteLine("The path is: " + path);
        ProcessStartInfo startInfo = new ProcessStartInfo("dotnet");
        startInfo.Arguments = "run";

        startInfo.WorkingDirectory = "../../../../../src/Bison.Web";

        if (Directory.Exists(startInfo.WorkingDirectory))
        {
            Console.WriteLine("The WorkingDirectory exitss!!!!!");

        }
        else
        {
            throw new DirectoryNotFoundException();
        }

        Process process = Process.Start(startInfo);

        if (process != null)
        {
            await Task.Delay(6000); //Allows Bison to build and start (10 sec)
            return process;
        }
        else
        {
            throw new InvalidOperationException("Failed to start Bison server");
        }


    }
}
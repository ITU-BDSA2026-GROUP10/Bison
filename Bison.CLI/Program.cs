using System;
using System.IO;
using System.Reflection.Metadata;

class Program
{
    //https://learn.microsoft.com/en-us/dotnet/standard/io/how-to-read-text-from-a-file
    //https://dev.to/maikomiyazaki/c-date-time-conversion-cheatsheet-3fm8#chapter-1
    static void Main (string[] args)
    {
        try
        {
            
            if(args[0] == "read")
            {
                using StreamReader reader = new("bison_observe_cli_db.csv");
                string text = reader.ReadLine(); //for the first line (not data)
                
                
                while ((text = reader.ReadLine()) != null)
                {
                    string[] info = text.Split(",");
                    Post post = new Post(info[0], info[1], info[2]);
                    //posts.Append(post);
                    Console.WriteLine(post.getAuthor() + " @ " + DateTimeOffset.FromUnixTimeSeconds(post.getTimecode()).DateTime + ": " + post.getObservation().Trim('\"'));

                    //the while loop that reads each line
                    //this is a more save way than regex for splitting at a new line with big data sets
                    //more on that at: https://stackoverflow.com/questions/1547476/split-a-string-on-newlines-in-net/23408020#23408020
                }
                
                //}
                
                /*
                Console.WriteLine(timecode); */

            } else if(args[0] == "observe") // https://learn.microsoft.com/en-us/dotnet/api/system.io.file.appendtext?view=net-8.0
            {
                string path = "bison_observe_cli_db.csv";
                using (StreamWriter writer = File.AppendText(path))
                {
                    //https://learn.microsoft.com/en-us/dotnet/api/system.environment.username?view=net-8.0
                    // https://learn.microsoft.com/en-us/dotnet/api/system.datetime.now?view=net-10.0
                    long lokalTid = DateTimeOffset.Now.ToUnixTimeSeconds();
                    
                    writer.WriteLine(Environment.UserName + ",\"" +  args[1] + "\"," + lokalTid);
                    
                    writer.Close();
                }
                
            }

        } catch (IOException e)
        {
            Console.WriteLine("The file could not be read:");
            Console.WriteLine(e.Message);
        } finally
        {
            
        }   
    }
}
 
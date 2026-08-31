using System;
using System.IO;
using System.Reflection.Metadata;

public class Program
{
    //https://learn.microsoft.com/en-us/dotnet/standard/io/how-to-read-text-from-a-file
    //https://dev.to/maikomiyazaki/c-date-time-conversion-cheatsheet-3fm8#chapter-1
    static void Main (string[] args)
    {
        try
        {
            using StreamReader reader = new("bison_observe_cli_db.csv");
            
            if(args[0] == "read")
            {
                string text = "hej";
                text = reader.ReadLine();

                string importantText = reader.ReadToEnd();
                
                string[] postInfo = importantText.Split("");
                Post[] posts = new Post[postInfo.Length];

                for (int i = 0; i < postInfo.Length; i++)
                {
                    string[] info = postInfo[i].Split(",");
                    Post post = new Post(info[0], info[1], long.Parse(info[2]));
                    posts.Append(post);
                    Console.WriteLine(post.GetAuthor() + post.GetObservation() + DateTimeOffset.FromUnixTimeSeconds(post.GetTime()).DateTime);
                }
                
                /*;
                Console.WriteLine(timecode); */

            } else if(args[0] == "observe")
            {
                
            }

        } catch (IOException e)
        {
            Console.WriteLine("The file could not be read:");
            Console.WriteLine(e.Message);
        }   
    }
}
 
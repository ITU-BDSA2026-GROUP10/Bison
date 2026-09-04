using System;
using System.IO;
using System.Reflection.Metadata;
using CsvHelper;
using System.Globalization;

public class Program
{

    static void Main (string[] args)
    {
        try
        {   
            if(args[0] == "read")
            {
                var reader = new StreamReader("bison_observe_cli_db.csv");
                var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                
                var records = csv.GetRecords<Cheep>();
                foreach (var r in records)
                {
                    Console.WriteLine($"{r.Author}, {r.Observation}, {r.Timestamp}");
                }
                
                

                /*using StreamReader reader = new("bison_observe_cli_db.csv");
                string text = reader.ReadLine(); //for the first line (not data)
                
                while ((text = reader.ReadLine()) != null)
                {
                    string[] info = text.Split(",");
                    Post post = new Post(info[0], info[1], info[2]);
                    //posts.Append(post);
                    DateTime time = DateTimeOffset.FromUnixTimeSeconds(post.getTimecode()).DateTime;
                    Console.WriteLine(post.getAuthor() + " @ " + time + ": " + post.getObservation().Trim('\"'));

                    //the while loop that reads each line
                    //this is a more save way than regex for splitting at a new line with big data sets
                    //more on that at: https://stackoverflow.com/questions/1547476/split-a-string-on-newlines-in-net/23408020#23408020
                }
                
                /*
                Console.WriteLine(timecode); */

            } else if(args[0] == "observe")
            {
                string path = "bison_observe_cli_db.csv";
                using (StreamWriter writer = File.AppendText(path))
                {
    
                    long lokalTid = DateTimeOffset.Now.ToUnixTimeSeconds() + 7200; //+7200 is to make the time match our time-zone
                    
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
 
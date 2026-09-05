using System;
using System.IO;
using System.Reflection.Metadata;
using CsvHelper;
using System.Globalization;
using SimpleDB;

public class Program
{

    static void Main (string[] args)
    {
        try
        {   
            if(args[0] == "read")
            {
                CSVDatabase<Cheep> csvDatabase = new CSVDatabase<Cheep>();
                IEnumerable<Cheep> enumerator = csvDatabase.Read();
                UserInterface.printObservations(enumerator);

                /*
                    //the while loop that reads each line
                    //this is a more save way than regex for splitting at a new line with big data sets
                    //more on that at: https://stackoverflow.com/questions/1547476/split-a-string-on-newlines-in-net/23408020#23408020
                */

            } else if(args[0] == "observe")
            {
                CSVDatabase<string> csvDatabase = new CSVDatabase<string>();
                csvDatabase.Store(args[1]);                
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
 
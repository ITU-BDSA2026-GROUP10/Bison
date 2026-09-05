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
 
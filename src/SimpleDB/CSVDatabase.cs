namespace SimpleDB;

using CsvHelper;
using System.ComponentModel.Design;
using System.Globalization;

sealed class CSVDatabase<T> : IDatabaseRepository<T> 
{
     
    public IEnumerable<T> Read(string path, int? limit = null) {
        IEnumerable <T> objects;
        var reader = new StreamReader(path);
        var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        objects = csv.GetRecords<T>();
        Console.WriteLine("Inde i read");
        Console.WriteLine(objects);
        return objects;
    }
 
    public void Store(T record, string path) {
        using (StreamWriter writer = File.AppendText(path))
        {

            long localTime = DateTimeOffset.Now.ToUnixTimeSeconds() + 7200; //+7200 is to make the time match our time-zone
            long id = Counter();

            writer.WriteLine(Environment.UserName + ",\"" +  record + "\"," + localTime + "," + id);
            
            writer.Close();
        }
    }

    //Used https://github.com/JoshClose/CsvHelper/issues/948 as reference
   private long Counter()
   {
       using(StreamReader reader = new StreamReader("bison_observe_cli_db.csv"))
       {
           int recordsLength = 0;
           while(reader.ReadLine() != null)
           {
               ++recordsLength;
           }
           recordsLength--; // line 1 doesn't count because there are column headers in first row.
           return recordsLength;
       }
   }
}
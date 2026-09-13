namespace SimpleDB;

using CsvHelper;
using System.ComponentModel.Design;
using System.Globalization;
using System.Linq.Expressions;

sealed public class CSVDatabase<T> : IDatabaseRepository<T> 
{
    public CSVDatabase(){}

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
            long id = Counter(path);

            writer.WriteLine(Environment.UserName + ",\"" +  record + "\"," + localTime + "," + id);
            
            writer.Close();
        }
    }

    public void StoreComment(T record, string path, string observePath, long ObservationID)
    {
        long count = Counter(observePath);
        try{
            if(count >= ObservationID)
            {
                using (StreamWriter writer = File.AppendText(path))
                {
                    long localTime = DateTimeOffset.Now.ToUnixTimeSeconds() + 7200; //+7200 is to make the time match our time-zone

                    writer.WriteLine(Environment.UserName + "," + ObservationID + ",\"" +  record + "\"," + localTime);
                    
                    writer.Close();
                }
            } else
            {
                throw new ArgumentException("The observation does not exist");
            }
        }
        catch (ArgumentException e)
        {
            Console.WriteLine(e.Message);
        }
    }

    //Used https://github.com/JoshClose/CsvHelper/issues/948 as reference
   private long Counter(string path)
   {
       using(StreamReader reader = new StreamReader(path))
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

   public long GetNumberOfLinesInAFile(string path)
    {
        return Counter(path);
    }
}
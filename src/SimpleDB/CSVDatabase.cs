namespace SimpleDB;

using CsvHelper;
using System.ComponentModel.Design;
using System.Globalization;
using System.Linq.Expressions;
using System.Security.AccessControl;
using System.Transactions;

sealed public class CSVDatabase<T> : IDatabaseRepository<T> 
{

    private static readonly CSVDatabase<T> instance = new CSVDatabase<T>();

    static CSVDatabase() {} 

    private CSVDatabase() {} 

    public static CSVDatabase<T> getInstance() 
    {
        return instance;
    }
     

    public IEnumerable<T> ReadObservation(string path, int? limit = null) {
        IEnumerable <T> objects;
        var reader = new StreamReader(path);
        var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        objects = csv.GetRecords<T>();
        return objects;
    }

    public IEnumerable<T> ReadDiscussion(string path, long observationId, int? limit = null) {
        IEnumerable <T> objects;
        var reader = new StreamReader(path);
        var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        objects = csv.GetRecords<T>();

        List<T> comments = new List<T>();
        foreach(var obj in objects)
        {
            if(obj is Comment comment && obj != null)
            {
                long id = comment.ObservationId;
                if(id == observationId)
                {
                    comments.Add(obj);
                }
            }
        }
        return comments;
    }
 
    public void Store(T record, string path) {
        using (StreamWriter writer = File.AppendText(path)) //"bison_observe_cli_cb.csv"
        {

            long localTime = DateTimeOffset.Now.ToUnixTimeSeconds() + 7200; //+7200 is to make the time match our time-zone
            long id = Counter(path); //"bison_observe_cli_cb.csv"

            writer.WriteLine(Environment.UserName + ",\"" +  record + "\"," + localTime + "," + id);
            
            writer.Close();
        }
    }

    public void StoreComment(T record, string observePath, long ObservationID)
    {
        long count = Counter(observePath);
        try{
            if(ObservationExists(count, ObservationID))
            {
                using (StreamWriter writer = File.AppendText("bison_comment_cli_db.csv"))
                {
                    long localTime = DateTimeOffset.Now.ToUnixTimeSeconds() + 7200; //+7200 is to make the time match our time-zone

                    writer.WriteLine(Environment.UserName + ",\"" +  record + "\"," + localTime + "," + ObservationID);
                    
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

    public bool ObservationExists(long count, long ObservationID)
    {
        return count >= ObservationID;
    }
}
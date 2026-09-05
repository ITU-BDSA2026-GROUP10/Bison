namespace SimpleDB;

using CsvHelper;
using System.ComponentModel.Design;
using System.Globalization;

sealed class CSVDatabase<T> : IDatabaseRepository<T> 
{
     
    public IEnumerable<T> Read(int? limit = null) {
        IEnumerable <T> objects;
        var reader = new StreamReader("bison_observe_cli_db.csv");
        var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        objects = csv.GetRecords<T>();
        return objects;

    }
 
    public void Store(T record) {
        string path = "bison_observe_cli_db.csv";
        using (StreamWriter writer = File.AppendText(path))
        {

            long localTime = DateTimeOffset.Now.ToUnixTimeSeconds() + 7200; //+7200 is to make the time match our time-zone
            
            writer.WriteLine(Environment.UserName + ",\"" +  record + "\"," + localTime);
            
            writer.Close();
        }
    }
}
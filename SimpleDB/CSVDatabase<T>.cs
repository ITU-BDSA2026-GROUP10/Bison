namespace simpleDB;

using CsvHelper;
using System.ComponentModel.Design;
using System.Globalization;

sealed class CSVDatabase<T> : IDatabaseRepository<T> 
{
    public IEnumerable<T> Read (int? limit = null) {
        IEnumerable <Cheep> objects;
        var reader = new StreamReader("bison_observe_cli_db.csv");
                var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                
                var records = csv.GetRecords<Cheep>();
                Console.WriteLine(records);

                
                foreach (var r in records)
                {
                    DateTime time = DateTimeOffset.FromUnixTimeSeconds(r.Timestamp).DateTime;
                    Console.WriteLine(r.Author + " @ " + time + ": " + r.Observation.Trim('\"'));
                    //Console.WriteLine($"{r.Author}, {r.Observation}, {r.Timestamp}");
                }
    }
 
    public void Store(T record) {
        
    }
}
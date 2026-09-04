namespace SimpleDB;

using CsvHelper;
using System.ComponentModel.Design;
using System.Globalization;

sealed class CSVDatabase<T> /*: IDatabaseRepository<T> */
{
     
    public /*IEnumerable<T> */ void Read(int? limit = null) {
        IEnumerable <T> objects;
        var reader = new StreamReader("bison_observe_cli_db.csv");
                var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                
                var records = csv.GetRecords<T>();
                Console.WriteLine(records);

                
                foreach (var r in records)
                {
                   //$ion.Trim('\"'));
                    //Console.WriteLine($"{r.Author}, {r.Observation}, {r.Timestamp}");
                }

    }
 
    public void Store(T record) {
        
    }
}

using System.Data;
using Microsoft.Data.Sqlite;
public class DBFacade
{
    string sqlDBFilePath = "bison.db"; //this does not work if the file does not exists:(
    string sqlQuery = @"SELECT observation.*, user.* FROM observation, user WHERE observation.author_id = user.user_id;";
    List<ObservationViewModel> list;

    public List<ObservationViewModel> writeObservations()
    {
        using (var connection = new SqliteConnection($"Data Source={sqlDBFilePath}"))
        {
            list = new List<ObservationViewModel>();

            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            using var reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                // https://learn.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqldatareader?view=dotnet-plat-ext-7.0#examples
                var dataRecord = (IDataRecord)reader;
                for (int i = 0; i < dataRecord.FieldCount; i++)
                {
                    //list.Add(new ObservationViewModel{dataRecord.GetName(i), dataRecord[i]});
                    Console.WriteLine($"{dataRecord.GetName(i)}: {dataRecord[i]}");
                }

                // See https://learn.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqldatareader.getvalues?view=dotnet-plat-ext-7.0
                // for documentation on how to retrieve complete columns from query results
                /*Object[] values = new Object[reader.FieldCount];
                int fieldCount = reader.GetValues(values);
                for (int i = 0; i < fieldCount; i++)
                    Console.WriteLine($"{reader.GetName(i)}: {values[i]}");*/
            }
            return list;
        }
    }
}
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
                string user_name = "";
                string message = "";
                string timestamp = "";

                // https://learn.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqldatareader?view=dotnet-plat-ext-7.0#examples
                var dataRecord = (IDataRecord)reader;
                for (int i = 0; i < dataRecord.FieldCount; i++)
                {
                    if (dataRecord.GetName(i) == "username")
                    {
                        user_name = (string) dataRecord[i];
                    } else if (dataRecord.GetName(i) == "text")
                    {
                        message =  (string) dataRecord[i];
                    } else if (dataRecord.GetName(i) == "pub_date")
                    {
                        timestamp = dataRecord[i].ToString();
                    }
                }

                list.Add(new ObservationViewModel(user_name, message, timestamp));
            }
            return list;
        }
    }
}
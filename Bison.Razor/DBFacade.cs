using System.Data;
using System;
using Microsoft.Data.Sqlite;
using System.IO;
public class DBFacade
{
    
    string sqlDBFilePath;
    string sqlQuery = @"SELECT observation.*, user.* FROM observation, user WHERE observation.author_id = user.user_id;";
    List<ObservationViewModel> list;

    public DBFacade()
    {   
        if(Environment.GetEnvironmentVariable("BISONDBPATH") != null)
        {
            sqlDBFilePath = Environment.GetEnvironmentVariable("BISONDBPATH");
        } else
        {
            sqlDBFilePath = Path.GetTempPath()+ "mybison.db";
        }

        /*using (var connection = new SqliteConnection($"Data Source={sqlDBFilePath}"))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = File.ReadAllText("data/schema.sql");
            command.ExecuteNonQuery();

            command.CommandText = File.ReadAllText("data/dump.sql");
            command.ExecuteNonQuery();
        }*/
    }
    
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
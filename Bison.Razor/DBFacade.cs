using System.Data;
using System;
using Microsoft.Data.Sqlite;
using Bison.Razor.BisonService;
namespace Bison.Razor;
public class DBFacade
{
    
    string sqlDBFilePath;
    string sqlQuery = @"SELECT observation.*, user.* FROM observation, user WHERE observation.author_id = user.user_id;";
    List<ObservationViewModel> list;

//https://learn.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlparametercollection.addwithvalue?view=netframework-4.8.1
// https://learn.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqldatareader?view=dotnet-plat-ext-7.0#examples
    public DBFacade()
    {   
        if(Environment.GetEnvironmentVariable("BISONDBPATH") != null)
        {
            sqlDBFilePath = Environment.GetEnvironmentVariable("BISONDBPATH");
        } else
        {
            sqlDBFilePath = Path.GetTempPath()+ "mybison.db";
        }

        using (var connection = new SqliteConnection($"Data Source={sqlDBFilePath}"))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = File.ReadAllText("data/schema.sql");
            command.ExecuteNonQuery();

            command.CommandText = File.ReadAllText("data/dump.sql");
            command.ExecuteNonQuery();
        }
    }
    
    public List<ObservationViewModel> getObservationsFromDatabase(string query, string? author = null)
    {
        using (var connection = new SqliteConnection($"Data Source={sqlDBFilePath}"))
        {
            sqlQuery = query;
            list = new List<ObservationViewModel>();

            connection.Open();

            var command = connection.CreateCommand();
            
            command.CommandText = sqlQuery;

            command.Parameters.AddWithValue("@author", author);

            using var reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                string user_name = "";
                string message = "";
                string timestamp = "";

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

                list.Add(new ObservationViewModel(user_name, message, ObservationService.UnixTimeStampToDateTimeString(Double.Parse(timestamp))));
            }
            return list;
        }
    }

    public List<ObservationViewModel> getObservationCommentsProposals(string[] queries, string? author = null)
    {
        using (var connection = new SqliteConnection($"Data Source={sqlDBFilePath}"))
        {

            list = new List<ObservationViewModel>();

            connection.Open();

            var command = connection.CreateCommand();
            
            command.CommandText = queries[0];

            command.Parameters.AddWithValue("@author", author);

            using var reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                string user_name = "";
                string message = "";
                string timestamp = "";

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
                    } else if (dataRecord.GetName(i) == "taxon_id")
                    {
                        message = (string) dataRecord[i];
                    }
                }

                list.Add(new ObservationViewModel(user_name, message, ObservationService.UnixTimeStampToDateTimeString(Double.Parse(timestamp))));
            }
            var command2 = connection.CreateCommand();
            
            command2.CommandText = queries[1];

            command2.Parameters.AddWithValue("@author", author);

            using var reader2 = command2.ExecuteReader();
            
            while (reader2.Read())
            {
                string user_name = "";
                string message = "";
                string timestamp = "";

                var dataRecord = (IDataRecord)reader2;
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
                    } else if (dataRecord.GetName(i) == "taxon_id")
                    {
                        message = (string) dataRecord[i];
                    }
                }

                list.Add(new ObservationViewModel(user_name, message, ObservationService.UnixTimeStampToDateTimeString(Double.Parse(timestamp))));
            }
            var command3 = connection.CreateCommand();
            
            command3.CommandText = queries[2];

            command3.Parameters.AddWithValue("@author", author);

            using var reader3 = command3.ExecuteReader();
            
            while (reader3.Read())
            {
                string user_name = "";
                string message = "";
                string timestamp = "";

                var dataRecord = (IDataRecord)reader3;
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
                    } else if (dataRecord.GetName(i) == "taxon_id")
                    {
                        message = (string) dataRecord[i];
                    }
                }

                list.Add(new ObservationViewModel(user_name, message, ObservationService.UnixTimeStampToDateTimeString(Double.Parse(timestamp))));
            }
            return list;
        }
    }
}
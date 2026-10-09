using System.Data;
using Microsoft.Data.Sqlite;
public class DBFacade
{
    string sqlDBFilePath = "bison.db"; //this does not work if the file does not exists:(
    string sqlQuery;
    List<ObservationViewModel> list;

//https://learn.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlparametercollection.addwithvalue?view=netframework-4.8.1
// https://learn.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqldatareader?view=dotnet-plat-ext-7.0#examples
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

    public List<ObservationViewModel> getObservationCommentsProposals(string [] queries, string? author = null)
    {
        using (var connection = new SqliteConnection($"Data Source={sqlDBFilePath}"))
        {
            string ObservationQuery = queries[0];
            string CommentQuery = queries[1];
            string ProposalQuery = queries[2];

            list = new List<ObservationViewModel>();

            connection.Open();

            var command = connection.CreateCommand();
            
            command.CommandText = ObservationQuery;

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
            var command2 = connection.CreateCommand();
            
            command2.CommandText = CommentQuery;
            
            using var reader2 = command2.ExecuteReader();
            
            while (reader2.Read())
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
                    } else if (dataRecord.GetName(i) == "pub_date_c")
                    {
                        timestamp = dataRecord[i].ToString();
                    }
                }

                list.Add(new ObservationViewModel(user_name, message, ObservationService.UnixTimeStampToDateTimeString(Double.Parse(timestamp))));
            }
            var command3 = connection.CreateCommand();
            
            command3.CommandText = ProposalQuery;
            
            using var reader3 = command3.ExecuteReader();
            
            while (reader3.Read())
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
                    } else if (dataRecord.GetName(i) == "taxon_id")
                    {
                        message =  (string) dataRecord[i];
                    } else if (dataRecord.GetName(i) == "pub_date_p")
                    {
                        timestamp = dataRecord[i].ToString();
                    }
                }

                list.Add(new ObservationViewModel(user_name, message, ObservationService.UnixTimeStampToDateTimeString(Double.Parse(timestamp))));
            }
            
            return list;
        }
    }
}
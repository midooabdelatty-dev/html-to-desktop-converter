using System.Data.SQLite;
using System.IO;
using Newtonsoft.Json;

namespace HtmlToDesktopConverter
{
    public class DatabaseBridge
    {
        private List<string> _dbPaths = new();
        private Dictionary<string, SQLiteConnection> _connections = new();

        public async Task<bool> InitializeDatabasesFromPath(string folderPath)
        {
            try
            {
                if (!Directory.Exists(folderPath))
                    return false;

                var dbFiles = Directory.GetFiles(folderPath, "*.db")
                    .Concat(Directory.GetFiles(folderPath, "*.db3"))
                    .ToList();

                foreach (var dbFile in dbFiles)
                {
                    _dbPaths.Add(dbFile);
                    var conn = new SQLiteConnection($"Data Source={dbFile};");
                    await conn.OpenAsync();
                    _connections[dbFile] = conn;
                }

                return _connections.Count > 0;
            }
            catch
            {
                return false;
            }
        }

        public dynamic? ExecuteQuery(string query)
        {
            try
            {
                var results = new List<dynamic>();

                foreach (var conn in _connections.Values)
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = query;
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var row = new Dictionary<string, object>();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    row[reader.GetName(i)] = reader.GetValue(i);
                                }
                                results.Add(row);
                            }
                        }
                    }
                }

                return results;
            }
            catch (Exception ex)
            {
                return new { error = ex.Message };
            }
        }

        public List<string> GetAllTables()
        {
            var tables = new List<string>();
            foreach (var conn in _connections.Values)
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table';";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tables.Add(reader["name"].ToString() ?? "");
                        }
                    }
                }
            }
            return tables;
        }

        public void Dispose()
        {
            foreach (var conn in _connections.Values)
            {
                conn?.Close();
                conn?.Dispose();
            }
            _connections.Clear();
        }
    }
}
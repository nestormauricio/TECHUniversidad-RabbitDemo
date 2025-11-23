using Microsoft.Data.Sqlite;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Consumer.Repositories
{
    public class SqliteMessageRepository
    {
        private readonly string _dbPath;

        public SqliteMessageRepository()
        {
            // Carpeta explícita dentro del bin para evitar ambigüedades
            var folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SqliteDb");
            Directory.CreateDirectory(folder);

            _dbPath = Path.Combine(folder, "messages.db");

            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            var tableCmd = @"
                CREATE TABLE IF NOT EXISTS Messages (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Content TEXT NOT NULL,
                    CreatedAt TEXT NOT NULL
                );
            ";

            using var command = new SqliteCommand(tableCmd, connection);
            command.ExecuteNonQuery();
        }

        // Método asíncrono (buena práctica)
        public async Task SaveMessageAsync(string message)
        {
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            await connection.OpenAsync();

            var insertCmd = @"
                INSERT INTO Messages (Content, CreatedAt)
                VALUES ($msg, $createdAt);
            ";

            using var command = new SqliteCommand(insertCmd, connection);
            command.Parameters.AddWithValue("$msg", message ?? string.Empty);
            command.Parameters.AddWithValue("$createdAt", DateTime.UtcNow.ToString("o"));

            await command.ExecuteNonQueryAsync();
        }

        // Envoltorio síncrono para compatibilidad con llamadas existentes
        public void SaveMessage(string message)
        {
            SaveMessageAsync(message).GetAwaiter().GetResult();
        }
    }
}






















// using Microsoft.Data.Sqlite;
// using System;
// using System.IO;
// using System.Threading.Tasks;

// namespace Consumer.Repositories
// {
//     public class SqliteMessageRepository
//     {
//         private readonly string _dbPath;

//         public SqliteMessageRepository()
//         {
//             var folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SqliteDb");
//             Directory.CreateDirectory(folder);

//             _dbPath = Path.Combine(folder, "messages.db");

//             InitializeDatabase();
//         }

//         private void InitializeDatabase()
//         {
//             using var connection = new SqliteConnection($"Data Source={_dbPath}");
//             connection.Open();

//             // Tabla corregida (SIN ERRORES)
//             var tableCmd = @"
//                 CREATE TABLE IF NOT EXISTS Messages (
//                     Id INTEGER PRIMARY KEY AUTOINCREMENT,
//                     Content TEXT NOT NULL,
//                     CreatedAt TEXT NOT NULL
//                 );
//             ";

//             using var command = new SqliteCommand(tableCmd, connection);
//             command.ExecuteNonQuery();
//         }

//         public async Task SaveMessageAsync(string message)
//         {
//             using var connection = new SqliteConnection($"Data Source={_dbPath}");
//             await connection.OpenAsync();

//             var insertCmd = @"
//                 INSERT INTO Messages (Content, CreatedAt)
//                 VALUES ($msg, $createdAt);
//             ";

//             using var command = new SqliteCommand(insertCmd, connection);
//             command.Parameters.AddWithValue("$msg", message);
//             command.Parameters.AddWithValue("$createdAt", DateTime.UtcNow.ToString("o"));

//             await command.ExecuteNonQueryAsync();
//         }
//     }
// }






















// using Microsoft.Data.Sqlite;
// using System;
// using System.IO;

// namespace Consumer.Repositories
// {
//     public class SqliteMessageRepository
//     {
//         private readonly string _dbFile;

//         public SqliteMessageRepository()
//         {
//             _dbFile = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\data\\rabbitdemo.db"));

//             var dir = Path.GetDirectoryName(_dbFile);
//             if (!Directory.Exists(dir))
//             {
//                 Directory.CreateDirectory(dir);
//             }

//             using var connection = new SqliteConnection($"Data Source={_dbFile}");
//             connection.Open();

//             using var cmd = connection.CreateCommand();
//             cmd.CommandText = @"
//                 CREATE TABLE IF NOT EXISTS Messages (
//                     Id INTEGER PRIMARY KEY AUTOINCREMENT,
//                     Text TEXT NOT NULL,
//                     CreatedAt TEXT NOT NULL
//                 );
//             ";
//             cmd.ExecuteNonQuery();
//         }

//         public void SaveMessage(string message)
//         {
//             using var connection = new SqliteConnection($"Data Source={_dbFile}");
//             connection.Open();

//             using var cmd = connection.CreateCommand();
//             cmd.CommandText = "INSERT INTO Messages (Text, CreatedAt) VALUES (, );";
//             cmd.Parameters.AddWithValue("", message ?? string.Empty);
//             cmd.Parameters.AddWithValue("", DateTime.UtcNow.ToString("o"));
//             cmd.ExecuteNonQuery();
//         }
//     }
// }

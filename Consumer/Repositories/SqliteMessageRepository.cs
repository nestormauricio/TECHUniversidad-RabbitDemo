using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace Consumer.Repositories
{
    public class SqliteMessageRepository
    {
        private readonly string _connectionString;
        private readonly string _dbPath;

        public SqliteMessageRepository()
        {
            // 📌 RUTA ABSOLUTA DEL ARCHIVO REAL QUE SE ESTÁ LLENANDO
            _dbPath = @"C:\Proyectos\TECHUniversidad\RabbitDemo\Consumer\bin\Debug\net9.0\SqliteDb\messages.db";

            // Asegurar carpeta
            var folder = Path.GetDirectoryName(_dbPath);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            _connectionString = $"Data Source={_dbPath}";

            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        CREATE TABLE IF NOT EXISTS Messages (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Content TEXT NOT NULL,
                            CreatedAt TEXT NOT NULL
                        );
                    ";
                    command.ExecuteNonQuery();
                }
            }
        }

        public void AddMessage(string messageContent)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        INSERT INTO Messages (Content, CreatedAt)
                        VALUES ($content, $createdAt);
                    ";
                    command.Parameters.AddWithValue("$content", messageContent);
                    command.Parameters.AddWithValue("$createdAt", DateTime.UtcNow.ToString("o"));

                    command.ExecuteNonQuery();
                }
            }
        }

        public string? GetMessage(int id)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        SELECT Content FROM Messages
                        WHERE Id = $id;
                    ";
                    command.Parameters.AddWithValue("$id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader.GetString(0);
                        }
                        return null;
                    }
                }
            }
        }

        public List<string> GetAllMessages()
        {
            var messages = new List<string>();

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        SELECT Content FROM Messages
                        ORDER BY Id;
                    ";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            messages.Add(reader.GetString(0));
                        }
                    }
                }
            }

            return messages;
        }

        // 🔥 NUEVO: MÉTODO PARA CONTAR MENSAJES
        public async Task<int> GetCountAsync()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"SELECT COUNT(*) FROM Messages;";
                    var result = await command.ExecuteScalarAsync();
                    return Convert.ToInt32(result);
                }
            }
        }
    }
}




















// using System;
// using System.Collections.Generic;
// using Microsoft.Data.Sqlite;

// namespace Consumer.Repositories
// {
//     public class SqliteMessageRepository
//     {
//         private readonly string _connectionString;

//         public SqliteMessageRepository(string databasePath)
//         {
//             _connectionString = $"Data Source={databasePath}";

//             using (var connection = new SqliteConnection(_connectionString))
//             {
//                 connection.Open();
//                 using (var command = connection.CreateCommand())
//                 {
//                     command.CommandText = @"
//                         CREATE TABLE IF NOT EXISTS Messages (
//                             Id INTEGER PRIMARY KEY AUTOINCREMENT,
//                             Content TEXT NOT NULL,
//                             CreatedAt TEXT NOT NULL
//                         );
//                     ";
//                     command.ExecuteNonQuery();
//                 }
//             }
//         }

//         public void AddMessage(string messageContent)
//         {
//             using (var connection = new SqliteConnection(_connectionString))
//             {
//                 connection.Open();
//                 using (var command = connection.CreateCommand())
//                 {
//                     command.CommandText = @"
//                         INSERT INTO Messages (Content, CreatedAt)
//                         VALUES ($content, $createdAt);
//                     ";
//                     command.Parameters.AddWithValue("$content", messageContent);
//                     command.Parameters.AddWithValue("$createdAt", DateTime.UtcNow.ToString("o"));

//                     command.ExecuteNonQuery();
//                 }
//             }
//         }

//         // ✅ Cambiado a string? para permitir null y eliminar CS8603
//         public string? GetMessage(int id)
//         {
//             using (var connection = new SqliteConnection(_connectionString))
//             {
//                 connection.Open();
//                 using (var command = connection.CreateCommand())
//                 {
//                     command.CommandText = @"
//                         SELECT Content FROM Messages
//                         WHERE Id = $id;
//                     ";
//                     command.Parameters.AddWithValue("$id", id);

//                     using (var reader = command.ExecuteReader())
//                     {
//                         if (reader.Read())
//                         {
//                             return reader.GetString(0);
//                         }
//                         return null;
//                     }
//                 }
//             }
//         }

//         public List<string> GetAllMessages()
//         {
//             var messages = new List<string>();

//             using (var connection = new SqliteConnection(_connectionString))
//             {
//                 connection.Open();
//                 using (var command = connection.CreateCommand())
//                 {
//                     command.CommandText = @"
//                         SELECT Content FROM Messages
//                         ORDER BY Id;
//                     ";

//                     using (var reader = command.ExecuteReader())
//                     {
//                         while (reader.Read())
//                         {
//                             messages.Add(reader.GetString(0));
//                         }
//                     }
//                 }
//             }

//             return messages;
//         }
//     }
// }





















// using System;
// using System.Collections.Generic;
// using Microsoft.Data.Sqlite;

// namespace Consumer.Repositories
// {
//     public class SqliteMessageRepository
//     {
//         private readonly string _connectionString;

//         public SqliteMessageRepository(string databasePath)
//         {
//             _connectionString = $"Data Source={databasePath}";

//             using (var connection = new SqliteConnection(_connectionString))
//             {
//                 connection.Open();
//                 using (var command = connection.CreateCommand())
//                 {
//                     command.CommandText = @"
//                         CREATE TABLE IF NOT EXISTS Messages (
//                             Id INTEGER PRIMARY KEY AUTOINCREMENT,
//                             Content TEXT NOT NULL,
//                             CreatedAt TEXT NOT NULL
//                         );
//                     ";
//                     command.ExecuteNonQuery();
//                 }
//             }
//         }

//         public void AddMessage(string messageContent)
//         {
//             using (var connection = new SqliteConnection(_connectionString))
//             {
//                 connection.Open();
//                 using (var command = connection.CreateCommand())
//                 {
//                     command.CommandText = @"
//                         INSERT INTO Messages (Content, CreatedAt)
//                         VALUES ($content, $createdAt);
//                     ";
//                     command.Parameters.AddWithValue("$content", messageContent);
//                     command.Parameters.AddWithValue("$createdAt", DateTime.UtcNow.ToString("o"));

//                     command.ExecuteNonQuery();
//                 }
//             }
//         }

//         // ✅ Cambiado a string? para permitir null y eliminar CS8603
//         public string? GetMessage(int id)
//         {
//             using (var connection = new SqliteConnection(_connectionString))
//             {
//                 connection.Open();
//                 using (var command = connection.CreateCommand())
//                 {
//                     command.CommandText = @"
//                         SELECT Content FROM Messages
//                         WHERE Id = $id;
//                     ";
//                     command.Parameters.AddWithValue("$id", id);

//                     using (var reader = command.ExecuteReader())
//                     {
//                         if (reader.Read())
//                         {
//                             return reader.GetString(0);
//                         }
//                         return null;
//                     }
//                 }
//             }
//         }

//         public List<string> GetAllMessages()
//         {
//             var messages = new List<string>();

//             using (var connection = new SqliteConnection(_connectionString))
//             {
//                 connection.Open();
//                 using (var command = connection.CreateCommand())
//                 {
//                     command.CommandText = @"
//                         SELECT Content FROM Messages
//                         ORDER BY Id;
//                     ";

//                     using (var reader = command.ExecuteReader())
//                     {
//                         while (reader.Read())
//                         {
//                             messages.Add(reader.GetString(0));
//                         }
//                     }
//                 }
//             }

//             return messages;
//         }
//     }
// }




















// using System;
// using System.Collections.Generic;
// using Microsoft.Data.Sqlite;

// namespace Consumer.Repositories
// {
//     public class SqliteMessageRepository
//     {
//         private readonly string _connectionString;

//         public SqliteMessageRepository(string databasePath)
//         {
//             _connectionString = $"Data Source={databasePath}";

//             using (var connection = new SqliteConnection(_connectionString))
//             {
//                 connection.Open();
//                 using (var command = connection.CreateCommand())
//                 {
//                     command.CommandText = @"
//                         CREATE TABLE IF NOT EXISTS Messages (
//                             Id INTEGER PRIMARY KEY AUTOINCREMENT,
//                             Content TEXT NOT NULL,
//                             CreatedAt TEXT NOT NULL
//                         );
//                     ";
//                     command.ExecuteNonQuery();
//                 }
//             }
//         }

//         public void AddMessage(string messageContent)
//         {
//             using (var connection = new SqliteConnection(_connectionString))
//             {
//                 connection.Open();
//                 using (var command = connection.CreateCommand())
//                 {
//                     command.CommandText = @"
//                         INSERT INTO Messages (Content, CreatedAt)
//                         VALUES ($content, $createdAt);
//                     ";
//                     command.Parameters.AddWithValue("$content", messageContent);
//                     command.Parameters.AddWithValue("$createdAt", DateTime.UtcNow.ToString("o"));

//                     command.ExecuteNonQuery();
//                 }
//             }
//         }

//         public string GetMessage(int id)
//         {
//             using (var connection = new SqliteConnection(_connectionString))
//             {
//                 connection.Open();
//                 using (var command = connection.CreateCommand())
//                 {
//                     command.CommandText = @"
//                         SELECT Content FROM Messages
//                         WHERE Id = $id;
//                     ";
//                     command.Parameters.AddWithValue("$id", id);

//                     using (var reader = command.ExecuteReader())
//                     {
//                         if (reader.Read())
//                         {
//                             return reader.GetString(0);
//                         }
//                         return null;
//                     }
//                 }
//             }
//         }

//         public List<string> GetAllMessages()
//         {
//             var messages = new List<string>();

//             using (var connection = new SqliteConnection(_connectionString))
//             {
//                 connection.Open();
//                 using (var command = connection.CreateCommand())
//                 {
//                     command.CommandText = @"
//                         SELECT Content FROM Messages
//                         ORDER BY Id;
//                     ";

//                     using (var reader = command.ExecuteReader())
//                     {
//                         while (reader.Read())
//                         {
//                             messages.Add(reader.GetString(0));
//                         }
//                     }
//                 }
//             }

//             return messages;
//         }
//     }
// }














// using System;
// using System.Collections.Generic;
// using Microsoft.Data.Sqlite;

// namespace Consumer.Repositories
// {
//     public class SqliteMessageRepository
//     {
//         private readonly string _connectionString;

//         public SqliteMessageRepository(string databasePath)
//         {
//             _connectionString = $"Data Source={databasePath}";

//             // Crear tabla si no existe
//             using var connection = new SqliteConnection(_connectionString);
//             connection.Open();
//             var command = connection.CreateCommand();
//             command.CommandText =
//             @"
//                 CREATE TABLE IF NOT EXISTS Messages (
//                     Id INTEGER PRIMARY KEY AUTOINCREMENT,
//                     Content TEXT NOT NULL,
//                     CreatedAt TEXT NOT NULL
//                 );
//             ";
//             command.ExecuteNonQuery();
//         }

//         public void AddMessage(string messageContent)
//         {
//             using var connection = new SqliteConnection(_connectionString);
//             connection.Open();

//             var command = connection.CreateCommand();
//             command.CommandText =
//             @"
//                 INSERT INTO Messages (Content, CreatedAt)
//                 VALUES ($content, $createdAt);
//             ";
//             command.Parameters.AddWithValue("$content", messageContent);
//             command.Parameters.AddWithValue("$createdAt", DateTime.UtcNow.ToString("o"));

//             command.ExecuteNonQuery();
//         }

//         public string GetMessage(int id)
//         {
//             using var connection = new SqliteConnection(_connectionString);
//             connection.Open();

//             var command = connection.CreateCommand();
//             command.CommandText =
//             @"
//                 SELECT Content FROM Messages
//                 WHERE Id = $id;
//             ";
//             command.Parameters.AddWithValue("$id", id);

//             using var reader = command.ExecuteReader();
//             if (reader.Read())
//             {
//                 return reader.GetString(0);
//             }
//             return null;
//         }

//         public List<string> GetAllMessages()
//         {
//             var messages = new List<string>();

//             using var connection = new SqliteConnection(_connectionString);
//             connection.Open();

//             var command = connection.CreateCommand();
//             command.CommandText =
//             @"
//                 SELECT Content FROM Messages
//                 ORDER BY Id;
//             ";

//             using var reader = command.ExecuteReader();
//             while (reader.Read())
//             {
//                 messages.Add(reader.GetString(0));
//             }

//             return messages;
//         }
//     }
// }
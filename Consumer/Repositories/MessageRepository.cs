using Microsoft.Data.Sqlite;
using RabbitDemo.MongoDb;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Consumer.Repositories
{
    public class MessageRepository
    {
        private readonly MongoDbService? _mongoDbService;
        private readonly string _sqliteFilePath;

        public MessageRepository()
        {
            // ===============================================
            // RUTA ABSOLUTA COMPARTIDA ENTRE API Y CONSUMER
            // ===============================================

            // Directorio raíz del proyecto Consumer (sube 4 niveles)
            var consumerRoot = Path.GetFullPath(
                Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\Consumer")
            );

            // Carpeta SQLite que ambos deben usar
            var sqliteFolder = Path.Combine(consumerRoot, "bin\\Debug\\net9.0\\SqliteDb");

            Directory.CreateDirectory(sqliteFolder);

            // ARCHIVO FINAL QUE AMBOS LEEN Y ESCRIBEN
            _sqliteFilePath = Path.Combine(sqliteFolder, "messages.db");

            // Asegurar BD y tabla
            EnsureSqliteDatabaseAndTable();

            // MongoDB inicializa si es posible
            try
            {
                _mongoDbService = new MongoDbService("mongodb://localhost:27017", "RabbitDemoDb");
            }
            catch
            {
                _mongoDbService = null;
            }
        }

        private void EnsureSqliteDatabaseAndTable()
        {
            var connectionString = $"Data Source={_sqliteFilePath};Mode=ReadWriteCreate;Cache=Shared";

            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Messages (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Content TEXT NOT NULL,
                    CreatedAt TEXT NOT NULL
                );
            ";
            command.ExecuteNonQuery();
        }

        // ================================================
        // GUARDAR MENSAJE EN SQLite Y MongoDB
        // ================================================
        public async Task SaveMessageAsync(string content)
        {
            var connectionString = $"Data Source={_sqliteFilePath};Mode=ReadWrite;Cache=Shared";

            using var connection = new SqliteConnection(connectionString);
            await connection.OpenAsync();
            using var command = connection.CreateCommand();

            command.CommandText = "INSERT INTO Messages (Content, CreatedAt) VALUES ($content, $createdAt);";
            command.Parameters.AddWithValue("$content", content);
            command.Parameters.AddWithValue("$createdAt", DateTime.UtcNow.ToString("o"));

            await command.ExecuteNonQueryAsync();

            // Guardar también en Mongo
            if (_mongoDbService != null)
            {
                try
                {
                    await Task.Run(() => _mongoDbService.AddMessage(content));
                }
                catch
                {
                    // Ignorar errores de Mongo sin romper flujo
                }
            }
        }

        // ================================================
        // OBTENER TODOS LOS MENSAJES DESDE SQLite
        // ================================================
        public List<string> GetAllMessages()
        {
            var messages = new List<string>();
            var connectionString = $"Data Source={_sqliteFilePath};Mode=ReadWrite;Cache=Shared";

            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "SELECT Content FROM Messages ORDER BY Id;";

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                messages.Add(reader.GetString(0));
            }

            return messages;
        }

        // ================================================
        // OBTENER CONTEO SQLite Y Mongo
        // ================================================
        public async Task<(int sqliteCount, long mongoCount)> GetCountsAsync(bool sqliteOnly = false)
        {
            int sqliteCount = 0;
            long mongoCount = 0;

            var connectionString = $"Data Source={_sqliteFilePath};Mode=ReadWrite;Cache=Shared";

            using (var connection = new SqliteConnection(connectionString))
            {
                await connection.OpenAsync();
                using var command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(*) FROM Messages;";
                var result = await command.ExecuteScalarAsync();
                sqliteCount = Convert.ToInt32(result ?? 0);
            }

            if (!sqliteOnly && _mongoDbService != null)
            {
                try
                {
                    mongoCount = await Task.Run(() => _mongoDbService.GetAllMessages().Count);
                }
                catch
                {
                    mongoCount = 0;
                }
            }

            return (sqliteCount, mongoCount);
        }
    }
}





















// using Microsoft.Data.Sqlite;
// using RabbitDemo.MongoDb;
// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Threading.Tasks;

// namespace Consumer.Repositories
// {
//     public class MessageRepository
//     {
//         private readonly MongoDbService? _mongoDbService;
//         private readonly string _sqliteFilePath;

//         public MessageRepository()
//         {
//             // Calcula ruta absoluta basada en el directorio donde corre la app (.NET)
//             // Esto funcionará tanto si ejecutas dotnet run --project RabbitDemo.Api
//             // como si ejecutas desde otro sitio.
//             var baseDir = AppContext.BaseDirectory;

//             // Carpeta local para SQLite dentro del folder de la API (asegura que exista)
//             var sqliteFolder = Path.Combine(baseDir, "SqliteDb");
//             Directory.CreateDirectory(sqliteFolder); // no falla si ya existe

//             // Nombre final del archivo DB
//             _sqliteFilePath = Path.Combine(sqliteFolder, "messages.db");

//             // Asegurar que el fichero existe y la tabla también (crea si no)
//             EnsureSqliteDatabaseAndTable();

//             // Inicializar MongoDB de forma segura (si falla, lo dejamos en null)
//             try
//             {
//                 _mongoDbService = new MongoDbService("mongodb://localhost:27017", "RabbitDemoDb");
//             }
//             catch
//             {
//                 _mongoDbService = null;
//             }
//         }

//         private void EnsureSqliteDatabaseAndTable()
//         {
//             // Usamos Mode=ReadWriteCreate para crear el fichero si no existe, y Cache=Shared para evitar bloqueos raros.
//             var connectionString = $"Data Source={_sqliteFilePath};Mode=ReadWriteCreate;Cache=Shared";

//             // Abrimos y creamos la tabla si no existe
//             using var connection = new SqliteConnection(connectionString);
//             connection.Open();
//             using var command = connection.CreateCommand();
//             command.CommandText = @"
//                 CREATE TABLE IF NOT EXISTS Messages (
//                     Id INTEGER PRIMARY KEY AUTOINCREMENT,
//                     Content TEXT NOT NULL,
//                     CreatedAt TEXT NOT NULL
//                 );
//             ";
//             command.ExecuteNonQuery();
//         }

//         // Guardar mensaje en SQLite (y Mongo cuando esté disponible)
//         public async Task SaveMessageAsync(string content)
//         {
//             var connectionString = $"Data Source={_sqliteFilePath};Mode=ReadWrite;Cache=Shared";
//             using var connection = new SqliteConnection(connectionString);
//             await connection.OpenAsync();
//             using var command = connection.CreateCommand();
//             command.CommandText = "INSERT INTO Messages (Content, CreatedAt) VALUES ($content, $createdAt);";
//             command.Parameters.AddWithValue("$content", content);
//             command.Parameters.AddWithValue("$createdAt", DateTime.UtcNow.ToString("o"));
//             await command.ExecuteNonQueryAsync();

//             // Intentar guardar en Mongo si está disponible (no rompe si falla)
//             if (_mongoDbService != null)
//             {
//                 try
//                 {
//                     await Task.Run(() => _mongoDbService.AddMessage(content));
//                 }
//                 catch
//                 {
//                     // ignorar errores de Mongo por ahora
//                 }
//             }
//         }

//         // Obtener todos los mensajes desde SQLite (rápido y fiable)
//         public List<string> GetAllMessages()
//         {
//             var messages = new List<string>();
//             var connectionString = $"Data Source={_sqliteFilePath};Mode=ReadWrite;Cache=Shared";
//             using var connection = new SqliteConnection(connectionString);
//             connection.Open();
//             using var command = connection.CreateCommand();
//             command.CommandText = "SELECT Content FROM Messages ORDER BY Id;";
//             using var reader = command.ExecuteReader();
//             while (reader.Read())
//             {
//                 messages.Add(reader.GetString(0));
//             }
//             return messages;
//         }

//         // Obtener conteos (SQLite siempre; Mongo solo si está disponible)
//         public async Task<(int sqliteCount, long mongoCount)> GetCountsAsync(bool sqliteOnly = false)
//         {
//             int sqliteCount = 0;
//             long mongoCount = 0;

//             var connectionString = $"Data Source={_sqliteFilePath};Mode=ReadWrite;Cache=Shared";
//             using (var connection = new SqliteConnection(connectionString))
//             {
//                 await connection.OpenAsync();
//                 using var command = connection.CreateCommand();
//                 command.CommandText = "SELECT COUNT(*) FROM Messages;";
//                 var result = await command.ExecuteScalarAsync();
//                 sqliteCount = Convert.ToInt32(result ?? 0);
//             }

//             if (!sqliteOnly && _mongoDbService != null)
//             {
//                 try
//                 {
//                     mongoCount = await Task.Run(() => _mongoDbService.GetAllMessages().Count);
//                 }
//                 catch
//                 {
//                     mongoCount = 0;
//                 }
//             }

//             return (sqliteCount, mongoCount);
//         }
//     }
// }






















// using Microsoft.Data.Sqlite;
// using RabbitDemo.MongoDb;
// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Threading.Tasks;

// namespace Consumer.Repositories
// {
//     public class MessageRepository
//     {
//         private readonly MongoDbService _mongoDbService;
//         private readonly string SqliteDbPath;

//         public MessageRepository()
//         {
//             // Ruta absoluta
//             string baseDir = AppContext.BaseDirectory; // Directorio donde corre el API
//             SqliteDbPath = Path.Combine(baseDir, "..\\..\\Consumer\\SqliteDb\\messages.db");
            
//             // Inicializar MongoDB
//             _mongoDbService = new MongoDbService("mongodb://localhost:27017", "RabbitDemoDb");
//         }

//         public async Task SaveMessageAsync(string content)
//         {
//             using var connection = new SqliteConnection($"Data Source={SqliteDbPath}");
//             await connection.OpenAsync();
//             var command = connection.CreateCommand();
//             command.CommandText = "INSERT INTO Messages (Content) VALUES (@content);";
//             command.Parameters.AddWithValue("@content", content);
//             await command.ExecuteNonQueryAsync();
//         }

//         public List<string> GetAllMessages()
//         {
//             var messages = new List<string>();
//             using var connection = new SqliteConnection($"Data Source={SqliteDbPath}");
//             connection.Open();
//             var command = connection.CreateCommand();
//             command.CommandText = "SELECT Content FROM Messages;";
//             using var reader = command.ExecuteReader();
//             while (reader.Read()) messages.Add(reader.GetString(0));
//             return messages;
//         }

//         public async Task<(int sqliteCount, int mongoCount)> GetCountsAsync()
//         {
//             int sqliteCount = 0;
//             int mongoCount = 0;

//             using (var connection = new SqliteConnection($"Data Source={SqliteDbPath}"))
//             {
//                 await connection.OpenAsync();
//                 var command = connection.CreateCommand();
//                 command.CommandText = "SELECT COUNT(*) FROM Messages;";
//                 sqliteCount = Convert.ToInt32(await command.ExecuteScalarAsync());
//             }

//             // MongoDB
//             mongoCount = _mongoDbService.GetAllMessages().Count;

//             return (sqliteCount, mongoCount);
//         }
//     }
// }





















// using Microsoft.Data.Sqlite;
// using RabbitDemo.MongoDb;
// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Threading.Tasks;

// namespace Consumer.Repositories
// {
//     public class MessageRepository
//     {
//         private readonly MongoDbService _mongoDbService;
//         private readonly string SqliteDbPath;

//         public MessageRepository()
//         {
//             // Path absoluto
//             SqliteDbPath = Path.Combine(AppContext.BaseDirectory, "Consumer\\SqliteDb\\messages.db");

//             // MongoDB opcional
//             try
//             {
//                 _mongoDbService = new MongoDbService("mongodb://localhost:27017", "RabbitDemoDb");
//             }
//             catch
//             {
//                 _mongoDbService = null;
//             }
//         }

//         public async Task SaveMessageAsync(string content)
//         {
//             using var connection = new SqliteConnection($"Data Source={SqliteDbPath}");
//             await connection.OpenAsync();
//             var command = connection.CreateCommand();
//             command.CommandText = "INSERT INTO Messages (Content) VALUES (@content);";
//             command.Parameters.AddWithValue("@content", content);
//             await command.ExecuteNonQueryAsync();
//         }

//         public List<string> GetAllMessages()
//         {
//             var messages = new List<string>();
//             using var connection = new SqliteConnection($"Data Source={SqliteDbPath}");
//             connection.Open();
//             var command = connection.CreateCommand();
//             command.CommandText = "SELECT Content FROM Messages;";
//             using var reader = command.ExecuteReader();
//             while (reader.Read()) messages.Add(reader.GetString(0));
//             return messages;
//         }

//         public async Task<(int sqliteCount, int mongoCount)> GetCountsAsync(bool sqliteOnly = false)
//         {
//             int sqliteCount = 0;
//             int mongoCount = 0;

//             using (var connection = new SqliteConnection($"Data Source={SqliteDbPath}"))
//             {
//                 await connection.OpenAsync();
//                 var command = connection.CreateCommand();
//                 command.CommandText = "SELECT COUNT(*) FROM Messages;";
//                 sqliteCount = Convert.ToInt32(await command.ExecuteScalarAsync());
//             }

//             if (!sqliteOnly && _mongoDbService != null)
//             {
//                 try
//                 {
//                     mongoCount = _mongoDbService.GetAllMessages().Count;
//                 }
//                 catch
//                 {
//                     mongoCount = 0;
//                 }
//             }

//             return (sqliteCount, mongoCount);
//         }
//     }
// }























// using Microsoft.Data.Sqlite;
// using RabbitDemo.MongoDb;
// using System;
// using System.Collections.Generic;
// using System.Threading.Tasks;

// namespace Consumer.Repositories
// {
//     public class MessageRepository
//     {
//         private readonly MongoDbService _mongoDbService;
//         private const string SqliteDbPath = "Consumer\\SqliteDb\\messages.db";

//         public MessageRepository()
//         {
//             // Solo inicializamos MongoDB si fuera necesario
//             try
//             {
//                 _mongoDbService = new MongoDbService("mongodb://localhost:27017", "RabbitDemoDb");
//             }
//             catch
//             {
//                 _mongoDbService = null;
//             }
//         }

//         // Guardar mensaje en SQLite
//         public async Task SaveMessageAsync(string content)
//         {
//             using (var connection = new SqliteConnection($"Data Source={SqliteDbPath}"))
//             {
//                 await connection.OpenAsync();
//                 var command = connection.CreateCommand();
//                 command.CommandText = "INSERT INTO Messages (Content) VALUES (@content);";
//                 command.Parameters.AddWithValue("@content", content);
//                 await command.ExecuteNonQueryAsync();
//             }
//         }

//         // Obtener todos los mensajes de SQLite
//         public List<string> GetAllMessages()
//         {
//             var messages = new List<string>();

//             using (var connection = new SqliteConnection($"Data Source={SqliteDbPath}"))
//             {
//                 connection.Open();
//                 var command = connection.CreateCommand();
//                 command.CommandText = "SELECT Content FROM Messages;";
//                 using (var reader = command.ExecuteReader())
//                 {
//                     while (reader.Read())
//                     {
//                         messages.Add(reader.GetString(0));
//                     }
//                 }
//             }

//             return messages;
//         }

//         // Obtener conteos, opcional SQLite-only
//         public async Task<(int sqliteCount, int mongoCount)> GetCountsAsync(bool sqliteOnly = false)
//         {
//             int sqliteCount = 0;
//             int mongoCount = 0;

//             // Contar SQLite
//             using (var connection = new SqliteConnection($"Data Source={SqliteDbPath}"))
//             {
//                 await connection.OpenAsync();
//                 var command = connection.CreateCommand();
//                 command.CommandText = "SELECT COUNT(*) FROM Messages;";
//                 sqliteCount = Convert.ToInt32(await command.ExecuteScalarAsync());
//             }

//             // MongoDB solo si no estamos en modo sqliteOnly y el servicio existe
//             if (!sqliteOnly && _mongoDbService != null)
//             {
//                 try
//                 {
//                     mongoCount = _mongoDbService.GetAllMessages().Count;
//                 }
//                 catch
//                 {
//                     mongoCount = 0; // ignorar errores de MongoDB temporalmente
//                 }
//             }

//             return (sqliteCount, mongoCount);
//         }
//     }
// }





















// using System;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using RabbitDemo.MongoDb;       // Para MongoDbService
// using Consumer.Repositories;    // Para SqliteMessageRepository

// namespace Consumer.Repositories
// {
//     public class MessageRepository
//     {
//         private readonly MongoDbService _mongoService;
//         private readonly SqliteMessageRepository _sqliteRepo;

//         public MessageRepository()
//         {
//             // --- MongoDB ---
//             _mongoService = new MongoDbService("mongodb://localhost:27017", "RabbitDemoDb");

//             // --- SQLite ---
//             var sqliteFolder = System.IO.Path.Combine(AppContext.BaseDirectory, "SqliteDb");
//             System.IO.Directory.CreateDirectory(sqliteFolder);
//             var sqlitePath = System.IO.Path.Combine(sqliteFolder, "messages.db");
//             _sqliteRepo = new SqliteMessageRepository(sqlitePath);
//         }

//         // Guardar mensaje en ambas bases de datos
//         public async Task SaveMessageAsync(string content)
//         {
//             // MongoDB
//             await Task.Run(() => _mongoService.AddMessage(content));

//             // SQLite
//             _sqliteRepo.AddMessage(content);
//         }

//         // Obtener todos los mensajes de ambas bases de datos
//         public List<string> GetAllMessages()
//         {
//             var mongoMessages = _mongoService.GetAllMessages();
//             var sqliteMessages = _sqliteRepo.GetAllMessages();

//             var allMessages = new List<string>();
//             allMessages.AddRange(mongoMessages);
//             allMessages.AddRange(sqliteMessages);

//             return allMessages;
//         }

//         // Obtener conteos separados
//         public async Task<(int sqliteCount, long mongoCount)> GetCountsAsync()
//         {
//             int sqliteCount = _sqliteRepo.GetAllMessages().Count;
//             long mongoCount = await Task.Run(() => _mongoService.GetAllMessages().Count);

//             return (sqliteCount, mongoCount);
//         }
//     }
// }




















// using System;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using RabbitDemo.MongoDb; // Para MongoDbService
// using Consumer.Repositories; // Para SqliteMessageRepository

// namespace Consumer.Repositories
// {
//     public class MessageRepository
//     {
//         private readonly MongoDbService _mongoService;
//         private readonly SqliteMessageRepository _sqliteRepo;

//         public MessageRepository()
//         {
//             // MongoDB
//             _mongoService = new MongoDbService("mongodb://localhost:27017", "RabbitDemoDb");

//             // SQLite
//             var sqliteFolder = System.IO.Path.Combine(AppContext.BaseDirectory, "SqliteDb");
//             System.IO.Directory.CreateDirectory(sqliteFolder);
//             var sqlitePath = System.IO.Path.Combine(sqliteFolder, "messages.db");
//             _sqliteRepo = new SqliteMessageRepository(sqlitePath);
//         }

//         public async Task SaveMessageAsync(string content)
//         {
//             // MongoDB
//             await Task.Run(() => _mongoService.AddMessage(content));

//             // SQLite
//             _sqliteRepo.AddMessage(content);
//         }

//         public List<string> GetAllMessages()
//         {
//             var mongoMessages = _mongoService.GetAllMessages();
//             var sqliteMessages = _sqliteRepo.GetAllMessages();

//             var allMessages = new List<string>();
//             allMessages.AddRange(mongoMessages);
//             allMessages.AddRange(sqliteMessages);

//             return allMessages;
//         }

//         public async Task<(int sqliteCount, long mongoCount)> GetCountsAsync()
//         {
//             int sqliteCount = _sqliteRepo.GetAllMessages().Count;
//             long mongoCount = await Task.Run(() => _mongoService.GetAllMessages().Count);

//             return (sqliteCount, mongoCount);
//         }
//     }
// }





















// using Consumer.Repositories;
// using RabbitDemo.MongoDb;
// using System;
// using System.Collections.Generic;
// using System.Threading.Tasks;

// namespace Consumer.Repositories
// {
//     public class MessageRepository
//     {
//         private readonly MongoDbService _mongoService;
//         private readonly SqliteMessageRepository _sqliteRepo;

//         public MessageRepository()
//         {
//             // MongoDB
//             _mongoService = new MongoDbService("mongodb://localhost:27017", "RabbitDemoDb");

//             // SQLite
//             var sqliteFolder = System.IO.Path.Combine(AppContext.BaseDirectory, "SqliteDb");
//             System.IO.Directory.CreateDirectory(sqliteFolder);
//             var sqlitePath = System.IO.Path.Combine(sqliteFolder, "messages.db");
//             _sqliteRepo = new SqliteMessageRepository(sqlitePath);
//         }

//         public async Task SaveMessageAsync(string content)
//         {
//             // MongoDB
//             await Task.Run(() => _mongoService.AddMessage(content));

//             // SQLite
//             _sqliteRepo.AddMessage(content);
//         }

//         public List<string> GetAllMessages()
//         {
//             var mongoMessages = _mongoService.GetAllMessages();
//             var sqliteMessages = _sqliteRepo.GetAllMessages();

//             var allMessages = new List<string>();
//             allMessages.AddRange(mongoMessages);
//             allMessages.AddRange(sqliteMessages);

//             return allMessages;
//         }

//         public async Task<(int sqliteCount, long mongoCount)> GetCountsAsync()
//         {
//             int sqliteCount = _sqliteRepo.GetAllMessages().Count;
//             long mongoCount = await Task.Run(() => _mongoService.GetAllMessages().Count);

//             return (sqliteCount, mongoCount);
//         }
//     }
// }

















// using MongoDB.Bson;
// using MongoDB.Driver;
// using System;
// using System.IO;
// using System.Threading.Tasks;

// namespace Consumer.Repositories
// {
//     public class MessageRepository
//     {
//         private readonly IMongoCollection<BsonDocument> _collection;
//         private readonly SqliteMessageRepository _sqliteRepo;

//         public MessageRepository()
//         {
//             // --- MongoDB ---
//             var client = new MongoClient("mongodb://localhost:27017");
//             var database = client.GetDatabase("RabbitDemoDb");
//             _collection = database.GetCollection<BsonDocument>("Messages");

//             var filter = new BsonDocument("name", "Messages");
//             var collections = database.ListCollections(new ListCollectionsOptions { Filter = filter }).ToList();
//             if (collections.Count == 0)
//             {
//                 database.CreateCollection("Messages");
//                 Console.WriteLine("Colección Messages creada en RabbitDemoDb");
//             }

//             // --- SQLite ---
//             var sqliteFolder = Path.Combine(AppContext.BaseDirectory, "SqliteDb");
//             Directory.CreateDirectory(sqliteFolder);
//             var sqlitePath = Path.Combine(sqliteFolder, "messages.db");

//             _sqliteRepo = new SqliteMessageRepository(sqlitePath);
//         }

//         public async Task SaveMessageAsync(string content)
//         {
//             // Guardar en Mongo
//             var document = new BsonDocument
//             {
//                 { "Content", content },
//                 { "CreatedAt", DateTime.UtcNow }
//             };
//             await _collection.InsertOneAsync(document);
//             Console.WriteLine($"[MongoDB] Mensaje guardado: {content}");

//             // Guardar en SQLite
//             _sqliteRepo.AddMessage(content);
//             Console.WriteLine($"[SQLite] Mensaje guardado: {content}");
//         }

//         public async Task<long> CountMessagesAsync()
//         {
//             return await _collection.CountDocumentsAsync(new BsonDocument());
//         }
//     }
// }




















// using MongoDB.Bson;
// using MongoDB.Driver;
// using System;
// using System.Threading.Tasks;

// namespace Consumer.Repositories
// {
//     public class MessageRepository
//     {
//         private readonly IMongoCollection<BsonDocument> _collection;

//         public MessageRepository()
//         {
//             var client = new MongoClient("mongodb://localhost:27017");
//             var database = client.GetDatabase("RabbitDemoDb"); // ✅ Base oficial
//             _collection = database.GetCollection<BsonDocument>("Messages");

//             // Crear colección si no existe (Mongo crea al insertar automáticamente)
//             var filter = new BsonDocument("name", "Messages");
//             var collections = database.ListCollections(new ListCollectionsOptions { Filter = filter }).ToList();
//             if (collections.Count == 0)
//             {
//                 database.CreateCollection("Messages");
//                 Console.WriteLine("Colección Messages creada en RabbitDemoDb");
//             }
//         }

//         public async Task SaveMessageAsync(string content)
//         {
//             var document = new BsonDocument
//             {
//                 { "Content", content },
//                 { "CreatedAt", DateTime.UtcNow }
//             };
//             await _collection.InsertOneAsync(document);
//             Console.WriteLine($"[MongoDB] Mensaje guardado: {content}");
//         }

//         public async Task<long> CountMessagesAsync()
//         {
//             return await _collection.CountDocumentsAsync(new BsonDocument());
//         }
//     }
// }


















// using MongoDB.Bson;
// using MongoDB.Driver;
// using System;
// using System.Threading.Tasks;

// namespace Consumer.Repositories
// {
//     public class MessageRepository
//     {
//         private readonly IMongoCollection<BsonDocument> _collection;

//         public MessageRepository()
//         {
//             var client = new MongoClient("mongodb://localhost:27017");
//             var database = client.GetDatabase("RabbitDemo");

//             // Crear colección si no existe
//             var collectionNames = database.ListCollectionNames().ToList();
//             if (!collectionNames.Contains("Messages"))
//             {
//                 database.CreateCollection("Messages");
//                 Console.WriteLine("Colección 'Messages' creada en MongoDB.");
//             }

//             _collection = database.GetCollection<BsonDocument>("Messages");
//         }

//         public async Task SaveMessageAsync(string message)
//         {
//             var doc = new BsonDocument
//             {
//                 { "Content", message },
//                 { "CreatedAt", DateTime.UtcNow }
//             };
//             await _collection.InsertOneAsync(doc);
//         }

//         public async Task<List<BsonDocument>> GetAllMessagesAsync()
//         {
//             return await _collection.Find(new BsonDocument()).Sort(Builders<BsonDocument>.Sort.Ascending("_id")).ToListAsync();
//         }
//     }
// }

























// using MongoDB.Bson;
// using MongoDB.Driver;

// namespace Consumer.Repositories
// {
//     public class MessageRepository
//     {
//         private readonly IMongoCollection<BsonDocument> _collection;

//         public MessageRepository()
//         {
//             var client = new MongoClient("mongodb://localhost:27017");
//             var database = client.GetDatabase("RabbitDemoDb");
//             _collection = database.GetCollection<BsonDocument>("Messages");
//         }

//         public async Task SaveMessageAsync(string message)
//         {
//             var doc = new BsonDocument
//             {
//                 { "text", message },
//                 { "createdAt", DateTime.UtcNow }
//             };

//             await _collection.InsertOneAsync(doc);
//         }
//     }
// }

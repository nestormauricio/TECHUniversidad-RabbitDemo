using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Consumer.Repositories
{
    public class MessageRepository
    {
        private readonly IMongoCollection<BsonDocument> _collection;
        private readonly SqliteMessageRepository _sqliteRepo;

        public MessageRepository()
        {
            // --- MongoDB ---
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("RabbitDemoDb");
            _collection = database.GetCollection<BsonDocument>("Messages");

            var filter = new BsonDocument("name", "Messages");
            var collections = database.ListCollections(new ListCollectionsOptions { Filter = filter }).ToList();
            if (collections.Count == 0)
            {
                database.CreateCollection("Messages");
                Console.WriteLine("Colección Messages creada en RabbitDemoDb");
            }

            // --- SQLite ---
            var sqliteFolder = Path.Combine(AppContext.BaseDirectory, "SqliteDb");
            Directory.CreateDirectory(sqliteFolder);
            var sqlitePath = Path.Combine(sqliteFolder, "messages.db");

            _sqliteRepo = new SqliteMessageRepository(sqlitePath);
        }

        public async Task SaveMessageAsync(string content)
        {
            // Guardar en Mongo
            var document = new BsonDocument
            {
                { "Content", content },
                { "CreatedAt", DateTime.UtcNow }
            };
            await _collection.InsertOneAsync(document);
            Console.WriteLine($"[MongoDB] Mensaje guardado: {content}");

            // Guardar en SQLite
            _sqliteRepo.AddMessage(content);
            Console.WriteLine($"[SQLite] Mensaje guardado: {content}");
        }

        public async Task<long> CountMessagesAsync()
        {
            return await _collection.CountDocumentsAsync(new BsonDocument());
        }
    }
}




















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

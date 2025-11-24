using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RabbitDemo.MongoDb
{
    public class MongoDbService
    {
        private readonly IMongoCollection<BsonDocument> _messagesCollection;

        public MongoDbService(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _messagesCollection = database.GetCollection<BsonDocument>("Messages");
        }

        public void AddMessage(string content)
        {
            var document = new BsonDocument
            {
                { "Content", content },
                { "CreatedAt", DateTime.UtcNow }
            };
            _messagesCollection.InsertOne(document);
        }

        public List<string> GetAllMessages()
        {
            return _messagesCollection.Find(new BsonDocument())
                                      .Sort(Builders<BsonDocument>.Sort.Ascending("_id"))
                                      .ToList()
                                      .Select(doc => doc["Content"].AsString)
                                      .ToList();
        }
    }
}























// using MongoDB.Bson;
// using MongoDB.Driver;
// using System;
// using System.Collections.Generic;
// using System.Linq;

// namespace RabbitDemo.MongoDb
// {
//     public class MongoDbService
//     {
//         private readonly IMongoCollection<BsonDocument> _messagesCollection;

//         // Constructor: conexión a MongoDB
//         public MongoDbService(string connectionString, string databaseName)
//         {
//             var client = new MongoClient(connectionString);
//             var database = client.GetDatabase(databaseName);
//             _messagesCollection = database.GetCollection<BsonDocument>("Messages");
//         }

//         // Agregar mensaje
//         public void AddMessage(string content)
//         {
//             var document = new BsonDocument
//             {
//                 { "Content", content },
//                 { "CreatedAt", DateTime.UtcNow } // Cambio seguro: sin BsonValue.Create
//             };
//             _messagesCollection.InsertOne(document);
//         }

//         // Obtener todos los mensajes
//         public List<string> GetAllMessages()
//         {
//             return _messagesCollection.Find(new BsonDocument())
//                                       .Sort(Builders<BsonDocument>.Sort.Ascending("_id"))
//                                       .ToList()
//                                       .Select(doc => doc["Content"].AsString)
//                                       .ToList();
//         }
//     }
// }




















// using MongoDB.Bson;
// using MongoDB.Driver;

// // using Microsoft.AspNetCore.Mvc;
// using RabbitDemo.MongoDb; // <-- Esto es clave

// using System;
// using System.Collections.Generic;
// using System.Linq;

// namespace RabbitDemo.MongoDb
// {
//     public class MongoDbService
//     {
//         private readonly IMongoCollection<BsonDocument> _messagesCollection;

//         public MongoDbService(string connectionString, string databaseName)
//         {
//             var client = new MongoClient(connectionString);
//             var database = client.GetDatabase(databaseName);
//             _messagesCollection = database.GetCollection<BsonDocument>("Messages");
//         }

//         public void AddMessage(string content)
//         {
//             var document = new BsonDocument
//             {
//                 { "Content", content },
//                 { "CreatedAt", BsonValue.Create(DateTime.UtcNow) }
//             };
//             _messagesCollection.InsertOne(document);
//         }

//         public List<string> GetAllMessages()
//         {
//             return _messagesCollection.Find(new BsonDocument())
//                                       .Sort(Builders<BsonDocument>.Sort.Ascending("_id"))
//                                       .ToList()
//                                       .Select(doc => doc["Content"].AsString)
//                                       .ToList();
//         }
//     }
// }





// using MongoDB.Bson;
// using MongoDB.Driver;

// // using Microsoft.AspNetCore.Mvc;
// using RabbitDemo.MongoDb; // <-- Esto es clave

// using System;
// using System.Collections.Generic;
// using System.Linq;



// namespace RabbitDemo.MongoDb
// {
//     public class MongoDbService
//     {
//         private readonly IMongoCollection<BsonDocument> _messagesCollection;

//         public MongoDbService(string connectionString, string databaseName)
//         {
//             var client = new MongoClient(connectionString);
//             var database = client.GetDatabase(databaseName);
//             _messagesCollection = database.GetCollection<BsonDocument>("Messages");
//         }

//         public void AddMessage(string content)
//         {
//             var document = new BsonDocument
//             {
//                 { "Content", content },
//                 { "CreatedAt", BsonValue.Create(DateTime.UtcNow) }
//             };
//             _messagesCollection.InsertOne(document);
//         }

//         public List<string> GetAllMessages()
//         {
//             return _messagesCollection.Find(new BsonDocument())
//                                       .Sort(Builders<BsonDocument>.Sort.Ascending("_id"))
//                                       .ToList()
//                                       .Select(doc => doc["Content"].AsString)
//                                       .ToList();
//         }
//     }
// }






















//                                                              Este ya no tiene errores
// using MongoDB.Bson;
// using MongoDB.Driver;
// using System;
// using System.Collections.Generic;
// using System.Linq;

// namespace RabbitDemo.MongoDb
// {
//     public class MongoDbService
//     {
//         private readonly IMongoCollection<BsonDocument> _messagesCollection;

//         public MongoDbService()
//         {
//             // Cambia la URL y DB según tu configuración real
//             var client = new MongoClient("mongodb://localhost:27017");
//             var database = client.GetDatabase("RabbitDemoDb");
//             _messagesCollection = database.GetCollection<BsonDocument>("Messages");
//         }

//         public int GetMessageCount()
//         {
//             return (int)_messagesCollection.CountDocuments(new BsonDocument());
//         }

//         public void AddMessage(string messageContent)
//         {
//             if (string.IsNullOrWhiteSpace(messageContent))
//                 throw new ArgumentException("El mensaje no puede ser vacío.", nameof(messageContent));

//             var document = new BsonDocument
//             {
//                 { "Content", messageContent },
//                 { "CreatedAt", DateTime.UtcNow }
//             };

//             _messagesCollection.InsertOne(document);
//         }

//         public List<string> GetAllMessages()
//         {
//             return _messagesCollection
//                 .Find(new BsonDocument())
//                 .ToList()
//                 .Select(doc => doc.GetValue("Content").AsString)
//                 .ToList();
//         }
//     }
// }
















// using MongoDB.Bson;
// using MongoDB.Driver;

// namespace RabbitDemo.MongoDb
// {
//     public class MongoDbService
//     {
//         private readonly IMongoCollection<BsonDocument> _messagesCollection;

//         public MongoDbService()
//         {
//             // Cambia "mongodb://localhost:27017" y "RabbitDemoDb" según tu configuración real
//             var client = new MongoClient("mongodb://localhost:27017");
//             var database = client.GetDatabase("RabbitDemoDb");
//             _messagesCollection = database.GetCollection<BsonDocument>("Messages");
//         }

//         // Retorna la cantidad de documentos en la colección Messages
//         public int GetMessageCount()
//         {
//             return (int)_messagesCollection.CountDocuments(new BsonDocument());
//         }

//         // Guarda un mensaje en MongoDB
//         public void AddMessage(string messageContent)
//         {
//             var document = new BsonDocument
//             {
//                 { "Content", messageContent },
//                 { "CreatedAt", DateTime.UtcNow }
//             };
//             _messagesCollection.InsertOne(document);
//         }

//         // Opcional: obtener todos los mensajes
//         public List<string> GetAllMessages()
//         {
//             var messages = _messagesCollection.Find(new BsonDocument()).ToList();
//             return messages.Select(m => m.GetValue("Content").AsString).ToList();
//         }
//     }
// }
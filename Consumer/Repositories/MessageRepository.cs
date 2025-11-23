using MongoDB.Bson;
using MongoDB.Driver;

namespace Consumer.Repositories
{
    public class MessageRepository
    {
        private readonly IMongoCollection<BsonDocument> _collection;

        public MessageRepository()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("RabbitDemoDb");
            _collection = database.GetCollection<BsonDocument>("Messages");
        }

        public async Task SaveMessageAsync(string message)
        {
            var doc = new BsonDocument
            {
                { "text", message },
                { "createdAt", DateTime.UtcNow }
            };

            await _collection.InsertOneAsync(doc);
        }
    }
}
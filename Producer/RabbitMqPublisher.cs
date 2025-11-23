using RabbitMQ.Client;
using System.Text;

namespace Producer
{
    public static class RabbitMqPublisher
    {
        private static readonly string QueueName = "demo-queue";

        public static void Publish(string message)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: "", routingKey: QueueName, basicProperties: null, body: body);
        }
    }
}

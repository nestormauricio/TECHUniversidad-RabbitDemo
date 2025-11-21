using RabbitMQ.Client;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };

        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(
                queue: "demo-queue",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            string message = "Hola desde RabbitMQ!";
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(
                exchange: "",
                routingKey: "demo-queue",
                basicProperties: null,
                body: body
            );

            Console.WriteLine($"[Producer] Mensaje enviado: {message}");
        }
    }
}



















// using RabbitMQ.Client;
// using System.Text;

// Console.WriteLine("Enviando mensaje a RabbitMQ...");

// var factory = new ConnectionFactory()
// {
//     HostName = "localhost"
// };

// using var connection = factory.CreateConnection();
// using var channel = connection.CreateModel();

// // Declaramos la cola donde enviaremos mensajes
// channel.QueueDeclare(queue: "demo-queue",
//                      durable: false,
//                      exclusive: false,
//                      autoDelete: false,
//                      arguments: null);

// string message = "Hola desde el Producer!";
// var body = Encoding.UTF8.GetBytes(message);

// // Publicamos el mensaje
// channel.BasicPublish(exchange: "",
//                      routingKey: "demo-queue",
//                      basicProperties: null,
//                      body: body);

// Console.WriteLine($"[x] Enviado: {message}");
















// // See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");
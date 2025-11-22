// using RabbitMQ.Client;
// using System;
// using System.Text;

// class Program
// {
//     static void Main(string[] args)
//     {
//         var factory = new ConnectionFactory()
//         {
//             HostName = "localhost",
//             UserName = "guest",
//             Password = "guest"
//         };

//         using var connection = factory.CreateConnection();
//         using var channel = connection.CreateModel();

//         // 1. Declarar exchange (ya existente)
//         channel.ExchangeDeclare(
//             exchange: "demo-exchange",
//             type: "direct",
//             durable: true,      // <- persistente
//             autoDelete: false,
//             arguments: null
//         );

//         // 2. Declarar cola principal persistente y con DLX
//         var queueArgs = new System.Collections.Generic.Dictionary<string, object>
//         {
//             { "x-dead-letter-exchange", "demo-dlx-exchange" } // <- Dead Letter Exchange
//         };

//         channel.QueueDeclare(
//             queue: "demo-queue",
//             durable: true,      // <- persistente
//             exclusive: false,
//             autoDelete: false,
//             arguments: queueArgs
//         );

//         channel.QueueBind(
//             queue: "demo-queue",
//             exchange: "demo-exchange",
//             routingKey: "demo.key"
//         );

//         // 3. Crear mensaje persistente
//         var message = "Hola desde el exchange con persistencia!";
//         var body = Encoding.UTF8.GetBytes(message);

//         var properties = channel.CreateBasicProperties();
//         properties.Persistent = true; // <- mensaje persistente

//         // 4. Publicar al exchange
//         channel.BasicPublish(
//             exchange: "demo-exchange",
//             routingKey: "demo.key",
//             basicProperties: properties,
//             body: body
//         );

//         Console.WriteLine("[Producer] Mensaje enviado al exchange con persistencia.");
//     }
// }



















using RabbitMQ.Client;
using System;
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

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        // 1. Declarar exchange
        channel.ExchangeDeclare(
            exchange: "demo-exchange",
            type: "direct",
            durable: false,
            autoDelete: false,
            arguments: null
        );

        // 2. Crear mensaje
        var message = "Hola desde el exchange!";
        var body = Encoding.UTF8.GetBytes(message);

        // 3. Publicar al exchange
        channel.BasicPublish(
            exchange: "demo-exchange",
            routingKey: "demo.key",
            basicProperties: null,
            body: body
        );

        Console.WriteLine("[Producer] Mensaje enviado al exchange.");
    }
}




















// using RabbitMQ.Client;
// using System.Text;

// class Program
// {
//     static void Main(string[] args)
//     {
//         var factory = new ConnectionFactory()
//         {
//             HostName = "localhost",
//             UserName = "guest",
//             Password = "guest"
//         };

//         using (var connection = factory.CreateConnection())
//         using (var channel = connection.CreateModel())
//         {
//             channel.QueueDeclare(
//                 queue: "demo-queue",
//                 durable: false,
//                 exclusive: false,
//                 autoDelete: false,
//                 arguments: null
//             );

//             string message = "Hola desde RabbitMQ!";
//             var body = Encoding.UTF8.GetBytes(message);

//             channel.BasicPublish(
//                 exchange: "",
//                 routingKey: "demo-queue",
//                 basicProperties: null,
//                 body: body
//             );

//             Console.WriteLine($"[Producer] Mensaje enviado: {message}");
//         }
//     }
// }



















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
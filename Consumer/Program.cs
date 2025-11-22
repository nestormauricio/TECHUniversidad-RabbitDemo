// using RabbitMQ.Client;
// using RabbitMQ.Client.Events;
// using System;
// using System.Text;
// using System.Collections.Generic;

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

//         // ========== COLA PRINCIPAL ==========
//         channel.QueueDeclare(
//             queue: "demo-queue",
//             durable: false,
//             exclusive: false,
//             autoDelete: false,
//             arguments: new Dictionary<string, object>
//             {
//                 { "x-dead-letter-exchange", "" },
//                 { "x-dead-letter-routing-key", "demo-queue-retry" }
//             }
//         );

//         // ========== COLA DE RETRY ==========
//         channel.QueueDeclare(
//             queue: "demo-queue-retry",
//             durable: false,
//             exclusive: false,
//             autoDelete: false,
//             arguments: new Dictionary<string, object>
//             {
//                 { "x-message-ttl", 5000 }, // 5 segundos
//                 { "x-dead-letter-exchange", "" },
//                 { "x-dead-letter-routing-key", "demo-queue" }
//             }
//         );

//         var consumer = new EventingBasicConsumer(channel);
//         consumer.Received += (model, ea) =>
//         {
//             var body = ea.Body.ToArray();
//             var message = Encoding.UTF8.GetString(body);

//             try
//             {
//                 Console.WriteLine($"[Consumer] Mensaje recibido: {message}");

//                 if (message.Contains("fail"))
//                     throw new Exception("Fallo simulado");

//                 Console.WriteLine("[Consumer] Procesado OK");
//                 channel.BasicAck(ea.DeliveryTag, false);
//             }
//             catch (Exception ex)
//             {
//                 Console.WriteLine($"[Consumer] Error: {ex.Message}");
//                 Console.WriteLine("[Consumer] Enviando a cola de retry...");
//                 channel.BasicReject(ea.DeliveryTag, false); // va a la dead-letter
//             }
//         };

//         channel.BasicConsume(
//             queue: "demo-queue",
//             autoAck: false,
//             consumer: consumer
//         );

//         Console.WriteLine("Esperando mensajes. Presiona ENTER para salir.");
//         Console.ReadLine();
//     }
// }





















using RabbitMQ.Client;
using RabbitMQ.Client.Events;
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

        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();

        channel.QueueDeclare(
            queue: "demo-queue",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"[Consumer] Mensaje recibido: {message}");
        };

        channel.BasicConsume(
            queue: "demo-queue",
            autoAck: true,
            consumer: consumer
        );

        Console.WriteLine("Esperando mensajes. Presiona ENTER para salir.");
        Console.ReadLine();
    }
}





















// using RabbitMQ.Client;
// using RabbitMQ.Client.Events;
// using System.Text;

// Console.WriteLine("Esperando mensajes...");

// var factory = new ConnectionFactory()
// {
//     HostName = "localhost"
// };

// using var connection = factory.CreateConnection();
// using var channel = connection.CreateModel();

// channel.QueueDeclare(
//     queue: "demo-queue",
//     durable: false,
//     exclusive: false,
//     autoDelete: false,
//     arguments: null
// );

// var consumer = new EventingBasicConsumer(channel);

// consumer.Received += (model, ea) =>
// {
//     var body = ea.Body.ToArray();
//     var message = Encoding.UTF8.GetString(body);

//     Console.WriteLine($"[x] Recibido: {message}");
// };

// channel.BasicConsume(
//     queue: "demo-queue",
//     autoAck: true,
//     consumer: consumer
// );

// Console.WriteLine("Presiona ENTER para salir");
// Console.ReadLine();






















// using RabbitMQ.Client;
// using RabbitMQ.Client.Events;
// using System.Text;

// Console.WriteLine("Esperando mensajes...");

// var factory = new ConnectionFactory()
// {
//     HostName = "localhost"
// };

// using var connection = factory.CreateConnection();
// using var channel = connection.CreateModel();

// // Declaramos la misma cola
// channel.QueueDeclare(queue: "demo-queue",
//                      durable: false,
//                      exclusive: false,
//                      autoDelete: false,
//                      arguments: null);

// // Creamos el consumidor
// var consumer = new EventingBasicConsumer(channel);

// consumer.Received += (model, ea) =>
// {
//     var body = ea.Body.ToArray();
//     var message = Encoding.UTF8.GetString(body);
//     Console.WriteLine($"[x] Recibido: {message}");
// };

// // Consumimos mensajes
// channel.BasicConsume(queue: "demo-queue",
//                      autoAck: true,
//                      consumer: consumer);

// // Mantenemos la app corriendo
// Console.WriteLine("Presiona [ENTER] para salir.");
// Console.ReadLine();



















// // See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");

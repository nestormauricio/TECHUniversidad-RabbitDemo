using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

class Program
{
    static void Main()
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        // 1. Declarar Dead Letter Exchange y Queue
        channel.ExchangeDeclare("demo-dlx-exchange", "direct", durable: true, autoDelete: false, arguments: null);
        channel.QueueDeclare("demo-dlx-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);
        channel.QueueBind("demo-dlx-queue", "demo-dlx-exchange", "dlx.key");

        // 2. Declarar exchange principal durable
        channel.ExchangeDeclare("demo-exchange", "direct", durable: true, autoDelete: false, arguments: null);

        // 3. Declarar queue principal con DLX
        var queueArgs = new System.Collections.Generic.Dictionary<string, object>
        {
            { "x-dead-letter-exchange", "demo-dlx-exchange" },
            { "x-dead-letter-routing-key", "dlx.key" }
        };
        channel.QueueDeclare("demo-queue", durable: true, exclusive: false, autoDelete: false, arguments: queueArgs);

        // 4. Hacer binding
        channel.QueueBind("demo-queue", "demo-exchange", "demo.key");

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"[Consumer] Mensaje recibido: {message}");
        };

        channel.BasicConsume("demo-queue", autoAck: true, consumer: consumer);

        Console.WriteLine("Esperando mensajes. Presiona ENTER para salir.");
        Console.ReadLine();
    }
}




















// using RabbitMQ.Client;
// using RabbitMQ.Client.Events;
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

//         var connection = factory.CreateConnection();
//         var channel = connection.CreateModel();

//         // 1. Declarar exchange
//         channel.ExchangeDeclare(
//             exchange: "demo-exchange",
//             type: "direct",
//             durable: false,
//             autoDelete: false,
//             arguments: null
//         );

//         // 2. Declarar queue
//         channel.QueueDeclare(
//             queue: "demo-queue",
//             durable: false,
//             exclusive: false,
//             autoDelete: false,
//             arguments: null
//         );

//         // 3. Hacer binding
//         channel.QueueBind(
//             queue: "demo-queue",
//             exchange: "demo-exchange",
//             routingKey: "demo.key"
//         );

//         var consumer = new EventingBasicConsumer(channel);

//         consumer.Received += (model, ea) =>
//         {
//             var body = ea.Body.ToArray();
//             var message = Encoding.UTF8.GetString(body);
//             Console.WriteLine($"[Consumer] Mensaje recibido: {message}");
//         };

//         channel.BasicConsume(
//             queue: "demo-queue",
//             autoAck: true,
//             consumer: consumer
//         );

//         Console.WriteLine("Esperando mensajes. Presiona ENTER para salir.");
//         Console.ReadLine();
//     }
// }
























// using RabbitMQ.Client;
// using RabbitMQ.Client.Events;
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

//         // 1. Declarar exchange principal
//         channel.ExchangeDeclare(
//             exchange: "demo-exchange",
//             type: "direct",
//             durable: true,
//             autoDelete: false
//         );

//         // 2. Declarar DLX y su cola
//         channel.ExchangeDeclare(
//             exchange: "demo-dlx-exchange",
//             type: "direct",
//             durable: true
//         );

//         channel.QueueDeclare(
//             queue: "demo-dlx-queue",
//             durable: true,
//             exclusive: false,
//             autoDelete: false,
//             arguments: null
//         );

//         channel.QueueBind(
//             queue: "demo-dlx-queue",
//             exchange: "demo-dlx-exchange",
//             routingKey: "dlx.key"
//         );

//         // 3. Declarar cola principal
//         var queueArgs = new System.Collections.Generic.Dictionary<string, object>
//         {
//             { "x-dead-letter-exchange", "demo-dlx-exchange" } // DLX
//         };

//         channel.QueueDeclare(
//             queue: "demo-queue",
//             durable: true,
//             exclusive: false,
//             autoDelete: false,
//             arguments: queueArgs
//         );

//         channel.QueueBind(
//             queue: "demo-queue",
//             exchange: "demo-exchange",
//             routingKey: "demo.key"
//         );

//         var consumer = new EventingBasicConsumer(channel);
//         consumer.Received += (model, ea) =>
//         {
//             var body = ea.Body.ToArray();
//             var message = Encoding.UTF8.GetString(body);
//             Console.WriteLine($"[Consumer] Mensaje recibido: {message}");
//             // opcional: channel.BasicAck(ea.DeliveryTag, false); si quieres manual ack
//         };

//         channel.BasicConsume(
//             queue: "demo-queue",
//             autoAck: true,
//             consumer: consumer
//         );

//         Console.WriteLine("Esperando mensajes. Presiona ENTER para salir.");
//         Console.ReadLine();
//     }
// }






















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

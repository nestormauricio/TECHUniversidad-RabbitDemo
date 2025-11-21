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

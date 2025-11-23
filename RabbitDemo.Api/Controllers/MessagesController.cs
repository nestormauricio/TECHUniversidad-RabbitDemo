using Microsoft.AspNetCore.Mvc;
using RabbitDemo.MongoDb; // <-- aquí sí se necesita el using

namespace RabbitDemo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly MongoDbService _mongoDbService;

        public MessagesController()
        {
            _mongoDbService = new MongoDbService("mongodb://localhost:27017", "RabbitDemoDb");
        }

        [HttpPost]
        public IActionResult AddMessage([FromBody] string content)
        {
            _mongoDbService.AddMessage(content);
            return Ok();
        }

        [HttpGet]
        public IActionResult GetMessages()
        {
            var messages = _mongoDbService.GetAllMessages();
            return Ok(messages);
        }
    }
}



















// using Microsoft.AspNetCore.Mvc;
// using Consumer.Repositories;
// using RabbitDemo.MongoDb;  // Ajusta al namespace real

// namespace RabbitDemo.Api.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class MessagesController : ControllerBase
//     {
//         private readonly SqliteMessageRepository _sqliteRepo;
//         private readonly MongoDbService _mongoService;

//         public MessagesController()
//         {
//             _sqliteRepo = new SqliteMessageRepository(@"..\Consumer\SqliteDb\messages.db");
//             _mongoService = new MongoDbService();
//         }

//         [HttpGet("sqlite/count")]
//         public IActionResult GetSqliteCount() => Ok(_sqliteRepo.GetAllMessages().Count);

//         [HttpGet("mongo/count")]
//         public IActionResult GetMongoCount() => Ok(_mongoService.GetMessageCount());
//     }
// }


















// using Microsoft.AspNetCore.Mvc;
// using Consumer.Repositories;         // Tu repositorio SQLite
// using MongoDb;                       // Tu proyecto MongoDb
// using Producer;                      // Tu publicador RabbitMQ

// namespace RabbitDemo.Api.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class MessagesController : ControllerBase
//     {
//         private readonly SqliteMessageRepository _sqliteRepo;
//         private readonly MongoDbService _mongoService;  // Ajusta según tu clase real
//         private readonly RabbitMqPublisher _publisher;

//         public MessagesController()
//         {
//             // Ajusta las rutas de tus bases de datos según tu proyecto
//             _sqliteRepo = new SqliteMessageRepository(@"..\Consumer\SqliteDb\messages.db");
//             _mongoService = new MongoDbService(); // Debes crear/ajustar esta clase
//             _publisher = new RabbitMqPublisher(); // Ajusta constructor si requiere config
//         }

//         [HttpGet("sqlite/count")]
//         public IActionResult GetSqliteCount()
//         {
//             var messages = _sqliteRepo.GetAllMessages();
//             return Ok(messages.Count);
//         }

//         [HttpGet("mongo/count")]
//         public IActionResult GetMongoCount()
//         {
//             var count = _mongoService.GetMessageCount(); // Ajusta método según tu MongoDbService
//             return Ok(count);
//         }

//         [HttpPost("send")]
//         public IActionResult SendMessage([FromBody] MessageDto dto)
//         {
//             if (string.IsNullOrWhiteSpace(dto.Message))
//                 return BadRequest("El mensaje no puede estar vacío.");

//             _publisher.Publish(dto.Message); // Publicar en RabbitMQ
//             return Ok(new { Status = "Mensaje enviado" });
//         }
//     }

//     public class MessageDto
//     {
//         public string Message { get; set; } = string.Empty;
//     }
// }













// using Microsoft.AspNetCore.Mvc;
// using Consumer.Repositories;
// using MongoDb;
// using Producer;

// namespace RabbitDemo.Api.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class MessagesController : ControllerBase
//     {
//         private readonly SqliteMessageRepository _sqliteRepo;
//         private readonly MongoMessageRepository _mongoRepo;

//         public MessagesController()
//         {
//             // Ruta completa a la DB SQLite
//             _sqliteRepo = new SqliteMessageRepository(
//                 @".\Consumer\bin\Debug\net9.0\SqliteDb\messages.db"
//             );

//             _mongoRepo = new MongoMessageRepository();
//         }

//         [HttpGet("sqlite/count")]
//         public IActionResult GetSqliteCount()
//         {
//             var count = _sqliteRepo.CountMessages();
//             return Ok(count);
//         }

//         [HttpGet("mongo/count")]
//         public IActionResult GetMongoCount()
//         {
//             var count = _mongoRepo.CountMessages();
//             return Ok(count);
//         }

//         [HttpPost("send")]
//         public IActionResult SendMessage([FromBody] MessageRequest request)
//         {
//             if (string.IsNullOrEmpty(request.Message))
//                 return BadRequest("Message cannot be empty.");

//             // Guardar en SQLite y MongoDB
//             _sqliteRepo.AddMessage(request.Message);
//             _mongoRepo.AddMessage(request.Message);

//             // Enviar a RabbitMQ
//             RabbitMqPublisher.Publish(request.Message);

//             return Ok();
//         }
//     }

//     public class MessageRequest
//     {
//         public string Message { get; set; }
//     }
// }


















// // URLs de tu back-end (ajusta según tu API real)
// const apiSQLiteCount = "http://localhost:5000/api/sqlite/count";
// const apiMongoCount = "http://localhost:5000/api/mongo/count";
// const apiSendMessage = "http://localhost:5000/api/messages/send";

// // Función para actualizar conteos
// async function updateCounts() {
//     try {
//         const sqliteRes = await fetch(apiSQLiteCount);
//         const mongoRes = await fetch(apiMongoCount);

//         const sqliteCount = await sqliteRes.json();
//         const mongoCount = await mongoRes.json();

//         document.getElementById("sqlite-count").textContent = sqliteCount;
//         document.getElementById("mongo-count").textContent = mongoCount;
//     } catch (err) {
//         console.error("Error obteniendo conteos:", err);
//     }
// }

// // Función para enviar mensaje a RabbitMQ
// async function sendMessage() {
//     const message = document.getElementById("message-input").value;
//     if (!message) return alert("Ingresa un mensaje.");

//     try {
//         const res = await fetch(apiSendMessage, {
//             method: "POST",
//             headers: { "Content-Type": "application/json" },
//             body: JSON.stringify({ message })
//         });
//         if (res.ok) {
//             alert("Mensaje enviado correctamente.");
//             document.getElementById("message-input").value = "";
//             updateCounts(); // actualizar conteos tras enviar
//         } else {
//             alert("Error enviando mensaje.");
//         }
//     } catch (err) {
//         console.error("Error enviando mensaje:", err);
//     }
// }

// // Eventos
// document.getElementById("send-btn").addEventListener("click", sendMessage);

// // Actualizar conteos al cargar
// updateCounts();










// using Microsoft.AspNetCore.Mvc;
// using Consumer.Repositories;
// using MongoDb; // Ajusta según tu namespace de MongoMessageRepository

// [ApiController]
// [Route("api/[controller]")]
// public class MessagesController : ControllerBase
// {
//     private readonly SqliteMessageRepository _sqliteRepo;
//     private readonly MongoMessageRepository _mongoRepo;

//     public MessagesController(SqliteMessageRepository sqliteRepo, MongoMessageRepository mongoRepo)
//     {
//         _sqliteRepo = sqliteRepo;
//         _mongoRepo = mongoRepo;
//     }

//     [HttpGet("sqlite/count")]
//     public IActionResult GetSqliteCount()
//     {
//         return Ok(new { count = _sqliteRepo.Count() });
//     }

//     [HttpGet("mongo/count")]
//     public IActionResult GetMongoCount()
//     {
//         return Ok(new { count = _mongoRepo.Count() });
//     }

//     [HttpPost("send")]
//     public IActionResult SendMessage([FromBody] string message)
//     {
//         Producer.RabbitMqPublisher.Publish(message); // Usando tu Publisher existente
//         return Ok(new { status = "Mensaje enviado" });
//     }
// }
using Microsoft.AspNetCore.Mvc;
using Consumer.Repositories; // Para MessageRepository
using System.Threading.Tasks;

namespace RabbitDemo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly MessageRepository _messageRepository;

        public MessagesController()
        {
            _messageRepository = new MessageRepository();
        }

        // POST api/messages
        [HttpPost]
        public async Task<IActionResult> AddMessage([FromBody] string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return BadRequest("El mensaje no puede estar vacío.");

            await _messageRepository.SaveMessageAsync(content);
            return Ok();
        }

        // GET api/messages
        [HttpGet]
        public IActionResult GetMessages()
        {
            var allMessages = _messageRepository.GetAllMessages();
            return Ok(allMessages);
        }

        // GET api/messages/count
        [HttpGet("count")]
        public async Task<IActionResult> GetCounts()
        {
            var counts = await _messageRepository.GetCountsAsync();

            return Ok(new
            {
                Sqlite = counts.sqliteCount,
                MongoDB = counts.mongoCount
            });
        }
    }
}



















// using Microsoft.AspNetCore.Mvc;
// using Consumer.Repositories; // Para MessageRepository
// using System.Threading.Tasks;

// namespace RabbitDemo.Api.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class MessagesController : ControllerBase
//     {
//         private readonly MessageRepository _messageRepository;

//         public MessagesController()
//         {
//             _messageRepository = new MessageRepository();
//         }

//         // POST api/messages
//         [HttpPost]
//         public async Task<IActionResult> AddMessage([FromBody] string content)
//         {
//             if (string.IsNullOrWhiteSpace(content))
//                 return BadRequest("El mensaje no puede estar vacío.");

//             await _messageRepository.SaveMessageAsync(content);
//             return Ok();
//         }

//         // GET api/messages
//         [HttpGet]
//         public IActionResult GetMessages()
//         {
//             var allMessages = _messageRepository.GetAllMessages();
//             return Ok(allMessages);
//         }

//         // GET api/messages/count
//         [HttpGet("count")]
//         public async Task<IActionResult> GetCounts()
//         {
//             var counts = await _messageRepository.GetCountsAsync();

//             return Ok(new
//             {
//                 Sqlite = counts.sqliteCount,
//                 MongoDB = counts.mongoCount
//             });
//         }
//     }
// }





















// using Microsoft.AspNetCore.Mvc;
// using Consumer.Repositories; // Para MessageRepository
// using System.Threading.Tasks;

// namespace RabbitDemo.Api.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class MessagesController : ControllerBase
//     {
//         private readonly MessageRepository _messageRepository;

//         public MessagesController()
//         {
//             _messageRepository = new MessageRepository();
//         }

//         // POST api/messages
//         [HttpPost]
//         public async Task<IActionResult> AddMessage([FromBody] string content)
//         {
//             if (string.IsNullOrWhiteSpace(content))
//                 return BadRequest("El mensaje no puede estar vacío.");

//             await _messageRepository.SaveMessageAsync(content);
//             return Ok();
//         }

//         // GET api/messages
//         [HttpGet]
//         public async Task<IActionResult> GetMessages()
//         {
//             var allMessages = _messageRepository.GetAllMessages();
//             var counts = await _messageRepository.GetCountsAsync();

//             return Ok(new
//             {
//                 Messages = allMessages,
//                 SqliteCount = counts.sqliteCount,
//                 MongoCount = counts.mongoCount
//             });
//         }
//     }
// }























// using Microsoft.AspNetCore.Mvc;
// using RabbitDemo.MongoDb; // <-- necesario

// namespace RabbitDemo.Api.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class MessagesController : ControllerBase
//     {
//         private readonly MongoDbService _mongoDbService;

//         public MessagesController()
//         {
//             _mongoDbService = new MongoDbService("mongodb://localhost:27017", "RabbitDemoDb");
//         }

//         [HttpPost]
//         public IActionResult AddMessage([FromBody] string content)
//         {
//             _mongoDbService.AddMessage(content);
//             return Ok();
//         }

//         [HttpGet]
//         public IActionResult GetMessages()
//         {
//             var messages = _mongoDbService.GetAllMessages();
//             return Ok(messages);
//         }
//     }
// }




















// using Microsoft.AspNetCore.Mvc;
// using RabbitDemo.MongoDb; // <-- aquí sí se necesita el using

// namespace RabbitDemo.Api.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class MessagesController : ControllerBase
//     {
//         private readonly MongoDbService _mongoDbService;

//         public MessagesController()
//         {
//             _mongoDbService = new MongoDbService("mongodb://localhost:27017", "RabbitDemoDb");
//         }

//         [HttpPost]
//         public IActionResult AddMessage([FromBody] string content)
//         {
//             _mongoDbService.AddMessage(content);
//             return Ok();
//         }

//         [HttpGet]
//         public IActionResult GetMessages()
//         {
//             var messages = _mongoDbService.GetAllMessages();
//             return Ok(messages);
//         }
//     }
// }

























// using Microsoft.AspNetCore.Mvc;
// using RabbitDemo.MongoDb;
// using RabbitDemo.Sqlite; // <-- necesitamos SQLite también

// namespace RabbitDemo.Api.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class MessagesController : ControllerBase
//     {
//         private readonly MongoDbService _mongoDbService;
//         private readonly SqliteMessageRepository _sqliteRepo;

//         public MessagesController()
//         {
//             _mongoDbService = new MongoDbService("mongodb://localhost:27017", "RabbitDemoDb");
            
//             // Ajusta la ruta a tu archivo SQLite messages.db según tu árbol de carpetas:
//             _sqliteRepo = new SqliteMessageRepository(
//                 @"C:\Proyectos\TECHUniversidad\RabbitDemo\Consumer\bin\Debug\net9.0\SqliteDb\messages.db"
//             );
//         }

//         // POST para agregar mensaje a MongoDB y SQLite
//         [HttpPost]
//         public IActionResult AddMessage([FromBody] string content)
//         {
//             try
//             {
//                 _mongoDbService.AddMessage(content);
//                 _sqliteRepo.AddMessage(content); // agrega también a SQLite
//                 return Ok();
//             }
//             catch (Exception ex)
//             {
//                 return BadRequest(new { error = ex.Message });
//             }
//         }

//         // GET todos los mensajes de MongoDB
//         [HttpGet("mongo")]
//         public IActionResult GetMongoMessages()
//         {
//             try
//             {
//                 var messages = _mongoDbService.GetAllMessages();
//                 return Ok(messages);
//             }
//             catch (Exception ex)
//             {
//                 return BadRequest(new { error = ex.Message });
//             }
//         }

//         // GET cantidad de mensajes en SQLite
//         [HttpGet("sqlite/count")]
//         public IActionResult GetSqliteCount()
//         {
//             try
//             {
//                 var count = _sqliteRepo.GetMessageCount();
//                 return Ok(new { count });
//             }
//             catch (Exception ex)
//             {
//                 return BadRequest(new { error = ex.Message });
//             }
//         }

//         // GET cantidad de mensajes en MongoDB
//         [HttpGet("mongo/count")]
//         public IActionResult GetMongoCount()
//         {
//             try
//             {
//                 var count = _mongoDbService.GetAllMessages().Count;
//                 return Ok(new { count });
//             }
//             catch (Exception ex)
//             {
//                 return BadRequest(new { error = ex.Message });
//             }
//         }
//     }
// }





















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
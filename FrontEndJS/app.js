// ===============================================
// Dashboard RabbitDemo – JavaScript Definitivo
// ===============================================

// URL base de la API
const API_BASE = "http://localhost:5049/api/messages";

// ===============================================
// Cargar contadores desde la API
// ===============================================
async function loadCounts() {
    try {
        const response = await fetch(`${API_BASE}/count`);
        if (!response.ok) throw new Error("Respuesta no OK");

        const data = await response.json();

        // Debe coincidir EXACTAMENTE con la respuesta de tu API
        document.getElementById("sqliteCount").textContent = data.sqlite;
        document.getElementById("mongoCount").textContent = data.mongoDB;

    } catch (err) {
        console.error("Error obteniendo contadores:", err);
        document.getElementById("sqliteCount").textContent = "Error";
        document.getElementById("mongoCount").textContent = "Error";
    }
}

// ===============================================
// Enviar mensaje a la API (Producer)
// ===============================================
async function sendMessage() {
    const input = document.getElementById("testMessage");
    const result = document.getElementById("sendResult");
    const content = input.value.trim();

    if (!content) {
        result.textContent = "El mensaje no puede estar vacío.";
        return;
    }

    try {
        const response = await fetch(API_BASE, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(content)
        });

        if (!response.ok) throw new Error("Error al enviar mensaje");

        result.textContent = "Mensaje enviado correctamente.";
        input.value = "";

        loadCounts(); // refrescar contadores

    } catch (err) {
        console.error("Error enviando mensaje:", err);
        result.textContent = "Error al enviar.";
    }
}

// Llamar al iniciar la página
window.onload = loadCounts;






















// // const apiMessages = "http://localhost:5000/api/messages";
// const apiMessages = "http://localhost:5049/api/messages";


// const sqliteCountEl = document.getElementById("sqlite-count");
// const mongoCountEl = document.getElementById("mongo-count");
// const messageInput = document.getElementById("message-input");
// const sendBtn = document.getElementById("send-btn");

// async function updateCounts() {
//     try {
//         const res = await fetch(apiMessages);
//         const messages = await res.json();

//         // Ambos contadores usan el mismo array, porque cada mensaje se guarda en Mongo y SQLite
//         sqliteCountEl.textContent = messages.length;
//         mongoCountEl.textContent = messages.length;
//     } catch (err) {
//         console.error("Error obteniendo mensajes:", err);
//         sqliteCountEl.textContent = "Error";
//         mongoCountEl.textContent = "Error";
//     }
// }

// async function sendMessage() {
//     const message = messageInput.value.trim();
//     if (!message) return alert("Ingresa un mensaje.");

//     try {
//         const res = await fetch(apiMessages, {
//             method: "POST",
//             headers: { "Content-Type": "application/json" },
//             body: JSON.stringify(message)
//         });

//         if (res.ok) {
//             messageInput.value = "";
//             updateCounts();
//         } else {
//             alert("Error enviando mensaje");
//         }
//     } catch (err) {
//         console.error("Error enviando mensaje:", err);
//     }
// }

// sendBtn.addEventListener("click", sendMessage);
// messageInput.addEventListener("keypress", e => {
//     if (e.key === "Enter") sendMessage();
// });

// // Actualizar conteos al cargar la página
// updateCounts();

























// const apiMessages = "http://localhost:5000/api/messages";

// const sqliteCountEl = document.getElementById("sqlite-count");
// const mongoCountEl = document.getElementById("mongo-count");
// const messageInput = document.getElementById("message-input");
// const sendBtn = document.getElementById("send-btn");

// async function updateCounts() {
//     try {
//         const res = await fetch(apiMessages);
//         const messages = await res.json();

//         // Ambos contadores usan el mismo array, porque cada mensaje se guarda en Mongo y SQLite
//         sqliteCountEl.textContent = messages.length;
//         mongoCountEl.textContent = messages.length;
//     } catch (err) {
//         console.error("Error obteniendo mensajes:", err);
//         sqliteCountEl.textContent = "Error";
//         mongoCountEl.textContent = "Error";
//     }
// }

// async function sendMessage() {
//     const message = messageInput.value.trim();
//     if (!message) return alert("Ingresa un mensaje.");

//     try {
//         const res = await fetch(apiMessages, {
//             method: "POST",
//             headers: { "Content-Type": "application/json" },
//             body: JSON.stringify(message)
//         });

//         if (res.ok) {
//             messageInput.value = "";
//             updateCounts();
//         } else {
//             alert("Error enviando mensaje");
//         }
//     } catch (err) {
//         console.error("Error enviando mensaje:", err);
//     }
// }

// sendBtn.addEventListener("click", sendMessage);
// messageInput.addEventListener("keypress", e => {
//     if (e.key === "Enter") sendMessage();
// });

// // Actualizar conteos al cargar la página
// updateCounts();





















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
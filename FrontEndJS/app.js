// URLs de tu back-end (ajusta según tu API real)
const apiSQLiteCount = "http://localhost:5000/api/sqlite/count";
const apiMongoCount = "http://localhost:5000/api/mongo/count";
const apiSendMessage = "http://localhost:5000/api/messages/send";

// Función para actualizar conteos
async function updateCounts() {
    try {
        const sqliteRes = await fetch(apiSQLiteCount);
        const mongoRes = await fetch(apiMongoCount);

        const sqliteCount = await sqliteRes.json();
        const mongoCount = await mongoRes.json();

        document.getElementById("sqlite-count").textContent = sqliteCount;
        document.getElementById("mongo-count").textContent = mongoCount;
    } catch (err) {
        console.error("Error obteniendo conteos:", err);
    }
}

// Función para enviar mensaje a RabbitMQ
async function sendMessage() {
    const message = document.getElementById("message-input").value;
    if (!message) return alert("Ingresa un mensaje.");

    try {
        const res = await fetch(apiSendMessage, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ message })
        });
        if (res.ok) {
            alert("Mensaje enviado correctamente.");
            document.getElementById("message-input").value = "";
            updateCounts(); // actualizar conteos tras enviar
        } else {
            alert("Error enviando mensaje.");
        }
    } catch (err) {
        console.error("Error enviando mensaje:", err);
    }
}

// Eventos
document.getElementById("send-btn").addEventListener("click", sendMessage);

// Actualizar conteos al cargar
updateCounts();
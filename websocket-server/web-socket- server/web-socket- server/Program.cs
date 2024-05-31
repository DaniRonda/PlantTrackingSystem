using System;
using System.Collections.Generic;
using System.Text.Json;
using Fleck;
using Npgsql;

namespace web_socket__server
{
    class Program
    {
        static List<IWebSocketConnection> allSockets = new List<IWebSocketConnection>();
        static Message lastReceivedData;

        static void Main(string[] args)
        {
            var server = new WebSocketServer("ws://0.0.0.0:8080");

            server.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    Console.WriteLine("Conexión WebSocket abierta");
                    allSockets.Add(socket);
                };

                socket.OnClose = () =>
                {
                    Console.WriteLine("Conexión WebSocket cerrada");
                    allSockets.Remove(socket);
                };

                socket.OnMessage = message =>
                {
                    Console.WriteLine("Mensaje recibido: " + message);
                    try
                    {
                        // Procesar el mensaje recibido
                        var messageObject = JsonSerializer.Deserialize<Message>(message);
                        Console.WriteLine($"Datos recibidos -> ID Record: {messageObject.idRecord}, ID Plant: {messageObject.idPlant}, DateTime: {messageObject.dateTime}, Temperature: {messageObject.temperature}, Humidity: {messageObject.humidity}");

                        // Guardar los datos recibidos
                        lastReceivedData = messageObject;

                        // Enviar datos a todos los clientes conectados
                        SendMessageToClients(messageObject);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al procesar el mensaje: {ex.Message}");
                    }
                };
            });

            // Inicializar el temporizador para guardar los datos cada hora
            var saveDataTimer = new System.Timers.Timer();
            saveDataTimer.Elapsed += SaveDataTimerElapsed;
            saveDataTimer.Interval = 3600000; // 1 hora (en milisegundos)
            saveDataTimer.Start();

            Console.WriteLine("Servidor WebSocket ejecutándose en ws://0.0.0.0:8080");
            Console.ReadLine(); // Mantener el servidor en ejecución
        }

        static void SaveDataTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (lastReceivedData != null)
            {
                try
                {
                    // Guardar los últimos datos recibidos en la base de datos
                    SaveDataToDatabase(lastReceivedData);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al guardar los datos en la base de datos: {ex}");
                }
            }
        }

        static void SendMessageToClients(Message message)
        {
            // Obtener el tiempo y la hora actual
            var currentTime = DateTime.Now;

            // Crear un objeto de respuesta que incluya el mensaje, temperatura, humedad, tiempo y hora, y el id de la planta
            var response = JsonSerializer.Serialize(new
            {
                message = "Datos recibidos correctamente",
                idPlant = message.idPlant,
                temperature = message.temperature,
                humidity = message.humidity,
                timeReceived = currentTime.ToString("yyyy-MM-dd HH:mm:ss")
            });

            // Enviar la respuesta a todos los clientes conectados
            foreach (var socket in allSockets)
            {
                socket.Send(response);
            }
        }


        static void SaveDataToDatabase(Message data)
        {
            // Conexión con la base de datos
            using (var conn = new NpgsqlConnection("Host=abul.db.elephantsql.com;Port=5432;Username=tivogyll;Password=D_aMUJJ9FWKwv0clIEsj4hoJzhuCf10E;Database=tivogyll"))
            {
                conn.Open();

                // Guardar los datos en la base de datos
                using (var cmd = new NpgsqlCommand("INSERT INTO data_records (id_plant, date_time, temperature, humidity) VALUES (@idPlant, @dateTime, @temperature, @humidity)", conn))
                {
                    cmd.Parameters.AddWithValue("idPlant", data.idPlant); // ID de la planta
                    cmd.Parameters.AddWithValue("dateTime",DateTime.Parse (data.dateTime));
                    cmd.Parameters.AddWithValue("temperature", data.temperature);
                    cmd.Parameters.AddWithValue("humidity", data.humidity);
                    cmd.ExecuteNonQuery();
                }

                Console.WriteLine("Datos guardados en la base de datos");
            }
        }
    }

    public class Message
    {
        public int idRecord { get; set; }
        public int idPlant { get; set; }
        public string dateTime { get; set; }
        public float temperature { get; set; }
        public float humidity { get; set; }
    }
}

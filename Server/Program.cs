using Common.Constants;
using Common.Models;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Server
{
    public class Program
    {
        /// <summary>
        /// Local endpoint for the server
        /// </summary>
        public static IPEndPoint ServerEndPoint { get; } = new IPEndPoint(IPAddress.Parse("26.137.156.223"), 6400);

        /// <summary>
        /// The data instance for handling contacts
        /// </summary>
        public static Data Data { get; set; } = new Data();

        static void Main(string[] args)
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            socket.Bind(ServerEndPoint);
            socket.Listen(10);

            var socketConnection = socket.Accept();
            Console.WriteLine("Conexión aceptada");

            Data.Seed();
            
            while (true)
            {
                byte[] incomingBuffer = new byte[1000];
                var incomingBytes = socketConnection.Receive(incomingBuffer, 0, incomingBuffer.Length, 0);
                Array.Resize(ref incomingBuffer, incomingBytes);
                var receivedData = Encoding.Default.GetString(incomingBuffer);
                Console.WriteLine($"La info recibida es {receivedData}");
                
                var response = HandleRequest(receivedData);
                Console.WriteLine($"La info a enviar es {response}");
                byte[] outgoingBuffer = Encoding.Default.GetBytes(response);
                socketConnection.Send(outgoingBuffer);
            }
        }

        public static string HandleRequest(string receivedData)
        {
            /*
            Receives a string with the following format:
            "COMMAND DATA" or "COMMAND"
            */
            var sections = receivedData.Split(' ', 2);
            var command = sections[0];
            var data = sections.Length > 1 ? sections[1] : string.Empty;
          
            switch (command)
            {
                case Command.GET:
                    return Data.GetContacts();
                case Command.POST:
                    var contact = JsonConvert.DeserializeObject<Contact>(data);
                    Console.WriteLine($"Nombre: {contact.Name}; Teléfono: {contact.PhoneNumber}");
                    return Data.AddContact(contact);
                case Command.DELETE:
                    var id = int.Parse(data);
                    return Data.DeleteContact(id);
                default:
                    return Response.Error;
            }
        }
    }
}
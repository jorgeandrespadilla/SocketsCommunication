using Common.Models;
using System.Net;
using System.Net.Sockets;

namespace Client
{
    public class Program
    {
        /// <summary>
        /// Endpoint for the server
        /// </summary>
        public static IPEndPoint ServerEndPoint { get; } = new IPEndPoint(IPAddress.Parse("26.137.156.223"), 6400);

        static void Main(string[] args)
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(ServerEndPoint);

            var connectionHandler = new ConnectionHandler(socket);
            var data = new Data(connectionHandler);

            while (true)
            {
                Console.WriteLine("1. Ver Contactos");
                Console.WriteLine("2. Agregar Contacto");
                Console.WriteLine("3. Eliminar Contacto");
                Console.Write("Ingrese una opción: ");
                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        data.GetContacts();
                        break;
                    case "2":
                        Console.WriteLine("Ingrese el nombre: ");
                        var name = ReadText();
                        Console.WriteLine("Ingrese el número: ");
                        var number = ReadText();
                        data.AddContact(new Contact
                        {
                            Name = name,
                            PhoneNumber = number,
                        });
                        break;
                    case "3":
                        Console.Write("Ingrese el ID: ");
                        var id = ReadText();
                        data.DeleteContact(id);
                        break;
                    case "4":
                        return;
                }
            }
        }

        public static string ReadText() 
        {
            var input = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Ingrese un valor válido");
                input = Console.ReadLine();
            }
            return input;
        }
    }
}
using System.Net.Sockets;
using System.Text;

namespace Client
{
    public class ConnectionHandler
    {
        public Socket _socket { get; set; }

        public ConnectionHandler(Socket socket)
        {
            _socket = socket;
        }

        public void Send(string message)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(message);
            _socket.Send(buffer);
        }

        public void Send(string command, string data)
        {
            Send(command + " " + data);
        }

        public string Receive()
        {
            byte[] incomingBuffer = new byte[1000];
            var incomingBytes = _socket.Receive(incomingBuffer, 0, incomingBuffer.Length, 0);
            Array.Resize(ref incomingBuffer, incomingBytes);
            var receivedData = Encoding.Default.GetString(incomingBuffer);
            return receivedData;
        }
    }
}

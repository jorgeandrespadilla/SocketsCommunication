using Common.Constants;
using Common.Models;
using Newtonsoft.Json;

namespace Client
{
    public class Data
    {
        private ConnectionHandler _connectionHandler { get; }

        public Data(ConnectionHandler connectionHandler)
        {
           _connectionHandler = connectionHandler;
        }

        public void GetContacts()
        {
            _connectionHandler.Send(Command.GET);
            var response = _connectionHandler.Receive();
            var contacts = JsonConvert.DeserializeObject<IList<Contact>>(response);
            foreach (Contact contact in contacts)
            {
                Console.WriteLine($"[{contact.Id}] Nombre: {contact.Name}; Teléfono: {contact.PhoneNumber}");
            }
        }
        public void AddContact(Contact contact)
        {
            _connectionHandler.Send(Command.POST, JsonConvert.SerializeObject(contact));
            var response = _connectionHandler.Receive();
            if (response == Response.Success)
            {
                Console.WriteLine("Contacto agregado");
            }
            else
            {
                Console.WriteLine("Error al agregar contacto");
            }
        }
        public void DeleteContact(string id)
        {
            _connectionHandler.Send(Command.DELETE, id.ToString());
            var response = _connectionHandler.Receive();
            if (response == Response.Success)
            {
                Console.WriteLine("Contacto eliminado");
            }
            else
            {
                Console.WriteLine("Error al eliminar contacto");
            }
        }
    }
}

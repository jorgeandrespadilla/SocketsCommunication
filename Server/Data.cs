using Common.Constants;
using Common.Models;
using Newtonsoft.Json;

namespace Server
{
    public class Data
    {
        public IList<Contact> Contacts { get; set; }
        public int CurrentId { get; set; }
        public Data()
        {
            Contacts = new List<Contact>();
            CurrentId = 1;
        }
        public string GetContacts()
        {
            var json = JsonConvert.SerializeObject(Contacts);
            return json;
        }
        public string AddContact(Contact contact)
        {
            contact.Id = CurrentId;
            Contacts.Add(contact);
            CurrentId++;
            return Response.Success;
        }
        public string DeleteContact(int id)
        {
            for (int i = 0; i < Contacts.Count; i++)
            {
                if (Contacts[i].Id == id)
                {
                    Contacts.RemoveAt(i);
                    return Response.Success;
                }
            }
            return Response.Error;
        }
    }
}

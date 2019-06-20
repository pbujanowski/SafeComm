using SafeComm.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SafeComm.Shared.Services
{
    public class ContactDataStore : IDataStore<Contact>
    {
        private readonly List<Contact> contacts;

        public ContactDataStore()
        {
            contacts = new List<Contact>();
            var mockContacts = new List<Contact>
            {
                new Contact{Id = 1, Name = "Jan Kowalski", Address = IPAddress.Parse("127.0.0.1"), Port = 8080 }
            };
            //var mockContacts = new List<Contact>
            //{
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "First item", Description="This is an item description." },
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "Second item", Description="This is an item description." },
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "Third item", Description="This is an item description." },
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "Fourth item", Description="This is an item description." },
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "Fifth item", Description="This is an item description." },
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "Sixth item", Description="This is an item description." },
            //};

            foreach (var contact in mockContacts)
            {
                contacts.Add(contact);
            }
        }

        public async Task<bool> AddItemAsync(Contact item)
        {
            contacts.Add(item);

            return await Task.FromResult(true).ConfigureAwait(false);
        }

        public async Task<bool> UpdateItemAsync(Contact item)
        {
            var oldContact = contacts.Find((Contact arg) => arg.Id == item.Id);
            contacts.Remove(oldContact);
            contacts.Add(item);

            return await Task.FromResult(true).ConfigureAwait(false);
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            var oldContact = contacts.Find((Contact arg) => arg.Id == id);
            contacts.Remove(oldContact);

            return await Task.FromResult(true).ConfigureAwait(false);
        }

        public async Task<Contact> GetItemAsync(int id)
        {
            return await Task.FromResult(contacts.Find(s => s.Id == id)).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Contact>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(contacts).ConfigureAwait(false);
        }
    }
}
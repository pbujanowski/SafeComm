using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using SafeComm.Shared.Views;
using SafeComm.Core.Models;

namespace SafeComm.Shared.ViewModels
{
    public class ContactsViewModel : BaseViewModel
    {
        public ObservableCollection<Contact> Contacts { get; set; }
        public Command LoadContactsCommand { get; set; }

        public ContactsViewModel()
        {
            Title = "Contacts";
            Contacts = new ObservableCollection<Contact>();
            LoadContactsCommand = new Command(async () => await ExecuteLoadContactsCommand().ConfigureAwait(false));

            MessagingCenter.Subscribe<NewContactPage, Contact>(this, "AddContact", async (obj, contact) =>
            {
                var newContact = contact;
                Contacts.Add(newContact);
                await DataStore.AddItemAsync(newContact).ConfigureAwait(false);
            });
        }

        private async Task ExecuteLoadContactsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Contacts.Clear();
                var contacts = await DataStore.GetItemsAsync(true).ConfigureAwait(false);
                foreach (var contact in contacts)
                {
                    Contacts.Add(contact);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
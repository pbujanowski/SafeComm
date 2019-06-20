using System;
using SafeComm.Shared.Models;

namespace SafeComm.Shared.ViewModels
{
    public class ContactDetailViewModel : BaseViewModel
    {
        public Contact Contact { get; set; }

        public ContactDetailViewModel(Contact contact = null)
        {
            Title = contact?.Name;
            Contact = contact;
        }
    }
}

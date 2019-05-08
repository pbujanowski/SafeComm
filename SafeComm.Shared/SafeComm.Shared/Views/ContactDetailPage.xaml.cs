using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SafeComm.Shared.ViewModels;
using SafeComm.Core.Models;
using System.Net;

namespace SafeComm.Shared.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactDetailPage : ContentPage
    {
        ContactDetailViewModel viewModel;

        public ContactDetailPage(ContactDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        public ContactDetailPage()
        {
            InitializeComponent();

            var contact = new Contact
            {
                Name = "Jan Kowalski",
                Address = IPAddress.Parse("127.0.0.1")
            };

            viewModel = new ContactDetailViewModel(contact);
            BindingContext = viewModel;
        }
    }
}
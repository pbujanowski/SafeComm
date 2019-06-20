using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using SafeComm.Shared.Views;
using SafeComm.Shared.ViewModels;
using SafeComm.Shared.Models;

namespace SafeComm.Shared.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactsPage : ContentPage
    {
        private readonly ContactsViewModel viewModel;

        public ContactsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new ContactsViewModel();
        }

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Contact;
            if (item == null)
                return;

            await Navigation.PushAsync(new ContactDetailPage(new ContactDetailViewModel(item))).ConfigureAwait(false);

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        private async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewContactPage())).ConfigureAwait(false);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Contacts.Count == 0)
                viewModel.LoadContactsCommand.Execute(null);
        }
    }
}
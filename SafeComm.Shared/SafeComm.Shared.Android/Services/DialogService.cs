using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SafeComm.Shared.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(SafeComm.Shared.Droid.Services.DialogService))]
namespace SafeComm.Shared.Droid.Services
{
    public class DialogService : IDialogService
    {
        public void ShowMessage(string title, string message)
        {
            var dialog = new AlertDialog.Builder(Android.App.Application.Context);
            dialog.SetTitle(title);
            dialog.SetMessage(message);
            dialog.SetCancelable(true);
            dialog.Show();
        }
    }
}
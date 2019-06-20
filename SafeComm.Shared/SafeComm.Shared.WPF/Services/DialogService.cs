using SafeComm.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Xamarin.Forms;

[assembly: Dependency(typeof(SafeComm.Shared.WPF.Services.DialogService))]
namespace SafeComm.Shared.WPF.Services
{
    public class DialogService : IDialogService
    {
        public void ShowMessage(string title, string message)
        {
            MessageBox.Show(message, title);
        }
    }
}

using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SafeComm.Shared.Views;
using SafeComm.Shared.Connection;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace SafeComm.Shared
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            DependencyService.Register<IConnectionClient, ConnectionClient>();
            DependencyService.Register<IConnectionServer, ConnectionServer>();

            MainPage = new TestPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}

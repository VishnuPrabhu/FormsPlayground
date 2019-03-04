using FormsPlayground.Features.Home.Views;
using FormsPlayground.Framework.InversionOfControl;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace FormsPlayground.Core
{
    public partial class App
    {
        public App()
        {
            InitializeComponent();
            
            Ioc.Bootstrapper.Bootstrap("Core", CoreBootstrap.Run);

            MainPage = new HomeTabbedPage();
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

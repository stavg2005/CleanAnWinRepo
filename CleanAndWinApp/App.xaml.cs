using CleanAndWinApp.Pages;

namespace CleanAndWinApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
            Routing.RegisterRoute(nameof(Maps),typeof(Maps));
        }
    }
}
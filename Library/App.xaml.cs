using Library.ViewModels;
using Microsoft.Maui.Controls.Handlers.Items;

namespace Library
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
        protected override void OnStart()
        {
            base.OnStart();
        }
    }
}
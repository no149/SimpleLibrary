using Library.Pages;

namespace Library
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(BookEditPage),typeof(BookEditPage));
        }
    }
}
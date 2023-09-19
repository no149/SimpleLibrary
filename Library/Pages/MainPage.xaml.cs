using Library.ViewModels;

namespace Library.Pages
{
    public partial class MainPage : ContentPage
    {
        private MainViewModel _vm;

        public MainPage(MainViewModel mainViewModel)
        {
            InitializeComponent();
            BindingContext=mainViewModel;
            _vm = mainViewModel;
        }

        private void ContentPage_Loaded(object sender, EventArgs e)
        {
            _vm.LoadBooks();
        }
    }
}
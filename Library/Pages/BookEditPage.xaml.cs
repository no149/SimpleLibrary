using Library.ViewModels;

namespace Library.Pages;

public partial class BookEditPage : ContentPage
{
	public BookEditPage(BookViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
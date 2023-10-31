using Library.ViewModels;

namespace Library.Pages;

public partial class BookEditPage : ContentPage
{
	public BookEditPage(BookPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
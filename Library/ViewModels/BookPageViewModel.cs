using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
namespace Library.ViewModels{
public partial class BookPageViewModel
{
    private Services.FinderService _finder;

    public BookPageViewModel(Services.FinderService finder,BookViewModel
      bookViewModel)
    {
        _finder= finder;
        BookVm= bookViewModel;
    }

    public BookViewModel BookVm {get;set;} 
       [RelayCommand]
        async Task ReadBarcode()
        {
            var books= await _finder.FindByIsbn(BookVm.Barcode);
            if(books.Length==0)
            return;
BookVm.Title= books[0].Title;
BookVm.Author= books[0].Author;
BookVm.Publisher= books[0].Publisher;
BookVm.Translator= books[0].Translator;
BookVm.Description= books[0].Subject;


        }
}
}
namespace Library.ViewModels{
class BookPageViewModel
{
    private Services.FinderService _finder;

    private BookViewModel _bookVm;
     BookPageViewModel(Services.FinderService finder,BookViewModel
      bookViewModel)
    {
        _finder= finder;
        _bookVm= bookViewModel;
    }

    public BookViewModel BookVm {get;set;} =>_bookVm;
}
}
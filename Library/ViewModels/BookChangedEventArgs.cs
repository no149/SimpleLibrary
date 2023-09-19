namespace Library.ViewModels
{
    public class BookChangedEventArgs:EventArgs
    {
        public BookViewModel Book;
        public bool IsNew;
    }
}
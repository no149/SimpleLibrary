namespace Library.ViewModels
{
    public enum ChangeType{
        Added,
        Changed,
        Deleted
    }
    public class BookChangedEventArgs:EventArgs
    {
        public BookViewModel Book;
        public ChangeType ChangeType;
    }
}
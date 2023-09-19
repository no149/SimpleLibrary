using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Library.Data;
using Library.Data.Models;
using Library.Models;
using Microsoft.UI.Xaml.Controls.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.ViewModels
{
    [QueryProperty("Id", "Id")]
    [QueryProperty("Title", "Title")]
    [QueryProperty("Description", "Description")]
    [QueryProperty("Price", "Price")]
    [QueryProperty("Publisher", "Publisher")]
    [QueryProperty("Author", "Author")]
    [QueryProperty("Translator", "Translator")]
    [QueryProperty("Language", "Language")]
    public partial class BookViewModel : ObservableObject
    {
        public int Id { get; set; }
        [ObservableProperty]
        private string _title;
        [ObservableProperty]
        private string _description;
        [ObservableProperty]
        private decimal _price;
        [ObservableProperty]
        private string _publisher;
        [ObservableProperty]
        private string _author;
        [ObservableProperty]
        private string _translator;
        [ObservableProperty]
        private string _language;
        [ObservableProperty]
        private string _coverImage;

        public BookViewModel()
        {

        }
        public BookViewModel(Book book)
        {
            Author = book.Author;
            Publisher = book.Publisher;
            Translator = book.Translator;
            Language = book.Language;
            Title = book.Title;
            Description = book.Description;
            Price = book.Price;
            Id = book.Id;
        }

        public bool IsNew
        {
            get
            {
                return Id == 0;
            }
        }

        [RelayCommand]
        private async Task Save()
        {
            using (var dbcontext = new LibraryDbContext())
            {
                Book book = null;
                if (Id == 0)
                {
                    book = new Book();
                    dbcontext.Books.Add(book);
                }
                else
                    book = dbcontext.Books.Single(book => book.Id == Id);

                book.Description = Description;
                book.Price = Price;
                book.Publisher = Publisher;
                book.Author = Author;
                book.Translator = Translator;
                book.Language = Language;
                book.Title = Title;

                dbcontext.SaveChanges();
                OnBookChanged?.Invoke(this, new BookChangedEventArgs() { Book = new BookViewModel(book), IsNew = Id == 0 });
                await Shell.Current.GoToAsync("..");
            }
        }


        [RelayCommand]
        async Task GoBack()
        {
        }

        internal void Copy(BookViewModel selectedItem)
        {
            Title = selectedItem.Title;
            Description = selectedItem.Description;
            Price = selectedItem.Price;
            Publisher = selectedItem.Publisher;
            Author = selectedItem.Author;
            Translator = selectedItem.Translator;
            Language = selectedItem.Language;
            Id = selectedItem.Id;

        }

        internal void Reset()
        {

            Title = "";
            Description = "";
            Price = 0;
            Publisher = "";
            Author = "";
            Translator = "";
            Language = "";
            Id = 0;
        }

        public event EventHandler<BookChangedEventArgs> OnBookChanged;
    }
}

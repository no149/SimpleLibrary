using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Library.Data;
using Library.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<BookViewModel> _books;
        [ObservableProperty]
        private string _editButtonText = "کتاب جدید";
        [ObservableProperty]
        private BookViewModel _selectedItem;
        private BookViewModel _bookVm;
        [ObservableProperty]
        private bool _isBookSelected = false;

        private void OnBookChanged(object sender, BookChangedEventArgs e)
        {
            if (e.ChangeType == ChangeType.Added)
            {
                Books.Add(e.Book);
            }
            else if (e.ChangeType == ChangeType.Changed)
            {
                var book = Books.Single(b => b.Id == e.Book.Id);
                book.Copy(e.Book);
            }
            else
            {
                Books.Remove(e.Book);
            }
        }

        [CommunityToolkit.Mvvm.Input.RelayCommand]
        async Task AddOrEditBook()
        {
            if (SelectedItem == null)
            {
                _bookVm.Reset();
                await Shell.Current.GoToAsync(nameof(BookEditPage));
            }
            else
            {
                _bookVm.Copy(SelectedItem);
                await Shell.Current.GoToAsync(nameof(BookEditPage));
            }
        }

        internal void LoadBooks()
        {
            var dbContext = new LibraryDbContext();
            using (dbContext)
            {
                var books = dbContext.Books;
                foreach (var book in books)
                {
                    Books.Add(new BookViewModel(book));
                }
            }
        }
        public MainViewModel(BookViewModel bookViewModel)
        {
            _bookVm = bookViewModel;
            Books = new ObservableCollection<BookViewModel>();
            PropertyChanged += HandlePropertyChanged;
            _bookVm.OnBookChanged += OnBookChanged;

        }

        private void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (SelectedItem == null)
                EditButtonText = "کتاب جدید";
            else
                EditButtonText = "ویرایش";
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            IsBookSelected = SelectedItem != null;
        }
        [RelayCommand]
        async Task DeleteBook(int bookId)
        {
            using (var dbcontext = new LibraryDbContext())
            {
                var book = dbcontext.Books.Single(book => book.Id == bookId);

                dbcontext.Remove(book);
                dbcontext.SaveChanges();
                var bookVm = Books.Single(b => b.Id == bookId);
                Books.Remove(bookVm);
            }
        }
    }
}

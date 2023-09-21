using CommunityToolkit.Mvvm.ComponentModel;
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
        private string _editButtonText = "Add";
        [ObservableProperty]
        private BookViewModel _selectedItem;
        private BookViewModel _bookVm;
        [ObservableProperty]
        private bool _isBookSelected = false;

        private void BookViewModel_OnBookChanged(object sender, BookChangedEventArgs e)
        {
            if (e.IsNew)
            {
                Books.Add(e.Book);
            }
            else
            {
                var book = Books.Single(b => b.Id == e.Book.Id);
                book.Copy(e.Book);
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
            bookViewModel.OnBookChanged += BookViewModel_OnBookChanged;
            PropertyChanged += HandlePropertyChanged;

        }

        private void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (SelectedItem == null)
                EditButtonText = "Add";
            else
                EditButtonText = "Edit";
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            IsBookSelected = SelectedItem != null;
        }
    }
}

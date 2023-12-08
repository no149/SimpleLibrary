using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Library.Data;
using Library.Pages;
using Library.Services;
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

        [ObservableProperty]
        private int _currentPage = 1;
        [ObservableProperty]
        private int _totalPages = 1;
        private int _pageSize = 10;
        [ObservableProperty]
        private bool _canNavigatePreviousPage = false;
        [ObservableProperty]
        private bool _canNavigateNextPage = false;

        [ObservableProperty]
        private bool _isSearchMode = false;
        [ObservableProperty]
        private string _searchText;
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

        internal void Init()
        {
            LoadBooks();
        }

        internal void LoadBooks()
        {
            var dbContext = new LibraryDbContext();
            Books.Clear();
            using (dbContext)
            {
                var books = dbContext.Books.AsQueryable();
                if (!string.IsNullOrEmpty(SearchText))
                    books = books.Where(b => b.Title.Contains(SearchText));

                books = books.OrderBy(b=>b.Title).Skip((CurrentPage - 1) * _pageSize).Take(_pageSize);
                foreach (var book in books)
                {
                    Books.Add(new BookViewModel(book));
                }
            }
            UpdatePaging();
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
        async Task DeleteBook()
        {
            using (var dbcontext = new LibraryDbContext())
            {
                var book = dbcontext.Books.Single(book => book.Id ==SelectedItem.Id);

                dbcontext.Remove(book);
                dbcontext.SaveChanges();
                var bookVm = Books.Single(b => b.Id == SelectedItem.Id);
                Books.Remove(bookVm);
            }
        }

        [RelayCommand]
        void NextPage()
        {
            CurrentPage++;
            LoadBooks();
        }

        [RelayCommand]
        void Search(string text)
        {
            LoadBooks();
        }

        [RelayCommand]
void SelectedBookChanged()
{
    foreach(var book in Books)
    {
        if(book.Id==SelectedItem.Id)
        {
            book.IsSelected=true;
        }
        else 
            book.IsSelected=false;
    }
    
}
        [RelayCommand]
        void ToggleSearch()
        {
            //todo
            IsSearchMode = !IsSearchMode;
            if (!IsSearchMode)
            {
                SearchText = "";
                CurrentPage=1;
                LoadBooks();
            }
        }
        [RelayCommand]
        void PreviousPage()
        {
            CurrentPage--;
            LoadBooks();

        }
        internal void UpdatePaging()
        {
            CanNavigatePreviousPage = CurrentPage >= 2;
            var dbContext = new LibraryDbContext();
            using (dbContext)
            {
                var totalpages = 0M;
                if (!string.IsNullOrEmpty(SearchText))
                    totalpages = Math.Ceiling((decimal)dbContext.Books.Where(b => b.Title.Contains(SearchText)).Count() / _pageSize);
                else
                    totalpages = Math.Ceiling((decimal)dbContext.Books.Count() / _pageSize);

                totalpages= totalpages==0?1:totalpages;
                CanNavigateNextPage = CurrentPage < totalpages;
                TotalPages = (int)totalpages;
            }


        }


    }
}

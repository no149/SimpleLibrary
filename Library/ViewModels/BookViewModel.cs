using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Library.Data;
using Library.Data.Models;
using Library.Models;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Platform;
using System;
using System.Collections.Generic;
using System.IO;
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
        private string _barcode;

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
        
        private ImageSource _coverImage;

         [ObservableProperty]
        private bool _isSelected;
         [ObservableProperty]
        private bool _hasCover;
         [ObservableProperty]
        private bool _hasNoCover;


        private string _selectedCoverImagePath;
        
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


            if (book.CoverImage != null)
            {
                Microsoft.Maui.Graphics.IImage image;
#if ANDROID
                // PlatformImage isn't currently supported on Windows.
                using (var memStream = new MemoryStream(book.CoverImage))
                     image = Microsoft.Maui.Graphics.Platform.PlatformImage.FromStream(memStream);
#elif WINDOWS
                 image = new Microsoft.Maui.Graphics.Win2D.W2DImageLoadingService().FromBytes(book.CoverImage);
#endif
                var extension = Path.GetExtension(_selectedCoverImagePath);
                var format = ImageFormat.Gif;
                switch (extension)
                {
                    case "jpg":
                        format = ImageFormat.Jpeg;
                        break;
                    case "png":
                        format = ImageFormat.Png;
                        break;

                }
                CoverImage = ImageSource.FromStream(x => Task.FromResult(image.AsStream(format)));
            }

        }

public ImageSource CoverImage
{
    get =>_coverImage;
    set {
        SetProperty(ref _coverImage,value);
        HasCover= value!=null;
        HasNoCover=!HasCover;
    }
}
public  ImageSource CoverImagePlaceHolder => "book_cover_placeholder.jpg";
        public bool IsNew
        {
            get
            {
                return Id == 0;
            }
        }


 [RelayCommand]
        private async Task SaveAndNew()
        {
            await SaveBook();
            Reset();
        }
        [RelayCommand]
        private async Task Save()
        {
          await SaveBook();
            //TODO: this method throws error because of dotnet bug
                await Shell.Current.GoToAsync("..");

        }

private async Task SaveBook()
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


                if (string.IsNullOrEmpty(_selectedCoverImagePath) == false)
                {
                    var imageStream = new FileStream(_selectedCoverImagePath, FileMode.Open);
                    using (imageStream)
                    {
                        var arr = new byte[imageStream.Length];
                        await imageStream.ReadAsync(arr, 0, arr.Length);
                        book.CoverImage = arr;
                    }
                }
                dbcontext.SaveChanges();
                OnBookChanged?.Invoke(this, new BookChangedEventArgs()
                {
                    Book = new BookViewModel(book),
                    ChangeType = Id == 0 ? ChangeType.Added : ChangeType.Changed
                });
            }
}

        
        [RelayCommand]
        async Task SelectImage()
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                FileResult photo = await MediaPicker.Default.PickPhotoAsync();
                if (photo != null)
                {
                    _selectedCoverImagePath = photo.FullPath;
                    CoverImage = StreamImageSource.FromStream(x => photo.OpenReadAsync());
                }

            }
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
            CoverImage = selectedItem.CoverImage;
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
            CoverImage=null;  
           _selectedCoverImagePath=null;
        }

        public event EventHandler<BookChangedEventArgs> OnBookChanged;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.Models
{
    public class Book
    {
        public bool IsNew
        {
            get
            {
                return Id == 0;
            }
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string Author { get; set; }
        public string? Translator { get; set; }
        public string Publisher { get; set; }
        public byte[]? CoverImage { get; set; }
        public string Language { get; set; }
        public decimal Price { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Data.Models;

namespace Library.Data
{
    public class DbInitializer
    {
        private readonly ModelBuilder _modelBuilder;

        public DbInitializer(ModelBuilder modelBuilder)
        {
            this._modelBuilder = modelBuilder;
        }

        public void Seed()
        {
            //_modelBuilder.Entity<Book>().HasData(
            //       new Book
            //       {
            //           Title = "book 1",
            //           Author = "author 1",
            //           Description = "desc 1",
            //           Translator = "translator 1",
            //           Publisher = "publisher 1",
            //           Price = 10.10M,
            //           Language = "eng",
            //           Id = 1,
            //           CoverImage = null

            //       }


            //);
        }
    }
}

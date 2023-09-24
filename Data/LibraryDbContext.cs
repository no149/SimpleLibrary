using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Library.Data.Models;

namespace Library.Data;

public partial class LibraryDbContext : DbContext
{
    public LibraryDbContext()
    {
    }

    public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
    {
        var path =System.AppContext.BaseDirectory;
        System.Console.WriteLine("path="+path);
        optionsBuilder.UseSqlite($"Data Source={path}\\libraryDB.sqlite3;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);

    }



    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    public DbSet<Book> Books { get; set; }
}

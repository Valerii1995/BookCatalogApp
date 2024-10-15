using BooksClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;

namespace BookCatalogApp.BooksClassLibrary
{
    public class BookMap : ClassMap<Book>
    {
        public BookMap()
        {
            Map(m => m.Title).Name("Title");
            Map(m => m.Pages).Name("Pages");
            Map(m => m.ReleaseDateString).Name("ReleaseDate");
            Map(m => m.Genre).Ignore();
            Map(m => m.Author).Ignore();
            Map(m => m.Publisher).Ignore();
        }
    }
}

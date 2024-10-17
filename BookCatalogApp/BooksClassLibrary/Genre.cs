using BookCatalogApp;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksClassLibrary
{
    public class Genre
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Genre() { }
        public Genre (string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }
    }
}

using BookCatalogApp;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksClassLibrary
{
    public class Publisher
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Publisher(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }
    }
}

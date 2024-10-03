using BookCatalogApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksClassLibrary
{
    public class Author
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Author() { }
        public Author(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }
    }
}

using BooksClassLibrary;
using Microsoft.EntityFrameworkCore;
using BookCatalogApp.BooksClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCatalogApp
{
    public class BookSearcher
    {
        private readonly BookCatalogContext _context;

        public BookSearcher(BookCatalogContext context)
        {
            _context = context;
        }

        public List<Book> SearchBooks(Filter filter)
        {
            var query = _context.Books.Include(b => b.Genre)
                                      .Include(b => b.Author)
                                      .Include(b => b.Publisher)
                                      .AsQueryable();

            if (!string.IsNullOrEmpty(filter.Title))
                query = query.Where(b => b.Title.Contains(filter.Title));

            if (!string.IsNullOrEmpty(filter.Genre))
                query = query.Where(b => b.Genre.Name == filter.Genre);

            if (!string.IsNullOrEmpty(filter.Author))
                query = query.Where(b => b.Author.Name == filter.Author);

            if (!string.IsNullOrEmpty(filter.Publisher))
                query = query.Where(b => b.Publisher.Name == filter.Publisher);

            if (filter.MoreThanPages.HasValue)
                query = query.Where(b => b.Pages > filter.MoreThanPages.Value);

            if (filter.LessThanPages.HasValue)
                query = query.Where(b => b.Pages < filter.LessThanPages.Value);

            if (filter.PublishedBefore.HasValue)
                query = query.Where(b => b.ReleaseDate < filter.PublishedBefore.Value);

            if (filter.PublishedAfter.HasValue)
                query = query.Where(b => b.ReleaseDate > filter.PublishedAfter.Value);

            return query.ToList();
        }

        public void SaveSearchResultsAsCsv(List<Book> books)
        {
            string directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\BookFile");

            string fullPath = Path.Combine(directoryPath, $"Results_{DateTime.Now:yyyyMMdd_HHmmss}.csv");

            using (var writer = new StreamWriter(fullPath))
            {
                writer.WriteLine("Title,Author,Genre,Publisher,Pages,ReleaseDate");

                foreach (var book in books)
                {
                    writer.WriteLine($"{book.Title},{book.Author?.Name},{book.Genre?.Name},{book.Publisher?.Name},{book.Pages},{book.ReleaseDate:yyyy-MM-dd}");
                }
            }
        }
    }
}

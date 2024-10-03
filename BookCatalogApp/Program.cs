using BookCatalogApp;
using BookCatalogApp.BooksClassLibrary;
using BooksClassLibrary;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;
using static System.Reflection.Metadata.BlobBuilder;

internal class Program
{
    private static void Main(string[] args)
    {
        try
        {
            string file = @"..\..\BookFile\books.csv";
            string filePath = @"..\..\BookFile\filter.json";
            Filter filter = Filter.LoadFilterSettings(filePath);

            using (BookCatalogContext context = new BookCatalogContext())
            {
                List<Book> books = BookLoader.LoadBooksFromCsv(file, context);

                BookSearcher bookSearcher = new BookSearcher(context);
                List<Book> filteredBooks = bookSearcher.SearchBooks(filter);

                Console.WriteLine($"Books found: {filteredBooks.Count}");
                Console.WriteLine("List of books:");
                foreach (var book in filteredBooks)
                {
                    Console.WriteLine($"- {book.Title}");
                }

                bookSearcher.SaveSearchResultsAsCsv(filteredBooks);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
using BookCatalogApp;
using BookCatalogApp.BooksClassLibrary;
using BooksClassLibrary;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;
using static System.Reflection.Metadata.BlobBuilder;
using BookCatalogApp.Resource;

internal class Program
{
    private static void Main(string[] args)
    {
        try
        {
            Console.Write(Resource.EnterFileAdres);
            string file = Console.ReadLine();
            Console.Write(Resource.EnterFilterAdres);
            string filePath = Console.ReadLine();

            if (file == "" )
            {
                file = @"..\..\BookFile\books.csv";
            }
            if (filePath == "" ) 
            {
                filePath = @"..\..\BookFile\filter.json";
            }
            Filter filter = Filter.LoadFilterSettings(filePath);


            using (BookCatalogContext context = new BookCatalogContext())
            {
                List<Book> books = BookLoader.LoadBooksFromCsv(file, context);

                BookSearcher bookSearcher = new BookSearcher(context);
                List<Book> filteredBooks = bookSearcher.SearchBooks(filter);

                Console.WriteLine($"{Resource.BooksFound}{filteredBooks.Count}");
                Console.WriteLine(Resource.ListOfBooks);
                foreach (var book in filteredBooks)
                {
                    Console.WriteLine($"- {book.Title}");
                }

                bookSearcher.SaveSearchResultsAsCsv(filteredBooks);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{Resource.Error}{ex.Message}");
        }
    }
}
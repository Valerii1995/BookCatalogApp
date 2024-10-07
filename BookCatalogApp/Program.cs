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
                file = @"..\..\BookFile\books.txt";
            }
            if (filePath == "" ) 
            {
                filePath = @"..\..\BookFile\filter.json";
            }


            Filter filter = Filter.LoadFilterSettings(filePath);
            string fileExtension = Path.GetExtension(file);

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

                bookSearcher.SaveSearchResultsAsCsv(filteredBooks, fileExtension);
            }
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine($"{Resource.Error} File not found. Details: {ex.Message}");
            Environment.Exit(1);
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"{Resource.Error} Access denied. Details: {ex.Message}");
            Environment.Exit(1);
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"{Resource.Error} Invalid JSON format in filter file. Details: {ex.Message}");
            Environment.Exit(1);
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"{Resource.Error} Database update failed. Details: {ex.Message}");
            Environment.Exit(1);
        }
        catch (IOException ex)
        {
            Console.WriteLine($"{Resource.Error} I/O error. Details: {ex.Message}");
            Environment.Exit(1);
        }
        catch (FormatException ex)
        {
            Console.WriteLine($"{Resource.Error} Invalid data format. Details: {ex.Message}");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"{Resource.Error}{ex.Message}");
        }
    }
}
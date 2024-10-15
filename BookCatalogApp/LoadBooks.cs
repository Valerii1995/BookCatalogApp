using BookCatalogApp.BooksClassLibrary;
using BooksClassLibrary;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCatalogApp
{
    public class BookLoader
    {
        public static List<Book> LoadBooksFromCsv(string file, BookCatalogContext context)
        {
            List<Book> books = new List<Book>();

            using (StreamReader reader = new StreamReader(file))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<BookMap>();

                var records = csv.GetRecords<Book>().ToList();

                for (int i = 1; i < records.Count; i++)
                {
                    Book record = records[i];

                    if (string.IsNullOrWhiteSpace(record.Genre?.Name) ||
                         string.IsNullOrWhiteSpace(record.Author?.Name) ||
                         string.IsNullOrWhiteSpace(record.Publisher?.Name) ||
                         string.IsNullOrWhiteSpace(record.Title))
                    {
                        continue;
                    }

                    DateTime releaseDate = ParseDateOrDefault(record.ReleaseDateString, i);

                    Genre genre = context.Genres.SingleOrDefault(g => g.Name == record.Genre.Name)
                        ?? new Genre(record.Genre.Name);
                    if (genre.Id == Guid.Empty)
                        context.Genres.Add(genre);

                    Author author = context.Authors.SingleOrDefault(a => a.Name == record.Author.Name)
                        ?? new Author(record.Author.Name);
                    if (author.Id == Guid.Empty)
                        context.Authors.Add(author);

                    Publisher publisher = context.Publishers.SingleOrDefault(p => p.Name == record.Publisher.Name)
                        ?? new Publisher(record.Publisher.Name);
                    if (publisher.Id == Guid.Empty)
                        context.Publishers.Add(publisher);

                    Book? existingBook = context.Books
                        .SingleOrDefault(b => b.Title.Trim() == record.Title.Trim() && b.AuthorId == author.Id);

                    if (existingBook == null)
                    {
                        Book book = new Book
                        (
                            record.Title,
                            record.Pages,
                            genre.Id,
                            author.Id,
                            publisher.Id,
                            releaseDate
                        );

                        context.Books.Add(book);
                        books.Add(book);
                    }

                    context.SaveChanges();
                }
            }

            return books;
        }

        static DateTime ParseDateOrDefault(string dateString, int lineNumber)
        {
            if (DateTime.TryParse(dateString, out DateTime parsedDate))
            {
                return parsedDate;
            }
            Console.WriteLine($"Некорректная дата в строке {lineNumber}: {dateString}. Используем значение по умолчанию: DateTime.MinValue.");
            return DateTime.MinValue;
        }
    }
}
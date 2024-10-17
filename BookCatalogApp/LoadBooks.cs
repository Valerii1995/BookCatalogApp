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
            EntityAdder entityAdder = new EntityAdder(context);

            using (StreamReader reader = new StreamReader(file))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<BookMap>();

                List<Book> records = csv.GetRecords<Book>().ToList();

                for (int i = 1; i < records.Count; i++)
                {
                    Book record = records[i];

                    Genre genre = entityAdder.GetOrAddGenre(record.Genre.Name);
                    Author author = entityAdder.GetOrAddAuthor(record.Author.Name);
                    Publisher publisher = entityAdder.GetOrAddPublisher(record.Publisher.Name);

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
                            record.ReleaseDate
                        );

                        context.Books.Add(book);
                        books.Add(book);
                    }

                    context.SaveChanges();
                }
            }

            return books;
        }
    }
}
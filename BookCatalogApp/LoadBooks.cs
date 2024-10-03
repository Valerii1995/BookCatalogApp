using BooksClassLibrary;
using System;
using System.Collections.Generic;
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
            List<Genre> genres = new List<Genre>();
            List<Author> authors = new List<Author>();
            List<Publisher> publishers = new List<Publisher>();

            using (StreamReader reader = new StreamReader(file))
            {
                string line;
                bool isFirstLine = true;
                int lineNumber = 0;

                while ((line = reader.ReadLine()) != null)
                {
                    lineNumber++;

                    if (isFirstLine)
                    {
                        isFirstLine = false;
                        continue;
                    }

                    string[] values = line.Split(',');

                    string genreName = values[2];
                    string authorName = values[3];
                    string publisherName = values[4];

                    Genre genre = genres.Find(g => g.Name == genreName) ?? new Genre(genreName);
                    if (!genres.Contains(genre)) genres.Add(genre);

                    Author author = authors.Find(a => a.Name == authorName) ?? new Author(authorName);
                    if (!authors.Contains(author)) authors.Add(author);

                    Publisher publisher = publishers.Find(p => p.Name == publisherName) ?? new Publisher(publisherName);
                    if (!publishers.Contains(publisher)) publishers.Add(publisher);

                    var existingBook = context.Books
                        .SingleOrDefault(b => b.Title == values[0] && b.AuthorId == author.Id);

                    DateTime publisherTime = IsValidDateTime(values[3], lineNumber);

                    if (existingBook == null)
                    {
                        Book book = new Book
                        (
                            values[0],
                            int.Parse(values[1]),
                            genre.Id,
                            author.Id,
                            publisher.Id,
                            publisherTime
                        );

                        context.Books.Add(book);
                    }
                }
            }

            context.SaveChanges();
            return books;

            static DateTime IsValidDateTime(string dateString, int lineNumber)
            {
                if (DateTime.TryParse(dateString, out _))
                {
                    return DateTime.Parse(dateString);
                }
                else
                {
                    Console.WriteLine($"In line {lineNumber} Incorrect time format: {dateString}");
                    return DateTime.Parse("1,1,1");
                }
            }
        }
    }
}

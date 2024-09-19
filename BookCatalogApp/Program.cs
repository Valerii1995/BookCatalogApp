using BooksClassLibrary;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

internal class Program
{
    private static void Main(string[] args)
    {
        List<BooksClassLibrary.Book> books = new List<BooksClassLibrary.Book>();
        List<BooksClassLibrary.Genre> genres = new List<BooksClassLibrary.Genre>();
        List<BooksClassLibrary.Author> authors = new List<BooksClassLibrary.Author>();
        List<BooksClassLibrary.Publisher> publishers = new List<BooksClassLibrary.Publisher>();

        FileInfo file = new FileInfo("C:\\Users\\vasin\\source\\repos\\bookcatalog\\BookFIle\\books.csv");

        using (StreamReader reader = new StreamReader(file.FullName))
        {
            string line;
            bool isFirstLine = true;

            while ((line = reader.ReadLine()) != null)
            {
                if (isFirstLine)
                {
                    isFirstLine = false;
                    continue;
                }

                string[] values = line.Split(',');

                string genreName = values[2];
                string authorName = values[4];
                string publisherName = values[5];

                Genre genre = genres.Find(g => g.Name == genreName);
                if (genre == null)
                {
                    genre = new Genre { Id = Guid.NewGuid(), Name = genreName };
                    genres.Add(genre);
                }

                Author author = authors.Find(a => a.Name == authorName);
                {
                    author = new Author { Id = Guid.NewGuid(), Name = authorName };
                    authors.Add(author);
                }

                Publisher publisher = publishers.Find(p => p.Name == publisherName);
                {
                    publisher = new Publisher { Id = Guid.NewGuid(), Name = publisherName };
                    publishers.Add(publisher);
                }

                Book book = new Book
                    (
                    values[0],
                    int.Parse(values[1]),
                    genre.Id,
                    author.Id,
                    publisher.Id,
                    DateTime.Parse(values[3])
                );

            }
        }
    }
}



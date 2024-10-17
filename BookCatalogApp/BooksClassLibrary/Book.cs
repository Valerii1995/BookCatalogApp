using System.ComponentModel.DataAnnotations.Schema;

namespace BooksClassLibrary
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int Pages { get; set; }
        public Guid GenreId { get; set; }
        public Guid AuthorId { get; set; }
        public Guid PublisherId { get; set; }
        public DateTime ReleaseDate { get; set; }

        public Genre Genre { get; set; }
        public Author Author { get; set; }
        public Publisher Publisher { get; set; }

        public Book() { }
        public Book(string title, int pages, Guid genreId, Guid authorId, Guid publisherId, DateTime releaseDate)
        {
            Id = Guid.NewGuid();
            Title = title;
            Pages = pages;
            GenreId = genreId;
            AuthorId = authorId;
            PublisherId = publisherId;
            ReleaseDate = releaseDate;
        }
    }
}

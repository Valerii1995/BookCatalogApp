using BookCatalogApp;
using BookCatalogApp.BooksClassLibrary;
using BooksClassLibrary;
using System;
using System.Linq;

namespace BookCatalogApp;
public class EntityAdder
{
    private readonly BookCatalogContext _context;

    public EntityAdder(BookCatalogContext context)
    {
        _context = context;
    }

    public Genre GetOrAddGenre(string genreName)
    {
        Genre genre = _context.Genres.SingleOrDefault(g => g.Name == genreName)
            ?? new Genre(genreName);

        if (genre.Id == Guid.Empty)
            _context.Genres.Add(genre);

        return genre;
    }

    public Author GetOrAddAuthor(string authorName)
    {
        Author author = _context.Authors.SingleOrDefault(a => a.Name == authorName)
            ?? new Author(authorName);

        if (author.Id == Guid.Empty)
            _context.Authors.Add(author);

        return author;
    }

    public Publisher GetOrAddPublisher(string publisherName)
    {
        Publisher publisher = _context.Publishers.SingleOrDefault(p => p.Name == publisherName)
            ?? new Publisher(publisherName);

        if (publisher.Id == Guid.Empty)
            _context.Publishers.Add(publisher);

        return publisher;
    }
}
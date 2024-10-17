using BooksClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace BookCatalogApp.BooksClassLibrary
{
    public class BookMap : ClassMap<Book>
    {
        public BookMap()
        {
            Map(m => m.Title).Name("Title");
            Map(m => m.Pages).Name("Pages");
            Map(m => m.Genre).Name("Genre");
            Map(m => m.ReleaseDate).TypeConverter<DateTimeConverter>();
            Map(m => m.Author).Name("Author");
            Map(m => m.Publisher).Name("Publisher");
        }

        public class DateTimeConverter : DefaultTypeConverter
        {
            public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
            {
                if (DateTime.TryParse(text, out var date))
                {
                    return date;
                }
                Console.WriteLine($"Error: Incorrect time format: {text}");
                return DateTime.MinValue; 
            }
        }
    }
}

namespace BookShop.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using BookShop.Data.Models.Enums;
    using BookShop.DataProcessor.ExportDto;
    using Data;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportMostCraziestAuthors(BookShopContext context)
        {  
            var crazyAuthors = context.Authors
                .Select(x => new
                {
                    AuthorName = x.FirstName + " " + x.LastName,
                    Books = x.AuthorsBooks
                         .OrderByDescending(b => b.Book.Price)
                         .Select(y => new
                         {
                             BookName = y.Book.Name,
                             BookPrice = y.Book.Price.ToString("F2")
                         })
                         .ToList()
                })
                .ToList()
                .OrderByDescending(a => a.Books.Count)
                .ThenBy(x => x.AuthorName)
                .ToList();
            return JsonConvert.SerializeObject(crazyAuthors, Formatting.Indented);
        }

        public static string ExportOldestBooks(BookShopContext context, DateTime date)
        {
            var oldestBooks = context.Books
                .Where(x => x.PublishedOn < date && x.Genre == Genre.Science)
                .OrderByDescending(x => x.Pages)
                .ThenByDescending(x => x.PublishedOn)
                .Select(x => new BookXmlExportModel
                {
                    Pages = x.Pages,
                    Name = x.Name,
                    Date = x.PublishedOn.ToString("d", CultureInfo.InvariantCulture)
                })
                .Take(10)
                .ToList();


            const string rootElement = "Books";
            var serializer = new XmlSerializer(typeof(List<BookXmlExportModel>), new XmlRootAttribute(rootElement));
            var textWriter = new StringWriter();
            using (textWriter)
            {
                serializer.Serialize(textWriter, oldestBooks, GetXmlNamespaces());
            }
            return textWriter.ToString();
        }

        private static XmlSerializerNamespaces GetXmlNamespaces()
        {
            XmlSerializerNamespaces xmlNamespaces = new XmlSerializerNamespaces();
            xmlNamespaces.Add(string.Empty, string.Empty);
            return xmlNamespaces;
        }
    }
}
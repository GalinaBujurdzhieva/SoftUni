namespace BookShop.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using BookShop.Data.Models;
    using BookShop.Data.Models.Enums;
    using BookShop.DataProcessor.ImportDto;
    using Data;
    using Newtonsoft.Json;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedBook
            = "Successfully imported book {0} for {1:F2}.";

        private const string SuccessfullyImportedAuthor
            = "Successfully imported author - {0} with {1} books.";

        public static string ImportBooks(BookShopContext context, string xmlString)
        {
            const string rootElement = "Books";
            var serializer = new XmlSerializer(typeof(List<BookXmlImportModel>), new XmlRootAttribute(rootElement));
            var textReader = new StringReader(xmlString);
            var booksDTO = serializer.Deserialize(textReader) as List<BookXmlImportModel>;

            StringBuilder sb = new StringBuilder();
            List<Book> validBooks = new List<Book>();

            foreach (var bookDTO in booksDTO)
            {
                if (!IsValid(bookDTO))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                DateTime publishedOn;
                var isDateValid = DateTime.TryParseExact(bookDTO.PublishedOn, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out publishedOn);
                if (!isDateValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var currentBook = new Book
                {
                    Name = bookDTO.Name,
                    Genre = (Genre)bookDTO.Genre,
                    Price = bookDTO.Price,
                    Pages = bookDTO.Pages,
                    PublishedOn = publishedOn
                };
                validBooks.Add(currentBook);
                sb.AppendLine(string.Format(SuccessfullyImportedBook, bookDTO.Name, bookDTO.Price));
            }
            context.Books.AddRange(validBooks);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }
        
        public static string ImportAuthors(BookShopContext context, string jsonString)
        {
            var authorsWithBooksDTO = JsonConvert.DeserializeObject<IEnumerable<AuthorJsonImportModel>>(jsonString);
            
            StringBuilder sb = new StringBuilder();
            List<Author> validAuthors = new List<Author>();

            foreach (var authorWithBookDTO in authorsWithBooksDTO)
            {
                bool authorsEmailExists = validAuthors.Any(x => x.Email == authorWithBookDTO.Email);
                
                if (!IsValid(authorWithBookDTO) || authorsEmailExists)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var currentAuthor = new Author
                {
                    FirstName = authorWithBookDTO.FirstName,
                    LastName = authorWithBookDTO.LastName,
                    Phone = authorWithBookDTO.Phone,
                    Email = authorWithBookDTO.Email,
                };

                foreach (var authorBook in authorWithBookDTO.Books)
                {
                    var currentBook = context.Books.FirstOrDefault(x => x.Id == authorBook.Id);
                    if (authorBook.Id == null || currentBook == null)
                    {
                        continue;
                    }
                    currentAuthor.AuthorsBooks.Add(new AuthorBook
                    {
                        Author = currentAuthor,
                        Book = currentBook
                    });
                }

                if (currentAuthor.AuthorsBooks.Count == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                validAuthors.Add(currentAuthor);
                sb.AppendLine(string.Format(SuccessfullyImportedAuthor, currentAuthor.FirstName + " " + currentAuthor.LastName, currentAuthor.AuthorsBooks.Count));
            }
            context.Authors.AddRange(validAuthors);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
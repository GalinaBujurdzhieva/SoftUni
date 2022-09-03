namespace BookShop
{
    using BookShop.Models;
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Z.EntityFramework.Plus;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);

            //02. string input = Console.ReadLine();
            //02. Console.WriteLine(GetBooksByAgeRestriction(db, input));

            Console.WriteLine(GetGoldenBooks(db));

            //04. Console.WriteLine(GetBooksByPrice(db));

            //05. int year = int.Parse(Console.ReadLine());
            //05. Console.WriteLine(GetBooksNotReleasedIn(db, year)); 

            //06. string categories = Console.ReadLine();
            //06. Console.WriteLine(GetBooksByCategory(db, categories));

            //07. string date = Console.ReadLine();
            //07. Console.WriteLine(GetBooksReleasedBefore(db, date));

            //08. string input = Console.ReadLine();
            //08. Console.WriteLine(GetAuthorNamesEndingIn(db, input));

            //09. string input = Console.ReadLine();
            //09. Console.WriteLine(GetBookTitlesContaining(db, input));

            //10. string input = Console.ReadLine();
            //10. Console.WriteLine(GetBooksByAuthor(db, input));

            //11. int titleLength = int.Parse(Console.ReadLine());
            //11. Console.WriteLine(CountBooks(db, titleLength));

            //12. Console.WriteLine(CountCopiesByAuthor(db));

            //13. Console.WriteLine(GetTotalProfitByCategory(db));

            //14. Console.WriteLine(GetMostRecentBooks(db));

            IncreasePrices(db);

            //16. Console.WriteLine(RemoveBooks(db));

        } 

        //02.
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var ageRestriction = Enum.Parse<AgeRestriction>(command, true);
            var books = context.Books
                .Where(x => x.AgeRestriction == ageRestriction)
                .Select(x => x.Title)
                .OrderBy(x => x)
                .ToList();
            
            var result = string.Join(Environment.NewLine, books);
            return result;
        }
        //03.
        public static string GetGoldenBooks(BookShopContext context)
        {
            //string query = @"SELECT* FROM Books
            //                 WHERE Copies < 5000 and EditionType = 2
            //                 ORDER BY BookId";
            //var books = context.Books.FromSqlRaw(query).Select(x => x.Title).ToList();

            var goldenBooks = context.Books
                .Where(x => x.EditionType == EditionType.Gold && x.Copies < 5000)
                .OrderBy(x => x.BookId)
                .Select(x => x.Title)
                .ToList();

            var result = string.Join(Environment.NewLine, goldenBooks);
            return result;
        }
        //04.
        public static string GetBooksByPrice(BookShopContext context)
        {
            var books = context.Books
                .Where(x => x.Price > 40)
                .OrderByDescending(x => x.Price)
                .Select(x => new
                {
                    Title = x.Title,
                    Price = x.Price
                })
                .ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - ${book.Price:F2}");
            }
            return sb.ToString().TrimEnd();
        }
        //05.
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var books = context.Books
                .Where(x => x.ReleaseDate.HasValue && x.ReleaseDate.Value.Year != year)
                .OrderBy(x => x.BookId)
                .Select(x => x.Title)
                .ToList();

            var result = string.Join(Environment.NewLine, books);
            return result;
        }
        //06.
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            string[] categories = input.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToLower()).ToArray();

            var books = context.BooksCategories
                .Where(x => categories.Contains(x.Category.Name.ToLower()))
                .OrderBy(x => x.Book.Title)
                .Select(x => x.Book.Title)
                .ToList();

            var result = string.Join(Environment.NewLine, books);
            return result;
        }
        //07.
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var dt = DateTime.ParseExact(date, "dd-MM-yyyy",
                                  CultureInfo.InvariantCulture);
            var books = context.Books
                .ToList()
                .Where(x => x.ReleaseDate.Value < dt)
                .OrderByDescending(x => x.ReleaseDate.Value)
                .Select(x => new
                {
                    x.Title,
                    x.EditionType,
                    x.Price
                });
            
            StringBuilder sb = new StringBuilder();
            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price:F2}");
            }
            return sb.ToString().TrimEnd();
        }
        //08.
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context.Authors
                .Where(x => x.FirstName.EndsWith(input))
                .Select(x => new
                {
                    FullName = x.FirstName + " " + x.LastName
                })
                .OrderBy(x => x.FullName)
                .ToList();

            var result = string.Join(Environment.NewLine, authors.Select(x => x.FullName));
            return result;
        }
        //09.
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var books = context.Books
                .ToList()
                .Where(x => x.Title.Contains(input, StringComparison.OrdinalIgnoreCase))
                .OrderBy(x => x.Title)
                .Select(x => x.Title);

            var result = string.Join(Environment.NewLine, books);
            return result;
        }
        //10.
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var booksAndAuthors = context.Books
                .Include(x => x.Author)
                .ToList()
                .Where(x => x.Author.LastName.StartsWith(input, StringComparison.OrdinalIgnoreCase))
                .OrderBy(x => x.BookId)
                .Select(x => new
                {
                    BookTitle = x.Title,
                    Author = x.Author.FirstName + " " + x.Author.LastName
                });

            StringBuilder sb = new StringBuilder();
            foreach (var bookAndAuthor in booksAndAuthors)
            {
                sb.AppendLine($"{bookAndAuthor.BookTitle} ({bookAndAuthor.Author})");
            }
            return sb.ToString().TrimEnd();
        }
        //11.
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var books = context.Books
                .Where(x => x.Title.Length > lengthCheck)
                .ToList();
            return books.Count();
        }
        //12.
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var booksCopies = context.Authors
                .Select(x => new
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Count = x.Books.Sum(y => y.Copies)
                })
               .OrderByDescending(x => x.Count)
               .ToList();

            var result = string.Join(Environment.NewLine, booksCopies.Select(x => $"{x.FirstName} {x.LastName} - {x.Count}"));
            return result;
        }
        //13.
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var categoryProfit = context.Categories
                .Select(x => new
                {
                    CategoryName = x.Name,
                    TotalProfit = x.CategoryBooks.Sum(y => y.Book.Copies * y.Book.Price)
                })
                .OrderByDescending(x => x.TotalProfit)
                .ThenBy(x => x.CategoryName)
                .ToList();

            var result = string.Join(Environment.NewLine, categoryProfit.Select(x => $"{x.CategoryName} ${x.TotalProfit:F2}"));
            return result;
        }
        //14.
        public static string GetMostRecentBooks(BookShopContext context)
        {
            var recentBooksCategories = context.Categories
                .OrderBy(x => x.Name)
                .Select(x => new
                {
                    CategoryName = x.Name,
                    Books = x.CategoryBooks.
                            Select(y => new
                            {
                                BookName = y.Book.Title,
                                ReleaseDate = y.Book.ReleaseDate.Value,
                                ReleaseYear = y.Book.ReleaseDate.Value.Year
                            })
                            .OrderByDescending(b => b.ReleaseDate)
                            .Take(3)
                            .ToList()
                })
                .ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var category in recentBooksCategories)
            {
                sb.AppendLine($"--{category.CategoryName}");
                foreach (var book in category.Books)
                {
                    sb.AppendLine($"{book.BookName} ({book.ReleaseYear})");
                }
            }
            return sb.ToString().TrimEnd();
        }
        //15.
        public static void IncreasePrices(BookShopContext context)
        {
            //context.Books.Where(x => x.ReleaseDate.Value.Year < 2010).Update(x => new Book { Price = x.Price + 5 });
            
            var booksToBeUpdated = context.Books
                .Where(x => x.ReleaseDate.Value.Year < 2010);

            foreach (var book in booksToBeUpdated)
            {
                book.Price += 5;
            }
            context.BulkUpdate(booksToBeUpdated);
        }
        //16.
        public static int RemoveBooks(BookShopContext context)
        {
            int count = context.Books
                .Where(x => x.Copies < 4200)
                .Delete();
            return count;
        }
    }
}

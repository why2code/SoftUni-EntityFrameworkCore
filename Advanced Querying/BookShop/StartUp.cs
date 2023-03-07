using System.Text;

namespace BookShop
{
    using BookShop.Models;
    using Data;
    using System.Globalization;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);

            int booksCount = int.Parse(Console.ReadLine());
            int res = CountBooks(db, booksCount);
            Console.WriteLine(res);

        }

        //02. Age Restriction 
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            StringBuilder sb = new StringBuilder();

            int groupId = 0; //for Minor
            if (command == "teen") //Teen
            {
                groupId = 1;
            }
            else if (command == "adult") //Adult
            {
                groupId = 2;
            }

            var books = context.Books
                .Where(b => (int)b.AgeRestriction == groupId)
                .Select(b => new
                {
                    title = b.Title
                })
                .OrderBy(b => b.title)
                .ToArray();

            foreach (var book in books)
            {
                sb.AppendLine(book.title);
            }

            return sb.ToString().TrimEnd();
        }

        //03. Golden Books 
        public static string GetGoldenBooks(BookShopContext context)
        {
            var books = context.Books
                .Where(b => (int)b.EditionType == 2)
                .Where(b => b.Copies < 5000)
                .OrderBy(b => b.BookId)
                .Select(b => new
                {
                    title = b.Title
                })
                .ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var book in books)
            {
                sb.AppendLine(book.title);
            }

            return sb.ToString().TrimEnd();
        }

        //04. Books by Price
        public static string GetBooksByPrice(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.Price > 40)
                .OrderByDescending(b => b.Price)
                .Select(b => new
                {
                    b.Title,
                    Price = b.Price.ToString("F2")
                })
                .ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - ${book.Price}");
            }
            return sb.ToString().TrimEnd();
        }

        //05. Not Released In 
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var books = context.Books
                .Where(b => ((DateTime)(b.ReleaseDate!)).Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => new
                {
                    b.Title
                })
                .ToArray();

            var sb = new StringBuilder();
            foreach (var book in books)
            {
                sb.AppendLine(book.Title);
            }

            return sb.ToString().TrimEnd();
        }

        //06. Book Titles by Category 
        public static string GetBooksByCategory(BookShopContext dbContext, string input)
        {
            // In-memory, we are still not approaching the DB
            string[] categories = input
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.ToLower())
                .ToArray();

            string[] bookTitles = dbContext.Books
                .Where(b => b.BookCategories
                                .Any(bc => categories.Contains(bc.Category.Name.ToLower())))
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, bookTitles);
        }

        //07. Released Before Date
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            DateTime convertedDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var books = context.Books
                .Where(b => b.ReleaseDate < convertedDate)
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => new
                {
                    b.Title,
                    b.EditionType,
                    Price = b.Price.ToString("F2")
                })
                .ToArray();


            var sb = new StringBuilder();
            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price}");
            }

            return sb.ToString().TrimEnd();
        }

        //08. Author Search
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            int requiredLenghtOfSubstring = input.Length;

            var authors = context.Authors
                .Where(a => 
                    a.FirstName.Substring((a.FirstName.Length - requiredLenghtOfSubstring)) == input)
                .OrderBy(a => a.FirstName + " " + a.LastName)
                .Select(a => new
                {
                    Name = a.FirstName + " " + a.LastName
                })
                .ToArray();

            var sb = new StringBuilder();
            foreach (var author in authors)
            {
                sb.AppendLine(author.Name);
            }

            return sb.ToString().TrimEnd();
        }

        //09. Book Search
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            string[] books = context.Books
                .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, books).TrimEnd();
        }

        //10.Book Search by Author
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var books = context.Books
                .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(b => b.BookId)
                .Select(b => $"{b.Title} ({b.Author.FirstName} {b.Author.LastName})")
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        //11. Count Books 
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var books = context.Books
                .Where(b => b.Title.Length > lengthCheck)
                .ToArray()
                .Count();

            return books; //$"There are {books} books with longer title than {lengthCheck} symbols";
        }


        //12. Total Book Copies
            public static string CountCopiesByAuthor(BookShopContext dbContext)
        {
            StringBuilder sb = new StringBuilder();
            var authorsWithBookCopies = dbContext.Authors
                .Select(a => new
                {
                    FullName = a.FirstName + " " + a.LastName,
                    TotalCopies = a.Books
                        .Sum(b => b.Copies)
                })
                .ToArray()
                .OrderByDescending(b => b.TotalCopies); // This is optimization

            foreach (var a in authorsWithBookCopies)
            {
                sb.AppendLine($"{a.FullName} - {a.TotalCopies}");
            }

            return sb.ToString().TrimEnd();
        }

        // 13. Profit by Category
        public static string GetTotalProfitByCategory(BookShopContext dbContext)
        {
            StringBuilder sb = new StringBuilder();
            var categoriesWithProfit = dbContext.Categories
                .Select(c => new
                {
                    CategoryName = c.Name,
                    TotalProfit = c.CategoryBooks
                        .Sum(cb => cb.Book.Copies * cb.Book.Price)
                })
                .ToArray()
                .OrderByDescending(c => c.TotalProfit)
                .ThenBy(c => c.CategoryName);

            foreach (var c in categoriesWithProfit)
            {
                sb.AppendLine($"{c.CategoryName} ${c.TotalProfit:f2}");
            }

            return sb.ToString().TrimEnd();
        }


        // 14. Most Recent Books
        public static string GetMostRecentBooks(BookShopContext dbContext)
        {
            StringBuilder sb = new StringBuilder();

            var categoriesWithMostRecentBooks = dbContext.Categories
                .OrderBy(c => c.Name)
                .Select(c => new
                {
                    CategoryName = c.Name,
                    MostRecentBooks = c.CategoryBooks
                        .OrderByDescending(cb => cb.Book.ReleaseDate)
                        .Take(3) // This can lower network load
                        .Select(cb => new
                        {
                            BookTitle = cb.Book.Title,
                            ReleaseYear = cb.Book.ReleaseDate.Value.Year
                        })
                        .ToArray()
                })
                .ToArray();

            foreach (var c in categoriesWithMostRecentBooks)
            {
                sb.AppendLine($"--{c.CategoryName}");

                foreach (var b in c.MostRecentBooks/*.Take(3) This is lowering query complexity*/)
                {
                    sb.AppendLine($"{b.BookTitle} ({b.ReleaseYear})");
                }
            }

            return sb.ToString().TrimEnd();
        }


        // 15. Increase Prices
        public static void IncreasePrices(BookShopContext context)
        {
            // Materializing the query does not detach entities from Change Tracker
            Book[] bookReleasedBefore2010 = context
                .Books
                .Where(b => b.ReleaseDate.HasValue &&
                            b.ReleaseDate.Value.Year < 2010)
                .ToArray();

            // Using BatchUpdate from EFCore.Extensions
            //dbContext
            //    .Books
            //    .Where(b => b.ReleaseDate.HasValue &&
            //                b.ReleaseDate.Value.Year < 2010)
            //    .UpdateFromQuery(b => new Book() { Price = b.Price + 5 });

            foreach (var book in bookReleasedBefore2010)
            {
                book.Price += 5;
            }

            // Using SaveChanges() -> 4544ms
            // Using BulkUpdate() -> 3677ms
            context.SaveChanges();
     
   }


        //16. Remove Books
        public static int RemoveBooks(BookShopContext context, int lessThanCopies = 4200)
        {
            var books = context.Books
                .Where(b => b.Copies < lessThanCopies)
                .ToArray();

            var removedBooks = books.Length;

            context.Books.RemoveRange(books);
            context.SaveChanges();

            return removedBooks;
        }
    }
}



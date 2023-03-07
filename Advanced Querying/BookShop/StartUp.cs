using System.Text;

namespace BookShop
{
    using Data;
    using System.Globalization;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);

            string strInput = Console.ReadLine();
            string res = GetBookTitlesContaining(db, strInput);
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
    }
}



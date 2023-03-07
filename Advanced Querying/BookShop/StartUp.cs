using System.Linq.Expressions;
using System.Text;

namespace BookShop
{
    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);

            //string command = Console.ReadLine().ToLower().TrimEnd();
            //string result = GetBooksByAgeRestriction(db, command);
            //Console.WriteLine(result);

            string res = GetGoldenBooks(db);
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

    }
}



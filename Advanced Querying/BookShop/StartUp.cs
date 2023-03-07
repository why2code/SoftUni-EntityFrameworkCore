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

            string command = Console.ReadLine().ToLower();
            string result = GetBooksByAgeRestriction(db, command);
            Console.WriteLine(result);

        }

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
    }
}



using ProductShop.Data;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using ProductShop.Utilities;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            using ProductShopContext context = new ProductShopContext();
            string inputXml = File.ReadAllText("../../../Datasets/users.xml");

            string result = ImportUsers(context, inputXml);
            Console.WriteLine(result);

        }

        // Problem 01
        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            XmlHelper xmlHelper = new XmlHelper();
            ImportUsersDTO[] usersDtos =
                xmlHelper.Deserialize<ImportUsersDTO[]>(inputXml, "Users");


            ICollection<User> validUsers = new HashSet<User>();
            foreach (ImportUsersDTO user in usersDtos)
            {
                if (string.IsNullOrEmpty(user.FirstName)
                    || string.IsNullOrEmpty(user.LastName))
                {
                    continue;
                }

                User currentValidUser = new User()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Age = user.Age
                };

                validUsers.Add(currentValidUser);
            }

            context.Users.AddRange(validUsers);
            context.SaveChanges();

            return $"Successfully imported {validUsers.Count}";
        }




    }
}
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
            string inputXml = File.ReadAllText("../../../Datasets/categories-products.xml");

            string result = ImportCategoryProducts(context, inputXml);
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


        // Problem 02
        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            XmlHelper xmlHelper = new XmlHelper();
            ImportProductsDTO[] productsDtos = xmlHelper.Deserialize<ImportProductsDTO[]>(inputXml, "Products");

            ICollection<Product> validProducts = new HashSet<Product>();

            foreach (ImportProductsDTO productsDto in productsDtos)
            {
                if (string.IsNullOrEmpty(productsDto.Name))
                {
                    continue;
                }

                if (string.IsNullOrEmpty(productsDto.SellerId.ToString()))
                {
                    continue;
                }

                //if (productsDto.BuyerId == null)
                //{
                //    continue;
                //}

                Product currentProduct = new Product()
                {
                    Name = productsDto.Name,
                    Price = productsDto.Price,
                    SellerId = productsDto.SellerId,
                    BuyerId = productsDto.BuyerId
                };
                validProducts.Add(currentProduct);

            }

            context.Products.AddRange(validProducts);
            context.SaveChanges();
            return $"Successfully imported {validProducts.Count}";
        }


        // Problem 03
        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            XmlHelper xmlHelper = new XmlHelper();
            ImportCategoryDTO[] categoryDtos = xmlHelper.Deserialize<ImportCategoryDTO[]>(inputXml, "Categories");

            ICollection<Category> validCategories = new HashSet<Category>();

            foreach (ImportCategoryDTO categoryDto in categoryDtos)
            {
                if (string.IsNullOrEmpty(categoryDto.Name))
                {
                    continue;
                }

                Category validCategory = new Category()
                {
                    Name = categoryDto.Name
                };
                validCategories.Add(validCategory);
            }
            context.Categories.AddRange(validCategories);
            context.SaveChanges();

            return $"Successfully imported {validCategories.Count}";
        }

        // Problem 04
        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            XmlHelper xmlHelper = new XmlHelper();
            ImportCategoryProductDTO[] categoryProductDtos =
                xmlHelper.Deserialize<ImportCategoryProductDTO[]>(inputXml, "CategoryProducts");

            ICollection<CategoryProduct> validCategoryProducts = new HashSet<CategoryProduct>();
            foreach (ImportCategoryProductDTO importCategoryProductDto in categoryProductDtos)
            {
                if (!context.Categories.Select(i => i.Id).Contains(importCategoryProductDto.CategoryId))
                {
                    continue;
                }

                if (!context.Products.Select(i => i.Id).Contains(importCategoryProductDto.ProductId))
                {
                    continue;
                }

                CategoryProduct validCategoryProduct = new CategoryProduct()
                {
                    CategoryId = importCategoryProductDto.CategoryId,
                    ProductId = importCategoryProductDto.ProductId
                };

                validCategoryProducts.Add(validCategoryProduct);
            }

            context.CategoryProducts.AddRange(validCategoryProducts);
            context.SaveChanges();

            return $"Successfully imported {validCategoryProducts.Count}";
        }
    }
}
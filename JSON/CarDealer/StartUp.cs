using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Newtonsoft.Json;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            CarDealerContext context = new CarDealerContext();
            string inputJson =
                File.ReadAllText(@"../../../Datasets/suppliers.json");

            string resultOfImport = ImportSuppliers(context, inputJson);
            Console.WriteLine(resultOfImport);

        }


        // Problem 09
        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            ImportSupplierDto[] supplierDtos =
                JsonConvert.DeserializeObject<ImportSupplierDto[]>(inputJson);

            List<Supplier> validSuppliers = new List<Supplier>();

            foreach (var importSupplierDto in supplierDtos)
            {
                if (string.IsNullOrEmpty(importSupplierDto.Name))
                {
                    continue;
                }

                Supplier currentValidSupplier = new Supplier()
                {
                    Name = importSupplierDto.Name,
                    IsImporter = importSupplierDto.IsImporter
                };

                validSuppliers.Add(currentValidSupplier);
            }

            context.Suppliers.AddRange(validSuppliers);
            context.SaveChanges();

            return $"Successfully imported {validSuppliers.Count()}.";
        }



    }
}
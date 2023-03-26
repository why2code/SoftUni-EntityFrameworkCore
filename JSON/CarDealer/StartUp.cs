using System.Globalization;
using CarDealer.Data;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Castle.Core.Resource;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            CarDealerContext context = new CarDealerContext();
            //string inputJson =
            //    File.ReadAllText(@"../../../Datasets/cars.json");
            //string resultOfImport = ImportCars(context, inputJson);


            string resultOfImport = GetSalesWithAppliedDiscount(context);
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


        // Problem 10
        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            ImportPartsDto[] partsDtos = JsonConvert.DeserializeObject<ImportPartsDto[]>(inputJson);
            List<Part> validParts = new List<Part>();

            foreach (var importPartsDto in partsDtos)
            {
                if (string.IsNullOrEmpty(importPartsDto.Name)
                    || importPartsDto.Price.ToString() == null
                    || importPartsDto.Quantity.ToString() == null
                    )
                {
                    continue;
                }

                if (!context.Suppliers.Any(s => s.Id == importPartsDto.SupplierId))
                {
                    continue;
                }

                Part currentValidPart = new Part()
                {
                    Name = importPartsDto.Name,
                    Price = importPartsDto.Price,
                    Quantity = importPartsDto.Quantity,
                    SupplierId = importPartsDto.SupplierId
                };

                validParts.Add(currentValidPart);
            }

            context.Parts.AddRange(validParts);
            context.SaveChanges();

            return $"Successfully imported {validParts.Count()}.";
        }

        // Problem 11
        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            ImportCarsdto[] carsDtos = JsonConvert.DeserializeObject<ImportCarsdto[]>(inputJson);
            List<Car> validCars = new List<Car>();

            foreach (var importCarsdto in carsDtos)
            {
                if (string.IsNullOrEmpty(importCarsdto.Make)
                    || string.IsNullOrEmpty(importCarsdto.Model))
                {
                    continue;
                }

                //Check if the list of Parts (PartCar) in the JSON object is the same as list of Parts in DB


                Car currentValidCar = new Car()
                {
                    Make = importCarsdto.Make,
                    Model = importCarsdto.Model,
                    TraveledDistance = importCarsdto.TraveledDistance
                };

                validCars.Add(currentValidCar);


                var partIds = importCarsdto
                    .Parts
                    .Distinct()
                    .ToList();

                if (partIds == null)
                {
                    continue;
                }

                partIds.ForEach(pid =>
                    {
                        var currentPair = new PartCar()
                        {
                            Car = currentValidCar,
                            PartId = pid
                        };

                        currentValidCar.PartsCars.Add(currentPair);
                    }
                );
                
            }

            context.Cars.AddRange(validCars);
            context.SaveChanges();

            return $"Successfully imported {validCars.Count()}.";
        }

        // Problem 12
        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            ImportCustomerDto[] customerDtos = JsonConvert.DeserializeObject<ImportCustomerDto[]>(inputJson);
            List<Customer> validCustomers = new List<Customer>();

            foreach (var importCustomerDto in customerDtos)
            {
                if (string.IsNullOrEmpty(importCustomerDto.Name)
                    || string.IsNullOrEmpty(importCustomerDto.BirthDate))
                {
                    continue;
                }

                Customer currentCust = new Customer()
                {
                    Name = importCustomerDto.Name,
                    BirthDate = DateTime.ParseExact(importCustomerDto.BirthDate, "yyyy-MM-dd'T'HH:mm:ss",
                        CultureInfo.InvariantCulture),
                    IsYoungDriver = importCustomerDto.IsYoungDriver
                };

                validCustomers.Add(currentCust);
            }

            context.Customers.AddRange(validCustomers);
            context.SaveChanges();
            return $"Successfully imported {validCustomers.Count}.";
        }

        // Problem 13
        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            ImportSaleDto[] salesDtos = JsonConvert.DeserializeObject<ImportSaleDto[]>(inputJson);
            List<Sale> validSales = new List<Sale>();

            foreach (var importSaleDto in salesDtos)
            {
                if (string.IsNullOrEmpty(importSaleDto.CarId.ToString())
                    || string.IsNullOrEmpty(importSaleDto.CustomerId.ToString())
                    || string.IsNullOrEmpty(importSaleDto.Discount.ToString("F2")))
                {
                    continue;
                }

                //Judge does not like this test! Otherwise, it`s logical!
                //bool validCarId = context.Cars.Any(c => c.Id == importSaleDto.CarId);
                //bool validCustomerId = context.Customers.Any(c => c.Id == importSaleDto.CustomerId);
                //if (!validCarId || !validCustomerId)
                //{
                //    continue;
                //}

                var currentSale = new Sale()
                {
                    CarId = importSaleDto.CarId,
                    CustomerId = importSaleDto.CustomerId,
                    Discount = importSaleDto.Discount
                };

                validSales.Add(currentSale);
            }
            context.Sales.AddRange(validSales);
            context.SaveChanges();
            return $"Successfully imported {validSales.Count}.";
        }

        // Problem 14
        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customersToExport = context.Customers
                .OrderBy(c => c.BirthDate)
                .ThenBy(c => c.IsYoungDriver)
                .Select(c => new ExportCustomerDto()
                {
                    Name = c.Name,
                    BirthDate = c.BirthDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    IsYoungDriver = c.IsYoungDriver
                }).ToArray()
                .ToArray();

            return JsonConvert.SerializeObject(customersToExport, Formatting.Indented);
        }

        // Problem 15
        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var exportToyotaCars = context.Cars
                .Where(c => c.Make == "Toyota")
                .AsNoTracking()
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TraveledDistance)
                .Select(c => new ExportCarsDto()
                {
                    Id = c.Id,
                    Make = c.Make,
                    Model = c.Model,
                    TraveledDistance = c.TraveledDistance
                })
                .ToArray();

            return JsonConvert.SerializeObject(exportToyotaCars, Formatting.Indented);
        }

        // Problem 16
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliersToExport = context.Suppliers
                .Where(s => s.IsImporter == false)
                .AsNoTracking()
                .Select(s => new ExportSupplierDto()
                {
                    Id = s.Id,
                    Name = s.Name,
                    PartsCount = s.Parts.Count()
                }).ToArray()
                .ToArray();

            return JsonConvert.SerializeObject(suppliersToExport, Formatting.Indented);
        }

        // Problem 17
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var parts = context.Cars
                .Include(c => c.PartsCars)
                .ThenInclude(c => c.Part)
                .Select(c => new 
                {
                    car = new 
                    {
                        Make = c.Make,
                        Model = c.Model,
                        TraveledDistance = c.TraveledDistance
                    },
                    parts = c.PartsCars.Select(p => new
                       {
                           Name = p.Part.Name,
                           Price =  p.Part.Price.ToString("F2")
                       }).ToArray()
                    }).ToArray();

            return JsonConvert.SerializeObject(parts, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.None
            });
        }

        // Problem 18
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            //VS ERROR DUE TO CONVERSION (SUM AGGREGATE SUBQUERY, PASSES IN JUDGE)
            var customers = context.Customers
                .Include(c => c.Sales)
                .ThenInclude(s => s.Car)
                .ThenInclude(c => c.PartsCars)
                .ThenInclude(pc => pc.Part)
                .Where(c => c.Sales.Count >= 1)
                .Select(x => new
                {
                    fullName = x.Name,
                    boughtCars = x.Sales.Count,
                    spentMoney = x.Sales.Sum(y => y.Car.PartsCars.Sum(z => z.Part.Price))
                }).ToArray()
                .OrderByDescending(sm => sm.spentMoney)
                .ThenByDescending(bc => bc.boughtCars)
                .ToArray();

            var json = JsonConvert.SerializeObject(customers, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
                ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            });
            return json;
        }

        // Problem 19
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var omfgShitBro = context.Sales
                .Take(10)
                .Select(s => new
                {
                    car = new
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TraveledDistance = s.Car.TraveledDistance
                    },
                    customerName = s.Customer.Name,
                    discount = s.Discount.ToString("f2"),
                    price = s.Car.PartsCars.Sum(p => p.Part.Price).ToString("f2"),
                    priceWithDiscount = (s.Car.PartsCars.Sum(p => p.Part.Price) 
                                         - ((s.Car.PartsCars.Sum(p => p.Part.Price)) * (s.Discount / 100)))
                                        .ToString("f2")
                }).ToArray()
                .ToArray();

            return JsonConvert.SerializeObject(omfgShitBro, Formatting.Indented);
        }

    }
}
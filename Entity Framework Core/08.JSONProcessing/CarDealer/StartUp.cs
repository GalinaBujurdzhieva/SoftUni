using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using AutoMapper;
using CarDealer.Data;
using CarDealer.DTO;
using CarDealer.Models;
using Newtonsoft.Json;

namespace CarDealer
{
    public class StartUp
    {
        static IMapper mapper;
        public static void Main(string[] args)
        {
            var dbContext = new CarDealerContext();
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            //09. Import Suppliers
            string suppliersInput = File.ReadAllText("../../../Datasets/suppliers.json");
            //Console.WriteLine(ImportSuppliers(dbContext, suppliersInput));

            //10. Import Parts
            ImportSuppliers(dbContext, suppliersInput);
            string partsInput = File.ReadAllText("../../../Datasets/parts.json");
            //Console.WriteLine(ImportParts(dbContext, partsInput));

            //11. Import Cars
            ImportParts(dbContext, partsInput);
            string carsInput = File.ReadAllText("../../../Datasets/cars.json");
            //Console.WriteLine(ImportCars(dbContext, carsInput));

            //12. Import Customers
            ImportCars(dbContext, carsInput);
            string customersInput = File.ReadAllText("../../../Datasets/customers.json");
            //Console.WriteLine(ImportCustomers(dbContext, customersInput));

            //13. Import Sales
            ImportCustomers(dbContext, customersInput);
            string salesInput = File.ReadAllText("../../../Datasets/sales.json");
            //Console.WriteLine(ImportSales(dbContext, salesInput));

            //14. Export Ordered Customers
            ImportSales(dbContext, salesInput);
            //Console.WriteLine(GetOrderedCustomers(dbContext));

            //15. Export Cars from make Toyota
            //Console.WriteLine(GetCarsFromMakeToyota(dbContext));

            //16. Export Local Suppliers
            //Console.WriteLine(GetLocalSuppliers(dbContext));

            //17.Export Cars with Their List of Parts
            Console.WriteLine(GetCarsWithTheirListOfParts(dbContext));

            //18. Export Total Sales by Customer
            //Console.WriteLine(GetTotalSalesByCustomer(dbContext));

            //19. Export Sales with Applied Discount
            //Console.WriteLine(GetSalesWithAppliedDiscount(dbContext));
        }

        //09. Import Suppliers
        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            InitializeMapper();
            var suppliersDTOObjects = JsonConvert.DeserializeObject<IEnumerable<SupplierDTOInputModel>>(inputJson);
            var suppliers = mapper.Map<IEnumerable<Supplier>>(suppliersDTOObjects);
            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();
            return $"Successfully imported {suppliers.Count()}.";
        }

        //10. Import Parts
        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            InitializeMapper();
            var suppliersIds = context.Suppliers.Select(x => x.Id).ToList();
            var partsDTOObjects = JsonConvert.DeserializeObject<IEnumerable<PartDTOInputModel>>(inputJson).Where(y => suppliersIds.Contains(y.SupplierId));
            var parts = mapper.Map<IEnumerable<Part>>(partsDTOObjects);
            context.Parts.AddRange(parts);
            context.SaveChanges();
            return $"Successfully imported {parts.Count()}.";
        }

        //11. Import Cars
        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            InitializeMapper();
            var carsDTOObjects = JsonConvert.DeserializeObject<IEnumerable<CarDtoInputModel>>(inputJson);
            var currentCarsCollection = new List<Car>();

            foreach (var car in carsDTOObjects)
            {
                var currentCar = new Car
                {
                    Make = car.Make,
                    Model = car.Model,
                    TravelledDistance = car.TravelledDistance
                };

                foreach (var partId in car.PartsId.Distinct())
                {
                    currentCar.PartCars.Add(new PartCar
                    {
                        PartId = partId
                    });
                }
                currentCarsCollection.Add(currentCar);
            }
            context.Cars.AddRange(currentCarsCollection);
            context.SaveChanges();
            return $"Successfully imported {currentCarsCollection.Count}.";
        }

        //12. Import Customers
        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            InitializeMapper();
            var customersDTOObjects = JsonConvert.DeserializeObject<IEnumerable<CustomerDTOInputModel>>(inputJson);
            var customers = mapper.Map<IEnumerable<Customer>>(customersDTOObjects);
            context.Customers.AddRange(customers);
            context.SaveChanges();
            return $"Successfully imported {customers.Count()}.";
        }

        //13. Import Sales
        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            InitializeMapper();
            var salesDTOObjects = JsonConvert.DeserializeObject<IEnumerable<SalesDTOInputModel>>(inputJson);
            var sales = mapper.Map<IEnumerable<Sale>>(salesDTOObjects);
            context.AddRange(sales);
            context.SaveChanges();
            return $"Successfully imported {sales.Count()}.";
        }

        //14. Export Ordered Customers
        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customers = context.Customers
                .OrderBy(x => x.BirthDate)
                .ThenBy(x => x.IsYoungDriver)
                .Select(x => new
                {
                    Name = x.Name,
                    BirthDate = x.BirthDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    IsYoungDriver = x.IsYoungDriver
                })
                .ToList();

            return JsonConvert.SerializeObject(customers, Formatting.Indented);
        }

        //15. Export Cars from make Toyota
        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var toyotaCars = context.Cars
                .Where(x => x.Make == "Toyota")
                .OrderBy(x => x.Model)
                .ThenByDescending(x => x.TravelledDistance)
                .Select(x => new
                {
                    Id = x.Id,
                    Make = x.Make,
                    Model = x.Model,
                    TravelledDistance = x.TravelledDistance
                })
                .ToList();

            return JsonConvert.SerializeObject(toyotaCars, Formatting.Indented);
        }

        //16. Export Local Suppliers
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var localSuppliers = context.Suppliers
                .Where(x => x.IsImporter == false)
                .Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name,
                    PartsCount = x.Parts.Count
                })
                .ToList();

            return JsonConvert.SerializeObject(localSuppliers, Formatting.Indented);
        }

        //17. Export Cars with Their List of Parts
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var carsWithParts = context.Cars
                .Select(c => new
                {
                    car = new
                    {
                        Make = c.Make,
                        Model = c.Model,
                        TravelledDistance = c.TravelledDistance
                    },
                    parts = c.PartCars.
                        Select(p => new
                        {
                            Name = p.Part.Name,
                            Price = p.Part.Price.ToString("F2")
                        }).ToList()
                }).ToList();
            
            return JsonConvert.SerializeObject(carsWithParts, Formatting.Indented);
        }

        //18. Export Total Sales by Customer
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers
                .Where(x => x.Sales.Count >= 1)
                .Select(c => new
                {
                    fullName = c.Name,
                    boughtCars = c.Sales.Count,
                    spentMoney = c.Sales.Sum(m => m.Car.PartCars.Sum(y => y.Part.Price))
                })
                .OrderByDescending(x => x.spentMoney)
                .ThenByDescending(x => x.boughtCars)
                .ToList();

            return JsonConvert.SerializeObject(customers, Formatting.Indented);
        }

        //19. Export Sales with Applied Discount
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .Take(10)
                .Select(x => new
                {
                    car = new
                    {
                        Make = x.Car.Make,
                        Model = x.Car.Model,
                        TravelledDistance = x.Car.TravelledDistance
                    },
                    customerName = x.Customer.Name,
                    Discount = x.Discount.ToString("F2"),
                    price = x.Car.PartCars.Sum(y => y.Part.Price).ToString("F2"),
                    priceWithDiscount = ((x.Car.PartCars.Sum(y => y.Part.Price)) * (1 - x.Discount / 100)).ToString("F2")
                }).ToList();

            return JsonConvert.SerializeObject(sales, Formatting.Indented);
        }

        private static void InitializeMapper()
        {
            var configuration = new MapperConfiguration(config =>
            {
                config.AddProfile<CarDealerProfile>();
            });
            mapper = configuration.CreateMapper();
        }
    }
}
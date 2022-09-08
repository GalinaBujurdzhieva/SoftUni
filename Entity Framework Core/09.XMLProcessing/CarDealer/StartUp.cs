using AutoMapper;
using CarDealer.Data;
using CarDealer.DTO.InputModels;
using CarDealer.DTO.OutputModels;
using CarDealer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        static IMapper mapper;
        public static void Main(string[] args)
        {
            var dbContext = new CarDealerContext();
            /*
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            */

            //09. Import Suppliers
            string suppliersInput = File.ReadAllText("./Datasets/suppliers.xml");
            //Console.WriteLine(ImportSuppliers(dbContext, suppliersInput));

            //10. Import Parts
            ImportSuppliers(dbContext, suppliersInput);
            string partsInput = File.ReadAllText("./Datasets/parts.xml");
            //Console.WriteLine(ImportParts(dbContext, partsInput));

            //11. Import Cars
            ImportParts(dbContext, partsInput);
            string carsInput = File.ReadAllText("./Datasets/cars.xml");
            //Console.WriteLine(ImportCars(dbContext, carsInput));

            //12. Import Customers
            ImportCars(dbContext, carsInput);
            string customersInput = File.ReadAllText("./Datasets/customers.xml");
            //Console.WriteLine(ImportCustomers(dbContext, customersInput));

            //13. Import Sales
            ImportCustomers(dbContext, customersInput);
            string salesInput = File.ReadAllText("./Datasets/sales.xml");
            //Console.WriteLine(ImportSales(dbContext, salesInput));

            //14. Export Cars With Distance
            ImportSales(dbContext, salesInput);
            //Console.WriteLine(GetCarsWithDistance(dbContext));

            //15. Export Cars From Make BMW
            //Console.WriteLine(GetCarsFromMakeBmw(dbContext));

            //16.Export Local Suppliers
            //Console.WriteLine(GetLocalSuppliers(dbContext));

            //17. Export Cars With Their List Of Parts
            Console.WriteLine(GetCarsWithTheirListOfParts(dbContext)); 

            //18. Export Total Sales By Customer
            //Console.WriteLine(GetTotalSalesByCustomer(dbContext));

            //19. Export Sales With Applied Discount
            //Console.WriteLine(GetSalesWithAppliedDiscount(dbContext));
        }

        //09. Import Suppliers
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            InitializeMapper();
            const string rootElement = "Suppliers";
            var serializer = new XmlSerializer(typeof(SupplierInputModel[]), new XmlRootAttribute(rootElement));
            var textReader = new StringReader(inputXml);
            var suppliersDTO = serializer.Deserialize(textReader) as SupplierInputModel[];
            var suppliers = mapper.Map<Supplier[]>(suppliersDTO);
            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Length}";
        }

        //10. Import Parts
        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            InitializeMapper();
            const string rootElement = "Parts";
            var serializer = new XmlSerializer(typeof(PartInputModel[]), new XmlRootAttribute(rootElement));
            var textReader = new StringReader(inputXml);
            var partsDTO = serializer.Deserialize(textReader) as PartInputModel[];
            var supplierIds = context.Suppliers.Select(x => x.Id).ToArray();
            var validPartsDTO = partsDTO.Where(x => supplierIds.Contains(x.SupplierId));

            var parts = mapper.Map<Part[]>(validPartsDTO);
            context.Parts.AddRange(parts);
            context.SaveChanges();
            
            return $"Successfully imported {parts.Length}";
        }

        //11. Import Cars
        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            const string rootElement = "Cars";
            var serializer = new XmlSerializer(typeof(CarInputModel[]), new XmlRootAttribute(rootElement));
            var textReader = new StringReader(inputXml);
            var carsDTO = serializer.Deserialize(textReader) as CarInputModel[];
            List<Car> cars = new List<Car>();

            foreach (var carDTO in carsDTO)
            {
                var allPartIds = context.Parts.Select(x => x.Id).ToList();
                var distinctPartIds = carDTO.PartCars.Select(x => x.PartId).Distinct();
                var validPartIds = distinctPartIds.Intersect(allPartIds);
                var currentCar = new Car
                {
                    Make = carDTO.Make,
                    Model = carDTO.Model,
                    TravelledDistance = carDTO.TravelledDistance
                };
                foreach (var partId in validPartIds)
                {
                    var currentCarPartCar = new PartCar
                    {
                        PartId = partId
                    };
                    currentCar.PartCars.Add(currentCarPartCar);
                }
                cars.Add(currentCar);
            }
            context.Cars.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}";
        }

        //12. Import Customers
        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            InitializeMapper();
            const string rootElement = "Customers";
            var serializer = new XmlSerializer(typeof(CustomInputModel[]), new XmlRootAttribute(rootElement));
            var textReader = new StringReader(inputXml);
            var customersDTO = serializer.Deserialize(textReader) as CustomInputModel[];
            var customers = mapper.Map<Customer[]>(customersDTO);
            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Length}";
        }

        //13. Import Sales
        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            InitializeMapper();
            const string rootElement = "Sales";
            var serializer = new XmlSerializer(typeof(SalesInputModel[]), new XmlRootAttribute(rootElement));
            var textReader = new StringReader(inputXml);
            var salesDTO = serializer.Deserialize(textReader) as SalesInputModel[];
            var allCarIds = context.Cars.Select(x => x.Id).ToList();
            var validSalesDTO = salesDTO.Where(x => allCarIds.Contains(x.CarId));
            var sales = mapper.Map<Sale[]>(validSalesDTO);
            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Length}";
        }

        //14. Export Cars With Distance
        public static string GetCarsWithDistance(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(x => x.TravelledDistance > 2_000_000)
                .OrderBy(x => x.Make)
                .ThenBy(x => x.Model)
                .Select(x => new CarWithDistanceOutputModel
                {
                    Make = x.Make,
                    Model = x.Model,
                    TravelledDistance = x.TravelledDistance
                })
                .Take(10)
                .ToArray();
            
            const string rootElement = "cars";
            var serializer = new XmlSerializer(typeof(CarWithDistanceOutputModel[]), new XmlRootAttribute(rootElement));

            var textWriter = new StringWriter();
            using (textWriter)
            {
                serializer.Serialize(textWriter, cars, GetXmlNamespaces());
            }
            return textWriter.ToString();
        }
        //15. Export Cars From Make BMW
        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            var bmvCars = context.Cars
                .Where(x => x.Make == "BMW")
                .Select(x => new BMWCarsOutputModel
                {
                    Id = x.Id,
                    Model = x.Model,
                    TravelledDistance = x.TravelledDistance
                })
                .OrderBy(x => x.Model)
                .ThenByDescending(x => x.TravelledDistance)
                .ToArray();

            const string rootElement = "cars";
            var serializer = new XmlSerializer(typeof(BMWCarsOutputModel[]), new XmlRootAttribute(rootElement));
            var textWriter = new StringWriter();
            using (textWriter)
            {
                serializer.Serialize(textWriter, bmvCars, GetXmlNamespaces());
            }
            return textWriter.ToString();
        }

        //16. Export Local Suppliers
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var localSuppliers = context.Suppliers
                .Where(x => x.IsImporter == false)
                .Select(x => new SupplierOutputModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Parts = x.Parts.Count()
                })
                .ToArray();

            const string rootElement = "suppliers";
            var serializer = new XmlSerializer(typeof(SupplierOutputModel[]), new XmlRootAttribute(rootElement));
            var textWriter = new StringWriter();
            using (textWriter)
            {
                serializer.Serialize(textWriter, localSuppliers, GetXmlNamespaces());
            }
            return textWriter.ToString();
        }

        //17. Export Cars With Their List Of Parts
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var carsWithTheirParts = context.Cars
                .Select(x => new CarWithItsPartsOutputModel
                {
                    Make = x.Make,
                    Model = x.Model,
                    TravelledDistance = x.TravelledDistance,
                    Parts = x.PartCars.Select(y => new PartInfoOutputModel
                    {
                        Name = y.Part.Name,
                        Price = y.Part.Price
                    })
                    .OrderByDescending(y => y.Price)
                    .ToArray()
                })
                .OrderByDescending(x => x.TravelledDistance)
                .ThenBy(x => x.Model)
                .Take(5)
                .ToArray();

            const string rootElement = "cars";
            //var xmlResult = XmlConverter.Serialize(carsWithTheirParts, rootElement);
            //return xmlResult;

            var serializer = new XmlSerializer(typeof(CarWithItsPartsOutputModel[]), new XmlRootAttribute(rootElement));
            var textWriter = new StringWriter();
            using (textWriter)
            {
                serializer.Serialize(textWriter, carsWithTheirParts, GetXmlNamespaces());
            }
            return textWriter.ToString();
        }

        //18. Export Total Sales By Customer
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customersWithAtLeastOneBoughtCar = context.Customers
                .Where(x => x.Sales.Count >= 1)
                .Select(x => new CustomersWithBoughtCarsOutputModel
                {
                    FullName = x.Name,
                    BoughtCars = x.Sales.Count(),
                    SpentMoney = x.Sales.Select(y => y.Car).SelectMany(z => z.PartCars).Sum(p => p.Part.Price)
                })
                .OrderByDescending(x => x.SpentMoney)
                .ToArray();
            
            const string rootElement = "customers";
            var serializer = new XmlSerializer(typeof(CustomersWithBoughtCarsOutputModel[]), new XmlRootAttribute(rootElement));
            var textWriter = new StringWriter();
            using (textWriter)
            {
                serializer.Serialize(textWriter, customersWithAtLeastOneBoughtCar, GetXmlNamespaces());
            }
            return textWriter.ToString();
        }

        //19. Export Sales With Applied Discount
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var salesWithDiscount = context.Sales
                .Select(x => new SalesWithDiscountOutputModel
                {
                    Car = new CarWithDiscountOutputModel
                    {
                        Make = x.Car.Make,
                        Model = x.Car.Model,
                        TravelledDistance = x.Car.TravelledDistance
                    },
                    Discount = x.Discount,
                    CustomerName = x.Customer.Name,
                    Price = x.Car.PartCars.Sum(y => y.Part.Price),
                    PriceWithDiscount = (x.Car.PartCars.Sum(y => y.Part.Price) * (100 * 1m - x.Discount) / 100)
                })
                .ToArray();

            const string rootElement = "sales";
            var serializer = new XmlSerializer(typeof(SalesWithDiscountOutputModel[]), new XmlRootAttribute(rootElement));
            var textWriter = new StringWriter();
            using (textWriter)
            {
                serializer.Serialize(textWriter, salesWithDiscount, GetXmlNamespaces());
            }
            return textWriter.ToString();
        }

        private static void InitializeMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });
            mapper = config.CreateMapper();
        }

        private static XmlSerializerNamespaces GetXmlNamespaces()
        {
            XmlSerializerNamespaces xmlNamespaces = new XmlSerializerNamespaces();
            xmlNamespaces.Add(string.Empty, string.Empty);
            return xmlNamespaces;
        }
    }
}
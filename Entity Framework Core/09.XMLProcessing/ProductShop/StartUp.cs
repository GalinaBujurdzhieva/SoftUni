using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ProductShop.Data;
using ProductShop.Dtos.Export;
using ProductShop.Dtos.Import;
using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        
        public static void Main(string[] args)
        {
            var dbContext = new ProductShopContext();
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            //01. Import Users
            string usersInput = File.ReadAllText("./Datasets/users.xml");
            //Console.WriteLine(ImportUsers(dbContext, usersInput));

            //02. Import Products
            ImportUsers(dbContext, usersInput);
            string productsInput = File.ReadAllText("./Datasets/products.xml");
            //Console.WriteLine(ImportProducts(dbContext, productsInput));
            
            //03. Import Categories
            ImportProducts(dbContext, productsInput);
            string categoriesInput = File.ReadAllText("./Datasets/categories.xml");
            //Console.WriteLine(ImportCategories(dbContext, categoriesInput));

            //04. Import Categories & Products
            ImportCategories(dbContext, categoriesInput);
            string categoriesAndProductsInput = File.ReadAllText("./Datasets/categories-products.xml");
            //Console.WriteLine(ImportCategoryProducts(dbContext, categoriesAndProductsInput));

            //05. Export Products in Range
            ImportCategoryProducts(dbContext, categoriesAndProductsInput);
            Console.WriteLine(GetProductsInRange(dbContext));

            //06. Export Sold Products
            //Console.WriteLine(GetSoldProducts(dbContext));

            //07. Categories By Products Count
            //Console.WriteLine(GetCategoriesByProductsCount(dbContext));

            //08. Export Users and Products
            //Console.WriteLine(GetUsersWithProducts(dbContext));
        }

        //01. Import Users
        public static string  ImportUsers(ProductShopContext context, string inputXml)
        {
            IMapper mapper = InitializeAutomapper();
            const string rootElement = "Users";
            var serializer = new XmlSerializer(typeof(UserInputModel[]), new XmlRootAttribute(rootElement));
            var textReader = new StringReader(inputXml);
            var usersDTO = serializer.Deserialize(textReader) as UserInputModel[];
            var users = mapper.Map<User[]>(usersDTO);
            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Length}";
        }

        //02.Import Products
        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            IMapper mapper = InitializeAutomapper();
            const string rootElement = "Products";
            var serializer = new XmlSerializer(typeof(List<ProductInputModel>), new XmlRootAttribute(rootElement));
            var textReader = new StringReader(inputXml);
            var productsDto = serializer.Deserialize(textReader) as List<ProductInputModel>;
            var products = mapper.Map<List<Product>>(productsDto);
            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count}";
        }

        //03. Import Categories
        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            IMapper mapper = InitializeAutomapper();
            const string rootElement = "Categories";
            var serializer = new XmlSerializer(typeof(CategoryInputModel[]), new XmlRootAttribute(rootElement));
            var textReader = new StringReader(inputXml);
            var categoriesDTO = serializer.Deserialize(textReader) as CategoryInputModel[];
            var filteredDTOs = categoriesDTO.Where(x => x.Name != null).ToList();
            var categories = mapper.Map<Category[]>(filteredDTOs);
            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Length}";
        }

        //04. Import Categories and Products
        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            IMapper mapper = InitializeAutomapper();
            const string rootElement = "CategoryProducts";
            var serializer = new XmlSerializer(typeof(CategoryProductInputModel[]), new XmlRootAttribute(rootElement));
            var textReader = new StringReader(inputXml);
            var categoriesAndProductsDto = serializer.Deserialize(textReader) as CategoryProductInputModel[];
            var productsIds = context.Products.Select(x => x.Id).ToList();
            var categoriesIds = context.Categories.Select(x => x.Id).ToList();

            var filteredCategoriesAndProductsDto = categoriesAndProductsDto.Where(x => productsIds.Contains(x.ProductId) && categoriesIds.Contains(x.CategoryId)).ToList();
            var categoriesAndProducts = mapper.Map<CategoryProduct[]>(filteredCategoriesAndProductsDto);
            context.CategoryProducts.AddRange(categoriesAndProducts);
            context.SaveChanges();

            return $"Successfully imported {categoriesAndProducts.Length}";
        }

        //05. Export Products In Range
        public static string GetProductsInRange(ProductShopContext context)
        {
            var productsInRange = context.Products
                 .Where(x => x.Price >= 500 && x.Price <= 1000)
                 .OrderBy(x => x.Price)
                 .Select(x => new ProductsInRangeOutputModel
                 {
                     Name = x.Name,
                     Price = x.Price,
                     Buyer = x.Buyer.FirstName + ' ' + x.Buyer.LastName
                 })
                 .Take(10)
                 .ToArray();

            /*IMapper mapper = InitializeAutomapper();
            ProductsInRangeOutputModel[] productsInRange1 = context.Products
              .Where(x => x.Price >= 500 && x.Price <= 1000)
              .OrderBy(x => x.Price)
              .Take(10)
              .ProjectTo<ProductsInRangeOutputModel>(mapper.ConfigurationProvider)
              .ToArray();*/

            const string rootElement = "Products";
            var serializer = new XmlSerializer(typeof(ProductsInRangeOutputModel[]), new XmlRootAttribute(rootElement));
            var textWriter = new StringWriter();
            using (textWriter)
            {
                serializer.Serialize(textWriter, productsInRange, GetXmlNamespaces());
            }
            return textWriter.ToString();
        }

        //06. Export Sold Products
        public static string GetSoldProducts(ProductShopContext context)
        {
            var usersWithSoldItems = context.Users
                .Where(x => x.ProductsSold.Count >= 1)
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .Select(x => new UsersWithSoldItemsOutputModel
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    SoldProducts = x.ProductsSold
                    .Select(y => new SoldByUsersProductsOutputModel
                    {
                        Name = y.Name,
                        Price = y.Price
                    })
                    .ToList()
                })
                .Take(5)
                .ToList();

            const string rootElement = "Users";
            var serializer = new XmlSerializer(typeof(List<UsersWithSoldItemsOutputModel>), new XmlRootAttribute(rootElement));
            var textWriter = new StringWriter();
            using (textWriter)
            {
                serializer.Serialize(textWriter, usersWithSoldItems, GetXmlNamespaces());
            }

            return textWriter.ToString();
        }

        //07. Categories By Products Count
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categoriesByProducts = context.Categories
                .Select(x => new CategoriesByProductsOutputModel
                {
                    Name = x.Name,
                    Count = x.CategoryProducts.Count,
                    AveragePrice = x.CategoryProducts.Average(y => y.Product.Price),
                    TotalRevenue = x.CategoryProducts.Sum(y => y.Product.Price)
                })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.TotalRevenue)
                .ToList();

            const string rootElement = "Categories";
            var serializer = new XmlSerializer(typeof(List<CategoriesByProductsOutputModel>), new XmlRootAttribute(rootElement));
            var textWriter = new StringWriter();
            using (textWriter)
            {
                serializer.Serialize(textWriter, categoriesByProducts, GetXmlNamespaces());
            }

            return textWriter.ToString();
        }

        //08. Export Users and Products
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var usersWithProducts = context.Users
                .Where(x => x.ProductsSold.Any())
                .OrderByDescending(x => x.ProductsSold.Count)
                .ToList()
                .Select(x => new UsersWithProductsOutputModel
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Age = x.Age,
                    SoldProducts = new SoldProductsOutputModel
                    {
                        Count = x.ProductsSold.Count,
                        Products = x.ProductsSold.
                        Select(y => new ProductsOutputModel
                        {
                            Name = y.Name,
                            Price = y.Price
                        })
                        .OrderByDescending(z => z.Price)
                        .ToList()
                    }
                })
                .Take(10)
                .ToList();

            var usersWithCountAndProducts = new UsersWithCountAndSoldItemsOutputModel
            {
                Count = context.Users.Where(x => x.ProductsSold.Any()).Count(),
                UsersWithProducts = usersWithProducts
            };

            const string rootElement = "Users";
            var serializer = new XmlSerializer(typeof(UsersWithCountAndSoldItemsOutputModel), new XmlRootAttribute(rootElement));
            var textWriter = new StringWriter();
            using (textWriter)
            {
                serializer.Serialize(textWriter, usersWithCountAndProducts, GetXmlNamespaces());
            }

            return textWriter.ToString();
        }

        private static IMapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            });
            return config.CreateMapper();
        }

        private static XmlSerializerNamespaces GetXmlNamespaces()
        {
            XmlSerializerNamespaces xmlNamespaces = new XmlSerializerNamespaces();
            xmlNamespaces.Add(string.Empty, string.Empty);
            return xmlNamespaces;
        }
    }
}
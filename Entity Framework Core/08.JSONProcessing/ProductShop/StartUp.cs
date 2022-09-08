using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.DTO;
using ProductShop.Models;

namespace ProductShop
{
    
    public class StartUp
    {
        static IMapper mapper;
        public static void Main(string[] args)
        {
            var dbContext = new ProductShopContext();
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            //01.Import Users
            string usersInput = File.ReadAllText("../../../Datasets/users.json");
            //Console.WriteLine(ImportUsers(dbContext, usersInput));


            //02. Import Products
            ImportUsers(dbContext, usersInput);
            string productsInput = File.ReadAllText("../../../Datasets/products.json");
            //Console.WriteLine(ImportProducts(dbContext, productsInput));

            //03. Import Categories
            ImportProducts(dbContext, productsInput);
            string categoriesInput = File.ReadAllText("../../../Datasets/categories.json");
            //Console.WriteLine(ImportCategories(dbContext, categoriesInput));

            //04. Import Categories and Products
            ImportCategories(dbContext, categoriesInput);
            string categoriesAndProductsInput = File.ReadAllText("../../../Datasets/categories-products.json");
            //Console.WriteLine(ImportCategoryProducts(dbContext, categoriesAndProductsInput));
            ImportCategoryProducts(dbContext, categoriesAndProductsInput);

            //05.Export Products In Range
            //Console.WriteLine(GetProductsInRange(dbContext));

            //06. Export Sold Products
            //Console.WriteLine(GetSoldProducts(dbContext));

            //07. Export Categories By Products Count
            //Console.WriteLine(GetCategoriesByProductsCount(dbContext));

            //08. Export Users and Products
            Console.WriteLine(GetUsersWithProducts(dbContext));
        }

        //01.Import Users
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            InitializeMapper();
            var usersDTOObjects = JsonConvert.DeserializeObject<IEnumerable<UserDTOInputModel>>(inputJson);
            var users = mapper.Map<IEnumerable<User>>(usersDTOObjects);
            context.Users.AddRange(users);
            context.SaveChanges();
            return $"Successfully imported {users.Count()}"; 
        }

        //02. Import Products
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            InitializeMapper();
            var productDTOObjects = JsonConvert.DeserializeObject<IEnumerable<ProductDTOInputModel>>(inputJson);
            var products = mapper.Map<IEnumerable<Product>>(productDTOObjects);
            context.Products.AddRange(products);
            context.SaveChanges();
            return $"Successfully imported {products.Count()}";
        }

        //03. Import Categories
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            InitializeMapper();
            var categoriesDTOObjects = JsonConvert.DeserializeObject<IEnumerable<CategoryDTOInputModel>>(inputJson);
            var categories = mapper.Map<IEnumerable<Category>>(categoriesDTOObjects).Where(x => x.Name != null);
            context.Categories.AddRange(categories);
            context.SaveChanges();
            return $"Successfully imported {categories.Count()}";
        }

        //04. Import Categories and Products
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            InitializeMapper();
            var categoriesAndProductsDTOObject = JsonConvert.DeserializeObject<IEnumerable<CategoryProductDTOInputModel>>(inputJson);
            var categoriesAndProducts = mapper.Map<IEnumerable<CategoryProduct>>(categoriesAndProductsDTOObject);
            context.CategoryProducts.AddRange(categoriesAndProducts);
            context.SaveChanges();
            return $"Successfully imported {categoriesAndProducts.Count()}";
        }

        //05.Export Products In Range
        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(x => x.Price >= 500 && x.Price <= 1000)
                .OrderBy(x => x.Price)
                .Select(x => new
                {
                    name = x.Name,
                    price = x.Price,
                    seller = x.Seller.FirstName + " " + x.Seller.LastName
                })
                .ToList();

            return JsonConvert.SerializeObject(products, Formatting.Indented);
        }

        //06. Export Sold Products
        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(x => x.ProductsSold.Any(y => y.BuyerId != null))
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    soldProducts = u.ProductsSold
                                    .Where(p => p.BuyerId != null)
                                    .Select(p => new
                                    {
                                        name = p.Name,
                                        price = p.Price,
                                        buyerFirstName = p.Buyer.FirstName,
                                        buyerLastName = p.Buyer.LastName
                                    }).ToList()
                }).ToList();

            return JsonConvert.SerializeObject(users, Formatting.Indented);
        }

        //07. Export Categories By Products Count
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .OrderByDescending(x => x.CategoryProducts.Count)
                .Select(c => new
                {
                    category = c.Name,
                    productsCount = c.CategoryProducts.Count,
                    averagePrice = c.CategoryProducts.Average(p => p.Product.Price).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture),
                    totalRevenue = c.CategoryProducts.Sum(p => p.Product.Price).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)
                })
                .ToList();
            
            return JsonConvert.SerializeObject(categories, Formatting.Indented);
        }

        //08. Export Users and Products
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any(p => p.BuyerId != null))
                .ToList()
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    age = u.Age,
                    soldProducts =  new
                    {
                        count = u.ProductsSold.Count(p => p.BuyerId != null),
                        products = u.ProductsSold.Where(p => p.BuyerId != null)
                        .Select(p => new
                            {
                               name = p.Name,
                               price = p.Price
                            })
                        .ToList()
                    }
                })
                .OrderByDescending(u => u.soldProducts.count)
                .ToList();
            
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            };

            var finalUsers = new
            {
                usersCount = users.Count,
                users
            };
            return JsonConvert.SerializeObject(finalUsers, settings);
        }

        private static void InitializeMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            });
            mapper = config.CreateMapper();
        }
    }
}
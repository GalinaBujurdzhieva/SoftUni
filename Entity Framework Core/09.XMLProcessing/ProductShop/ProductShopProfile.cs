using AutoMapper;
using ProductShop.Dtos.Export;
using ProductShop.Dtos.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            this.CreateMap<UserInputModel, User>();
            this.CreateMap<ProductInputModel, Product>();
            this.CreateMap<CategoryInputModel, Category>();
            this.CreateMap<CategoryProductInputModel, CategoryProduct>();
            this.CreateMap<Product, ProductsInRangeOutputModel>()
                .ForMember(x => x.Name, opt => opt.MapFrom(s => s.Buyer.FirstName + " " + s.Buyer.LastName));
        }
    }
}

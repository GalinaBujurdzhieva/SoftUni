using AutoMapper;
using ProductShop.DTO;
using ProductShop.Models;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            this.CreateMap<UserDTOInputModel, User>();
            this.CreateMap<ProductDTOInputModel, Product>();
            this.CreateMap<CategoryDTOInputModel, Category>();
            this.CreateMap<CategoryProductDTOInputModel, CategoryProduct>();
        }
    }
}

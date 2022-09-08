using AutoMapper;
using CarDealer.DTO.InputModels;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            this.CreateMap<SupplierInputModel, Supplier>();
            this.CreateMap<PartInputModel, Part>();
            this.CreateMap<CustomInputModel, Customer>();
            this.CreateMap<SalesInputModel, Sale>();
        }
    }
}

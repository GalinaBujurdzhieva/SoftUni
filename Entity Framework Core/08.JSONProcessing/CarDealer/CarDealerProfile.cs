using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using CarDealer.DTO;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            this.CreateMap<SupplierDTOInputModel, Supplier>();
            this.CreateMap<PartDTOInputModel, Part>();
            this.CreateMap<CustomerDTOInputModel, Customer>();
            this.CreateMap<SalesDTOInputModel, Sale>();
        }
    }
}

namespace Artillery
{
    using Artillery.Data.Models;
    using Artillery.DataProcessor.ImportDto;
    using AutoMapper;
    class ArtilleryProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
        public ArtilleryProfile()
        {
            this.CreateMap<CountryXmlImportModel, Country>();
            this.CreateMap<ManufacturerXmlImportModel, Manufacturer>();
            this.CreateMap<ShellXmlImportModel, Shell>();
        }
    }
}
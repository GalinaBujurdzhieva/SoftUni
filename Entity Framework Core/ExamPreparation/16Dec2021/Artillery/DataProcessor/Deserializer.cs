namespace Artillery.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Artillery.Data;
    using Artillery.Data.Models;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ImportDto;
    using AutoMapper;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage =
                "Invalid data.";
        private const string SuccessfulImportCountry =
            "Successfully import {0} with {1} army personnel.";
        private const string SuccessfulImportManufacturer =
            "Successfully import manufacturer {0} founded in {1}.";
        private const string SuccessfulImportShell =
            "Successfully import shell caliber #{0} weight {1} kg.";
        private const string SuccessfulImportGun =
            "Successfully import gun {0} with a total weight of {1} kg. and barrel length of {2} m.";
        
        static IMapper mapper;

        public static string ImportCountries(ArtilleryContext context, string xmlString)
        {
            const string rootElement = "Countries";
            var serializer = new XmlSerializer(typeof(List<CountryXmlImportModel>), new XmlRootAttribute(rootElement));
            var textReader = new StringReader(xmlString);
            var countriesDTO = serializer.Deserialize(textReader) as List<CountryXmlImportModel>;
            
            List<Country> validCountries = new List<Country>();
            StringBuilder sb = new StringBuilder();
            InitializeMapper();

            foreach (var countryDTO in countriesDTO)
            {
                if (!IsValid(countryDTO))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                var currentValidCountry = mapper.Map<Country>(countryDTO);
                validCountries.Add(currentValidCountry);
                sb.AppendLine(string.Format(SuccessfulImportCountry, currentValidCountry.CountryName, currentValidCountry.ArmySize));
            }
            context.Countries.AddRange(validCountries);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportManufacturers(ArtilleryContext context, string xmlString)
        {
            const string rootElement = "Manufacturers";
            var serializer = new XmlSerializer(typeof(List<ManufacturerXmlImportModel>), new XmlRootAttribute(rootElement));
            var textReader = new StringReader(xmlString);
            var manufacturersDTO = serializer.Deserialize(textReader) as List<ManufacturerXmlImportModel>;

            List<Manufacturer> validManufacturers = new List<Manufacturer>();
            StringBuilder sb = new StringBuilder();
            InitializeMapper();

            foreach (var manufacturerDTO in manufacturersDTO)
            {
                bool isManufacturerNotUnique = validManufacturers.Any(x => x.ManufacturerName == manufacturerDTO.ManufacturerName);
                
                if (!IsValid(manufacturerDTO) || isManufacturerNotUnique)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                var currentValidManufacturer = mapper.Map<Manufacturer>(manufacturerDTO);
                validManufacturers.Add(currentValidManufacturer);
                var currentValidManufacturerFoundedDetails = currentValidManufacturer.Founded.Split(", ").ToArray();
                var currentValidManufacturerCityAndCountry = currentValidManufacturerFoundedDetails.Skip(currentValidManufacturerFoundedDetails.Length - 2).ToArray();

                sb.AppendLine(string.Format(SuccessfulImportManufacturer, currentValidManufacturer.ManufacturerName, string.Join(", ", currentValidManufacturerCityAndCountry)));
            }
            context.Manufacturers.AddRange(validManufacturers);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportShells(ArtilleryContext context, string xmlString)
        {
            const string rootElement = "Shells";
            var serializer = new XmlSerializer(typeof(List<ShellXmlImportModel>), new XmlRootAttribute(rootElement));
            var textReader = new StringReader(xmlString);
            var shellsDTO = serializer.Deserialize(textReader) as List<ShellXmlImportModel>;

            List<Shell> validShells = new List<Shell>();
            StringBuilder sb = new StringBuilder();
            InitializeMapper();

            foreach (var shellDTO in shellsDTO)
            {
                if (!IsValid(shellDTO))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                var currentValidShell = mapper.Map<Shell>(shellDTO);
                validShells.Add(currentValidShell);
                sb.AppendLine(string.Format(SuccessfulImportShell, currentValidShell.Caliber, currentValidShell.ShellWeight));
            }
            context.Shells.AddRange(validShells);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportGuns(ArtilleryContext context, string jsonString)
        {
            var gunsWithCountriesDTO = JsonConvert.DeserializeObject<IEnumerable<GunJsonImportModel>>(jsonString);
            List<Gun> validGuns = new List<Gun>();
            StringBuilder sb = new StringBuilder();

            foreach (var gunWithCountryDTO in gunsWithCountriesDTO)
            {
                bool isCurrentDTOEnumValid = Enum.TryParse(gunWithCountryDTO.GunType, out GunType gunType);
                
                if (!IsValid(gunWithCountryDTO) || !isCurrentDTOEnumValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                var currentGunWithCountry = new Gun
                {
                    ManufacturerId = gunWithCountryDTO.ManufacturerId,
                    GunWeight = gunWithCountryDTO.GunWeight,
                    BarrelLength = gunWithCountryDTO.BarrelLength,
                    NumberBuild = gunWithCountryDTO.NumberBuild,
                    Range = gunWithCountryDTO.Range,
                    GunType = gunType,
                    ShellId = gunWithCountryDTO.ShellId
                };
                foreach (var countryDto in gunWithCountryDTO.Countries)
                {
                    var currentGunCountry = context.Countries.FirstOrDefault(x => x.Id == countryDto.Id);

                    currentGunWithCountry.CountriesGuns.Add(new CountryGun
                    {
                        Gun = currentGunWithCountry,
                        Country = currentGunCountry
                    });
                }
                validGuns.Add(currentGunWithCountry);
                sb.AppendLine(string.Format(SuccessfulImportGun, currentGunWithCountry.GunType, currentGunWithCountry.GunWeight, currentGunWithCountry.BarrelLength));
            }
            context.Guns.AddRange(validGuns);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }
        private static bool IsValid(object obj)
        {
            var validator = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
        private static void InitializeMapper()
        {
            var configuration = new MapperConfiguration(config =>
            {
                config.AddProfile<ArtilleryProfile>();
            });
            mapper = configuration.CreateMapper();
        }
    }

    
}

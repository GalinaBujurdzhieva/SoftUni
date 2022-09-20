
namespace Artillery.DataProcessor
{
    using Artillery.Data;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ExportDto;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportShells(ArtilleryContext context, double shellWeight)
        {
            var shellsToBeExported = context.Shells
                .Where(x => x.ShellWeight > shellWeight)
                .OrderBy(x => x.ShellWeight)
                .Select(x => new
                {
                    ShellWeight = x.ShellWeight,
                    Caliber = x.Caliber,
                    Guns = x.Guns
                    .Where(g => g.GunType == GunType.AntiAircraftGun)
                    .OrderByDescending(g => g.GunWeight)
                    .Select(g => new
                    {
                        GunType = g.GunType.ToString(),
                        GunWeight = g.GunWeight,
                        BarrelLength = g.BarrelLength,
                        Range = g.Range > 3000 ? "Long-range" : "Regular range"
                    }).ToList()

                }).ToList();

            return JsonConvert.SerializeObject(shellsToBeExported, Formatting.Indented);
        }

        public static string ExportGuns(ArtilleryContext context, string manufacturer)
        {
            var gunsWithCountries = context.Guns
                .Where(x => x.Manufacturer.ManufacturerName == manufacturer)
                .OrderBy(x => x.BarrelLength)
                .Select(x => new GunWithCountryXmlExportModel
                {
                    Manufacturer = x.Manufacturer.ManufacturerName,
                    GunType = x.GunType.ToString(),
                    GunWeight = x.GunWeight,
                    BarrelLength = x.BarrelLength,
                    Range = x.Range,
                    Countries = x.CountriesGuns
                           .Where(x => x.Country.ArmySize > 4500000)
                           .OrderBy(c => c.Country.ArmySize)
                           .Select(c => new CountryXmlExportModel
                                   {
                                       CountryName = c.Country.CountryName,
                                       ArmySize = c.Country.ArmySize
                                   }).ToArray()
                }).ToArray();

            const string rootElement = "Guns";
            var serializer = new XmlSerializer(typeof(GunWithCountryXmlExportModel[]), new XmlRootAttribute(rootElement));
            var textWriter = new StringWriter();
            using (textWriter)
            {
                serializer.Serialize(textWriter, gunsWithCountries, GetXmlNamespaces());
            }
            return textWriter.ToString();
        }
        private static XmlSerializerNamespaces GetXmlNamespaces()
        {
            XmlSerializerNamespaces xmlNamespaces = new XmlSerializerNamespaces();
            xmlNamespaces.Add(string.Empty, string.Empty);
            return xmlNamespaces;
        }
    }
}

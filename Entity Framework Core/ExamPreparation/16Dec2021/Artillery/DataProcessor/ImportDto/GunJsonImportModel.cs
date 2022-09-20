using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace Artillery.DataProcessor.ImportDto
{
    public class GunJsonImportModel
    {
        public GunJsonImportModel()
        {
            Countries = new HashSet<GunCountryJsonImportModel>();
        }
        public int ManufacturerId { get; set; }

        [Range(100, 1_350_000)]
        public int GunWeight { get; set; }

        [Range(2.00, 35.00)]
        public double BarrelLength { get; set; }
        public int? NumberBuild { get; set; }

        [Range(1, 100_000)]
        public int Range { get; set; }

        public string GunType { get; set; }
        public int ShellId { get; set; }
        public ICollection<GunCountryJsonImportModel> Countries { get; set; }

    }
}

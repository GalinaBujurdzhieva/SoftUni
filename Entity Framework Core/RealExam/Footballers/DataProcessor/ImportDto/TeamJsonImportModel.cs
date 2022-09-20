using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Footballers.DataProcessor.ImportDto
{
    public class TeamJsonImportModel
    {
        
        [Required]
        [StringLength(40, MinimumLength = 3)]
        [RegularExpression(@"^[\w\s.-]{1,}$")]
        public string Name { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 2)]
        public string Nationality { get; set; }

        public int Trophies { get; set; }
        public IEnumerable<int> Footballers { get; set; }
    }
}

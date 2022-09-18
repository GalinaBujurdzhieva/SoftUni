using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VaporStore.DataProcessor.Dto.Import
{
    public class UserJsonImportModel
    {
        public UserJsonImportModel()
        {
            Cards = new List<CardJsonImportModel>();
        }
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Username { get; set; }

        [Required]
        [RegularExpression("^[A-Z]{1}[a-z]{1,} [A-Z]{1}[a-z]{1,}$")]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Range(3,103)]
        public int Age { get; set; }

        [MinLength(1)]
        public List<CardJsonImportModel> Cards { get; set; }
    }
}

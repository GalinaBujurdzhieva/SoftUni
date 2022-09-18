using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using VaporStore.Data.Models.Enums;

namespace VaporStore.DataProcessor.Dto.Import
{
    public class CardJsonImportModel
    {
        [Required]
        [MaxLength(19)]
        [RegularExpression("^[0-9]{4} [0-9]{4} [0-9]{4} [0-9]{4}$")]
        public string Number { get; set; }

        [Required]
        [MaxLength(3)]
        [RegularExpression("^[0-9]{3}$")]
        public string Cvc { get; set; }

        [Required]
        public CardType? Type { get; set; }
    }
}

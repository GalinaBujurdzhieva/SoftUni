using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Theatre.Data.Models
{
    public class Cast
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string FullName { get; set; }
        public bool IsMainCharacter { get; set; }

        [Required]
        [MaxLength(15)]
        public string PhoneNumber { get; set; }

        public int PlayId { get; set; }
        public virtual Play Play { get; set; }
    }
}

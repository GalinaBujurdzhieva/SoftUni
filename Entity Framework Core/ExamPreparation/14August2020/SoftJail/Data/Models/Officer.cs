using SoftJail.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SoftJail.Data.Models
{
    public class Officer
    {
        public Officer()
        {
            OfficerPrisoners = new HashSet<OfficerPrisoner>();
        }
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string FullName { get; set; }
        public decimal Salary { get; set; }
        public Position Position { get; set; }
        public Weapon Weapon { get; set; }
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }
        public virtual ICollection<OfficerPrisoner> OfficerPrisoners { get; set; }

    }
}

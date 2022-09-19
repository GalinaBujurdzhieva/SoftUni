﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SoftJail.Data.Models
{
    public class Department
    {
        public Department()
        {
            Cells = new HashSet<Cell>();
        }
        public int Id { get; set; }

        [Required]
        [MaxLength(25)]
        public string Name { get; set; }
        public virtual ICollection<Cell> Cells { get; set; }
    }
}

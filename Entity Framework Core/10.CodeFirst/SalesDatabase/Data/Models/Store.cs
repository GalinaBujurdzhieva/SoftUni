﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace P03_SalesDatabase.Data.Models
{
    public class Store
    {
        public Store()
        {
            Sales = new HashSet<Sale>();
        }
        public int StoreId { get; set; }

        [Required]
        [MaxLength(80)]
        public string Name { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }
    }
}

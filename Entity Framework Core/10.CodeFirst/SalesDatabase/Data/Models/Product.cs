using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace P03_SalesDatabase.Data.Models
{
    public class Product
    {
        public Product()
        {
            Sales = new HashSet<Sale>();
        }
        public int ProductId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public double Quantity  { get; set; }
        public decimal Price { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }

        [MaxLength(250)]
        public string Description { get; set; }
    }
}

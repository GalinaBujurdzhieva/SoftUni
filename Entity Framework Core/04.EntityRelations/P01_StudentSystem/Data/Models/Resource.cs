using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P01_StudentSystem.Data.Models
{
    public class Resource
    {
        public int ResourceId { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public ResourceTypeEnum ResourceType { get; set; }

        [Required]
        [Column(TypeName = "varchar(max)")]
        public string Url { get; set; }
  
        
        
    }
}

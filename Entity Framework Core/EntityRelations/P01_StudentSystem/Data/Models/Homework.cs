using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P01_StudentSystem.Data.Models
{
    [Table("HomeworkSubmissions")]
    public class Homework
    {

        public int HomeworkId { get; set; }
        [Required]
        [Column(TypeName = "varchar(max)")]
        public string Content { get; set; }
        public ContentTypeEnum ContentType { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public DateTime SubmissionTime { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SoftJail.DataProcessor.ImportDto
{
    public class DepartmentJsonImportModel
    {
        public DepartmentJsonImportModel()
        {
            Cells = new List<CellDepartmentJsonImportModel>();
        }

        [Required]
        [MinLength(3)]
        [MaxLength(25)]
        public string Name { get; set; }

        public List<CellDepartmentJsonImportModel> Cells { get; set; }
    }
}

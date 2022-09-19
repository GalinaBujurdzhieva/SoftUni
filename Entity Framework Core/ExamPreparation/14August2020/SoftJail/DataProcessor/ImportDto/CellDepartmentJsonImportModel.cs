using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SoftJail.DataProcessor.ImportDto
{
    public class CellDepartmentJsonImportModel
    {
        [Range(1,1000)]
        public int CellNumber { get; set; }
        public bool HasWindow { get; set; }
    }
}

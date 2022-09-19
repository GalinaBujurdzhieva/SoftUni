using System;
using System.Collections.Generic;
using System.Text;

namespace SoftJail.Data.Models
{
    public class OfficerPrisoner
    {
        public int PrisonerId { get; set; }
        public virtual Prisoner Prisoner { get; set; }
        public int OfficerId { get; set; }
        public virtual Officer Officer { get; set; }
    }
}

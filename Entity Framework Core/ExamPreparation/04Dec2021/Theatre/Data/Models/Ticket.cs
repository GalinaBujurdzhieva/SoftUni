using System;
using System.Collections.Generic;
using System.Text;

namespace Theatre.Data.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        public decimal Price { get; set; }
        public sbyte RowNumber { get; set; }
        public int PlayId { get; set; }
        public virtual Play Play { get; set; }
        public int TheatreId { get; set; }
        public virtual Theatre Theatre { get; set; }
    }
}

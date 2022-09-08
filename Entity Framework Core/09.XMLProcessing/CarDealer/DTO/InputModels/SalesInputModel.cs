using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer.DTO.InputModels
{
    [XmlType("Sale")]
    public class SalesInputModel
    {
        [XmlElement("discount")]
        public decimal Discount { get; set; }

        [XmlElement("carId")]
        public int CarId { get; set; }

        [XmlElement("customerId")]
        public int CustomerId { get; set; }
    }
}
//< Sale >
//        < carId > 342 </ carId >
//        < customerId > 29 </ customerId >
//        < discount > 0 </ discount >
//</ Sale >
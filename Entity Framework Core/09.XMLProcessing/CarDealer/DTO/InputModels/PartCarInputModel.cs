using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer.DTO.InputModels
{
    [XmlType("partId")]
    public class PartCarInputModel
    {
        [XmlAttribute("id")]
        public int PartId { get; set; }
    }
}
//    < partId id = "38" />

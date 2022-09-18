using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace VaporStore.DataProcessor.Dto.Export
{
    [XmlType("Game")]
    public class GameXmlExportModel
    {
        [XmlAttribute("title")]
        public string Name { get; set; }
        public string Genre { get; set; }
        public decimal Price { get; set; }
    }
}

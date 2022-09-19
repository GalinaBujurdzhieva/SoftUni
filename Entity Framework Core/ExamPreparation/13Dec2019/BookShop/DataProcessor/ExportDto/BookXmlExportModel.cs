using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace BookShop.DataProcessor.ExportDto
{
    [XmlType("Book")]
    public class BookXmlExportModel
    {
        [XmlAttribute("Pages")]
        public int Pages { get; set; }

        public string Name { get; set; }

        public string Date { get; set; }
    }
}

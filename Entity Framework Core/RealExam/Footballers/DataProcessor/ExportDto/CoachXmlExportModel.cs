using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ExportDto
{
    [XmlType("Coach")]
    public class CoachXmlExportModel
    {
        [XmlAttribute("FootballersCount")]
        public int FootballersCount { get; set; }

        public string CoachName { get; set; }

        [XmlArray("Footballers")]
        public FootballerCoachXmlExportModel[] Footballers { get; set; }
    }
}

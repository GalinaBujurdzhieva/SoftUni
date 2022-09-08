using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    [XmlType("User")]
    public class UsersWithProductsOutputModel
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; }

        [XmlElement("lastName")]
        public string LastName { get; set; }

        [XmlElement("age")]
        public int? Age { get; set; }

        [XmlElement("SoldProducts")]
        public SoldProductsOutputModel SoldProducts { get; set; }
    }
}
//<User>
//   <firstName> Cathee </firstName>
//   <lastName> Rallings </lastName>
//   <age> 33 </age>
//   <SoldProducts>
//    <count> 9 </count>
//    <products>
//       <Product>
//         <name> Fair Foundation SPF 15</name>
//         <price>1394.24</price>
//       </Product>
//       <Product>
//         <name>IOPE RETIGEN MOISTURE TWIN CAKE NO.21</name>
//         <price>1257.71</price>
//       </Product>

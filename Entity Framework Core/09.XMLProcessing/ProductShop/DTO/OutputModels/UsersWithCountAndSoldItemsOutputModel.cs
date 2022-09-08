using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    [XmlType("Users")]
    public class UsersWithCountAndSoldItemsOutputModel
    {
        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("users")]
        public List<UsersWithProductsOutputModel> UsersWithProducts { get; set; }
    }
}
//<Users >
//  <count> 54 </count>
//  <users>
//   <User>
//     <firstName> Cathee </firstName>
//     <lastName> Rallings </lastName>
//     <age> 33 </age>
//     <SoldProducts>
//      <count> 9 </count>
//      <products>
//       <Product>
//         <name> Fair Foundation SPF 15</name>
//         <price>1394.24</price>
//       </Product>
//       <Product>
//          <name>IOPE RETIGEN MOISTURE TWIN CAKE NO.21</name>
//          <price>1257.71</price>
//       </Product>
//       <Product>
//          <name>ESIKA</name>
//          <price>879.37</price>

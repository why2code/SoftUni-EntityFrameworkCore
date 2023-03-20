using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;

namespace CarDealer.DTOs.Export
{
    [XmlType("sale")]
    public class ExportCarSalesWithDiscNondiscPrices
    {
        [XmlElement("car")] 
        public ExportCardDTOEmbedded Car { get; set; }

        [XmlElement("discount")] 
        public string Discount { get; set; }

        [XmlElement("customer-name")] 
        public string CustomerName { get; set; } = null!;

        
        [XmlElement("price")] 
        public string Price { get; set; }


        [XmlElement("price-with-discount")] 
        public double PriceDiscounted { get; set; }


    }
}

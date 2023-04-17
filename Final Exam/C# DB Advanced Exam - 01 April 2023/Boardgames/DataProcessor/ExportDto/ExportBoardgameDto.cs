using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ExportDto
{
    [XmlType("Boardgame")]
    public class ExportBoardgameDto
    {
        
        [Required]
        [MaxLength(20)]
        [MinLength(10)]
        [XmlElement("BoardgameName")]
        public string Name { get; set; } = null!;

        [Required]
        [Range(2018, 2023)]
        [XmlElement("BoardgameYearPublished")]
        public int YearPublished { get; set; }
    }
}

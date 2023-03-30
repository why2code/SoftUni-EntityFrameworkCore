using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ImportDto
{
    [XmlType("Project")]
    public class ImportProjectDto
    {
        [Required]
        [MaxLength(40)]
        [MinLength(2)]
        [XmlElement("Name")]
        public string Name { get; set; } = null!;

        [XmlElement("OpenDate")]
        [Required] 
        public string OpenDate { get; set; } = null!;

        [XmlElement("DueDate")]
        public string? DueDate { get; set; }

        
        [XmlArray("Tasks")] 
        public ImportTaskDto[] Tasks { get; set; }
    }
}

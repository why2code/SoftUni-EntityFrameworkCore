using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ImportDto
{
    [XmlType("Task")]
    public class ImportTaskDto
    {
        [Required]
        [MaxLength(40)]
        [MinLength(2)]
        [XmlElement("")]
        public string Name { get; set; } = null!;

        
        [Required]
        [XmlElement("OpenDate")]
        public string OpenDate { get; set; } = null!;

        [Required]
        [XmlElement("DueDate")]
        public string DueDate { get; set; } = null!;

        [Required]
        [XmlElement("ExecutionType")]
        public int ExecutionType { get; set; }

        [Required]
        [XmlElement("LabelType")]
        public int LabelType { get; set; }
    }
}

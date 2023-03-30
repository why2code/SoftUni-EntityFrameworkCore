using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ExportDto
{
    [XmlType("Project")]
    public class ExportProjectDto
    {
        [Required]
        [XmlAttribute("TasksCount")]
        public int TaskCount { get; set; }

        [Required]
        [MaxLength(40)]
        [MinLength(2)]
        [XmlElement("ProjectName")]
        public string Name { get; set; } = null!;

        [Required]
        [XmlElement("HasEndDate")]
        public string HasDate { get; set; } = null!;


        [XmlArray("Tasks")]
        public ExportTaskDto[] Tasks { get; set; }
    }
}

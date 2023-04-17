﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Theatre.Data.Models.Enums;

namespace Theatre.DataProcessor.ImportDto
{
    [XmlType("Play")]
    public class ImportPlaysDto
    {
        [Required]
        [MaxLength(50)]
        [MinLength((4))]
        [XmlElement("Title")]
        public string Title { get; set; } = null!;

        [Required]
        [XmlElement("Duration")]
        public string Duration { get; set; } = null!;

        [Required]
        [XmlElement("Raiting")]
        [Range(0, 10)]
        public float Rating { get; set; }

        [Required]
        [XmlElement("Genre")]
        public string Genre { get; set; } = null!;

        [Required]
        [MaxLength(700)]
        [XmlElement("Description")]
        public string Description { get; set; } = null!;

        [Required]
        [MaxLength(30)]
        [MinLength(4)]
        [XmlElement("Screenwriter")]
        public string Screenwriter { get; set; } = null!;
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ExportDto
{
    [XmlType("Creator")]
    public class ExportCreatorDto
    {
        [XmlAttribute("BoardgamesCount")]
        public int BoardGamesCount { get; set; }

        [XmlElement("CreatorName")]
        public string CreatorFullName { get; set; }


        [XmlArray("Boardgames")]
        public ExportBoardgameDto[] BoardGames { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicHub.Data.Models.Enums;

namespace MusicHub.Data.Models
{
    public class Song
    {
        public Song()
        {
            this.SongPerformers = new HashSet<SongPerformer>();
        }
        public int Id { get; set; }

        [Required]
        [MaxLength(CommonConfigs.SongNameLength)]
        public string Name { get; set; } = null!;

        public TimeSpan Duration { get; set; }

        public DateTime CreatedOn { get; set; }

        [Required]
        public Genre Genre { get; set; }


        [ForeignKey(nameof(Album))]
        public int? AlbumId { get; set; }
        public virtual Album? Album { get; set; }


        [ForeignKey(nameof(Writer))]
        public int WriterId { get; set; }
        public virtual Writer Writer { get; set; } = null!;

        public decimal Price { get; set; }

        public virtual ICollection<SongPerformer> SongPerformers { get; set; } = null!;

    }
}

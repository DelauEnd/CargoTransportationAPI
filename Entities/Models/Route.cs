using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Entities.Models
{
    public class Route : Entity
    {
        [Key]
        [Column("RouteId")]
        public int Id { get; set; }

        [Required(ErrorMessage = "TransportId - required field")]
        [ForeignKey(nameof(Transport))]
        public int TransportId { get; set; }

        public Transport Transport { get; set; }

        public List<Cargo> Cargoes { get; set; }
    }
}

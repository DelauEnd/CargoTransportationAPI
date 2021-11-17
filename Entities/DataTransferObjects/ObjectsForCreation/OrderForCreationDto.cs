using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.DataTransferObjects
{
    public class OrderForCreationDto
    {
        [Required(ErrorMessage = "DestinationId - required field")]
        public int SenderId { get; set; }

        [Required(ErrorMessage = "DestinationId - required field")]
        public int DestinationId { get; set; }

        public IEnumerable<CargoForCreationDto> Cargoes { get; set; }
    }
}

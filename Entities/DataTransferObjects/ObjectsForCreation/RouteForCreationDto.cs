using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.DataTransferObjects
{
    public class RouteForCreationDto
    {
        [Required(ErrorMessage = "TransportId - required field")]
        public string TransportRegistrationNumber { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public class RouteForCreationDto
    {
        [Required(ErrorMessage = "TransportId - required field")]
        public string TransportRegistrationNumber { get; set; }
    }
}

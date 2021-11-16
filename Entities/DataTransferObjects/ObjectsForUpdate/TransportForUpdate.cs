using Entities.Models;
using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects.ObjectsForUpdate
{
    public class TransportForUpdate
    {
        [MaxLength(30, ErrorMessage = "RegistrationNumber max length - 30 simbols.")]
        public string RegistrationNumber { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "LoadCapacity - required field and can not be less then 0")]
        public double LoadCapacity { get; set; }

        public Person Driver { get; set; }
    }
}

using Entities.Models;
using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects.ObjectsForUpdate
{
    public class CustomerForUpdateDto
    {
        [MaxLength(30, ErrorMessage = "Address max length - 30 simbols.")]
        public string Address { get; set; }

        public Person ContactPerson { get; set; }
    }
}

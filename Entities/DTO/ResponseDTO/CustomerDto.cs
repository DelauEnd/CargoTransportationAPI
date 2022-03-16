using Entities.Models;

namespace Entities.DataTransferObjects
{
    public class CustomerDto
    {
        public int Id { get; set; }

        public string Address { get; set; }

        public Person ContactPerson { get; set; }
    }
}

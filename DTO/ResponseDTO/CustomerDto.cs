using DTO.OwnedModels;

namespace DTO.ResponseDTO
{
    public class CustomerDto
    {
        public int Id { get; set; }

        public string Address { get; set; }

        public Person ContactPerson { get; set; }
    }
}

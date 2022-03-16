using DTO.OwnedModels;

namespace DTO.ResponseDTO
{
    public class TransportDto
    {
        public int Id { get; set; }

        public string RegistrationNumber { get; set; }

        public double LoadCapacity { get; set; }

        public Person Driver { get; set; }
    }
}

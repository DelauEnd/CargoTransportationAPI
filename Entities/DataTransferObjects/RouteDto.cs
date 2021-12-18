namespace Entities.DataTransferObjects
{
    public class RouteDto : IModelFormatter
    {
        public int Id { get; set; }

        public string TransportRegistrationNumber { get; set; }

        public string FormatToCsv()
        {
            var separator = ",\"";

            return string.Join
                (
                separator,
                Id,
                TransportRegistrationNumber
                );
        }
    }
}

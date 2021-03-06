namespace Entities.DataTransferObjects
{
    public class OrderDto : IModelFormatter
    {
        public int Id { get; set; }

        public string Status { get; set; }


        public string Sender { get; set; }


        public string Destination { get; set; }


        public string FormatToCsv()
        {
            var separator = ",\"";

            return string.Join
            (
                separator,
                Id,
                Status,
                Sender,
                Destination
            );
        }
    }
}

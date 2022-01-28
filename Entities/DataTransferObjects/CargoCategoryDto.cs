using System;

namespace Entities.DataTransferObjects
{
    public class CargoCategoryDto : IModelFormatter
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string FormatToCsv()
        {
            var separator = ",\"";

            return string.Join
            (
                separator,
                Id,
                Title
            );
        }
    }
}

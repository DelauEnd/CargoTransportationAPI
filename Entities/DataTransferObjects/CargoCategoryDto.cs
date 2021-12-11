using System;

namespace Entities.DataTransferObjects
{
    public class CargoCategoryDto : IModelFormatter
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string FormatToCsv()
        {
            throw new NotImplementedException();
        }
    }
}

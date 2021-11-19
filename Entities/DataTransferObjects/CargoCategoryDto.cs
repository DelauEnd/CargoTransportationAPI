using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects
{
    public class CargoCategoryDto : Dto, IModelFormatter
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string FormatToCsv()
        {
            throw new NotImplementedException();
        }
    }
}

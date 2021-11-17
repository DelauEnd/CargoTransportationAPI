using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.DataTransferObjects.ObjectsForUpdate
{
    public class CargoCategoryForUpdateDto
    {
        [MaxLength(30, ErrorMessage = "Title max length - 30 simbols.")]
        public string Title { get; set; }
    }
}

using Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.DataTransferObjects.ObjectsForUpdate
{
    public class CustomerForUpdate
    {
        [MaxLength(30, ErrorMessage = "Address max length - 30 simbols.")]
        public string Address { get; set; }

        public Person ContactPerson { get; set; }
    }
}

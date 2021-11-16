using Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.DataTransferObjects
{
    public class CustomerForCreation
    {
        [Required(ErrorMessage = "Address - required field")]
        [MaxLength(30, ErrorMessage = "RegistrationNumber max length - 30 simbols.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "ContactPerson - required fields")]
        public Person ContactPerson { get; set; }
    }
}

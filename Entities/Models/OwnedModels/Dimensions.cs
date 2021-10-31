using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities.Models
{
    [Owned]
    public class Dimensions
    {
        [Required(ErrorMessage = "Long - required field")]
        public double Length { get; set; }

        [Required(ErrorMessage = "Height - required field")]
        public double Height { get; set; }

        [Required(ErrorMessage = "Width - required field")]
        public double Width { get; set; }
    }
}

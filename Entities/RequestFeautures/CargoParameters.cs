﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.RequestFeautures
{
    public class CargoParameters : RequestParameters
    {
        public DateTime ArrivalDateFrom { get; set; } = DateTime.MinValue;
        public DateTime ArrivalDateTo { get; set; } = DateTime.MaxValue;

        public DateTime DepartureDateFrom { get; set; } = DateTime.MinValue;
        public DateTime DepartureDateTo { get; set; } = DateTime.MaxValue;

        public bool IsValid()
        {
            if (!IsValidDateFilter())
                return false;

            return true;
        }

        public bool IsValidDateFilter()
        {
            if (ArrivalDateFrom > ArrivalDateTo)
                return false;
            if (DepartureDateFrom > DepartureDateTo)
                return false;
            return true;
        }
    }
}

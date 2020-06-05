using System;
using System.Collections.Generic;

namespace PrimeService.Domain
{
    public class DeskBookingResult : DeskBookingBase
    {
        public DeskBookingResultCode Code { get; set; }
    }
}
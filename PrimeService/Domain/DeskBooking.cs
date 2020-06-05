using System.Collections.Generic;

namespace PrimeService.Domain
{
    public class DeskBooking : DeskBookingBase
    {
        public int? Id { get; set; }
        public int DeskId { get; set; }
    }
}
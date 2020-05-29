using System;
using System.Collections.Generic;
using System.Text;
using PrimeService.Domain;

namespace PrimeService.DataInterface
{
    public interface IDeskBookingRepository
    {
        void Save(DeskBooking deskBooking);
    }
}
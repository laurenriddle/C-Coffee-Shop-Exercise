using System;
using System.Linq;
using PrimeService.DataInterface;
using PrimeService.Domain;

namespace PrimeService.Processor
{
    public interface IDeskBookingRequestProcessor
    {
        DeskBookingResult BookDesk(DeskBookingRequest request);
    }

}
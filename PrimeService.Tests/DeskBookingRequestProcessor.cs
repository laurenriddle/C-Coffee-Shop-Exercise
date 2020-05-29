using System;

namespace PrimeService.Tests
{
    internal class DeskBookingRequestProcessor
    {
        public DeskBookingRequestProcessor()
        {
        }

        internal DeskBookingResult BookDesk(DeskBookingRequest request)
        {
            return new DeskBookingResult
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Date = request.Date,
                
            };
        }

    }
}
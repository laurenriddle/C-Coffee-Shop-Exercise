using System;
using Xunit;

namespace PrimeService.Tests
{
    public class DeskBookingRequestProcessorTests
    {
        [Fact]
        public void ShouldReturnDeskBookingResultsWithRequestValues()
        {
            var request = new DeskBookingRequest
            {
                FirstName = "Thomas",
                LastName = "Huber",
                Email= "thomas@thomas.com",
                Date = new DateTime(2020, 1, 28)
            };
            var processor = new DeskBookingRequestProcessor();
            DeskBookingResult result = processor.BookDesk(request);

            Assert.NotNull(result);
            Assert.Equal(request.FirstName, result.FirstName);
            Assert.Equal(request.LastName, result.LastName);
            Assert.Equal(request.Email, result.FirstName);
            Assert.Equal(request.Date, result.Date);
        }
    }
}

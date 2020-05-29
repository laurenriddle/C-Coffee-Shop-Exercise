using System;
using PrimeService.Domain;
using PrimeService.Processor;
using Xunit;

namespace PrimeService.Tests
{
    public class DeskBookingRequestProcessorTests
    {
        private DeskBookingRequestProcessor _processor;

        public DeskBookingRequestProcessorTests()
        {
            _processor = new DeskBookingRequestProcessor();
            
        }

        [Fact]
        public void ShouldReturnDeskBookingResultsWithRequestValues()
        {
            var request = new DeskBookingRequest
            {
                FirstName = "Thomas",
                LastName = "Huber",
                Email = "thomas@thomas.com",
                Date = new DateTime(2020, 1, 28)
            };
            // var processor = new DeskBookingRequestProcessor();
            DeskBookingResult result = _processor.BookDesk(request);

            Assert.NotNull(result);
            Assert.Equal(request.FirstName, result.FirstName);
            Assert.Equal(request.LastName, result.LastName);
            Assert.Equal(request.Email, result.Email);
            Assert.Equal(request.Date, result.Date);
        }
        [Fact]
        public void ShouldThrowExceptionIfRequestIsNull()
        {
            // var processor = new DeskBookingRequestProcessor();

            var exception = Assert.Throws<ArgumentNullException>(() => _processor.BookDesk(null));

            Assert.Equal("request", exception.ParamName);
        }
    }
}



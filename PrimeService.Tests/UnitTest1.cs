using System;
using PrimeService.DataInterface;
using PrimeService.Domain;
using PrimeService.Processor;
using Xunit;
using Moq;

namespace PrimeService.Tests
{
    public class DeskBookingRequestProcessorTests
    {
        private DeskBookingRequestProcessor _processor;
        private DeskBookingRequest _request;
        private Mock<IDeskBookingRepository> _deskBookingRepositoryMock;

        public DeskBookingRequestProcessorTests()
        {
            _processor = new DeskBookingRequestProcessor();
            _request = new DeskBookingRequest
            {
                FirstName = "Thomas",
                LastName = "Huber",
                Email = "thomas@thomas.com",
                Date = new DateTime(2020, 1, 28)
            };
            _deskBookingRepositoryMock = new Mock<IDeskBookingRepository>();
            
        }

        [Fact]
        public void ShouldReturnDeskBookingResultsWithRequestValues()
        {
            
            DeskBookingResult result = _processor.BookDesk(_request);

            Assert.NotNull(result);
            Assert.Equal(_request.FirstName, result.FirstName);
            Assert.Equal(_request.LastName, result.LastName);
            Assert.Equal(_request.Email, result.Email);
            Assert.Equal(_request.Date, result.Date);
        }
        [Fact]
        public void ShouldThrowExceptionIfRequestIsNull()
        {

            var exception = Assert.Throws<ArgumentNullException>(() => _processor.BookDesk(null));

            Assert.Equal("request", exception.ParamName);
        }

        [Fact]
        public void ShouldSaveDeskBooking()
        {
            _processor.BookDesk(_request);

        }
    }
}



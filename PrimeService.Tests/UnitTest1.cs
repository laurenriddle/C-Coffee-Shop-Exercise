using System;
using PrimeService.DataInterface;
using PrimeService.Domain;
using PrimeService.Processor;
using Xunit;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace PrimeService.Tests
{
    public class DeskBookingRequestProcessorTests
    {
        private DeskBookingRequestProcessor _processor;
        private DeskBookingRequest _request;
        private List<Desk> _availableDesks;
        private Mock<IDeskBookingRepository> _deskBookingRepositoryMock;
        private Mock<IDeskRepository> _deskRepositoryMock;

        public DeskBookingRequestProcessorTests()
        {
            // Setup for the tests below
            _request = new DeskBookingRequest
            {
                // instance of a desk booking request
                FirstName = "Thomas",
                LastName = "Huber",
                Email = "thomas@thomas.com",
                Date = new DateTime(2020, 1, 28)
            };
            // list of available desks
            _availableDesks = new List<Desk> { new Desk { Id = 7 } };
            _deskBookingRepositoryMock = new Mock<IDeskBookingRepository>();
            _deskRepositoryMock = new Mock<IDeskRepository>();

            // retrieve the available desks for the requested date
            _deskRepositoryMock.Setup(x => x.GetAvailableDesks(_request.Date))
            .Returns(_availableDesks);

            _processor = new DeskBookingRequestProcessor(_deskBookingRepositoryMock.Object, _deskRepositoryMock.Object);

        }

        [Fact]
        public void ShouldReturnDeskBookingResultsWithRequestValues()
        {
            // books a desk
            DeskBookingResult result = _processor.BookDesk(_request);

            // checks to make sure desk is booked
            Assert.NotNull(result);
            Assert.Equal(_request.FirstName, result.FirstName);
            Assert.Equal(_request.LastName, result.LastName);
            Assert.Equal(_request.Email, result.Email);
            Assert.Equal(_request.Date, result.Date);
        }
        [Fact]
        public void ShouldThrowExceptionIfRequestIsNull()
        {
            // defines the exception when you pass in null instead of request. 
            var exception = Assert.Throws<ArgumentNullException>(() => _processor.BookDesk(null));

            Assert.Equal("request", exception.ParamName);
        }

        [Fact]
        public void ShouldSaveDeskBooking()
        {
            DeskBooking savedDeskBooking = null;
            // checks to make sure this is a deskbooking 
            _deskBookingRepositoryMock.Setup(x => x.Save(It.IsAny<DeskBooking>()))
        
            .Callback<DeskBooking>(deskBooking =>
            {
                savedDeskBooking = deskBooking;
            });

            _processor.BookDesk(_request);
            // verifies the saved object is a desk booking
            _deskBookingRepositoryMock.Verify(x => x.Save(It.IsAny<DeskBooking>()), Times.Once);

            Assert.NotNull(savedDeskBooking);

            Assert.Equal(_request.FirstName, savedDeskBooking.FirstName);
            Assert.Equal(_request.LastName, savedDeskBooking.LastName);
            Assert.Equal(_request.Email, savedDeskBooking.Email);
            Assert.Equal(_request.Date, savedDeskBooking.Date);
            Assert.Equal(_availableDesks.First().Id, savedDeskBooking.DeskId);

        }

        [Fact]
        public void ShouldNotSaveDeskBookingIfNoDeskIsAvailable()
        {
            _availableDesks.Clear();

            _processor.BookDesk(_request);

            _deskBookingRepositoryMock.Verify(x => x.Save(It.IsAny<DeskBooking>()), Times.Never);

        }

        [Theory]
        [InlineData(DeskBookingResultCode.Successs, true)]
        [InlineData(DeskBookingResultCode.NoDeskAvailable, false)]
        public void ShouldReturnExpectedResultCode(DeskBookingResultCode expectedResultCode, bool isDeskAvailable)
        {

            if (!isDeskAvailable)
            {
                _availableDesks.Clear();
            }

            var result = _processor.BookDesk(_request);

            Assert.Equal(expectedResultCode, result.Code);

        }

        [Theory]
        [InlineData(5, true)]
        [InlineData(null, false)]
        public void ShouldReturnExpectedDeskBookingId(int? expectedDeskBookingId, bool isDeskAvailable)
        {

            if (!isDeskAvailable)
            {
                _availableDesks.Clear();
            }
            else
            {
                _deskBookingRepositoryMock.Setup(x => x.Save(It.IsAny<DeskBooking>()))
                .Callback<DeskBooking>(deskbooking =>
                {
                    deskbooking.Id = expectedDeskBookingId;
                });
            }

            var result = _processor.BookDesk(_request);

            Assert.Equal(expectedDeskBookingId, result.DeskBookingId);

        }

        // [Fact]
        // public void ShouldCallBookDeskMethodOfProcessor()
        // {
        //     var processorMock = new Mock<IDeskBookingRequestProcessor>();

        //     var bookDeskModel = new BookDeskModel(processorMock.Object)
        //     {
        //         DeskBookingRequest = new DeskBookingRequest()
        //     };

        //     bookDeskModel.OnPost();

        //     processorMock.Verify(x => x.BookDesk(bookDeskModel.DeskBookingRequest), Times.Once);


        // }


        // [Fact]
        // public void ShouldAddModelErrorIfNoDeskIsAvailable()
        // {
        //     var processorMock = new Mock<IDeskBookingRequestProcessor>();

        //     var bookDeskModel = new BookDeskModel(processorMock.Object)
        //     {
        //         DeskBookingRequest = new DeskBookingRequest()
        //     };

        //     processorMock.Setup(x => x.BookDesk(bookDeskModel.DeskBookingRequest))
        //     .Returns(new DeskBookingResult 
        //     {
        //         Code = DeskBookingResultCode.NoDeskAvailable
        //     });

             
        //     bookDeskModel.OnPost();

        //     var modelStateEntry = Assert.Contains("DeskBookingRequest.Date", bookDeskModel.ModelState);

        //     var modelError = Assert.Single(modelStateEntry.Errors);

        //     Assert.Equal("No desk available for selected date.", modelError.ErrorMessage);

        // }
    }
}



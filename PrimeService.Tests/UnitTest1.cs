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
        private DeskBookingResult _deskBookingResult;
        private Mock<IDeskBookingRequestProcessor> _processorMock;
        private BookDeskModel _bookDeskModel;
        private int expectedBookDeskCalls;

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

        // public BookDeskModelTests()
        // {
        //     _processorMock = new Mock<IDeskBookingRequestProcessor>();

        //     _bookDeskModel = new BookDeskModel(_processorMock.Object)
        //     {
        //         DeskBookingRequest = new DeskBookingRequest()
        //     };

        //     _deskBookingResult = new DeskBookingResult
        //     {
        //         Code = DeskBookingResultCode.Successs
        //     };


        //     _processorMock.Setup(x => x.BookDesk(_bookDeskModel.DeskBookingRequest))
        //     .Returns(_deskBookingResult);
        // }

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

        // [Theory]
        // [InlineData(1, true)]
        // [InlineData(0, false)]
        // public void ShouldCallBookDeskMethodOfProcessorIfModelIsValid(int? expectedDeskBookingId, bool isModelValid)
        // {
        //     if(!isModelValid)
        //     {
        //         _bookDeskModel.ModelState.AddModelError("JustAKey", "AnErrorMessage")
        //     }

        //     _bookDeskModel.OnPost();

        //     _processorMock.Verify(x => x.BookDesk(_bookDeskModel.DeskBookingRequest), Times.Exactly(expectedBookDeskCalls));


        // }

        // [Fact]
        // public void ShouldCallBookDeskMethodOfProcessor()
        // {
      
        //     _bookDeskModel.OnPost();

        //     processorMock.Verify(x => x.BookDesk(_bookDeskModel.DeskBookingRequest), Times.Once);


        // }


        // [Fact]
        // public void ShouldAddModelErrorIfNoDeskIsAvailable()
        // {

        //     _deskBookingResult.Code = DeskBookingResultCode.NoDeskAvailable;

        //     _bookDeskModel.OnPost();

        //     var modelStateEntry = Assert.Contains("DeskBookingRequest.Date", _bookDeskModel.ModelState);

        //     var modelError = Assert.Single(modelStateEntry.Errors);

        //     Assert.Equal("No desk available for selected date.", modelError.ErrorMessage);

        // }

        // [Fact]
        // public void ShouldNotAddModelErrorIfDeskIsAvailable()
        // {
        //      _deskBookingResult.Code = DeskBookingResultCode.NoDeskAvailable;


        //     _bookDeskModel.OnPost();

        //     Assert.DoesNotContain("DeskBookingRequest.Date", _bookDeskModel.ModelState);


        // }

        // [Theory]
        // [InlineData(typeof(PageResult), false, null)]
        // [InlineData(typeof(PageResult), true, DeskBookingResultCode.NoDeskAvailable)]
        // [InlineData(typeof(RedirectToPageResult), true, DeskBookingResultCode.Successs)]
        // public void ShouldReturnExpectedActionResult(Type expectedActionResultType, bool isModelValid, DeskBookingResultCode? DeskBookingResultCode)
        // {
        //     // Arrange
        //     if(!isModelValid)
        //     {
        //         _bookDeskModel.ModelState.AddModelError("JustAKey", "AnErrorMessage");

        //     }

        //     if(DeskBookingResultCode.HasValue)
        //     {
        //         _deskBookingResult.Code = DeskBookingResultCode.Value;
        //     }

        //     // Act
        //     IActionResult actionResult = _bookDeskModel.OnPost();


        //     //Assert
        //     Assert.IsType(expectedActionResultType, actionResult)
        // }


        [Fact]
        public void ShouldRedirectToBookDeskConfirmationPage()
        {
        //Given
        
        //When
        
        //Then
        }
    }
}



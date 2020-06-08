using PrimeService.Domain;
using PrimeService.Processor;

namespace PrimeService.Tests
{
    internal class BookDeskModel
    {
        private IDeskBookingRequestProcessor @object;

        public BookDeskModel(IDeskBookingRequestProcessor @object)
        {
            this.@object = @object;
        }

        public DeskBookingRequest DeskBookingRequest { get; set; }
    }
}
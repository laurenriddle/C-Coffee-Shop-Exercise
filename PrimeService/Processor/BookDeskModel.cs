using PrimeService.Domain;
using PrimeService.Processor;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PrimeService.Processor
{
    public class BookDeskModel // : PageModel
    {
        private IDeskBookingRequestProcessor _deskBookingRequestProcessor;

        public BookDeskModel(IDeskBookingRequestProcessor deskBookingRequestProcessor)
        {
            _deskBookingRequestProcessor = deskBookingRequestProcessor;
        }

        public DeskBookingRequest DeskBookingRequest { get; set; }

        public void OnPost()
        {
            // if (ModelState.IsValid)
            // {
            //     var result = _deskBookingRequestProcessor.BookDesk(DeskBookingRequest);
            //     if (result.Code == DeskBookingResultCode.NoDeskAvailable)
            //     {
            //         ModelState.AddModelError("DeskBookingRequest.Date", "No desk available for selected date.");
            //     }
            // }
        }
    }
}
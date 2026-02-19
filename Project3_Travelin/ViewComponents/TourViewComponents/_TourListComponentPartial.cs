using Microsoft.AspNetCore.Mvc;
using Project3_Travelin.Services.TourServices;

namespace Project3_Travelin.ViewComponents.TourViewComponents
{
    public class _TourListComponentPartial:ViewComponent
    {
        private readonly ITourService _tourService;

        public _TourListComponentPartial(ITourService tourService)
        {
            _tourService = tourService;
        }

        public async Task <IViewComponentResult> InvokeAsync()
        {
            var values = await _tourService.GetAllTourAsync();
            return View(values);
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace Project3_Travelin.ViewComponents.TourViewComponents
{
    public class _TourHeadComponentPartial :ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View(); 
        }
    }
}

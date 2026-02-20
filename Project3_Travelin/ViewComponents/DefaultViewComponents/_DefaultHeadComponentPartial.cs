using Microsoft.AspNetCore.Mvc;

namespace Project3_Travelin.ViewComponents.HomeViewComponent
{
    public class _DefaultHeadComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace Project3_Travelin.ViewComponents.DefaultViewComponents
{
    public class _DefaultHeaderComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace Project3_Travelin.ViewComponents.HomeViewComponents
{
    public class _HowItWorksComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View(); 
        }
    }
}

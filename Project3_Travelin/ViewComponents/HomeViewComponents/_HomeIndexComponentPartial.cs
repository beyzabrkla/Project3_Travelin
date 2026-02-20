using Microsoft.AspNetCore.Mvc;

namespace Project3_Travelin.ViewComponents.HomeViewComponents
{
    public class _HomeIndexComponentPartial :ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View(); 
        }
    }
}

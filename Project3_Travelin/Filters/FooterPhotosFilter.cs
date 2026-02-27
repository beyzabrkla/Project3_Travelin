using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Project3_Travelin.Filters
{
    public class FooterPhotosFilter : IAsyncActionFilter
    {
        private readonly ITourService _tourService;

        public FooterPhotosFilter(ITourService tourService)
        {
            _tourService = tourService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Action çalışmadan önce footer fotoğraflarını hazırla
            if (context.Controller is Controller controller)
            {
                var allTours = await _tourService.GetAllTourAsync();

                // Sadece aktif turların ImageUrl'leri
                var footerPhotos = allTours
                    .Where(x => x.IsStatus == true && x.IsDrafts == false && !string.IsNullOrEmpty(x.ImageUrl))
                    .Select(x =>
                    {
                        var url = x.ImageUrl.Trim();
                        return (url.StartsWith("http://") || url.StartsWith("https://") || url.StartsWith("/"))
                            ? url
                            : "/userimages/" + url;
                    })
                    .OrderBy(_ => Guid.NewGuid())
                    .Take(12)
                    .ToList();

                controller.ViewBag.FooterPhotos = footerPhotos;
            }

            await next();
        }
    }
}
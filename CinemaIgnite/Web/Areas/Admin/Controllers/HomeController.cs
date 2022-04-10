using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;

namespace Web.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IHtmlLocalizer<HomeController> localizer;

        public HomeController(IHtmlLocalizer<HomeController> localizer)
        {
            this.localizer = localizer;
        }

        public IActionResult Index()
        {
            var adminWelcome = localizer["AdminWelcome"];
            var areaSummary = localizer["AreaSummary"];
            var pageTitle = localizer["PageTitle"];
            ViewData["AdminWelcome"] = adminWelcome;
            ViewData["AreaSummary"] = areaSummary;
            ViewData["PageTitle"] = pageTitle;

            return View();
        }
    }
}

using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers
{
    [Authorize(Roles = RoleConstants.Administrator)]
    [Area(RoleConstants.AdminArea)]
    public class BaseController : Controller
    {
        [HttpPost]
        public IActionResult SetLanguage(string language)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(language)),
                new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddDays(30),
                    SameSite = SameSiteMode.Strict
                });

            string url = Request.Headers["Referer"].ToString();

            return Redirect(url);
        }
    }
}

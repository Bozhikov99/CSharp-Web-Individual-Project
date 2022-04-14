using Common.TemplateConstants;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;

namespace Web.Controllers
{
    public class BaseController : Controller
    {

        public BaseController()
        {
        }

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace WebUI.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        [AcceptVerbs(HttpVerbs.Get)]
        public ViewResult Login()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Login(string name, string password, string returnUrl)
        {
            if (FormsAuthentication.Authenticate(name, password))
            {
                returnUrl = returnUrl ?? Url.Action("Index", "Admin");
                FormsAuthentication.SetAuthCookie(name, false);
                return Redirect(returnUrl);
            }
            else
            {
                ViewBag.LastLoginFailed = true;
                return View();
            }
        }

    }
}

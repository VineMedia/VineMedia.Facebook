using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace VineMedia.Facebook.Web.Controllers
{
    public class AccountController : Controller
    {
		public IFacebookAuthenticationProvider FacebookAuthProvider { get; set; }

        public ActionResult Logout()
        {
			FormsAuthentication.SignOut();
			return RedirectToAction("Index", "Home");
        }

		public ActionResult Login()
		{
			FacebookAuthProvider.Authenticate();
			return new EmptyResult();
		}
    }
}

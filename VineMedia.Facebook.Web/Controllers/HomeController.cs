using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VineMedia.Facebook.Web.Controllers
{
    public class HomeController : Controller
    {
		public IFacebookAuthProvider AuthProvider { get; set; }

        public ActionResult Index()
        {

			if (!Request.IsAuthenticated)
			{
				var currentUser = AuthProvider.GetCurrentUser();
			}

            return View();
        }

    }
}

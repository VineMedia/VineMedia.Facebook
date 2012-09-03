using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VineMedia.Facebook.Web.Controllers
{
	[Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
			if (Profile != null)
			{
				ViewBag.FirstName = ((ProfileCommon)Profile).FirstName;
			}

            return View();
        }

    }
}

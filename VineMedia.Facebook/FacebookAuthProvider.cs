using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using Facebook;
using VineMedia.Facebook.Properties;

namespace VineMedia.Facebook
{
	public class FacebookAuthProvider : IFacebookAuthProvider
	{
		public HttpContextBase HttpContext { get; set; }

		private const string OAuthUrl = "https://www.facebook.com/dialog/oauth?client_id={0}&redirect_uri={1}&scope={2}&state={3}";
		internal const string StateCookie = "fsc";
		internal const string ReturnCookie = "frc";
		internal const string TokenCookie = "ftc";

		public ICurrentUser GetCurrentUser()
		{
			if (!HttpContext.Request.IsAuthenticated)
			{
				var state = Guid.NewGuid();
				HttpContext.Response.Cookies.Add(new HttpCookie(StateCookie, state.ToString()) { Expires = DateTime.Now.AddSeconds(30) });
				HttpContext.Response.Cookies.Add(new HttpCookie(ReturnCookie, HttpContext.Request.Url.ToString()) { Expires = DateTime.Now.AddSeconds(30) });

				var url = string.Format(OAuthUrl, Settings.Default.FacebookAppId, "http://facebook.dev.vinemedia.com.au/facebookouth.ashx", "", state);
				HttpContext.Response.Redirect(url, false);
			}

			return new CurrentUser();
		}
	}

	public interface IFacebookAuthProvider 
	{
		ICurrentUser GetCurrentUser();
	}
}

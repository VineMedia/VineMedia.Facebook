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
	public class FacebookAuthenticationProvider : IFacebookAuthenticationProvider
	{
		public HttpContextBase HttpContext { get; set; }

		private const string OAuthUrl = "https://www.facebook.com/dialog/oauth?client_id={0}&redirect_uri={1}/facebookouth.axd&scope={2}&state={3}";
		internal const string StateCookieName = "FSC-50AD5C44-8BF2-4B4A-B87A-2B39FBF781EF";
		private const int StateCookieExpirySeconds = 30;

		public void Authenticate()
		{
			if (!HttpContext.Request.IsAuthenticated)
			{
				RedirectToFacebook();
			}			
		}

		private void RedirectToFacebook()
		{
			var state = Guid.NewGuid();
			HttpContext.Response.Cookies.Add(new HttpCookie(StateCookieName, state.ToString()) { Expires = DateTime.Now.AddSeconds(StateCookieExpirySeconds) });

			var url = string.Format(OAuthUrl, Settings.Default.FacebookAppId, HttpContext.Request.Url.GetLeftPart(UriPartial.Authority), "email", state);
			HttpContext.Response.Redirect(url, false);
		}
	}

	public interface IFacebookAuthenticationProvider 
	{
		void Authenticate();
	}
}

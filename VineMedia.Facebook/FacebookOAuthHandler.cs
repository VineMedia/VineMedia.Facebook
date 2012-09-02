using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using Facebook;
using VineMedia.Facebook.Properties;

namespace VineMedia.Facebook
{
	public class FacebookOAuthHandler : IHttpHandler
	{
		public bool IsReusable
		{
			get { return false; }
		}

		public void ProcessRequest(HttpContext context)
		{
			var stateCookie = context.Request.Cookies[FacebookAuthProvider.StateCookie];
			Guid cookieStateValue;
			Guid querystringStateValue;
			FacebookOAuthResult facebookResult;

			var client = new FacebookClient();
			

			if (stateCookie == null  
				|| !Guid.TryParse(stateCookie.Value, out cookieStateValue)
				|| !client.TryParseOAuthCallbackUrl(context.Request.Url, out facebookResult)
				|| !Guid.TryParse(facebookResult.State, out querystringStateValue)
				|| cookieStateValue != querystringStateValue)
			{
				throw new AuthenticationException();
			}

			var url = string.Format("https://graph.facebook.com/oauth/access_token?client_id={0}&redirect_uri={1}&client_secret={2}&code={3}",
				Settings.Default.FacebookAppId, "http://facebook.dev.vinemedia.com.au/facebookouth.ashx", Settings.Default.FacebookAppSecret, facebookResult.Code);
			var response = new WebClient().DownloadString(new Uri(url));

			//var u = new Uri("http://facebook.dev.vinemedia.com.au/facebookouth.ashx?" + response);
			var p = HttpUtility.ParseQueryString(response);

			var token = p["access_token"];
			var expires = int.Parse(p["expires"]);

			context.Response.Cookies.Add(new HttpCookie(FacebookAuthProvider.TokenCookie, p["access_token"]) { Expires = DateTime.Now.AddSeconds(expires) });
			client.AccessToken = token;
			dynamic me = client.Get("me");

			FormsAuthentication.SetAuthCookie(me.username, true);

			string redirectUrl = context.Request.Cookies[FacebookAuthProvider.ReturnCookie] == null ? context.Request.Cookies[FacebookAuthProvider.ReturnCookie].Value : null;

			context.Response.Cookies[FacebookAuthProvider.StateCookie].Expires = DateTime.Now.AddSeconds(-60);
			context.Response.Cookies[FacebookAuthProvider.StateCookie].Expires = DateTime.Now.AddSeconds(-60);

			if (redirectUrl != null)
			{
				context.Response.Redirect(context.Request.Cookies[FacebookAuthProvider.ReturnCookie].Value, false);
			}


		}
	}
}

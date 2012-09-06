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
	public class FacebookAuthenticationProvider : IFacebookAuthenticationProvider
	{
		public HttpContextBase HttpContext { get; set; }
		public FacebookConfig FacebookConfig 
		{
			get 
			{
				if (facebookConfig == null)
				{
					facebookConfig = new FacebookConfig
					{
						FacebookAppId = Settings.Default.FacebookAppId,
						FacebookAppSecret = Settings.Default.FacebookAppSecret,
						OAuthCallbackPath = Settings.Default.OAuthCallbackPath
					};

					if (string.IsNullOrWhiteSpace(facebookConfig.OAuthCallbackPath))
					{
						facebookConfig.OAuthCallbackPath = "/facebookouth.axd";
					}
				}

				return facebookConfig;
			}
			set { facebookConfig = value; } 
		}
		private FacebookConfig facebookConfig;

		private const string OAuthUrl = "https://www.facebook.com/dialog/oauth?client_id={0}&redirect_uri={1}&scope={2}&state={3}";
		private const string TokenUrl = "https://graph.facebook.com/oauth/access_token?client_id={0}&redirect_uri={1}&client_secret={2}&code={3}";
		private const string StateCookieName = "FSC-50AD5C44-8BF2-4B4A-B87A-2B39FBF781EF";
		private const int StateCookieExpirySeconds = 30;

		public void Authenticate()
		{
			if (!HttpContext.Request.IsAuthenticated)
			{
				var state = Guid.NewGuid();
				HttpContext.Response.Cookies.Add(new HttpCookie(StateCookieName, state.ToString()) { Expires = DateTime.Now.AddSeconds(StateCookieExpirySeconds) });

				var url = string.Format(OAuthUrl, FacebookConfig.FacebookAppId, HttpContext.Request.Url.GetLeftPart(UriPartial.Authority), "email", state);
				HttpContext.Response.Redirect(url, false);
			}			
		}

		public FacebookOAuthResponse ParseResponse(HttpContext context)
		{
			var stateCookie = context.Request.Cookies[StateCookieName];
			context.Response.Cookies[FacebookAuthenticationProvider.StateCookieName].Expires = DateTime.Now.AddSeconds(-60);

			Guid cookieStateValue;
			Guid querystringStateValue;
			FacebookOAuthResult facebookResult;			

			var client = new FacebookClient();			

			if (stateCookie == null || !Guid.TryParse(stateCookie.Value, out cookieStateValue))
			{
				throw new AuthenticationException("State cookie not set");
			}

			if (!client.TryParseOAuthCallbackUrl(context.Request.Url, out facebookResult))
			{
				throw new AuthenticationException(string.Format("Request is not a valid Facebook OAuthCallbackUrl {0}", context.Request.Url));
			}

			if (!Guid.TryParse(facebookResult.State, out querystringStateValue) || cookieStateValue != querystringStateValue)
			{
				throw new AuthenticationException(string.Format("Response state value {0} does not equal state cookie {1}", querystringStateValue, cookieStateValue));
			}

			var url = string.Format(TokenUrl,
				FacebookConfig.FacebookAppId, context.Request.Url.GetLeftPart(UriPartial.Authority), FacebookConfig.FacebookAppSecret, facebookResult.Code);

			var response = new WebClient().DownloadString(new Uri(url));
			var p = HttpUtility.ParseQueryString(response);

			var token = p["access_token"];
			var expires = int.Parse(p["expires"]);

			client.AccessToken = token;
			dynamic me = client.Get("me");

			return new FacebookOAuthResponse
			{
				Token = token,
				ExpirySeconds = expires,
				User = me
			};
		}
	}

	public interface IFacebookAuthenticationProvider 
	{
		void Authenticate();
		FacebookOAuthResponse ParseResponse(HttpContext context);

	}

	public class FacebookOAuthResponse
	{
		public string Token { get; set; }
		public int ExpirySeconds { get; set; }
		public dynamic User { get; set; }
	}
}

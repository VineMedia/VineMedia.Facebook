using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using Castle.Windsor;
using Facebook;
using VineMedia.Facebook.FormsAuth;
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
			var stateCookie = context.Request.Cookies[FacebookAuthenticationProvider.StateCookieName];
			Guid cookieStateValue;
			Guid querystringStateValue;
			FacebookOAuthResult facebookResult;
			string redirectUrl = context.Request.QueryString["redir"];

			var client = new FacebookClient();
			

			if (stateCookie == null  
				|| !Guid.TryParse(stateCookie.Value, out cookieStateValue)
				|| !client.TryParseOAuthCallbackUrl(context.Request.Url, out facebookResult)
				|| !Guid.TryParse(facebookResult.State, out querystringStateValue)
				|| cookieStateValue != querystringStateValue)
			{
				context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
				context.Response.End();
				return;
			}

			var url = string.Format("https://graph.facebook.com/oauth/access_token?client_id={0}&redirect_uri={1}&client_secret={2}&code={3}",
				Settings.Default.FacebookAppId, "http://facebook.dev.vinemedia.com.au/facebookouth.axd", Settings.Default.FacebookAppSecret, facebookResult.Code);

			try
			{
				var response = new WebClient().DownloadString(new Uri(url));
				var p = HttpUtility.ParseQueryString(response);

				var token = p["access_token"];
				var expires = int.Parse(p["expires"]);

				client.AccessToken = token;
				dynamic me = client.Get("me");

				FormsAuthentication.SetAuthCookie(me.username, true);

				MembershipUser user = Membership.GetUser(me.username, true);
				if (user == null)
				{
					user = Membership.CreateUser(me.username, "idontneedapassword");
				}


				if (!string.IsNullOrWhiteSpace(me.email) && string.IsNullOrWhiteSpace(user.Email))
				{
					user.Email = me.email;
					Membership.UpdateUser(user);
				}

				var profile = ProfileCommon.Create(me.username, true) as ProfileCommon;
				profile.FirstName = me.first_name;
				profile.LastName = me.last_name;
				profile.Name = me.name;
				profile.Locale = me.locale;
				profile.Gender = me.gender;
				profile.FacebookUserId = me.id;
				profile.FacebookToken = token;
				profile.Timezone = me.timezone;
				profile.Save();

				context.Response.Cookies[FacebookAuthenticationProvider.StateCookieName].Expires = DateTime.Now.AddSeconds(-60);

				if (redirectUrl != null)
				{
					context.Response.Redirect(redirectUrl, false);
				}
				else
				{
					context.Response.Redirect("http://" + context.Request.Url.DnsSafeHost, false);
				}
			}
			catch (Exception)
			{
				context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
				context.Response.End();
				return;		
			}
		}
	}
}

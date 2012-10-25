using System;
using System.Net;
using System.Web;
using System.Web.Security;
using Castle.Windsor;

namespace VineMedia.Facebook
{
	public class FacebookOAuthHandler : IHttpHandler
	{
		internal static IWindsorContainer Container { get; set; }

		public IFacebookAuthenticationProvider FacebookAuthenticationProvider { get; set; }

		public FacebookOAuthHandler()
		{
			FacebookAuthenticationProvider = Container.Resolve<IFacebookAuthenticationProvider>();			
		}

		public bool IsReusable
		{
			get { return false; }
		}

		public void ProcessRequest(HttpContext context)
		{			
			try
			{
				var response = FacebookAuthenticationProvider.ParseResponse(context);				

				MembershipUser user = Membership.GetUser(response.User.username, true);
				if (user == null)
				{
					var password = Membership.GeneratePassword(Membership.MinRequiredPasswordLength, Membership.MinRequiredNonAlphanumericCharacters);
					user = Membership.CreateUser(response.User.username, password);
				}


				if (!string.IsNullOrWhiteSpace(response.User.email) && string.IsNullOrWhiteSpace(user.Email))
				{
					user.Email = response.User.email;
					Membership.UpdateUser(user);
				}

				var profile = ProfileCommon.Create(response.User.username, true) as ProfileCommon;
				profile.FirstName = response.User.first_name;
				profile.LastName = response.User.last_name;
				profile.Name = response.User.name;
				profile.Locale = response.User.locale;
				profile.Gender = response.User.gender;
				profile.FacebookUserId = response.User.id;
				profile.FacebookToken = response.Token;
				profile.Timezone = response.User.timezone;
				profile.Email = response.User.email;
				profile.Save();

				FormsAuthentication.SetAuthCookie(response.User.username, true);
				
				context.Response.Redirect("http://" + context.Request.Url.DnsSafeHost, false);
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

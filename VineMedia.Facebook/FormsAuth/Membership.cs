using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace VineMedia.Facebook.FormsAuth
{
	public class MembershipService : IMembershipService
	{
		public MembershipProvider MembershipProvider 
		{ 
			get 
			{
 				if (_membershipProvider == null)
				{
					_membershipProvider = Membership.Provider;
				}

				return _membershipProvider;
			}
			set { _membershipProvider = value; }
		}
		private MembershipProvider _membershipProvider;

		public MembershipUser GetMember(string username)
		{
			return MembershipProvider.GetUser(username, false);
		}

		public MembershipUser CreateMember(string username, string password, string email, bool isApproved, object providerUserKey)
		{
			MembershipCreateStatus createStatus;
			var user = MembershipProvider.CreateUser(username, password, email, null, null, isApproved, providerUserKey, out createStatus);

			if (createStatus != MembershipCreateStatus.Success)
			{
				throw new Exception(string.Format("Error when creating user: {0}", GetErrorMessage(createStatus)));
			}

			return user;
		}

		private string GetErrorMessage(MembershipCreateStatus status)
		{
			switch (status)
			{
				case MembershipCreateStatus.DuplicateUserName:
					return "Username already exists. Please enter a different user name.";

				case MembershipCreateStatus.DuplicateEmail:
					return "A username for that e-mail address already exists. Please enter a different e-mail address.";

				case MembershipCreateStatus.InvalidPassword:
					return "The password provided is invalid. Please enter a valid password value.";

				case MembershipCreateStatus.InvalidEmail:
					return "The e-mail address provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.InvalidAnswer:
					return "The password retrieval answer provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.InvalidQuestion:
					return "The password retrieval question provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.InvalidUserName:
					return "The user name provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.ProviderError:
					return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

				case MembershipCreateStatus.UserRejected:
					return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

				default:
					return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
			}
		}

	}

	public interface IMembershipService
	{
		MembershipUser GetMember(string username);
		MembershipUser CreateMember(string username, string password, string email, bool isApproved, object providerUserKey);
	}
}

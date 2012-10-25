using System.Web.Profile;

namespace VineMedia.Facebook
{
	public class ProfileCommon : ProfileBase
	{
		public virtual ProfileCommon GetProfile(string username)
		{
			return Create(username) as ProfileCommon;
		}

		public virtual string FirstName
		{
			get
			{
				return ((string)(GetPropertyValue("FirstName")));
			}
			set
			{
				SetPropertyValue("FirstName", value);
			}
		}

		public virtual string Name
		{
			get
			{
				return ((string)(GetPropertyValue("Name")));
			}
			set
			{
				SetPropertyValue("Name", value);
			}
		}

		public virtual long Timezone
		{
			get
			{
				return ((long)(GetPropertyValue("Timezone")));
			}
			set
			{
				SetPropertyValue("Timezone", value);
			}
		}

		public virtual string Locale
		{
			get
			{
				return ((string)(GetPropertyValue("Locale")));
			}
			set
			{
				SetPropertyValue("Locale", value);
			}
		}

		public virtual string Gender
		{
			get
			{
				return ((string)(GetPropertyValue("Gender")));
			}
			set
			{
				SetPropertyValue("Gender", value);
			}
		}

		public virtual string LastName
		{
			get
			{
				return ((string)(GetPropertyValue("LastName")));
			}
			set
			{
				SetPropertyValue("LastName", value);
			}
		}

		public virtual string FacebookUserId
		{
			get
			{
				return ((string)(GetPropertyValue("FacebookUserId")));
			}
			set
			{
				SetPropertyValue("FacebookUserId", value);
			}
		}

		public virtual string FacebookToken
		{
			get
			{
				return ((string)(GetPropertyValue("FacebookToken")));
			}
			set
			{
				SetPropertyValue("FacebookToken", value);
			}
		}

		public virtual string Email
		{
			get
			{
				return ((string)(GetPropertyValue("Email")));
			}
			set
			{
				SetPropertyValue("Email", value);
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
				return ((string)(this.GetPropertyValue("FirstName")));
			}
			set
			{
				this.SetPropertyValue("FirstName", value);
			}
		}

		public virtual string Name
		{
			get
			{
				return ((string)(this.GetPropertyValue("Name")));
			}
			set
			{
				this.SetPropertyValue("Name", value);
			}
		}

		public virtual long Timezone
		{
			get
			{
				return ((long)(this.GetPropertyValue("Timezone")));
			}
			set
			{
				this.SetPropertyValue("Timezone", value);
			}
		}

		public virtual string Locale
		{
			get
			{
				return ((string)(this.GetPropertyValue("Locale")));
			}
			set
			{
				this.SetPropertyValue("Locale", value);
			}
		}

		public virtual string Gender
		{
			get
			{
				return ((string)(this.GetPropertyValue("Gender")));
			}
			set
			{
				this.SetPropertyValue("Gender", value);
			}
		}

		public virtual string LastName
		{
			get
			{
				return ((string)(this.GetPropertyValue("LastName")));
			}
			set
			{
				this.SetPropertyValue("LastName", value);
			}
		}

		public virtual string FacebookUserId
		{
			get
			{
				return ((string)(this.GetPropertyValue("FacebookUserId")));
			}
			set
			{
				this.SetPropertyValue("FacebookUserId", value);
			}
		}

		public virtual string FacebookToken
		{
			get
			{
				return ((string)(this.GetPropertyValue("FacebookToken")));
			}
			set
			{
				this.SetPropertyValue("FacebookToken", value);
			}
		}
	}
}

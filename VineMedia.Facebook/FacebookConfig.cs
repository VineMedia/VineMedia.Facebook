using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VineMedia.Facebook
{
	public sealed class FacebookConfig
	{
		public string FacebookAppId { get; set; }
		public string FacebookAppSecret { get; set; }
		public string OAuthCallbackPath { get; set; }
		public string RedirectUrl { get; set; }
	}
}

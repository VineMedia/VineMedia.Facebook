using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Castle.MicroKernel.Registration;
using Dapper;

namespace VineMedia.Facebook.Web
{
	public class FacebookConfigInstaller : IWindsorInstaller
	{
		public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
		{
			List<Setting> settings;

			using(var connection = new MySql.Data.MySqlClient.MySqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
			{				
				connection.Open();
				settings = connection.Query<Setting>("SELECT SettingName, SettingValue FROM settings WHERE SettingName LIKE 'facebook%'").ToList();
			}

			var facebookConfig = new FacebookConfig
			{
				FacebookAppSecret = settings.FirstOrDefault(s => s.SettingName == "FacebookAppSecret").SettingValue,
				FacebookAppId = settings.FirstOrDefault(s => s.SettingName == "FacebookAppId").SettingValue,
				OAuthCallbackPath = "/facebookouth.axd";
			};

			container.Register(Component.For<FacebookConfig>().LifestyleSingleton().Instance(facebookConfig));
		}

		public class Setting
		{
			public string SettingName { get; set; }
			public string SettingValue { get; set; }
			public string OAuthCallbackPath { get; set; }
		}
	}
}

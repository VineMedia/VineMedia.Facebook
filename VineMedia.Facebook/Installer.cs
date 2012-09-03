using System.Web.Security;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using VineMedia.Facebook.FormsAuth;

namespace VineMedia.Facebook
{
	public class WindsorInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(Component.For<IFacebookAuthenticationProvider>().LifestylePerWebRequest().ImplementedBy<FacebookAuthenticationProvider>());
			container.Register(Component.For<IMembershipService>().LifestylePerWebRequest().ImplementedBy<MembershipService>());
			
			container.Register(Component.For<MembershipProvider>().LifestyleSingleton().Instance(Membership.Provider));
		}
	}
}

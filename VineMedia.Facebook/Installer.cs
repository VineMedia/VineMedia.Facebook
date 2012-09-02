using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace VineMedia.Facebook
{
	public class WindsorInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(Component.For<IFacebookAuthProvider>().LifestylePerWebRequest().ImplementedBy<FacebookAuthProvider>());

		}
	}
}

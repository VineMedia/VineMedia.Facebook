using System.Web;
using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace VineMedia.Facebook.Web
{
	public class ControllersInstaller : IWindsorInstaller
	{
		/// <summary>
		/// Installs the controllers
		/// </summary>
		/// <param name="container"></param>
		/// <param name="store"></param>
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(Component.For<HttpContextBase>().LifeStyle.PerWebRequest.UsingFactoryMethod(() => new HttpContextWrapper(HttpContext.Current)));			
			container.Register(FindControllers().LifestyleTransient());
		}

		/// <summary>
		/// Find controllers within this assembly in the same namespace as HomeController
		/// </summary>
		/// <returns></returns>
		private BasedOnDescriptor FindControllers()
		{
			return AllTypes.FromThisAssembly()
				.BasedOn<IController>()
				//.If(Component.IsInSameNamespaceAs<HomeController>())
				.If(t => typeof(Controller).IsAssignableFrom(t));
		}
	}
}
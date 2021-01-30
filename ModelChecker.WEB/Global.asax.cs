using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ModelChecker.WEB.Util;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Mvc;

namespace ModelChecker.WEB
{
	public class MvcApplication : HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);

			DependencyResolver.SetResolver(
			new NinjectDependencyResolver(
				new StandardKernel(new ModelCheckerModule("localhost"))));
		}
	}
}

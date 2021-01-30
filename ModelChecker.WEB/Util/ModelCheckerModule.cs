using ModelChecker.ISRC;
using Ninject.Modules;
using ModelChecker.SRC;


namespace ModelChecker.WEB.Util
{
	public class ModelCheckerModule : NinjectModule
	{
		private readonly string url;
		public ModelCheckerModule(string url)
		{
			this.url = url;
		}
		public override void Load()
		{
			Bind<IWebService>().To<WebService<IWebService>>().WithConstructorArgument(url);
		}
	}
}
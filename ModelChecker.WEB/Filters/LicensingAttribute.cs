using System.Web;
using System.Web.Mvc;

namespace ModelChecker.WEB.Filters
{
	public class LicensingAttribute : FilterAttribute, IActionFilter
	{
		public void OnActionExecuting(ActionExecutingContext filterContext)
		{
			//var slic = filterContext.HttpContext.Session["License"];
			//if (slic != null)
			//{
			//	if (slic is bool lic && lic == true)
			//		return;
			//}
			//filterContext.Result = new RedirectToRouteResult(
			//		new System.Web.Routing.RouteValueDictionary {
			//		{ "controller", "License" },
			//		{ "action", "CheckLicense" },
			//		{ "previewurl", filterContext.HttpContext.Request.RawUrl }
			//	   });
		}

		public void OnActionExecuted(ActionExecutedContext filterContext)
		{

		}
	}
}
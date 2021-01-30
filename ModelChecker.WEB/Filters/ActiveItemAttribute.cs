using ModelChecker.WEB.Models;
using System.Web.Mvc;


namespace ModelChecker.WEB.Filters
{
	public class ActiveItemAttribute : FilterAttribute, IResultFilter
	{

		public MenuItem Item { get; }

		public ActiveItemAttribute(MenuItem item)
		{
			this.Item = item;
		}

		public void OnResultExecuting(ResultExecutingContext filterContext) =>
			filterContext.Controller.TempData["ActiveItem"] = Item;


		public void OnResultExecuted(ResultExecutedContext filterContext)
		{

		}
	}
}
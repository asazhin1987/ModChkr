using System.Web.Mvc;

namespace ModelChecker.WEB.Filters
{
	public class CheckerExceptionAttribute : FilterAttribute, IExceptionFilter
	{

		public void OnException(ExceptionContext e)
		{
			if (!e.ExceptionHandled)
			{
				e.HttpContext.Response.Write(
					$"<br/><h2 class='text-secondary text-center w-100'>{e.Exception.Message}</h2><br/>");
				e.ExceptionHandled = true;
			}
		}
	}
}
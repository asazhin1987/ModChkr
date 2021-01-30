using ModelChecker.ISRC;
using ModelChecker.WEB.Filters;
using ModelChecker.WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ModelChecker.DTO;
using ModelChecker.DTO.Interfaces;
using System.IO;
using System.Threading.Tasks;
using Bimacad.Sys;


namespace ModelChecker.WEB.Controllers
{
	[CheckerException]
	public class ModelCheckerController : Controller
    {
		protected readonly IWebService src;
		protected readonly string updateScript = "UpdateResult()";

		public ModelCheckerController(IWebService service)
		{
			src = service;
		}

	

		public ActionResult Index()
        {
			return RedirectToAction("Constructions", "Report");
        }

		[ActiveItem(MenuItem.Reference)]
		public ActionResult Reference()
		{
			return View();
		}

		[NonAction]
		internal string CreateExcelFileTempPath()
		{
			return $"{ Server.MapPath(@"~\")}{Guid.NewGuid()}.xlsx";
		}

		internal async Task<FileContentResult> DownloadFile(string contentType, string path, string name)
		{
			byte[] bytes;
			using (FileStream stream = new FileStream(path, FileMode.Open))
			{
				bytes = new byte[stream.Length];
				await stream.ReadAsync(bytes, 0, bytes.Length);
			}
			System.IO.File.Delete (path);
			return File(bytes, contentType, name);

		}

		[HttpGet]
		public ActionResult PageMenu() =>
			PartialView(new PageMenuViewModel()
			{
				Items = new List<LinqMenuViewModel>
				{
					new LinqMenuViewModel(){Title = Resources.Menu.Constructions, MenuItem = MenuItem.Report, ActionName = "Constructions", ControllerName = "Report", Image = "" },
					new LinqMenuViewModel(){Title = Resources.Menu.Checks, MenuItem = MenuItem.ClashDetective, ActionName = "Checks", ControllerName = "ClashDetective", Image = "" },
					new LinqMenuViewModel(){Title = Resources.Menu.Statistic, MenuItem = MenuItem.Statistic, ActionName = "Statistic", ControllerName = "Statistic", Image = "" },
					new LinqMenuViewModel(){Title = Resources.Menu.Params, MenuItem = MenuItem.Setting, ActionName = "Params", ControllerName = "Setting", Image = "" },
					new LinqMenuViewModel(){Title = Resources.Menu.License, MenuItem = MenuItem.License, ActionName = "License", ControllerName = "License", Image = "" },
					//new LinqMenuViewModel(){Title = Resources.Menu.Reference, MenuItem = MenuItem.Reference, ActionName = "Index", ControllerName = "Help", Image = "" },
				}
			});


		/*если норм - замени на делегат*/
		internal static IEnumerable<FullCheckDTO> OrderBy(IEnumerable<FullCheckDTO> items, Orders orders)
		{
			switch (orders)
			{
				case Orders.Name:
					return items.OrderBy(x => x.Name);
				case Orders.Clashes:
					return items.OrderByDescending(x => x.ClashesQnt);
				case Orders.Create:
					return items.OrderByDescending(x => x.CreatedQnt);
				case Orders.Active:
					return items.OrderByDescending(x => x.ActiveQnt);
				case Orders.Analized:
					return items.OrderByDescending(x => x.AnalizedQnt);
				case Orders.Confirmed:
					return items.OrderByDescending(x => x.ConfirmedQnt);
				case Orders.Corrected:
					return items.OrderByDescending(x => x.CorrectedQnt);
				case Orders.Date:
					return items.OrderByDescending(x => x.Date);
				default:
					return items;
			}
		}

		internal static IEnumerable<ConstructionDTO> OrderBy(IEnumerable<ConstructionDTO> items, Orders orders)
		{
			switch (orders)
			{
				case Orders.Name:
					return items.OrderBy(x => x.Name);
				case Orders.Clashes:
					return items.OrderByDescending(x => x.ClashesQnt);
				case Orders.Create:
					return items.OrderByDescending(x => x.CreatedQnt);
				case Orders.Active:
					return items.OrderByDescending(x => x.ActiveQnt);
				case Orders.Analized:
					return items.OrderByDescending(x => x.AnalizedQnt);
				case Orders.Confirmed:
					return items.OrderByDescending(x => x.ConfirmedQnt);
				case Orders.Corrected:
					return items.OrderByDescending(x => x.CorrectedQnt);
				case Orders.Date:
					return items.OrderByDescending(x => x.Date);
				default:
					return items;
			}
		}
	}
}
using ModelChecker.ISRC;
using ModelChecker.WEB.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ModelChecker.WEB.Models;
using System.Threading.Tasks;
using ModelChecker.DTO;
using Bimacad.Sys;


namespace ModelChecker.WEB.Controllers
{
	
	public class ReportController : ModelCheckerController
    {
		public ReportController(IWebService service) : base(service) { }


		#region Constructions

		[HttpGet]
		[Licensing]
		[ActiveItem(MenuItem.Report)]
		public async Task<ActionResult> Constructions()
		{
			ViewBag.Orders = Orders.Name;
			return View(OrderBy(await src.GetAllConstructionsAsync(), Orders.Name));
		}

		[HttpGet]
		public async Task<PartialViewResult> ConstructionList(Orders orders)
		{
			ViewBag.Orders = orders;
			return PartialView("_ConstructionList",
				OrderBy(await src.GetAllConstructionsAsync(), orders));
		}


		public PartialViewResult ConstructionsFilter(int Id)
		{
			var model = src.GetAllConstructionsEmptyAsync();
			ViewBag.SelectedId = Id;
			return PartialView("_ConstructionFilter", OrderBy(model, Orders.Name));
		}

		[HttpGet]
		public async Task<PartialViewResult> EditConstruction(int Id)
		{
			return PartialView("_ConstructionEdit",
				(ConstructionViewModel)await src.GetConstructionAsync(Id));
		}

		[HttpGet]
		public PartialViewResult CreateConstruction()
		{
			return PartialView("_ConstructionEdit", new ConstructionViewModel());
		}

		[HttpGet]
		public async Task<PartialViewResult> DashBoard(IEnumerable<int> ConstructionIds)
		{
			return PartialView("_DashBoard", new StatusDashBoardViewModel()
			{
				StatusPipe = new StatusReportViewModel(await src.GetDailyConstructionsSlice(ConstructionIds, null)),
				Slices = await src.GetDailyConstructionSlices(ConstructionIds, null, null)
			});

		}

		[HttpPost]
		public async Task<PartialViewResult> MergeConstruction(ConstructionViewModel model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					await src.UpdateConstructionAsync(new ConstructionDTO()
					{
						Id = model.Id,
						Name = model.Name,
						Description = model.Description
					});
					return PartialView("_Success", new SucceedResultActionViewModel(updateScript, Resources.Global.SuccessMsg));
				}
				catch (Exception ex)
				{
					return PartialView("_Success", new FailureResultActionViewModel("", ex.Message));
				}
			}
			else
			{
				return PartialView("_ConstructionEdit", model);
			}
		}


		[HttpPost]
		public async Task<string> DeleteConstruction(int Id)
		{
			try
			{
				await src.RemoveConstruction(Id);
				return Resources.Global.SuccessRemoveMsg;
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}

		#endregion Constructions

		#region Filters

		//public async Task<PartialViewResult> FIlterList()
		//{
		//	return PartialView("_FIlterList");
		//}


		#endregion Filters

	}
}




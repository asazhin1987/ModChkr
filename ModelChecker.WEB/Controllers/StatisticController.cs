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
    public class StatisticController : ModelCheckerController
	{

		public StatisticController(IWebService service) : base(service) { }

		[HttpGet]
		[Licensing]
		[ActiveItem(MenuItem.Statistic)]
		public async Task<ActionResult> Statistic()
        {
			try
			{
				int? id = null;
				DateTime sdate = DateTime.Now.Date.AddDays(-14);
				DateTime edate = DateTime.Now.Date;

				if (Session["SelectedConstructionId"] is int cId)
					id = cId;
				if (Session["SDate"] is DateTime _sdate)
					sdate = _sdate;
				if (Session["EDate"] is DateTime _edate)
					edate = _edate;

				var constr = await src.GetConstructionEmptyAsync(id);

				return View(new StatisticViewModel()
				{
					Construction = constr, EDate = edate, SDate = sdate
				});
			}
			catch (NotFoundException)
			{
				throw new Exception(Resources.Global.NotFoundConstructions);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		[HttpGet]
		public async Task<PartialViewResult> StatisticReport(StatisticViewModel model)
		{
			Session["SelectedConstructionId"] = model.ConstructionId;
			Session["SDate"] = model.SDate;
			Session["EDate"] = model.EDate;

			if (model.ReportType == ReportType.CategoryMatrix)
				return await CategoryMatrix(model);
			else if (model.ReportType == ReportType.ModelMatrix)
				return await ModelMatrix(model);

			model.AllChecks = await src.GetDynamicChecksAsync(model.ConstructionId, model.SDate, model.EDate);
			model.SelectedChecks = model.SelectedChecks ?? model.AllChecks.Select(x => x.Id).ToList();

			if (model.ReportType == ReportType.Compare)
				return await Compare(model);
			else if (model.ReportType == ReportType.Charts)
				return await Dynamic(model);
			else
				throw new Exception($"404 Not Found \r {model.ReportType}");
		}

	
		[HttpGet]
		public async Task<PartialViewResult> Dynamic(StatisticViewModel model)
		{
			if (model.AllChecks.Count() == 0)
				return PartialView("_Dynamic", new ChartViewModel());
			return PartialView("_Dynamic", 
				new ChartViewModel(await src.GetDailySlices(model.SelectedChecks, model.SDate, model.EDate)){
					Checks = model.AllChecks, SelectedChecks = model.SelectedChecks
				});
		}

		[HttpGet]
		public async Task<PartialViewResult> DynamicCharts(StatisticViewModel model)
		{
			return PartialView("_Chart", await src.GetDailySlices(model.SelectedChecks, model.SDate, model.EDate));
		}

		[HttpGet]
		public async Task<PartialViewResult> StatusPipe(StatisticViewModel model)
		{
			return PartialView("_Pipe", new StatusReportViewModel(await src.GetDailyChecksSlice(model.SelectedChecks, model.EDate)));
		}

		[HttpGet]
		public async Task<PartialViewResult> Compare(StatisticViewModel model)
		{
			return PartialView("_Compare", new CompareViewModel(model.AllChecks));
		}


		[HttpGet]
		public async Task<PartialViewResult> CategoryMatrix(StatisticViewModel model)
		{
			return PartialView("_CategoryMatrix", 
				new MatrixViewModel(await src.GetCategoryMatrixAsync(model.ConstructionId), Resources.Global.CategoryName));
		}

		[HttpGet]
		public async Task<PartialViewResult> ModelMatrix(StatisticViewModel model)
		{
			return PartialView("_ModelMatrix",
				new MatrixViewModel(await src.GetModelMatrixAsync(model.ConstructionId), Resources.Global.ModelName));
		}

		//[HttpGet]
		//public async Task<PartialViewResult> ChecksPercentReport(int constructionId)
		//{
		//	return PartialView("_ChecksPercentReport",
		//		new ChecksPersentReportViewModel(await src.GetChecksAsync(constructionId)));
		//}
	}
}
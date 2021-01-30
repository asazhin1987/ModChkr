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
using ModelChecker.WEB.Util;
using Bimacad.Sys;


namespace ModelChecker.WEB.Controllers
{
	public class ClashDetectiveController : ModelCheckerController
	{
		public ClashDetectiveController(IWebService service) : base(service) { }


		[HttpGet]
		[Licensing]
		[ActiveItem(MenuItem.ClashDetective)]
		public async Task<ActionResult> Checks()
		{
			try
			{
				int? id = null;
				if (Session["SelectedConstructionId"] is int cId)
					id = cId;
				var constr = await src.GetConstructionEmptyAsync(id);
				var checks = OrderBy(await src.GetChecksAsync(constr.Id), Orders.Name);

				ViewBag.Orders = Orders.Name;
				return View(new ChecksPageViewModel()
				{
					Construction = constr,
					Checks = checks
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


		public async Task<PartialViewResult> CheckList(int constructionId, Orders orders)
		{
			Session["SelectedConstructionId"] = constructionId;
			ViewBag.Orders = orders;
			return PartialView("_ChecksList", OrderBy(await src.GetChecksAsync(constructionId), orders));
		}


		public async Task<FileResult> ExcelReport(int id)
		{
			try
			{
				string temppath = CreateExcelFileTempPath();
				ExcelFactory.CreateExcelReporChecks(new ChecksPageViewModel()
				{
					 Checks = OrderBy(await src.GetChecksAsync(id), Orders.Name), Construction = await src.GetConstructionAsync(id)
				}, temppath);
				return  await DownloadFile("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", temppath, $"{Resources.Global.ClashesReport}_{DateTime.Now.ToShortDateString()}.xlsx");
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}


		public async Task<string> DeleteChecks(IEnumerable<int> checks)
		{
			if (checks != null)
			{
				try
				{
					await src.RemoveChecksAsync(checks);
					return Resources.Global.SuccessRemoveMsg;
				}
				catch (Exception ex)
				{
					return ex.Message;
				}
			}
			else
			{
				return Resources.Global.NullListMsg;
			}
		}


		[HttpGet]
		public async Task<PartialViewResult> UpdateCheck(int Id)
		{
			try
			{
				var dto = await src.GetCheckAsync(Id);
				return PartialView("_EditCheck", new CheckViewModel() { Id = dto.Id, Name = dto.Name });
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


		[HttpPost]
		public async Task<PartialViewResult> UpdateCheck(CheckViewModel model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					await src.UpdateCheckAsync(new FullCheckDTO() { Id = model.Id, Name = model.Name });
					return PartialView("_Success", new SucceedResultActionViewModel(updateScript, Resources.Global.SuccessMsg));
				}
				catch (NotFoundException)
				{
					throw new Exception(Resources.Global.NotFoundConstructions);
				}
				catch (ModelCheckerException cex)
				{
					throw cex;
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
			else
			{
				return PartialView("_EditCheck", model);
			}
		}
	}
}
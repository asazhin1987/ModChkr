using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ModelChecker.WEB.Models;
using ModelChecker.WEB.Filters;
using ModelChecker.DTO;
using ModelChecker.ISRC;
using Bimacad.Sys;

namespace ModelChecker.WEB.Controllers
{
    public class LicenseController : ModelCheckerController
    {
		public LicenseController(IWebService service) : base(service) { }

		[ActiveItem(MenuItem.License)]
		public async Task<ActionResult> License(LicenseViewModel model)
		{
			return View(await GetLicenseReportViewModel());
		}


		public async Task<PartialViewResult> LicensePartial()
		{
			return PartialView("_License", await GetLicenseReportViewModel());
		}

		[NonAction]
		public async Task<LicenseReportViewModel> GetLicenseReportViewModel()
		{
			try
			{
				return new LicenseReportViewModel()
				{
					License = await GetLicenseVM(),
					AllQnt = await src.GetLicenseQnt(),
					Last3MonthsQnt = await src.GetLicenseUsedQnt(3),
					LicenseUsings = await src.GetLicenseMonthsUsingAsync(12),
					Licenses = await src.GetAllLicenseUsingsAsync(),
					UsingQnt = await src.GetAllLicenseUsedQnt()
				};
			}
			catch (Exception ex)
			{
				throw ex;
			}
			
		}

		[HttpGet]
		public PartialViewResult EditLicense()
		{
			return PartialView("_EditLicense");
		}

		[HttpGet]
		public async Task<PartialViewResult> LicenseResult()
		{
			return PartialView("_LicenseResult", await GetLicenseVM());
		}

		[HttpPost]
		public async Task<PartialViewResult> EditLicense(LicenseViewModel model)
		{
			if (ModelState.IsValid)
			{
				await src.SetLicenseAsync(model.Key);
				SetLicenseCookie(src.CheckLicense());
				return PartialView("_Success", new SucceedResultActionViewModel(updateScript, Resources.Global.SuccessMsg));
			}
			return PartialView("_EditLicense", model);
		}

		public async Task<PartialViewResult> GetTableUse()
		{
			return PartialView("_TableUse", await src.GetAllLicenseUsingsAsync());
		}

		[HttpPost]
		public async Task<PartialViewResult> DeleteLicenses(IEnumerable<int> lics)
		{
			await src.BreakLicensesAsync(lics);
			return PartialView("_License", await GetLicenseReportViewModel());
			//return PartialView("_TableUse", await src.GetAllLicenseUsingsAsync());
		}


		[NonAction]
		public async Task<LicenseViewModel> GetLicenseVM()
		{
			try
			{
				var lic = await src.GetLicenseAsync();
				return new LicenseViewModel(item: lic);
			}
			catch (WrongKeyException)
			{
				return new LicenseViewModel(false, Resources.Global.LicenseWrongKeyMsg);
			}
			catch (NullKeyException)
			{
				return new LicenseViewModel(false, Resources.Global.LicenseNullKeyMsg);
			}
			catch (ZeroQntException)
			{
				return new LicenseViewModel(false, Resources.Global.LicenseZeroQntMsg);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		[HttpGet]
		public ActionResult CheckLicense(string previewurl)
		{
			try
			{
				bool lic = src.CheckLicense();
				SetLicenseCookie(lic);
				if (lic == true)
					return Redirect(previewurl);
				else
					return RedirectToAction("License");
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		private void SetLicenseCookie(bool lic)
		{
			Session["License"] = lic;
		}


		#region Dashboard
		public PartialViewResult GetPipe()
		{
			return PartialView("_Pipe");
		}

		public PartialViewResult GetAllQnt()
		{
			return PartialView("_AllQnt");
		}

		public PartialViewResult GetUsesQnt()
		{
			return PartialView("_UsesQnt");
		}

		#endregion Dashboard
	}
}
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

    public class SettingController : ModelCheckerController
	{
		
		public SettingController(IWebService service) : base(service) { }

		[HttpGet]
		[Licensing]
		[ActiveItem(MenuItem.Setting)]
		public async Task<ActionResult> Params()
        {
            return View(new ParamsViewModel(await src.GetParamsAsync()));
        }


		public async Task<PartialViewResult> ParamsPartial()
		{
			return PartialView("_Params", new ParamsViewModel(await src.GetParamsAsync()));
		}


		[HttpGet]
		[Licensing]
		public async Task<PartialViewResult> EditParams()
		{
			return PartialView("_EditParams", new ParamsViewModel(await src.GetParamsAsync()));
		}

		[HttpPost]
		public async Task<PartialViewResult> EditParams(ParamsViewModel item)
		{
			if (ModelState.IsValid)
			{
				await src.SetParams(new ParamsDTO() { DatRetentionPeriod = item.DatRetentionPeriod, Delimiter = item.Delimiter });
				return PartialView("_Params", new ParamsViewModel(await src.GetParamsAsync()));
			}
			else {
				return PartialView("_EditParams", item);
			}
			
		}
	}
}
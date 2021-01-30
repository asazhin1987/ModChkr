using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coordinator.DTO;
using Coordinator.DTO.Infrastructure;
using Coordinator.Forms.Applications;
using Coordinator.Forms.Model;
using Coordinator.Forms.Xaml;
using Microsoft.Win32;

namespace Coordinator.Forms.Infrastructure
{
	/// <summary>
	/// Фабрика ClashesApplication
	/// Реализует паттерн наблюдатель
	/// </summary>
	public class ClashReportFactory : ClientFactory
	{
		ClashesApplication app;
		public EventHandler<CoordinatorEventArgs> ShowElementEvent;
		public EventHandler<CoordinatorEventArgs> CutViewEvent;

		public ClashReportFactory() : base(true, true) { }

		public void Create()
		{
			if (IsLicensed)
			{
				app = new ClashesApplication()
				{
					Cut3dView = true,
					CutValue = 1,
					HelpAddress = $"http://{HostName}/Bimacad_Model_Checker_help"
				};
				ClashFilterDTO clashFilter = new ClashFilterDTO()
				{
					PageNum = 1,
					PageSize = 100,
					Sorts = Sorts.CreateDate,
					Orders = Orders.Descending
				};
				app.ClashFilter = clashFilter;

				app.FilterChanged += UpdateCollection;
				app.ShowElement += ShowElement;
				app.CutView += CutView;
				app.SetStatus += SetStatus;
				app.SetFilters += SetFilters;
				app.GetExcelCurrentPage += GetExcelCurrentPage;
				app.GetExcelAllPages += GetExcelAllPages;

				UpdateCollection(clashFilter);

				ClashesForm form = new ClashesForm(app)
				{
					//Topmost = true
				};

				form.Show();
			}

		}

		private void UpdateCollection(object sender, CoordinatorFilterEventArgs e)
		{
			UpdateCollection(e.ClashFilter);
		}

		private void UpdateCollection(ClashFilterDTO _filter)
		{
			var data = src.GetClashes(_filter);
			app.Clashes = new ObservableCollection<ClashDTO>(data.Clashes);
			app.PagesQnt = data.PagesCount;
			app.ClashesQnt = data.ClashesCount;
		}

		private void ShowElement(object sender, CoordinatorEventArgs e)
		{
			ShowElementEvent?.Invoke(sender, e);
		}

		private void CutView(object sender, CoordinatorEventArgs e)
		{
			CutViewEvent?.Invoke(sender, e);
		}

		private void SetStatus(object sender, CoordinatorEventArgs e)
		{
			var statuses = src.GetAllStatuses().Where(x => x.Id == 3 || x.Id == 4);
			StatusApplication _app = new StatusApplication()
			{
				Statuses = new ObservableCollection<StatusDTO>(statuses), 
				SelectedStatus = statuses.FirstOrDefault()
			};

			StatusForm form = new StatusForm(_app) {Topmost=true };
			
			form.ShowDialog();
			if (_app.OkResult)
			{
				src.UpdateClash(new ClashDTO()
				{
					Id = e.Clash.Id, ClashStatusId = _app.SelectedStatus.Id
				});
			}
		}

		private void SetFilters(object sender, CoordinatorFilterEventArgs e)
		{
			FilterApplication fapp = new FilterApplication(
				app.ClashFilter, src.GetAllConstructions(), src.GetAllChecks(), src.GetAllRevitModels(), src.GetAllStatuses());

			FilterForm form = new FilterForm(fapp) { Topmost = true };
			form.ShowDialog();

			if (fapp.OkResult)
			{
				app.ClashFilter.Constructions = fapp.ConstructionApplication.SelectedItemIds;
				app.ClashFilter.RevitModels = fapp.ModelApplication.SelectedItemIds;
				app.ClashFilter.Checks = fapp.CheckApplication.SelectedItemIds;
				app.ClashFilter.Statuses = fapp.StatusApplication.SelectedItemIds;
				app.ClashFilter.Sorts = fapp.Sorts;
				app.ClashFilter.Orders = fapp.Orders;
				app.ClashFilter.PageNum = 1;
			}
		}




		private void GetExcel(Func<ClashFilterDTO, ClashesResultDTO> _func, ClashFilterDTO e)
		{
			try
			{
				SaveFileDialog dialog = new SaveFileDialog()
				{
					Filter = "Excel file | *.xlsx",
					DefaultExt = ".xlsx",
					OverwritePrompt = true,
					AddExtension = true,
				};
				dialog.ShowDialog();

				if (!dialog.FileName.Equals(string.Empty))
				{
					Reporter.Excel.ExcelFactory.Create(dialog.FileName, _func.Invoke(e).Clashes);
					
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private void GetExcelCurrentPage(object sender, CoordinatorFilterEventArgs e)
		{
			GetExcel(src.GetClashes, e.ClashFilter);
		}


		private void GetExcelAllPages(object sender, CoordinatorFilterEventArgs e)
		{
			GetExcel(src.GetAllClashes, e.ClashFilter);
		}
			
	}
}

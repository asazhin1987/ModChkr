using Coordinator.DTO.Infrastructure;
using Coordinator.Forms.Applications;
using Coordinator.Forms.Xaml;
using Microsoft.Win32;
using System;
using Coordinator.ClashXMLModel;
using System.Xml.Serialization;
using System.IO;
using Coordinator.DTO;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Coordinator.Forms.Infrastructure
{
	public class XMLReportFactory : ClientFactory
	{
		public EventHandler<CoordinatorEventArgs> ShowElementEvent;
		public EventHandler<CoordinatorEventArgs> CutViewEvent;
		ClashesApplication app;

		private IEnumerable<ClashDTO> FullClashes { get; set; }

		public XMLReportFactory() : base(true, true) { }

		public void Create()
		{
			if (IsLicensed)
			{
				try
				{

					string file = GetXMLFile();
					if (file != string.Empty)
					{
						FullClashes = GetClashes(GetExchangeSerializator(file));

						app = new ClashesApplication()
						{
							Cut3dView = true,
							CutValue = 1,
							StatusBtnEnable = false,
							UpdateBtnEnable = false,
							FilterBtnEnable = false,
							HelpAddress = $"http://{HostName}/Bimacad_Model_Checker_help"
						};

						ClashFilterDTO clashFilter = new ClashFilterDTO()
						{
							PageNum = 1,
							PageSize = 100,
						};

						app.ShowElement += ShowElement;
						app.CutView += CutView;
						app.FilterChanged += UpdateCollection;
						app.GetExcelCurrentPage += GetExcelCurrentPage;
						app.GetExcelAllPages += GetExcelAllPages;

						app.ClashFilter = clashFilter;
						UpdateCollection(clashFilter);

						ClashesForm form = new ClashesForm(app);

						form.Show();
					}
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
				
		}

		#region Initial and filter

		private string GetXMLFile()
		{
			OpenFileDialog dialog = new OpenFileDialog()
			{
				Filter = "xml files|*.xml",
				Multiselect = false
			};
			if (dialog.ShowDialog() == true)
				return dialog.FileName;
			return string.Empty;
		}

		private void UpdateCollection(object sender, CoordinatorFilterEventArgs e)
		{
			UpdateCollection(e.ClashFilter);
		}

		private void UpdateCollection(ClashFilterDTO _filter)
		{
			IEnumerable<ClashDTO> _clashes = 
			_filter.SearchId?.Length > 0 && int.TryParse(_filter.SearchId, out int _search) ?
				_clashes = FullClashes.Where(x => x.RevitElement1Id == _search || x.RevitElement2Id == _search)
				: _clashes = FullClashes;

			app.ClashesQnt = _clashes.Count();
			app.PagesQnt = (int)Math.Ceiling((double)_clashes.Count() / _filter.PageSize);
			app.Clashes = new ObservableCollection<ClashDTO>(_clashes.Skip((_filter.PageNum - 1) * _filter.PageSize)
				.Take(_filter.PageSize));
		}

		
		private void ShowElement(object sender, CoordinatorEventArgs e)
		{
			ShowElementEvent?.Invoke(sender, e);
		}

		private void CutView(object sender, CoordinatorEventArgs e)
		{
			CutViewEvent?.Invoke(sender, e);
		}

		#endregion Initial and filter

		#region Serializ

		private IEnumerable<ClashDTO> GetClashes(Exchange exchange)
		{
			//var res = exchange.batchtest.clashtests.clashtest.SelectMany(x => x.clashresults.clashresult).ToList();
			ICollection<ClashDTO> result = new Collection<ClashDTO>();
			foreach (var test in exchange.batchtest.clashtests.clashtest.Where(x => x.clashresults.clashresult != null).AsParallel().AsOrdered())
			{
				foreach (var s in test.clashresults.clashresult.AsParallel().AsOrdered())
				{
					try
					{
						result.Add(new ClashDTO()
						{
							Grid = s.guid,
							CheckName = test.name,
							ConstructionName = exchange.filename,
							X = (double)s.clashpoint.pos3f.x,
							Y = (double)s.clashpoint.pos3f.y,
							Z = (double)s.clashpoint.pos3f.z,
							ClashStatusName = s.resultstatus,
							Distance = (double)s.distance,
							Element1Level = s.clashobjects.clashobject[0].layer,
							Element2Level = s.clashobjects.clashobject[1].layer,
							RevitModel1Name = string.Join(" > ", s.clashobjects.clashobject[0].pathlink.node),
							RevitModel2Name = string.Join(" > ", s.clashobjects.clashobject[1].pathlink.node),
							RevitElement1Id = (int)s.clashobjects.clashobject[0].objectattribute.value,
							RevitElement2Id = (int)s.clashobjects.clashobject[1].objectattribute.value,
							CategoryElement1Name = s.clashobjects.clashobject[0].smarttags.smarttag[1].value,
							CategoryElement2Name = s.clashobjects.clashobject[1].smarttags.smarttag[1].value,
							RevitElement1Name = s.clashobjects.clashobject[0].smarttags.smarttag[0].value,
							RevitElement2Name = s.clashobjects.clashobject[1].smarttags.smarttag[0].value,
							FoundDate = new DateTime(s.createddate.date.year, s.createddate.date.month, s.createddate.date.day, s.createddate.date.hour, s.createddate.date.minute, s.createddate.date.second),
							Odate = new DateTime(s.createddate.date.year, s.createddate.date.month, s.createddate.date.day, s.createddate.date.hour, s.createddate.date.minute, s.createddate.date.second)
						});
					}
					catch (Exception ex)
					{
						continue;
					}

				}
			}
			
			return result;
		}


		private Exchange GetExchangeSerializator(string path)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(Exchange));
			using (FileStream file = new FileStream(path, FileMode.Open))
			{
				try
				{
					if (xmlSerializer.Deserialize(file) is Exchange _ex)
						return _ex;
					else
						throw new Exception("Неверный формат xml");
				}
				catch (Exception ex)
				{
					throw ex;
				}
				
			}
			
		}
		#endregion Serializ

		#region excel

		private void GetExcel(IEnumerable<ClashDTO> items)
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
					Reporter.Excel.ExcelFactory.Create(dialog.FileName, items);

				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private void GetExcelCurrentPage(object sender, CoordinatorFilterEventArgs e)
		{
			GetExcel(app.Clashes.ToList());
		}


		private void GetExcelAllPages(object sender, CoordinatorFilterEventArgs e)
		{
			GetExcel(FullClashes);
		}
		#endregion excel


	}
}

using System;
using System.Collections.Generic;
using Autodesk.Navisworks.Api.Clash;
using Autodesk.Navisworks.Api.Plugins;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Text;
using System.Linq;
using Autodesk.Navisworks.Api;
using ModelChecker.Plugin.Forms;
using ModelChecker.Plugin.Model;
using ModelChecker.DTO;
using System.Collections.ObjectModel;
using ModelChecker.ISRC;
using ModelChecker.SRC;
using winapp = System.Windows;
using Bimacad.Sys;


namespace ModelChecker
{
	[Plugin("ModelChecker", "ADSK", DisplayName = "Model Checker")]

	[Strings("ModelChecker.name")]
	[RibbonLayout("ModelChecker.xaml")]
	[RibbonTab("ID_ModelCheckerTab", DisplayName = "Model Checker")]

	[Command("Id_ShowMasterBtn", Icon = "checker16.ico", LargeIcon = "checker32.ico", LoadForCanExecute = true,
		CallCanExecute = CallCanExecute.DocumentNotClear, CanToggle = true, DisplayName = "Show Clash")]

	[Command("Id_SettingBtn", Icon = "settings16.ico", LargeIcon = "settings32.ico", LoadForCanExecute = true,
		CallCanExecute = CallCanExecute.Always, CanToggle = true, DisplayName = "Settings")]

	public class WriteClashes : CommandHandlerPlugin
	{

		ClashMasterViewModel model;
		SettingsViewModel settingModel;
		WriteClashesMasterWIndow dialog;
		IClashService src;
		string determ;
		//string Address { get { return $"{Environment.SpecialFolder.CommonApplicationData}\\Autodesk\\ApplicationPlugins\\ADSK.ModelChecker.bundle\\Contents"; } }

		public override int ExecuteCommand(string name, params string[] parameters)
		{
			typeof(WriteClashes).GetMethod(name).Invoke(this, new object[] { });
			return 0;

		}

		public void Id_ShowMasterBtn()
		{
			try
			{
				SetService(CheckHost());
				if (src.CheckLicense() == false)
					throw new Exception("Лицензия недействительна");
				determ = src.GetDelimiter();
				//создание модели
				model = new ClashMasterViewModel(
				checks: GetChecksObservableCollection(GetChecks()),
				constructions: GetConstructionsObservableCollection(GetConstructions()));

				//запуск диалога
				model.MasterComplete += Model_MasterComplete;
				dialog = new WriteClashesMasterWIndow(model);
				dialog.ShowDialog();
			}
			catch (Exception ex)
			{
				Inform(ex.Message);
			}
		}

		public void Id_SettingBtn()
		{
			try
			{
				settingModel = new SettingsViewModel()
				{
					Host = BTextWriter.ReadCurrentFile($"host.txt")
				};
				settingModel.SaveHost += SettingModel_SaveHost;
				settingModel.CheckHost += SettingModel_CheckHost;
				SettingsForm sf = new SettingsForm(settingModel);

				sf.ShowDialog();
			}
			catch (Exception ex)
			{
				Inform(ex.Message);
			}

		}

		private void SettingModel_CheckHost(object sender, EventArgs e)
		{
			try
			{
				string _host = ClaerStrinng(settingModel.Host);
				var _src = new ClashService<IClashService>(_host);

				if (_src.CheckConnection(_host))
				{
					Inform("Соединение установлено");
					_src = null;
				}
			}
			catch
			{
				Inform("Соединение не установлено, проверьте правильность написания адреса хоста или натройки сети");
			}
		}

		private string ClaerStrinng(string _str) =>
			_str.Replace("\r", "").Replace("\n", "").Trim();

		private void SettingModel_SaveHost(object sender, EventArgs e)
		{
			try
			{
				BTextWriter.WriteCurrentFile(ClaerStrinng(settingModel.Host), $"host.txt");
				Inform("Данные сохранены");
			}
			catch (Exception ex)
			{
				Inform(ex.Message);
			}

		}

		private void Model_MasterComplete(object sender, EventArgs e)
		{
			try
			{
				DateTime odate = DateTime.Now;
				//WriteParans();
				//Log("======================================================Start===================================");
				IEnumerable<ClashTest> navisChecks = GetChecks(model.SelectedChecks).ToList();
				int constructionId = model.SelectedConstruction.Id;
				//Log("Get Checks Complete");
				//сравнить проверки, присвоить ид
				IEnumerable<FullCheckDTO> checks = src.MergeChecks(GetChecks(navisChecks, constructionId, odate).ToList(), constructionId).Where(x => x.Id > 0).ToList();
				//Log("Merge Checks Complete");
				//получение клешей
				IEnumerable<ClashDTO> clashes = GetClashes(navisChecks, constructionId).ToList();
				IEnumerable<ClashDTO> _clashes = clashes.Where(x => x.RevitElement1Id > 0 && x.RevitElement2Id > 0).ToList();
				//Log("Get Clashes complete: " + _clashes.Count() + " All: " + clashes.Count());

				//сравнить файлы, присвоить ид
				var _modls = GetRevitModels(_clashes).ToList();
				IEnumerable<RevitModelDTO> models = src.MergeModels(_modls);
				//Log("Merge Models Complete");

				//сравнить категории, присвоить ид
				var _cats = GetCategories(_clashes).ToList();
				IEnumerable<RevitCategoryDTO> cats = src.MergeCategories(_cats);
				//Log("Merge Categories Complete");

				//сравнить элементы, присвоить ид
				IEnumerable<RevitElementDTO> elems = GetElements(_clashes, models, cats).ToList();
				MergeElements(ref elems);
				//Log("Merge Elements Complete");
				//mega join получение ид
				IEnumerable<ClashDTO> resuleClashes = GetClashes(_clashes, elems, checks, odate);
				//Log($"GetClashes Complete.Count = {resuleClashes.Count()}");
				//сравнить конфликты
				int r = MergeClashes(resuleClashes);
				//Log("Merge Clashes Complete");
				//обновить старые статусы
				SetNotFoundClashes(checks, odate);
				//Log("SetNotFoundClashes Complete");
				//дата объекта
				src.SetConstructionDate(constructionId, odate);
				//Log("Set Construction Date Complete");
				Inform($"Загрузка завершена. Ошибок: {r}");


			}
			catch (Exception ex)
			{
				Inform(ex.Message);
			}

		}

		private void Log(string msg)
		{
			BTextWriter.WriteLog(msg);
		}

		private void Inform(string msg)
		{
			winapp.MessageBox.Show(msg);
		}

		#region
		private void SetService(string host)
		{
			try
			{
				src = new ClashService<IClashService>(host);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private ObservableCollection<CheckViewModel> GetChecksObservableCollection(IEnumerable<CheckViewModel> items)
		{
			return new ObservableCollection<CheckViewModel>(items);
		}

		private ObservableCollection<ConstructionViewModel> GetConstructionsObservableCollection(IEnumerable<ConstructionDTO> items)
		{
			try
			{
				return new ObservableCollection<ConstructionViewModel>(items.Select(x => new ConstructionViewModel(x)));
			}
			catch (Exception ex)
			{
				throw new Exception($"Ошибка преобразования Constructions {ex.Message}");
			}
		}
		

		private IEnumerable<ConstructionDTO> GetConstructions()
		{
			try
			{
				var constrs = src.GetAllConstructions();
				if (constrs.Count() == 0)
					throw new Exception("Нет проектов");
				return constrs;
			}
			catch (Exception ex)
			{
				throw new Exception($"Ошибка получения объектов. {ex.Message}");
			}
			
		}

		private string CheckHost()
		{
			string host = BTextWriter.ReadCurrentFile("host.txt");
			if (host == "")
				throw new Exception("Укажите адрес сервера размещения службы.");
			return ClaerStrinng(host);
		}

		//private IEnumerable<CheckDTO> MergeChecks(IEnumerable<CheckDTO> items, int constructionId) =>
		//	src.MergeChecks(items, constructionId);

		private IEnumerable<FullCheckDTO> GetChecks(IEnumerable<ClashTest> items, int constrId, DateTime date) =>
			items.Select(x => new FullCheckDTO() { Name = x.DisplayName, ConstructionId = constrId, Date = date });

		private IEnumerable<ClashTest> GetChecks(IEnumerable<CheckViewModel> checks) =>
			from t1 in Application.MainDocument.GetClash().TestsData.Tests
				   join t2 in checks on t1.Guid equals t2.Guid
				   select t1 as ClashTest;
		

		public override CommandState CanExecuteCommand(string commandId) =>
			new CommandState
			{
				IsEnabled = true
			};

		public override bool CanExecuteRibbonTab(string ribbonTabId)
			=> true;

		private IEnumerable<CheckViewModel> GetChecks()
		{
			var tests = Application.MainDocument.GetClash().TestsData.Tests.Where(x => x.DisplayName != null 
			&& x.DisplayName.Contains(determ) && x.DisplayName.StartsWith(determ)== false && x.DisplayName.EndsWith(determ) == false);
			if(tests != null && tests.Count() > 0)
				return tests.Select(x => new CheckViewModel() {Name = x.DisplayName, Checked = true, Guid = x.Guid });
			throw new Exception($"В модели отсутствуют проверки с соответствующей маской разделителя: {determ} ");
		}

		#endregion
		#region clashes

		void SetNotFoundClashes(IEnumerable<FullCheckDTO> checks, DateTime odate)
		{
			SetNotFoundClashes(checks.Select(x => x.Id).ToList(), odate);
		}

		void SetNotFoundClashes(IEnumerable<int> checkIds, DateTime odate)
		{
			foreach (int checkId in checkIds.AsParallel())
			{
				IEnumerable<int> clashIds = src.GetNotFoundClashes(checkId, odate);
				foreach (int clashId in clashIds.AsParallel())
				{
					src.SetNotFoundClash(clashId, odate);
				}
			}
		}

		void MergeElements(ref IEnumerable<RevitElementDTO> items)
		{
			foreach (var item in items.AsParallel())
				item.Id = src.MergeElement(item).Id;
		}

		IEnumerable<ClashDTO> GetClashes(IEnumerable<ClashDTO> clashes, IEnumerable<RevitElementDTO> els, IEnumerable<FullCheckDTO> checks, DateTime date)
		{
			return from t1 in clashes
				   join t2 in els
				   on new { x1 = t1.RevitElement1Id, x2 = t1.RevitModel1Name } equals new { x1 = t2.RevitId, x2 = t2.ModelName }
				   join t3 in els
				   on new { x1 = t1.RevitElement2Id, x2 = t1.RevitModel2Name } equals new { x1 = t3.RevitId, x2 = t3.ModelName }
				   join t4 in checks on t1.CheckName equals t4.Name
				   select new ClashDTO()
				   {
					   ConstructionId = t1.ConstructionId,
					   ClashStatusId = t1.ClashStatusId,
					   RevitModel1Id = t2.RevitModelId,
					   RevitElement1Id = t2.RevitId,
					   CategotyElement1Id = t2.CategoryId,
					   Element1Id = t2.Id,
					   RevitModel2Id = t3.RevitModelId,
					   RevitElement2Id = t3.RevitId,
					   CategoryElement2Id = t3.CategoryId,
					   Element2Id = t3.Id,
					   Distance = t1.Distance,
					   X = t1.X,
					   Y = t1.Y,
					   Z = t1.Z,
					   CheckId = t4.Id,
					   Odate = date
				   };
		}

		int MergeClashes(IEnumerable<ClashDTO> items)
		{
			//string h = CheckHost();
			//items.AsParallel().ForAll(x => src.MergeClash(x));
			//return 0;
			int errqnt = 0;
			foreach (var item in items.AsParallel())
			{
				bool res = src.MergeClash(item);
				if (res == false)
					errqnt++;
			}
			return errqnt;
		}

		//bool MergeClash(ClashDTO cl, string host)
		//{
		//	IClashService csrc = new ClashService<IClashService>(host);
		//	return csrc.MergeClash(cl);
			
		//}

		IEnumerable<RevitElementDTO> GetElements(IEnumerable<ClashDTO> clashes, IEnumerable<RevitModelDTO> models, IEnumerable<RevitCategoryDTO> cats)
		{
			var el1 = clashes.Select(x => new RevitElementDTO() { Name = x.RevitElement1Name, RevitId = x.RevitElement1Id, CategoryName = x.CategoryElement1Name, ModelName = x.RevitModel1Name, Level = x.Element1Level }).Distinct().ToList();
			var el2 = clashes.Select(x => new RevitElementDTO() { Name = x.RevitElement2Name, RevitId = x.RevitElement2Id, CategoryName = x.CategoryElement2Name, ModelName = x.RevitModel2Name, Level = x.Element2Level }).Distinct().ToList();

			var _elems = el1.Union(el2).ToList(); ;

			return from t1 in _elems
				   join t2 in models on t1.ModelName equals t2.Name
				   join t3 in cats on t1.CategoryName equals t3.Name
				   select new RevitElementDTO()
				   {
					   RevitId = t1.RevitId,
					   Name = t1.Name,
					   CategoryId = t3.Id,
					   RevitModelId = t2.Id,
					   Level = t1.Level,
					   ModelName = t1.ModelName
				   };
		}


		private IEnumerable<RevitCategoryDTO> GetCategories(IEnumerable<ClashDTO> clashes)
		{
			var items1 = clashes.Select(x => new RevitCategoryDTO() { Name = x.CategoryElement1Name }).ToList().Distinct();
			var items2 = clashes.Select(x => new RevitCategoryDTO() { Name = x.CategoryElement2Name }).ToList().Distinct();
			return items1.Union(items2).ToList();
		}

		private IEnumerable<RevitModelDTO> GetRevitModels(IEnumerable<ClashDTO> clashes)
		{
			var items1 = clashes.Select(x => new RevitModelDTO() { Name = x.RevitModel1Name }).ToList().Distinct();
			var items2 = clashes.Select(x => new RevitModelDTO() { Name = x.RevitModel2Name }).ToList().Distinct();
			return items1.Union(items2).ToList();
		}

		private IEnumerable<ClashDTO> GetClashes(IEnumerable<ClashTest> navisChecks, int constructionId)
		{
			IEnumerable<ClashDTO> result = new List<ClashDTO>();
			foreach (ClashTest check in navisChecks.AsParallel())
			{
				var _resilt = GetClashes(check, constructionId);
				result = result.Concat(_resilt);
			}
			return result.ToList();
		}

		private IEnumerable<ClashDTO> GetClashes(ClashTest navisCheck, int constructionId)
		{
			ICollection<ClashDTO> result = new List<ClashDTO>();
			foreach (SavedItem clash in navisCheck.Children.AsParallel())
			{
				try
				{
					if (clash is ClashResult _clash)
					{
						var dto = GetClashDTO(_clash, navisCheck.DisplayName, constructionId);
						result.Add(dto);
					}
					
				}
				catch (Exception ex)
				{
					continue;
				}
			}
			return result;
		}

		private ClashDTO GetClashDTO(ClashResult clash, string checkname, int constructionId)
		{
			return new ClashDTO()
			{
				CheckName = checkname,
				ClashStatusName = clash.Status.ToString(),
				ClashStatusId = (int)clash.Status + 1,
				RevitModel1Name = GetRevitModel(clash.CompositeItem1), 
				RevitModel2Name = GetRevitModel(clash.CompositeItem2),
				RevitElement1Name = GetElementName(clash.CompositeItem1),
				RevitElement2Name = GetElementName(clash.CompositeItem2),
				CategoryElement1Name = GetCategory(clash.CompositeItem1),
				CategoryElement2Name = GetCategory(clash.CompositeItem2),
				Distance = ConvertToMetreDistance(clash.Distance),
				X = ConvertToMetre(clash.Center.X),
				Y = ConvertToMetre(clash.Center.Y),
				Z = ConvertToMetre(clash.Center.Z), 
				RevitElement1Id = GetElementId(clash.CompositeItem1),
				RevitElement2Id = GetElementId(clash.CompositeItem2), 
				ConstructionId = constructionId,
				Element1Level = GetElementLevel(clash.CompositeItem1),
				Element2Level = GetElementLevel(clash.CompositeItem2)
			};
		}

		#region test

		private void WriteParans()
		{
			foreach (ClashTest test in Application.MainDocument.GetClash().TestsData.Tests)
			{
				int i = 0;
				foreach (ClashResult clash in test.Children)
				{
					WriteParams(clash.CompositeItem1, i);
					i++;
					WriteParams(clash.CompositeItem2, i);
					i++;
				}
				
			}
		}


		private void WriteParams(ModelItem item, int i)
		{
			foreach (var c in item.PropertyCategories)
			{
				foreach (var p in c.Properties)
				{
					var str = p.Value.IsDisplayString ? p.Value.ToDisplayString() : $"no-display - {p.Value.ToString()}" ;
					BTextWriter.WriteLog($"{i} | {c.Name} | {p.Name} | {str}");
				}
			}
		}

		#endregion test


		private string GetRevitModel(ModelItem item)
		{
			try
			{
				return item.PropertyCategories.FindPropertyByName("LcOaNode", "LcOaNodeSourceFile").Value.ToDisplayString();
			}
			catch (Exception ex)
			{
				return "NoModel";
			}
		}


		private int GetElementId(ModelItem item)
		{
			try
			{
				return int.Parse(item.PropertyCategories.FindPropertyByName("LcRevitId", "LcOaNat64AttributeValue").Value.ToDisplayString());
			}
			catch (Exception ex)
			{
				return 0;
			}
		}



		private string GetCategory(ModelItem item)
		{
			try
			{
				return item.PropertyCategories.FindPropertyByName("LcOaNode", "LcOaSceneBaseClassUserName")
			.Value.ToDisplayString().Split(new[] { ":" }, StringSplitOptions.None).First();
			}
			catch (Exception ex)
			{
				return "NoCategory";
			}
		}


		private string GetElementName(ModelItem item)
		{
			try
			{
				return item.PropertyCategories.FindPropertyByName("LcOaNode", "LcOaSceneBaseUserName").Value.ToDisplayString();
			}
			catch (Exception ex)
			{
				return "NoName";
			}
		}

		private string GetElementLevel(ModelItem item)
		{
			try
			{
				return item.PropertyCategories.FindPropertyByName("LcOaNode", "LcOaNodeLayer").Value.ToDisplayString();
			}
			catch (Exception ex)
			{
				return "NoLevel";
			}
		}

		private double ConvertToMetre(double coordinate) =>
			Math.Round(coordinate / 3.28085, 3);

		private double ConvertToMetreDistance(double coordinate) =>
			Math.Round(coordinate / 3.27488, 3);

		#endregion clashes
	}


}

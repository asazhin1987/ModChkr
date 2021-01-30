using Coordinator.DTO;
using Coordinator.DTO.Infrastructure;
using Coordinator.Forms.Commands;
using Coordinator.Forms.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Resources;

namespace Coordinator.Forms.Applications
{
	public class ClashesApplication : INotifyPropertyChanged
	{
		public string HelpAddress { get; set; }
		private ClashDTO selectedClash;
		public ClashDTO SelectedClash { get { return selectedClash; }
			set { selectedClash = value; OnPropertyChanged(nameof(ClashButtonsEnabled)); OnPropertyChanged(nameof(StatusBtnEnable)); } }

		public bool ClashButtonsEnabled { get { return SelectedClash != null; } }
		public ClashFilterDTO ClashFilter { get; set; }

		//Clash Data
		public ObservableCollection<ClashDTO> Clashes { get; set; }

		//Pagination
		public int PriviewPage { get { return CurrentPage - 1; } }
		public bool PriviewPageEnable { get { return PriviewPage != 0; } }
		public int CurrentPage { get { return ClashFilter.PageNum; } }
		public int NextPage { get { return CurrentPage + 1; } }
		public bool NextPageEnable { get { return NextPage <= PagesQnt; } }
		public int PagesQnt { get; set; }
		public int ClashesQnt { get; set; }

		//Disabled 

		private bool statusBtnEnable = true;

		public bool UpdateBtnEnable { get; set; } = true;
		public bool FilterBtnEnable { get; set; } = true;

		public bool StatusBtnEnable { get { return ClashButtonsEnabled && statusBtnEnable; } set { statusBtnEnable = value; OnPropertyChanged(nameof(StatusBtnEnable)); } }

		//Cut3D
		private bool cut3dView;
		public bool Cut3dView { get { return cut3dView; } set { cut3dView = value; CutViewCommand.Execute(null); OnPropertyChanged(nameof(Cut3dView)); } }
		private double cutVavue;
		public double CutValue { get { return cutVavue; } set { cutVavue = value; CutViewCommand.Execute(null); OnPropertyChanged(nameof(CutValue)); } }

		public bool ShowCut3DView { get; set; } = true;

		#region Events
		public EventHandler<CoordinatorFilterEventArgs> FilterChanged;
		public EventHandler<CoordinatorEventArgs> ShowElement;
		public EventHandler<CoordinatorEventArgs> CutView;
		public EventHandler<CoordinatorEventArgs> SetStatus;
		public EventHandler<CoordinatorFilterEventArgs> SetFilters;
		public EventHandler<CoordinatorFilterEventArgs> GetExcelCurrentPage;
		public EventHandler<CoordinatorFilterEventArgs> GetExcelAllPages;

		private RelayCommand updateCommand;
		private RelayCommand setFilterCommand;
		private RelayCommand previewPageCommand;
		private RelayCommand nextPageCommand;
		private RelayCommand showElementCommand;
		private RelayCommand setStateCommand;
		private RelayCommand setFilterFormCommand;
		private RelayCommand cutViewCommand;
		private RelayCommand getCurrPageExcelCommand;
		private RelayCommand getAllPagesExcelCommand;
		private RelayCommand helpCommand;

		public RelayCommand HelpCommand
		{
			get
			{
				return helpCommand ??
					(helpCommand = new RelayCommand(obj =>
					{
						try
						{
							Process.Start(HelpAddress);
						}
						catch (Exception ex)
						{
							MessageBox.Show(ex.Message);
						}
						
					}));
			}
		}

		public RelayCommand GetCurrPageExcelCommand
		{
			get
			{
				return getCurrPageExcelCommand ??
					(getCurrPageExcelCommand = new RelayCommand(obj =>
					{
						GetExcelCurrentPage?.Invoke(this, new CoordinatorFilterEventArgs(ClashFilter) { });
					}));
			}
		}

		public RelayCommand GetAllPagesExcelCommand
		{
			get
			{
				return getAllPagesExcelCommand ??
					(getAllPagesExcelCommand = new RelayCommand(obj =>
					{
						GetExcelAllPages?.Invoke(this, new CoordinatorFilterEventArgs(ClashFilter) { });
					}));
			}
		}

		public RelayCommand SetFilterFormCommand
		{
			get
			{

				return setFilterFormCommand ??
					(setFilterFormCommand = new RelayCommand(obj =>
					{
						SetFilters?.Invoke(this, new CoordinatorFilterEventArgs(ClashFilter));
						UpdateCommand.Execute(null);
					}));
			}
		}

		public RelayCommand SetStateCommand
		{
			get
			{

				return setStateCommand ??
					(setStateCommand = new RelayCommand(obj =>
					{
						SetStatus?.Invoke(this, new CoordinatorEventArgs(SelectedClash));
						UpdateCommand.Execute(null);
					}));
			}
		}

		public RelayCommand ShowElenemtCommand
		{
			get
			{

				return showElementCommand ??
					(showElementCommand = new RelayCommand(obj =>
					{
						ShowElement?
						.Invoke(this, new CoordinatorEventArgs(SelectedClash, cut3dView, cutVavue) { });
						
					}));
			}
		}

		public RelayCommand CutViewCommand
		{
			get
			{

				return cutViewCommand ??
					(cutViewCommand = new RelayCommand(obj =>
					{
						if(cut3dView)
							CutView?
							.Invoke(this, new CoordinatorEventArgs(SelectedClash, cut3dView, cutVavue));
					}));
			}
		}

		public RelayCommand UpdateCommand
		{
			get
			{
				
				return updateCommand ??
					(updateCommand = new RelayCommand(obj =>
					{
						FilterChanged?.Invoke(this, new CoordinatorFilterEventArgs(ClashFilter) { });
						OnPropertyChanged(nameof(Clashes));
						OnPropertyChanged(nameof(CurrentPage));
						OnPropertyChanged(nameof(ClashesQnt));
						OnPropertyChanged(nameof(NextPageEnable));
						OnPropertyChanged(nameof(PriviewPageEnable));
						OnPropertyChanged(nameof(PagesQnt));
						OnPropertyChanged(nameof(ClashButtonsEnabled));
					}));
			}
		}

		public RelayCommand SetFilterCommand
		{
			get
			{
				return setFilterCommand ??
					(setFilterCommand = new RelayCommand(obj =>
					{
						ClashFilter.PageNum = 1;
						UpdateCommand.Execute(null);
					}));
			}
		}


		public RelayCommand NextPageCommand
		{
			get
			{
				return nextPageCommand ??
					(nextPageCommand = new RelayCommand(obj =>
					{
						ClashFilter.PageNum++;
						UpdateCommand.Execute(null);
					}));
			}
		}

		public RelayCommand PreviewPageCommand
		{
			get
			{
				return previewPageCommand ??
					(previewPageCommand = new RelayCommand(obj =>
					{
						ClashFilter.PageNum--;
						UpdateCommand.Execute(null);
					}));
			}
		}

		#endregion Events

		#region Columns Checks

		private bool showSider;
		public bool ShowSider { get { return showSider; } set { showSider = value; OnPropertyChanged(nameof(ShowSider)); } }

		public bool siderChecked = true;
		public bool SiderChecked { get { return siderChecked; } set { siderChecked = value; OnPropertyChanged(nameof(ColumnsFilterWidth)); } }

		public double ColumnsFilterWidth { get { return SiderChecked ? 250 : 0; } }

		//Visibility Columns
		private bool showId = true;
		private bool showConstruction  = true;
		private bool showCheck = true;
		private bool showStatus  = true;
		private bool showModel1  = true;
		private bool showModel2  = true;
		private bool showElement1Id  = true;
		private bool showElement2Id = true;
		private bool showElement1Name;
		private bool showElement2Name;
		private bool showLevel1;
		private bool showLevel2;
		private bool showCategory1;
		private bool showCategory2;
		private bool showDistansion = true;
		private bool showFoundDate;
		private bool showOdate;
		private bool showPoint;


		public bool ShowId { get { return showId; } set { showId = value; OnPropertyChanged(nameof(ShowId)); } } 
		public bool ShowConstruction { get { return showConstruction; } set { showConstruction = value; OnPropertyChanged(nameof(ShowConstruction)); } }
		public bool ShowCheck { get { return showCheck; } set { showCheck = value; OnPropertyChanged(nameof(ShowCheck)); } }
		public bool ShowStatus { get { return showStatus; } set { showStatus = value; OnPropertyChanged(nameof(ShowStatus)); } }
		public bool ShowModel1 { get { return showModel1; } set { showModel1 = value; OnPropertyChanged(nameof(ShowModel1)); } }
		public bool ShowModel2 { get { return showModel2; } set { showModel2 = value; OnPropertyChanged(nameof(ShowModel2)); } }
		public bool ShowElement1Id { get { return showElement1Id; } set { showElement1Id = value; OnPropertyChanged(nameof(ShowElement1Id)); } }
		public bool ShowElement2Id { get { return showElement2Id; } set { showElement2Id = value; OnPropertyChanged(nameof(ShowElement2Id)); } }
		public bool ShowElement1Name { get { return showElement1Name; } set { showElement1Name = value; OnPropertyChanged(nameof(ShowElement1Name)); } }
		public bool ShowElement2Name { get { return showElement2Name; } set { showElement2Name = value; OnPropertyChanged(nameof(ShowElement2Name)); } }
		public bool ShowLevel1 { get { return showLevel1; } set { showLevel1 = value; OnPropertyChanged(nameof(ShowLevel1)); } }
		public bool ShowLevel2 { get { return showLevel2; } set { showLevel2 = value; OnPropertyChanged(nameof(ShowLevel2)); } }
		public bool ShowCategory1 { get { return showCategory1; } set { showCategory1 = value; OnPropertyChanged(nameof(ShowCategory1)); } }
		public bool ShowCategory2 { get { return showCategory2; } set { showCategory2 = value; OnPropertyChanged(nameof(ShowCategory2)); } }
		public bool ShowDistansion { get { return showDistansion; } set { showDistansion = value; OnPropertyChanged(nameof(ShowDistansion)); } }
		public bool ShowFoundDate { get { return showFoundDate; } set { showFoundDate = value; OnPropertyChanged(nameof(ShowFoundDate)); } }
		public bool ShowOdate { get { return showOdate; } set { showOdate = value; OnPropertyChanged(nameof(ShowOdate)); } }
		public bool ShowPoint { get { return showPoint; } set { showPoint = value; OnPropertyChanged(nameof(ShowPoint)); } }

		#endregion Columns Checks

		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName]string prop = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
		}
	}
}

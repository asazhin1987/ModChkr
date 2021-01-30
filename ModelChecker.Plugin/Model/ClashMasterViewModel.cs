using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using ModelChecker.Plugin.Commands;
using System.Collections.Generic;
using System;
using System.Linq;

namespace ModelChecker.Plugin.Model
{
	public class ClashMasterViewModel : INotifyPropertyChanged
	{
		public ObservableCollection<ConstructionViewModel> Constructions { get; set; }
		public ObservableCollection<CheckViewModel> Checks { get; set; }
		public IEnumerable<CheckViewModel> SelectedChecks { get { return Checks.Where(x => x.Checked == true).ToList(); } }

		
		public bool CheckAll { get; set; }
		public bool HasSelected { get { return SelectedChecks.Count() > 0; } }

		private ConstructionViewModel selectedConstruction;

		public  event EventHandler<EventArgs> MasterComplete;

		public ConstructionViewModel SelectedConstruction
		{
			get { return selectedConstruction; }
			set
			{
				selectedConstruction = value;
				OnPropertyChanged("SelectedConstruction");
			}
		}

		private RelayCommand checkAllCommand;
		private RelayCommand completeCommand;

		public RelayCommand CompleteCommand
		{
			get
			{
				return completeCommand ?? (completeCommand = new RelayCommand(obj =>
				{
					MasterComplete?.Invoke(this, new EventArgs() { });
				}));
			}
		}


		public RelayCommand CheckAllCommand
		{
			get
			{
				return checkAllCommand ??
					(checkAllCommand = new RelayCommand(obj =>
					{
						foreach (var check in Checks)
							check.Checked = CheckAll;
					}));
			}
		}

		public ClashMasterViewModel(ObservableCollection<ConstructionViewModel> constructions, ObservableCollection<CheckViewModel> checks)
		{
			Constructions = constructions;
			Checks = checks;
			CheckAll = true;
			SelectedConstruction = Constructions.First();
		}

		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName]string prop = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
		}
	}
}

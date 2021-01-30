using Coordinator.DTO;
using Coordinator.DTO.Infrastructure;
using Coordinator.Forms.Commands;
using Coordinator.Forms.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;



namespace Coordinator.Forms.Applications
{

	public class StatusApplication : INotifyPropertyChanged
	{
		public ObservableCollection<StatusDTO> Statuses { get; set; }
		public StatusDTO SelectedStatus { get; set; }
		public bool OkResult { get; set; }

		private RelayCommand cancesCommand;
		private RelayCommand okCommand;

		public RelayCommand CancelCommand
		{
			get
			{
				return cancesCommand ??
					(cancesCommand = new RelayCommand(obj =>
					{
						OkResult = false;
					}));
			}
		}

		public RelayCommand OkCommand
		{
			get
			{
				return okCommand ??
					(okCommand = new RelayCommand(obj =>
					{
						OkResult = true;
					}));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName]string prop = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
		}
	}
}

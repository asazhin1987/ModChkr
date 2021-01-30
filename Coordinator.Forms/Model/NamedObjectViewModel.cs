using Coordinator.DTO;
using Coordinator.DTO.Infrastructure;
using Coordinator.Forms.Commands;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace Coordinator.Forms.Model
{
	public class NamedObjectViewModel : INotifyPropertyChanged
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public EventHandler<EventArgs> ParentChanged;

		private bool _checkk;

		public bool Check
		{
			get { return _checkk; }
			set
			{
				_checkk = value;
				OnPropertyChanged("Check");
				ParentChanged?.Invoke(this, null );
			}
		}
		public NamedObjectViewModel(INamedObject item, EventHandler<EventArgs> _changed, bool _checked)
		{
			Id = item.Id;
			Name = item.Name;
			_checkk = _checked;
			ParentChanged = _changed;
		}

		private RelayCommand checkCommand;
		public RelayCommand CheckCommand
		{
			get
			{
				return checkCommand ?? (checkCommand = new RelayCommand(obj =>
				{
					var _ch = obj as bool?;
					if (_ch != null)
						Check = _ch.Value;

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

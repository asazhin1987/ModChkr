using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ModelChecker.Plugin.Commands;

namespace ModelChecker.Plugin.Model
{
	public class CheckViewModel : INotifyPropertyChanged
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public Guid Guid { get; set; }

		private bool selected;

		public bool Checked
		{
			get { return selected; }
			set
			{
				selected = value;
				OnPropertyChanged("Checked");
			}
		}
		public override string ToString()
		{
			return Name.ToString();
		}

		public override int GetHashCode()
		{
			return ToString().GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj is CheckViewModel _ch)
				return this.ToString() == _ch.ToString();
			return false;
		}

		//public event EventHandler<EventArgs> CheckChanged;
		private RelayCommand checkCommand;
		public RelayCommand CheckCommand
		{
			get
			{
				return checkCommand ?? (checkCommand = new RelayCommand(obj =>
				{
					var _ch = obj as bool?;
					if (_ch != null)
						Checked = _ch.Value;
					
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

using ModelChecker.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ModelChecker.Plugin.Model
{
	public class ConstructionViewModel : INotifyPropertyChanged
	{
		private int _Id;
		private string _name;

		public int Id 
		{
			get
			{
				return _Id;
			}
			set
			{
				_Id = value;
				OnPropertyChanged(nameof(_Id));
			}
		}
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
				OnPropertyChanged(nameof(_name));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName]string prop = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
		}

		public ConstructionViewModel() { }

		public ConstructionViewModel(ConstructionDTO item)
		{
			Id = item.Id;
			Name = item.Name;
		}

		public static implicit operator ConstructionViewModel(ConstructionDTO item) =>
			new ConstructionViewModel(item);
	}
}

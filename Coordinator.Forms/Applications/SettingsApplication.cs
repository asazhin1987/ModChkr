﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using System;
using Coordinator.Forms.Commands;

namespace Coordinator.Forms.Model
{
	public class SettingsViewModel : INotifyPropertyChanged
	{
		public string Host { get; set; }

		private RelayCommand seveHostCommand;
		private RelayCommand checkHostCommand;

		public event EventHandler<EventArgs> CheckHost;
		public event EventHandler<EventArgs> SaveHost;

		public RelayCommand SeveHostCommand
		{
			get
			{
				return seveHostCommand ?? (seveHostCommand = new RelayCommand(obj =>
				{
					SaveHost?.Invoke(this, new EventArgs() { });
				}));
			}
		}


		public RelayCommand CheckHostCommand
		{
			get
			{
				return checkHostCommand ??
					(checkHostCommand = new RelayCommand(obj =>
					{
						CheckHost?.Invoke(this, new EventArgs() { });
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

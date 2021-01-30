using Coordinator.DTO;
using Coordinator.DTO.Infrastructure;
using Coordinator.Forms.Commands;
using Coordinator.Forms.Model;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Coordinator.Forms.Applications
{
	public class HTMLReportApplication : INotifyPropertyChanged
	{
		public Uri HTMLFilePath { get; set; }
		public int? ElementId { get; set; }
		private  ClashDTO Clash { get {
				return new ClashDTO()
				{
					RevitElement1Id = ElementId ?? 0, UndercutView3d = Cut3dView, Offset = CutValue
				};
			} }
		//Cut3D
		private bool cut3dView;
		public bool Cut3dView { get { return cut3dView; } set { cut3dView = value; CutViewCommand.Execute(null); OnPropertyChanged(nameof(Cut3dView)); } }
		private double cutVavue;
		public double CutValue { get { return cutVavue; } set { cutVavue = value; CutViewCommand.Execute(null); OnPropertyChanged(nameof(CutValue)); } }

		public HTMLReportApplication( string _path)
		{
			HTMLFilePath = new Uri(_path);
		}

		private RelayCommand showElementCommand;
		private RelayCommand cutViewCommand;

		public EventHandler<CoordinatorEventArgs> ShowElement;
		public EventHandler<CoordinatorEventArgs> CutView;

		public RelayCommand ShowElenemtCommand
		{
			get
			{

				return showElementCommand ??
					(showElementCommand = new RelayCommand(obj =>
					{
						ShowElement?
						.Invoke(this, new CoordinatorEventArgs(Clash, cut3dView, cutVavue) { });

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
						if (cut3dView)
							CutView?
								.Invoke(this, new CoordinatorEventArgs(Clash, cut3dView, cutVavue));
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

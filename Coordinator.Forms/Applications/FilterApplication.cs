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
using System.Windows;

namespace Coordinator.Forms.Applications
{
	public class FilterApplication : INotifyPropertyChanged
	{


		public FilterApplication(ClashFilterDTO dto, IEnumerable<ConstructionDTO> constructions, IEnumerable<CheckDTO> checks, IEnumerable<RevitModelDTO> models, IEnumerable<StatusDTO> stetuses)
		{
			DistanceOrder = dto.Sorts == Sorts.Distantion;
			CreateDateOrder = dto.Sorts == Sorts.CreateDate;
			OdateOrder = dto.Sorts == Sorts.ChangeDate;
			AscendingOrder = dto.Orders == Orders.Ascending;
			DescendingOrder = dto.Orders == Orders.Descending;

			StatusApplication = new NamedObjectApplication(stetuses, dto.Statuses);
			ModelApplication = new NamedObjectApplication(models, dto.RevitModels, dto.Checks, ModelChanged);
			CheckApplication = new NamedObjectApplication(checks, dto.Checks, dto.Constructions, CheckChanged);
			ConstructionApplication = new NamedObjectApplication(constructions, dto.Constructions, null, ConstructionChanged);
			
			
		}

		public Sorts Sorts
		{
			get
			{
				if (DistanceOrder)
					return Sorts.Distantion;
				else if (CreateDateOrder)
					return Sorts.CreateDate;
				else if (OdateOrder)
					return Sorts.ChangeDate;
				else 
					return Sorts.Default;
			}
		}

		public Orders Orders
		{
			get
			{
				if (AscendingOrder)
					return Orders.Ascending;
				else if (DescendingOrder)
					return Orders.Descending;
				else
					return Orders.Default;
			}
		}


		public NamedObjectApplication ConstructionApplication { get; set; }
		public NamedObjectApplication CheckApplication { get; set; }
		public NamedObjectApplication ModelApplication { get; set; }
		public NamedObjectApplication StatusApplication { get; set; }

		public bool OkResult { get; set; }

		private RelayCommand cancesCommand;
		private RelayCommand okCommand;
		private RelayCommand breakCommand;



		private void ConstructionChanged(object sender, ParentFilterChangetEventArgs e)
		{
			CheckApplication.Parents = e.Parents;
		}

		private void CheckChanged(object sender, ParentFilterChangetEventArgs e)
		{
			if (e.Parents != null && e.Parents.Count() > 0)
				ModelApplication.Parents = e.Parents;
			else
				ModelApplication.Parents = ((NamedObjectApplication)sender).ObjectItemIds;
		}

		private void ModelChanged(object sender, ParentFilterChangetEventArgs e)
		{
			//MessageBox.Show("ModelChanged");
		}

		private void StetusChanged(object sender, ParentFilterChangetEventArgs e)
		{
			//MessageBox.Show("StetusChanged");
		}



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

		public RelayCommand BreakCommand
		{
			get
			{
				return breakCommand ??
					(breakCommand = new RelayCommand(obj =>
					{
						ConstructionApplication.BreakFilter();
						CheckApplication.BreakFilter();
						ModelApplication.BreakFilter();
						StatusApplication.BreakFilter();
						OkResult = true;
					}));
			}
		}


		#region Sort

		private bool distanceOrder;
		private bool createDateOrder;
		private bool odateOrder;
		private bool ascendingOrder;
		private bool descendingOrder;

		public bool DistanceOrder
		{
			get { return distanceOrder; }
			set
			{
				distanceOrder = value;
				if (value == true)
				{
					CreateDateOrder = false;
					OdateOrder = false;
				}
				OnPropertyChanged(nameof(DistanceOrder));
			}
		}

		public bool CreateDateOrder
		{
			get { return createDateOrder; }
			set
			{
				createDateOrder = value;
				if (value == true)
				{
					DistanceOrder = false;
					OdateOrder = false;
				}
				OnPropertyChanged(nameof(CreateDateOrder));
			}
		}
		public bool OdateOrder
		{
			get { return odateOrder; }
			set
			{
				odateOrder = value;
				if (value == true)
				{
					CreateDateOrder = false;
					DistanceOrder = false;
				}
				OnPropertyChanged(nameof(OdateOrder));
			}
		}


		public bool AscendingOrder
		{
			get { return ascendingOrder; }
			set
			{
				ascendingOrder = value;
				if (value == true)
				{
					DescendingOrder = false;
				}
				OnPropertyChanged(nameof(AscendingOrder));
			}
		}
		public bool DescendingOrder
		{
			get { return descendingOrder; }
			set
			{
				descendingOrder = value;
				if (value == true)
				{
					AscendingOrder = false;
				}
				OnPropertyChanged(nameof(DescendingOrder));
			}
		}

		#endregion Sort

		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName]string prop = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
		}
	}
}

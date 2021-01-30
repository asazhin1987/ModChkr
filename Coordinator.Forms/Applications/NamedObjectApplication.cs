using Coordinator.DTO;
using Coordinator.DTO.Infrastructure;
using Coordinator.Forms.Commands;
using Coordinator.Forms.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System;

namespace Coordinator.Forms.Applications
{
	public class NamedObjectApplication : INotifyPropertyChanged
	{
		private bool EnableEvents { get; set; } = true;

		public ObservableCollection<NamedObjectViewModel> ObjectItems { get; set; }
		public IEnumerable<int> SelectedItemIds { get { return ObjectItems.Where(x => x.Check == true).Select(s => s.Id).ToList(); } }
		public IEnumerable<int> ObjectItemIds { get { return ObjectItems.Select(x => x.Id).ToList(); ; } }

		private IEnumerable<int> parents;
		public IEnumerable<int> Parents { get { return parents; } set { parents = value; SetParentFilter(); ThisChanged(this, null); } }
		private IEnumerable<INamedObject> Items { get; }
		

		private EventHandler<ParentFilterChangetEventArgs> ParentChenged { get;  }
		private IEnumerable<int> Checked { get; }

		private bool checkAll;
		public bool CheckAll { get { return checkAll; }  set { checkAll = value; OnPropertyChanged(nameof(CheckAll)); SetCheck(); ThisChanged(this, null); } }

		public NamedObjectApplication(IEnumerable<INamedObject> _items, IEnumerable<int> _checked, 
			IEnumerable<int> parents = null, EventHandler<ParentFilterChangetEventArgs> _changed = null)
		{
			Items = _items;
			Checked = _checked;
			ParentChenged = _changed;
			Parents = parents;
		}

		private void SetParentFilter()
		{
			if (EnableEvents)
			{
				var _items = (Parents == null || Parents.Count() == 0)
					? Items
					: Items.Where(x => x.Parents != null && x.Parents.Intersect(parents).Count() > 0);
				ObjectItems = new ObservableCollection<NamedObjectViewModel>(_items.Select(x => new NamedObjectViewModel(x, ThisChanged, Checked.Contains(x.Id))));
				OnPropertyChanged(nameof(ObjectItems));
			}
		}

		private void SetCheck()
		{
			EnableEvents = false;
			foreach (var item in ObjectItems)
				item.Check = CheckAll;
			EnableEvents = true;
		}

		public void BreakFilter()
		{
			CheckAll = false;
		}


		private void ThisChanged(object sender, EventArgs e)
		{
			ParentChenged?.Invoke(this, new ParentFilterChangetEventArgs(SelectedItemIds));
		}


		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName]string prop = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
		}
	}
}

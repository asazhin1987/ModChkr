using System;
using System.Collections.Generic;

namespace Coordinator.DTO.Infrastructure
{
	public class CoordinatorEventArgs : EventArgs
	{
		public ClashDTO Clash { get; set; }

		public readonly string Message;
		public bool Success { get; set; }

		public CoordinatorEventArgs(ClashDTO clash, bool undercut = false, double offset = 0)
		{
			Success = true;
			Clash = clash;
			Clash.UndercutView3d = undercut;
			Clash.Offset = offset;
		}


		public CoordinatorEventArgs(string message, bool success = true) 
		{
			Message = message;
			Success = success;
		}
	}

	public class CoordinatorFilterEventArgs : EventArgs
	{
		public ClashFilterDTO ClashFilter { get; set; }

		public CoordinatorFilterEventArgs(ClashFilterDTO clashFilter)
		{
			ClashFilter = clashFilter;
		}
	}

	public class ParentFilterChangetEventArgs : EventArgs
	{
		public IEnumerable<int> Parents { get; set; }

		public ParentFilterChangetEventArgs(IEnumerable<int> parents)
		{
			Parents = parents;
		}
	}
}

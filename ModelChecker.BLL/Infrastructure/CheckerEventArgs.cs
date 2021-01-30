using System;

namespace ModelChecker.BLL.Infrastructure
{

	public class CheckerEventArgs : EventArgs
	{
		public readonly string Message;
		public bool Success { get; set; }

		public CheckerEventArgs(string message, bool success = true)
		{
			Message = message;
			Success = success;
		}

	}


	public class ServiceEventArgs : CheckerEventArgs
	{
		public ServiceEventArgs(string message, bool success = true) : base(message, success){}
	}

	//public class ScheduleEventArgs : CheckerEventArgs
	//{
	//	public ScheduleEventArgs(string message, bool success = true) : base(message, success) { }
	//}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ModelChecker.WEB.Models
{
	public class ResultActionViewModel
	{
		public bool Succees { get; set; }
		public string Message { get; set; }
		public string ActionResultFunction { get; set; }

		public ResultActionViewModel(string actionResult, bool success, string msg)
		{
			Succees = success;
			ActionResultFunction = actionResult;
			Message = msg;
		}
	}

	public class SucceedResultActionViewModel : ResultActionViewModel
	{
		public SucceedResultActionViewModel(string actionResult, string msg) : base(actionResult, true, msg)
		{
		}
	}

	public class FailureResultActionViewModel : ResultActionViewModel
	{
		public FailureResultActionViewModel(string actionResult, string msg) : base(actionResult, false, msg)
		{
		}
	}
}
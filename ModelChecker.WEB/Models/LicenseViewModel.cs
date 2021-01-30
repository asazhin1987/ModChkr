using ModelChecker.DTO;
using System;
using System.ComponentModel.DataAnnotations;

namespace ModelChecker.WEB.Models
{
	public class LicenseViewModel
	{
		[Display(Name = "LicenseKey", ResourceType = typeof(Resources.Global))]
		[Required(AllowEmptyStrings = true)]
		[StringLength(125)]
		public string Key { get; set; }
		public bool Success { get; }
		public string Message { get; }
		public string Caption { get { return Resources.Global.License; } }
		public int LicensseQnt { get;  }

		public LicenseViewModel()
		{
		}

		public LicenseViewModel(LicenseDTO item)
		{
			Success = item.Success;
			LicensseQnt = item.LicensseQnt;
			Key = item.Key;
			if (Success)
				Message = $"{Resources.Global.LicenseSuccessMsg} {item.EndLicDate.ToShortDateString()}";
			else
				Message = $"{Resources.Global.LicenseTimeOutMsg} {item.EndLicDate.ToShortDateString()}";
		}

		public LicenseViewModel(bool succes, string msg)
		{
			Success = succes;
			Message = msg;
		}

	}
}
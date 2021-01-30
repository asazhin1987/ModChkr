using ModelChecker.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ModelChecker.WEB.Models
{
	public class LicenseReportViewModel
	{
		public LicenseViewModel License { get; set; }
		public int AllQnt { get; set; }
		public int UsingQnt { get; set; }
		public int Last3MonthsQnt { get; set; }
		public IEnumerable<LicenseUsingDTO> Licenses { get; set; }
		public IEnumerable<LicenseMonthUsingDTO> LicenseUsings { get; set; }
	}
}
using ModelChecker.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ModelChecker.WEB.Models
{
	public class LicenseMonthsDiagramViewModel
	{
		public IEnumerable<string> Dates { get; }
		public IEnumerable<int> Qnter1 { get; }
		public IEnumerable<int> Qnter2 { get; }

		public LicenseMonthsDiagramViewModel(IEnumerable<LicenseMonthUsingDTO> items, int licqnt)
		{
			Qnter1 = items.Select(x => x.UniqQnt).ToList();
			Qnter2 = items.Select(x => licqnt).ToList();
			Dates = items.Select(x => new DateTime(x.Year, x.MonthNum, 1).ToString("yyyy MM")).ToList();
		}
	}
}
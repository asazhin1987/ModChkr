using ModelChecker.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ModelChecker.WEB.Models
{
	public class CompareViewModel
	{
		public IEnumerable<FullCheckDTO> Checks { get; set; }
		public IEnumerable<string> LeftChecks { get; set; }
		public IEnumerable<string> RightChecks { get; set; }

		public CompareViewModel(IEnumerable<FullCheckDTO> checks)
		{
			Checks = checks;
			LeftChecks = Checks.Select(x => x.LeftCheckName).Distinct();
			RightChecks = Checks.Select(x => x.RightCheckName).Distinct();
		}
	}
}
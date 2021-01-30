using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ModelChecker.DTO;


namespace ModelChecker.WEB.Models
{
	public class ChecksPageViewModel
	{
		public ConstructionDTO Construction { get; set; }
		public IEnumerable<FullCheckDTO> Checks { get; set; }



	}
}
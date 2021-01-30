using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ModelChecker.DTO;

namespace ModelChecker.WEB.Models
{
	public class StatusDashBoardViewModel
	{
		public IEnumerable<DailySliceDTO> Slices { get; set; }
		public StatusReportViewModel StatusPipe { get; set; }
	}
}
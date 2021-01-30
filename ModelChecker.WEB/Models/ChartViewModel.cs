using ModelChecker.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ModelChecker.WEB.Models
{

	public class ChartViewModel: StatusReportViewModel
	{
		public IEnumerable<DailySliceDTO> Slices { get; set; }
		public DailySliceDTO LastSlice { get; set; }
		public DateTime? SDate { get; set; }
		public DateTime? EDate { get; set; }


		public IEnumerable<FullCheckDTO> Checks { get; set; }
		public IEnumerable<int> SelectedChecks { get; set; }

		public ChartViewModel()
		{
		}

		public ChartViewModel(IEnumerable<DailySliceDTO> slices)
		{
			Slices = slices;
			EDate = slices.Max(x => x.Date);
			SDate = slices.Min(x => x.Date);
			LastSlice = slices.Where(x => x.Date == EDate).FirstOrDefault();

			if (LastSlice != null)
			{
				ClashesQnt = LastSlice.ClashesQnt;
				ActiveQnt = LastSlice.ActiveQnt;
				AnalizedQnt = LastSlice.AnalizedQnt;
				CorrectedQnt = LastSlice.CorrectedQnt;
				ConfirmedQnt = LastSlice.ConfirmedQnt;
				CreatedQnt = LastSlice.CreatedQnt;

				SetPercent();
			}
			
			
		}
	}
}
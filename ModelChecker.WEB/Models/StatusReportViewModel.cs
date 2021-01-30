using ModelChecker.DTO;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ModelChecker.WEB.Models
{
	public enum Orders {Name, Clashes, Create, Active, Analized, Confirmed, Corrected, Date }

	public class StatusReportViewModel
	{
		[Required]
		[Display(Name = "Name", ResourceType = typeof(Resources.Global))]
		[StringLength(125)]
		public string Name { get; set; }

		[ScaffoldColumn(false)]
		[Display(Name = "Date", ResourceType = typeof(Resources.Global))]
		public DateTime? Date { get; set; }

		[ScaffoldColumn(false)]
		[Display(Name = "Clashes", ResourceType = typeof(Resources.Global))]
		[DisplayFormat(DataFormatString = "{0:N0}")]
		public int ClashesQnt { get; set; }

		[ScaffoldColumn(false)]
		[DisplayFormat(DataFormatString = "{0:N0}")]
		public int AtWorkClashesQnt { get; set; }

		[ScaffoldColumn(false)]
		[DisplayFormat(DataFormatString = "{0:N0}")]
		public int CompletedClashesQnt { get; set; }

		[ScaffoldColumn(false)]
		[Display(Name = "StatusCreate", ResourceType = typeof(Resources.Global))]
		[DisplayFormat(DataFormatString = "{0:N0}")]
		public int CreatedQnt { get; set; }

		[ScaffoldColumn(false)]
		[Display(Name = "StatusActive", ResourceType = typeof(Resources.Global))]
		[DisplayFormat(DataFormatString = "{0:N0}")]
		public int ActiveQnt { get; set; }

		[ScaffoldColumn(false)]
		[Display(Name = "StatusAnalized", ResourceType = typeof(Resources.Global))]
		[DisplayFormat(DataFormatString = "{0:N0}")]
		public int AnalizedQnt { get; set; }

		[ScaffoldColumn(false)]
		[Display(Name = "StatusConfirmed", ResourceType = typeof(Resources.Global))]
		[DisplayFormat(DataFormatString = "{0:N0}")]
		public int ConfirmedQnt { get; set; }

		[ScaffoldColumn(false)]
		[Display(Name = "StatusCorrected", ResourceType = typeof(Resources.Global))]
		[DisplayFormat(DataFormatString = "{0:N0}")]
		public int CorrectedQnt { get; set; }

		


		public int ActiveQntPerc { get; set; }
		public int AnalizedQntPerc { get; set; }
		public int CorrectedQntPerc { get; set; }
		public int ConfirmedQntPerc { get; set; }
		public int CreatedQntPerc { get; set; }

		public decimal ActiveQntPercDec { get; set; }
		public decimal AnalizedQntPercDec { get; set; }
		public decimal CorrectedQntPercDec { get; set; }
		public decimal ConfirmedQntPercDec { get; set; }
		public decimal CreatedQntPercDec { get; set; }

		public StatusReportViewModel() { }

		public StatusReportViewModel(IEnumerable<ReportStatusDTO> items)
		{
			ClashesQnt = items.Sum(x => x.ClashesQnt);
			CreatedQnt = items.Sum(x => x.CreatedQnt);
			ActiveQnt = items.Sum(x => x.ActiveQnt);
			AnalizedQnt = items.Sum(x => x.AnalizedQnt);
			ConfirmedQnt = items.Sum(x => x.ConfirmedQnt);
			CorrectedQnt = items.Sum(x => x.CorrectedQnt);
			AtWorkClashesQnt = items.Sum(x => x.AtWorkClashesQnt);
			CompletedClashesQnt = items.Sum(x => x.CompletedClashesQnt);
			SetPercent();
		}

		public StatusReportViewModel(ReportStatusDTO item)
		{
			ClashesQnt = item.ClashesQnt;
			CreatedQnt = item.CreatedQnt;
			ActiveQnt = item.ActiveQnt;
			AnalizedQnt = item.AnalizedQnt;
			ConfirmedQnt = item.ConfirmedQnt;
			CorrectedQnt = item.CorrectedQnt;
			AtWorkClashesQnt = item.AtWorkClashesQnt;
			CompletedClashesQnt = item.CompletedClashesQnt;
			SetPercent();
		}

		internal void SetPercent()
		{
			if (ClashesQnt > 0)
			{
				ActiveQntPerc = GetPerc(ActiveQnt);
				AnalizedQntPerc = GetPerc(AnalizedQnt);
				CorrectedQntPerc = GetPerc(CorrectedQnt);
				ConfirmedQntPerc = GetPerc(ConfirmedQnt);
				CreatedQntPerc = GetPerc(CreatedQnt);
			}
			if (AtWorkClashesQnt > 0)
			{
				ActiveQntPercDec = GetPercWorkDec(ActiveQnt);
				CreatedQntPercDec = GetPercWorkDec(CreatedQnt);
			}
			if (CompletedClashesQnt > 0)
			{
				AnalizedQntPercDec = GetPercComplDec(AnalizedQnt);
				CorrectedQntPercDec = GetPercComplDec(CorrectedQnt);
				ConfirmedQntPercDec = GetPercComplDec(ConfirmedQnt);
			}
		}

		private int GetPerc(int qnt)
		{
			return (int)((decimal)qnt / ClashesQnt * 100);
		}

		private decimal GetPercWorkDec(int qnt)
		{
			return ((decimal)qnt / AtWorkClashesQnt);
		}

		private decimal GetPercComplDec(int qnt)
		{
			return ((decimal)qnt / CompletedClashesQnt);
		}
	}
}
using ModelChecker.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ModelChecker.WEB.Models
{
	public enum ReportType {Compare, Charts, CategoryMatrix, ModelMatrix }
	public class StatisticViewModel
	{
		public int ConstructionId { get; set; }
		public ConstructionDTO Construction { get; set; }
		public ReportType ReportType { get; set; }

		[Display(Name = "SDate", ResourceType = typeof(Resources.Global))]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime? SDate { get; set; }

		[Display(Name = "EDate", ResourceType = typeof(Resources.Global))]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime? EDate { get; set; }

		public IEnumerable<FullCheckDTO> AllChecks { get; set; }
		public IEnumerable<int> SelectedChecks { get; set; }
	}
}
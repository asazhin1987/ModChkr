using ModelChecker.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ModelChecker.WEB.Models
{
	public class ConstructionViewModel : StatusReportViewModel
	{
		public int Id { get; set; }

		[Display(Name = "Description", ResourceType = typeof(Resources.Global))]
		[StringLength(500, MinimumLength = 0)]
		public string Description { get; set; }


		public ConstructionViewModel() : base() { }

		public ConstructionViewModel(ConstructionDTO item) :base(item)
		{
			Id = item.Id;
			Name = item.Name;
			Description = item.Description;
			Date = item.Date;
			SetPercent();

		}

		public static implicit operator ConstructionViewModel(ConstructionDTO item) =>
			new ConstructionViewModel(item);
	}
}
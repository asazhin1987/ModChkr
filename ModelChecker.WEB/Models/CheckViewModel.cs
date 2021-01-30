using ModelChecker.DTO;
using ModelChecker.DTO.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ModelChecker.WEB.Models
{
	public class CheckViewModel : StatusReportViewModel, INamedObject
	{
		public int Id { get; set; }

		public CheckViewModel() : base() { }

		public CheckViewModel(FullCheckDTO item) : base(item)
		{
			Id = item.Id;
			Name = item.Name;

			Date = item.Date;
			SetPercent();
		}

		public static implicit operator CheckViewModel(FullCheckDTO item) =>
			new CheckViewModel(item);
	}
}

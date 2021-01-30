using ModelChecker.DTO;
using System.ComponentModel.DataAnnotations;

namespace ModelChecker.WEB.Models
{
	public class ParamsViewModel
	{
		[Required]
		[Display(Name = "Delimiter", ResourceType = typeof(Resources.Global))]
		[StringLength(10)]
		public string Delimiter { get; set; }

		[Range(1, 120)]
		[Display(Name = "DatRetentionPeriod", ResourceType = typeof(Resources.Global))]
		public int DatRetentionPeriod { get; set; }


		public ParamsViewModel() { }

		public ParamsViewModel(ParamsDTO item)
		{
			Delimiter = item.Delimiter;
			DatRetentionPeriod = item.DatRetentionPeriod;
		}
	}
}
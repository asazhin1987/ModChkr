using System.ComponentModel.DataAnnotations;

namespace ModelChecker.DAL.Entities
{
	public class Params
	{
		public int Id { get; set; }
		[StringLength(10)]
		public string Delimiter { get; set; }
		public int DatRetentionPeriod { get; set; }
	}
}

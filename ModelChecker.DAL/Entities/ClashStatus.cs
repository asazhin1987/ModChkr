using System.ComponentModel.DataAnnotations;
namespace ModelChecker.DAL.Entities
{
	public class ClashStatus
	{
		public int Id { get; set; }

		[StringLength(125)]
		public string Name { get; set; }
	}
}

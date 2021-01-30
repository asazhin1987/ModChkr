using System.ComponentModel.DataAnnotations;

namespace ModelChecker.DAL.Entities
{
	public class Setting
	{
		[StringLength(125)]
		[Key]
		public string Key { get; set; }
	}
}

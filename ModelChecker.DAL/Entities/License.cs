
using System.ComponentModel.DataAnnotations;

namespace ModelChecker.DAL.Entities
{
	public class License
	{
		[StringLength(125)]
		[Key]
		public string Key { get; set; }
	}
}

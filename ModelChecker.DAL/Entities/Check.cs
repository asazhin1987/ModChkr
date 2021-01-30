using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelChecker.DAL.Entities
{
	public class Check
	{
		public int Id { get; set; }

		//[StringLength(50)]
		//[Index(IsClustered = false, IsUnique = true)]
		//public string Name { get; set; }

		//public int FullCheckId { get; set; }
		//public FullCheck FullCheck { get; set; }


	}
}

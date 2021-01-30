using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace ModelChecker.DAL.Entities
{
	public class FullCheck
	{
		public int Id { get; set; }

		[StringLength(125)]
		[Index(IsClustered = false, IsUnique = false)]
		public string Name { get; set; }

		public int ConstructionId { get; set; }
		public Construction Construction { get; set; }

		[StringLength(50)]
		[Index(IsClustered = false, IsUnique = false)]
		public string LeftName { get; set; }

		[StringLength(50)]
		[Index(IsClustered = false, IsUnique = false)]
		public string RightName { get; set; } 

		public DateTime? Odate { get; set; }

		public ICollection<DailySlice> DailySlices { get; set; }

		//public int? Test { get; set; }

		public FullCheck()
		{
			DailySlices = new List<DailySlice>();
		}
	}
}

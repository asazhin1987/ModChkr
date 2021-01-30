using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ModelChecker.DAL.Entities
{
	public class Construction
	{
		public int Id { get; set; }

		[StringLength(125)]
		public string Name { get; set; }

		[StringLength(500)]
		public string Description { get; set; }

		public DateTime? Odate { get; set; }

		public ICollection<FullCheck> FullChecks { get; set; }

		public Construction()
		{
			FullChecks = new List<FullCheck>();
		}

	}
}

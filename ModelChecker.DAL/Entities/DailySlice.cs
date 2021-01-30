using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelChecker.DAL.Entities
{
	public  class DailySlice
	{
		[Key, Column(Order = 0)]
		public DateTime Date { get; set; }

		[Key, Column(Order = 1)]
		public int FullCheckId { get; set; }
		public FullCheck FullCheck { get; set; }

		[Index(IsClustered = false, IsUnique = false)]
		public int ConstructionId { get; set; }

		public int ClashesQnt { get; set; }
		public int ActiveQnt { get; set; }
		public int AnalizedQnt { get; set; }
		public int CorrectedQnt { get; set; }
		public int ConfirmedQnt { get; set; }
		public int CreatedQnt { get; set; }
		public DateTime? Odate { get; set; }

	}
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelChecker.DAL.Entities
{
	public class Clash
	{
		public int Id { get; set; }

		public int FullCheckId { get; set; }
		public FullCheck FullCheck { get; set; }

		//[Index(IsClustered = false, IsUnique = false)]
		//public int Check1Id { get; set; }

		//[Index(IsClustered = false, IsUnique = false)]
		//public int Check2Id { get; set; }

		[Index(IsClustered = false, IsUnique = false)]
		public int ConstructionId { get; set; }

		public int? ClashStatusId { get; set; }
		public ClashStatus ClashStatus { get; set; }

		public int? RevitModel1Id { get; set; }
		public RevitModel RevitModel1 { get; set; } 

		public int? RevitModel2Id { get; set; }
		public RevitModel RevitModel2 { get; set; }

		public int? RevitElement1Id { get; set; }
		public RevitElement RevitElement1 { get; set; }

		public int? RevitElement2Id { get; set; }
		public RevitElement RevitElement2 { get; set; }


		public int? CategoryElement1Id { get; set; }
		public RevitCategory CategoryElement1 { get; set; }

		public int? CategoryElement2Id { get; set; }
		public RevitCategory CategoryElement2 { get; set; }


		public DateTime FoundDate { get; set; }
		public DateTime Odate { get; set; }

		[StringLength(1)]
		public string Act { get; set; }

		public double Distance { get; set; }
		public double X { get; set; }
		public double Y { get; set; }
		public double Z { get; set; }

		//[StringLength(25)]
		//public string Grid { get; set; }
	}
}

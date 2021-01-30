using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelChecker.DAL.Entities
{
	public class RevitElement
	{
		public int Id { get; set; }

		[Index(IsClustered = false, IsUnique = false)]
		public int RevitId { get; set; }

		[StringLength(255)]
		public string Name { get; set; }

		[StringLength(255)]
		public string Level { get; set; }

		public int RevitModelId { get; set; }
		public RevitModel RevitModel { get; set; }

		public int? RevitCategoryId { get; set; }
		public RevitCategory RevitCategory { get; set; }
	}
}

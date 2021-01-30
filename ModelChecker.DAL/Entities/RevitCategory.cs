using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ModelChecker.DAL.Entities
{
	public class RevitCategory
	{
		public int Id { get; set; }

		[StringLength(255)]
		public string Name { get; set; }
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ModelChecker.DAL.Entities
{
	public class RevitModel
	{
		public int Id { get; set; }

		[StringLength(256)]
		public string Name { get; set; }

		public ICollection<RevitElement> RevitElements { get; set; }

		public RevitModel()
		{
			RevitElements = new List<RevitElement>();
		}

	}
}

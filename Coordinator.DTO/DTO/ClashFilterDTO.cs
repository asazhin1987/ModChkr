using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Coordinator.DTO
{
	public enum Sorts { Distantion, CreateDate, ChangeDate, ClashesCount, Default }
	public enum Orders { Ascending, Descending, Default }

	[DataContract]
	public class ClashFilterDTO
	{
		[DataMember]
		public IEnumerable<int> RevitModels { get; set; }

		[DataMember]
		public IEnumerable<int> Constructions { get; set; }

		[DataMember]
		public IEnumerable<int> Checks { get; set; }

		[DataMember]
		public IEnumerable<int> Statuses { get; set; }

		[DataMember]
		public Sorts Sorts { get; set; }

		[DataMember]
		public Orders Orders { get; set; }

		[DataMember]
		public int PageNum { get; set; }

		[DataMember]
		public int PageSize { get; set; }

		[DataMember]
		public string SearchId { get; set; }

		public ClashFilterDTO()
		{
			Checks = new List<int>();
			Statuses = new List<int>();
			Constructions = new List<int>();
			RevitModels = new List<int>();
			SearchId = string.Empty;
		}
	}
}

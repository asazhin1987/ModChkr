using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Coordinator.DTO
{
	[DataContract]
	public class ClashesResultDTO
	{
		[DataMember]
		public IEnumerable<ClashDTO> Clashes { get; set; }

		[DataMember]
		public int ClashesCount { get; set; }

		[DataMember]
		public int PagesCount { get; set; }
	}
}

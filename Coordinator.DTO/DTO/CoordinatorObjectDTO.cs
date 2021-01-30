using System.Runtime.Serialization;
using System.Collections.Generic;
namespace Coordinator.DTO
{
	[DataContract]
	public class CoordinatorObjectDTO : INamedObject
	{
		[DataMember]
		public int Id { get; set; }
		[DataMember]
		public string Name { get; set; }
		[DataMember]
		public IEnumerable<int> Parents { get; set; }

	}
}

using ModelChecker.DTO.Interfaces;
using System.Runtime.Serialization;


namespace ModelChecker.DTO
{
	[DataContract]
	public class CheckDTO : ModelCheckerDTO, INamedObject
	{
		[DataMember]
		public int Id { get; set; }

		[DataMember]
		public string Name { get; set; }

		//[DataMember]
		//public int ConstructionId { get; set; }
	}
}

using System.Runtime.Serialization;

namespace ModelChecker.DTO
{
	[DataContract]
	public class ParamsDTO
	{
		[DataMember]
		public string Delimiter { get; set; }

		[DataMember]
		public int DatRetentionPeriod { get; set; }
	}
}

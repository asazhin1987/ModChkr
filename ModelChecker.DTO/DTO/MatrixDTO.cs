using System.Runtime.Serialization;
using System;

namespace ModelChecker.DTO
{
	[DataContract]
	public class MatrixDTO : ModelCheckerDTO
	{
		[DataMember]
		public int LeftId { get; set; }

		[DataMember]
		public string LeftName { get; set; }

		[DataMember]
		public int RightId { get; set; }

		[DataMember]
		public string RightName { get; set; }

		[DataMember]
		public int Qnt { get; set; }
	}
}

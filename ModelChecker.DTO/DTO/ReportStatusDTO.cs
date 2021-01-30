using System.Runtime.Serialization;
using System;
using ModelChecker.DTO.Interfaces;

namespace ModelChecker.DTO
{
	[DataContract]
	public class ReportStatusDTO : ModelCheckerDTO
	{
		[DataMember]
		public int ClashesQnt { get; set; }

		[DataMember]
		public int AtWorkClashesQnt { get; set; }

		[DataMember]
		public int CompletedClashesQnt { get; set; }

		[DataMember]
		public int CreatedQnt { get; set; }

		[DataMember]
		public int ActiveQnt { get; set; }

		[DataMember]
		public int AnalizedQnt { get; set; }

		[DataMember]
		public int ConfirmedQnt { get; set; }

		[DataMember]
		public int CorrectedQnt { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public DateTime? Date { get; set; }
	}
}

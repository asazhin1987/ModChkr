using ModelChecker.DTO.Interfaces;
using System;
using System.Runtime.Serialization;

namespace ModelChecker.DTO
{
	[DataContract]
	public class FullCheckDTO: ReportStatusDTO, INamedObject, IReportStatusDTO
	{
		[DataMember]
		public int Id { get; set; }

		//[DataMember]
		//public string Name { get; set; }

		[DataMember]
		public int ConstructionId { get; set; }

		[DataMember]
		public string LeftCheckName { get; set; }

		[DataMember]
		public string RightCheckName { get; set; }

		//[DataMember]
		//public DateTime? Odate { get; set; }

		//[DataMember]
		//public DateTime? Date { get; set; }

		//[DataMember]
		//public int ClashesQnt { get; set; }

		//[DataMember]
		//public int CreatedQnt { get; set; }

		//[DataMember]
		//public int ActiveQnt { get; set; }

		//[DataMember]
		//public int AnalizedQnt { get; set; }

		//[DataMember]
		//public int ConfirmedQnt { get; set; }

		//[DataMember]
		//public int CorrectedQnt { get; set; }

		[DataMember]
		public int StartPeriodClashesQnt { get; set; }

		[DataMember]
		public int StartPeriodCreatedQnt { get; set; }

		[DataMember]
		public int StartPeriodActiveQnt { get; set; }

		[DataMember]
		public int StartPeriodAnalizedQnt { get; set; }

		[DataMember]
		public int StartPeriodConfirmedQnt { get; set; }

		[DataMember]
		public int StartPeriodCorrectedQnt { get; set; }

		[DataMember]
		public int EndPeriodClashesQnt { get; set; }

		[DataMember]
		public int EndPeriodCreatedQnt { get; set; }

		[DataMember]
		public int EndPeriodActiveQnt { get; set; }

		[DataMember]
		public int EndPeriodAnalizedQnt { get; set; }

		[DataMember]
		public int EndPeriodConfirmedQnt { get; set; }

		[DataMember]
		public int EndPeriodCorrectedQnt { get; set; }
	}
}

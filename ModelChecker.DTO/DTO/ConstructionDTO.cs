using System.Runtime.Serialization;
using System;
using ModelChecker.DTO.Interfaces;

namespace ModelChecker.DTO
{
	[DataContract]
	public class ConstructionDTO: ReportStatusDTO, INamedObject, IReportStatusDTO
	{
		[DataMember]
		public int Id { get; set; }


		[DataMember]
		public string Description { get; set; }


		public ConstructionDTO()
		{
		}
	}
}

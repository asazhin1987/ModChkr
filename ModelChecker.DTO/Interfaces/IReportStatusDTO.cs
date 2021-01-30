using System;


namespace ModelChecker.DTO.Interfaces
{
	public interface IReportStatusDTO
	{
		int ClashesQnt { get; set; }
		int CreatedQnt { get; set; }
		int ActiveQnt { get; set; }
		int AnalizedQnt { get; set; }
		int ConfirmedQnt { get; set; }
		int CorrectedQnt { get; set; }
		string Name { get; set; }
		DateTime? Date { get; set; }
	}
}

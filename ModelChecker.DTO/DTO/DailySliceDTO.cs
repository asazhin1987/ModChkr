using System;


namespace ModelChecker.DTO
{
	public class DailySliceDTO
	{
		public DateTime Date { get; set; }
		public string FullCheckName { get; set; }
		public string Chack1Name { get; set; }
		public string Check2Name { get; set; }

		public int FullCheckId { get; set; }
		public int ConstructionId { get; set; }

		public int ClashesQnt { get; set; }
		public int ActiveQnt { get; set; }
		public int AnalizedQnt { get; set; }
		public int CorrectedQnt { get; set; }
		public int ConfirmedQnt { get; set; }
		public int CreatedQnt { get; set; }
	}
}

using System.Runtime.Serialization;

namespace ModelChecker.DTO
{
	[DataContract]
	public class LicenseMonthUsingDTO : ModelCheckerDTO
	{
		[DataMember]
		public int Year { get; set; }

		[DataMember]
		public int MonthNum { get; set; }

		[DataMember]
		public int AllQnt { get; set; }

		[DataMember]
		public int UniqQnt { get; set; }
	}
}

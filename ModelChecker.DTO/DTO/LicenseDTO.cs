using System;
using System.Runtime.Serialization;

namespace ModelChecker.DTO
{
	[DataContract]
	[Serializable()]
	public class LicenseDTO: ModelCheckerDTO
	{
		[DataMember]
		public bool Success { get; set; }
		[DataMember]
		public DateTime EndLicDate { get; set; }
		[DataMember]
		public string Key { get; set; }
		[DataMember]
		public int LicensseQnt { get; set; }

	}
}

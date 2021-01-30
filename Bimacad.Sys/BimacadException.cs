using System;
using System.Runtime.Serialization;

namespace Bimacad.Sys
{
	[DataContract]
	public class ModelCheck
	{
		public string Message { get; set; }
		public ModelCheck() { }
		public ModelCheck(Exception ex) { Message = ex.Message; }
	}



	[DataContract]
	public class ModelCheckerException : ApplicationException
	{
		public ModelCheckerException() { }
		public ModelCheckerException(string message) : base(message) { }
		public ModelCheckerException(string message, Exception inner) : base(message, inner) { }
	}

	[DataContract]
	public class NotFound { }

	[DataContract]
	public class NotFoundException : ModelCheckerException { }

	[DataContract]
	public class CheckLicense { }

	[DataContract]
	public class CheckLicenseException : ModelCheckerException { }

	[DataContract]
	public class NullKey : CheckLicense { }

	[DataContract]
	public class NullKeyException : CheckLicenseException { }

	[DataContract]
	public class WrongKey : CheckLicense { }

	[DataContract]
	public class WrongKeyException : CheckLicenseException { }

	[DataContract]
	public class ZeroQnt : CheckLicense { }

	[DataContract]
	public class ZeroQntException : CheckLicenseException { }
}

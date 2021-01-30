//using System;
//using System.Runtime.Serialization;

//namespace Coordinator.DTO
//{

//	[DataContract]
//	public class ExceptionData
//	{
//		public string Message { get; set; }
//		public ExceptionData() { }
//		public ExceptionData(Exception ex) { Message = ex.Message; }
//	}

//	[DataContract]
//	public class CoordinatorException : ApplicationException
//	{
//		[DataContract]
//		public class NotFound { }

//		[DataContract]
//		public class NotFoundException : CoordinatorException { }

//		[DataContract]
//		public class CheckLicense { }

//		[DataContract]
//		public class CheckLicenseException : CoordinatorException { }

//		[DataContract]
//		public class NullKey : CheckLicense { }

//		[DataContract]
//		public class NullKeyException : CheckLicenseException { }

//		[DataContract]
//		public class WrongKey : CheckLicense { }

//		[DataContract]
//		public class WrongKeyException : CheckLicenseException { }
//	}
//}

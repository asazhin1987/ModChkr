using System;
using System.Xml.Serialization;

namespace Coordinator.ClashXMLModel
{
	/// <remarks/>
	[Serializable()]
	[XmlRoot("createddate")]
	public partial class Createddate
	{
		/// <remarks/>
		[XmlElement()]
		public Date date { get; set; }
	}
}

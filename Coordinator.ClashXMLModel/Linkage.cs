using System;
using System.Xml.Serialization;

namespace Coordinator.ClashXMLModel
{
	/// <remarks/>
	[Serializable()]
	[XmlRoot("linkage")]
	public partial class Linkage
	{
		/// <remarks/>
		[XmlAttribute()]
		public string mode { get; set; }
	}
}

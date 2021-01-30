using System;
using System.Xml.Serialization;

namespace Coordinator.ClashXMLModel
{
	/// <remarks/>
	[Serializable()]
	[XmlRoot("pos3f")]
	public partial class Pos3f
	{
		/// <remarks/>
		[XmlAttribute()]
		public decimal x { get; set; }

		/// <remarks/>
		[XmlAttribute()]
		public decimal y { get; set; }

		/// <remarks/>
		[XmlAttribute()]
		public decimal z { get; set; }
	}
}

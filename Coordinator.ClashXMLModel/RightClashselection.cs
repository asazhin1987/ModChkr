using System;
using System.Xml.Serialization;

namespace Coordinator.ClashXMLModel
{
	/// <remarks/>
	[Serializable()]
	[XmlRoot("clashselection")]
	public partial class RightClashselection
	{
		/// <remarks/>
		public string locator { get; set; }

		/// <remarks/>
		[XmlAttribute()]
		public byte selfintersect { get; set; }

		/// <remarks/>
		[XmlAttribute()]
		public byte primtypes { get; set; }

	}
}

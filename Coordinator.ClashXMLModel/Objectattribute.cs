using System;
using System.Xml.Serialization;

namespace Coordinator.ClashXMLModel
{
	/// <remarks/>
	[Serializable()]
	[XmlRoot("objectattribute")]
	public partial class Objectattribute
	{
		/// <remarks/>
		public string name { get; set; }

		/// <remarks/>
		public uint value { get; set; }
	}
}

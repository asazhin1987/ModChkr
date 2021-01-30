using System;
using System.Xml.Serialization;

namespace Coordinator.ClashXMLModel
{
	/// <remarks/>
	[Serializable()]
	[XmlRoot("smarttag")]
	public partial class Smarttag
	{
		/// <remarks/>
		public string name { get; set; }

		/// <remarks/>
		public string value { get; set; }
	}
}

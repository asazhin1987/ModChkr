using System;
using System.Xml.Serialization;

namespace Coordinator.ClashXMLModel
{
	/// <remarks/>
	[Serializable()]
	[XmlRoot("clashobject")]
	public partial class Clashobject
	{
		/// <remarks/>
		[XmlElement()]
		public Objectattribute objectattribute { get; set; }

		/// <remarks/>
		public string layer { get; set; }

		[XmlElement("pathlink")]
		public Pathlink pathlink { get; set; }

		/// <remarks/>
		//[System.Xml.Serialization.XmlArrayItemAttribute("smarttag", IsNullable = false)]
		[XmlElement()]
		public Smarttags smarttags { get; set; }
	}
}

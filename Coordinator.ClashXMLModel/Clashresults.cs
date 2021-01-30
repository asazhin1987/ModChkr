using System;
using System.Xml.Serialization;

namespace Coordinator.ClashXMLModel
{
	[Serializable()]
	[XmlRoot("clashresults")]
	public class Clashresults
	{
		[XmlElement("clashresult")]
		public Clashresult[] clashresult { get; set; }
	}
}

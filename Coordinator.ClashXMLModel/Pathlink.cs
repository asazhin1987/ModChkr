using System;
using System.Xml.Serialization;
namespace Coordinator.ClashXMLModel
{
	[Serializable()]
	[XmlRoot("pathlink")]
	public class Pathlink
	{
		[XmlElement("node")]
		public string[] node { get; set; }
	}
}

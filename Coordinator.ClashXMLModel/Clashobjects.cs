using System;
using System.Xml.Serialization;

namespace Coordinator.ClashXMLModel
{
	[Serializable()]
	[XmlRoot("clashobjects")]
	public partial class Clashobjects
	{
		[XmlElement("clashobject")]
		public Clashobject[] clashobject { get; set; }
	}
}

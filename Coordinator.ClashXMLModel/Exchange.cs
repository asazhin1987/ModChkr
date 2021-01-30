using System;
using System.Xml.Serialization;


namespace Coordinator.ClashXMLModel
{
	[Serializable()]
	[XmlRoot("exchange")]
	public partial class Exchange
	{
		[XmlElement()]
		public Batchtest batchtest { get; set; }

		[XmlAttribute()]
		public string units { get; set; }

		[XmlAttribute()]
		public string filename { get; set; }

		[XmlAttribute()]
		public string filepath { get; set; }
	}
}

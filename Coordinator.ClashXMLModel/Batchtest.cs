using System;
using System.Xml.Serialization;
namespace Coordinator.ClashXMLModel
{
	[Serializable()]
	[XmlRoot("batchtest")]
	public partial class Batchtest
	{

		[XmlElement()]
		public Clashtests clashtests { get; set; }

		/// <remarks/>
		public object selectionsets { get; set; }

		/// <remarks/>
		[XmlAttribute()]
		public string name { get; set; }

		/// <remarks/>
		[XmlAttribute()]
		public string internal_name { get; set; }

		/// <remarks/>
		[XmlAttribute()]
		public string units { get; set; }
	}
}

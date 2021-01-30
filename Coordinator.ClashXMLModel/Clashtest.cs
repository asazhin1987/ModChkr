using System;
using System.Xml.Serialization;

namespace Coordinator.ClashXMLModel
{
	/// <remarks/>
	[Serializable()]
	[XmlRoot("clashtest")]
	public partial class Clashtest
	{

		/// <remarks/>
		[XmlElement("linkage")]
		public Linkage linkage { get; set; }

		/// <remarks/>
		[XmlElement("left")]
		public Left left { get; set; }

		/// <remarks/>
		[XmlElement("right")]
		public Right right { get; set; }

		/// <remarks/>
		public object rules { get; set; }

		/// <remarks/>
		[XmlElement("summary")]
		public Summary summary { get; set; }

		[XmlElement("clashresults")]
		public Clashresults clashresults { get; set; }
		/// <remarks/>
	
		/// <remarks/>
		[XmlAttribute()]
		public string name { get; set; }

		/// <remarks/>
		[XmlAttribute()]
		public string test_type { get; set; }

		/// <remarks/>
		[XmlAttribute()]
		public string status { get; set; }

		/// <remarks/>
		[XmlAttribute()]
		public decimal tolerance { get; set; }

		/// <remarks/>
		[XmlAttribute()]
		public byte merge_composites { get; set; }
	}
}

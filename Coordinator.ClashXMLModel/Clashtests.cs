using System;
using System.Xml.Serialization;


namespace Coordinator.ClashXMLModel
{
	/// <remarks/>
	[Serializable()]
	[XmlRoot("clashtests")]
	public partial class Clashtests
	{
		[XmlElement("clashtest")]
		public Clashtest[] clashtest { get; set; }


	}
}

using System;
using System.Xml.Serialization;

namespace Coordinator.ClashXMLModel
{
	/// <remarks/>
	[Serializable()]
	[XmlRoot("left")]
	public partial class Left
	{
		/// <remarks/>
		[XmlElement()]
		public LeftClashselection clashselection { get; set; }
	}
}

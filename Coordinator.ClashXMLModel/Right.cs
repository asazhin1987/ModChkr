using System;
using System.Xml.Serialization;

namespace Coordinator.ClashXMLModel
{
	/// <remarks/>
	[Serializable()]
	[XmlRoot("right")]
	public partial class Right
	{
		/// <remarks/>
		[XmlElement()]
		public RightClashselection clashselection { get; set; }
	}
}

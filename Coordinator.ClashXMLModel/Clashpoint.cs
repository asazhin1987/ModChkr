using System;
using System.Xml.Serialization;

namespace Coordinator.ClashXMLModel
{
	/// <remarks/>
	[Serializable()]
	[XmlRoot("clashpoint")]
	public partial class Clashpoint
	{
		/// <remarks/>
		public Pos3f pos3f { get; set; }
	}
}

using System;
using System.Xml.Serialization;

namespace Coordinator.ClashXMLModel
{
	[Serializable()]
	[XmlRoot("smarttags")]
	public partial class Smarttags
	{
		[XmlElement("smarttag")]
		public Smarttag[] smarttag { get; set; }
	}
}

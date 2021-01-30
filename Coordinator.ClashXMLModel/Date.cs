using System;
using System.Xml.Serialization;

namespace Coordinator.ClashXMLModel
{
	/// <remarks/>
	[Serializable()]
	[XmlRoot("date")]
	public partial class Date
	{
		/// <remarks/>
		[XmlAttribute()]
		public ushort year { get; set; }


		/// <remarks/>
		[XmlAttribute()]
		public byte month { get; set; }


		/// <remarks/>
		[XmlAttribute()]
		public byte day { get; set; }

		/// <remarks/>
		[XmlAttribute()]
		public byte hour { get; set; }

		/// <remarks/>
		[XmlAttribute()]
		public byte minute { get; set; }

		/// <remarks/>
		[XmlAttribute()]
		public byte second { get; set; }
	}
}

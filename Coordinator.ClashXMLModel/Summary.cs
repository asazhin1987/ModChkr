using System;
using System.Xml.Serialization;

namespace Coordinator.ClashXMLModel
{
	/// <remarks/>
	[Serializable()]
	[XmlRoot("summary")]
	public partial class Summary
	{
		/// <remarks/>
		public string testtype { get; set; }

		/// <remarks/>
		public string teststatus { get; set; }


		/// <remarks/>
		[XmlAttribute()]
		public uint total { get; set; }

		/// <remarks/>
		[XmlAttribute()]
		public uint @new { get; set; }

		/// <remarks/>
		[XmlAttribute()]
		public uint active { get; set; }

		/// <remarks/>
		[XmlAttribute()]
		public uint reviewed { get; set; }

		/// <remarks/>
		[XmlAttribute()]
		public uint approved { get; set; }

		/// <remarks/>
		[XmlAttribute()]
		public uint resolved { get; set; }

	}
}

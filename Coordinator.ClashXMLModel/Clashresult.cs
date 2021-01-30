using System;
using System.Xml.Serialization;

namespace Coordinator.ClashXMLModel
{
	[Serializable()]
	[XmlRoot("clashresult")]
	public partial class Clashresult
	{

		/// <remarks/>
		public string description { get; set; }

		/// <remarks/>
		public string resultstatus { get; set; }

		/// <remarks/>
		[XmlElement()]
		public Clashpoint clashpoint { get; set; }

		/// <remarks/>
		public string gridlocation { get; set; }

		/// <remarks/>
		[XmlElement()]
		public Createddate createddate { get; set; }

		/// <remarks/>
		//[XmlArrayItem("clashobject", IsNullable = true)]
		[XmlElement("clashobjects")]
		public Clashobjects clashobjects { get; set; }
		//public exchangeBatchtestClashtestsClashtestClashresultClashobject[] clashobjects { get; set; }




		/// <remarks/>
		[XmlAttribute()]
		public string name { get; set; }

		/// <remarks/>
		[XmlAttribute()]
		public string guid { get; set; }

		/// <remarks/>
		[XmlAttribute()]
		public string status { get; set; }

		/// <remarks/>
		[XmlAttribute()]
		public decimal distance { get; set; }
	}
}

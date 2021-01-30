using System.Runtime.Serialization;
using System;

namespace ModelChecker.DTO
{
	[DataContract]
	public class ClashDTO : ModelCheckerDTO
	{
		[DataMember]
		public int ConstructionId { get; set; }

		[DataMember]
		public int CheckId { get; set; }

		[DataMember]
		public string CheckName { get; set; }

		[DataMember]
		public int ClashStatusId { get; set; }

		[DataMember]
		public string ClashStatusName { get; set; }

		[DataMember]
		public int Element1Id { get; set; }

		[DataMember]
		public int Element2Id { get; set; }

		[DataMember]
		public int RevitModel1Id { get; set; }

		[DataMember]
		public string RevitModel1Name { get; set; }

		[DataMember]
		public int RevitModel2Id { get; set; }

		[DataMember]
		public string RevitModel2Name { get; set; }

		[DataMember]
		public int RevitElement1Id { get; set; }

		[DataMember]
		public string RevitElement1Name { get; set; }

		[DataMember]
		public int RevitElement2Id { get; set; }

		[DataMember]
		public string RevitElement2Name { get; set; }

		[DataMember]
		public int CategotyElement1Id { get; set; }

		[DataMember]
		public string CategoryElement1Name { get; set; }

		[DataMember]
		public int CategoryElement2Id { get; set; }

		[DataMember]
		public string CategoryElement2Name { get; set; }

		[DataMember]
		public string Element1Level { get; set; }

		[DataMember]
		public string Element2Level { get; set; }

		[DataMember]
		public DateTime FoundDate { get; set; }

		[DataMember]
		public DateTime Odate { get; set; }

		[DataMember]
		public string Act { get; set; }

		[DataMember]
		public double Distance { get; set; }

		[DataMember]
		public double X { get; set; }

		[DataMember]
		public double Y { get; set; }

		[DataMember]
		public double Z { get; set; }

		[DataMember]
		public string Grid { get; set; }

	}
}

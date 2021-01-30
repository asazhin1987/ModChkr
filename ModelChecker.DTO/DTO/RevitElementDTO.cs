using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ModelChecker.DTO
{
	[DataContract]
	public class RevitElementDTO
	{
		[DataMember]
		public int Id { get; set; }

		[DataMember]
		public int RevitModelId { get; set; }

		[DataMember]
		public int RevitId { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public int CategoryId { get; set; }

		[DataMember]
		public string ModelName { get; set; }

		[DataMember]
		public string CategoryName { get; set; }

		[DataMember]
		public string Level { get; set; }

		public override bool Equals(object obj)
		{
			if (obj is RevitElementDTO && obj != null)
			{
				return this.ToString() == ((RevitElementDTO)obj).ToString();
			}
			else
			{
				return false;
			}
		}

		public override string ToString()
		{
			return $"{RevitId}_{ModelName}";
		}

		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}
	}
}

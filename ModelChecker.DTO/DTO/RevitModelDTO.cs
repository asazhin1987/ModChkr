using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ModelChecker.DTO
{
	[DataContract]
	public class RevitModelDTO
	{
		[DataMember]
		public int Id { get; set; }
		[DataMember]
		public string Name { get; set; }

		public override bool Equals(object obj)
		{
			if (obj is RevitModelDTO && obj != null)
			{
				return this.ToString() == ((RevitModelDTO)obj).ToString();
			}
			else
			{
				return false;
			}
			
		}

		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}

		public override string ToString()
		{
			return Name.ToString();
		}
	}
}

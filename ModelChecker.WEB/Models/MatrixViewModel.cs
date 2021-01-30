using ModelChecker.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ModelChecker.WEB.Models
{
	public class MatrixViewModel
	{
		public IEnumerable<MatrixDTO> Matrix { get; set; }
		public IEnumerable<MatrixDemention> LeftDementions { get; set; }
		public IEnumerable<MatrixDemention> RightDementions { get; set; }
		public int ItemsCount { get { return Matrix.Sum(x => x.Qnt); } }
		public bool HasItems { get { return Matrix.Count() > 0; } }
		public string DementionName { get; set; }

		public MatrixViewModel(IEnumerable<MatrixDTO> items, string  name)
		{
			Matrix = items;
			LeftDementions = items.Select(x => new MatrixDemention(x.LeftId, x.LeftName)).Distinct();
			RightDementions = items.Select(x => new MatrixDemention(x.RightId, x.RightName)).Distinct();
			DementionName = name;
		}
	}

	public class MatrixDemention
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string DisplayName { get; set; }

		public MatrixDemention(int id, string name)
		{
			Id = id;
			Name = name;
			DisplayName = name.Replace("_", " ");
		}
		public override bool Equals(object obj)
		{
			if (obj is MatrixDemention && obj != null)
			{
				return this.ToString() == ((MatrixDemention)obj).ToString();
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
			return $"{Id} {Name}".ToString();
		}
	}
}
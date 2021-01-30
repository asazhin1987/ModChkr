using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelChecker.DTO;
using ModelChecker.DAL.Entities;


namespace ModelChecker.BLL.Extensions
{
	public static class ModelExtensions
	{
		public static ConstructionDTO Map(this ConstructionDTO dto, Construction item)
		{
			dto.Id = item.Id;
			dto.Name = item.Name;
			dto.Description = item.Description;
			return dto;
		}

		public static ConstructionDTO Map(this ConstructionDTO dto, Construction item, IEnumerable<Clash> clashes)
		{
			dto.Id = item.Id;
			dto.Name = item.Name;
			dto.Description = item.Description;
			return dto;
		}

		public static Construction Map(this Construction item, ConstructionDTO dto)
		{
			item.Id = dto.Id;
			item.Name = dto.Name;
			item.Description = dto.Description;
			return item;
		}


		public static FullCheckDTO Map(this FullCheckDTO dto, FullCheck item)
		{
			dto.Id = item.Id;
			dto.Name = item.Name;
			return dto;
		}

		public static FullCheck Map(this FullCheck item, FullCheckDTO dto)
		{
			item.Id = dto.Id;
			item.Name = dto.Name;
			return item;
		}
	}

}

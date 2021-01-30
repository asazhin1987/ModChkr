using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace ModelChecker.DAL.Entities
{
	public class LicenseUsing
	{
		public int Id { get; set; }

		[Index(IsClustered = false, IsUnique = false)]
		[StringLength(125)]
		public string StantionName { get; set; }

		[StringLength(125)]
		public string UserName { get; set; }

		public DateTime LastAccess { get; set; }

		[Index(IsClustered = false, IsUnique = false)]
		public DateTime LastAccessDate { get; set; }

		public ICollection<LicenseUsersStatistic> LicenseUsersStatistics { get; set; }

		public LicenseUsing()
		{
			LicenseUsersStatistics = new List<LicenseUsersStatistic>();
		}

	}
}

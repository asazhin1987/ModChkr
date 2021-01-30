using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelChecker.DAL.Entities
{
	public class LicenseUsersStatistic
	{
		public int Id { get; set; }
		public int StantionId { get; set; }
		public LicenseUsing Stantion { get; set; }

		[Index(IsClustered = false, IsUnique = false)]
		public DateTime Date { get; set; }

		[Index(IsClustered = false, IsUnique = false)]
		public int MonthNum { get; set; }

		[Index(IsClustered = false, IsUnique = false)]
		public int YearNum { get; set; }

		public int Qnt { get; set; }
		public int UniqQnt { get; set; }
	}
}

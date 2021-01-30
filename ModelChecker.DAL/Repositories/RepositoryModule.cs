using ModelChecker.DAL.EF;
using ModelChecker.DAL.Entities;
using ModelChecker.DAL.Interfaces;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ModelChecker.DAL.Repositories
{
	public class FullChecksRepository : Repository<FullCheck>, IRepository<FullCheck>
	{
		public FullChecksRepository(ModelCheckerContext context) : base(context) { }
	}
	public class CheckRepository : Repository<Check>, IRepository<Check>
	{
		public CheckRepository(ModelCheckerContext context) : base(context) { }
	}
	public class ConstructionRepository : Repository<Construction>, IRepository<Construction>
	{
		public ConstructionRepository(ModelCheckerContext context) : base(context) { }
	}
	public class ClashStatusRepository : Repository<ClashStatus>, IRepository<ClashStatus>
	{
		public ClashStatusRepository(ModelCheckerContext context) : base(context) { }
	}
	public class RevitCategoryRepository : Repository<RevitCategory>, IRepository<RevitCategory>
	{
		public RevitCategoryRepository(ModelCheckerContext context) : base(context) { }
	}
	public class RevitModelRepository : Repository<RevitModel>, IRepository<RevitModel>
	{
		public RevitModelRepository(ModelCheckerContext context) : base(context) { }
	}
	public class RevitElementRepository : Repository<RevitElement>, IRepository<RevitElement>
	{
		public RevitElementRepository(ModelCheckerContext context) : base(context) { }
	}
	public class DailySliceRepository : Repository<DailySlice>, IRepository<DailySlice>
	{
		public DailySliceRepository(ModelCheckerContext context) : base(context) { }
	}
	public class ClashRepository : Repository<Clash>, IRepository<Clash>
	{
		public ClashRepository(ModelCheckerContext context) : base(context) { }
	}
	public class SettingRepository : Repository<Setting>, IRepository<Setting>
	{
		public SettingRepository(ModelCheckerContext context) : base(context) { }
	}

	public class LicenseUsingRepository : Repository<LicenseUsing>, IRepository<LicenseUsing>
	{
		public LicenseUsingRepository(ModelCheckerContext context) : base(context) { }
	}

	public class LicenseStatisticRepository : Repository<LicenseUsersStatistic>, IRepository<LicenseUsersStatistic>
	{
		public LicenseStatisticRepository(ModelCheckerContext context) : base(context) { }
	}

	public class LicenseRepository : Repository<License>, IRepository<License>
	{
		public LicenseRepository(ModelCheckerContext context) : base(context) { }

		public override async Task DeleteAsync(int id)
		{
			var lic = await GetAsync(id);
			if (lic != null)
				await base.DeleteAsync(lic);
		}
		public override License Get(int Id)
		{
			return base.GetAll().FirstOrDefault();
		}

		public override async Task<License> GetAsync(int id)
		{
			return await base.GetAll().FirstOrDefaultAsync();
		}
	}

	public class ParamsRepository : Repository<Params>, IRepository<Params>
	{
		public ParamsRepository(ModelCheckerContext context) : base(context) { }

		public override async Task DeleteAsync(int id)
		{
			var lic = await GetAsync(id);
			if (lic != null)
				await base.DeleteAsync(lic);
		}
		public override Params Get(int Id)
		{
			return base.GetAll().AsNoTracking().FirstOrDefault();
		}

		public override async Task<Params> GetAsync(int id)
		{
			return await base.GetAll().FirstOrDefaultAsync();
		}
	}


}

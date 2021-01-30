using ModelChecker.DAL.Entities;
using System;


namespace ModelChecker.DAL.Interfaces
{
	public interface IUnitOfWork : IDisposable//, ICloneable
	{
		IRepository<FullCheck> FullChecks { get;}
		IRepository<Check> Checks { get; }
		IRepository<Construction> Constructions { get; }
		IRepository<ClashStatus> ClashStatuses { get;  }
		IRepository<RevitCategory> RevitCategories { get;  }
		IRepository<RevitModel> RevitModels { get;  }
		IRepository<RevitElement> RevitElements { get;  }
		IRepository<DailySlice> DailySlices { get;  }
		IRepository<Clash> Clashes { get;  }
		IRepository<Setting> Settings { get;  }
		IRepository<License> License { get; }
		IRepository<Params> Params { get; }
		IRepository<LicenseUsing> LicenseUsings { get; }
		IRepository<LicenseUsersStatistic> LicenseStatistics { get; }
		
	}
}

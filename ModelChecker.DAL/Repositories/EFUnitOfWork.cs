using ModelChecker.DAL.EF;
using ModelChecker.DAL.Entities;
using ModelChecker.DAL.Interfaces;
using System;


namespace ModelChecker.DAL.Repositories
{
	public class EFUnitOfWork : IUnitOfWork//, ICloneable
	{
		readonly ModelCheckerContext db;
		//private readonly string ConnectionString;

		FullChecksRepository fullChecksRepository;
		CheckRepository checkRepository;
		ConstructionRepository constructionRepository;
		ClashStatusRepository clashStatusRepository;
		RevitCategoryRepository revitCategoryRepository;
		RevitModelRepository revitModelRepository;
		RevitElementRepository revitElementRepository;
		DailySliceRepository dailySliceRepository;
		ClashRepository clashRepository;
		SettingRepository settingRepository;
		LicenseRepository licenseRepository;
		ParamsRepository paramsRepository;
		LicenseUsingRepository licenseUsingRepository;
		LicenseStatisticRepository licenseStatisticRepository;


		

		public EFUnitOfWork(string connectionString)
		{
			db = new ModelCheckerContext(connectionString);
			//ConnectionString = connectionString;
		}

		public IRepository<FullCheck> FullChecks
		{
			get
			{
				if (fullChecksRepository == null)
					fullChecksRepository = new FullChecksRepository(db);
				return fullChecksRepository;
			}
		}

		public IRepository<Check> Checks
		{
			get
			{
				if (checkRepository == null)
					checkRepository = new CheckRepository(db);
				return checkRepository;
			}
		}

		public IRepository<Construction> Constructions
		{
			get
			{
				if (constructionRepository == null)
					constructionRepository = new ConstructionRepository(db);
				return constructionRepository;
			}
		}

		public IRepository<ClashStatus> ClashStatuses
		{
			get
			{
				if (clashStatusRepository == null)
					clashStatusRepository = new ClashStatusRepository(db);
				return clashStatusRepository;
			}
		}

		public IRepository<RevitCategory> RevitCategories
		{
			get
			{
				if (revitCategoryRepository == null)
					revitCategoryRepository = new RevitCategoryRepository(db);
				return revitCategoryRepository;
			}
		}

		public IRepository<RevitModel> RevitModels
		{
			get
			{
				if (revitModelRepository == null)
					revitModelRepository = new RevitModelRepository(db);
				return revitModelRepository;
			}
		}

		public IRepository<RevitElement> RevitElements
		{
			get
			{
				if (revitElementRepository == null)
					revitElementRepository = new RevitElementRepository(db);
				return revitElementRepository;
			}
		}

		public IRepository<DailySlice> DailySlices
		{
			get
			{
				if (dailySliceRepository == null)
					dailySliceRepository = new DailySliceRepository(db);
				return dailySliceRepository;
			}
		}

		public IRepository<Clash> Clashes
		{
			get
			{
				if (clashRepository == null)
					clashRepository = new ClashRepository(db);
				return clashRepository;
			}
		}

		public IRepository<Setting> Settings
		{
			get
			{
				if (settingRepository == null)
					settingRepository = new SettingRepository(db);
				return settingRepository;
			}
		}

		public IRepository<License> License
		{
			get
			{
				if (licenseRepository == null)
					licenseRepository = new LicenseRepository(db);
				return licenseRepository;
			}
		}

		public IRepository<Params> Params
		{
			get
			{
				if (paramsRepository == null)
					paramsRepository = new ParamsRepository(db);
				return paramsRepository;
			}
		}

		public IRepository<LicenseUsing> LicenseUsings
		{
			get
			{
				if (licenseUsingRepository == null)
					licenseUsingRepository = new LicenseUsingRepository(db);
				return licenseUsingRepository;
			}
		}

		public IRepository<LicenseUsersStatistic> LicenseStatistics
		{
			get
			{
				if (licenseStatisticRepository == null)
					licenseStatisticRepository = new LicenseStatisticRepository(db);
				return licenseStatisticRepository;
			}
		}

		private bool disposed = false;

		public virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					db.Dispose();
				}
				this.disposed = true;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		//public object Clone()
		//{
		//	return new EFUnitOfWork(ConnectionString);
		//}
	}
}

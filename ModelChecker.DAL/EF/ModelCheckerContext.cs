using System;
using System.Collections.Generic;
using System.Data.Entity;
using Bimacad.Sys;
using ModelChecker.DAL.Entities;

namespace ModelChecker.DAL.EF
{
	public class ModelCheckerContext : DbContext
	{
		public DbSet<FullCheck> FullChecks { get; set; }
		public DbSet<Check> Checks { get; set; }
		public DbSet<Construction> Constructions { get; set; }
		public DbSet<ClashStatus> ClashStatuses { get; set; }
		public DbSet<RevitCategory> RevitCategories { get; set; }
		public DbSet<RevitModel> RevitModels { get; set; }
		public DbSet<RevitElement> RevitElements { get; set; }
		public DbSet<DailySlice> DailySlices { get; set; }
		public DbSet<Clash> Clashes { get; set; }
		public DbSet<Setting> Settings { get; set; }
		public DbSet<License> Licenses { get; set; }
		public DbSet<Params> Params { get; set; }
		public DbSet<LicenseUsing> LicenseUsings { get; set; }
		public DbSet<LicenseUsersStatistic> LicenseStatistics { get; set; }

		public ModelCheckerContext(string connectionString) : base(connectionString)
		{
			try
			{
				Database.SetInitializer(new DbInitializer());
			}
			catch (Exception ex)
			{
				BTextWriter.WriteCurrentFile(ex.Message, "DAL_Exception_Log.txt", false);
			}
			
		}


		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
		
		}

		public class DbInitializer : CreateDatabaseIfNotExists<ModelCheckerContext>
		{
			protected override void Seed(ModelCheckerContext db)
			{
				db.ClashStatuses.AddRange(new List<ClashStatus> {
					new ClashStatus(){Id = 1, Name = "Новый" },
					new ClashStatus(){Id = 2, Name = "Активный" },
					new ClashStatus(){Id = 3, Name = "Проанализирован" },
					new ClashStatus(){Id = 4, Name = "Подтвержден" },
					new ClashStatus(){Id = 5, Name = "Исправлен" },
				});

				db.Params.Add(new Params() { Id = 1, DatRetentionPeriod = 3, Delimiter = "_VS_" });
				db.SaveChanges();
			}
		}

	}
}

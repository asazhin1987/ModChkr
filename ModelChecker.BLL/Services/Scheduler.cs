using ModelChecker.DAL.Interfaces;
using ModelChecker.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using System.Data.Entity;
using Bimacad.Sys;

namespace ModelChecker.BLL.Services
{
	[DisallowConcurrentExecution]
	public class Scheduler : IJob
	{

		public async Task Execute(IJobExecutionContext context)
		{
			try
			{
				
				JobDataMap dataMap = context.JobDetail.JobDataMap;
				IUnitOfWork db = (IUnitOfWork)dataMap["src"];
				await UpdateStatistic(db);
				await RemoveStatistic(db);
			}
			catch(Exception ex)
			{
				
			}
		}


		private async Task UpdateStatistic(IUnitOfWork db)
		{
			var lastcheck = await db.Constructions.GetAll().Select(x => x.Odate).MaxAsync();
			
			if (lastcheck != null)
			{
				var slicedatetime = await db.DailySlices.GetAll().Select(x => x.Odate).MaxAsync();
				if (slicedatetime.HasValue == false || slicedatetime < lastcheck)
				{
					DateTime odate = DateTime.Now.Date;

					//Createreport
					var result = await db.FullChecks.GetAll().GroupJoin(db.Clashes.GetAll(),
						fch => fch.Id,
						cl => cl.FullCheckId,
						(t1, t2) =>
						new
						{
							FullCheckId = t1.Id,
							ConstructionId = t1.ConstructionId,
							ClashesQnt = t2.Count(),
							ActiveQnt = t2.Where(x => x.ClashStatusId == 2).Count(),
							AnalizedQnt = t2.Where(x => x.ClashStatusId == 3).Count(),
							CorrectedQnt = t2.Where(x => x.ClashStatusId == 5).Count(),
							ConfirmedQnt = t2.Where(x => x.ClashStatusId == 4).Count(),
							CreatedQnt = t2.Where(x => x.ClashStatusId == 1).Count(),
						}).AsNoTracking().ToListAsync();

					foreach (var res in result)
					{
						var item = await db.DailySlices.GetAll()
							.Where(x => x.ConstructionId == res.ConstructionId && x.FullCheckId == res.FullCheckId && x.Date == odate)
							.FirstOrDefaultAsync();
						if (item == null)
						{
							await db.DailySlices.CreateAsync(new DailySlice()
							{
								FullCheckId = res.FullCheckId,
								ConstructionId = res.ConstructionId,
								ClashesQnt = res.ClashesQnt,
								ActiveQnt = res.ActiveQnt,
								AnalizedQnt = res.AnalizedQnt,
								CorrectedQnt = res.CorrectedQnt,
								ConfirmedQnt = res.ConfirmedQnt,
								CreatedQnt = res.CreatedQnt,
								Date = odate,
								Odate = lastcheck
							});
						}
						else
						{
							item.ClashesQnt = res.ClashesQnt;
							item.ActiveQnt = res.ActiveQnt;
							item.AnalizedQnt = res.AnalizedQnt;
							item.CorrectedQnt = res.CorrectedQnt;
							item.ConfirmedQnt = res.ConfirmedQnt;
							item.CreatedQnt = res.CreatedQnt;
							item.Odate = lastcheck;
							await db.DailySlices.UpdateAsync(item);
						}
					}
				}
			}
			
		}

		private async Task RemoveStatistic(IUnitOfWork db)
		{
			int months = (await db.Params.GetAsync(0)).DatRetentionPeriod;
			DateTime odate = DateTime.Now.Date.AddMonths(-months);
			var result = await db.DailySlices.GetAll().Where(x => x.Date < odate).ToListAsync();
			foreach (var slice in result)
				await db.DailySlices.DeleteAsync(slice);
		}
	}
}

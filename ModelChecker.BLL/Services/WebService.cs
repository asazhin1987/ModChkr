using ModelChecker.ISRC;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ServiceModel;
using ModelChecker.DTO;
using ModelChecker.DAL.Interfaces;
using ModelChecker.DAL.Entities;
using ModelChecker.BLL.Extensions;
using System.Data.Entity;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using Bimacad.Sys;


namespace ModelChecker.BLL.Services
{
	[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple,
	   InstanceContextMode = InstanceContextMode.Single,
	   IncludeExceptionDetailInFaults = true)]

	public class WebService : BaseService, IWebService
	{
		public WebService(IUnitOfWork uw) : base(uw) { }

		/*CONSTRUCTIONS*/
		#region Constructions

		public async Task<IEnumerable<ConstructionDTO>> GetAllConstructionsAsync()
		{
			try
			{
				return await db.Constructions.GetAll().GroupJoin(db.Clashes.GetAll(),
					co => co.Id,
					cl => cl.ConstructionId,
					(con, cls) =>
					new ConstructionDTO()
					{
						Id = con.Id,
						Name = con.Name,
						Description = con.Description,
						ClashesQnt = cls.Count(),
						ActiveQnt = cls.Where(x => x.ClashStatusId == 2).Count(),
						AnalizedQnt = cls.Where(x => x.ClashStatusId == 3).Count(),
						CorrectedQnt = cls.Where(x => x.ClashStatusId == 5).Count(),
						ConfirmedQnt = cls.Where(x => x.ClashStatusId == 4).Count(),
						CreatedQnt = cls.Where(x => x.ClashStatusId == 1).Count(),
						Date = con.Odate,
						AtWorkClashesQnt = cls.Where(x => x.ClashStatusId == 1 || x.ClashStatusId == 2).Count(),
						CompletedClashesQnt = cls.Where(x => x.ClashStatusId != 1 && x.ClashStatusId != 2).Count(),
					}).AsNoTracking().ToListAsync();
			}
			catch (Exception ex)
			{
				throw new FaultException<ModelCheck>(new ModelCheck(ex));
			}

		}

		public async Task<ConstructionDTO> GetConstructionAsync(int Id)
		{
			try
			{
				Construction item = await db.Constructions.GetAsync(Id);
				if (item == null)
					throw new FaultException<NotFound>(new NotFound());
				return new ConstructionDTO().Map(item);
			}
			catch (Exception ex)
			{
				throw new FaultException<ModelCheck>(new ModelCheck(ex));
			}

		}

		public async Task UpdateConstructionAsync(ConstructionDTO dto)
		{
			try
			{
				Construction item = await db.Constructions.GetAsync(dto.Id);
				if (item == null)
				{
					item = new Construction();
				}
				item.Map(dto);
				if (dto.Id == 0)
					await db.Constructions.CreateAsync(item);
				else
					await db.Constructions.UpdateAsync(item);
			}
			catch (Exception ex)
			{
				throw new FaultException<ModelCheck>(new ModelCheck(ex));
			}

		}

		public async Task RemoveConstruction(int Id)
		{
			try
			{
				await db.Constructions.DeleteAsync(Id);
			}
			catch (Exception ex)
			{
				throw new FaultException<ModelCheck>(new ModelCheck(ex));
			}
		}


		public async Task<ConstructionDTO> GetConstructionEmptyAsync(int? ID)
		{
			if (ID.HasValue)
				return await GetConstructionAsync(ID.Value);
			if (await db.Constructions.GetAll().CountAsync() > 0)
				return new ConstructionDTO().Map(await db.Constructions.GetAll().FirstOrDefaultAsync());
			throw new FaultException<NotFound>(new NotFound());
		}

		public IEnumerable<ConstructionDTO> GetAllConstructionsEmptyAsync()
		{
			return db.Constructions.GetAll().AsNoTracking().ToList().Select(x => new ConstructionDTO() { Id = x.Id, Name = x.Name });
		}
		#endregion Constructions

		#region Checks
		/*CHECKS*/

		public async Task<IEnumerable<FullCheckDTO>> GetChecksAsync(int constructionId)
		{
			try
			{
				return await db.FullChecks.GetAll().Where(x => x.ConstructionId == constructionId).GroupJoin(db.Clashes.GetAll(),
					co => co.Id,
					cl => cl.FullCheckId,
					(con, cls) =>
					new FullCheckDTO()
					{
						Id = con.Id,
						Name = con.Name,
						ClashesQnt = cls.Count(),
						ActiveQnt = cls.Where(x => x.ClashStatusId == 2).Count(),
						AnalizedQnt = cls.Where(x => x.ClashStatusId == 3).Count(),
						CorrectedQnt = cls.Where(x => x.ClashStatusId == 5).Count(),
						ConfirmedQnt = cls.Where(x => x.ClashStatusId == 4).Count(),
						CreatedQnt = cls.Where(x => x.ClashStatusId == 1).Count(),
						Date = con.Odate,
						AtWorkClashesQnt = cls.Where(x => x.ClashStatusId == 1 || x.ClashStatusId == 2).Count(),
						CompletedClashesQnt = cls.Where(x => x.ClashStatusId != 1 && x.ClashStatusId != 2).Count(),
					}).ToListAsync();
			}
			catch (Exception ex)
			{
				throw new FaultException<ModelCheck>(new ModelCheck(ex));
			}
		}


		public async Task<FullCheckDTO> GetCheckAsync(int Id)
		{
			try
			{
				FullCheck item = await db.FullChecks.GetAsync(Id);
				if (item == null)
					throw new FaultException<NotFound>(new NotFound());
				return new FullCheckDTO().Map(item);
			}
			catch (Exception ex)
			{
				throw new FaultException<ModelCheck>(new ModelCheck(ex));
			}
		}

		public async Task UpdateCheckAsync(FullCheckDTO item)
		{
			FullCheck _item = await db.FullChecks.GetAsync(item.Id);
			if (_item == null)
				throw new FaultException<NotFound>(new NotFound());
			string del = await GetDelimiterAsync();
			try
			{
				var chs = item.Name.Split(new string[] { del }, StringSplitOptions.None);
				_item.Name = item.Name;
				_item.LeftName = chs[0];
				_item.RightName = chs[1];
				await db.FullChecks.UpdateAsync(_item);
			}
			catch (Exception ex)
			{
				throw new FaultException<ModelCheck>(new ModelCheck(ex));
			}


		}


		public async Task RemoveCheckAsync(int Id)
		{
			try
			{
				await db.FullChecks.DeleteAsync(Id);
			}
			catch (Exception ex)
			{
				throw new FaultException<ModelCheck>(new ModelCheck(ex));
			}
		}

		public async Task RemoveChecksAsync(IEnumerable<int> Ids)
		{
			try
			{
				foreach (int Id in Ids)
					await db.FullChecks.DeleteAsync(Id);
			}
			catch (Exception ex)
			{
				throw new FaultException<ModelCheck>(new ModelCheck(ex));
			}
		}

		#endregion Checks

		/*LICENSE*/
		#region Lic

		public async Task BreakLicenseAsync(int Id)
		{
			var lic = await db.LicenseUsings.GetAsync(Id);
			if (lic != null)
				await db.LicenseUsings.DeleteAsync(lic);
		}

		public async Task BreakLicensesAsync(IEnumerable<int> Ids)
		{
			foreach (int id in Ids)
				await BreakLicenseAsync(id);
		}

		public async Task BreakAllLicensesAsync()
		{
			foreach (var lic in await db.LicenseUsings.GetAllAsync())
				await db.LicenseUsings.DeleteAsync(lic);
		}

		
		public async Task<LicenseDTO> GetLicenseAsync()
		{
			var lic = await db.License.GetAsync(0);
			if (lic == null)
				throw new FaultException<NullKey>(new NullKey());

			(DateTime? date, int qnt) = GetLicenseParamsAsync(lic.Key);
			if (date == null || qnt == 0)
				throw new FaultException<WrongKey>(new WrongKey());
		
			return new LicenseDTO()
			{
				EndLicDate = date.Value,
				Success = date >= DateTime.Now.Date && qnt > 0,
				Key = lic.Key, 
				LicensseQnt = qnt
			};
		}


		public async Task SetLicenseAsync(string key)
		{
			var lic = await db.License.GetAsync(0);
			if (lic != null)
				await db.License.DeleteAsync(0);
			await db.License.CreateAsync(new License() { Key = key });

		}

		public async Task<IEnumerable<LicenseUsingDTO>> GetAllLicenseUsingsAsync()
		{
			return await GetLicenseUsingAsync(db.LicenseStatistics.GetAll());
		}


		public async Task<IEnumerable<LicenseUsingDTO>> GetLicenseUseingAsync(int monthsQnt)
		{
			DateTime _date = DateTime.Now.AddMonths(-monthsQnt);
			return await GetLicenseUsingAsync(db.LicenseStatistics.GetAll().Where(l => l.Date >= _date));
		}

		private async Task<IEnumerable<LicenseUsingDTO>> GetLicenseUsingAsync(IQueryable<LicenseUsersStatistic> iqstat)
		{
			return await db.LicenseUsings.GetAll().GroupJoin(iqstat,
			u => u.Id,
			us => us.StantionId,
			(lu, lus) =>
			new LicenseUsingDTO()
			{
				Id = lu.Id,
				StantionName = lu.StantionName,
				UserName = lu.UserName,
				LastAccess = lu.LastAccess,
				LastAllUsingQnt = lus.Sum(x => x.Qnt),
				LastUniqUsingQnt = lus.Sum(x => x.UniqQnt)
			}).ToListAsync();
		}

		public async Task<IEnumerable<LicenseMonthUsingDTO>> GetLicenseMonthsUsingAsync(int monthsQnt)
		{
			DateTime _date = DateTime.Now.AddMonths(-monthsQnt);
			DateTime date = new DateTime(_date.Year, _date.Month, 1);
			ICollection<LicenseMonthUsingDTO> result = new List<LicenseMonthUsingDTO>();
			for (int i = 1; i <= monthsQnt; i++)
			{
				DateTime _d = date.AddMonths(i);
				var qw = db.LicenseStatistics.GetAll().Where(x => x.YearNum == _d.Year && x.MonthNum == _d.Month);
				result.Add(new LicenseMonthUsingDTO()
				{
					Year = _d.Year,
					MonthNum = _d.Month,
					AllQnt = qw.Count() > 0 ? await qw.Select(s => s.Qnt).SumAsync() : 0,
					UniqQnt = qw.Count() > 0 ? await qw.Select(s => s.UniqQnt).SumAsync() : 0
				});
			}
			return result;

		}

		public async Task<IEnumerable<LicenseCategogiesPercentDTO>> GetLicenseCategogiesPercentAsync(int monthsQnt)
		{
			DateTime _date = DateTime.Now.Date.AddMonths(-monthsQnt);
			var iqw = db.LicenseStatistics.GetAll()
				.Where(x => x.Date >= _date);
			int qnt = await db.LicenseUsings.GetAll().CountAsync();

			return new List<LicenseCategogiesPercentDTO>()
			{
				new LicenseCategogiesPercentDTO(){ CategoryName = "firstcat" },
				new LicenseCategogiesPercentDTO(){ CategoryName = "secondcat" },
				new LicenseCategogiesPercentDTO(){ CategoryName = "thirdcat" },
			};

		}

		public async Task<int> GetLicenseQnt()
		{
			try
			{
				return (GetLicenseParamsAsync((await db.License.GetAsync(0)).Key)).qnt;
			}
			catch
			{
				return 0;
			}
			
		}

		public async Task<int> GetLicenseUsedQnt(int monthsQnt)
		{
			try
			{
				DateTime _date = DateTime.Now.Date.AddMonths(-monthsQnt);
				return await db.LicenseUsings.GetAll().Where(x => x.LastAccess >= _date).CountAsync();
			}
			catch
			{
				return 0;
			}
			
		}

		public async Task<int> GetAllLicenseUsedQnt()
		{
			return await db.LicenseUsings.GetAll().CountAsync();
		}

		#endregion Lic

		/*PARAMS*/
		#region Params

		public async Task<ParamsDTO> GetParamsAsync()
		{
			var par = await db.Params.GetAsync(0);
			return new ParamsDTO()
			{
				Delimiter = par.Delimiter,
				DatRetentionPeriod = par.DatRetentionPeriod
			};
		}

		public async Task SetParams(ParamsDTO item)
		{
			var par = await db.Params.GetAsync(0);
			par.Delimiter = item.Delimiter;
			par.DatRetentionPeriod = item.DatRetentionPeriod;
			await db.Params.UpdateAsync(par);
		}

		#endregion Params

		/*REPORTS*/
		#region Reports

		public async Task<ReportStatusDTO> GetDailyChecksSlice(IEnumerable<int> checkIds, DateTime? date)
		{
			return await GetDailySlice(db.DailySlices.GetAll().Where(x => checkIds.Contains(x.FullCheckId)), date);
		}

		public async Task<ReportStatusDTO> GetDailyConstructionsSlice(IEnumerable<int> constructionIds, DateTime? date = null)
		{
			if (constructionIds == null)
				return new ReportStatusDTO();
			return await GetDailySlice(db.DailySlices.GetAll().Where(x => constructionIds.Contains(x.ConstructionId)), date);
		}

		private async Task<ReportStatusDTO> GetDailySlice(IQueryable<DailySlice> result, DateTime? date)
		{
			if (date == null)
			{
				var q = await db.DailySlices.GetAll().CountAsync();
				if (q == 0)
					date = DateTime.Now;
				else
					date = await db.DailySlices.GetAll().MaxAsync(d => d.Date);
			}

			result = result.Where(x => x.Date == date.Value);

			if (await result.CountAsync() == 0)
				return new ReportStatusDTO();
			return new ReportStatusDTO()
			{
				ActiveQnt = await result.SumAsync(s => s.ActiveQnt),
				AnalizedQnt = await result.SumAsync(s => s.AnalizedQnt),
				ClashesQnt = await result.SumAsync(s => s.ClashesQnt),
				ConfirmedQnt = await result.SumAsync(s => s.ConfirmedQnt),
				CorrectedQnt = await result.SumAsync(s => s.CorrectedQnt),
				CreatedQnt = await result.SumAsync(s => s.CreatedQnt)
			};
		}

		public async Task<IEnumerable<DailySliceDTO>> GetDailySlices(IEnumerable<int> checkIds, DateTime? sdate, DateTime? edate)
		{
			if (checkIds == null)
				return new List<DailySliceDTO>();
			IQueryable<DailySlice> result = db.DailySlices.GetAll().Where(x => checkIds.Contains(x.FullCheckId));
			if (sdate != null)
				result = result.Where(x => x.Date >= sdate.Value);
			if (edate != null)
				result = result.Where(x => x.Date <= edate.Value);

			return await GetSlices(result);

		}

		public async Task<IEnumerable<DailySliceDTO>> GetDailyConstructionSlices(IEnumerable<int> Ids, DateTime? sdate, DateTime? edate)
		{
			if (Ids == null)
				return new List<DailySliceDTO>();
			
			if (sdate == null)
				sdate = DateTime.Now.Date.AddDays(-14);
			if (edate == null)
				edate = DateTime.Now.Date;
			IQueryable<DailySlice> result = db.DailySlices.GetAll().Where(x => Ids.Contains(x.ConstructionId) && x.Date >= sdate && x.Date <=edate); 
			return await GetSlices(result);

		}

		private async Task<IEnumerable<DailySliceDTO>> GetSlices(IQueryable<DailySlice> qresult)
		{
			return await qresult.GroupBy(g => g.Date).Select(x => new DailySliceDTO()
			{
				Date = x.Key,
				ActiveQnt = x.Sum(s => s.ActiveQnt),
				AnalizedQnt = x.Sum(s => s.AnalizedQnt),
				ClashesQnt = x.Sum(s => s.ActiveQnt) + x.Sum(s => s.CreatedQnt),
				ConfirmedQnt = x.Sum(s => s.ConfirmedQnt),
				CorrectedQnt = x.Sum(s => s.CorrectedQnt),
				CreatedQnt = x.Sum(s => s.CreatedQnt)
			}).AsNoTracking().ToListAsync();
		}

		public async Task<IEnumerable<FullCheckDTO>> GetDynamicChecksAsync(int constructionId, DateTime? sdate, DateTime? edate)
		{
			IQueryable<DailySlice> result = db.DailySlices.GetAll().Where(x => x.ConstructionId == constructionId);
			IQueryable<FullCheck> checks = db.FullChecks.GetAll().Where(x => x.ConstructionId == constructionId);
			if (await checks.CountAsync() == 0 || await result.CountAsync() == 0)
				return new List<FullCheckDTO>();
			DateTime _sdate = await result.MinAsync(x => x.Date);
			DateTime _edate = await result.MaxAsync(x => x.Date);

			if (sdate == null || sdate < _sdate)
				sdate = _sdate;
			if (edate == null || edate > _edate)
				edate = _edate;

			IQueryable<DailySlice> firstresult = result.Where(x => x.Date == sdate);
			IQueryable<DailySlice> lastresult = result.Where(x => x.Date == edate);

			return await (from t1 in checks
						  from t2 in firstresult.Where(x => x.FullCheckId == t1.Id).DefaultIfEmpty()
						  from t3 in lastresult.Where(x => x.FullCheckId == t1.Id).DefaultIfEmpty()
						  select new FullCheckDTO()
						  {
							  Id = t1.Id,
							  Name = t1.Name,
							  LeftCheckName = t1.LeftName,
							  RightCheckName = t1.RightName,

							  StartPeriodClashesQnt = t2 == null ? 0 : t2.CreatedQnt + t2.ActiveQnt,
							  EndPeriodClashesQnt = t3 == null ? 0 : t3.CreatedQnt + t3.ActiveQnt,

							  StartPeriodCreatedQnt = t2 == null ? 0 : t2.CreatedQnt,
							  EndPeriodCreatedQnt = t3 == null ? 0 : t3.CreatedQnt,

							  StartPeriodActiveQnt = t2 == null ? 0 : t2.ActiveQnt,
							  EndPeriodActiveQnt = t3 == null ? 0 : t3.ActiveQnt,

							  StartPeriodAnalizedQnt = t2 == null ? 0 : t2.AnalizedQnt,
							  EndPeriodAnalizedQnt = t3== null ? 0 : t3.AnalizedQnt,

							  StartPeriodConfirmedQnt = t2 == null ? 0 : t2.ConfirmedQnt,
							  EndPeriodConfirmedQnt = t3 == null ? 0 : t3.ConfirmedQnt,

							  StartPeriodCorrectedQnt = t2 == null ? 0 : t2.CorrectedQnt,
							  EndPeriodCorrectedQnt = t3 == null ? 0 : t3.CorrectedQnt,

						  }).AsNoTracking().ToListAsync();
		}

		#endregion Reports

		/*MATRIX*/
		#region Matrix
		public async Task<IEnumerable<MatrixDTO>> GetCategoryMatrixAsync(int constructionId)
		{
			var result = db.Clashes.GetAll().Where(x => x.ConstructionId == constructionId && (x.ClashStatusId == 1 || x.ClashStatusId == 2));
			if (await result.CountAsync() > 0)
			{
				return await result.GroupBy(g => new { left = g.CategoryElement1Id.Value, lname = g.CategoryElement1.Name, right = g.CategoryElement2Id.Value, rname = g.CategoryElement2.Name }).Select(
					x => new MatrixDTO()
					{
						 LeftId  = x.Key.left, LeftName = x.Key.lname, RightId = x.Key.right, RightName = x.Key.rname, Qnt = x.Count()
					}).AsNoTracking().ToListAsync();
			}
			return new List<MatrixDTO>();
		}

		public async Task<IEnumerable<MatrixDTO>> GetModelMatrixAsync(int constructionId)
		{
			var result = db.Clashes.GetAll().Where(x => x.ConstructionId == constructionId && (x.ClashStatusId == 1 || x.ClashStatusId == 2));
			if (await result.CountAsync() > 0)
			{
				return await result.GroupBy(g => new { left = g.RevitModel1Id.Value, lname = g.RevitModel1.Name, right = g.RevitModel2Id.Value, rname = g.RevitModel2.Name }).Select(
					x => new MatrixDTO()
					{
						LeftId = x.Key.left,
						LeftName = x.Key.lname,
						RightId = x.Key.right,
						RightName = x.Key.rname,
						Qnt = x.Count()
					}).AsNoTracking().ToListAsync();
			}
			return new List<MatrixDTO>();
		}

		#endregion Matrix
	}
}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Bimacad.Sys;
using Coordinator.DTO;
using Coordinator.ISRC;
using ModelChecker.DAL.Entities;
using ModelChecker.DAL.Interfaces;

namespace ModelChecker.BLL.Services
{
	[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple,
	   InstanceContextMode = InstanceContextMode.Single,
	   IncludeExceptionDetailInFaults = false)]

	public class CoordinatorService : BaseService, ICoordinatorService
	{
		public CoordinatorService(IUnitOfWork uw) : base(uw) { }

		public bool CheckConnection() => true;

		public IEnumerable<ConstructionDTO> GetAllConstructions() =>
			GetAllConstructionsAsync().GetAwaiter().GetResult();

		public async Task<IEnumerable<ConstructionDTO>> GetAllConstructionsAsync()
		{
			return (await db.Constructions.GetAll().Where(x => x.FullChecks.Count() > 0).ToListAsync()).Select(x => new ConstructionDTO()
			{
				Id = x.Id,
				Name = x.Name,
				Parents = new List<int>()
			});
		}

		public IEnumerable<StatusDTO> GetAllStatuses() =>
			GetAllStatusesAsync().GetAwaiter().GetResult();

		public async Task<IEnumerable<StatusDTO>> GetAllStatusesAsync()
		{
			return (await db.ClashStatuses.GetAllAsync()).Select(x => new StatusDTO()
			{
				Id = x.Id,
				Name = x.Name,
				Parents = new List<int>()
			});
		}

		public IEnumerable<CheckDTO> GetAllChecks() =>
			 GetAllChecksAsync().GetAwaiter().GetResult();

		public async Task<IEnumerable<CheckDTO>> GetAllChecksAsync()
		{
			return (await db.FullChecks.GetAllAsync()).Select(x => new CheckDTO()
			{
				Id = x.Id,
				Name = x.Name, Parents = new List<int> {x.ConstructionId }
			});
		}


		public IEnumerable<RevitModelDTO> GetAllRevitModels() =>
			GetAllRevitModelsAsync().GetAwaiter().GetResult();

		public async Task<IEnumerable<RevitModelDTO>> GetAllRevitModelsAsync()
		{
			var ids1 = await db.Clashes.GetAll().Select(g => new ModelFullCheck { RevitModelId = g.RevitModel1Id.Value, FullCheckId = g.FullCheckId }).Distinct().ToListAsync();
			var ids2 = await db.Clashes.GetAll().Select(g => new ModelFullCheck { RevitModelId = g.RevitModel2Id.Value, FullCheckId = g.FullCheckId }).Distinct().ToListAsync();
			var ids = ids1.Union(ids2);
			var result = (await db.RevitModels.GetAllAsync()).GroupJoin(ids,
				m => m.Id,
				t => t.RevitModelId,
				(rm, mc) =>
				new RevitModelDTO()
				{
					Id = rm.Id, Name = rm.Name, Parents = mc.Select(s => s.FullCheckId).ToList()
				}).ToList();
			return result;
		}

		private class ModelFullCheck
		{
			internal int RevitModelId { get; set; }
			internal int FullCheckId { get; set; }

			public override bool Equals(object obj)
			{
				if (obj is ModelFullCheck _mc)
					return RevitModelId == _mc.RevitModelId && FullCheckId == _mc.FullCheckId;
				return false;
			}

			public override string ToString()
			{
				return $"{RevitModelId} {FullCheckId}";
			}

			public override int GetHashCode()
			{
				return ToString().GetHashCode();
			}
		}

		public ClashesResultDTO GetClashes(ClashFilterDTO clashFilter) =>
			GetClashesAsync(clashFilter).GetAwaiter().GetResult();

		public ClashesResultDTO GetAllClashes(ClashFilterDTO clashFilter) =>
			GetAllClashesAsync(clashFilter).GetAwaiter().GetResult();

		public async Task<ClashesResultDTO> GetAllClashesAsync(ClashFilterDTO clashFilter)
		{
			IQueryable<Clash> result = GetSQLClashes(clashFilter);
			return new ClashesResultDTO()
			{
				ClashesCount = await result.CountAsync(),
				PagesCount = (int)Math.Ceiling((double)await result.CountAsync() / clashFilter.PageSize),
				Clashes = await GetClashesAsync(result)
			};
		}

		
		public async Task<ClashesResultDTO> GetClashesAsync(ClashFilterDTO clashFilter)
		{
			IQueryable<Clash> result = GetSQLClashes(clashFilter);
			//результат
			return new ClashesResultDTO()
			{
				ClashesCount = await result.CountAsync(),
				PagesCount = (int)Math.Ceiling((double)await result.CountAsync() / clashFilter.PageSize),
				Clashes = await GetClashesAsync(result.Skip((clashFilter.PageNum - 1) * clashFilter.PageSize)
				.Take(clashFilter.PageSize))
			};
		}

		private async Task<IEnumerable<ClashDTO>> GetClashesAsync(IQueryable<Clash> result)
		{
			return await result
				.Select(x => new ClashDTO()
				{
					Id = x.Id,
					ConstructionName = x.FullCheck.Construction.Name,
					CheckName = x.FullCheck.Name,
					ClashStatusName = x.ClashStatus.Name,
					Distance = x.Distance,
					Odate = x.Odate,
					FoundDate = x.FoundDate,
					X = x.X,
					Y = x.Y,
					Z = x.Z,
					//element 1
					RevitElement1Id = x.RevitElement1.RevitId,
					Element1Level = x.RevitElement1.Level,
					RevitElement1Name = x.RevitElement1.Name,
					CategoryElement1Name = x.CategoryElement1.Name,
					RevitModel1Name = x.RevitModel1.Name,

					//element 2
					RevitElement2Id = x.RevitElement2.RevitId,
					Element2Level = x.RevitElement2.Level,
					RevitElement2Name = x.RevitElement2.Name,
					CategoryElement2Name = x.CategoryElement2.Name,
					RevitModel2Name = x.RevitModel2.Name,
				}).ToListAsync();
		}

		private IQueryable<Clash> GetSQLClashes(ClashFilterDTO clashFilter)
		{
			//выборка
			IQueryable<Clash> result = db.Clashes.GetAll();

			if (clashFilter.SearchId?.Length > 0 && int.TryParse(clashFilter.SearchId, out int _search))
			{
				result = result.Where(x => x.RevitElement1.RevitId == _search || x.RevitElement2.RevitId == _search);
			}
			else
			{
				if (clashFilter.Constructions != null && clashFilter.Constructions.Count() > 0)
					result = result.Where(x => clashFilter.Constructions.Contains(x.ConstructionId));
				if (clashFilter.Checks != null && clashFilter.Checks.Count() > 0)
					result = result.Where(x => clashFilter.Checks.Contains(x.FullCheckId));
				if (clashFilter.Statuses != null && clashFilter.Statuses.Count() > 0)
					result = result.Where(x => clashFilter.Statuses.Contains(x.ClashStatusId.Value));
				if (clashFilter.RevitModels != null && clashFilter.RevitModels.Count() > 0)
					result = result.Where(x => clashFilter.RevitModels.Contains(x.RevitModel1Id.Value) || clashFilter.RevitModels.Contains(x.RevitModel2Id.Value));
			}
			//сортировка
			if (clashFilter.Sorts == Sorts.Distantion)
			{
				if (clashFilter.Orders == Orders.Descending)
					result = result.OrderByDescending(x => x.Distance);
				else
					result = result.OrderBy(x => x.Distance);
			}
			else if (clashFilter.Sorts == Sorts.ChangeDate)
			{
				if (clashFilter.Orders == Orders.Descending)
					result = result.OrderByDescending(x => x.Odate);
				else 
					result = result.OrderBy(x => x.Odate);
			}
			else if (clashFilter.Sorts == Sorts.CreateDate)
			{
				if (clashFilter.Orders == Orders.Descending)
					result = result.OrderByDescending(x => x.FoundDate);
				else
					result = result.OrderBy(x => x.FoundDate);
			}
			else
			{
				result = result.OrderBy(x => x.Id);
			}
			return result;
		}

		

		public bool UpdateClash(ClashDTO clash) =>
			UpdateClashAsync(clash).GetAwaiter().GetResult();

		public async Task<bool> UpdateClashAsync(ClashDTO clash)
		{
			try
			{
				var _clash = await db.Clashes.GetAsync(clash.Id);
				if (_clash != null)
				{
					_clash.ClashStatusId = clash.ClashStatusId;
					await db.Clashes.UpdateAsync(_clash);
				}
				return true;
			}
			catch (Exception ex)
			{
				throw new FaultException<ModelCheck>(new ModelCheck(ex));
			}
		}


		//[FaultContract(typeof(ModelCheck))]
		//[FaultContract(typeof(NullKey))]
		//[FaultContract(typeof(WrongKey))]
		//[FaultContract(typeof(ZeroQnt))]
		//public bool TakeLicense(string clientname, string username)
		//{
		//	try
		//	{
		//		return TakeLicenseAsync(clientname, username).GetAwaiter().GetResult();
		//	}
		//	catch (FaultException<WrongKey> kl)
		//	{
		//		throw kl;
		//	}
		//	catch (Exception ex)
		//	{
		//		throw ex;
		//	}
		//}


		public bool TakeLicense(string clientname, string username)
		{

			//проверяем есть ли лицензия
			var lic = db.License.Get(0);
			if (lic == null)
				throw new FaultException<NullKey>(new NullKey());

			(DateTime? date, int qnt) = GetLicenseParamsAsync(lic.Key);
			if (date == null || qnt == 0)
				throw new FaultException<WrongKey>(new WrongKey());
			//сверяем количество
			int usqnt = qnt - db.LicenseUsings.GetAll().Where(x => x.StantionName == null).Count();
			LicenseUsing licpc = db.LicenseUsings.GetAll().Where(x => x.StantionName == clientname).FirstOrDefault();

			if (usqnt == 0 && licpc == null)
				throw new FaultException<ZeroQnt>(new ZeroQnt());

			//если новый пк - регистрируем
			DateTime today = DateTime.Now;
			bool unic;
			if (licpc == null)
			{
				unic = true;
				licpc = new LicenseUsing()
				{
					StantionName = clientname,
					UserName = username,
					LastAccess = today,
					LastAccessDate = today.Date
				};
				db.LicenseUsings.CreateAsync(licpc).GetAwaiter().GetResult();
			}
			else
			{
				unic = today.Date != licpc.LastAccessDate;
				licpc.LastAccess = today;
				licpc.LastAccessDate = today.Date;
				licpc.UserName = username;
				db.LicenseUsings.UpdateAsync(licpc).GetAwaiter().GetResult();
			}
			//пишем статистику
			LicenseUsersStatistic stat = db.LicenseStatistics.GetAll().Where(x => x.Date == today.Date && x.StantionId == licpc.Id).FirstOrDefault();
			if (stat == null)
			{
				db.LicenseStatistics.CreateAsync(new LicenseUsersStatistic()
				{
					Date = today.Date,
					MonthNum= DateTime.Now.Month,
					YearNum = DateTime.Now.Year,
					Qnt = 1,
					UniqQnt = 1, 
					StantionId = licpc.Id
				}).GetAwaiter().GetResult();
			}
			else
			{
				stat.Qnt += 1;
				//if (unic)
				//	stat.UniqQnt += 1;
				db.LicenseStatistics.UpdateAsync(stat).GetAwaiter().GetResult();
			}
			return true;

		}
	}
}

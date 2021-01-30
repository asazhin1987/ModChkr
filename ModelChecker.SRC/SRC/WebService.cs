using ModelChecker.ISRC;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ModelChecker.DTO;
using System.ServiceModel;
using ModelChecker.SRC.Factories;
using Bimacad.Sys;


namespace ModelChecker.SRC
{
	[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple,
	   InstanceContextMode = InstanceContextMode.Single,
	   IncludeExceptionDetailInFaults = true)]

	public class WebService<T> : IWebService where T : IService
	{
		private readonly string Url;

		public WebService(string host)
		{
			Url = $"http://{host}:80/ModelCheckerWebService";
		}

		#region Proxy


		//void 
		private async Task UseWebProxyClient(Func<IWebService, Task> accessor)
		{
			using (ProxyFactory<T> proxy = new ProxyFactory<T>(Url))
			{
				try
				{
					await accessor((IWebService)proxy.Service);
				}
				catch (CommunicationException e)
				{
					throw new ModelCheckerException(e.Message);
				}
			}
		}

		//object 
		private async Task<M> UseWebProxyClient<M>(Func<IWebService, Task<M>> accessor) where M: ModelCheckerDTO
		{
			using (ProxyFactory<T> proxy = new ProxyFactory<T>(Url))
			{
				try
				{
					return await accessor((IWebService)proxy.Service);
				}
				catch (FaultException<NullKey>)
				{
					throw new NullKeyException();
				}
				catch (FaultException<WrongKey>)
				{
					throw new WrongKeyException();
				}
				catch (FaultException<ZeroQnt>)
				{
					throw new ZeroQntException();
				}
				catch (FaultException fkex)
				{
					throw fkex;
				}
				catch (CommunicationException e)
				{
					throw new ModelCheckerException(e.Message);
				}
				
				catch (Exception ex)
				{
					throw new ModelCheckerException(ex.Message);
				}
			}
		}

		//object collection
		private async Task<IEnumerable<M>> UseWebProxyClient<M>(Func<IWebService, Task<IEnumerable<M>>> accessor) where M : ModelCheckerDTO
		{
			using (ProxyFactory<T> proxy = new ProxyFactory<T>(Url))
			{
				try
				{
					return await accessor((IWebService)proxy.Service);
				}
				catch (CommunicationException e)
				{
					throw new ModelCheckerException(e.Message);
				}
			}
		}


		//sync type
		private M UseWebProxyClient<M> (Func<IWebService, M> accessor)
		{
			using (ProxyFactory<T> proxy = new ProxyFactory<T>(Url))
			{
				try
				{
					return accessor((IWebService)proxy.Service);
				}
				catch (CommunicationException e)
				{
					throw new ModelCheckerException(e.Message);
				}
			}
		}
		#endregion Proxy

		#region License

		public bool CheckLicense()
		{
			return UseWebProxyClient(x => x.CheckLicense());
		}

		public async Task<LicenseDTO> GetLicenseAsync() =>
			await UseWebProxyClient(x => x.GetLicenseAsync());


		public async Task SetLicenseAsync(string key) =>
			await UseWebProxyClient(x => x.SetLicenseAsync(key));

		public async Task BreakLicenseAsync(int Id) =>
			await UseWebProxyClient(x => x.BreakLicenseAsync(Id));

		public async Task BreakLicensesAsync(IEnumerable<int> Ids) =>
			await UseWebProxyClient(x => x.BreakLicensesAsync(Ids));

		public async Task BreakAllLicensesAsync() =>
			await UseWebProxyClient(x => x.BreakAllLicensesAsync());

		public async Task<IEnumerable<LicenseUsingDTO>> GetAllLicenseUsingsAsync() =>
			await UseWebProxyClient(x => x.GetAllLicenseUsingsAsync());

		public async Task<IEnumerable<LicenseUsingDTO>> GetLicenseUseingAsync(int monthsQnt) =>
			await UseWebProxyClient(x => x.GetLicenseUseingAsync(monthsQnt));

		public async Task<IEnumerable<LicenseMonthUsingDTO>> GetLicenseMonthsUsingAsync(int nomthQnt) =>
			await UseWebProxyClient(x => x.GetLicenseMonthsUsingAsync(nomthQnt));

		public async Task<IEnumerable<LicenseCategogiesPercentDTO>> GetLicenseCategogiesPercentAsync(int monthsQnt) =>
			await UseWebProxyClient(x => x.GetLicenseCategogiesPercentAsync(monthsQnt));

		public async Task<int> GetLicenseQnt() =>
			await UseWebProxyClient(x => x.GetLicenseQnt());

		public async Task<int> GetLicenseUsedQnt(int monthsQnt) =>
			await UseWebProxyClient(x => x.GetLicenseUsedQnt(monthsQnt));

		public async Task<int> GetAllLicenseUsedQnt() =>
		await UseWebProxyClient(x => x.GetAllLicenseUsedQnt());

		#endregion License

		#region Constructons

		public async Task<ConstructionDTO> GetConstructionAsync(int ID) =>
			await UseWebProxyClient(x => x.GetConstructionAsync(ID));

		public async Task<ConstructionDTO> GetConstructionEmptyAsync(int? ID) =>
			await UseWebProxyClient(x => x.GetConstructionEmptyAsync(ID));

		public async Task<IEnumerable<ConstructionDTO>> GetAllConstructionsAsync() =>
			await UseWebProxyClient(x => x.GetAllConstructionsAsync());

		public async Task RemoveConstruction(int Id)
		{
			await UseWebProxyClient(x => x.RemoveConstruction(Id));
		}


		public async Task UpdateConstructionAsync(ConstructionDTO item)
		{
			await UseWebProxyClient(x => x.UpdateConstructionAsync(item));
		}

		public IEnumerable<ConstructionDTO> GetAllConstructionsEmptyAsync()
		{
			return UseWebProxyClient(x => x.GetAllConstructionsEmptyAsync());
		}

		public async Task<IEnumerable<DailySliceDTO>> GetDailyConstructionSlices(IEnumerable<int> Ids, DateTime? sdate, DateTime? edate)
		{
			return await UseWebProxyClient(x => x.GetDailyConstructionSlices(Ids, sdate, edate));
		}
		#endregion Constructions

		#region Check
		public async Task<IEnumerable<FullCheckDTO>> GetChecksAsync(int constructionId) =>
			await UseWebProxyClient(x => x.GetChecksAsync(constructionId));


		public async Task<FullCheckDTO> GetCheckAsync(int Id) =>
			await UseWebProxyClient(x => x.GetCheckAsync(Id));

		public async Task RemoveCheckAsync(int Id) =>
			await UseWebProxyClient(x => x.RemoveCheckAsync(Id));

		public async Task RemoveChecksAsync(IEnumerable<int> Ids)
		{
			await UseWebProxyClient(x => x.RemoveChecksAsync(Ids));
		}

		public async Task UpdateCheckAsync(FullCheckDTO item)
		{
			await UseWebProxyClient(x => x.UpdateCheckAsync(item));
		}



		#endregion Check


		#region params

		public async Task<ParamsDTO> GetParamsAsync()
		{
			return await UseWebProxyClient(x => x.GetParamsAsync());
		}

		public async Task SetParams(ParamsDTO item)
		{
			await UseWebProxyClient(x => x.SetParams(item));
		}

		#endregion params

		#region Reports

		public async Task<IEnumerable<DailySliceDTO>> GetDailySlices(IEnumerable<int> checkIds, DateTime? sdate, DateTime? edate)
		{
			return await UseWebProxyClient(x => x.GetDailySlices(checkIds, sdate, edate));
		}

		public async Task<IEnumerable<FullCheckDTO>> GetDynamicChecksAsync(int constructionId, DateTime? sdate, DateTime? edate)
		{
			return await UseWebProxyClient(x => x.GetDynamicChecksAsync(constructionId, sdate, edate));
		}

		#endregion Reports


		#region Matrix
		public async Task<IEnumerable<MatrixDTO>> GetCategoryMatrixAsync(int constructionId)
		{
			return await UseWebProxyClient(x => x.GetCategoryMatrixAsync(constructionId));
		}

		public async Task<IEnumerable<MatrixDTO>> GetModelMatrixAsync(int constructionId)
		{
			return await UseWebProxyClient(x => x.GetModelMatrixAsync(constructionId));
		}


		public async Task<ReportStatusDTO> GetDailyChecksSlice(IEnumerable<int> checkIds, DateTime? date)
		{
			return await UseWebProxyClient(x => x.GetDailyChecksSlice(checkIds, date));
		}

		public async Task<ReportStatusDTO> GetDailyConstructionsSlice(IEnumerable<int> constructionIds, DateTime? date = null)
		{
			return await UseWebProxyClient(x => x.GetDailyConstructionsSlice(constructionIds, date));
		}


		#endregion Matrix
	}
}

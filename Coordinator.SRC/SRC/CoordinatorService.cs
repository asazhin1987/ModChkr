using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Bimacad.Sys;
using Coordinator.ISRC;
using Coordinator.SRC.Factories;
using Coordinator.DTO;

namespace Coordinator.SRC
{
	[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple,
	   InstanceContextMode = InstanceContextMode.Single,
	   IncludeExceptionDetailInFaults = true)]

	public class CoordinatorService : ICoordinatorService
	{

		private readonly string Url;

		public CoordinatorService(string host)
		{
			Url = $"http://{host}:80/CoordinatorService";
		}


		#region Proxy

		private M UseWebProxyClient<M>(Func<ICoordinatorService, M> accessor)
		{
			using (ProxyFactory<ICoordinatorService> proxy = new ProxyFactory<ICoordinatorService>(Url))
			{
				try
				{
					return accessor((ICoordinatorService)proxy.Service);
				}
				catch (FaultException<NullKey>)
				{
					throw new NullKeyException();
				}
				catch (FaultException<ZeroQnt>)
				{
					throw new ZeroQntException();
				}
				catch (FaultException<WrongKey>)
				{
					throw new WrongKeyException();
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

		private async Task<M> UseWebProxyClient<M>(Func<ICoordinatorService, Task<M>> accessor)// where M : ModelCheckerDTO
		{
			using (ProxyFactory<ICoordinatorService> proxy = new ProxyFactory<ICoordinatorService>(Url))
			{
				try
				{
					return await accessor((ICoordinatorService)proxy.Service);
				}
				catch (FaultException<NullKey>)
				{
					throw new NullKeyException();
				}
				catch (FaultException<ZeroQnt>)
				{
					throw new ZeroQntException();
				}
				catch (FaultException<WrongKey>)
				{
					throw new WrongKeyException();
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

		#endregion Proxy


		#region src
		public bool TakeLicense(string clientname, string username) =>
			UseWebProxyClient(x => x.TakeLicense(clientname, username));

		public bool CheckLicense() =>
			UseWebProxyClient(x => x.CheckLicense());

		public bool CheckConnection() =>
			UseWebProxyClient(x => x.CheckConnection());

		public IEnumerable<ConstructionDTO> GetAllConstructions() =>
			UseWebProxyClient(x => x.GetAllConstructions());

		public async Task<IEnumerable<ConstructionDTO>> GetAllConstructionsAsync() =>
			await UseWebProxyClient(x => x.GetAllConstructionsAsync());

		public IEnumerable<StatusDTO> GetAllStatuses() =>
			UseWebProxyClient(x => x.GetAllStatuses());

		public async Task<IEnumerable<StatusDTO>> GetAllStatusesAsync() =>
			await UseWebProxyClient(x => x.GetAllStatusesAsync());

		public IEnumerable<CheckDTO> GetAllChecks() =>
			UseWebProxyClient(x => x.GetAllChecks());

		public async Task<IEnumerable<CheckDTO>> GetAllChecksAsync() =>
			await UseWebProxyClient(x => x.GetAllChecksAsync());

		public ClashesResultDTO GetClashes(ClashFilterDTO clashFilter) =>
			UseWebProxyClient(x => x.GetClashes(clashFilter));

		public async Task<ClashesResultDTO> GetClashesAsync(ClashFilterDTO clashFilter) =>
			await UseWebProxyClient(x => x.GetClashesAsync(clashFilter));

		public ClashesResultDTO GetAllClashes(ClashFilterDTO clashFilter) =>
			UseWebProxyClient(x => x.GetAllClashes(clashFilter));

		public async Task<ClashesResultDTO> GetAllClashesAsync(ClashFilterDTO clashFilter) =>
			await UseWebProxyClient(x => x.GetAllClashesAsync(clashFilter));


		public IEnumerable<RevitModelDTO> GetAllRevitModels() =>
			UseWebProxyClient(x => x.GetAllRevitModels());

		public async Task<IEnumerable<RevitModelDTO>> GetAllRevitModelsAsync() =>
			await UseWebProxyClient(x => x.GetAllRevitModelsAsync());

		public bool UpdateClash(ClashDTO clash) =>
			UseWebProxyClient(x => x.UpdateClash(clash));


		public async Task<bool> UpdateClashAsync(ClashDTO clash) =>
			await UseWebProxyClient(x => x.UpdateClashAsync(clash));




		#endregion src
	}
}

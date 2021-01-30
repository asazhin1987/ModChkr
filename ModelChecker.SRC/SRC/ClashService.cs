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

	public class ClashService<T> : IClashService where T : IService
	{
		private readonly string Url;

		public ClashService(string host) 
		{
			Url = $"http://{host}:80/ModelCheckerClashService";
		}


		#region Proxy

		private void UseProxyClient(Action<IClashService> accessor)
		{
			using (ProxyFactory<T> proxy = new ProxyFactory<T>(Url))
			{
				try
				{
					accessor((IClashService)proxy.Service);
				}
				catch (FaultException<ModelCheck> cex)
				{
					throw new ModelCheckerException(cex.Message);
				}
				catch (CommunicationException e)
				{
					throw new ModelCheckerException(e.Message);
				}
			}
		}

		//sync type
		private M UseProxyClient<M>(Func<IClashService, M> accessor)
		{
			using (ProxyFactory<T> proxy = new ProxyFactory<T>(Url))
			{
				try
				{
					return accessor((IClashService)proxy.Service);
				}
				catch (FaultException<ModelCheck> cex)
				{
					throw new ModelCheckerException(cex.Message);
				}
				catch (CommunicationException e)
				{
					throw new ModelCheckerException(e.Message);
				}
			}
		}

		private IEnumerable<M> UseProxyClient<M>(Func<IClashService, IEnumerable<M>> accessor) where M : ModelCheckerDTO
		{
			using (ProxyFactory<T> proxy = new ProxyFactory<T>(Url))
			{
				try
				{
					return accessor((IClashService)proxy.Service);
				}
				catch (FaultException<ModelCheck> cex)
				{
					throw new ModelCheckerException(cex.Message);
				}
				catch (CommunicationException e)
				{
					throw new ModelCheckerException(e.Message);
				}
			}
		}


		#endregion Proxy
		public bool CheckLicense()
		{
			return UseProxyClient(x => x.CheckLicense());
		}

		public bool CheckConnection(string host)
		{
			return UseProxyClient(x => x.CheckConnection(host));
		}

		public string GetDelimiter()
		{
			return UseProxyClient(x => x.GetDelimiter());
		}

		public IEnumerable<ConstructionDTO> GetAllConstructions()
		{
			return UseProxyClient(x => x.GetAllConstructions());
		}

		public IEnumerable<FullCheckDTO> MergeChecks(IEnumerable<FullCheckDTO> items, int constructionId)
		{
			return UseProxyClient(x => x.MergeChecks(items, constructionId));
		}

		public IEnumerable<RevitModelDTO> MergeModels(IEnumerable<RevitModelDTO> items)
		{
			return UseProxyClient(x => x.MergeModels(items));
		}

		public IEnumerable<RevitCategoryDTO> MergeCategories(IEnumerable<RevitCategoryDTO> items)
		{
			return UseProxyClient(x => x.MergeCategories(items));
		}

		public IEnumerable<RevitElementDTO> MergeElements(IEnumerable<RevitElementDTO> items)
		{
			return UseProxyClient(x => x.MergeElements(items));
		}

		public RevitElementDTO MergeElement(RevitElementDTO item)
		{
			return UseProxyClient(x => x.MergeElement(item));
		}

		public void MergeClashes(IEnumerable<ClashDTO> items)
		{
			UseProxyClient(x => x.MergeClashes(items));
		}


		public int SetNotFoundClashStatuses(IEnumerable<int> checks, DateTime odate)
		{
			return UseProxyClient(x => x.SetNotFoundClashStatuses(checks, odate));
		}


		public bool MergeClash(ClashDTO item)
		{
			return UseProxyClient(x => x.MergeClash(item));
		}

		public IEnumerable<int> GetNotFoundClashes(int checkId, DateTime odate)
		{
			return UseProxyClient(x => x.GetNotFoundClashes(checkId, odate));
		}


		public void SetNotFoundClash(int Id, DateTime odate)
		{
			UseProxyClient(x => x.SetNotFoundClash(Id, odate));
		}

		public void SetConstructionDate(int Id, DateTime odate)
		{
			UseProxyClient(x => x.SetConstructionDate(Id, odate));
		}
	}
}

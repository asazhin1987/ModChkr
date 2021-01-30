using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Coordinator.DTO;
using Bimacad.Sys;

namespace Coordinator.ISRC
{
	[ServiceContract]
	public interface ICoordinatorService : IService
	{
		[OperationContract]
		[FaultContract(typeof(ModelCheck))]
		bool CheckLicense();

		[OperationContract]
		[FaultContract(typeof(ModelCheck))]
		[FaultContract(typeof(NullKey))]
		[FaultContract(typeof(WrongKey))]
		[FaultContract(typeof(ZeroQnt))]
		bool TakeLicense(string clientname, string username);

		[OperationContract]
		[FaultContract(typeof(ModelCheck))]
		bool CheckConnection();

		[OperationContract(Name = "UpdateClash")]
		[FaultContract(typeof(ModelCheck))]
		bool UpdateClash(ClashDTO clash);

		[OperationContract(Name = "UpdateClashAsync")]
		[FaultContract(typeof(ModelCheck))]
		Task<bool> UpdateClashAsync(ClashDTO clash);

		[OperationContract(Name = "GetAllStatuses")]
		[FaultContract(typeof(ModelCheck))]
		IEnumerable<StatusDTO> GetAllStatuses();

		[OperationContract(Name = "GetAllStatusesAsync")]
		[FaultContract(typeof(ModelCheck))]
		Task<IEnumerable<StatusDTO>> GetAllStatusesAsync();

		[OperationContract(Name = "GetAllConstructions")]
		[FaultContract(typeof(ModelCheck))]
		IEnumerable<ConstructionDTO> GetAllConstructions();

		[OperationContract(Name = "GetAllConstructionsAsync")]
		[FaultContract(typeof(ModelCheck))]
		Task<IEnumerable<ConstructionDTO>> GetAllConstructionsAsync();

		[OperationContract(Name = "GetAllChecks")]
		[FaultContract(typeof(ModelCheck))]
		IEnumerable<CheckDTO> GetAllChecks();

		[OperationContract(Name = "GetAllChecksAsync")]
		[FaultContract(typeof(ModelCheck))]
		Task<IEnumerable<CheckDTO>> GetAllChecksAsync();

		[OperationContract(Name = "GetAllRevitModels")]
		[FaultContract(typeof(ModelCheck))]
		IEnumerable<RevitModelDTO> GetAllRevitModels();

		[OperationContract(Name = "GetAllRevitModelsAsync")]
		[FaultContract(typeof(ModelCheck))]
		Task<IEnumerable<RevitModelDTO>> GetAllRevitModelsAsync();

		[OperationContract(Name = "GetClashes")]
		[FaultContract(typeof(ModelCheck))]
		ClashesResultDTO GetClashes(ClashFilterDTO clashFilter);

		[OperationContract(Name = "GetClashesAsync")]
		[FaultContract(typeof(ModelCheck))]
		Task<ClashesResultDTO> GetClashesAsync(ClashFilterDTO clashFilter);

		[OperationContract(Name = "GetAllClashes")]
		[FaultContract(typeof(ModelCheck))]
		ClashesResultDTO GetAllClashes(ClashFilterDTO clashFilter);

		[OperationContract(Name = "GetAllClashesAsync")]
		[FaultContract(typeof(ModelCheck))]
		Task<ClashesResultDTO> GetAllClashesAsync(ClashFilterDTO clashFilter);

		

	}
}

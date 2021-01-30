using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using ModelChecker.DTO;
using Bimacad.Sys;


namespace ModelChecker.ISRC
{
	[ServiceContract]
	public interface IWebService : IService
	{
		/*CONSTRUCTIONS*/
		[OperationContract]
		[FaultContract(typeof(ModelCheck))]
		Task<IEnumerable<ConstructionDTO>> GetAllConstructionsAsync();

		[OperationContract]
		[FaultContract(typeof(ModelCheck))]
		IEnumerable<ConstructionDTO> GetAllConstructionsEmptyAsync();

		[OperationContract]
		[FaultContract(typeof(ModelCheck))]
		Task<ConstructionDTO> GetConstructionAsync(int ID);

		[OperationContract]
		[FaultContract(typeof(ModelCheck))]
		Task<IEnumerable<DailySliceDTO>> GetDailyConstructionSlices(IEnumerable<int> Ids, DateTime? sdate, DateTime? edate);

		[OperationContract]
		[FaultContract(typeof(ModelCheck))]
		Task<ConstructionDTO> GetConstructionEmptyAsync(int? ID);

		[OperationContract]
		[FaultContract(typeof(ModelCheck))]
		Task UpdateConstructionAsync(ConstructionDTO item);

		[OperationContract]
		[FaultContract(typeof(ModelCheck))]
		Task RemoveConstruction(int Id);

		/*CHECKS*/

		[OperationContract]
		[FaultContract(typeof(ModelCheck))]
		Task<IEnumerable<FullCheckDTO>> GetChecksAsync(int constructionId);

		[OperationContract]
		[FaultContract(typeof(ModelCheck))]
		Task<FullCheckDTO> GetCheckAsync(int Id);

		[OperationContract]
		[FaultContract(typeof(ModelCheck))]
		Task UpdateCheckAsync(FullCheckDTO item);

		[OperationContract]
		[FaultContract(typeof(ModelCheck))]
		Task RemoveCheckAsync(int Id);

		[OperationContract]
		[FaultContract(typeof(ModelCheck))]
		Task RemoveChecksAsync(IEnumerable<int> Ids);

		/*REPORTS*/
		[OperationContract]
		[FaultContract(typeof(ModelCheck))]
		Task<IEnumerable<DailySliceDTO>> GetDailySlices(IEnumerable<int> checkIds, DateTime? sdate, DateTime? edate);

		[OperationContract]
		[FaultContract(typeof(ModelCheck))]
		Task<ReportStatusDTO> GetDailyChecksSlice(IEnumerable<int> checkIds, DateTime? date);

		[OperationContract]
		[FaultContract(typeof(ModelCheck))]
		Task<ReportStatusDTO> GetDailyConstructionsSlice(IEnumerable<int> constructionIds, DateTime? date = null);


		[OperationContract]
		[FaultContract(typeof(ModelCheck))]
		Task<IEnumerable<FullCheckDTO>> GetDynamicChecksAsync(int constructionId, DateTime? sdate, DateTime? edate);

		/*LICENSE*/
		[OperationContract]
		[FaultContract(typeof(NullKey))]
		[FaultContract(typeof(WrongKey))]
		[FaultContract(typeof(ZeroQnt))]
		Task<LicenseDTO> GetLicenseAsync();

		[OperationContract]
		bool CheckLicense();

		[OperationContract]
		Task SetLicenseAsync(string key);

		[OperationContract]
		Task BreakLicenseAsync(int Id);

		[OperationContract]
		Task BreakLicensesAsync(IEnumerable<int> Ids);

		[OperationContract]
		Task BreakAllLicensesAsync();

		[OperationContract]
		Task<IEnumerable<LicenseUsingDTO>> GetAllLicenseUsingsAsync();

		[OperationContract]
		Task<IEnumerable<LicenseUsingDTO>> GetLicenseUseingAsync(int monthsQnt);

		[OperationContract]
		Task<IEnumerable<LicenseMonthUsingDTO>> GetLicenseMonthsUsingAsync(int nomthQnt);

		[OperationContract]
		Task<IEnumerable<LicenseCategogiesPercentDTO>> GetLicenseCategogiesPercentAsync(int monthsQnt);

		[OperationContract]
		Task<int> GetLicenseQnt();

		[OperationContract]
		Task<int> GetLicenseUsedQnt(int monthsQnt);

		[OperationContract]
		Task<int> GetAllLicenseUsedQnt();

		/*PARAMS*/
		[OperationContract]
		[FaultContract(typeof(ModelCheck))]
		Task<ParamsDTO> GetParamsAsync();

		[OperationContract]
		[FaultContract(typeof(ModelCheck))]
		Task SetParams(ParamsDTO item);


		/*MATRIX*/
		[OperationContract]
		[FaultContract(typeof(ModelCheck))]
		Task<IEnumerable<MatrixDTO>> GetCategoryMatrixAsync(int constructionId);

		[OperationContract]
		[FaultContract(typeof(ModelCheck))]
		Task<IEnumerable<MatrixDTO>> GetModelMatrixAsync(int constructionId);
	}
}

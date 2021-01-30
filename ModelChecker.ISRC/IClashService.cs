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
	public interface IClashService : IService
	{
		[OperationContract]
		string GetDelimiter();

		[OperationContract]
		bool CheckLicense();

		[OperationContract]
		bool CheckConnection(string host);

		[OperationContract]
		[FaultContract(typeof(ModelCheck))]
		IEnumerable<ConstructionDTO> GetAllConstructions();

		[OperationContract]
		[FaultContract(typeof(ModelCheck))]
		IEnumerable<FullCheckDTO> MergeChecks(IEnumerable<FullCheckDTO> items, int constructionId);

		[OperationContract]
		[FaultContract(typeof(ModelCheck))]
		IEnumerable<RevitModelDTO> MergeModels(IEnumerable<RevitModelDTO> items);

		[OperationContract]
		[FaultContract(typeof(ModelCheck))]
		IEnumerable<RevitCategoryDTO> MergeCategories(IEnumerable<RevitCategoryDTO> items);

		[OperationContract]
		[FaultContract(typeof(ModelCheck))]
		IEnumerable<RevitElementDTO> MergeElements(IEnumerable<RevitElementDTO> items);

		[OperationContract]
		[FaultContract(typeof(ModelCheck))]
		RevitElementDTO MergeElement(RevitElementDTO item);

		[OperationContract]
		[FaultContract(typeof(ModelCheck))]
		void MergeClashes(IEnumerable<ClashDTO> items);

		[OperationContract]
		[FaultContract(typeof(ModelCheck))]
		bool MergeClash(ClashDTO item);

		[OperationContract]
		[FaultContract(typeof(ModelCheck))]
		int SetNotFoundClashStatuses(IEnumerable<int> checks, DateTime odate);

		[OperationContract]
		[FaultContract(typeof(ModelCheck))]
		IEnumerable<int> GetNotFoundClashes(int checkId, DateTime odate);


		[OperationContract]
		[FaultContract(typeof(ModelCheck))]
		void SetNotFoundClash(int Id, DateTime odate);

		[OperationContract]
		[FaultContract(typeof(ModelCheck))]
		void SetConstructionDate(int Id, DateTime odate);
	}
}

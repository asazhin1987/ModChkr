using ModelChecker.ISRC;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ServiceModel;
using ModelChecker.DTO;
using ModelChecker.DAL.Interfaces;
using ModelChecker.DAL.Entities;
using System.Data.Entity;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using ModelChecker.BLL.Extensions;
using Bimacad.Sys;

namespace ModelChecker.BLL.Services
{

	[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple,
	   InstanceContextMode = InstanceContextMode.Single,
	   IncludeExceptionDetailInFaults = true)]
	public class ClashService : BaseService, IClashService
	{

		public ClashService(IUnitOfWork uw) : base(uw) { }

		public bool CheckConnection(string host)
		{
			return true;
		}


		public IEnumerable<ConstructionDTO> GetAllConstructions()
		{
			try
			{
				return db.Constructions.GetAll().AsNoTracking().ToList()
					.Select(x => new ConstructionDTO().Map(x));
				
			}
			catch (Exception ex)
			{
				throw new FaultException<ModelCheck>(new ModelCheck(ex));
			}
		}


		#region MERGE

		public void SetConstructionDate(int Id, DateTime odate)
		{
			try
			{
				var item = db.Constructions.Get(Id);
				if (item != null)
				{
					item.Odate = odate;
					Task t = db.Constructions.UpdateAsync(item);
					t.Wait();
					//db.Constructions.UpdateNotDetectAsync(item).GetAwaiter().GetResult();
				}
			}
			catch { }

		}

		public IEnumerable<RevitModelDTO> MergeModels(IEnumerable<RevitModelDTO> items)
		{
			foreach (var mod in items)
				yield return MergeModel(mod);
		}

		private RevitModelDTO MergeModel(RevitModelDTO item)
		{
			try
			{
				var _item = db.RevitModels.GetAll().Where(x => x.Name == item.Name).FirstOrDefault();
				if (_item == null)
				{
					_item = new RevitModel() { Name = item.Name };
					Task t = db.RevitModels.CreateAsync(_item);
					t.Wait();
					//db.RevitModels.CreateNotDetectAsync(_item).GetAwaiter().GetResult();
				}
				item.Id = _item.Id;
				return item;
			}
			catch (Exception ex)
			{
				throw new FaultException<ModelCheck>(new ModelCheck(ex));
			}
		}


		public IEnumerable<RevitCategoryDTO> MergeCategories(IEnumerable<RevitCategoryDTO> items)
		{
			foreach (var cat in items)
				yield return MergeCategory(cat);
		}

		private RevitCategoryDTO MergeCategory(RevitCategoryDTO item)
		{
			try
			{
				var _item = db.RevitCategories.GetAll().Where(x => x.Name == item.Name).FirstOrDefault();
				if (_item == null)
				{
					_item = new RevitCategory() { Name = item.Name };
					Task t = db.RevitCategories.CreateAsync(_item);
					t.Wait();
					//db.RevitCategories.CreateNotDetectAsync(_item).GetAwaiter().GetResult();
				}
				item.Id = _item.Id;
				return item;
			}
			catch (Exception ex)
			{
				throw new FaultException<ModelCheck>(new ModelCheck(ex));
			}


		}

		public IEnumerable<FullCheckDTO> MergeChecks(IEnumerable<FullCheckDTO> items, int constructionId)
		{
			string del = GetDelimiter();

			foreach (var ch in items)
				yield return MergeCheck(ch, del);
		}

		private FullCheckDTO MergeCheck(FullCheckDTO item, string del)
		{
			try
			{
				var _item = db.FullChecks.GetAll()
					.Where(x => x.Name.ToLower() == item.Name.ToLower() && x.ConstructionId == item.ConstructionId)
					.FirstOrDefault();
				if (_item == null)
				{

					var chs = item.Name.Split(new string[] { del }, StringSplitOptions.None);
					_item = new FullCheck()
					{
						Name = item.Name,
						ConstructionId = item.ConstructionId,
						LeftName = chs[0],
						RightName = chs[1],
						Odate = item.Date
					};
					Task t = db.FullChecks.CreateAsync(_item);
					t.Wait();
					//db.FullChecks.CreateNotDetectAsync(_item).GetAwaiter().GetResult();
				}
				else
				{
					_item.Odate = item.Date;
					Task t = db.FullChecks.UpdateAsync(_item);
					t.Wait();
					//db.FullChecks.UpdateNotDetectAsync(_item).GetAwaiter().GetResult();
				}
				item.Id = _item.Id;
				return item;
			}
			catch (Exception ex)
			{
				throw new FaultException<ModelCheck>(new ModelCheck(ex));
			}
		}


		public IEnumerable<RevitElementDTO> MergeElements(IEnumerable<RevitElementDTO> items)
		{
			foreach (var item in items)
				yield return MergeElement(item);
		}


		public RevitElementDTO MergeElement(RevitElementDTO item)
		{
			try
			{
				var _item = db.RevitElements.GetAll()
					.Where(x => x.RevitId == item.RevitId && x.RevitModelId == item.RevitModelId).FirstOrDefault();
				if (_item == null)
				{
					_item = new RevitElement()
					{
						Name = item.Name,
						RevitCategoryId = item.CategoryId,
						RevitModelId = item.RevitModelId,
						RevitId = item.RevitId,
						Level = item.Level
					};
					//db.RevitElements.CreateNotDetectAsync(_item).GetAwaiter().GetResult();
					Task t = db.RevitElements.CreateAsync(_item);
					t.Wait();
				}
				item.Id = _item.Id;
				return item;
			}
			catch (Exception ex)
			{
				throw new FaultException<ModelCheck>(new ModelCheck(ex));
			}

		}

		public void MergeClashes(IEnumerable<ClashDTO> items)
		{
			foreach (var item in items)
				MergeClash(item);
		}


		public int SetNotFoundClashStatuses(IEnumerable<int> checks, DateTime odate)
		{
			var _items = db.Clashes.GetAll().Where(x => checks.Contains(x.FullCheckId) && x.Odate < odate && x.ClashStatusId != 5).Select(x => x.Id).ToList();
			foreach (var _item in _items)
			{
				SetNotFoundClash(_item, odate);
			}
			return _items.Count();
		}

		public IEnumerable<int> GetNotFoundClashes(int checkId, DateTime odate)
		{
			DateTime _odate = odate.AddSeconds(-1);
			var res = db.Clashes.GetAll().Where(x => x.FullCheckId == checkId && x.Odate < _odate && x.ClashStatusId != 5).Select(s => s.Id).ToList();
			return res;
		}

		public void SetNotFoundClash(int Id, DateTime odate)
		{
			try
			{
				var _item = db.Clashes.Get(Id);
				if (_item != null)
				{
					_item.Odate = odate;
					_item.Act = "D";
					_item.ClashStatusId = 5;
					Task t = db.Clashes.UpdateAsync(_item);
					t.Wait();
					//db.Clashes.UpdateNotDetectAsync(_item).GetAwaiter().GetResult();
				}
			}
			catch (Exception ex)
			{
				throw new FaultException<ModelCheck>(new ModelCheck(ex));
			}

		}

		public bool MergeClash(ClashDTO item)
		{
			//IUnitOfWork _db = (IUnitOfWork)db.Clone();
			try
			{
				var _item = db.Clashes.GetAll().Where(x =>
				x.FullCheckId == item.CheckId
				&& x.RevitElement1Id == item.Element1Id
				&& x.RevitModel1Id == item.RevitModel1Id
				&& x.RevitElement2Id == item.Element2Id
				&& x.RevitModel2Id == item.RevitModel2Id).FirstOrDefault();

				if (_item == null)
				{
					_item = new Clash()
					{
						FullCheckId = item.CheckId,

						ConstructionId = item.ConstructionId,
						ClashStatusId = item.ClashStatusId,

						RevitModel1Id = item.RevitModel1Id,
						CategoryElement1Id = item.CategotyElement1Id,
						RevitElement1Id = item.Element1Id,

						RevitModel2Id = item.RevitModel2Id,
						RevitElement2Id = item.Element2Id,
						CategoryElement2Id = item.CategoryElement2Id,

						Distance = item.Distance,
						X = item.X,
						Y = item.Y,
						Z = item.Z,
						FoundDate = DateTime.Now,
						Act = "I",
						Odate = item.Odate
					};
					//db.Clashes.CreateNotDetectAsync(_item).GetAwaiter().GetResult();
					Task t = db.Clashes.CreateAsync(_item);
					t.Wait();
				}
				else
				{
					DateTime _odate = item.Odate.AddSeconds(-1);
					if ((item.ClashStatusId != _item.ClashStatusId) || (item.ClashStatusId == 1 && _item.ClashStatusId == 1 && _odate > _item.Odate))
					{
						if ((item.ClashStatusId == 3 || item.ClashStatusId == 4) && (_item.ClashStatusId != 3 || _item.ClashStatusId != 4))
							_item.ClashStatusId = item.ClashStatusId;
						else if (_item.ClashStatusId < item.ClashStatusId)
							_item.ClashStatusId = item.ClashStatusId;
						else if (item.ClashStatusId == 1 && _item.ClashStatusId == 1)
							_item.ClashStatusId = 2;
					}
					_item.Act = "U";
					_item.Odate = item.Odate;
					//db.Clashes.UpdateNotDetectAsync(_item).GetAwaiter().GetResult();
					Task ut = db.Clashes.UpdateAsync(_item);
					ut.Wait();
				}
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
			finally
			{
				//_db.Dispose();
			}
		}

		#endregion MERGE
	}
}

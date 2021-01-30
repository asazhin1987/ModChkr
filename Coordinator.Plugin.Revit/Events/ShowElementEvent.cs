using System;
using Autodesk.Revit.UI;
using System.Linq;
using Coordinator.DTO;
using Autodesk.Revit.DB;
using System.Collections.Generic;
using Bimacad.Sys;

namespace Coordinator.Plugin.Revit.Events
{
	public class ShowElementEvent : IExternalEventHandler
	{
		public ClashDTO Clash { get; set; }

		private bool Undercut3D { get { return Clash.UndercutView3d; } }
		public double Offset { get { return Clash.Offset; } }
		public double X { get { return Clash.X; } }
		public double Y { get { return Clash.Y; } }
		public double Z { get { return Clash.Z; } }

		private UIDocument uI;
		private Document doc;
		private UIApplication app;

		public void Execute(UIApplication _app)
		{
			app = _app;
			uI = app.ActiveUIDocument;
			doc = uI.Document;
			try
			{
				IEnumerable<ElementId> elements = GetElementsByIds(new List<int> { Clash.RevitElement1Id, Clash.RevitElement2Id }).ToList();
				if (elements.Count() == 0)
					throw new Exception("Элементы не найдены в модели");
				if (Undercut3D)
					Set3DShow(elements);
				else
					SetRevitShow(elements);
			}
			catch (Exception ex)
			{
				TaskDialog.Show("Coordinator", ex.Message);
			}

		}

		private void Set3DShow(IEnumerable<ElementId> elements)
		{
			using (Transaction tr = new Transaction(doc, "Coordinator Create view"))
			{
				try
				{
					tr.Start();
					using (View3D view = GetView3D())
					{
						view.IsSectionBoxActive = false;
						view.CropBoxActive = false;
						tr.Commit();

						if (uI.ActiveView.Id != view.Id)
							uI.ActiveView = view;
						SetPosition(view);
					}
					app.ActiveUIDocument.Selection.SetElementIds(elements.ToList());
				}
				catch (Exception ex)
				{
					tr.RollBack();
					throw ex;
				}
			}
		}

		private void SetRevitShow(IEnumerable<ElementId> elements)
		{
			uI.ShowElements(elements.ToList());
		}

		#region Section Box

		private void SetPosition(View3D view)
		{
			if (X == 0 && Y == 0 && Z == 0)
			{
				SetElementPointPosition(view);
			}
			else
			{
				SetCentralPointPosition(view);
			}
		}

		private void SetCentralPointPosition(View3D view)
		{
			XYZ centerpoint = GetXYZ();
			SetSectonBox(view, centerpoint);
			ZoomToFit(view.Id, centerpoint);
		}

		private void SetElementPointPosition(View3D view)
		{
			Element el = doc.GetElement(new ElementId(Clash.RevitElement1Id));
			BoundingBoxXYZ box = el.get_BoundingBox(view);
			XYZ vector = (box.Max - box.Min).Normalize();
			XYZ Max = box.Max + vector * Offset;
			XYZ Min = box.Min - vector * Offset;
			SetSectonBox(view, box.Min + (box.Max - box.Min) / 2);
			ZoomToFit(view.Id, Min, Max);
		}


		private void ZoomToFit(ElementId viewId, XYZ centerpoint)
		{
			ZoomToFit(viewId, GetMinPoint(centerpoint, Offset), GetMaxPoint(centerpoint, Offset));
		}

		private void ZoomToFit(ElementId viewId, XYZ min, XYZ max)
		{
			UIView v = uI.GetOpenUIViews().Where(x => x.ViewId == viewId).First();
			v.ZoomAndCenterRectangle(min, max);
		}



		private XYZ GetXYZ()
		{
			return doc.ActiveProjectLocation.GetTransform().OfPoint(new XYZ(ConvertToMetre(X), ConvertToMetre(Y), ConvertToMetre(Z)));
		}


		private void SetSectonBox(View3D view, XYZ centerpoint)
		{
			using (Transaction tr = new Transaction(doc, "Coordinator SetSectionBox"))
			{
				try
				{
					tr.Start();
					view.SetSectionBox(CreateBoundingBox(centerpoint));
					tr.Commit();
				}
				catch (Exception ex)
				{
					tr.RollBack();
					throw ex;
				}
				
			}

		}

		private BoundingBoxXYZ CreateBoundingBox(XYZ centerPoint)
		{
			BoundingBoxXYZ box = new BoundingBoxXYZ()
			{
				Max = GetMaxPoint(centerPoint, Offset),
				Min = GetMinPoint(centerPoint, Offset)
			};
			return box;
		}

		private XYZ GetMaxPoint(XYZ centerPoint, double _offset) =>
			new XYZ(centerPoint.X + _offset, centerPoint.Y + _offset, centerPoint.Z + _offset);

		private XYZ GetMinPoint(XYZ centerPoint, double _offset) =>
			new XYZ(centerPoint.X - _offset, centerPoint.Y - _offset, centerPoint.Z - _offset);

		private double ConvertToMetre(double val)
		{
			try
			{
				return val * 1000 / Converter.Fut;
			}
			catch
			{
				return Converter.ToDouble(val.ToString()) * 1000 / Converter.Fut;
			}
		}

		#endregion Section Box

		#region Selection

		private IEnumerable<ElementId> GetElementsByIds(IEnumerable<int> list)
		{
			foreach (int Id in list.Where(x => x > 0))
			{
				Element el = doc.GetElement(new ElementId(Id));
				if (el != null)
					yield return el.Id;
			}
		}

		#endregion Selection

		#region View3D
		private View3D GetView3D()
		{
			try
			{
				string viewname = $"3D - Model Checker {doc.Application.Username}";
				using (var coll = new FilteredElementCollector(doc))
				{
					if (!(coll.OfClass(typeof(View3D)).Where(x => x.Name == viewname).FirstOrDefault() is View3D view3D))
						view3D = CreateView3D(viewname);
					return view3D;
				}

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private View3D CreateView3D(string viewname)
		{
			try
			{
				View3D view3D = View3D.CreateIsometric(doc, Get3DViewTypeId());
				view3D.Name = viewname;
				return view3D;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private ElementId Get3DViewTypeId()
		{
			using (var coll = new FilteredElementCollector(doc))
			{
				return (from elem in coll.OfClass(typeof(ViewFamilyType))
						let type = elem as ViewFamilyType
						where type.ViewFamily == ViewFamily.ThreeDimensional
						select type).First().Id;
			}
		}

		#endregion View3D
		

		

		public string GetName() => ToString();

	}
}

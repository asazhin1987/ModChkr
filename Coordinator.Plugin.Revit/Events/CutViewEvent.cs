using System;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coordinator.DTO;
using Autodesk.Revit.DB;
using Bimacad.Sys;


namespace Coordinator.Plugin.Revit.Events
{
	public class CutViewEvent : IExternalEventHandler
	{
		public ClashDTO Clash { get; set; }

		private bool Undercut3D { get { return Clash.UndercutView3d; } }
		public double Offset { get { return Clash.Offset; } }
		public double X { get { return Clash.X; } }
		public double Y { get { return Clash.Y; } }
		public double Z { get { return Clash.Z; } }

		public void Execute(UIApplication app)
		{
			UIDocument uI = app.ActiveUIDocument;
			Document doc = uI.Document;
			View view = uI.ActiveView;
			View3D view3D;

			try
			{
				if (view is View3D)
					view3D = (View3D)view;
				else
				{
					view3D = GetView3D(doc);
					uI.ActiveView = view3D;
				}
				if (Undercut3D)
				{
					SetSectonBox(view3D, doc);
				}
				var aaa = uI.Selection.GetElementIds();
				uI.ShowElements(uI.Selection.GetElementIds());
				
				ZoomToFit(view3D.Id, uI);
				
			}
			catch (Exception ex)
			{
				TaskDialog.Show("Coordinator", ex.Message);
			}
		}



		private View3D GetView3D(Document doc)
		{
			try
			{
				string viewname = $"3D - Model Checker {doc.Application.Username}";
				using (var coll = new FilteredElementCollector(doc))
				{
					if ((coll.OfClass(typeof(View3D)).Where(x => x.Name == viewname).FirstOrDefault() is View3D view3D))
						return view3D;
					else
						throw new Exception("Не удалось найти подходящий 3D вид");
				}

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private void ZoomToFit(ElementId viewId, UIDocument uI)
		{
			uI.GetOpenUIViews().Where(x => x.ViewId == viewId).First().ZoomToFit();
		}

		private XYZ GetXYZ(Document doc)
		{
			return doc.ActiveProjectLocation.GetTransform().OfPoint(new XYZ(ConvertToMetre(X), ConvertToMetre(Y), ConvertToMetre(Z)));
		}


		private void SetSectonBox(View3D view, Document doc)
		{
			using (Transaction tr = new Transaction(doc, "Coordinator SetSectionBox"))
			{
				tr.Start();
				view.SetSectionBox(CreateBoundingBox(GetXYZ(doc)));
				//view.c
				tr.Commit();
			}
				
		}

		private BoundingBoxXYZ CreateBoundingBox(XYZ centerPoint)
		{
			BoundingBoxXYZ box = new BoundingBoxXYZ()
			{
				Max = new XYZ(centerPoint.X + Offset, centerPoint.Y + Offset, centerPoint.Z + Offset),
				Min = new XYZ(centerPoint.X - Offset, centerPoint.Y - Offset, centerPoint.Z - Offset)
			};
			return box;
		}

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


		
		public string GetName() => ToString();
	}
}

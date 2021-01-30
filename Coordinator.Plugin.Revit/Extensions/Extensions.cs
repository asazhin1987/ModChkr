//using System;
//using Autodesk.Revit.UI;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Coordinator.DTO;
//using Autodesk.Revit.DB;
//using System.DirectoryServices.AccountManagement;
//using Bimacad.Sys;

//namespace Coordinator.Plugin.Revit
//{
//	public static class Extensions
//	{
//		public static XYZ GetXYZ(Document doc, double X, double Y, double Z)
//		{
//			return doc.ActiveProjectLocation.GetTransform().OfPoint(new XYZ(ConvertToMetre(X), ConvertToMetre(Y), ConvertToMetre(Z)));
//		}

//		public static double ConvertToMetre(double val)
//		{
//			try
//			{
//				return val * 1000 / Converter.Fut;
//			}
//			catch
//			{
//				return Converter.ToDouble(val.ToString()) * 1000 / Converter.Fut;
//			}
//		}

//		public static void SetSectonBox(View3D view, Document doc, UIDocument uI, ClashDTO clash)
//		{
//			view.SetSectionBox(CreateBoundingBox(GetXYZ(doc, clash.X, clash.Y, clash.Z), clash.Offset));
//			ZoomInClashReport(view.Id, uI, clash);
//		}

//		public static void ZoomInClashReport(ElementId viewId, UIDocument uI, ClashDTO clash)
//		{
//			var view = uI.GetOpenUIViews().Where(x => x.ViewId == viewId).First();
//			XYZ centerPoint = GetXYZ(uI.Document, clash.X, clash.Y, clash.Z);
//			var locoffset = clash.Offset > 1 ? clash.Offset : 10;
//			var Max = new XYZ(centerPoint.X + locoffset, centerPoint.Y + locoffset, centerPoint.Z + locoffset);
//			var Min = new XYZ(centerPoint.X - locoffset, centerPoint.Y - locoffset, centerPoint.Z - locoffset);
//			view.ZoomAndCenterRectangle(Min, Max);
//			view.Zoom(0.8);
//		}

//		public static BoundingBoxXYZ CreateBoundingBox(XYZ centerPoint, double Offset)
//		{
//			BoundingBoxXYZ box = new BoundingBoxXYZ()
//			{
//				Max = new XYZ(centerPoint.X + Offset, centerPoint.Y + Offset, centerPoint.Z + Offset),
//				Min = new XYZ(centerPoint.X - Offset, centerPoint.Y - Offset, centerPoint.Z - Offset)
//			};
//			return box;
//		}
//	}
//}

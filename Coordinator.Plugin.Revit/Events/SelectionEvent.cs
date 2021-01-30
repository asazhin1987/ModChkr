using System;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;
using Coordinator.DTO;
using Autodesk.Revit.DB;



namespace Coordinator.Plugin.Revit.Events
{
	public class SelectionEvent : IExternalEventHandler
	{
		public ClashDTO Clash { get; set; }

		public void Execute(UIApplication app)
		{
			IEnumerable<ElementId> elements = GetElementsByIds(new List<int> { Clash.RevitElement1Id, Clash.RevitElement2Id }, app.ActiveUIDocument.Document);
			if (elements.Count() == 0)
				throw new Exception("Элементы не найдены в модели");

			app.ActiveUIDocument.Selection.SetElementIds(elements.ToList());

		}

		private IEnumerable<ElementId> GetElementsByIds(IEnumerable<int> list, Document doc)
		{
			foreach (int Id in list)
			{
				Element el = doc.GetElement(new ElementId(Id));
				if (el != null)
					yield return el.Id;
			}
		}

		public string GetName() => ToString();
	}
}

using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using Coordinator.Forms.Infrastructure;
using Coordinator.DTO.Infrastructure;
using Coordinator.Plugin.Revit.Events;

namespace Coordinator.Plugin.Revit.Commands
{
	[Transaction(TransactionMode.Manual)]
	[Regeneration(RegenerationOption.Manual)]

	public class ShowHTMLReportCommand : IExternalCommand
	{
		private ShowElementEvent showEvent;
		private ExternalEvent showExternalEvent;

		public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
		{
			try
			{
				showEvent = new ShowElementEvent();
				showExternalEvent = ExternalEvent.Create(showEvent);

				HTMLReportFactory factory = new HTMLReportFactory();
				factory.ShowElementEvent += ShowElement;
				factory.CutViewEvent += ShowElement;
				factory.Create();
				return Result.Succeeded;
			}
			catch (Exception ex)
			{
				TaskDialog.Show("Coordinator", ex.Message);
				return Result.Failed;
			}
		}

		private void ShowElement(object sender, CoordinatorEventArgs e)
		{
			try
			{
				showEvent.Clash = e.Clash;
				showExternalEvent.Raise();
			}
			catch (Exception ex)
			{
				TaskDialog.Show("Coordinator", ex.Message);
			}
		}
	}
}

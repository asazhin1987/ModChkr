using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using Coordinator.Forms.Infrastructure;

namespace Coordinator.Plugin.Revit.Commands
{
	[Transaction(TransactionMode.Manual)]
	[Regeneration(RegenerationOption.Manual)]

	public class SettingsCommand : IExternalCommand
	{
		public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
		{
			try
			{
				new SettingFactory().Create();
				return Result.Succeeded;
			}
			catch (Exception ex)
			{
				TaskDialog.Show("Coordinator", ex.Message);
				return Result.Failed;
			}
		}
	}
}

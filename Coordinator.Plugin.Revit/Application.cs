using System;
using System.IO;
using Autodesk.Revit.UI;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace Coordinator.Plugin.Revit
{
	public class Application : IExternalApplication
	{
		public Result OnStartup(UIControlledApplication app)
		{
			try
			{
				string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
				//create ribbon
				BitmapImage imgchecker = new BitmapImage(new Uri($"pack://application:,,,/{assemblyName};component/Content/checker.png"));
				BitmapImage imgsetting = new BitmapImage(new Uri($"pack://application:,,,/{assemblyName};component/Content/settings-16.png"));
				BitmapImage imghtml = new BitmapImage(new Uri($"pack://application:,,,/{assemblyName};component/Content/html-16.png"));
				BitmapImage imgxml = new BitmapImage(new Uri($"pack://application:,,,/{assemblyName};component/Content/code-16.png"));

				string location = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), $"{assemblyName}.dll");
				RibbonPanel ribbonPanel = CreateRibbonPanel(app, title: "BIMACAD Model Coordinator");
				//TaskDialog.Show("BEER", "BEER");
				//var res = new Uri($"pack://application:,,,/WpfStyles;component/Core.xaml");
				Assembly.LoadFrom(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), $"WpfStyles.dll"));


				//clash table
				ribbonPanel.AddItem(new PushButtonData("ClashBtn", "Clashes Table", location, $"{assemblyName}.Commands.ShowClashTableCommand")
				{
					Image = imgchecker,
					ToolTip = "Show clashes table report",
					LargeImage = imgchecker
				});

				//setting
				ribbonPanel.AddItem(new PushButtonData("SettingsBtn", "Settings", location, $"{assemblyName}.Commands.SettingsCommand")
				{
					Image = imgsetting,
					ToolTip = "",
					LargeImage = imgsetting
				});

				//html
				ribbonPanel.AddItem(new PushButtonData("HtmlBtn", "HTML Report", location, $"{assemblyName}.Commands.ShowHTMLReportCommand")
				{
					Image = imghtml,
					ToolTip = "",
					LargeImage = imghtml
				});

				//xml
				ribbonPanel.AddItem(new PushButtonData("XmlBtn", "XML Report", location, $"{assemblyName}.Commands.ShowXMLReportCommand")
				{
					Image = imgxml,
					ToolTip = "",
					LargeImage = imgxml
				});
				return Result.Succeeded;
			}
			catch(Exception ex)
			{
				TaskDialog.Show("Coordinator", ex.Message);
				return Result.Failed;
			}
			
		}

		private RibbonPanel CreateRibbonPanel(UIControlledApplication app, string tabName = "BIMACAD", string title = "")
		{
			try
			{
				app.CreateRibbonTab(tabName);
			}
			catch (Exception) { }

			return app.CreateRibbonPanel(tabName, title);
		}


		public Result OnShutdown(UIControlledApplication application) => 
			Result.Succeeded;
		
		
	}
}

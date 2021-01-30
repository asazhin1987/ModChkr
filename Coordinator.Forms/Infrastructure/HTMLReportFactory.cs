using Coordinator.DTO.Infrastructure;
using Coordinator.Forms.Applications;
using Coordinator.Forms.Xaml;
using Microsoft.Win32;
using System;


namespace Coordinator.Forms.Infrastructure
{
	public class HTMLReportFactory : ClientFactory
	{
		public EventHandler<CoordinatorEventArgs> ShowElementEvent;
		public EventHandler<CoordinatorEventArgs> CutViewEvent;
		HTMLReportApplication app;

		public HTMLReportFactory() : base(true, true) { }

		public void Create()
		{
			if (IsLicensed)
			{
				string file = GetHTMLFile();
				if (file != string.Empty)
				{
					app = new HTMLReportApplication(file)
					{
						Cut3dView = true,
						CutValue = 1,
					};
					app.ShowElement += ShowElement;
					app.CutView += CutView;

					HTMLViewer form = new HTMLViewer(app)
					{
						//Topmost = true
					};
					form.Show();
				}
			}
				
		}


		private void ShowElement(object sender, CoordinatorEventArgs e)
		{
			ShowElementEvent?.Invoke(sender, e);
		}

		private void CutView(object sender, CoordinatorEventArgs e)
		{
			CutViewEvent?.Invoke(sender, e);
		}

		private string GetHTMLFile()
		{
			OpenFileDialog dialog = new OpenFileDialog()
			{
				Filter = "html files|*.html",
				Multiselect = false
			};
			if(dialog.ShowDialog() == true)
				return dialog.FileName;
			return string.Empty;
		}

	}
}

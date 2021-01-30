using Coordinator.Forms.Applications;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System;

namespace Coordinator.Forms.Xaml
{

	public partial class HTMLViewer : Window
	{
		HTMLReportApplication App;
		public HTMLViewer(HTMLReportApplication app)
		{
			InitializeComponent();
			DataContext = app;
			App = app;
		}

		private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
		{
			Regex regex = new Regex("[^0-9]+");
			e.Handled = regex.IsMatch(e.Text);
		}

		private void IdBox_PreviewKeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				try
				{
					if (int.TryParse(((TextBox)sender).Text, out int id))
					{
						App.ElementId = id;
						App.ShowElenemtCommand.Execute(null);
					}

				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}

			}
		}
	}
}

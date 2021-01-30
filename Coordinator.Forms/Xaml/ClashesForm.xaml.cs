using Coordinator.Forms.Applications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Coordinator.Forms.Xaml
{
	/// <summary>
	/// Логика взаимодействия для ClashesForm.xaml
	/// </summary>
	public partial class ClashesForm : Window
	{
		ClashesApplication App { get; }
		public ClashesForm(ClashesApplication app)
		{

			InitializeComponent();
			DataContext = app;
			App = app;
		}

		private void TextBox_PreviewKeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				App.ClashFilter.SearchId = ((TextBox)sender).Text;
				App.SetFilterCommand.Execute(null);
			}
				
		}

	}
}

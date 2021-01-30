using Coordinator.Forms.Applications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
	/// Логика взаимодействия для FilterForm.xaml
	/// </summary>
	public partial class FilterForm : Window
	{
		private FilterApplication App { get; set; }

		public FilterForm(FilterApplication app)
		{
			InitializeComponent();
			DataContext = app;
			App = app;
			ConstructionsTab.Content = new NamedObjectTable(app.ConstructionApplication);
			ChecksTab.Content = new NamedObjectTable(app.CheckApplication);
			ModelsTab.Content = new NamedObjectTable(app.ModelApplication);
			StatusesTab.Content = new NamedObjectTable(app.StatusApplication);
		}

		private void CancelBtn_Click(object sender, RoutedEventArgs e)
		{
			App.CancelCommand.Execute(null);
			Close();
		}

		private void OkBtn_Click(object sender, RoutedEventArgs e)
		{
			App.OkCommand.Execute(null);
			Close();
		}

		private void BreakBtn_Click(object sender, RoutedEventArgs e)
		{
			App.BreakCommand.Execute(null);
			Close();
		}

	}
}

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
using Coordinator.Forms.Applications;

namespace Coordinator.Forms.Xaml
{
	/// <summary>
	/// Логика взаимодействия для StatusForm.xaml
	/// </summary>
	public partial class StatusForm : Window
	{
		StatusApplication App;
		public StatusForm(StatusApplication app)
		{
			InitializeComponent();
			DataContext = app;
			App = app;
		}

		private void ConcelButton_Click(object sender, RoutedEventArgs e)
		{
			App.CancelCommand.Execute(null);
			Close();
		}

		private void OkButton_Click(object sender, RoutedEventArgs e)
		{
			App.OkCommand.Execute(null);
			Close();
		}
	}
}

using Coordinator.Forms.Applications;
using Coordinator.Forms.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
	/// Логика взаимодействия для NamedObjectTable.xaml
	/// </summary>
	public partial class NamedObjectTable : UserControl
	{
		private readonly NamedObjectApplication App;

		public NamedObjectTable(NamedObjectApplication app)
		{
			InitializeComponent();
			DataContext = app;
			App = app;
		}

		private void CheckBox_Checked(object sender, RoutedEventArgs e)
		{
			App.CheckAll = true;
		}

		private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
		{
			App.CheckAll = false;
		}

		private void TextBox_KeyUp(object sender, KeyEventArgs e)
		{
			string txt = ((TextBox)sender).Text;
			ICollectionView defaultView = CollectionViewSource.GetDefaultView(Table.ItemsSource);

			if (!string.IsNullOrEmpty(txt))
			{
				defaultView.Filter = delegate (object o)
				{
					return (new Regex(txt, RegexOptions.IgnoreCase)).IsMatch(((NamedObjectViewModel)o).Name);
				};
			}
			else
			{
				defaultView.Filter = ((object o) => true);
			}
		}
	}
}

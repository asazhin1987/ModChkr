using Coordinator.DTO.Infrastructure;
using Coordinator.Forms.Infrastructure;
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

namespace Coordinator.Test
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		//EventHandler<CoordinatorEventArgs> ShowElementEvent;

		public MainWindow()
		{
			InitializeComponent();
			 
		}

		//clashes
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				ClashReportFactory factory = new ClashReportFactory();
				factory.ShowElementEvent += ShowClash;
				factory.CutViewEvent += CutView;
				factory.Create();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}


		//settings
		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			try
			{
				new SettingFactory().Create();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			
		}

		//html
		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			try
			{
				HTMLReportFactory factory = new HTMLReportFactory();
				factory.ShowElementEvent += ShowClash;
				factory.CutViewEvent += CutView;
				factory.Create();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			
		}

		//xml
		private void Button_Click_3(object sender, RoutedEventArgs e)
		{
			try
			{
				XMLReportFactory factory = new XMLReportFactory();
				factory.ShowElementEvent += ShowClash;
				factory.CutViewEvent += CutView;
				factory.Create();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

			
		}

		private void ShowHTMLElement(object sender, CoordinatorEventArgs e)
		{
			MessageBox.Show(e.Clash.RevitElement1Id.ToString());
		}

		private void ShowClash(object sender, CoordinatorEventArgs e)
		{
			MessageBox.Show($"Show Clash: {e.Clash.RevitElement1Id.ToString()}");
		}

		

		private void CutView(object sender, CoordinatorEventArgs e)
		{
			MessageBox.Show($"CutView: {e.Clash.Offset}");
		}
	}
}


using System.Windows;
using System.Windows.Controls;
using ModelChecker.Plugin.Model;

namespace ModelChecker.Plugin.Forms
{

	public partial class WriteClashesMasterWIndow : Window
	{
		private readonly ClashMasterViewModel _model;
		public WriteClashesMasterWIndow(ClashMasterViewModel model)
		{
			InitializeComponent();
			DataContext = model;
			_model = model;
		}

		//private void CheckBox_Click(object sender, RoutedEventArgs e)
		//{
			
		//	//var ccc = sender as CheckBox;
		//	//string sss = "";
		//}
	}
}

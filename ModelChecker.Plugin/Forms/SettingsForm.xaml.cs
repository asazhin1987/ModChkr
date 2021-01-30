using System.Windows;
using ModelChecker.Plugin.Model;

namespace ModelChecker.Plugin.Forms
{

	public partial class SettingsForm : Window
	{
		private readonly SettingsViewModel _model;
		public SettingsForm(SettingsViewModel model)
		{
			InitializeComponent();
			DataContext = model;
			_model = model;
		}
	}
}

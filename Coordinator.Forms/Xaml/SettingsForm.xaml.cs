using Coordinator.Forms.Model;
using System.Windows;

namespace Coordinator.Forms.Xaml
{
	public partial class SettingsForm : Window
	{
		public SettingsForm(SettingsViewModel model)
		{
			InitializeComponent();
			DataContext = model;
		}
	}
}

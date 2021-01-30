using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using winapp = System.Windows;
using Bimacad.Sys;
using Coordinator.Forms.Model;
using Coordinator.Forms.Xaml;
using Coordinator.ISRC;
using Coordinator.SRC;

namespace Coordinator.Forms.Infrastructure
{
	public class SettingFactory : ClientFactory
	{
		SettingsViewModel settingModel;
		public SettingFactory() : base() { }
		public SettingFactory(bool checkConnection) : base(checkConnection, false) { }

		public void Create()
		{
			try
			{

				settingModel = new SettingsViewModel()
				{
					Host = HostName
				};
				settingModel.SaveHost += SettingModel_SaveHost;
				settingModel.CheckHost += SettingModel_CheckHost;
				SettingsForm sf = new SettingsForm(settingModel);

				sf.ShowDialog();
			}
			catch (Exception ex)
			{
				Inform(ex.Message);
			}
		}

		private void SettingModel_CheckHost(object sender, EventArgs e)
		{
			try
			{
				string _host = ClaerStrinng(settingModel.Host);
				var _src = new CoordinatorService(_host);

				if (_src.CheckConnection())
				{
					Inform("Соединение установлено");
					_src = null;
				}
			}
			catch
			{
				Inform("Соединение не установлено, проверьте правильность написания адреса хоста или натройки сети");
			}
		}

		private void SettingModel_SaveHost(object sender, EventArgs e)
		{
			try
			{
				BTextWriter.WriteCurrentFile(ClaerStrinng(settingModel.Host), "host.txt");
				Inform("Данные сохранены");
			}

			catch (Exception ex)
			{
				Inform(ex.Message);
			}
		}
	}
}

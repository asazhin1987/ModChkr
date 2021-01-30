using Bimacad.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using winapp = System.Windows;
using Coordinator.ISRC;
using Coordinator.SRC;

namespace Coordinator.Forms.Infrastructure
{
	public class ClientFactory
	{
		internal ICoordinatorService src;
		internal string HostName { get { return GetHostName(); } }
		internal bool IsLicensed { get; } 
		public ClientFactory()
		{

		}

		public ClientFactory(bool checkConnection, bool checklic)
		{
			if (checkConnection)
			{
				try
				{
					src = new CoordinatorService(HostName);
					bool b = src.CheckConnection();
				}
				catch (Exception)
				{
					new SettingFactory().Create();
				}
			}
			if (checklic)
				IsLicensed = CheckLicense();
		}


		private string GetHostName() =>
			ClaerStrinng(BTextWriter.ReadCurrentFile("host.txt"));

		internal string ClaerStrinng(string _str) =>
		_str.Replace("\r", "").Replace("\n", "").Trim();

		internal void Inform(string msg)
		{
			winapp.MessageBox.Show(msg);
		}


		internal bool CheckLicense()
		{
			try
			{
				return src.TakeLicense(Environment.MachineName, Environment.UserName);
			}
			catch (WrongKeyException)
			{
				Inform("Срок действиея лицензии истек или введен неверный ключ");
				return false;
			}
			catch (NullKeyException)
			{
				Inform("Отсутствует лицензия");
				return false;
			}
			catch (ZeroQntException)
			{
				Inform("Число лицензий закончилось");
				return false;
			}
			catch (Exception ex)
			{
				Inform(ex.Message);
				return false;
			}
		}
	}
}

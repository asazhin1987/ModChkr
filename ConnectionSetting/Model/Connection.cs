using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionSetting.Model
{
	public class Connection : INotifyPropertyChanged
	{
		internal event EventHandler<EventArgs> OnConnectionChanged;

		private bool localHost;
		public bool LocalHost
		{
			get { return localHost; }
			set
			{
				localHost = value;
				if (value == true)
				{
					LocalDB = false;
					SqlSerevr = false;
				}
				OnPropertyChanged();
			}
		}

		private bool localDb;
		public bool LocalDB
		{
			get { return localDb; }
			set
			{
				localDb = value;
				if (value == true)
				{
					LocalHost = false;
					SqlSerevr = false;
				}
				OnPropertyChanged();
			}
		}

		private bool sclServer;
		public bool SqlSerevr
		{
			get { return sclServer; }
			set
			{
				sclServer = value;
				if (value == true)
				{
					LocalDB = false;
					LocalHost = false;
				}
				OnPropertyChanged();
			}
		}

		private string serverName;
		public string ServerName { get { return serverName; } set { serverName = value; OnPropertyChanged(); } }

		private string connectionName;
		public string ConnectionName { get { return connectionName; } set { connectionName = value; OnPropertyChanged(); } }

		private string userName;
		public string UserName { get { return userName; } set { userName = value; OnPropertyChanged(); } }

		private string password;
		public string Password { get { return password; } set { password = value; OnPropertyChanged(); } }

		private string providerName;
		public string ProviderName { get { return providerName; } set { providerName = value; OnPropertyChanged(); } }

		private string dataBaseName;
		public string DataBaseName { get { return dataBaseName; } set { dataBaseName = value; OnPropertyChanged(); } }

		private string dataBasePath;
		public string DataBasePath { get { return dataBasePath; } set { dataBasePath = value; OnPropertyChanged(); } }


		internal string GetConnectionString()
		{
			if (LocalHost)
				return GetLocalHostConnStr();
			else if (LocalDB)
				return GetLocalDBConnStr();
			else
				return GetRemotelyServerConnStr();
		}

		private string GetLocalHostConnStr() =>
			$"Server=localhost\\SQLEXPRESS;Database={DataBaseName};Trusted_Connection=True;";

		private string GetLocalDBConnStr() =>
			$"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename='{DataBasePath}';Integrated Security=True";

		private string GetRemotelyServerConnStr() =>
			$"Data Source={ServerName}; Initial Catalog={DataBaseName}; Persist Security Info=True;User ID={UserName};Password={Password}";


		private void ChangedConnStr()
		{
			OnConnectionChanged?.Invoke(this, new EventArgs());
		}



		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName]string prop = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
			ChangedConnStr();
		}
	}
}

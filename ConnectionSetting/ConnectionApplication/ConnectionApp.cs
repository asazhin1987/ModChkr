using ConnectionSetting.Commands;
using ConnectionSetting.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace ConnectionSetting.ConnectionApplication
{
	public class ConnectionApp : INotifyPropertyChanged
	{
		private Connection connection;
		public Connection Connection { get { return connection; } set { connection = value; OnPropertyChanged(); } }

		private string resultMessage;
		public string ResultMessage { get { return resultMessage; } set { resultMessage = value; OnPropertyChanged(); } }

		private string filePath;
		public string FilePath { get { return filePath; } set { filePath = value; OnPropertyChanged(); } }

		private string connStr;
		public string ConnectionStr { get { return connStr; } set { connStr = value; OnPropertyChanged(); } }

		private bool hasConfig;
		public bool HasConfig { get { return hasConfig; } set { hasConfig = value; OnPropertyChanged(); } }

		#region Visible

		private bool serverNameVisible;
		private bool userNameVisible;
		private bool passwordVisible;
		private bool databaseNameisible;
		private bool databasePathVisible;
		private bool connectionNameVisible;

		public bool ServerNameVisible { get { return serverNameVisible; } set { serverNameVisible = value; OnPropertyChanged(); } }
		public bool UserNameVisible { get { return userNameVisible; } set { userNameVisible = value; OnPropertyChanged(); } }
		public bool PasswordVisible { get { return passwordVisible; } set { passwordVisible = value; OnPropertyChanged(); } }
		public bool DatabaseNameVisible { get { return databaseNameisible; } set { databaseNameisible = value; OnPropertyChanged(); } }
		public bool DatabasePathVisible { get { return databasePathVisible; } set { databasePathVisible = value; OnPropertyChanged(); } }
		public bool ConnectionNameVisible { get { return connectionNameVisible; } set { connectionNameVisible = value; OnPropertyChanged(); } }


		#endregion Visible

		public ConnectionApp()
		{
			HasConfig = false;
			FilePath = "Выберите файл конфигурации";
			//FindFirstConfig();
		}

		private void Connection_OnConnectionChanged(object sender, EventArgs e)
		{
			UpdateUI();
		}

		void UpdateUI()
		{
			ServerNameVisible = Connection.SqlSerevr;
			UserNameVisible = Connection.SqlSerevr;
			PasswordVisible = Connection.SqlSerevr;
			DatabaseNameVisible = !Connection.LocalDB;
			DatabasePathVisible = Connection.LocalDB;
			ConnectionNameVisible = Connection.ConnectionName == string.Empty;
			ConnectionStr = Connection.GetConnectionString();
		}



		#region
		//private void ReadXml(string path)
		//{
		//	try
		//	{

		//		XmlDocument doc = ReadFile(path);
		//		var conn = CereateConnectionInstance(doc);
		//		HasConfig = true;
		//		ResultMessage = "Файл конфигурации успешно прочитан.";
		//		FilePath = path;
		//	}
		//	catch (Exception ex)
		//	{
		//		ResultMessage = ex.Message;
		//		HasConfig = false;
		//	}
		//}

		//private XmlDocument ReadFile(string path)
		//{
		//	if (path == string.Empty)
		//		throw new Exception("Не выбран файл конфигурации");

		//	else if (File.Exists(path))
		//	{
		//		try
		//		{
		//			XmlDocument doc = new XmlDocument();
		//			doc.LoadXml(path);

		//			return doc;
		//		}
		//		catch (XmlException )
		//		{
		//			throw new Exception("Ошибка чтения XML");
		//		}
		//		catch (Exception ex)
		//		{
		//			throw ex;
		//		}
		//	}
		//	else
		//	{
		//		throw new FileNotFoundException($"Ошибка чтения файла. Файл не найден. Path: {path}");
		//	}
		//}

		//private void FindFirstConfig()
		//{
		//	string firstIteration = FindFirstConfig("*.exe.config");
		//	string secondIteration = FindFirstConfig("*.config");
		//	if (firstIteration != string.Empty)
		//		ReadXml(firstIteration);
		//	else if (secondIteration != string.Empty)
		//		ReadXml(secondIteration);
		//	else
		//	{
		//		ReadXml(FindConfigFromWindow());
		//	}

		//}

		//private string FindFirstConfig(string mask)
		//{
		//	var files = Directory.GetFiles(Directory.GetCurrentDirectory(), mask, SearchOption.TopDirectoryOnly);
		//	if (files.Count() > 0)
		//		return files.First();
		//	else
		//		return string.Empty;
		//}

		//private string FindConfigFromWindow()
		//{
		//	OpenFileDialog dialog = new OpenFileDialog()
		//	{
		//		InitialDirectory = Directory.GetCurrentDirectory(),
		//		Filter = "config files (*.config)|*.config|All files (*.*)|*.*",
		//		Multiselect = false
		//	};
		//	if (dialog.ShowDialog() == true)
		//	{
		//		return dialog.FileName;
		//	}
		//	else
		//	{
		//		return string.Empty;
		//	}
		//}

		//private Connection CereateConnectionInstance(XmlDocument xml)
		//{
		//	try
		//	{
		//		XmlNode node = xml.SelectSingleNode("configuration/connectionStrings/add");
		//		XmlAttribute name = node.Attributes["name"];
		//		XmlAttribute connstr = node.Attributes["connectionString"];
		//		XmlAttribute provider = node.Attributes["providerName"];

		//		var result = new Connection()
		//		{
		//			ProviderName = provider.Value,
		//			ConnectionName = name.Value,
		//			LocalDB = connstr.Value.Contains("(LocalDB)")
		//		};
		//		result.OnConnectionChanged += Connection_OnConnectionChanged;
		//		return result;

		//	}
		//	catch (NullReferenceException nex)
		//	{
		//		throw nex;
		//	}
		//	catch (Exception ex)
		//	{
		//		throw ex;
		//	}

		//	//Connection.OnConnectionChanged += Connection_OnConnectionChanged;
		//}

		#endregion

		#region private methods
		(bool valid, string message) Validation()
		{
			bool valid;
			string msg = "";
			valid = ValidString(ServerNameVisible, Connection.ServerName) &&
				ValidString(UserNameVisible, Connection.UserName) &&
				ValidString(PasswordVisible, Connection.Password) &&
				ValidString(DatabaseNameVisible, Connection.DataBaseName) &&
				ValidString(DatabasePathVisible, Connection.DataBasePath) &&
				ValidString(ConnectionNameVisible, Connection.ConnectionName);

			if (!valid)
				msg = "Одно или несколько полей не заполнены";

			return (valid, msg);

			bool ValidString(bool b, string s) =>
				(b == true && s != null && s.Length > 0) || b == false;
		}

		private string GetFileByDialog()
		{
			OpenFileDialog dialog = new OpenFileDialog()
			{
				InitialDirectory = FilePath ?? Directory.GetCurrentDirectory(),
				Filter = "config files (*.config)|*.config|All files (*.*)|*.*",
				Multiselect = false
			};
			if (dialog.ShowDialog() == true)
			{
				return dialog.FileName;
			}
			else
			{
				return string.Empty;
			}
		}

		private XmlDocument GetXmlDocument(string file)
		{
			try
			{
				StreamReader reader = new StreamReader
				(
					new FileStream(
						file,
						FileMode.Open,
						FileAccess.Read,
						FileShare.Read)
				);
				XmlDocument doc = new XmlDocument();
				string xmlIn = reader.ReadToEnd();
				reader.Close();
				doc.LoadXml(xmlIn);
				//XmlDocument doc = new XmlDocument();
				//doc.LoadXml(file);

				return doc;
			}
			catch (XmlException)
			{
				throw new Exception("Ошибка чтения XML");
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private Connection GetConnection(XmlDocument doc)
		{
			Connection con = new Connection()
			{
				ProviderName = "System.Data.SqlClient"
			};
			try
			{
				XmlNode connsNode = doc.SelectSingleNode("configuration/connectionStrings");
				if (connsNode == null)
					throw new Exception("Файл конфигураци не содержит узла подключений");
				var firstConn = connsNode.SelectSingleNode("add");
				if (firstConn != null)
				{
					XmlAttribute name = firstConn.Attributes["name"];
					XmlAttribute str = firstConn.Attributes["connectionString"];
					XmlAttribute provider = firstConn.Attributes["providerName"];
					if (name == null || str == null || provider == null)
						throw new Exception("Неверный файл конфигурации");
					string strVal = str.Value;
					IEnumerable<string> connArr = ParseConnStr(strVal);
					con.ConnectionName = name.Value;
					con.LocalDB = Islocaldb(strVal);
					con.LocalHost = IslocalHost(strVal);
					con.SqlSerevr = IsServer(strVal);

					//server name
					if (con.SqlSerevr)
					{
						con.DataBaseName = GetArrayVal(connArr, "Initial Catalog=");
						con.UserName = GetArrayVal(connArr, "User ID=");
						con.Password = GetArrayVal(connArr, "Password=");
					}
					if (con.LocalHost)
					{
						con.DataBaseName = GetArrayVal(connArr, "Database=");
					}
					if (con.LocalDB)
					{
						con.DataBasePath = GetArrayVal(connArr, "AttachDbFilename=").Replace("'", "").Trim(); ;
					}
				}
				else
				{
					con.ConnectionName = null;
				}
				return con;
			}
			catch (Exception ex)
			{
				throw ex;
			}

			string GetArrayVal(IEnumerable<string> str, string mask) => str.Where(x => x.Contains(mask)).First().Replace(mask, "").Trim();
			string[] ParseConnStr(string connstr) => connstr.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
			bool IslocalHost(string en) => en.Contains("localhost");
			bool Islocaldb(string en) => en.Contains("LocalDB");
			bool IsServer(string en) => en.Contains("Initial Catalog");
		}

		

		#endregion private methods

		void UpdateData()
		{
			try
			{
				//get file
				string file = GetFileByDialog();
				if (!file.Equals(string.Empty))
				{
					//read xml
					XmlDocument doc = GetXmlDocument(file);
					//parse connstr
					Connection connection = GetConnection(doc);
					connection.OnConnectionChanged += Connection_OnConnectionChanged;
					//set data
					FilePath = file;
					Connection = connection;
					HasConfig = true;
					ResultMessage = "Файл конфигурации успешно прочитан.";
				}
			}
			catch (Exception ex)
			{
				Connection = new Connection();
				HasConfig = false;
				ResultMessage = ex.Message;
			}
			UpdateUI();
		}

		void SaveData()
		{
			try
			{
				var (b, m) = Validation();
				if (b)
				{
					//read xml
					XmlDocument doc = GetXmlDocument(FilePath);
					//find node
					XmlNode connsNode = doc.SelectSingleNode("configuration/connectionStrings");
					XmlNode firstConn = connsNode.SelectSingleNode("add");
					if (firstConn != null)
						connsNode.RemoveChild(firstConn);
					//create node
					firstConn = doc.CreateElement("add");
					//attributes
					XmlAttribute name = doc.CreateAttribute("name");
					name.Value = Connection.ConnectionName;
					XmlAttribute connstr = doc.CreateAttribute("connectionString");
					connstr.Value = ConnectionStr;
					XmlAttribute provider = doc.CreateAttribute("providerName");
					provider.Value = "System.Data.SqlClient";
					firstConn.Attributes.Append(name);
					firstConn.Attributes.Append(connstr);
					firstConn.Attributes.Append(provider);
					connsNode.AppendChild(firstConn);
					doc.Save(FilePath);
					ResultMessage = "Конфигурация сохранена";
				}
				else
				{
					throw new Exception(m);
				}
				//elem.Attributes.Append(new XmlAttribute()
			}
			catch (Exception ex)
			{
				ResultMessage = ex.Message;
			}
		}

		#region Commands
		private RelayCommand getFilePathCommand;
		private RelayCommand saveConfigCommand;


		public RelayCommand GetFilePathCommand
		{
			get
			{
				return getFilePathCommand ??
					(getFilePathCommand = new RelayCommand(obj =>
					{
						UpdateData();
					}));
			}
		}

		public RelayCommand SaveConfigCommand
		{
			get
			{
				return saveConfigCommand ??
					(saveConfigCommand = new RelayCommand(obj =>
					{
						SaveData();
					}));
			}
		}

		#endregion Commands



		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName]string prop = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelChecker.BLL.Infrastructure;
using System.Configuration;
using System.Runtime.InteropServices;


namespace ModelChecker.Service
{

	public class Program
	{
		[DllImport("kernel32.dll")]
		static extern IntPtr GetConsoleWindow();

		[DllImport("user32.dll")]
		static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		const int SW_HIDE = 0;
		//const int SW_SHOW = 5;

		static async Task Main(string[] args)
		{
			ServiceFactory service = new ServiceFactory(ConfigurationManager.ConnectionStrings["ModelCheckerConnection"].ConnectionString, ConfigurationManager.ConnectionStrings["ModelCheckerConnection"].ProviderName);

			service.OnServiceRuning += ServiceEventLog;
			service.OnServiceRunned += ServiceEventLog;
			service.OnServicesDisposed += ServiceEventLog;
			service.OnServiceDisposed += ServiceEventLog;
			service.OnBeginMigrate += ServiceEventLog;
			service.OnEndMigrate += ServiceEventLog;
			service.OnFailureMigrate += OnServiceFailureLog;
			service.OnServiceFailure += OnServiceFailureLog;
			service.OnServicesFailure += OnServiceFailureLog;
			service.OnScheduleRunned += ServiceEventLog;
			service.OnScheduleFailure += OnServiceFailureLog;

			service.Migrate();
			service.RunServices();
			await service.RunSchedule();
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("Press any key to hidde console");
			Console.ReadLine();
			var handle = GetConsoleWindow();
			ShowWindow(handle, SW_HIDE);
			Console.ReadLine();
			service.Dispose();

			Console.ReadLine();
		}


		private static void ServiceEventLog(object sender, ServiceEventArgs e)
		{
			Console.ForegroundColor = ConsoleColor.White; Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine(e.Message);
		}

		private static void OnServiceFailureLog(object sender, ServiceEventArgs e)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(e.Message);
		}
	}
}

using ModelChecker.BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Bimacad.Sys;
using System.Runtime.InteropServices;
using System.Configuration;

namespace ModelChecker.WindService
{
	public partial class MCService : ServiceBase
	{
		ServiceFactory service;
		EventLog log;

		public MCService()
		{
			InitializeComponent();
		}

		protected override void OnStart(string[] args)
		{
			Run();
			SysLog("Started");
		}

		protected override void OnStop()
		{
			SysLog("Stopped");
			ComponentsDispose();
		}

		protected override void OnPause()
		{
			SysLog("Paused");
			ComponentsDispose();
		}

		protected override void OnContinue()
		{
			
			Run();
			SysLog("Continued");
		}

		private void Run()
		{
			try
			{
				service = new ServiceFactory(ConfigurationManager.ConnectionStrings["ModelCheckerConnection"].ConnectionString, ConfigurationManager.ConnectionStrings["ModelCheckerConnection"].ProviderName);
				log = new EventLog();
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
				Task t = service.RunSchedule();
				t.Wait();
			}
			catch (Exception ex)
			{
				BTextWriter.WriteCurrentFile(ex.Message, "srclog.txt", false);
			}
			
		}

		private void SysLog(string msg)
		{
			try
			{
				string name = "BIMACADModelChecker Service";
				if (!EventLog.SourceExists(name))
				{
					EventLog.CreateEventSource(name, name);
				}
				log.Source = name;
				log.WriteEntry(msg);
			}
			catch { }
		}

		public void ComponentsDispose()
		{
			service.Dispose();
			service = null;
			log.Dispose();
			log = null;
		}

		private void ServiceEventLog(object sender, ServiceEventArgs e)
		{
			BTextWriter.WriteCurrentFile($"{e.Message} \t {DateTime.Now} \r", "srclog.txt", false);
		}

		private void OnServiceFailureLog(object sender, ServiceEventArgs e)
		{
			BTextWriter.WriteCurrentFile($"EXCEPTION: {e.Message} \t {DateTime.Now} \r", "srclog.txt");
		}
	}
}

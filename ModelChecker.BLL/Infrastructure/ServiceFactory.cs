using ModelChecker.BLL.Services;
using ModelChecker.DAL.Interfaces;
using ModelChecker.DAL.Repositories;
using ModelChecker.ISRC;
using Ninject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Migrations;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using Bimacad.Sys;
using Coordinator.ISRC;
using ModelChecker.DAL.EF;

namespace ModelChecker.BLL.Infrastructure
{
	public class ServiceFactory : IDisposable
	{
		private readonly string connectionString;
		private readonly string providerName;
		private Dictionary<string, ServiceHost> ServiceHosts { get; set; }
		private IScheduler scheduler;

		readonly StandardKernel kerner;

		//services
		public event EventHandler<ServiceEventArgs> OnServicesDisposed;
		public event EventHandler<ServiceEventArgs> OnServiceDisposed;
		public event EventHandler<ServiceEventArgs> OnServiceRuning;
		public event EventHandler<ServiceEventArgs> OnServiceRunned;
		public event EventHandler<ServiceEventArgs> OnServicesRunned;
		public event EventHandler<ServiceEventArgs> OnServiceFailure;
		public event EventHandler<ServiceEventArgs> OnServicesFailure;

		//db migrate
		public event EventHandler<ServiceEventArgs> OnBeginMigrate;
		public event EventHandler<ServiceEventArgs> OnEndMigrate;
		public event EventHandler<ServiceEventArgs> OnFailureMigrate;

		//schedule
		public event EventHandler<ServiceEventArgs> OnScheduleRunned;
		public event EventHandler<ServiceEventArgs> OnScheduleFailure;


		public ServiceFactory(string connStr, string providerName)
		{
			connectionString = connStr;
			ServiceHosts = new Dictionary<string, ServiceHost>();
			kerner = new StandardKernel();
			kerner.Bind<IUnitOfWork>().To<EFUnitOfWork>().WithConstructorArgument(connectionString);
			this.providerName = providerName;
		}

		public void Migrate()
		{
			OnBeginMigrate?.Invoke(this, new ServiceEventArgs("Begin Migrate"));
			try
			{
				var config = new DAL.Migrations.Configuration
				{
					TargetDatabase = new System.Data.Entity.Infrastructure.DbConnectionInfo(connectionString, providerName)
				};
				var context = new ModelCheckerContext(connectionString);
				var migrator = new DbMigrator(config, context);
				if (context.Database.Exists())
				{
					migrator.Update();
				}
				OnEndMigrate?.Invoke(this, new ServiceEventArgs("End Migrate success"));
			}
			catch (Exception ex)
			{
				OnFailureMigrate?.Invoke(this, new ServiceEventArgs(ex.Message, false));
			}
		}


		public void RunServices()
		{
			try
			{
				kerner.Bind<IWebService>().To<WebService>().WithConstructorArgument(kerner.Get<IUnitOfWork>());
				kerner.Bind<IClashService>().To<ClashService>().WithConstructorArgument(kerner.Get<IUnitOfWork>());
				kerner.Bind<ICoordinatorService>().To<CoordinatorService>().WithConstructorArgument(kerner.Get<IUnitOfWork>());


				CreateEndpoint(typeof(IWebService), kerner.Get<IWebService>(), "ModelCheckerWebService");
				CreateEndpoint(typeof(IClashService), kerner.Get<IClashService>(), "ModelCheckerClashService");
				CreateEndpoint(typeof(ICoordinatorService), kerner.Get<ICoordinatorService>(), "CoordinatorService");


				OnServicesRunned?.Invoke(this, new ServiceEventArgs("All Services Runned"));
			}
			catch (Exception ex)
			{
				OnServicesFailure?.Invoke(this, new ServiceEventArgs(ex.Message, false));
			}
		}

		private void CreateEndpoint(Type iType, IService inst, string Name)
		{
			try
			{
				OnServiceRuning?.Invoke(inst, new ServiceEventArgs($"Service {Name} Running"));
				ServiceHost Host = new ServiceHost(inst, new Uri($"http://localhost:80/{Name}")) { };

				BasicHttpBinding binding = new BasicHttpBinding() { MaxReceivedMessageSize = 2147483647 };
				Host.AddServiceEndpoint(iType, binding, "");
				Host.Open();
				ServiceHosts.Add(Name, Host);
				OnServiceRunned?.Invoke(inst, new ServiceEventArgs($"Service {Name} Runned"));
			}
			catch (Exception ex)
			{
				OnServiceFailure?.Invoke(inst, new ServiceEventArgs($"Service {Name} - {ex.Message}"));
				throw ex;
			}
		}

		public async Task RunSchedule()
		{
			try
			{

				scheduler = await StdSchedulerFactory.GetDefaultScheduler();
				await scheduler.Start();
				await RunSchedule<Scheduler>("Scheduler", "Schedules");
				
			}
			catch (Exception ex)
			{
				OnScheduleFailure?.Invoke(this, new ServiceEventArgs(ex.Message, false));
			}

		}

		private async Task RunSchedule<T>(string Name, string GroupName, int interval = 1) where T : IJob
		{
			try
			{
				IJobDetail job = JobBuilder.Create<T>()
					.UsingJobData(new JobDataMap
					(new Dictionary<string, IUnitOfWork>() { { "src", kerner.Get<IUnitOfWork>() } })).Build();

				ITrigger trigger = TriggerBuilder.Create()  // создаем триггер
					.WithIdentity(Name, GroupName)     // идентифицируем триггер с именем и группой
					.StartNow()                            // запуск сразу после начала выполнения
					.WithSimpleSchedule(x => x            // настраиваем выполнение действия
						.WithIntervalInMinutes(interval)          // через {} минут
						.RepeatForever())                   // бесконечное повторение
					.Build();                               // создаем триггер

				await scheduler.ScheduleJob(job, trigger);
				OnScheduleRunned?.Invoke(this, new ServiceEventArgs($"{Name} Runned"));
			}
			catch (Exception ex)
			{
				OnScheduleFailure?.Invoke(this, new ServiceEventArgs(ex.Message, false));
			}

		}

		

		//public void CrveateSchedule()
		//{
		//	//kerner.Get<IUnitOfWork>()
		//}

		public void Dispose()
		{
			foreach (var h in ServiceHosts.Keys)
			{
				ServiceHost host = ServiceHosts[h];
				if (host != null && host.State == CommunicationState.Opened)
				{
					host.Close();
					OnServiceDisposed?.Invoke(this, new ServiceEventArgs($"{h} Disposed"));
				}
			}
			ServiceHosts.Clear();
			kerner.Dispose();
			if (!scheduler.IsShutdown)
				scheduler.Shutdown(true);

			OnServicesDisposed?.Invoke(this, new ServiceEventArgs("All Services Disposed"));
		}
	}
}

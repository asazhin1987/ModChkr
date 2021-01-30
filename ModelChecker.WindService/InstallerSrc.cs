using System.ComponentModel;
using System.ServiceProcess;
using System.Configuration.Install;

namespace ModelChecker.WindService
{
	[RunInstaller(true)]
	public partial class InstallerSrc : Installer
	{
		ServiceInstaller serviceInstaller;
		ServiceProcessInstaller processInstaller;

		public InstallerSrc()
		{
			InitializeComponent();
			serviceInstaller = new ServiceInstaller();
			processInstaller = new ServiceProcessInstaller();

			processInstaller.Account = ServiceAccount.LocalSystem;
			serviceInstaller.StartType = ServiceStartMode.Automatic;
			serviceInstaller.ServiceName = "BIMACAD ModelCheckerService";
			serviceInstaller.DisplayName = "BIMACAD ModelChecker Service";
			Installers.Add(processInstaller);
			Installers.Add(serviceInstaller);
		}
	}
}


    [RunInstaller(true)]
    public partial class GuardServiceInstaller : Installer
    {
        private ServiceProcessInstaller process;
        private ServiceInstaller service;
        ServiceController controller;
        private const string ServiceName = "yourservicename";

        public GuardServiceInstaller()
        {
            InitializeComponent();

            process = new ServiceProcessInstaller();
            process.Account = ServiceAccount.LocalSystem;

            service = new ServiceInstaller();
            service.ServiceName = ServiceName;
            service.StartType = ServiceStartMode.Automatic;
            service.Description = ServiceName;

            controller = new ServiceController();
            controller.ServiceName = ServiceName;
            controller.MachineName = ".";

            Installers.Add(process);
            Installers.Add(service);
        }

        private void WCFWindowsService_AfterInstall(object sender, InstallEventArgs e)
        {
            try
            {
                if (controller.Status != ServiceControllerStatus.Running)
                    controller.Start();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(ServiceName, "Failed to start the service: " + ex.Message);
            }
        }

        private void WCFWindowsService_BeforeUninstall(object sender, InstallEventArgs e)
        {
            if (controller.Status == ServiceControllerStatus.Running)
            {
                controller.Stop();
            }
        }

        private void WCFWindowsService_BeforeRollback(object sender, InstallEventArgs e)
        {
            if (controller.Status == ServiceControllerStatus.Running)
            {
                controller.Stop();
            }
        }
    }


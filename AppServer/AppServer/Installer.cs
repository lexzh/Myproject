using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;


namespace AppServer
{
    [RunInstaller(true)]
    public partial class Installer : System.Configuration.Install.Installer
    {
        public Installer()
        {
            ServiceInstaller installer = new ServiceInstaller
            {
                Description = "业务数据处理服务",
                DisplayName = "AppServer",
                ServiceName = "AppServer",
                StartType = ServiceStartMode.Automatic
            };
            ServiceProcessInstaller installer2 = new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalSystem
            };
            base.Installers.Add(installer);
            base.Installers.Add(installer2);

            InitializeComponent();
        }
    }
}

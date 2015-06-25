using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;

namespace TimerServer
{
    [RunInstaller(true)]
    public partial class Installer : System.Configuration.Install.Installer
    {
        public Installer()
        {
            ServiceInstaller serviceInstaller = new ServiceInstaller()
            {
                Description = "定时处理服务",
                DisplayName = "AppTimerServer",
                ServiceName = "AppTimerServer",
                StartType = ServiceStartMode.Automatic
            };
            ServiceProcessInstaller serviceProcessInstaller = new ServiceProcessInstaller()
            {
                Account = ServiceAccount.LocalSystem
            };
            base.Installers.Add(serviceInstaller);
            base.Installers.Add(serviceProcessInstaller);
            InitializeComponent();
        }
    }
}

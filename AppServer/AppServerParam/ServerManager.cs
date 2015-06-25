namespace AppServerParam
{
    using System;
    using System.ServiceProcess;

    public class ServerManager
    {
        private ServiceController serviceController_0;
        private string string_0 = string.Empty;

        public ServerManager(string string_1)
        {
            this.ServerName = string_1;
            this.ServiceControlObject = this.GetServicObject();
        }

        public ServiceController GetServicObject()
        {
            return new ServiceController(this.ServerName);
        }

        public void ReStarService()
        {
            this.StopService();
            this.StarService();
        }

        public void StarService()
        {
            if (this.StopState)
            {
                this.ServiceControlObject.Start();
                this.ServiceControlObject.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(240.0));
            }
        }

        public void StopService()
        {
            if (this.RuningState)
            {
                this.ServiceControlObject.Stop();
                this.ServiceControlObject.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(120.0));
            }
        }

        public bool IsExistService
        {
            get
            {
                if (!string.IsNullOrEmpty(this.ServerName))
                {
                    foreach (ServiceController controller in ServiceController.GetServices())
                    {
                        if (this.ServerName.Equals(controller.ServiceName))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        public bool RuningState
        {
            get
            {
                bool flag = false;
                if ((this.ServiceControlObject.Status != ServiceControllerStatus.Paused) && (this.ServiceControlObject.Status != ServiceControllerStatus.Running))
                {
                    return flag;
                }
                return true;
            }
        }

        public string ServerName
        {
            get
            {
                return this.string_0;
            }
            set
            {
                this.string_0 = value;
            }
        }

        public ServiceController ServiceControlObject
        {
            get
            {
                return this.serviceController_0;
            }
            set
            {
                this.serviceController_0 = value;
            }
        }

        public bool StopState
        {
            get
            {
                bool flag = false;
                if ((this.ServiceControlObject.Status != ServiceControllerStatus.Paused) && (this.ServiceControlObject.Status != ServiceControllerStatus.Stopped))
                {
                    return flag;
                }
                return true;
            }
        }
    }
}


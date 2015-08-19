using System;
using System.ServiceProcess;

namespace TimerServerParam
{
	public class ServerManager
	{
		private string _serverName = string.Empty;

		private ServiceController _serviceController;

		public bool IsExistService
		{
			get
			{
				if (string.IsNullOrEmpty(this.ServerName))
				{
					return false;
				}
				ServiceController[] services = ServiceController.GetServices();
				for (int i = 0; i < (int)services.Length; i++)
				{
					ServiceController serviceController = services[i];
					if (this.ServerName.Equals(serviceController.ServiceName))
					{
						return true;
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
				if (this.ServiceControlObject.Status == ServiceControllerStatus.Paused || this.ServiceControlObject.Status == ServiceControllerStatus.Running)
				{
					flag = true;
				}
				return flag;
			}
		}

		public string ServerName
		{
			get
			{
				return this._serverName;
			}
			set
			{
				this._serverName = value;
			}
		}

		public ServiceController ServiceControlObject
		{
			get
			{
				return this._serviceController;
			}
			set
			{
				this._serviceController = value;
			}
		}

		public bool StopState
		{
			get
			{
				bool flag = false;
				if (this.ServiceControlObject.Status == ServiceControllerStatus.Paused || this.ServiceControlObject.Status == ServiceControllerStatus.Stopped)
				{
					flag = true;
				}
				return flag;
			}
		}

		public ServerManager(string serviceName)
		{
			this.ServerName = serviceName;
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
				this.ServiceControlObject.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(240));
			}
		}

		public void StopService()
		{
			if (this.RuningState)
			{
				this.ServiceControlObject.Stop();
				this.ServiceControlObject.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(120));
			}
		}
	}
}
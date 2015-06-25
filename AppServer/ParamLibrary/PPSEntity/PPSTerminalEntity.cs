using System;

namespace ParamLibrary.PPSEntity
{
	[Serializable]
	public class PPSTerminalEntity
	{
		private int areaId;

		private int carId;

		private string carNum;

		private string oldCarNum;

		private string carPwd;

		private int terminalTypeId;

		private string termSerial;

		private string icon;

		private string simNum;

		private string oldSimNum;

		private DateTime svrStartTime;

		private DateTime svrEndTime;

		public int AreaId
		{
			get
			{
				return this.areaId;
			}
			set
			{
				this.areaId = value;
			}
		}

		public int CarId
		{
			get
			{
				return this.carId;
			}
			set
			{
				this.carId = value;
			}
		}

		public string CarNum
		{
			get
			{
				return this.carNum;
			}
			set
			{
				this.carNum = value;
			}
		}

		public string CarPwd
		{
			get
			{
				return this.carPwd;
			}
			set
			{
				this.carPwd = value;
			}
		}

		public string Icon
		{
			get
			{
				return this.icon;
			}
			set
			{
				this.icon = value;
			}
		}

		public string OldCarNum
		{
			get
			{
				return this.oldCarNum;
			}
			set
			{
				this.oldCarNum = value;
			}
		}

		public string OldSimNum
		{
			get
			{
				return this.oldSimNum;
			}
			set
			{
				this.oldSimNum = value;
			}
		}

		public string SimNum
		{
			get
			{
				return this.simNum;
			}
			set
			{
				this.simNum = value;
			}
		}

		public DateTime SVREndTime
		{
			get
			{
				return this.svrEndTime;
			}
			set
			{
				this.svrEndTime = value;
			}
		}

		public DateTime SVRStartTime
		{
			get
			{
				return this.svrStartTime;
			}
			set
			{
				this.svrStartTime = value;
			}
		}

		public int TerminalTypeId
		{
			get
			{
				return this.terminalTypeId;
			}
			set
			{
				this.terminalTypeId = value;
			}
		}

		public string TermSerial
		{
			get
			{
				return this.termSerial;
			}
			set
			{
				this.termSerial = value;
			}
		}

		public PPSTerminalEntity()
		{
		}
	}
}
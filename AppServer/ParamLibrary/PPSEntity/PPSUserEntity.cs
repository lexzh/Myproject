using System;

namespace ParamLibrary.PPSEntity
{
	[Serializable]
	public class PPSUserEntity
	{
		private string userId;

		private string userName;

		private string password;

		private string telephone;

		private int groupId;

		private int areaId;

		private int carId;

		private bool isStop;

		private bool isPersonal;

		private bool needPwd = false;

		private bool sudoOverDue = true;

		private bool multiSend = true;

		private bool allowSetParm = true;

		private string remark;

		private string areaName;

		public bool AllowSetParm
		{
			get
			{
				return this.allowSetParm;
			}
			set
			{
				this.allowSetParm = value;
			}
		}

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

		public string AreaName
		{
			get
			{
				return this.areaName;
			}
			set
			{
				this.areaName = value;
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

		public int GroupId
		{
			get
			{
				return this.groupId;
			}
			set
			{
				this.groupId = value;
			}
		}

		public bool IsPersonal
		{
			get
			{
				return this.isPersonal;
			}
			set
			{
				this.isPersonal = value;
			}
		}

		public bool IsStop
		{
			get
			{
				return this.isStop;
			}
			set
			{
				this.isStop = value;
			}
		}

		public bool MultiSend
		{
			get
			{
				return this.multiSend;
			}
			set
			{
				this.multiSend = value;
			}
		}

		public bool NeedPwd
		{
			get
			{
				return this.needPwd;
			}
			set
			{
				this.needPwd = value;
			}
		}

		public string Password
		{
			get
			{
				return this.password;
			}
			set
			{
				this.password = value;
			}
		}

		public string Remark
		{
			get
			{
				return this.remark;
			}
			set
			{
				this.remark = value;
			}
		}

		public bool SudoOverDue
		{
			get
			{
				return this.sudoOverDue;
			}
			set
			{
				this.sudoOverDue = value;
			}
		}

		public string Telephone
		{
			get
			{
				return this.telephone;
			}
			set
			{
				this.telephone = value;
			}
		}

		public string UserId
		{
			get
			{
				return this.userId;
			}
			set
			{
				this.userId = value;
			}
		}

		public string UserName
		{
			get
			{
				return this.userName;
			}
			set
			{
				this.userName = value;
			}
		}

		public PPSUserEntity()
		{
		}
	}
}
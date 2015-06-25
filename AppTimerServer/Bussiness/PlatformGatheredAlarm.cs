using System;
using System.Collections;
using System.Data;
using System.Timers;
using Library;

namespace Bussiness
{
	public class PlatformGatheredAlarm : ProcessBase
	{
		private Timer tCheckGatheredAlarm;

		private int CheckGatheredAlarmInterval = 60000 * ReadDataFromXml.iGatheredAlarmInterval;

		private int EffectiveTime = ReadDataFromXml.iEffectiveTime;

		private string SqlAll = "";

		private string Sql1 = " Insert into GpsGatheredAlarmInfo(RegionID, RegionName, AllowLargest, CurrentCount) select {0}, '{1}', {2}, {3} ";

		private string Sql2 = " UNION ALL select {0}, '{1}', {2}, {3} ";

		public PlatformGatheredAlarm()
		{
		}

		private DataTable getCurrentPosInfo()
		{
			DataTable dataBySql;
			try
			{
				string str = " select a.*, b.CarNum from GpsCarCurrentPosInfo a WITH(NOLOCK) INNER JOIN GisCar b on a.telephone = b.SimNum where dateadd(Hh, {0}, gpsTime) > getdate() ";
				dataBySql = SqlDataAccess.getDataBySql(string.Format(str, this.EffectiveTime));
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("PlatformGatheredAlarm", "getCurrentPosInfo", string.Concat("获取末次位置信息,", exception.Message));
				this.logHelper.WriteError(errorMsg);
				dataBySql = null;
			}
			return dataBySql;
		}

		private DataTable getGatheredAlarmConfig()
		{
			DataTable dataBySql;
			try
			{
				dataBySql = SqlDataAccess.getDataBySql(" select a.*, b.RegionName, b.RegionDot from GpsGatheredAlarmConfig a INNER JOIN GpsRegionType b on a.RegionId = b.RegionId ");
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("PlatformGatheredAlarm", "getGatheredAlarmConfig", string.Concat("获取聚集报警配置信息,", exception.Message));
				this.logHelper.WriteError(errorMsg);
				dataBySql = null;
			}
			return dataBySql;
		}

		private void InsertSql()
		{
			try
			{
				if (!string.IsNullOrEmpty(this.SqlAll))
				{
					int num = SqlDataAccess.insertBySql(this.SqlAll);
					if (num <= 0)
					{
						ErrorMsg errorMsg = new ErrorMsg("PlatformGatheredAlarm", "InsertSql", string.Concat("将聚集报警插入GpsGatheredAlarmInfo表错误，,返回值!", num.ToString(), "sql语句：", this.SqlAll));
						this.logHelper.WriteError(errorMsg);
					}
					this.SqlAll = "";
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg1 = new ErrorMsg("PlatformGatheredAlarm", "InsertSql", string.Concat("插入聚集报警信息,", exception.Message));
				this.logHelper.WriteError(errorMsg1);
			}
		}

		private string MergeSql(int RegionId, string RegionName, int AllowLargest, int CurrentCount, bool IsFirst)
		{
			if (!IsFirst)
			{
				PlatformGatheredAlarm platformGatheredAlarm = this;
				string sqlAll = platformGatheredAlarm.SqlAll;
				string sql2 = this.Sql2;
				object[] regionId = new object[] { RegionId, RegionName, AllowLargest, CurrentCount };
				platformGatheredAlarm.SqlAll = string.Concat(sqlAll, string.Format(sql2, regionId));
			}
			else
			{
				string sql1 = this.Sql1;
				object[] objArray = new object[] { RegionId, RegionName, AllowLargest, CurrentCount };
				this.SqlAll = string.Format(sql1, objArray);
			}
			LogMsg logMsg = new LogMsg("PlatformGatheredAlarm", "MergeSql", "");
			object[] regionId1 = new object[] { "发生聚集报警-区域ID：", RegionId, ",区域名称：", RegionName, "，允许最大车辆：", AllowLargest, "，当前车辆数：", CurrentCount };
			logMsg.Msg = string.Concat(regionId1);
			this.logHelper.WriteLog(logMsg);
			return this.SqlAll;
		}

		public override void start()
		{
			try
			{
				this.tCheckGatheredAlarm = new Timer(100);
				this.tCheckGatheredAlarm.Elapsed += new ElapsedEventHandler(this.tCheckGatheredAlarm_Elapsed);
				this.tCheckGatheredAlarm.Enabled = true;
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("PlatformGatheredAlarm", "start", string.Concat("开启检测聚集报警错误,", exception.Message));
				this.logHelper.WriteError(errorMsg);
			}
		}

		public override void stop()
		{
			this.tCheckGatheredAlarm.Stop();
		}

		private void tCheckGatheredAlarm_Elapsed(object sender, ElapsedEventArgs e)
		{
			this.tCheckGatheredAlarm.Enabled = false;
			this.tCheckGatheredAlarm.Interval = (double)this.CheckGatheredAlarmInterval;
			try
			{
				try
				{
					DataTable gatheredAlarmConfig = this.getGatheredAlarmConfig();
					if (gatheredAlarmConfig == null || gatheredAlarmConfig.Rows.Count <= 0)
					{
						return;
					}
					else
					{
						DataTable currentPosInfo = this.getCurrentPosInfo();
						if (currentPosInfo == null || currentPosInfo.Rows.Count <= 0)
						{
							return;
						}
						else
						{
							bool flag = true;
							foreach (DataRow row in gatheredAlarmConfig.Rows)
							{
								int num = Convert.ToInt32(row["AllowLargest"]);
								int num1 = 0;
								int num2 = Convert.ToInt32(row["RegionId"]);
								string str = row["RegionName"].ToString();
								string str1 = row["RegionDot"].ToString();
								foreach (DataRow dataRow in currentPosInfo.Rows)
								{
									if (!Check.IsInRegion(dataRow["Longitude"].ToString(), dataRow["Latitude"].ToString(), str1))
									{
										continue;
									}
									num1++;
								}
								if (num1 <= num)
								{
									continue;
								}
								this.MergeSql(num2, str, num, num1, flag);
								flag = false;
							}
							this.InsertSql();
						}
					}
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					ErrorMsg errorMsg = new ErrorMsg("PlatformGatheredAlarm", "tCheckGatheredAlarm_Elapsed", string.Concat("检测聚集报警，", exception.Message));
					this.logHelper.WriteError(errorMsg);
				}
			}
			finally
			{
				this.tCheckGatheredAlarm.Enabled = true;
			}
		}
	}
}
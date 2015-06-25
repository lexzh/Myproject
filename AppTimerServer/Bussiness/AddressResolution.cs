using GisServices;
using System;
using System.Collections;
using System.Data;
using System.Timers;
using Library;

namespace Bussiness
{
	public class AddressResolution : ProcessBase
	{
		private Timer tUpdateRemedyBillTimer;

		private Timer tGetCarDayPos;

		private int iGetBillPosInterval = 60000 * ReadDataFromXml.BillTime;

		private int iGetCarDayPosInterval = 3600000;

		private bool IsAccessInsertCarDayPos;

		public AddressResolution()
		{
		}

		private bool CompareAddress(string planAddress, string localAddress)
		{
			bool flag = false;
			if (string.IsNullOrEmpty(planAddress))
			{
				return flag;
			}
			if (string.IsNullOrEmpty(localAddress))
			{
				return flag;
			}
			planAddress = planAddress.Replace(" ", "");
			localAddress = localAddress.Replace(" ", "");
			if (localAddress.IndexOf(planAddress) != -1)
			{
				flag = true;
			}
			return flag;
		}

		private DataTable GetCarDayPos()
		{
			DataTable dataBySql;
			try
			{
				dataBySql = SqlDataAccess.getDataBySql(" exec WebMgr_DF_GetCarDayPos ");
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				LogHelper logHelper = new LogHelper();
				ErrorMsg errorMsg = new ErrorMsg("AddressResolution", "GetCarDayPos", string.Concat("获取车辆当天末次坐标发生错误!", exception.Message));
				logHelper.WriteError(errorMsg);
				dataBySql = null;
			}
			return dataBySql;
		}

		private DataTable GetOblivionOrder()
		{
			DataTable dataBySql;
			try
			{
				string str = string.Concat(" select * ", " FROM GpsShippingInfo ");
				DateTime now = DateTime.Now;
				str = string.Concat(str, " WHERE Status= 0 and '", now.ToString(), "' > ShippingTime and IsNotRemedy = 0 ");
				dataBySql = SqlDataAccess.getDataBySql(str);
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				LogHelper logHelper = new LogHelper();
				ErrorMsg errorMsg = new ErrorMsg()
				{
					ClassName = "AddressResolution",
					FunctionName = "GetOblivionOrder",
					ErrorText = string.Concat("获得未完成订单发生错误!", exception.Message)
				};
				logHelper.WriteError(errorMsg, exception);
				return null;
			}
			return dataBySql;
		}

		private DataTable GetOblivionOrderCoordinates(DataRow dr, DateTime eTime)
		{
			DateTime dateTime;
			DataTable dataBySql;
			try
			{
				string str = " select recetime, gpstime, Longitude, Latitude, DistanceDiff ";
				str = string.Concat(str, " from GpsReceRealTime WITH(NOLOCK) ");
				str = string.Concat(str, " where telephone = '{0}' and recetime between '{1}' and '{2}' ");
				if (dr["RemedyTime"].ToString() == null || !(dr["RemedyTime"].ToString() != ""))
				{
					dateTime = Convert.ToDateTime(dr["ShippingTime"].ToString());
				}
				else
				{
					DateTime dateTime1 = Convert.ToDateTime(dr["RemedyTime"].ToString());
					dateTime = dateTime1.AddMinutes(-3);
					dateTime = (dateTime < Convert.ToDateTime(dr["ShippingTime"].ToString()) ? Convert.ToDateTime(dr["ShippingTime"].ToString()) : dateTime);
				}
				DateTime dateTime2 = eTime;
				str = string.Format(str, dr["SimNum"], dateTime, dateTime2);
				dataBySql = SqlDataAccess.getDataBySql(str);
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				LogHelper logHelper = new LogHelper();
				ErrorMsg errorMsg = new ErrorMsg()
				{
					ClassName = "AddressResolution",
					FunctionName = "GetOblivionOrderCoordinates",
					ErrorText = string.Concat("获得未完成订单坐标信息发生错误!", exception.Message)
				};
				logHelper.WriteError(errorMsg, exception);
				return null;
			}
			return dataBySql;
		}

		private void InsCarDayPos()
		{
			try
			{
				LogMsg logMsg = new LogMsg();
				DataTable carDayPos = this.GetCarDayPos();
				if (carDayPos == null || carDayPos.Rows.Count <= 0)
				{
					this.IsAccessInsertCarDayPos = false;
					logMsg.ClassName = "AddressResolution";
					logMsg.FunctionName = "GetCarDayPos";
					logMsg.Msg = "获取车辆位置为空\n";
					this.logHelper.WriteLog(logMsg);
				}
				else
				{
					logMsg.Msg = string.Concat("获得车辆轨迹信息数量为", carDayPos.Rows.Count);
					logMsg.ClassName = "AddressResolution";
					logMsg.FunctionName = "GetCarDayPos";
					this.logHelper.WriteLog(logMsg);
					string[] strArrays = new string[carDayPos.Rows.Count];
					int num = 0;
					foreach (DataRow row in carDayPos.Rows)
					{
						string[] str = new string[] { row["gpstime"].ToString(), "@", row["Telephone"].ToString(), "@", row["Longitude"].ToString(), "@", row["Latitude"].ToString() };
						strArrays[num] = string.Concat(str, ",", row["Longitude"].ToString(), ",", row["Latitude"].ToString());
						num++;
					}
					string[] billCarAddress = ReadDataFromGis.GetBillCarAddress(strArrays);
					if (billCarAddress != null && (int)billCarAddress.Length > 0)
					{
						int num5 = 0;
						if (this.InsertCarDayPos(billCarAddress, ref num5) != -1)
						{
							logMsg.ClassName = "AddressResolution";
							logMsg.FunctionName = "GetCarDayPos";
							logMsg.Msg = string.Concat("插入车辆位置信息成功，数量：", num5, "\n");
							this.logHelper.WriteLog(logMsg);
						}
						else
						{
							this.IsAccessInsertCarDayPos = false;
							logMsg.ClassName = "AddressResolution";
							logMsg.FunctionName = "GetCarDayPos";
							logMsg.Msg = "插入车辆位置失败\n";
							this.logHelper.WriteLog(logMsg);
						}
					}
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				this.IsAccessInsertCarDayPos = false;
				ErrorMsg errorMsg = new ErrorMsg()
				{
					ClassName = "AddressResolution",
					FunctionName = "GetCarDayPos",
					ErrorText = string.Concat("插入车辆最接近12点详细位置信息!", exception.Message, "\n")
				};
				this.logHelper.WriteError(errorMsg, exception);
			}
		}

		private int InsertCarDayPos(string[] Locations, ref int count)
		{
			int num;
			try
			{
				if (Locations == null || (int)Locations.Length <= 0)
				{
					num = -1;
				}
				else
				{
					string str = " insert into DF_CarDayPos(Gpstime, SimNum, Longitude, Latitude, CarPos) select  '{0}','{1}','{2}','{3}','{4}' ";
					string str1 = " UNION ALL SELECT '{0}','{1}','{2}','{3}','{4}' ";
					bool flag = true;
					string[] locations = Locations;
					for (int i = 0; i < (int)locations.Length; i++)
					{
						string str2 = locations[i];
						char[] chrArray = new char[] { ':' };
						string[] strArrays = str2.Split(chrArray, 2);
						string[] strArrays1 = strArrays[1].Split(new char[] { ',' });
						if (strArrays1 != null && (int)strArrays1.Length > 0)
						{
							string[] strArrays2 = strArrays1;
							for (int j = 0; j < (int)strArrays2.Length; j++)
							{
								string str3 = strArrays2[j];
								string[] strArrays3 = str3.Split(new char[] { '@' });
								if ((int)strArrays3.Length < 4)
								{
									LogHelper logHelper = new LogHelper();
									LogMsg logMsg = new LogMsg()
									{
										ClassName = "ReadDataFromDB",
										FunctionName = "InsertCarDayPos",
										Msg = string.Concat("组合车辆位置信息失败，", str3)
									};
									logHelper.WriteLog(logMsg);
								}
								else if (!flag)
								{
									object[] objArray = new object[] { strArrays3[0], strArrays3[1], strArrays3[2], strArrays3[3], strArrays[0] };
									str = string.Concat(str, string.Format(str1, objArray));
									count = count + 1;
								}
								else
								{
									flag = false;
									object[] objArray1 = new object[] { strArrays3[0], strArrays3[1], strArrays3[2], strArrays3[3], strArrays[0] };
									str = string.Format(str, objArray1);
									count = 1;
								}
							}
						}
					}
					num = SqlDataAccess.insertBySql(str);
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				LogHelper logHelper1 = new LogHelper();
				ErrorMsg errorMsg = new ErrorMsg("AddressResolution", "InsertCarDayPos", string.Concat("插入车辆最接近12点详细位置信息发生错误!", exception.Message));
				logHelper1.WriteError(errorMsg);
				num = -1;
			}
			return num;
		}

		private void RemedyBills()
		{
			try
			{
				LogMsg logMsg = new LogMsg();
				DataTable oblivionOrder = this.GetOblivionOrder();
				DateTime now = DateTime.Now;
				if (oblivionOrder != null && oblivionOrder.Rows.Count > 0)
				{
					logMsg.Msg = string.Concat("获得未完成订单数量为", oblivionOrder.Rows.Count);
					logMsg.ClassName = "AddressResolution";
					logMsg.FunctionName = "RemedyBills";
					this.logHelper.WriteLog(logMsg);
					foreach (DataRow row in oblivionOrder.Rows)
					{
						DataTable oblivionOrderCoordinates = this.GetOblivionOrderCoordinates(row, now);
						if (oblivionOrderCoordinates == null || oblivionOrderCoordinates.Rows.Count <= 0)
						{
							continue;
						}
						string[] strArrays = new string[oblivionOrderCoordinates.Rows.Count - (int)oblivionOrderCoordinates.Select(string.Concat("gpstime < '", row["ShippingTime"].ToString(), "'")).Length];
						int num = 0;
						foreach (DataRow dataRow in oblivionOrderCoordinates.Rows)
						{
							if (Convert.ToDateTime(dataRow["gpstime"]) < Convert.ToDateTime(row["ShippingTime"]))
							{
								continue;
							}
							strArrays[num] = string.Concat(dataRow["recetime"].ToString(), "@", dataRow["DistanceDiff"].ToString(), ",", dataRow["Longitude"].ToString(), ",", dataRow["Latitude"].ToString());
							num++;
						}
						string[] billCarAddress = ReadDataFromGis.GetBillCarAddress(strArrays);
						int num5 = 0;
						if (billCarAddress == null || (int)billCarAddress.Length <= 0)
						{
							int num6 = Convert.ToInt32(ReadDataFromXml.OrderDays);
							if (now <= Convert.ToDateTime(row["ArrivalDate"]).AddDays((double)num6))
							{
								row["IsNotRemedy"] = 0;
								row["RemedyTime"] = now;
								num5 = 0;
							}
							else
							{
								row["IsNotRemedy"] = -1;
								row["RemedyTime"] = now;
								num5 = -1;
							}
							logMsg.Msg = string.Concat("地图返回地址信息为null。单号：", row["WaybillCode"]);
							this.logHelper.WriteLog(logMsg);
						}
						else
						{
							string[] strArrays9 = billCarAddress;
							int num7 = 0;
							while (num7 < (int)strArrays9.Length)
							{
								string str = strArrays9[num7];
								char[] chrArray = new char[] { ':' };
								string[] strArrays10 = str.Split(chrArray, 2);
								if ((row["ActShippingTime"] == DBNull.Value || row["ActShippingTime"].ToString() == "") && this.CompareAddress(row["ShippingLocation"].ToString(), strArrays10[0]))
								{
									string[] strArrays11 = strArrays10[1].Split(new char[] { ',' });
									if ((int)strArrays11.Length <= 0)
									{
										string[] strArrays12 = strArrays10[1].Split(new char[] { '@' });
										row["ActShippingTime"] = strArrays12[0];
										if ((int)strArrays12.Length <= 1 || strArrays12[1] == "")
										{
											row["BeginMileage"] = 0;
										}
										else
										{
											row["BeginMileage"] = strArrays12[1];
										}
									}
									else
									{
										string[] strArrays13 = strArrays11[0].Split(new char[] { '@' });
										row["ActShippingTime"] = strArrays13[0];
										if ((int)strArrays13.Length <= 1 || strArrays13[1] == "")
										{
											row["BeginMileage"] = 0;
										}
										else
										{
											row["BeginMileage"] = strArrays13[1];
										}
									}
								}
								if (!this.CompareAddress(row["Destination"].ToString(), strArrays10[0]))
								{
									num7++;
								}
								else
								{
									string[] strArrays14 = strArrays10[1].Split(new char[] { ',' });
									if ((int)strArrays14.Length <= 0)
									{
										string[] strArrays15 = strArrays10[1].Split(new char[] { '@' });
										row["ActArrivalDate"] = strArrays15[0];
										row["EndMileage"] = strArrays15[1];
										row["LastPosition"] = strArrays10[0];
										row["IsNotRemedy"] = 1;
										row["RemedyTime"] = now;
										row["Status"] = 1;
										num5 = 1;
										break;
									}
									else
									{
										string[] strArrays16 = strArrays14[0].Split(new char[] { '@' });
										row["ActArrivalDate"] = strArrays16[0];
										if ((int)strArrays16.Length <= 1 || strArrays16[1] == "")
										{
											row["EndMileage"] = 0;
										}
										else
										{
											row["EndMileage"] = strArrays16[1];
										}
										row["LastPosition"] = strArrays10[0];
										row["IsNotRemedy"] = 1;
										row["RemedyTime"] = now;
										row["Status"] = 1;
										num5 = 1;
										break;
									}
								}
							}
							if (num5 != 1)
							{
								int num8 = Convert.ToInt32(ReadDataFromXml.OrderDays);
								if (now <= Convert.ToDateTime(row["ArrivalDate"]).AddDays((double)num8))
								{
									row["IsNotRemedy"] = 0;
									row["RemedyTime"] = now;
									num5 = 0;
								}
								else
								{
									row["IsNotRemedy"] = -1;
									row["RemedyTime"] = now;
									num5 = -1;
								}
							}
							num5 = 0;
						}
					}
					this.UpdateRemedyBill(oblivionOrder);
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg()
				{
					ClassName = "AddressResolution",
					FunctionName = "RemedyBills",
					ErrorText = string.Concat("订单解析!", exception.Message, "\n", exception.StackTrace)
				};
				this.logHelper.WriteError(errorMsg, exception);
			}
		}

		public override void start()
		{
			try
			{
				this.tUpdateRemedyBillTimer = new Timer((double)this.iGetBillPosInterval);
				this.tUpdateRemedyBillTimer.Elapsed += new ElapsedEventHandler(this.UpdateRemedyBill_Elapsed);
				this.tUpdateRemedyBillTimer.Enabled = true;
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				this.tUpdateRemedyBillTimer.Enabled = true;
				ErrorMsg errorMsg = new ErrorMsg("AddressResolution", "start", string.Concat("定时补救订单", exception.Message));
				this.logHelper.WriteError(errorMsg);
			}
			try
			{
				this.tGetCarDayPos = new Timer((double)this.iGetCarDayPosInterval);
				this.tGetCarDayPos.Elapsed += new ElapsedEventHandler(this.tGetCarDayPos_Elapsed);
				this.tGetCarDayPos.Enabled = true;
			}
			catch (Exception exception3)
			{
				Exception exception2 = exception3;
				this.tGetCarDayPos.Enabled = true;
				ErrorMsg errorMsg1 = new ErrorMsg("AddressResolution", "start", string.Concat("车辆当天末次位置", exception2.Message));
				this.logHelper.WriteError(errorMsg1);
			}
		}

		public override void stop()
		{
			this.tUpdateRemedyBillTimer.Stop();
			this.tGetCarDayPos.Stop();
		}

		private void tGetCarDayPos_Elapsed(object sender, ElapsedEventArgs e)
		{
			this.tGetCarDayPos.Enabled = false;
			try
			{
				try
				{
					if (DateTime.Now.Hour == 0 || DateTime.Now.Hour == 1 && !this.IsAccessInsertCarDayPos)
					{
						this.IsAccessInsertCarDayPos = true;
						this.InsCarDayPos();
					}
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					this.tGetCarDayPos.Interval = (double)this.iGetCarDayPosInterval;
					this.tGetCarDayPos.Enabled = true;
					ErrorMsg errorMsg = new ErrorMsg("AddressResolution", "tGetCarDayPos_Elapsed", string.Concat("车辆当天末次位置:", exception.Message));
					this.logHelper.WriteError(errorMsg);
				}
			}
			finally
			{
				this.tGetCarDayPos.Interval = (double)this.iGetCarDayPosInterval;
				this.tGetCarDayPos.Enabled = true;
			}
		}

		private void UpdateRemedyBill(DataTable dtOrder)
		{
			LogHelper logHelper = new LogHelper();
			LogMsg logMsg = new LogMsg();
			try
			{
				string str = "";
				string str1 = " update GpsShippingInfo set ActArrivalDate = {0},LastPosition = '{1}',Status = '{2}',ActShippingTime = {3},BeginMileage = {4},EndMileage = {5},IsNotRemedy = '{6}', RemedyTime = {7}  where Id = {8};";
				foreach (DataRow row in dtOrder.Rows)
				{
					string str2 = (row["ActArrivalDate"] == DBNull.Value ? "null" : string.Concat("'", row["ActArrivalDate"].ToString(), "'"));
					string str3 = row["LastPosition"].ToString();
					string str4 = row["Status"].ToString();
					string str5 = (row["ActShippingTime"] == DBNull.Value ? "null" : string.Concat("'", row["ActShippingTime"].ToString(), "'"));
					string str6 = (row["BeginMileage"] == DBNull.Value ? "null" : string.Concat("'", row["BeginMileage"].ToString(), "'"));
					string str7 = (row["EndMileage"] == DBNull.Value ? "null" : string.Concat("'", row["EndMileage"].ToString(), "'"));
					string str8 = row["IsNotRemedy"].ToString();
					string str9 = (row["RemedyTime"] == DBNull.Value ? "null" : string.Concat("'", row["RemedyTime"].ToString(), "'"));
					string str10 = row["Id"].ToString();
					object[] objArray = new object[] { str2, str3, str4, str5, str6, str7, str8, str9, str10 };
					str = string.Concat(str, string.Format(str1, objArray));
					if (str8 != "-1")
					{
						if (str8 != "1")
						{
							continue;
						}
						logMsg.ClassName = "ReadDataFromDB";
						logMsg.FunctionName = "UpdateRemedyBill";
						string[] strArrays = new string[] { "Id为", str10, ",订单号为", row["WaybillCode"].ToString(), "的订单已经到达" };
						logMsg.Msg = string.Concat(strArrays);
						logHelper.WriteLog(logMsg);
					}
					else
					{
						logMsg.ClassName = "ReadDataFromDB";
						logMsg.FunctionName = "UpdateRemedyBill";
						string[] strArrays1 = new string[] { "Id为", str10, ",订单号为", row["WaybillCode"].ToString(), "的订单已经过期， IsNotRemedy为-1" };
						logMsg.Msg = string.Concat(strArrays1);
						logHelper.WriteLog(logMsg);
					}
				}
				SqlDataAccess.insertBySql(str);
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg()
				{
					ClassName = "AddressResolution",
					FunctionName = "UpdateRemedyBill",
					ErrorText = string.Concat("更新订单状态发生错误!", exception.Message)
				};
				logHelper.WriteError(errorMsg, exception);
			}
		}

		private void UpdateRemedyBill_Elapsed(object sender, ElapsedEventArgs e)
		{
			this.tUpdateRemedyBillTimer.Enabled = false;
			try
			{
				try
				{
					this.RemedyBills();
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					this.tUpdateRemedyBillTimer.Interval = (double)this.iGetBillPosInterval;
					this.tUpdateRemedyBillTimer.Enabled = true;
					ErrorMsg errorMsg = new ErrorMsg("AddressResolution", "UpdateRemedyBill_Elapsed", string.Concat("订单解析发生错误:", exception.Message));
					this.logHelper.WriteError(errorMsg);
				}
			}
			finally
			{
				this.tUpdateRemedyBillTimer.Interval = (double)this.iGetBillPosInterval;
				this.tUpdateRemedyBillTimer.Enabled = true;
			}
		}
	}
}
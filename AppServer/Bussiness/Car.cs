namespace Bussiness
{
    using ParamLibrary.Application;
    using ParamLibrary.CarEntity;
    using ParamLibrary.CmdParamInfo;
    using DataAccess;
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Runtime.InteropServices;
    using System.Text;

    public class Car : ReceiveDataBase
    {
        public DataTable Car_AddPassWayPathIdToTmp(string string_0, int int_0, int int_1, string string_1, string string_2, int int_2)
        {
            SqlDataAccess access = new SqlDataAccess();
            string str = "AppSvr_UpdateGpsCarPassageWayParam_tmp";
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@WrkID", int_0), new SqlParameter("@OrderID", int_1), new SqlParameter("@carID", string_0), new SqlParameter("@PathID", string_1), new SqlParameter("@PassWayID", string_2), new SqlParameter("@Speed", int_2) };
            return access.getDataBySP(str, parameterArray);
        }

        public DataTable Car_GetPassWayPathID(string string_0)
        {
            return new SqlDataAccess().getDataBySql(string.Format("select PathID,Speed from GpsCarPassageWayParam where carid='{0}'", string_0));
        }

        public int DeletePathAlarm(string string_0)
        {
            string format = "delete from GpsCarPathParam where carID ={0}";
            format = string.Format(format, string_0);
            SqlDataAccess access = new SqlDataAccess();
            return access.updateBySql(format);
        }

        public int DeleteRegionAlarm(string string_0, int int_0)
        {
            string format = "delete from GpsCarRegionParam where carID ={0} and regionFeature = {1}";
            format = string.Format(format, string_0, int_0);
            SqlDataAccess access = new SqlDataAccess();
            return access.updateBySql(format);
        }

        public int ExecNoQuery(string string_0)
        {
            new SqlDataAccess().updateBySql(string_0);
            return 0;
        }

        public DataTable ExecSql(string string_0)
        {
            SqlDataAccess access = new SqlDataAccess();
            return access.getDataBySql(string_0);
        }

        public DataTable GetAlarmPathDotFromGisCar(string string_0)
        {
            string format = " select AlarmPathDot from GisCarInfoTable where carID={0}";
            format = string.Format(format, string_0);
            SqlDataAccess access = new SqlDataAccess();
            return access.getDataBySql(format);
        }

        public DataTable GetCaptureMoniterDataByCarId(string string_0)
        {
            string format = "select * from GpsCarCaptureParam where carId = {0}";
            format = string.Format(format, string_0);
            SqlDataAccess access = new SqlDataAccess();
            return access.getDataBySql(format);
        }

        public DataTable GetCarAlarmRegionInfo(string string_0)
        {
            string format = "select RegionDot, regionFeature from GisCarInfoTable where CarId={0}";
            format = string.Format(format, string_0);
            SqlDataAccess access = new SqlDataAccess();
            return access.getDataBySql(format);
        }

        public DataTable GetCarAlarmState(string string_0)
        {
            string format = "select CarId,cust_carAlarmSwitch,cust_name from GisCarInfoTable where CarId={0}";
            format = string.Format(format, string_0);
            SqlDataAccess access = new SqlDataAccess();
            return access.getDataBySql(format);
        }

        public DataTable GetCarConfig(string string_0)
        {
            string format = "select * from GpsCarConfig where carID={0}";
            format = string.Format(format, string_0);
            SqlDataAccess access = new SqlDataAccess();
            return access.getDataBySql(format);
        }

        public CommonCar GetCarDetailInfoByCarId(string carId)
        {
            CommonCar car = new CommonCar();
            string str = "select a.CarModel, a.CarColor, a.PlateColor, b.TermSerial, a.PersonID, a.HomeAddress, a.ConnectorName, a.FirstConnectTele,a.OtherCommunication, a.CorpName, a.ConnectTele, a.OwnerSimNum, a.OwnerEmail,a.CustomType, a.ReportCenterNum, a.DTMFAlarmNum, a.ListenRecordNum, a.HaveOwnTele,a.MedicalNum, a.RepairNum, a.HelpNum, a.Sex, a.PostCode, a.FirstConnectorName, a.APNAddr,a.GPRSUser, a.GPRSPassword, a.TcpIP, a.TcpPort, a.UdpIP, a.UdpPort, a.AgentIp, a.AgentPort,a.isUseProxy, a.carCommunicationType, a.carAlarmSwitch, a.carAlarmFlag,a.AlarmFlagType,a.AlarmFlagEx,a.CarAlarmSwitchEx,a.CarShowAlarmEx,a.isShowForm,a.callInLst, a.callOutLst, a.MobileCenter, a.carControlType, a.carControlMask,a.cust_carAlarmSwitch, a.cust_carAlarmFlag, a.cust_isShowForm, a.cust_level, a.cust_name, a.ServerType,b.carNum,b.simNum, b.CreateTime, b.OwnerName,b.Remark,b.SIMpassport, b.SVRStartTime, b.SVREndTime, b.SIMbeginTime,b.SIMendTime,b.PromptDays ,d.sms, d.gprs, d.cdma, d.name, e.SoftVersion, f.CarTypeName, c.areaName,a.FactoryPlateModel,g.[name] as drivername,g.telephone,g.LaborQualificationNo from GisCar b INNER JOIN GisCarInfoTable a ON a.CarID = b.CarID  INNER JOIN  GpsArea c ON b.AreaId = c.AreaID  INNER JOIN GpsTerminalType d ON b.TerminalTypeID = d.TerminalTypeID  INNER JOIN GpsProtocol e ON d.ProtocolCode = e.ProtocolCode  INNER JOIN CarTypeTable f ON b.CarType = f.CarType left JOIN gpsdriver g on b.carid=g.carid where a.carID=" + carId;
            DataTable table = new SqlDataAccess().getDataBySql(str);
            if ((table != null) && (table.Rows.Count >= 1))
            {
                DataRow row = table.Rows[0];
                car.id = int.Parse(carId);
                car.carNum = row["carNum"] as string;
                car.tele = row["simNum"] as string;
                car.areaName = row["areaName"] as string;
                car.createTime = (DateTime) row["CreateTime"];
                car.OwnerName = row["OwnerName"] as string;
                car.CarModel = row["CarModel"] as string;
                car.CarColor = row["CarColor"] as string;
                car.platecolor = row["PlateColor"] as string;
                //switch ((row["PlateColor"]==DBNull.Value)?0:int.Parse(row["PlateColor"].ToString()))
                //{
                //    case 1:
                //        car.platecolor = "蓝色";
                //        break;
                //    case 2:
                //        car.platecolor = "黄色";
                //        break;
                //    case 3:
                //        car.platecolor = "黑色";
                //        break;
                //    case 4:
                //        car.platecolor = "白色";
                //        break;
                //    case 9:
                //        car.platecolor = "其他";
                //        break;
                //    default:
                //        car.platecolor = string.Empty;
                //        break;
                //};
                car.TermSerial = row["TermSerial"] as string;
                car.PersonID = row["PersonID"] as string;
                car.HomeAddress = row["HomeAddress"] as string;
                car.ConnectorName = row["ConnectorName"] as string;
                car.FirstConnectTele = row["FirstConnectTele"] as string;
                car.OtherCommunication = row["OtherCommunication"] as string;
                car.CorpName = row["CorpName"] as string;
                car.ConnectTele = row["ConnectTele"] as string;
                car.OwnerSimNum = row["OwnerSimNum"] as string;
                car.CustomType = row["CustomType"] as string;
                car.OwnerEmail = row["OwnerEmail"] as string;
                car.Remark = row["Remark"] as string;
                car.ReportCenterNum = row["ReportCenterNum"] as string;
                car.DTMFAlarmNum = row["DTMFAlarmNum"] as string;
                car.HaveOwnTele = row["HaveOwnTele"] as string;
                car.ListenRecordNum = row["ListenRecordNum"] as string;
                car.MedicalNum = row["MedicalNum"] as string;
                car.RepairNum = row["RepairNum"] as string;
                car.HelpNum = row["HelpNum"] as string;
                car.MobileCenterNum = row["MobileCenter"] as string;
                car.APNAddr = row["APNAddr"] as string;
                car.GPRSUser = row["GPRSUser"] as string;
                car.GPRSPassword = row["GPRSPassword"] as string;
                car.TcpIP = row["TcpIP"] as string;
                car.TcpPort = row["TcpPort"] as string;
                car.UdpPort = row["UdpPort"] as string;
                car.UdpIP = row["UdpIP"] as string;
                car.AgentIP = row["AgentIP"] as string;
                car.AgentPort = row["AgentPort"] as string;
                car.isUseProxy = (bool) row["isUseProxy"];
                car.AlarmPathDot = "";
                car.carCommunicationType = row["carCommunicationType"] as string;
                car.carControlType = row["carControlType"] as string;
                car.carControlMask = row["carControlMask"] as string;
                car.CarAlarmSwitch = row["carAlarmSwitch"] as string;
                car.CarAlarmFlag = row["CarAlarmFlag"] as string;
                car.ShowAlarm = row["isShowForm"] as string;
                car.AlarmFlagType = (row["AlarmFlagType"] == DBNull.Value) ? -1 : int.Parse(row["AlarmFlagType"].ToString());
                car.CarAlarmFlagEx = (row["AlarmFlagEx"] == DBNull.Value) ? "" : row["AlarmFlagEx"].ToString();
                car.CarAlarmSwitchEx = (row["CarAlarmSwitchEx"] == DBNull.Value) ? "" : row["CarAlarmSwitchEx"].ToString();
                car.ShowAlarmEx = (row["CarShowAlarmEx"] == DBNull.Value) ? "" : row["CarShowAlarmEx"].ToString();
                car.ServerType = (row["ServerType"] == DBNull.Value) ? -1 : int.Parse(row["ServerType"].ToString());
                car.DrverName = (row["drivername"] == DBNull.Value) ? "" : row["drivername"].ToString();
                car.DriverPhone = (row["telephone"] == DBNull.Value) ? "" : row["telephone"].ToString();
                car.SteelGrade = (row["FactoryPlateModel"] == DBNull.Value) ? "" : row["FactoryPlateModel"].ToString();
                car.callInList = row["callInLst"] as string;
                car.callOutList = row["callOutLst"] as string;
                if (row["SVRStartTime"] != DBNull.Value)
                {
                    car.svrBeginTime = (DateTime) row["SVRStartTime"];
                }
                if (row["SVREndTime"] != DBNull.Value)
                {
                    car.svrEndTime = (DateTime) row["SVREndTime"];
                }
                if (row["SIMbeginTime"] != DBNull.Value)
                {
                    car.SimBeginTime = (DateTime) row["SIMbeginTime"];
                }
                if (row["SIMendTime"] != DBNull.Value)
                {
                    car.SimEndTime = (DateTime) row["SIMendTime"];
                }
                if (row["PromptDays"] != DBNull.Value)
                {
                    car.awokeDays = (int) row["PromptDays"];
                }
                if (row["Sex"] == DBNull.Value)
                {
                    car.Sex = this.method_2(3);
                }
                else
                {
                    car.Sex = this.method_2(int.Parse(row["Sex"].ToString()));
                }
                car.PostCode = row["PostCode"] as string;
                car.FirstConnectorName = row["FirstConnectorName"] as string;
                car.regionDotStr = "";
                car.terminalName = row["name"] as string;
                car.terminalSoftVersion = row["SoftVersion"] as string;
                car.isSupportMSM = (bool) row["sms"];
                car.isSupportGPRS = (bool) row["gprs"];
                car.isSupportCDMA = (bool) row["cdma"];
                car.carType = row["carTypeName"] as string;
                try
                {
                    car.cust_CarAlarmSwitch = row["cust_carAlarmSwitch"] as string;
                    car.cust_CarAlarmFlag = row["cust_CarAlarmFlag"] as string;
                    car.cust_ShowAlarm = row["cust_isShowForm"] as string;
                    car.cust_level = row["cust_level"] as string;
                    car.cust_Name = row["cust_name"] as string;
                    car.SIMpassport = row["SIMpassport"] as string;
                }
                catch
                {
                }
                car.LaborQualificationNo = row["LaborQualificationNo"] as string;
            }
            return car;
        }
        
        public DataTable GetCarImgInfo(string strPhone, string strGpsTime, string strCaremaId, string strReceTime)
        {
            int num = this.convertCaremaId(strCaremaId);
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@simnum", strPhone), new SqlParameter("@gpsTime", strGpsTime), new SqlParameter("@caramId", num.ToString()), new SqlParameter("@receTime", strReceTime) };
            return new SqlDataAccess().getDataBySP("WebGpsClient_ReceImgQuery", parameterArray);
        }

        public DataTable GetCarInfoByConditionLikeAll(string string_0, string string_1, string string_2, string string_3)
        {
            if (!string.IsNullOrEmpty(string_1))
            {
                string_1 = string_1.Replace("%", @"\%");
            }
            string str = "WebGpsClient_QueryCarInfoUser";
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@iType", string_0), new SqlParameter("@vchrContent", string_1), new SqlParameter("@vchrUserId", string_2), new SqlParameter("@vchrAreaCode", string_3) };
            return new SqlDataAccess().getDataBySP(str, parameterArray);
        }

        public DataTable GetCarList(string string_0, int int_0, int int_1)
        {
            DataTable table2;
            try
            {
                SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@UserId", string_0), new SqlParameter("@Page", int_0), new SqlParameter("@RecsPerPage", int_1) };
                table2 = new SqlDataAccess().getDataBySP("WebGpsClient_GetCarListEx", parameterArray);
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return table2;
        }

        public DataTable GetCarListEx(string string_0, int int_0, int int_1)
        {
            DataTable table2;
            try
            {
                SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@UserId", string_0), new SqlParameter("@Page", int_0), new SqlParameter("@RecsPerPage", int_1) };
                table2 = new SqlDataAccess().getDataBySP("WebGpsClient_GetCarListEx", parameterArray);
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return table2;
        }
        
        public DataTable GetCarMutilVideoInfo(string strPhone, string strGpsTime, string strCaremaId, string picDataType, string strReceTime)
        {
            if ("1".Equals(picDataType))
            {
                return this.GetCarImgInfo(strPhone, strGpsTime, strCaremaId, strReceTime);
            }
            return this.GetCarVideoInfo(strPhone, strGpsTime, picDataType, strReceTime);
        }

        public DataTable GetCommonData(string string_0, int int_0, string[] string_1)
        {
            string str = string.Empty;
            if (int_0 == 1)
            {
                str = "Select * From GpsJtbCarPathAlarm_Platform Where CarID='" + string_0 + "'";
                return this.ExecSql(str);
            }
            if (int_0 == 2)
            {
                str = "Select CarID,RegionID,ISNULL(RegionType,0) AS REgionType,BeginTime,EndTime From GpsJtbCarRegionAlarm_Platform Where CarID='" + string_0 + "'";
                return this.ExecSql(str);
            }
            if (int_0 == 3)
            {
                str = "Select CarID From GpsJtbCarPathSegmentAlarm_Platform Where CarID='" + string_0 + "'";
                return this.ExecSql(str);
            }
            return null;
        }

        public DataTable GetDeviceShareRef()
        {
            string str = " select * from GpsCarDeviceShareRef ";
            SqlDataAccess access = new SqlDataAccess();
            return access.getDataBySql(str);
        }

        public DataTable GetDeviceType(string string_0)
        {
            string format = " SELECT a.id, a.DeviceName,a.DevCode,a.isPtp,b.carId,b.ComNum, b.sysResult  FROM GpsCarDeviceType a LEFT OUTER JOIN GpsCarDeviceParam b ON  b.Deviceid = a.id and b.CarId={0} and b.sysResult = 0 ";
            format = string.Format(format, string_0);
            SqlDataAccess access = new SqlDataAccess();
            return access.getDataBySql(format);
        }

        public DataTable GetImportWatchCarInfo(int int_0)
        {
            string format = "select b.CarNum,b.SimNum,b.CarId from  GpsCarFilter a inner join giscar b on a.phone = b.simnum where a.wrkid = {0} and a.isimportwatch = 1 order by b.CarId desc";
            format = string.Format(format, int_0);
            SqlDataAccess access = new SqlDataAccess();
            return access.getDataBySql(format);
        }

        public DataTable GetInterestPointMulti(string string_0, string string_1, int int_0)
        {
            string str = "appSvr_GetAreaMapInterest_mul";
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@_sOperName  ", string_0), new SqlParameter("@PoITypeStr", string_1), new SqlParameter("@POIAuth", int_0) };
            SqlDataAccess access = new SqlDataAccess();
            return access.getDataBySP(str, parameterArray);
        }

        public DataTable GetInterestPointSingle(string string_0, string string_1, int int_0)
        {
            string str = "appSvr_GetAreaMapInterest";
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@areaCode ", string_0), new SqlParameter("@PoITypeStr", string_1), new SqlParameter("@POIAuth", int_0) };
            SqlDataAccess access = new SqlDataAccess();
            return access.getDataBySP(str, parameterArray);
        }

        public DataRow GetLastDotData(string string_0)
        {
            string str = "AppSvr_GetLastDot";
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@phone", string_0) };
            DataTable table = new SqlDataAccess().getDataBySP(str, parameterArray);
            if ((table != null) && (table.Rows.Count >= 1))
            {
                return table.Rows[0];
            }
            return null;
        }

        public DataTable GetMapType()
        {
            string str = "select a.Name from GpsMapFlagType a";
            SqlDataAccess access = new SqlDataAccess();
            return access.getDataBySql(str);
        }

        public DataTable GetNewPathId(string string_0, string string_1, int int_0)
        {
            string str = "WebGpsClient_GetNewPathId";
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@CarId", string_0), new SqlParameter("@PathName", string_1), new SqlParameter("@OriginMaxPathId", int_0) };
            SqlDataAccess access = new SqlDataAccess();
            return access.getDataBySP(str, parameterArray);
        }

        public DataTable GetNewRegionId(string string_0, string string_1, int int_0)
        {
            string str = "WebGpsClient_GetNewRegionId";
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@CarId", string_0), new SqlParameter("@RegionName", string_1), new SqlParameter("@OriginMaxRegionId", int_0) };
            SqlDataAccess access = new SqlDataAccess();
            return access.getDataBySP(str, parameterArray);
        }

        public string GetParamData(string carid, int OrderCoder)
        {
            string str = "";
            object obj2 = ("" + " Select  Param " + " From    GpsCarSetParam ") + " Where   CarId = " + carid;
            string str2 = string.Concat(new object[] { obj2, " And     MsgType = '", OrderCoder, "'" });
            DataTable table = this.ExecSql(str2);
            if ((table != null) && (table.Rows.Count > 0))
            {
                str = table.Rows[0]["Param"].ToString();
            }
            return str;
        }

        public DataTable GetPathAlarm(string string_0, string string_1)
        {
            string format = "SELECT A.PathName,A.PathID, b.carID, b.Choose, case b.Choose when 1 then 'checked' else 'unchecked' end as IsChoose, b.HoldTime, b.TopSpeed ,b.BeginTime,b.EndTime,a.PathGroupId,cast(b.PathFlag as varchar(1000)) as PathFlag FROM (SELECT DISTINCT A.PathName,A.PathID,B.PathGroupId  FROM gpsPathType A,GpsPathInGroup B,GpsPathGroupAuth C WHERE C.USERID='{0}' AND (c.AUTH&1<>0 or c.AUTH&2<>0) AND B.PathID IS NOT NULL AND C.pathgroupID=B.pathgroupID AND A.PathID=B.PathID) A LEFT OUTER JOIN  GpsCarPathParam b ON a.PathID = b.pathID and (b.carID = {1} or b.carID is null) ";
            format = string.Format(format, string_0, string_1);
            SqlDataAccess access = new SqlDataAccess();
            return access.getDataBySql(format);
        }

        public DataTable GetPathAlarmAnother(string string_0, string string_1)
        {
            string format = "SELECT A.*, b.carID, b.Choose, b.HoldTime, b.TopSpeed FROM (SELECT A.*,B.PathGroupId FROM gpsPathType A,GpsPathInGroup B,GpsPathGroupAuth C WHERE C.USERID='{0}' AND (c.AUTH&1<>0 or c.AUTH&2<>0) AND B.PathID IS NOT NULL AND C.pathgroupID=B.pathgroupID AND A.PathID=B.PathID) A LEFT OUTER JOIN  GpsCarPathParam b ON a.PathID = b.pathID and (b.carID = {1} or b.carID is null) ";
            format = string.Format(format, string_0, string_1);
            SqlDataAccess access = new SqlDataAccess();
            return access.getDataBySql(format);
        }

        public DataTable GetPathAlarmChecked(string string_0, string string_1)
        {
            string format = "SELECT A.PathName,b.NEWPathID FROM (SELECT DISTINCT A.PathName,A.PathID  FROM gpsPathType A,GpsPathInGroup B,GpsPathGroupAuth C WHERE C.USERID='{0}' AND (c.AUTH&1<>0 or c.AUTH&2<>0) AND B.PathID IS NOT NULL AND C.pathgroupID=B.pathgroupID AND A.PathID=B.PathID) A inner join GpsCarPathParam b ON a.PathID = b.pathID and (b.carID = {1} or b.carID is null) ";
            format = string.Format(format, string_0, string_1);
            SqlDataAccess access = new SqlDataAccess();
            return access.getDataBySql(format);
        }

        public DataTable GetPathRouteByPathName(string string_0)
        {
            string format = "select alarmPathDot,PathName from gpsPathType  where pathName in({0})";
            format = string.Format(format, string_0);
            SqlDataAccess access = new SqlDataAccess();
            return access.getDataBySql(format);
        }

        public DataTable GetPathSegmentAlarm(string string_0, string string_1)
        {
            string format = "SELECT b.PathSegmentName,b.PathSegmentID, b.PathID,d.HoldTime,d.DriEnough,d.DriNoEnough,d.TopSpeed,d.Flag,b.AlarmSegmentDot,d.PathWidth FROM (SELECT DISTINCT A.PathName,A.PathID,B.PathGroupId  FROM gpsPathType A,GpsPathInGroup B,GpsPathGroupAuth C WHERE C.USERID='{0}' AND (c.AUTH&1<>0 or c.AUTH&2<>0) AND B.PathID IS NOT NULL AND C.pathgroupID=B.pathgroupID AND A.PathID=B.PathID) A inner  JOIN  GpsPathSegment b ON a.PathID = b.pathID  left join GpsPathSegmentParam d on  b.pathID= d.pathID  and b.PathSegmentID = d.PathSegmentID  and (d.carID ={1} or d.carID is null)";
            format = string.Format(format, string_0, string_1);
            SqlDataAccess access = new SqlDataAccess();
            return access.getDataBySql(format);
        }

        public DataTable GetPhoneNumTextByCarid(string string_0)
        {
            if (string.IsNullOrEmpty(string_0))
            {
                return null;
            }
            SqlDataAccess access = new SqlDataAccess();
            string str = string.Concat(new object[] { " select Param from GpsCarSetParam where CarID=", string_0, "and MsgType=", 0x4017 });
            object returnBySql = access.GetReturnBySql(str);
            if (returnBySql == null)
            {
                return null;
            }
            string str2 = returnBySql.ToString();
            string[] separator = new string[] { "#" };
            DataTable table = new DataTable();
            DataColumn column = new DataColumn("Flag");
            DataColumn column2 = new DataColumn("Phone");
            DataColumn column3 = new DataColumn("Name");
            table.Columns.AddRange(new DataColumn[] { column, column2, column3 });
            string[] strArray2 = str2.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < strArray2.Length; i++)
            {
                DataRow row = table.NewRow();
                string[] strArray3 = strArray2[i].Split(new char[] { ',' });
                if (strArray3.Length == 3)
                {
                    row["Flag"] = strArray3[0];
                    row["Phone"] = strArray3[1];
                    row["Name"] = strArray3[2];
                    table.Rows.Add(row);
                }
            }
            return table;
        }

        public void GetPhoneNumTextPara(out string string_0, out string string_1, TrafficPhoneNumText trafficPhoneNumText_0)
        {
            string[] separator = new string[] { "," };
            new SqlDataAccess();
            string[] strArray2 = trafficPhoneNumText_0.FlagList.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            string[] strArray3 = trafficPhoneNumText_0.PhoneListList.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            string[] strArray4 = trafficPhoneNumText_0.NameList.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            string_0 = "#";
            string_1 = "设置：";
            if (((strArray2.Length != 0) && (strArray3.Length != 0)) && (strArray4.Length != 0))
            {
                for (int i = 0; i < strArray2.Length; i++)
                {
                    if ((i >= strArray3.Length) || (i >= strArray4.Length))
                    {
                        break;
                    }
                    string str = strArray2[i];
                    string str2 = strArray3[i];
                    string str3 = strArray4[i];
                    string str4 = string_0;
                    string_0 = str4 + str + "," + str2 + "," + str3 + "#";
                    string str5 = string_1;
                    string_1 = str5 + str + "," + str2 + "," + str3 + "#";
                }
            }
        }

        public DataTable GetPhonesByType(CmdParam.PhoneType phoneType_0, string string_0)
        {
            string str = string.Format("Select {0} From GisCarInfoTable Where carID = {1}", ((CmdParam.PhoneTypeField) phoneType_0).ToString(), string_0);
            SqlDataAccess access = new SqlDataAccess();
            return access.getDataBySql(str);
        }

        public DataTable GetPOIAuth()
        {
            string str = "select POIAuth from GpsSysConfig";
            SqlDataAccess access = new SqlDataAccess();
            return access.getDataBySql(str);
        }

        /// <summary>
        /// 查找区域内的车辆并返回末次位置信息
        /// </summary>
        /// <param name="minLng"></param>
        /// <param name="minLat"></param>
        /// <param name="maxLng"></param>
        /// <param name="maxLat"></param>
        /// <param name="strAreaCode"></param>
        /// <param name="strUseId"></param>
        /// <returns></returns>
        public DataTable GetRefRegion(double minLng, double minLat, double maxLng, double maxLat, string strAreaCode, string strUseId)
        {
            string str = "WebGpsClient_GetCarsInSelArea";
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@MinX", minLng), new SqlParameter("@MinY", minLat), new SqlParameter("@MaxX", maxLng), new SqlParameter("@MaxY", maxLat), new SqlParameter("@AreaCode", strAreaCode), new SqlParameter("@userID", strUseId) };
            DataTable table = new SqlDataAccess().getDataBySP(str, parameterArray);
            DataTable table2 = this.method_3();
            CarAlarmType type = new CarAlarmType();
            foreach (DataRow row in table.Rows)
            {
                Bussiness.CarInfo dataCarInfoBySimNum = CarDataInfoBuffer.GetDataCarInfoBySimNum(row["telephone"].ToString());
                if (dataCarInfoBySimNum != null)
                {
                    DataRow row2 = table2.NewRow();
                    row2["CarNum"] = dataCarInfoBySimNum.CarNum;
                    string str2 = row["speed"].ToString();
                    row2["Speed"] = str2.Substring(0, str2.IndexOf('.') + 3);
                    string str3 = row["Latitude"].ToString();
                    string str4 = row["Longitude"].ToString();
                    row2["Longitude"] = str4.Substring(0, str4.IndexOf('.') + 7);
                    row2["Latitude"] = str3.Substring(0, str3.IndexOf('.') + 7);
                    row2["SimNum"] = dataCarInfoBySimNum.SimNum;
                    row2["SvrTime"] = dataCarInfoBySimNum.SvrEndTime;
                    row2["AreaName"] = dataCarInfoBySimNum.AreaName;
                    row2["CarId"] = dataCarInfoBySimNum.CarId;
                    int carStatu = Convert.ToInt32(row["CarStatu"]);
                    long carStatuEx = 0L;
                    if (table.Columns.Contains("CarStatuEx"))
                    {
                        carStatuEx = Convert.ToInt64(row["CarStatuEx"]);
                    }
                    row2["StatuName"] = AlamStatus.GetStatusNameByCarStatu((long) carStatu) + AlamStatus.GetStatusNameByCarStatuExt(carStatuEx) + type.GetCustAlarmName(dataCarInfoBySimNum.SimNum, carStatu);
                    row2["GpsTime"] = row["gpstime"].ToString();
                    if (AlamStatus.IsAlarmReport(Convert.ToInt32(row["reserved"])))
                    {
                        row2["CarStatus"] = 1;
                    }
                    else
                    {
                        row2["CarStatus"] = 2;
                    }
                    row2["AlarmType"] = type.GetAlarmTypeValue(dataCarInfoBySimNum.SimNum, carStatu, carStatuEx);
                    int result = 0;
                    int.TryParse(row["TransportStatus"].ToString(), out result);
                    if (result == 3)
                    {
                        row2["IsFill"] = 1;
                    }
                    else
                    {
                        row2["IsFill"] = 0;
                    }
                    if (base.isPosStatus(carStatu))
                    {
                        row2["GpsValid"] = 1;
                    }
                    else
                    {
                        row2["GpsValid"] = 0;
                    }
                    if ((carStatu & 0x4000) == 0)
                    {
                        row2["AccOn"] = 0;
                    }
                    else
                    {
                        row2["AccOn"] = 1;
                    }
                    table2.Rows.Add(row2);
                }
            }
            return table2;
        }

        public DataTable GetRegionInfo(string userId, string strCarId, int iRegionFeature)
        {
            string format = "select b.*, a.* from GpsCarRegionParam a right join (SELECT A.regionID, A.regionName, A.regionDot,B.pathgroupid FROM gpsRegionType A, (select distinct b.REGIONID,b.pathgroupid from GpsPathGroupAuth a , GpsPathInGroup b                                     where a.pathgroupID=b.pathgroupID                                     and a.USERID='{0}' and (a.AUTH&1<>0 or a.AUTH&2<>0) and B.REGIONID IS NOT NULL) b WHERE A.REGIONID=B.REGIONID)  b on a.regionID = b.regionID  AND a.STATE='Y' AND a.carID={1} AND isnull(regionFeature,0) = {2}";
            format = string.Format(format, userId, strCarId, iRegionFeature);
            SqlDataAccess access = new SqlDataAccess();
            return access.getDataBySql(format);
        }

        public int InsertAlamFlagIntoGisCar(string string_0, string string_1, int int_0, int int_1, int int_2, int int_3, long long_0, long long_1, long long_2, long long_3)
        {
            string format = " insert into GisCarInfoTable_Tmp(carID, wrkID, orderID, carAlarmSwitch, carAlarmFlag, isShowForm,AlarmFlagType,AlarmFlagEx,CarAlarmSwitchEx,CarShowAlarmEx)  values({0}, {1}, {2}, '{3}','{4}', '{5}',{6},{7},{8},{9})";
            format = string.Format(format, new object[] { string_0, string_1, int_0, int_1, int_2, int_3, long_0, long_1, long_2, long_3 });
            SqlDataAccess access = new SqlDataAccess();
            return access.updateBySql(format);
        }

        public int InsertComArsgIntoGisCar(string string_0, string string_1, int int_0, int int_1, string string_2, string string_3, string string_4, string string_5, string string_6, string string_7, string string_8, int int_2, string string_9, string string_10, string string_11)
        {
            string format = "  insert into GisCarInfoTable_Tmp(carID, wrkID, orderID, carCommunicationType, APNAddr, GPRSUser, GPRSPassword, TCPIP, TCPPort, UDPIP, UDPPort, AgentIp, AgentPort, isUseProxy,ServerType)  values({0}, {1}, {2}, {3}, '{4}', '{5}','{6}', '{7}','{8}','{9}',{10}, '{11}', '{12}',{13},'{14}')";
            format = string.Format(format, new object[] { string_0, string_1, int_0, int_1, string_2, string_3, string_4, string_5, string_6, string_7, string_8, string_9, string_10, int_2, string_11 });
            SqlDataAccess access = new SqlDataAccess();
            return access.updateBySql(format);
        }

        public int InsertCommandParameterToDB(string string_0, int int_0, int int_1, string string_1)
        {
            string format = "insert into GPSJTBSysSndCmd(SimNum,CmdCode,CmdContent,OrderId,AddTime,bSend) values('{0}', {1}, '{2}',{3}, getdate(),-1)";
            string str2 = string.Format(format, new object[] { string_0, int_1, string_1, int_0 });
            SqlDataAccess access = new SqlDataAccess();
            return access.insertBySql(str2);
        }

        public int InsertCustAlarmIntoGisCar(string string_0, string string_1, int int_0, long long_0, long long_1, long long_2, long long_3, string string_2)
        {
            string format = "insert into GisCarInfoTable_Tmp(carID, wrkID, orderID, cust_carAlarmSwitch, cust_carAlarmFlag, cust_isShowForm, cust_level, cust_name) values({0}, {1}, {2}, '{3}', '{4}', '{5}', '{6}', '{7}')";
            format = string.Format(format, new object[] { string_0, string_1, int_0, long_0, long_1, long_2, long_3, string_2 });
            SqlDataAccess access = new SqlDataAccess();
            return access.updateBySql(format);
        }

        public int InsertGpsCarDeviceParam(string string_0, int int_0, int int_1, string string_1, string string_2)
        {
            string str = "delete from GpsCarDeviceParam where carId = " + string_0;
            //string[] strArray = null;
            if (string_1.Length > 0)
            {
                foreach (string str2 in string_1.Split(new char[] { '\\' }))
                {
                    str = str + string.Format(" insert into GpsCarDeviceParam(carId,WrkId,OrdId,DeviceId,ComNum,sysResult)  values({0}, {1}, {2}, '{3}', '{4}', '{5}') ", new object[] { string_0, int_0, int_1, str2, "1", "-1" });
                }
            }
            //strArray = null;
            if (string_2.Length > 0)
            {
                foreach (string str3 in string_2.Split(new char[] { '\\' }))
                {
                    str = str + string.Format(" insert into GpsCarDeviceParam(carId,WrkId,OrdId,DeviceId,ComNum,sysResult)  values({0}, {1}, {2}, '{3}', '{4}', '{5}') ", new object[] { string_0, int_0, int_1, str3, "2", "-1" });
                }
            }
            SqlDataAccess access = new SqlDataAccess();
            return access.updateBySql(str);
        }

        public int InsertIntoCaptureParam(string string_0, CaptureEx captureEx_0)
        {
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@CarId", string_0), new SqlParameter("@IsMultiFrame ", captureEx_0.IsMultiFrame), new SqlParameter("@CamerasID ", captureEx_0.CamerasID), new SqlParameter("@CaptureFlag ", captureEx_0.CaptureFlag), new SqlParameter("@CapTureMask ", captureEx_0.CaptureCache), new SqlParameter("@Times ", captureEx_0.Times), new SqlParameter("@CatchInterval ", captureEx_0.Interval), new SqlParameter("@Quality ", captureEx_0.Quality), new SqlParameter("@Brightness ", captureEx_0.Brightness), new SqlParameter("@Contrast ", captureEx_0.Contrast), new SqlParameter("@Saturation ", captureEx_0.Saturation), new SqlParameter("@Chroma ", captureEx_0.Chroma), new SqlParameter("@capWhenStop ", captureEx_0.CapWhenStop) };
            string str = "WebGpsClient_UpdateImageControl";
            SqlDataAccess access = new SqlDataAccess();
            return access.updateBySp(str, parameterArray);
        }

        public int InsertPathIdsIntoGisCar(string string_0, string string_1, string string_2, string string_3)
        {
            string str = string.Format("insert into GisCarInfoTable_Tmp(carID, wrkID, orderID, delPathID)  values({0}, {1}, {2}, '{3}')", new object[] { string_0, string_1, string_2, string_3 });
            SqlDataAccess access = new SqlDataAccess();
            return access.updateBySql(str);
        }

        public int InsertPathIdsIntoPathParam(string string_0, string string_1, string string_2, string string_3, string string_4)
        {
            string str = string.Format("insert into GpsCarSetPathParam_TMP(carID, wrkID, orderID, PathID,NewPathId)  values({0}, {1}, {2}, {3},{4})", new object[] { string_0, string_1, string_2, string_3, string_4 });
            SqlDataAccess access = new SqlDataAccess();
            return access.updateBySql(str);
        }

        public int InsertPathIntoGisCar(string string_0, int int_0, int int_1, string string_1)
        {
            string str = string.Format("insert into GisCarInfoTable_Tmp(carID, wrkID, orderID, alarmPathDot) values({0},{1},{2},'{3}')", new object[] { string_0, int_0, int_1, string_1 });
            SqlDataAccess access = new SqlDataAccess();
            return access.updateBySql(str);
        }

        public int InsertPhonesIntoGisCar(CmdParam.PhoneType phoneType_0, string string_0, string string_1, string string_2, string string_3)
        {
            CmdParam.PhoneTypeField field = (CmdParam.PhoneTypeField) phoneType_0;
            string str = string.Format("insert into GisCarInfoTable_Tmp(carID, wrkID, orderID, {0}) values({1}, {2},{3},'{4}')", new object[] { field.ToString(), string_0, string_1, string_2, string_3 });
            SqlDataAccess access = new SqlDataAccess();
            return access.updateBySql(str);
        }

        public int InsertRegionIdsIntoGisCar(string string_0, string string_1, string string_2, string string_3)
        {
            string str = string.Format("insert into GisCarInfoTable_Tmp(carID, wrkID, orderID, delRegionID)  values({0}, {1}, {2}, '{3}')", new object[] { string_0, string_1, string_2, string_3 });
            SqlDataAccess access = new SqlDataAccess();
            return access.updateBySql(str);
        }

        public int InsertRegionIntoGisCar(string string_0, int int_0, int int_1, string string_1, string string_2)
        {
            string str = string.Format("insert into GisCarInfoTable_Tmp(carID, wrkID, orderID, RegionDot, regionFeature) values({0},{1},{2},'{3}', {4})", new object[] { string_0, int_0, int_1, string_1, string_2 });
            SqlDataAccess access = new SqlDataAccess();
            return access.updateBySql(str);
        }

        public int InsertTrafficSegmentParam(string string_0, int int_0, int int_1, int? nullable_0, int? nullable_1, int? nullable_2, int? nullable_3, int int_2, int int_3)
        {
            string str = "WebGpsClient_UpdateGpsPathSegmentParam";
            SqlParameter[] parameterArray = new SqlParameter[9];
            parameterArray[0] = new SqlParameter("@carID", string_0);
            parameterArray[1] = new SqlParameter("@pathID", int_0);
            parameterArray[2] = new SqlParameter("@PathSegmentID", int_1);
            if (!nullable_0.HasValue)
            {
                parameterArray[3] = new SqlParameter("@topSpeed", DBNull.Value);
            }
            else
            {
                parameterArray[3] = new SqlParameter("@topSpeed", nullable_0);
            }
            if (!nullable_1.HasValue)
            {
                parameterArray[4] = new SqlParameter("@holdTime", DBNull.Value);
            }
            else
            {
                parameterArray[4] = new SqlParameter("@holdTime", nullable_1);
            }
            if (!nullable_2.HasValue)
            {
                parameterArray[5] = new SqlParameter("@DriEnough", DBNull.Value);
            }
            else
            {
                parameterArray[5] = new SqlParameter("@DriEnough", nullable_2);
            }
            if (!nullable_3.HasValue)
            {
                parameterArray[6] = new SqlParameter("@DriNoEnough", DBNull.Value);
            }
            else
            {
                parameterArray[6] = new SqlParameter("@DriNoEnough", nullable_3);
            }
            parameterArray[7] = new SqlParameter("@Flag", int_2);
            parameterArray[8] = new SqlParameter("@PathWidth", int_3);
            SqlDataAccess access = new SqlDataAccess();
            return access.updateBySp(str, parameterArray);
        }

        private int convertCaremaId(string strCaremaId)
        {
            switch (strCaremaId)
            {
                case "8":
                    return 0x80;

                case "7":
                    return 0x40;

                case "6":
                    return 0x20;

                case "5":
                    return 0x10;

                case "4":
                    return 8;

                case "3":
                    return 4;

                case "2":
                    return 2;

                case "1":
                    return 1;

                case "0":
                    return 0;
            }
            return 1;
        }
 
        private DataTable GetCarVideoInfo(string strPhone, string strGpsTime, string picDataType, string strReceTime)
        {
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@simnum", strPhone), new SqlParameter("@gpsTime", strGpsTime), new SqlParameter("@picDataType", picDataType), new SqlParameter("@receTime", strReceTime) };
            return new SqlDataAccess().getDataBySP("WebGpsClient_ReceVideoData", parameterArray);
        }

        private string method_2(int int_0)
        {
            if (int_0 == 0)
            {
                return "男";
            }
            if (int_0 == 1)
            {
                return "女";
            }
            return "未知";
        }

        private DataTable method_3()
        {
            DataTable table = new DataTable("GpsRectQueryCar");
            table.Columns.Add(new DataColumn("CarNum"));
            table.Columns.Add(new DataColumn("SimNum"));
            table.Columns.Add(new DataColumn("GpsTime"));
            table.Columns.Add(new DataColumn("StatuName"));
            table.Columns.Add(new DataColumn("Longitude"));
            table.Columns.Add(new DataColumn("Latitude"));
            table.Columns.Add(new DataColumn("Speed"));
            table.Columns.Add(new DataColumn("CarStatus"));
            table.Columns.Add(new DataColumn("AlarmType"));
            table.Columns.Add(new DataColumn("AccOn"));
            table.Columns.Add(new DataColumn("IsFill"));
            table.Columns.Add(new DataColumn("GpsValid"));
            table.Columns.Add(new DataColumn("CarId"));
            table.Columns.Add(new DataColumn("AreaName"));
            table.Columns.Add(new DataColumn("SvrTime"));
            return table;
        }

        public int SaveCarCmdParam(int int_0, string string_0, string string_1, string string_2, string string_3, string string_4, string string_5)
        {
            string str = "WebGpsClient_InsertCmdLog";
            SqlParameter[] parameterArray = new SqlParameter[7];
            parameterArray[0] = new SqlParameter("@WrkID", int_0);
            parameterArray[1] = new SqlParameter("@UserID", string_1);
            parameterArray[2] = new SqlParameter("@CmdType", string_2);
            parameterArray[3] = new SqlParameter("@OrderCode", string_3);
            if (string.IsNullOrEmpty(string_4))
            {
                parameterArray[4] = new SqlParameter("@OptCodeDetail", DBNull.Value);
            }
            else
            {
                parameterArray[4] = new SqlParameter("@OptCodeDetail", string_4);
            }
            parameterArray[5] = new SqlParameter("@CarIDandOrderId", string_0);
            parameterArray[6] = new SqlParameter("@CmdContent", string_5);
            SqlDataAccess access = new SqlDataAccess();
            return access.updateBySp(str, parameterArray);
        }

        public int SaveCarSetParam(int int_0, string string_0, string string_1, string string_2)
        {
            string str = "Appsvr_GpsCarSetParam";
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@WrkID", int_0), new SqlParameter("@CarParamInfo", string_0), new SqlParameter("@MsgType", string_1), new SqlParameter("@Param", string_2) };
            SqlDataAccess access = new SqlDataAccess();
            return access.updateBySp(str, parameterArray);
        }

        public int SetCarPathAlarm_Platform(string string_0, DataTable dataTable_0)
        {
            int num;
            try
            {
                if (dataTable_0 == null)
                {
                    return 1;
                }
                string format = "Insert Into GpsJtbCarPathAlarm_Platform(CarID,PathID,BeginTime,EndTime)Values('{0}','{1}','{2}','{3}')\r\n";
                StringBuilder builder = new StringBuilder();
                builder.Append("begin tran\r\n");
                builder.Append("Delete From GpsJtbCarPathAlarm_Platform Where CarID='" + string_0 + "'\r\n ");
                foreach (DataRow row in dataTable_0.Rows)
                {
                    builder.Append(string.Format(format, new object[] { string_0, row["PathID"], row["BeginTime"], row["EndTime"] }));
                }
                builder.Append("if(@@error<>0) begin rollback select 0 Result end else begin commit select 1 Result end ");
                num = this.ExecNoQuery(builder.ToString());
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return num;
        }

        public int SetCarPathSegmentAlarm_Platform(string string_0, object object_0)
        {
            int num2;
            int num = 0;
            try
            {
                string format = "Insert Into GpsJtbCarPathSegmentAlarm_Platform(CarID)Values('{0}')\r\n";
                StringBuilder builder = new StringBuilder();
                builder.Append("begin tran\r\n");
                builder.Append("Delete From GpsJtbCarPathSegmentAlarm_Platform Where CarID='" + string_0 + "'\r\n ");
                if (Convert.ToInt32(object_0) == 1)
                {
                    builder.Append(string.Format(format, string_0));
                }
                builder.Append("if(@@error<>0) begin rollback select 0 Result end else begin commit select 1 Result end ");
                this.ExecSql(builder.ToString());
                return num;
            }
            catch (Exception)
            {
                num2 = 1;
            }
            return num2;
        }

        public int setCarPicTimeParam(string string_0, CaptureEx captureEx_0, string string_1)
        {
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@SimNum", string_0), new SqlParameter("@IsMultiFrame", captureEx_0.IsMultiFrame), new SqlParameter("@CamerasID", captureEx_0.CamerasID), new SqlParameter("@CaptureFlag", captureEx_0.CaptureFlag), new SqlParameter("@CapTureMask", captureEx_0.CaptureCache), new SqlParameter("@Times", captureEx_0.Times), new SqlParameter("@CatchInterval", captureEx_0.Interval), new SqlParameter("@Quality", captureEx_0.Quality), new SqlParameter("@Brightness", captureEx_0.Brightness), new SqlParameter("@Contrast", captureEx_0.Contrast), new SqlParameter("@Saturation", captureEx_0.Saturation), new SqlParameter("@Chroma", captureEx_0.Chroma), new SqlParameter("@capWhenStop", captureEx_0.CapWhenStop), new SqlParameter("@PicTime", string_1) };
            SqlDataAccess access = new SqlDataAccess();
            return access.insertBySp("WebGpsClient_UpdateCarPicParam", parameterArray);
        }

        public int SetCarRegionAlarm_Platform(string string_0, DataTable dataTable_0)
        {
            int num2;
            int num = 0;
            try
            {
                string format = "Insert Into GpsJtbCarRegionAlarm_Platform(CarID,RegionID,RegionType,BeginTime,EndTime)Values('{0}','{1}','{2}','{3}','{4}')\r\n";
                StringBuilder builder = new StringBuilder();
                builder.Append("begin tran\r\n");
                builder.Append("Delete From GpsJtbCarRegionAlarm_Platform Where CarID='" + string_0 + "'\r\n ");
                foreach (DataRow row in dataTable_0.Rows)
                {
                    builder.Append(string.Format(format, new object[] { string_0, row["RegionID"], row["RegionType"], row["BeginTime"], row["EndTime"] }));
                }
                builder.Append("if(@@error<>0) begin rollback select 0 Result end else begin commit select 1 Result end ");
                this.ExecSql(builder.ToString());
                return num;
            }
            catch (Exception)
            {
                num2 = 1;
            }
            return num2;
        }

        public int SetCriticalRegionToTmp(string string_0, int int_0, int int_1, string string_1, string string_2, string string_3)
        {
            try
            {
                return this.ExecNoQuery(string.Format("AppSvr_UpdateCriticalRegion_tmp {0}, {1}, {2}, {3}, {4}, {5}", new object[] { int_0, int_1, string_0, string_1, string_2, string_3 }));
            }
            catch (Exception)
            {
                return 1;
            }
        }

        public int UpdateCarconfigOnDuty(string string_0, int int_0, int int_1, int int_2)
        {
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@carid", string_0), new SqlParameter("@isOnDuty", int_0), new SqlParameter("@isCloseGsm", int_1), new SqlParameter("@isCloseDial", int_2) };
            string str = "WebGpsClient_Updategpscarconfig";
            SqlDataAccess access = new SqlDataAccess();
            return access.updateBySp(str, parameterArray);
        }

        public int UpdateGisCarCommandTime(string string_0)
        {
            string format = "update giscar set CommandEnableTime = null where carID= {0} and CommandEnableTime > getdate()";
            format = string.Format(format, string_0);
            SqlDataAccess access = new SqlDataAccess();
            return access.updateBySql(format);
        }

        public int UpdateImportWatch(string string_0, string string_1, int int_0)
        {
            string format = " update GpsCarFilter set isImportWatch = {0} where wrkid={1}";
            if (!string.IsNullOrEmpty(string_1))
            {
                format = string.Format(format, int_0, string_0);
                string[] strArray = string_1.Split(new char[] { ',' });
                format = string.Format(format + " and phone = '{0}' ", strArray[0]);
                for (int i = 1; i < strArray.Length; i++)
                {
                    format = format + string.Format(" or phone = '{0}' ", strArray[i]);
                }
            }
            else
            {
                format = string.Format(format, 0, string_0);
            }
            return new SqlDataAccess().updateBySql(format);
        }

        public int UpdatePathAlarm(string string_0, string string_1, int int_0, int int_1, int int_2, int int_3, string string_2, string string_3, int int_4, int int_5, int int_6)
        {
            string str = "AppSvr_UpdatePathParam";
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@carID", string_0), new SqlParameter("@pathName", string_1), new SqlParameter("@choose", int_0), new SqlParameter("@topSpeed", int_1), new SqlParameter("@holdTime", int_2), new SqlParameter("@NewPathID", int_3), new SqlParameter("@BeginTime", string_2), new SqlParameter("@EndTime", string_3), new SqlParameter("@PathFlag", int_4), new SqlParameter("@DriEnough", int_5), new SqlParameter("@DriNoEnough", int_6) };
            SqlDataAccess access = new SqlDataAccess();
            return access.updateBySp(str, parameterArray);
        }

        public int UpdateRegionParam(int int_0, int int_1, int int_2, int int_3, int int_4, float float_0, float float_1, int int_5, string string_0, string string_1, int int_6, string string_2, string string_3, string string_4, int int_7, int int_8, int? nullable_0, int? nullable_1)
        {
            string str = "AppSvr_UpdateRegionParam_tmp";
            SqlParameter[] parameterArray = new SqlParameter[0x12];
            parameterArray[0] = new SqlParameter("@WrkID ", int_0);
            parameterArray[1] = new SqlParameter("@OrderID ", int_1);
            parameterArray[2] = new SqlParameter("@carID ", int_2);
            parameterArray[3] = new SqlParameter("@regionID ", int_3);
            parameterArray[4] = new SqlParameter("@param ", int_4);
            parameterArray[5] = new SqlParameter("@toEndTime ", float_0);
            parameterArray[6] = new SqlParameter("@toBackTime ", float_1);
            parameterArray[7] = new SqlParameter("@regionType ", int_5);
            parameterArray[8] = new SqlParameter("@time1 ", string_0);
            parameterArray[9] = new SqlParameter("@time2 ", string_1);
            parameterArray[10] = new SqlParameter("@regionFeature ", int_6);
            parameterArray[11] = new SqlParameter("@AlarmCondition", string_2);
            parameterArray[12] = new SqlParameter("@planUpTime ", string_3);
            parameterArray[13] = new SqlParameter("@planDownTime ", string_4);
            parameterArray[14] = new SqlParameter("@NewRegionID ", int_7);
            parameterArray[15] = new SqlParameter("@AlarmFlag ", int_8);
            if (!nullable_0.HasValue)
            {
                parameterArray[0x10] = new SqlParameter("@MaxSpeed ", DBNull.Value);
            }
            else
            {
                parameterArray[0x10] = new SqlParameter("@MaxSpeed ", nullable_0);
            }
            if (!nullable_1.HasValue)
            {
                parameterArray[0x11] = new SqlParameter("@HodeTime ", DBNull.Value);
            }
            else
            {
                parameterArray[0x11] = new SqlParameter("@HodeTime ", nullable_1);
            }
            SqlDataAccess access = new SqlDataAccess();
            return access.updateBySp(str, parameterArray);
        }

        public int UpdateTrafficPathAlarm_tmp(int int_0, int int_1, string string_0, string string_1, int int_2, int int_3, int int_4, int int_5, string string_2, string string_3, int int_6, int int_7, int int_8)
        {
            string str = "AppSvr_UpdateTrafficPathAlarm_tmp";
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@WrkID", int_0), new SqlParameter("@OrderID", int_1), new SqlParameter("@carID", string_0), new SqlParameter("@pathName", string_1), new SqlParameter("@choose", int_2), new SqlParameter("@topSpeed", int_3), new SqlParameter("@holdTime", int_4), new SqlParameter("@NewPathID", int_5), new SqlParameter("@BeginTime", string_2), new SqlParameter("@EndTime", string_3), new SqlParameter("@PathFlag", int_6), new SqlParameter("@DriEnough", int_7), new SqlParameter("@DriNoEnough", int_8) };
            SqlDataAccess access = new SqlDataAccess();
            return access.updateBySp(str, parameterArray);
        }
    }
}


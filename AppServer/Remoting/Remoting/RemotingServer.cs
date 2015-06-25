using Contract;
namespace Remoting
{
    using PublicClass;
    using ParamLibrary.Application;
    using ParamLibrary.Bussiness;
    using ParamLibrary.CarEntity;
    using ParamLibrary.CmdParamInfo;
    using ParamLibrary.Entity;
    using ParamLibrary.GpsEntity;
    //using GAS;
    //using Library;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Runtime.Remoting.Lifetime;
    using System.Threading;
    using Bussiness;
    using Library;

    public class RemotingServer : MarshalByRefObject, IRemotingServer
    {
        private RemotingDataBS _DataBSList;
        private RemotingDataCS _DataCSList;
        public OnlineUserInfo _OnlineUserInfo;
        public RemotingState _RemotingState;
        private LogHelper m_LogHelper;
        private Alarm myAlarm;
        private Car myCar;
        private DownData myDownData;
        private DownDataXCJLY myDownDataDB44;
        private DownDataPassThrough myDownDataPass;
        private DownDataFJYD myDownDataYD;

        public RemotingServer(UserInfoEntity userInfo, string UserId, int ModuleId)
        {
            this._RemotingState = new RemotingState();
            this.myAlarm = new Alarm();
            this.myCar = new Car();
            this.m_LogHelper = new LogHelper();
            this._DataCSList = new RemotingDataCS();
            this._DataBSList = new RemotingDataBS();
            this._OnlineUserInfo = new OnlineUserInfo(userInfo.WorkId, UserId, userInfo.GroupId, ModuleId, userInfo.AllowSelMutil, userInfo.AllowEmptyPw, userInfo.SudoOverDue, userInfo.RoadTransportID, userInfo.AreaCode);
            this.myDownData = new DownData(userInfo.WorkId, userInfo.AllowEmptyPw, userInfo.SudoOverDue, userInfo.AllowSelMutil, this._OnlineUserInfo);
            this.myDownDataYD = new DownDataFJYD(userInfo.WorkId, userInfo.AllowEmptyPw, userInfo.SudoOverDue, userInfo.AllowSelMutil, this._OnlineUserInfo);
            this.myDownDataDB44 = new DownDataXCJLY(userInfo.WorkId, userInfo.AllowEmptyPw, userInfo.SudoOverDue, userInfo.AllowSelMutil, this._OnlineUserInfo);
            this.myDownDataPass = new DownDataPassThrough(userInfo.WorkId, userInfo.AllowEmptyPw, userInfo.SudoOverDue, userInfo.AllowSelMutil, this._OnlineUserInfo);
        }

        public RemotingServer(int workId, string UserId, int GroupId, int ModuleId, bool AllowSelMutil, bool AllowEmptyPw, bool SudoOverDue)
        {
            this._RemotingState = new RemotingState();
            this.myAlarm = new Alarm();
            this.myCar = new Car();
            this.m_LogHelper = new LogHelper();
            this._DataCSList = new RemotingDataCS();
            this._DataBSList = new RemotingDataBS();
            this._OnlineUserInfo = new OnlineUserInfo(workId, UserId, GroupId, ModuleId, AllowSelMutil, AllowEmptyPw, SudoOverDue, "");
            this.myDownData = new DownData(workId, AllowEmptyPw, SudoOverDue, AllowSelMutil, this._OnlineUserInfo);
            this.myDownDataYD = new DownDataFJYD(workId, AllowEmptyPw, SudoOverDue, AllowSelMutil, this._OnlineUserInfo);
            this.myDownDataDB44 = new DownDataXCJLY(workId, AllowEmptyPw, SudoOverDue, AllowSelMutil, this._OnlineUserInfo);
            this.myDownDataPass = new DownDataPassThrough(workId, AllowEmptyPw, SudoOverDue, AllowSelMutil, this._OnlineUserInfo);
        }

        public int Alarm_DelPath(int iPathId, string sPathName)
        {
            try
            {
                return this.myAlarm.DelPath(iPathId, sPathName);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("Alarm", "DelPath", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return -1;
            }
        }

        public bool Alarm_DelPathCheckAuth(int iPathId, string sPathName)
        {
            try
            {
                return this.myAlarm.DelPathCheckAuth(iPathId, sPathName, this._OnlineUserInfo.UserId);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("Alarm", "DelPathCheckAuth", exception.Message);
                this.m_LogHelper.WriteError(msg);
            }
            return false;
        }

        public int Alarm_DelRegion(string sPathName)
        {
            try
            {
                return this.myAlarm.DelRegion(sPathName);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("Alarm", "DelRegion", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return -1;
            }
        }

        public bool Alarm_DelRegionCheckAuth(string sRegioinName)
        {
            try
            {
                return this.myAlarm.DelRegionCheckAuth(sRegioinName, this._OnlineUserInfo.UserId);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("Alarm", "DelRegionCheckAuth", exception.Message);
                this.m_LogHelper.WriteError(msg);
            }
            return false;
        }

        public DataTable Alarm_GetGroupType()
        {
            try
            {
                return this.myAlarm.GetGroupType(this._OnlineUserInfo.UserId);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("Alarm", "Alarm_ShowGroupType", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public DataTable Alarm_GetPathInfo()
        {
            try
            {
                return this.myAlarm.GetPathInfo(this._OnlineUserInfo.UserId);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("Alarm", "GetPathInfo", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public DataTable Alarm_GetPathSegmentInfo()
        {
            try
            {
                return this.myAlarm.GetPathSegmentInfo(this._OnlineUserInfo.UserId);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("Alarm", "GetPathInfo", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public DataTable Alarm_GetRegionInfo()
        {
            try
            {
                return this.myAlarm.GetRegionInfo(this._OnlineUserInfo.UserId);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("Alarm", "GetRegionInfo", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public int Alarm_InsertPathRelatedType(int pathGroupId, int pathId)
        {
            try
            {
                return this.myAlarm.InsertPathRelatedType(pathGroupId, pathId);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("Alarm", "InsertRelatedType", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return -1;
            }
        }

        public int Alarm_InsertRelatedType(int pathGroupId, int regionId)
        {
            try
            {
                return this.myAlarm.InsertRelatedType(pathGroupId, regionId);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("Alarm", "InsertRelatedType", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return -1;
            }
        }

        public int Alarm_InsertRelatedType(string[] pathGroupIdLst, int pathId)
        {
            try
            {
                return this.myAlarm.InsertRelatedType(pathGroupIdLst, pathId);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("Alarm", "InsertRelatedType", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return -1;
            }
        }

        public DataTable Alarm_PreSetPath(string pathStr, string pathName, int pathType, int region_Radius, string factoryName, double lon_Factory, double lat_Factory, string buildingSitName, double lon_BuildingSit, double lat_BuildingSit)
        {
            try
            {
                return this.myAlarm.PreSetPath(pathStr, pathName, pathType, region_Radius, factoryName, lon_Factory, lat_Factory, buildingSitName, lon_BuildingSit, lat_BuildingSit);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("Alarm", "PreSetPath", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public DataTable Alarm_PreSetPathEx(string pathStr, string pathName, int pathType, int region_Radius, string factoryName, double lon_Factory, double lat_Factory, string buildingSitName, double lon_BuildingSit, double lat_BuildingSit, string remark, string[][] PathSegments)
        {
            try
            {
                return this.myAlarm.PreSetPathEx(pathStr, pathName, pathType, region_Radius, factoryName, lon_Factory, lat_Factory, buildingSitName, lon_BuildingSit, lat_BuildingSit, remark, PathSegments);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("Alarm", "PreSetPathEx", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public DataTable Alarm_PreSetRegion(string regionStr, string regionName, int iRegionFeature)
        {
            try
            {
                return this.myAlarm.PreSetRegion(regionStr, regionName, iRegionFeature);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("Alarm", "PreSetRegion", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public DataTable Alarm_ShowGroupType()
        {
            try
            {
                return this.myAlarm.ShowGroupType(this._OnlineUserInfo.UserId);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("Alarm", "Alarm_ShowGroupType", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public Response Alarm_UdateRegionDot(string regionId, string regionDot)
        {
            Response response = new Response();
            try
            {
                if (this.myAlarm.UpdateRegionType(regionId, regionDot) != 0)
                {
                    response.ResultCode = 0L;
                    return response;
                }
                response.ErrorMsg = "更新区域失败！";
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "UpdateRegionType", exception.Message);
                this.m_LogHelper.WriteError(msg);
                response.ErrorMsg = exception.Message;
            }
            return response;
        }

        public Response Alarm_UdateRegionDot(string regionId, string regionDot, string RegionName, int pathgroupID)
        {
            Response response = new Response();
            try
            {
                if (this.myAlarm.UpdateRegionType(regionId, regionDot, RegionName, pathgroupID) != 0)
                {
                    response.ResultCode = 0L;
                    return response;
                }
                response.ErrorMsg = "更新区域失败！";
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "UpdateRegionType", exception.Message);
                this.m_LogHelper.WriteError(msg);
                response.ErrorMsg = exception.Message;
            }
            return response;
        }

        public DataSet Area_getAllTreeInfo()
        {
            try
            {
                Area area = new Area(this._OnlineUserInfo.UserId);
                return area.GetAllTreeInfo(this._OnlineUserInfo.WorkId.ToString());
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("Alarm", "Area_getAllTreeInfo", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public byte[] Area_getAllTreeInfoComPress()
        {
            try
            {
                DataSet allTreeInfo = new Area(this._OnlineUserInfo.UserId).GetAllTreeInfo(this._OnlineUserInfo.WorkId.ToString());
                DataTable table = null;
                DataTable table2 = null;
                DataTable table3 = null;
                if ((allTreeInfo != null) && (allTreeInfo.Tables.Count > 0))
                {
                    table = allTreeInfo.Tables[0];
                    if (allTreeInfo.Tables.Count > 1)
                    {
                        table2 = allTreeInfo.Tables[1];
                    }
                    if (allTreeInfo.Tables.Count > 2)
                    {
                        table3 = allTreeInfo.Tables[2];
                    }
                }
                List<GpsDataTable> list = new List<GpsDataTable>();
                if (table != null)
                {
                    Dictionary<string, int> columns = new Dictionary<string, int>();
                    for (int i = 0; i < table.Columns.Count; i++)
                    {
                        columns.Add(table.Columns[i].ColumnName, i);
                    }
                    GpsDataTable item = new GpsDataTable(columns);
                    foreach (DataRow row in table.Rows)
                    {
                        item.InsertRows(row.ItemArray);
                    }
                    list.Add(item);
                }
                if (table2 != null)
                {
                    Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
                    for (int j = 0; j < table2.Columns.Count; j++)
                    {
                        dictionary2.Add(table2.Columns[j].ColumnName, j);
                    }
                    GpsDataTable table5 = new GpsDataTable(dictionary2);
                    foreach (DataRow row2 in table2.Rows)
                    {
                        table5.InsertRows(row2.ItemArray);
                    }
                    list.Add(table5);
                }
                if (table3 != null)
                {
                    Dictionary<string, int> dictionary3 = new Dictionary<string, int>();
                    for (int k = 0; k < table3.Columns.Count; k++)
                    {
                        dictionary3.Add(table3.Columns[k].ColumnName, k);
                    }
                    GpsDataTable table6 = new GpsDataTable(dictionary3);
                    foreach (DataRow row3 in table3.Rows)
                    {
                        table6.InsertRows(row3.ItemArray);
                    }
                    list.Add(table6);
                }
                return CompressHelper.CompressToSelf(list);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("Alarm", "Area_getAllTreeInfo", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public DataTable Area_getAreaInfoAll()
        {
            try
            {
                Area area = new Area(this._OnlineUserInfo.UserId);
                return area.GetAreaInfoAll();
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "getAllAreaInfosByParentId", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public byte[] Area_getAreaInfoAllByCompress()
        {
            try
            {
                Area area = new Area(this._OnlineUserInfo.UserId);
                return CompressHelper.Compress(area.GetAreaInfoAll());
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "getAllAreaInfosByParentId", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public byte[] Area_getAreaInfoAllExByCompress()
        {
            try
            {
                Area area = new Area(this._OnlineUserInfo.UserId);
                return CompressHelper.Compress(area.GetAreaInfoAllEx());
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "Area_getAreaInfoAllExByCompress", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public DataTable Area_GetUserAreaInfo()
        {
            try
            {
                Area area = new Area(this._OnlineUserInfo.UserId);
                return area.GetUserAreaInfo();
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("Alarm", "Area_GetUserAreaInfo", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public Response Car_CommandParameterInsterToDB(CmdParam.ParamType ParamType, string CarValues, string CarPw, SimpleCmd cmdParameter, string cmdContent, string desc)
        {
            try
            {
                return this.myDownDataYD.Car_CommandParameterInsterToDB(this.myCar, ParamType, CarValues, CarPw, cmdParameter, cmdContent, desc);
            }
            catch (Exception exception)
            {
                this.m_LogHelper.WriteError(new ErrorMsg("RemotingServer", "Car_CommandParameterInsterToDB", exception.ToString()));
                return new Response();
            }
        }

        public int Car_DeletePathAlarm(string strCarId)
        {
            try
            {
                return this.myCar.DeletePathAlarm(strCarId);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("Car", "DeletePathAlarm", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return -1;
            }
        }

        public DataTable Car_GetAlarmPathDotFromGisCar(string strCarId)
        {
            try
            {
                return this.myCar.GetAlarmPathDotFromGisCar(strCarId);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "GetAlarmPathDotFromGisCar", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public DataTable Car_GetCaptureMoniterDataByCarId(string strCarId)
        {
            try
            {
                return this.myCar.GetCaptureMoniterDataByCarId(strCarId);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "GetCaptureMoniterDataByCarId", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public DataTable Car_GetCarAlarmRegionInfo(string strCarId)
        {
            try
            {
                return this.myCar.GetCarAlarmRegionInfo(strCarId);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "GetCarAlarmRegionInfo", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public DataTable Car_GetCarAlarmState(string strCarId)
        {
            try
            {
                return this.myCar.GetCarAlarmState(strCarId);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "GetCarAlarmState", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public DataTable Car_GetCarConfig(string sCarId)
        {
            try
            {
                return this.myCar.GetCarConfig(sCarId);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "GetCarConfig", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public CommonCar Car_GetCarDetailInfoByCarId(string CarId)
        {
            try
            {
                return this.myCar.GetCarDetailInfoByCarId(CarId);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "GetCarDetailInfoByCarId", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public DataTable Car_GetCarImgInfo(string strPhone, string strGpsTime, string strCaremaId)
        {
            try
            {
                return this.myCar.GetCarImgInfo(strPhone, strGpsTime, strCaremaId, null);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "GetCarImgInfo", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public DataTable Car_GetCarImgInfo(string strPhone, string strGpsTime, string strCaremaId, string strReceTime)
        {
            try
            {
                return this.myCar.GetCarImgInfo(strPhone, strGpsTime, strCaremaId, strReceTime);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "GetCarImgInfo", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public DataTable Car_GetCarList(int iPage, int iRecsPerPage)
        {
            try
            {
                return this.myCar.GetCarList(this._OnlineUserInfo.UserId, iPage, iRecsPerPage);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("Car", "GetCarList", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public byte[] Car_GetCarListByCompress(int iPage, int iRecsPerPage)
        {
            try
            {
                return CompressHelper.Compress(this.myCar.GetCarList(this._OnlineUserInfo.UserId, iPage, iRecsPerPage));
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("Car", "GetCarList", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public DataTable Car_GetCarListEx(int iPage, int iRecsPerPage)
        {
            try
            {
                return this.myCar.GetCarListEx(this._OnlineUserInfo.UserId, iPage, iRecsPerPage);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("Car", "GetCarListEx", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public byte[] Car_GetCarListExByCompress(int iPage, int iRecsPerPage)
        {
            try
            {
                return CompressHelper.Compress(this.myCar.GetCarListEx(this._OnlineUserInfo.UserId, iPage, iRecsPerPage));
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("Car", "Car_GetCarListExByCompress", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public DataTable Car_GetCarMutilVideoInfo(string strPhone, string strGpsTime, string strCaremaId, string picDataType)
        {
            try
            {
                return this.myCar.GetCarMutilVideoInfo(strPhone, strGpsTime, strCaremaId, picDataType, null);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "Car_GetCarMutilVideoInfo", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public DataTable Car_GetCarMutilVideoInfo(string strPhone, string strGpsTime, string strCaremaId, string picDataType, string strReceTime)
        {
            try
            {
                return this.myCar.GetCarMutilVideoInfo(strPhone, strGpsTime, strCaremaId, picDataType, strReceTime);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "Car_GetCarMutilVideoInfo", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public DataTable Car_GetDevice(string sCarId)
        {
            try
            {
                return this.myCar.GetDeviceType(sCarId);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "GetDeviceType", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public DataTable Car_GetDeviceShareRef()
        {
            try
            {
                return this.myCar.GetDeviceShareRef();
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "GetDeviceShareRef", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public DataTable Car_GetImportWatchCarInfo()
        {
            try
            {
                return this.myCar.GetImportWatchCarInfo(this._OnlineUserInfo.WorkId);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "GetImportWatchCarInfo", exception.Message + exception.StackTrace);
                this.m_LogHelper.WriteError(msg);
                return new DataTable();
            }
        }

        public DataTable Car_GetInterestPointMulti(string strMapType, int iPoiAutn)
        {
            try
            {
                return this.myCar.GetInterestPointMulti(this._OnlineUserInfo.UserId, strMapType, iPoiAutn);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "GetInterestPointMulti", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public DataTable Car_GetInterestPointSingle(string strMapType, int iPoiAutn)
        {
            try
            {
                return this.myCar.GetInterestPointSingle(this._OnlineUserInfo.AreaCode, strMapType, iPoiAutn);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "GetInterestPointSingle", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public DataTable Car_GetMapType()
        {
            try
            {
                return this.myCar.GetMapType();
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "GetMapType", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public DataTable Car_GetNewPathId(string strCarId, string strPathName, int iMaxPathId)
        {
            try
            {
                return this.myCar.GetNewPathId(strCarId, strPathName, iMaxPathId);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "GetNewPathId", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public DataTable Car_GetNewRegionId(string strCarId, string strRegionName, int iMaxRegionId)
        {
            try
            {
                return this.myCar.GetNewRegionId(strCarId, strRegionName, iMaxRegionId);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "Car_GetNewRegionId", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public DataTable Car_GetPassWayPathID(string strCarId)
        {
            try
            {
                return this.myCar.Car_GetPassWayPathID(strCarId);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "Car_GetPassWayPathID", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public DataTable Car_GetPathAlarm(string strCarId)
        {
            try
            {
                return this.myCar.GetPathAlarm(this._OnlineUserInfo.UserId, strCarId);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "GetPathAlarm", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public DataTable Car_GetPathAlarmAnother(string strCarId)
        {
            try
            {
                return this.myCar.GetPathAlarmAnother(this._OnlineUserInfo.UserId, strCarId);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "GetPathAlarmAnother", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public DataTable Car_GetPathRouteByPathName(string strPathNames)
        {
            try
            {
                return this.myCar.GetPathRouteByPathName(strPathNames);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "GetPathRouteByPathName", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public DataTable Car_GetPathSegmentAlarm(string strCarId)
        {
            try
            {
                return this.myCar.GetPathSegmentAlarm(this._OnlineUserInfo.UserId, strCarId);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "GetPathAlarm", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public DataTable Car_GetPhonesByType(CmdParam.PhoneType phoneType, string sCarID)
        {
            try
            {
                return this.myCar.GetPhonesByType(phoneType, sCarID);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "GetPhonesByType", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public DataTable Car_GetPOIAuth()
        {
            try
            {
                return this.myCar.GetPOIAuth();
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "GetPOIAuth", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public DataTable Car_GetRefRegion(double Xmin, double Ymin, double Xmax, double Ymax)
        {
            try
            {
                return this.myCar.GetRefRegion(Xmin, Ymin, Xmax, Ymax, this._OnlineUserInfo.AreaCode, this._OnlineUserInfo.UserId);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "GetRefRegion", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public DataTable Car_GetRegionInfo(string strCarId, int iRegionFeature)
        {
            try
            {
                return this.myCar.GetRegionInfo(this._OnlineUserInfo.UserId, strCarId, iRegionFeature);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "GetRegionInfo", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public int Car_InsertPathIdsIntoGisCar(string carID, string orderID, string delPathID)
        {
            try
            {
                return this.myCar.InsertPathIdsIntoGisCar(carID, this._OnlineUserInfo.WorkId.ToString(), orderID, delPathID);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("Car", "InsertPathIdsIntoGisCar", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return -1;
            }
        }

        public int Car_InsertPhonesIntoGisCar(CmdParam.PhoneType phoneType, string CarID, string wrkId, string OrderID, string phoneValue)
        {
            try
            {
                return this.myCar.InsertPhonesIntoGisCar(phoneType, CarID, wrkId, OrderID, phoneValue);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("Car", "InsertPhonesIntoGisCar", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return -1;
            }
        }

        public int Car_SaveFormCmdParam(string sCarParamInfo, string sCmdType, string sCmdContent)
        {
            try
            {
                return this.myCar.SaveCarCmdParam(this._OnlineUserInfo.WorkId, sCarParamInfo, this._OnlineUserInfo.UserId, sCmdType, this.myDownData.m_OrderCode, "", sCmdContent);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "Car_SaveCarCmdParam", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return -1;
            }
        }

        public int Car_SaveFormSetParam(string sCarParamInfo, string sMsgType, string sParam)
        {
            try
            {
                return this.myCar.SaveCarSetParam(this._OnlineUserInfo.WorkId, sCarParamInfo, sMsgType, sParam);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "Car_SaveCarSetParam", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return -1;
            }
        }

        public Response Car_SetCarPicTimeParam(string sSimNum, CaptureEx m_CaptureEx, string sPicTime)
        {
            Response response = new Response();
            try
            {
                if (this.myCar.setCarPicTimeParam(sSimNum, m_CaptureEx, sPicTime) > 0)
                {
                    response.ResultCode = 0L;
                }
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "Car_SetCarPicTimeParam", exception.Message);
                this.m_LogHelper.WriteError(msg);
                response.ErrorMsg = exception.Message;
            }
            return response;
        }

        public Response Car_SmallArea(string sLongtide, string sLatitude, string sDis, TxtMsg MsgContext)
        {
            try
            {
                return this.myDownData.icar_SmallArea(sLongtide, sLatitude, sDis, MsgContext, this._OnlineUserInfo.AreaCode, this._OnlineUserInfo.UserId);
            }
            catch (Exception exception)
            {
                new LogHelper().WriteError(new ErrorMsg("RemotingServer", "Car_SmallArea", exception.ToString() + exception.Message));
                return new Response();
            }
        }

        public Response Car_SmallArea_FJYD(string sLeftLon, string sLeftLat, string sRightLon, string sRightLat, string sResWay, string stelNumber, TxtMsg MsgContext, CmdParam.CommMode CommMode)
        {
            try
            {
                return this.myDownDataYD.icar_SmallArea(sLeftLon, sLeftLat, sRightLon, sRightLat, sResWay, stelNumber, MsgContext, this._OnlineUserInfo.AreaCode, this._OnlineUserInfo.UserId, CommMode);
            }
            catch (Exception exception)
            {
                new LogHelper().WriteError(new ErrorMsg("RemotingServer", "Car_SmallArea_FJYD", exception.ToString() + exception.Message));
                return new Response();
            }
        }

        public int Car_UpdateImportWatch(string sPhone, int iFlag)
        {
            try
            {
                return this.myCar.UpdateImportWatch(this._OnlineUserInfo.WorkId.ToString(), sPhone, iFlag);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("Car", "UpdateImportWatch", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return -1;
            }
        }

        public int Car_UpdatePathAlarm(string strCarId, string strPathName, int iChoose, int iTopSpeed, int iHoldTime, int iNewPathId, string strBeginTime, string strEndTime)
        {
            try
            {
                return this.myCar.UpdatePathAlarm(strCarId, strPathName, iChoose, iTopSpeed, iHoldTime, iNewPathId, strBeginTime, strEndTime, 0, 0, 0);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("Car", "UpdatePathAlarm", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return -1;
            }
        }

        public DataTable CarDataInfoBuffer_GetArarmCarList()
        {
            try
            {
                return null;
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "CarDataInfoBuffer_GetArarmCarList", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public byte[] CarDataInfoBuffer_GetArarmCarListByCompress()
        {
            try
            {
                return null;
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "CarDataInfoBuffer_GetArarmCarList", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public int CarFilter_SetCheckedCar(string AreaCodeOrCarId, bool isRegion, bool isAdd)
        {
            int num = -1;
            try
            {
                num = this._OnlineUserInfo.CarFilter.SetSelectCar(this._OnlineUserInfo.WorkId, AreaCodeOrCarId, isRegion, isAdd);
                if (num > 0)
                {
                    num = 0;
                }
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("Car", "CarFilter_SetCheckedCar", exception.Message + exception.Source);
                this.m_LogHelper.WriteError(msg);
            }
            return num;
        }

        public DataTable CarOil_GetOilBoxInfo(string sCarId)
        {
            CarOil oil = new CarOil();
            return oil.GetOilBoxInfo(sCarId);
        }

        public int CarOil_GetOilBoxVol(string sCarId)
        {
            CarOil oil = new CarOil();
            return oil.GetOilBoxVol(sCarId);
        }

        private DataTable CreateDataTable(DataTable sourceCarList)
        {
            DataTable table = new DataTable("gpsCall");
            table.Columns.Add(new DataColumn("simNum"));
            table.Columns.Add(new DataColumn("carNum"));
            if ((sourceCarList == null) || (sourceCarList.Rows.Count <= 0))
            {
                return table;
            }
            string str = string.Empty;
            Bussiness.CarInfo dataCarInfoBySimNum = null;
            foreach (DataRow row in sourceCarList.Rows)
            {
                str = row["telephone"] as string;
                dataCarInfoBySimNum = CarDataInfoBuffer.GetDataCarInfoBySimNum(str);
                if (dataCarInfoBySimNum != null)
                {
                    DataRow row2 = table.NewRow();
                    row2["simNum"] = str;
                    row2["carNum"] = dataCarInfoBySimNum.CarNum;
                    table.Rows.Add(row2);
                }
            }
            return table.Copy();
        }

        public DataTable DownData_GetCarInfoByArea_By_Circle(string longtide, string latitude, string radius)
        {
            try
            {
                DataTable sourceCarList = this.myDownDataYD.GetCarInfoByArea(longtide, latitude, radius, this._OnlineUserInfo.AreaCode, this._OnlineUserInfo.UserId);
                return this.CreateDataTable(sourceCarList);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg {
                    ClassName = "RemotingServer",
                    FunctionName = "DownData_GetCarInfoByArea_By_Circle",
                    ErrorText = exception.ToString() + exception.Message
                };
                new LogHelper().WriteError(msg);
                return null;
            }
        }

        public DataTable DownData_GetCarInfoByArea_By_Rectangle(string leftLon, string leftLat, string rightLon, string rightLat)
        {
            try
            {
                DataTable sourceCarList = this.myDownDataYD.GetCarInfoByArea(leftLon, leftLat, rightLon, rightLat, this._OnlineUserInfo.AreaCode, this._OnlineUserInfo.UserId);
                return this.CreateDataTable(sourceCarList);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg {
                    ClassName = "RemotingServer",
                    FunctionName = "DownData_GetCarInfoByArea_By_Rectangle",
                    ErrorText = exception.ToString() + exception.Message
                };
                new LogHelper().WriteError(msg);
                return null;
            }
        }

        public AppRespone DownData_icar_SendRawPackage(AppRequest pRequest, object pvArg)
        {
            try
            {
                return this.myDownData.icar_SendRawPackage(pRequest, pvArg);
            }
            catch (Exception exception)
            {
                this.m_LogHelper.WriteError(new ErrorMsg("RemotingServer", "DownData_icar_SendRawPackage", exception.ToString()));
                return new AppRespone();
            }
        }

        public AppRespone DownData_icar_SetCommonCmd_Pass(AppRequest pAppRequest)
        {
            try
            {
                return this.myDownDataPass.icar_SendIOCommand(pAppRequest);
            }
            catch (Exception exception)
            {
                this.m_LogHelper.WriteError(new ErrorMsg("RemotingServer", "DownData_icar_SetCommonCmd_Pass", exception.ToString()));
                return new AppRespone();
            }
        }

        public Response DownData_icar_SetCommonCmd_XCJLY(CmdParam.ParamType ParamType, string CarValues, string CarPw, CmdParam.CommMode CommMode, SimpleCmd simpleCmd)
        {
            try
            {
                return this.myDownDataDB44.icar_SetCommonCmd(ParamType, CarValues, CarPw, CommMode, simpleCmd);
            }
            catch (Exception exception)
            {
                this.m_LogHelper.WriteError(new ErrorMsg("RemotingServer", "DownData_icar_SetCommonCmd_XCJLY", exception.ToString()));
                return new Response();
            }
        }

        public Response DownData_icar_SmallArea_FJYD(string leftLon, string leftLat, string rightLon, string rightLat, string tryWay, string stelNumber, ArrayList sendCarlist, TxtMsg MsgContext, CmdParam.CommMode commMode)
        {
            try
            {
                return this.myDownDataYD.icar_SmallArea(leftLon, leftLat, rightLon, rightLat, tryWay, stelNumber, sendCarlist, MsgContext, commMode);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg {
                    ClassName = "RemotingServer",
                    FunctionName = "DownData_icar_SmallArea_FJYD",
                    ErrorText = exception.ToString() + exception.Message
                };
                new LogHelper().WriteError(msg);
                return new Response();
            }
        }

        public Response DownData_RemoteDial(CmdParam.ParamType ParamType, string CarValues, string CarPw, CmdParam.CommMode CommMode, RemoteDial remoteDial)
        {
            try
            {
                return this.myDownData.icar_RemoteDial(ParamType, CarValues, CarPw, CommMode, remoteDial);
            }
            catch (Exception exception)
            {
                this.m_LogHelper.WriteError(new ErrorMsg("RemotingServer", "DownData_RemoteDial", exception.ToString()));
                return new Response();
            }
        }

        public Response DownData_RemoteUpdate(CmdParam.ParamType ParamType, string CarValues, string CarPw, CmdParam.CommMode CommMode)
        {
            try
            {
                return this.myDownData.icar_RemoteUpdate(ParamType, CarValues, CarPw, CommMode);
            }
            catch (Exception exception)
            {
                this.m_LogHelper.WriteError(new ErrorMsg("RemotingServer", "DownData_RemoteUpdate", exception.ToString()));
                return new Response();
            }
        }

        public Response DownData_SelMultiPathAlarm(CmdParam.ParamType ParamType, string CarValues, string CarPw, CmdParam.CommMode CommMode, PathAlarmList pathAlarmList)
        {
            try
            {
                return this.myDownData.icar_SelMultiPathAlarm(ParamType, CarValues, CarPw, CommMode, pathAlarmList);
            }
            catch (Exception exception)
            {
                this.m_LogHelper.WriteError(new ErrorMsg("RemotingServer", "DownData_SelMultiPathAlarm", exception.ToString()));
                return new Response();
            }
        }

        public Response DownData_SelMultiPathAlarm_FJYD(CmdParam.ParamType ParamType, string CarValues, string CarPw, CmdParam.CommMode CommMode, PathAlarmList pathAlarmList)
        {
            try
            {
                return this.myDownDataYD.icar_SelMultiPathAlarm(ParamType, CarValues, CarPw, CommMode, pathAlarmList);
            }
            catch (Exception exception)
            {
                this.m_LogHelper.WriteError(new ErrorMsg("RemotingServer", "DownData_SelMultiPathAlarm_FJYD", exception.ToString()));
                return new Response();
            }
        }

        public Response DownData_SendRawPackage(CmdParam.ParamType ParamType, string CarValues, string CarPw, CmdParam.CommMode CommMode, SimpleCmd simpleCmd)
        {
            try
            {
                return this.myDownDataYD.icar_SendRawPackage(ParamType, CarValues, CarPw, CommMode, simpleCmd);
            }
            catch (Exception exception)
            {
                this.m_LogHelper.WriteError(new ErrorMsg("RemotingServer", "DownData_SendRawPackage", exception.ToString()));
                return new Response();
            }
        }

        public Response DownData_SendTxtMsg(CmdParam.ParamType ParamType, string sCarValue, string sPw, CmdParam.CommMode CommMode, TxtMsg TxtMsg)
        {
            try
            {
                return this.myDownData.icar_SendTxtMsg(ParamType, sCarValue, sPw, CommMode, TxtMsg);
            }
            catch (Exception exception)
            {
                this.m_LogHelper.WriteError(new ErrorMsg("RemotingServer", "DownData_SendTxtMsg", exception.ToString()));
                return new Response();
            }
        }

        public Response DownData_SetAlarmFlag(CmdParam.ParamType ParamType, string CarValues, string CarPw, CmdParam.CommMode CommMode, AlarmEntity arlamArgs)
        {
            try
            {
                return this.myDownData.icar_SetAlarmFlag(ParamType, CarValues, CarPw, CommMode, arlamArgs);
            }
            catch (Exception exception)
            {
                this.m_LogHelper.WriteError(new ErrorMsg("RemotingServer", "DownData_SetAlarmFlag", exception.ToString()));
                return new Response();
            }
        }

        public Response DownData_SetBlackBox(CmdParam.ParamType ParamType, string CarValues, string CarPw, CmdParam.CommMode CommMode, BlackBox blackbox)
        {
            try
            {
                return this.myDownData.icar_SetBlackBox(ParamType, CarValues, CarPw, CommMode, blackbox);
            }
            catch (Exception exception)
            {
                this.m_LogHelper.WriteError(new ErrorMsg("RemotingServer", "DownData_SetBlackBox", exception.ToString()));
                return new Response();
            }
        }

        public Response DownData_SetCallLimit(CmdParam.ParamType ParamType, string CarValues, string CarPw, CmdParam.CommMode CommMode, CallLimit callLimit)
        {
            try
            {
                return this.myDownData.icar_SetCallLimit(ParamType, CarValues, CarPw, CommMode, callLimit);
            }
            catch (Exception exception)
            {
                this.m_LogHelper.WriteError(new ErrorMsg("RemotingServer", "DownData_SetCallLimit", exception.ToString()));
                return new Response();
            }
        }

        public Response DownData_SetCaptureEx(CmdParam.ParamType ParamType, string CarValues, string CarPw, CmdParam.CommMode CommMode, CaptureEx captureEx)
        {
            try
            {
                return this.myDownData.icar_SetCaptureEx(ParamType, CarValues, CarPw, CommMode, captureEx);
            }
            catch (Exception exception)
            {
                this.m_LogHelper.WriteError(new ErrorMsg("RemotingServer", "DownData_SetCaptureEx", exception.ToString()));
                return new Response();
            }
        }

        public Response DownData_SetCommArg(CmdParam.ParamType ParamType, string CarValues, string CarPw, CmdParam.CommMode CommMode, CommArgs commArgs)
        {
            try
            {
                return this.myDownData.icar_SetCommArg(ParamType, CarValues, CarPw, CommMode, commArgs);
            }
            catch (Exception exception)
            {
                this.m_LogHelper.WriteError(new ErrorMsg("RemotingServer", "DownData_SetCommArg", exception.ToString()));
                return new Response();
            }
        }

        public Response DownData_SetCommonCmd_FJYD(CmdParam.ParamType ParamType, string CarValues, string CarPw, CmdParam.CommMode CommMode, SimpleCmd simpleCmd)
        {
            try
            {
                return this.myDownDataYD.icar_SetCommonCmd(ParamType, CarValues, CarPw, CommMode, simpleCmd);
            }
            catch (Exception exception)
            {
                this.m_LogHelper.WriteError(new ErrorMsg("RemotingServer", "DownData_SetCommonCmd_FJYD", exception.ToString()));
                return new Response();
            }
        }

        public Response DownData_SetCustomAlarmer(CmdParam.ParamType ParamType, string CarValues, string CarPw, CmdParam.CommMode CommMode, AlarmEntity alarmEntity)
        {
            try
            {
                return this.myDownData.icar_SetCustomAlarmer(ParamType, CarValues, CarPw, CommMode, alarmEntity);
            }
            catch (Exception exception)
            {
                this.m_LogHelper.WriteError(new ErrorMsg("RemotingServer", "DownData_SetCustomAlarmer", exception.ToString()));
                return new Response();
            }
        }

        public Response DownData_SetLastDotQuery(CmdParam.ParamType ParamType, string CarValues, string CarPw, CmdParam.CommMode CommMode, CmdParam.OrderCode ordercode)
        {
            try
            {
                return this.myDownData.icar_SetLastDotQuery(ParamType, CarValues, CarPw, CommMode, ordercode);
            }
            catch (Exception exception)
            {
                this.m_LogHelper.WriteError(new ErrorMsg("RemotingServer", "DownData_SetLastDotQuery", exception.ToString()));
                return new Response();
            }
        }

        public Response DownData_SetMinSMSReportInterval(CmdParam.ParamType ParamType, string CarValues, string CarPw, CmdParam.CommMode CommMode, MinSMSReportInterval minSmsReport)
        {
            try
            {
                return this.myDownData.icar_SetMinSMSReportInterval(ParamType, CarValues, CarPw, CommMode, minSmsReport);
            }
            catch (Exception exception)
            {
                this.m_LogHelper.WriteError(new ErrorMsg("RemotingServer", "DownData_SetMinSMSReportInterval", exception.ToString()));
                return new Response();
            }
        }

        public Response DownData_SetMultiSegSpeedAlarm(CmdParam.ParamType ParamType, string CarValues, string CarPw, CmdParam.CommMode CommMode, PathAlarmList pathAlarmList)
        {
            try
            {
                return this.myDownData.icar_SetMultiSegSpeedAlarm(ParamType, CarValues, CarPw, CommMode, pathAlarmList);
            }
            catch (Exception exception)
            {
                this.m_LogHelper.WriteError(new ErrorMsg("RemotingServer", "DownData_SetMultiSegSpeedAlarm", exception.ToString()));
                return new Response();
            }
        }

        public Response DownData_SetPhone(CmdParam.ParamType ParamType, string CarValues, string CarPw, CmdParam.CommMode CommMode, SetPhone setPhone)
        {
            try
            {
                return this.myDownData.icar_SetPhone(ParamType, CarValues, CarPw, CommMode, setPhone);
            }
            catch (Exception exception)
            {
                this.m_LogHelper.WriteError(new ErrorMsg("RemotingServer", "DownData_SetPhone", exception.ToString()));
                return new Response();
            }
        }

        public Response DownData_SetPosReport(CmdParam.ParamType ParamType, string CarValues, string CarPw, CmdParam.CommMode CommMode, PosReport posReport)
        {
            try
            {
                return this.myDownData.icar_SetPosReport(ParamType, CarValues, CarPw, CommMode, posReport);
            }
            catch (Exception exception)
            {
                this.m_LogHelper.WriteError(new ErrorMsg("RemotingServer", "DownData_SetPosReport", exception.ToString()));
                return new Response();
            }
        }

        public Response DownData_SetRegionAlarm(CmdParam.ParamType ParamType, string CarValues, string CarPw, CmdParam.CommMode CommMode, RegionAlarmList regionAlarmList)
        {
            try
            {
                return this.myDownData.icar_SetRegionAlarm(ParamType, CarValues, CarPw, CommMode, regionAlarmList);
            }
            catch (Exception exception)
            {
                this.m_LogHelper.WriteError(new ErrorMsg("RemotingServer", "DownData_SetRegionAlarm", exception.ToString()));
                return new Response();
            }
        }

        public Response DownData_SetRegionAlarm_FJYD(CmdParam.ParamType ParamType, string CarValues, string CarPw, CmdParam.CommMode CommMode, RegionAlarmList regionAlarmList)
        {
            try
            {
                return this.myDownDataYD.icar_SetRegionAlarm(ParamType, CarValues, CarPw, CommMode, regionAlarmList);
            }
            catch (Exception exception)
            {
                this.m_LogHelper.WriteError(new ErrorMsg("RemotingServer", "DownData_SetRegionAlarm_FJYD", exception.ToString()));
                return new Response();
            }
        }

        public Response DownData_SetSpeedAlarm(CmdParam.ParamType ParamType, string CarValues, string CarPw, CmdParam.CommMode CommMode, SpeedAlarm speedAlarm)
        {
            try
            {
                return this.myDownData.icar_SetSpeedAlarm(ParamType, CarValues, CarPw, CommMode, speedAlarm);
            }
            catch (Exception exception)
            {
                this.m_LogHelper.WriteError(new ErrorMsg("RemotingServer", "DownData_SetSpeedAlarm", exception.ToString()));
                return new Response();
            }
        }

        public Response DownData_SetTransportReport(CmdParam.ParamType ParamType, string CarValues, string CarPw, CmdParam.CommMode CommMode, TransportReport transport)
        {
            try
            {
                return this.myDownData.icar_SetTransportReport(ParamType, CarValues, CarPw, CommMode, transport);
            }
            catch (Exception exception)
            {
                this.m_LogHelper.WriteError(new ErrorMsg("RemotingServer", "DownData_SetTransportReport", exception.ToString()));
                return new Response();
            }
        }

        public Response DownData_SimpleCmd(CmdParam.ParamType ParamType, string CarValues, string CarPw, CmdParam.CommMode CommMode, SimpleCmd simpleCmd)
        {
            try
            {
                return this.myDownData.icar_SimpleCmd(ParamType, CarValues, CarPw, CommMode, simpleCmd);
            }
            catch (Exception exception)
            {
                this.m_LogHelper.WriteError(new ErrorMsg("RemotingServer", "DownData_SimpleCmd", exception.ToString()));
                return new Response();
            }
        }

        public Response DownData_SimpleCmdEx(CmdParam.ParamType ParamType, string CarValues, string CarPw, CmdParam.CommMode CommMode, SimpleCmdEx simpleCmdEx)
        {
            try
            {
                return this.myDownData.icar_SimpleCmdEx(ParamType, CarValues, CarPw, CommMode, simpleCmdEx);
            }
            catch (Exception exception)
            {
                this.m_LogHelper.WriteError(new ErrorMsg("RemotingServer", "DownData_SimpleCmdEx", exception.ToString()));
                return new Response();
            }
        }

        public Response DownData_StopCapture(CmdParam.ParamType ParamType, string CarValues, string CarPw, CmdParam.CommMode CommMode, StopCapture stopCapture)
        {
            try
            {
                return this.myDownData.icar_StopCapture(ParamType, CarValues, CarPw, CommMode, stopCapture);
            }
            catch (Exception exception)
            {
                this.m_LogHelper.WriteError(new ErrorMsg("RemotingServer", "DownData_StopCapture", exception.ToString()));
                return new Response();
            }
        }

        public Response ExecNoQuery(string sql)
        {
            Response response = new Response();
            try
            {
                string str = Library.SecurityHelper.DecryptString(sql);
                LogMsg msg = new LogMsg {
                    ClassName = "RemotingServer",
                    FunctionName = "ExecNoQuery",
                    Msg = string.Concat(new object[] { @"WorkID\SQL:", this._OnlineUserInfo.WorkId, @"\", str })
                };
                new LogHelper().WriteLog(msg);
                if (this.myCar.ExecNoQuery(str) == 0)
                {
                    response.ResultCode = 0L;
                }
            }
            catch (Exception exception)
            {
                ErrorMsg msg2 = new ErrorMsg("RemotingServer", "ExecNoQuery", exception.Message);
                this.m_LogHelper.WriteError(msg2);
                response.ErrorMsg = exception.Message;
            }
            return response;
        }

        public DataTable ExecSql(string sql)
        {
            try
            {
                if (!this._RemotingState.IsConnected)
                {
                    new Exception("数据库连接已经断开！");
                }
                string str = Library.SecurityHelper.DecryptString(sql);
                LogMsg msg = new LogMsg {
                    ClassName = "RemotingServer",
                    FunctionName = "ExecSql",
                    Msg = string.Concat(new object[] { @"WorkID\SQL:", this._OnlineUserInfo.WorkId, @"\", str })
                };
                new LogHelper().WriteLog(msg);
                return this.myCar.ExecSql(str);
            }
            catch (Exception exception)
            {
                ErrorMsg msg2 = new ErrorMsg("RemotingServer", "ExecSql", exception.Message);
                this.m_LogHelper.WriteError(msg2);
                return null;
            }
        }

        public byte[] ExecSqlByCompress(string sql)
        {
            try
            {
                if (!this._RemotingState.IsConnected)
                {
                    new Exception("数据库连接已经断开！");
                }
                string str = Library.SecurityHelper.DecryptString(sql);
                LogMsg msg = new LogMsg {
                    ClassName = "RemotingServer",
                    FunctionName = "ExecSql",
                    Msg = string.Concat(new object[] { @"WorkID\SQL:", this._OnlineUserInfo.WorkId, @"\", str })
                };
                new LogHelper().WriteLog(msg);
                return CompressHelper.Compress(this.myCar.ExecSql(str));
            }
            catch (Exception exception)
            {
                ErrorMsg msg2 = new ErrorMsg("RemotingServer", "ExecSqlByCompress", exception.Message);
                this.m_LogHelper.WriteError(msg2);
                return null;
            }
        }

        public int FloatCarFilterToGpsCarFilter(int workid, string companycode)
        {
            try
            {
                return 1;
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("FloatCarManager", "FloatCarFilterToGpsCarFilter", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return -1;
            }
        }

        public string GetBufferSize()
        {
            string str = "1、登入用户数为：" + new OnlineUserManager().Count.ToString() + "\r\n";
            string str2 = "2、报警列表大小：" + BussinessHelper.upResponse.AlarmListSize.ToString() + "\r\n";
            string str3 = "3、应答列表大小：" + BussinessHelper.upResponse.RespListSize.ToString() + "/" + BussinessHelper.upResponse.RespListByWorkId.ToString() + "\r\n";
            string str4 = "4、车辆总数：" + CarDataInfoBuffer.Count.ToString() + "\r\n";
            string str5 = "5、内存大小：" + MemeroyHelper.AppMemerorySize.ToString() + "MB/" + MemeroyHelper.AppVirtualMemerorySize.ToString() + "MB\r\n";
            string str6 = "6、线程数：" + MemeroyHelper.ThreadCount.ToString() + "\r\n";
            string str7 = "7、服务器版本：V" + Assembly.GetEntryAssembly().GetName().Version.ToString() + "\r\n";
            return (str + str2 + str3 + str4 + str5 + str6 + str7);
        }

        public DataTable GetCommonData(string sCarId, int queryType, string[] queryCondition)
        {
            try
            {
                return this.myCar.GetCommonData(sCarId, queryType, queryCondition);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "GetCommonData", exception.Message);
                new LogHelper().WriteError(msg);
                return null;
            }
        }


        /// <summary>
        /// 客户端获取公司名称
        /// </summary>
        /// <returns></returns>
        public string GetCorpName()
        {
            return Const.CorpName;
        }

        public string GetCustomInfo()
        {
            return Const.CustomInfo;
        }

        public string GetMapAddress()
        {
            return Const.sMapAddr + "/" + Const.sMapName;
        }

        public string GetDBCurrentDateTime()
        {
            string dBCurrentDateTime = string.Empty;
            try
            {
                dBCurrentDateTime = this.myCar.GetDBCurrentDateTime();
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "GetDBCurrentDateTime", exception.Message);
                new LogHelper().WriteError(msg);
            }
            return dBCurrentDateTime;
        }

        /// <summary>
        /// 客户端获取授权接口
        /// </summary>
        /// <param name="sComputerId"></param>
        /// <param name="sDogSg"></param>
        /// <param name="sSystemId"></param>
        /// <param name="sIp"></param>
        /// <param name="sErrMsg"></param>
        /// <param name="sEmpowerCd"></param>
        /// <returns></returns>
        public int getE(string sComputerId, string sDogSg, string sSystemId, string sIp, ref string sErrMsg, ref string sEmpowerCd)
        {
            sErrMsg = "";
            sEmpowerCd = "";
            return 0;
            //try
            //{
            //    GLSServer server = new GLSServer(Const.GlsIp, Const.GlsPort);
            //    return server.getE(sComputerId, sDogSg, sSystemId, sIp, ref sErrMsg, ref sEmpowerCd);
            //}
            //catch (Exception exception)
            //{
            //    ErrorMsg msg = new ErrorMsg("GLSServer", "getE", exception.Message);
            //    this.m_LogHelper.WriteError(msg);
            //    return -1;
            //}
        }

        public DataTable GetFloatCarFilterByCompany(string companycode)
        {
            try
            {
                return null;
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("FloatCarManager", "GetFloatCarFilterByCompany", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return new DataTable();
            }
        }

        public DataTable GetLatestThreeMinCar(string companycode)
        {
            try
            {
                return null;
            }
            catch (Exception)
            {
                return new DataTable();
            }
        }

        public int getLocation(GetLocationParam getLocationParam, out ResponseLocationParam responseLocationParam)
        {
            responseLocationParam = new ResponseLocationParam();
            try
            {
                Random random = new Random();
                int num = random.Next(1, 100);
                responseLocationParam.dLongitude = 119.29717 + (num * 0.0001);
                num = random.Next(1, 100);
                responseLocationParam.dLatitude = 26.08478 + (num * 0.0001);
                responseLocationParam.sUserName = "TEST";
                responseLocationParam.sAddress = "福建省南平市延平区 XX路 105# 202";
                return 0;
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "getLocation", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return -1;
            }
        }

        public string GetParamData(string carid, int OrderCoder)
        {
            try
            {
                return this.myCar.GetParamData(carid, OrderCoder);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "GetParamData", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return "";
            }
        }

        public DataTable GetPathAlarmChecked(string strCarId)
        {
            try
            {
                return this.myCar.GetPathAlarmChecked(this._OnlineUserInfo.UserId, strCarId);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "GetPathAlarm", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public DataTable GetPhoneNumTextByCarid(string carid)
        {
            try
            {
                return this.myCar.GetPhoneNumTextByCarid(carid);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "GetPhoneNumTextByCarid", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public DataTable GetPrePathLongAndLat(string telephone, string BeginTime, string EndTime)
        {
            try
            {
                Alarm alarm = new Alarm();
                return alarm.GetPrePathLongAndLat(telephone, BeginTime, EndTime);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "GetPrePathLongAndLat", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        /// <summary>
        /// 客户端获取标题名称
        /// </summary>
        /// <returns></returns>
        public string GetTitleName()
        {
            return Const.Title;
        }

        public string GetMapAddr()
        {
            return "";
        }

        public Response icar_SendRawPackage(CmdParam.ParamType ParamType, string CarValues, string CarPw, CmdParam.CommMode CommMode, TrafficRawPackage trafficRawPackage)
        {
            try
            {
                return this.myDownDataYD.icar_SendRawPackage(ParamType, CarValues, CarPw, CommMode, trafficRawPackage);
            }
            catch (Exception exception)
            {
                this.m_LogHelper.WriteError(new ErrorMsg("RemotingServer", "icar_SendRawPackage", exception.ToString()));
                return new Response();
            }
        }

        public Response icar_SetCommonCmdTraffic(CmdParam.ParamType ParamType, string CarValues, string CarPw, CmdParam.CommMode CommMode, TrafficSimpleCmd trafficSimpleCmd)
        {
            try
            {
                return this.myDownDataYD.icar_SetCommonCmdTraffic(ParamType, CarValues, CarPw, CommMode, trafficSimpleCmd);
            }
            catch (Exception exception)
            {
                this.m_LogHelper.WriteError(new ErrorMsg("RemotingServer", "icar_SetCommonCmdTraffic", exception.ToString()));
                return new Response();
            }
        }

        public Response icar_SetPhoneNumText(CmdParam.ParamType ParamType, string CarValues, string CarPw, CmdParam.CommMode CommMode, TrafficPhoneNumText trafficPhoneNumText)
        {
            try
            {
                return this.myDownDataYD.icar_SetPhoneNumText(ParamType, CarValues, CarPw, CommMode, trafficPhoneNumText);
            }
            catch (Exception exception)
            {
                this.m_LogHelper.WriteError(new ErrorMsg("RemotingServer", "icar_SetPhoneNumText", exception.ToString()));
                return new Response();
            }
        }

        public Response icar_SetPlatformAlarmCmd(CmdParam.ParamType ParamType, string CarValues, string CarPw, CmdParam.CommMode CommMode, TrafficSimpleCmd trafficSimpleCmd)
        {
            try
            {
                return this.myDownDataYD.icar_SetPlatformAlarmCmd(ParamType, CarValues, CarPw, CommMode, trafficSimpleCmd);
            }
            catch (Exception exception)
            {
                this.m_LogHelper.WriteError(new ErrorMsg("RemotingServer", "icar_SetPlatformAlarmCmd", exception.ToString()));
                return new Response();
            }
        }

        public Response icar_SetPosReportConditions(CmdParam.ParamType ParamType, string CarValues, string CarPw, CmdParam.CommMode CommMode, TrafficPosReport trafficPosReport)
        {
            try
            {
                return this.myDownDataYD.icar_SetPosReportConditions(ParamType, CarValues, CarPw, CommMode, trafficPosReport);
            }
            catch (Exception exception)
            {
                this.m_LogHelper.WriteError(new ErrorMsg("RemotingServer", "icar_SetPosReportConditions", exception.ToString()));
                return new Response();
            }
        }

        public Response icar_SetTextMsg(CmdParam.ParamType ParamType, string CarValues, string CarPw, CmdParam.CommMode CommMode, TxtMsg trafficTextMsg)
        {
            try
            {
                return this.myDownDataYD.icar_SetTextMsg(ParamType, CarValues, CarPw, CommMode, trafficTextMsg, this._OnlineUserInfo.AreaCode, this._OnlineUserInfo.UserId);
            }
            catch (Exception exception)
            {
                this.m_LogHelper.WriteError(new ErrorMsg("RemotingServer", "icar_SetTextMsg", exception.ToString()));
                return new Response();
            }
        }

        public override object InitializeLifetimeService()
        {
            ILease lease = (ILease) base.InitializeLifetimeService();
            if (lease.CurrentState == LeaseState.Initial)
            {
                lease.InitialLeaseTime = TimeSpan.FromMinutes(5.0);
                lease.RenewOnCallTime = TimeSpan.FromMinutes(5.0);
            }
            return lease;
        }

        public bool IsAvtive()
        {
            return !this._RemotingState.IsOutTime;
        }

        public bool LoginSys_CheckUser(string UserId, string Pass)
        {
            try
            {
                UserLoginInterface interface2 = new UserLoginInterface(UserId, Pass, "4096");
                return interface2.IsCutUser();
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("LoginSys", "IsUserButCUser", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return false;
            }
        }

        public DataTable LoginSys_GetAllMenu()
        {
            try
            {
                Menu menu = new Menu();
                return menu.GetAllMenu(this._OnlineUserInfo.UserId, this._OnlineUserInfo.ModuleId, this._OnlineUserInfo.GroupId);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("LoginSys", "GetAllMenu", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public int MapFlag_AddFlagMap(float lon, float lat, string name, string areaCode, int flagTypeCode)
        {
            try
            {
                MapFlag flag = new MapFlag();
                return flag.AddFlagMap(lon, lat, name, areaCode, flagTypeCode);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("MapFlag", "AddFlagMap", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return -1;
            }
        }

        public bool MapFlag_DeleteFlagMap(string name)
        {
            try
            {
                MapFlag flag = new MapFlag();
                return flag.DeleteFlagMap(name);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("MapFlag", "DeleteFlagMap", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return false;
            }
        }

        public DataTable MapFlag_FlagMapType()
        {
            try
            {
                MapFlag flag = new MapFlag();
                return flag.FlagMapType(this._OnlineUserInfo.UserId);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("MapFlag", "FlagMapType", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public DataTable MapFlag_showFlagMap()
        {
            try
            {
                MapFlag flag = new MapFlag();
                return flag.showFlagMap(this._OnlineUserInfo.UserId);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("MapFlag", "showFlagMap", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        private void Pulse()
        {
            TimeSpan span = (TimeSpan) (DateTime.Now - this._OnlineUserInfo.SynDbUserTime);
            double totalMinutes = span.TotalMinutes;
            if (totalMinutes >= 6.0)
            {
                this._RemotingState.IsOutTime = true;
            }
            if (!this._RemotingState.IsOutTime && (totalMinutes >= 1.0))
            {
                try
                {
                    int num2 = new LoginOnlieUserInfo(this._OnlineUserInfo.WorkId).UpdateUserOnline();
                    if (!this._RemotingState.IsConnected)
                    {
                        this._RemotingState.IsConnected = true;
                        this._RemotingState.DataBaseInfo = "数据库已经连接上！";
                    }
                    if (num2 == 1)
                    {
                        this._OnlineUserInfo.SynDbUserTime = DateTime.Now;
                    }
                    else
                    {
                        this._RemotingState.IsOutTime = true;
                    }
                }
                catch (Exception exception)
                {
                    this._RemotingState.IsConnected = false;
                    this._RemotingState.DataBaseInfo = "数据库连接出错，详细信息：" + exception.Message;
                    Thread.Sleep(0x2710);
                }
            }
        }

        public int SendAuthorityFromClient(string oldMachineCode, string newMachineCode, string clientIp, string systemId, string sVersion, out string errorMessage, out string authorityCode)
        {
            try
            {
                errorMessage = "";
                authorityCode = "";
                return 0;
                //return GasInterface.SendAuthorityFromClient(string.Format("http://{0}:{1}/GLS/GLSService.asmx", Const.GlsIp, Const.GlsPort), string.Format("http://{0}:{1}/GLS/GLSService.asmx", Const.StandbyGlsIp, Const.StandbyGlsPort), oldMachineCode, newMachineCode, clientIp, systemId, sVersion, out errorMessage, out authorityCode);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("GLSServer", "SendAuthorityFromClient", exception.Message);
                this.m_LogHelper.WriteError(msg);
                errorMessage = exception.Message;
                authorityCode = "";
                return -1;
            }
        }

        public Response SetParam(string sOrderCode, string sParams1, string sParams2)
        {
            Response response = new Response();
            try
            {
                this.myDownDataPass.m_OrderCode = this.myDownDataDB44.m_OrderCode = this.myDownData.m_OrderCode = this.myDownDataYD.m_OrderCode = sOrderCode;
                this.myDownDataPass.m_Params1 = this.myDownDataDB44.m_Params1 = this.myDownData.m_Params1 = this.myDownDataYD.m_Params1 = sParams1;
                this.myDownDataPass.m_Params2 = this.myDownDataDB44.m_Params2 = this.myDownData.m_Params2 = this.myDownDataYD.m_Params2 = sParams2;
                this.myDownDataPass.m_UserId = this.myDownDataDB44.m_UserId = this.myDownData.m_UserId = this.myDownDataYD.m_UserId = this._OnlineUserInfo.UserId;
                response.ResultCode = 0L;
            }
            catch (Exception exception)
            {
                response.ErrorMsg = exception.Message;
            }
            return response;
        }

        public Response StopAlarmDeal(CmdParam.ParamType ParamType, string CarValues, string CarPw, CmdParam.CommMode CommMode, TrafficALarmHandle alarmHandle, object objOrder)
        {
            try
            {
                return this.myDownData.icar_StopAlarmDeal(ParamType, CarValues, CarPw, CommMode, alarmHandle, objOrder);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "StopAlarmDeal", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public DataTable TrackReplay_GetReplayData(string BeginTime, string EndTime, string Tele, int RecordCount, int PageNum, int PageCount, int IsComputeMile)
        {
            return this.TrackReplay_GetReplayData(BeginTime, EndTime, Tele, RecordCount, PageNum, PageCount, IsComputeMile, 1);
        }

        public DataTable TrackReplay_GetReplayData(string BeginTime, string EndTime, string Tele, int RecordCount, int PageNum, int PageCount, int IsComputeMile, int IsQueryPic)
        {
            try
            {
                TrackReplay replay = new TrackReplay();
                return replay.GetReplayData(BeginTime, EndTime, Tele, RecordCount, PageNum, PageCount, IsComputeMile, IsQueryPic);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("TrackReplay", "TrackReplay_GetReplayData", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public DataTable TrackReplay_GetReplayData(string BeginTime, string EndTime, string strSimNum, int RecordCount, int PageNum, int PageCount, int IsComputeMile, string strHand, string strDistance)
        {
            try
            {
                TrackReplay replay = new TrackReplay();
                return replay.GetReplayData(BeginTime, EndTime, strSimNum, RecordCount, PageNum, PageCount, IsComputeMile, strHand, strDistance);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("TrackReplay", "GetReplayDataCount", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public byte[] TrackReplay_GetReplayDataByCompress(string BeginTime, string EndTime, string Tele, int RecordCount, int PageNum, int PageCount, int IsComputeMile)
        {
            return this.TrackReplay_GetReplayDataByCompress(BeginTime, EndTime, Tele, RecordCount, PageNum, PageCount, IsComputeMile, 1);
        }

        public byte[] TrackReplay_GetReplayDataByCompress(string BeginTime, string EndTime, string Tele, int RecordCount, int PageNum, int PageCount, int IsComputeMile, int IsQueryPic)
        {
            try
            {
                return CompressHelper.Compress(this.TrackReplay_GetReplayData(BeginTime, EndTime, Tele, RecordCount, PageNum, PageCount, IsComputeMile, IsQueryPic));
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("TrackReplay", "TrackReplay_GetReplayDataByCompress", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public DataTable TrackReplay_GetReplayDataCount(string BeginTime, string EndTime, string SimNum)
        {
            try
            {
                TrackReplay replay = new TrackReplay();
                return replay.GetReplayDataCount(BeginTime, EndTime, SimNum);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "TrackReplay_GetReplayDataCount", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public DataTable TrackReplay_GetReplayPicData(string BeginTime, string EndTime, string Tele)
        {
            try
            {
                TrackReplay replay = new TrackReplay();
                return replay.GetReplayPicDataFromDB(BeginTime, EndTime, Tele);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("TrackReplay", "TrackReplay_GetReplayPicData", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public byte[] TrackReplay_GetReplayPicDataByCompress(string BeginTime, string EndTime, string Tele)
        {
            try
            {
                return CompressHelper.Compress(this.TrackReplay_GetReplayPicData(BeginTime, EndTime, Tele));
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("TrackReplay", "TrackReplay_GetReplayPicDataByCompress", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public byte[] TrackReplay_GetReplayPicDataByGpsRece(string GpsTime, string ReceTime, string Tele)
        {
            try
            {
                TrackReplay replay = new TrackReplay();
                return CompressHelper.Compress(replay.GetReplayPicDataFromDBByGpsRece(GpsTime, ReceTime, Tele));
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("TrackReplay", "TrackReplay_GetReplayPicData", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public DataTable Updata_GetCarAlarmLog()
        {
            DataTable alarmData = null;
            try
            {
                alarmData = BussinessHelper.upResponse.GetAlarmData(this._OnlineUserInfo);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "Updata_GetCarAlarmLog", exception.Message + exception.StackTrace);
                new LogHelper().WriteError(msg);
            }
            return alarmData;
        }

        public byte[] Updata_GetCarAlarmLogBSByCompress()
        {
            GpsDataTable table = this._DataBSList.ConvertToBSDataTable(this.Updata_GetCarAlarmLog());
            if ((table != null) && (table.Rows.Count > 0))
            {
                return CompressHelper.CompressToSelf(table);
            }
            return null;
        }

        public byte[] Updata_GetCarAlarmLogByCompress()
        {
            DataTable table = this.Updata_GetCarAlarmLog();
            if ((table != null) && (table.Rows.Count > 0))
            {
                return CompressHelper.Compress(table);
            }
            return null;
        }

        public DataTable Updata_GetCarCurrentPos()
        {
            DataTable cloneDataTableColumn = UpdataStruct.CloneDataTableColumn;
            try
            {
                lock (this._OnlineUserInfo.CarFilter.CarFilterList.SyncRoot)
                {
                    foreach (CarFilterInfo info in this._OnlineUserInfo.CarFilter.CarFilterList.Values)
                    {
                        if ((info.CarInfoData != null) && (info.CarInfoData.IsNewPosTime.CompareTo(info.PosReadTime) == 1))
                        {
                            info.PosReadTime = info.CarInfoData.IsNewPosTime;
                            if (info.IsPosSearchFlag && (Convert.ToInt32(info.CarInfoData.CarPosData[1]) == info.OrderId))
                            {
                                info.CarInfoData.CarPosData[13] = "-1";
                                this._OnlineUserInfo.DownCommd.AddNewLog(info.CarInfoData.CarPosData);
                                info.IsPosSearchFlag = false;
                            }
                            else
                            {
                                cloneDataTableColumn.Rows.Add(info.CarInfoData.CarPosData);
                            }
                        }
                    }
                    return cloneDataTableColumn;
                }
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "Updata_GetCarCurrentPos", exception.Message + exception.StackTrace);
                new LogHelper().WriteError(msg);
            }
            return cloneDataTableColumn;
        }

        public byte[] Updata_GetCarCurrentPosBsCompress()
        {
            if (!this._DataBSList.IsExistsBufferData)
            {
                DataTable data = this.Updata_GetCarCurrentPos();
                if ((data == null) || (data.Rows.Count <= 0))
                {
                    return null;
                }
                this._DataBSList.Add(data);
            }
            return this._DataBSList.Get();
        }

        public byte[] Updata_GetCarCurrentPosByCompress()
        {
            if (!this._DataCSList.IsExistsBufferData)
            {
                DataTable data = this.Updata_GetCarCurrentPos();
                if ((data == null) || (data.Rows.Count <= 0))
                {
                    return null;
                }
                this._DataCSList.Add(data);
            }
            return this._DataCSList.Get();
        }

        public byte[] Updata_GetCarCurrentPosNotSelBSByCompress()
        {
            GpsDataTable table = new GpsDataTable(UpdataStruct.ColNameList);
            try
            {
                Bussiness.CarInfo dataCarInfoByCarId = null;
                foreach (int num in this._OnlineUserInfo.UserCarId.Keys)
                {
                    dataCarInfoByCarId = CarDataInfoBuffer.GetDataCarInfoByCarId(num.ToString());
                    if (((dataCarInfoByCarId != null) && !this._OnlineUserInfo.CarFilter.CarFilterList.ContainsKey(dataCarInfoByCarId.SimNum)) && (dataCarInfoByCarId.CarPosData != null))
                    {
                        table.InsertRows(dataCarInfoByCarId.CarPosData);
                    }
                }
                if (table.Rows.Count <= 0)
                {
                    return null;
                }
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "Updata_GetCarCurrentPosNotSelBSByCompress", exception.Message + exception.StackTrace);
                new LogHelper().WriteError(msg);
                return null;
            }
            return CompressHelper.CompressToSelf(table);
        }

        public byte[] Updata_GetCarCurrentPosNotSelByCompress()
        {
            DataTable cloneDataTableColumn = UpdataStruct.CloneDataTableColumn;
            try
            {
                Bussiness.CarInfo dataCarInfoByCarId = null;
                foreach (int num in this._OnlineUserInfo.UserCarId.Keys)
                {
                    dataCarInfoByCarId = CarDataInfoBuffer.GetDataCarInfoByCarId(num.ToString());
                    if (((dataCarInfoByCarId != null) && !this._OnlineUserInfo.CarFilter.CarFilterList.ContainsKey(dataCarInfoByCarId.SimNum)) && (dataCarInfoByCarId.CarPosData != null))
                    {
                        cloneDataTableColumn.Rows.Add(dataCarInfoByCarId.CarPosData);
                    }
                }
                if (cloneDataTableColumn.Rows.Count <= 0)
                {
                    return null;
                }
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "Updata_GetCarCurrentPosNotSelByCompress", exception.Message + exception.StackTrace);
                new LogHelper().WriteError(msg);
                return null;
            }
            return CompressHelper.Compress(cloneDataTableColumn);
        }

        public DataTable Updata_GetCarNewLog()
        {
            try
            {
                if ((this._RemotingState.DataBaseInfo != null) && (this._RemotingState.DataBaseInfo.Length > 0))
                {
                    DataTable cloneDataTableColumn = UpdataStruct.CloneDataTableColumn;
                    DataRow row = cloneDataTableColumn.NewRow();
                    row["GpsTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    row["receTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    row["OrderID"] = 0;
                    row["OrderType"] = "信息";
                    row["OrderName"] = "提示信息";
                    row["msgType"] = -1;
                    row["CommFlag"] = "";
                    row["Describe"] = this._RemotingState.DataBaseInfo;
                    cloneDataTableColumn.Rows.Add(row);
                    this._RemotingState.DataBaseInfo = "";
                    return cloneDataTableColumn;
                }
                //System.Diagnostics.Trace.Write("appserver - Updata_GetCarNewLog this._OnlineUserInfo.DownCommd.ReadCarNewLogData();");
                DataTable newLogExt = this._OnlineUserInfo.DownCommd.ReadCarNewLogData();
                if ((newLogExt != null) && (newLogExt.Rows.Count > 0))
                {
                    return newLogExt;
                }
                //System.Diagnostics.Trace.Write("appserver - Updata_GetCarNewLog BussinessHelper.upResponse.GetNewLogExt(this._OnlineUserInfo);");
                newLogExt = BussinessHelper.upResponse.GetNewLogExt(this._OnlineUserInfo);
                if ((newLogExt != null) && (newLogExt.Rows.Count > 0))
                {
                    return newLogExt;
                }
                //System.Diagnostics.Trace.Write("appserver - Updata_GetCarNewLog BussinessHelper.upResponse.GetNewLog(this._OnlineUserInfo);");
                newLogExt = BussinessHelper.upResponse.GetNewLog(this._OnlineUserInfo);
                DataTable data = null;
                if ((newLogExt != null) && (newLogExt.Rows.Count > 0))
                {
                    data = newLogExt.Clone();
                    foreach (DataRow row2 in newLogExt.Rows)
                    {
                        if ((row2["CarId"] != DBNull.Value) && this._OnlineUserInfo.UserCarId.IsExistCarID(Convert.ToInt32(row2["CarId"])))
                        {
                            data.Rows.Add(row2.ItemArray);
                        }
                    }
                }
                else
                {
                    //System.Diagnostics.Trace.Write("appserver - Updata_GetCarNewLog BussinessHelper.upOutEquipmentData.GetData(this._OnlineUserInfo);");
                    newLogExt = BussinessHelper.upOutEquipmentData.GetData(this._OnlineUserInfo);
                    data = null;
                    if ((newLogExt != null) && (newLogExt.Rows.Count > 0))
                    {
                        data = newLogExt.Clone();
                        foreach (DataRow row3 in newLogExt.Rows)
                        {
                            if (string.IsNullOrEmpty(row3["SimNum"].ToString()))
                            {
                                data.Rows.Add(row3.ItemArray);
                            }
                            else if (this._OnlineUserInfo.UserCarId.IsExistCarID(Convert.ToInt32(row3["CarId"])))
                            {
                                data.Rows.Add(row3.ItemArray);
                            }
                        }
                    }
                    else
                    {
                        //System.Diagnostics.Trace.Write("appserver - Updata_GetCarNewLog BussinessHelper.upOtherData.GetData(this._OnlineUserInfo);");
                        data = BussinessHelper.upOtherData.GetData(this._OnlineUserInfo);
                    }
                }
                return data;
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "Updata_GetCarNewLog", exception.Message + exception.StackTrace);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public byte[] Updata_GetCarNewLogBSByCompress()
        {
            GpsDataTable table = this._DataBSList.ConvertToBSDataTable(this.Updata_GetCarNewLog());
            if ((table != null) && (table.Rows.Count > 0))
            {
                return CompressHelper.CompressToSelf(table);
            }
            return null;
        }

        public byte[] Updata_GetCarNewLogByCompress()
        {
            DataTable table = this.Updata_GetCarNewLog();
            if ((table != null) && (table.Rows.Count > 0))
            {
                return CompressHelper.Compress(table);
            }
            return null;
        }

        public DataTable Updata_GetCarPic()
        {
            DataTable pictureData = null;
            try
            {
                pictureData = BussinessHelper.upResponse.GetPictureData(this._OnlineUserInfo);
                this.Pulse();
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "Updata_GetCarPic", exception.Message + exception.StackTrace);
                new LogHelper().WriteError(msg);
            }
            return pictureData;
        }

        public byte[] Updata_GetCarPicBSByCompress()
        {
            GpsDataTable table = this._DataBSList.ConvertToBSDataTable(this.Updata_GetCarPic());
            if ((table != null) && (table.Rows.Count > 0))
            {
                return CompressHelper.CompressToSelf(table);
            }
            return null;
        }

        public byte[] Updata_GetCarPicBSByCompress(ref DateTime readtime)
        {
            byte[] buffer = null;
            try
            {
                this.Pulse();
                GpsDataTable table = this._DataBSList.ConvertToBSDataTable(BussinessHelper.upResponse.GetPictureData(this._OnlineUserInfo, ref readtime));
                if ((table == null) || (table.Rows.Count <= 0))
                {
                    return null;
                }
                buffer = CompressHelper.CompressToSelf(table);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "Updata_GetCarPicBSByCompress", exception.Message + exception.StackTrace);
                new LogHelper().WriteError(msg);
            }
            return buffer;
        }

        public byte[] Updata_GetCarPicByCompress()
        {
            DataTable table = this.Updata_GetCarPic();
            if ((table != null) && (table.Rows.Count > 0))
            {
                return CompressHelper.Compress(table);
            }
            return null;
        }

        public DataTable Updata_GetCarReachDateData()
        {
            try
            {
                return BussinessHelper.upReachTime.GetUpdata(this._OnlineUserInfo.UserId);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingServer", "Updata_GetCarReachDateData", exception.Message + exception.StackTrace);
                this.m_LogHelper.WriteError(msg);
                return null;
            }
        }

        public int UpdateFloatCarFilter(string companycode, int workid)
        {
            try
            {
                return 0;
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("FloatCarManager", "UpdateFloatCarFilter", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return -1;
            }
        }

        public int UpdatePathEx(TrafficPath traffic)
        {
            try
            {
                return this.myAlarm.UpdatePathEx(traffic);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("Alarm", "UpdatePathEx", exception.Message);
                this.m_LogHelper.WriteError(msg);
                return -1;
            }
        }

        public string User_ChangePassword(string sUser, string sOldPassword, string sNewPassword)
        {
            LoginUserInfo info = new LoginUserInfo(sUser, sOldPassword);
            return info.ChangePassword(sNewPassword);
        }


        public DataTable CarOil_GetOilAnalogValue(string sCarId)
        {
            throw new NotImplementedException();
        }

        public Response ExecParamNoQuery(string sql, List<SqlParam> list)
        {
            throw new NotImplementedException();
        }

        public byte[] ExecParamSqlByCompress(string sql, List<SqlParam> list)
        {
            throw new NotImplementedException();
        }

        public Response SaveIOStatusName(string sCarId, string IOStatusName)
        {
            throw new NotImplementedException();
        }
    }
}


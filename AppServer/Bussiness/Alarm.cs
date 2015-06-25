using DataAccess;
using Library;
using System;
using System.Data;
using System.Data.SqlClient;
using ParamLibrary.CmdParamInfo;

namespace Bussiness
{
    public class Alarm : Base
    {
        public int DelPath(int int_0, string string_0)
        {
            string str = "AppSvr_DelPath";
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@pathID", int_0), new SqlParameter("@pathName", string_0) };
            SqlDataAccess access = new SqlDataAccess();
            return access.updateBySp(str, parameterArray);
        }

        public bool DelPathCheckAuth(int int_0, string string_0, string string_1)
        {
            string format = " SELECT A.PathID, A.PathName FROM gpsPathType A,GpsPathInGroup B,GpsPathGroupAuth C  WHERE C.USERID='{0}' AND c.AUTH&2<>0 AND B.PathID IS NOT NULL  AND c.pathgroupID=b.pathgroupID AND A.PathID=B.PathID  AND A.PathID='{1}' ";
            format = string.Format(format, string_1, int_0);
            if (int_0 == -1)
            {
                format = " SELECT A.PathID, A.PathName FROM gpsPathType A,GpsPathInGroup B,GpsPathGroupAuth C  WHERE C.USERID='{0}' AND c.AUTH&2<>0 AND B.PathID IS NOT NULL  AND c.pathgroupID=b.pathgroupID AND A.PathID=B.PathID  AND A.PathName='{1}' ";
                format = string.Format(format, string_1, string_0);
            }
            DataTable table = new SqlDataAccess().getDataBySql(format);
            return ((table != null) && (table.Rows.Count > 0));
        }

        public int DelRegion(string string_0)
        {
            string str = "AppSvr_DelRegion";
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@regionName", string_0) };
            SqlDataAccess access = new SqlDataAccess();
            return access.updateBySp(str, parameterArray);
        }

        public bool DelRegionCheckAuth(string string_0, string string_1)
        {
            string format = " SELECT A.regionID, A.regionName FROM gpsRegionType A,GpsPathInGroup B,GpsPathGroupAuth C  WHERE C.USERID='{0}' and A.regionName='{1}' and c.AUTH&2<>0 AND B.REGIONID IS NOT NULL AND c.pathgroupID=b.pathgroupID AND A.REGIONID=B.REGIONID ";
            format = string.Format(format, string_1, string_0);
            DataTable table = new SqlDataAccess().getDataBySql(format);
            return ((table != null) && (table.Rows.Count > 0));
        }

        public DataTable GetGroupType(string string_0)
        {
            string str = " SELECT B.pathgroupName,A.pathgroupID FROM GpsPathGroupAuth A,GpsPathGroup B ";
            str = str + " WHERE A.USERID='" + string_0 + "' AND (A.AUTH&1<>0 or A.AUTH&2<>0) AND A.pathgroupID=B.pathgroupID";
            return new SqlDataAccess().getDataBySql(str);
        }

        public DataTable GetPathInfo(string string_0)
        {
            string format = @"SELECT D.*,E.AlarmPathDot,(case when E.pathtype=0 then  ''  else SUBSTRING(E.regioninfo,0,CHARINDEX('\1*',E.regionInfo)) end) as factoryName,(case when E.pathtype=0 then  ''  else  SUBSTRING(E.regioninfo,CHARINDEX('\1*',E.regionInfo)+3,CHARINDEX('\4*',E.regionInfo)-CHARINDEX('\1*',E.regionInfo)-3) end) as buildingSitName,(case when E.pathtype=0 then  ''  else  SUBSTRING(E.regioninfo,CHARINDEX('\4*',E.regionInfo)+3,len(E.regioninfo)-CHARINDEX('\4*',E.regionInfo)-2) end) as region_Radius  FROM (SELECT DISTINCT A.PathID, A.PathName,A.Remark,B.pathgroupID,A.pathType  FROM gpsPathType A,GpsPathInGroup B,GpsPathGroupAuth C  WHERE C.USERID='{0}' AND (c.AUTH&1<>0 or c.AUTH&2<>0) AND B.PathID IS NOT NULL AND c.pathgroupID=b.pathgroupID AND A.PathID=B.PathID) D,gpsPathType E  WHERE D.PathID=E.PathID ORDER BY D.PathID desc ";
            format = @"SELECT D.*,E.AlarmPathDot, (SELECT RegionName FROM GpsRegionType Where Convert(Varchar,RegionID)=(case when E.pathtype=0 then  ''  else SUBSTRING(E.regioninfo,0,CHARINDEX('\1*',E.regionInfo)) end)) as factoryName, (SELECT RegionName FROM GpsRegionType Where Convert(Varchar,RegionID)=(case when E.pathtype=0 then  ''  else  SUBSTRING(E.regioninfo,CHARINDEX('\1*',E.regionInfo)+3,CHARINDEX('\4*',E.regionInfo)-CHARINDEX('\1*',E.regionInfo)-3) end)) as buildingSitName,(case when E.pathtype=0 then  ''  else  SUBSTRING(E.regioninfo,CHARINDEX('\4*',E.regionInfo)+3,len(E.regioninfo)-CHARINDEX('\4*',E.regionInfo)-2) end) as region_Radius  FROM (SELECT DISTINCT A.PathID, A.PathName,A.Remark,B.pathgroupID,A.pathType  FROM gpsPathType A,GpsPathInGroup B,GpsPathGroupAuth C  WHERE C.USERID='{0}' AND (c.AUTH&1<>0 or c.AUTH&2<>0) AND B.PathID IS NOT NULL AND c.pathgroupID=b.pathgroupID AND A.PathID=B.PathID) D,gpsPathType E  WHERE D.PathID=E.PathID ORDER BY D.PathID desc ";
            SqlDataAccess access = new SqlDataAccess();
            format = string.Format(format, string_0);
            return access.getDataBySql(format);
        }

        public DataTable GetPathSegmentInfo(string string_0)
        {
            string format = "select PathSegmentName,e.PathID,PathSegmentID,alarmSegmentDot from GpsPathSegment e inner join (SELECT DISTINCT A.PathID FROM gpsPathType A,GpsPathInGroup B,GpsPathGroupAuth C  WHERE C.USERID='{0}' AND (c.AUTH&1<>0 or c.AUTH&2<>0) AND B.PathID IS NOT NULL AND c.pathgroupID=b.pathgroupID AND A.PathID=B.PathID) d on e.PathID=d.PathID ";
            SqlDataAccess access = new SqlDataAccess();
            format = string.Format(format, string_0);
            return access.getDataBySql(format);
        }

        public DataTable GetPrePathLongAndLat(string string_0, string string_1, string string_2)
        {
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@telephone", string_0), new SqlParameter("@BeginTime", string_1), new SqlParameter("@EndTime", string_2) };
            string str = "Select Longitude,Latitude from gpsrecerealtime Where telephone=@telephone  and gpsTIME>=@BeginTime and gpsTIME<=@EndTime and not  Longitude is null and not  Latitude  is null and Longitude>0.0000001 and  Latitude>0.0000001 order by gpsTIME";
            SqlDataAccess access = new SqlDataAccess();
            return access.getDataBySqlParam(str, parameterArray);
        }

        public DataTable GetRegionInfo(string string_0)
        {
            string format = " SELECT D.*,E.RegionDot FROM (SELECT DISTINCT A.regionID, A.regionName,B.pathgroupID,C.AUTH FROM gpsRegionType A,GpsPathInGroup B,GpsPathGroupAuth C  WHERE C.USERID='{0}' and (c.AUTH&1<>0 or c.AUTH&2<>0) AND B.REGIONID IS NOT NULL AND c.pathgroupID=b.pathgroupID AND A.REGIONID=B.REGIONID) D, gpsRegionType E WHERE D.regionID=E.regionID order by D.regionID desc";
            format = string.Format(format, string_0);
            SqlDataAccess access = new SqlDataAccess();
            return access.getDataBySql(format);
        }

        public int InsertAlarmArea(string string_0, string string_1, int int_0)
        {
            string str = "WebGpsClient_AddMapFlag";
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@regionStr", SqlDbType.Text), new SqlParameter("@regionName", SqlDbType.VarChar), new SqlParameter("@regionFeature", SqlDbType.Int) };
            parameterArray[0].Value = string_0;
            parameterArray[1].Value = string_1;
            parameterArray[2].Value = int_0;
            SqlDataAccess access = new SqlDataAccess();
            return access.insertBySp(str, parameterArray);
        }

        public static int InsertAlarmResult(TrafficALarmHandle trafficALarmHandle_0)
        {
            string format = "insert into GpsJTBAlarmProc values('{0}','{1}',getdate(),{2},{3},'{4}','{5}')";
            format = string.Format(format, new object[] { trafficALarmHandle_0.CarId, trafficALarmHandle_0.GpsTime, trafficALarmHandle_0.WorkID, trafficALarmHandle_0.OrderID, trafficALarmHandle_0.ProcMode, trafficALarmHandle_0.Remark });
            SqlDataAccess access = new SqlDataAccess();
            return access.insertBySql(format);
        }

        public int InsertPathRelatedType(int int_0, int int_1)
        {
            string str = string.Concat(new object[] { "insert into GpsPathInGroup(pathgroupID,pathID) VALUES(", int_0, ",", int_1, ")" });
            SqlDataAccess access = new SqlDataAccess();
            return access.insertBySql(str);
        }

        public int InsertRelatedType(int int_0, int int_1)
        {
            SqlDataAccess access = new SqlDataAccess();
            string str = string.Concat(new object[] { "insert into GpsPathInGroup(pathgroupID,REGIONID) VALUES(", int_0, ",", int_1, ")" });
            return access.insertBySql(str);
        }

        public int InsertRelatedType(string[] string_0, int int_0)
        {
            string str = "";
            foreach (string str2 in string_0)
            {
                object obj2 = str;
                str = string.Concat(new object[] { obj2, " insert into GpsPathInGroup(pathgroupID,pathID) VALUES(", str2, ",", int_0, ") " });
            }
            SqlDataAccess access = new SqlDataAccess();
            return access.insertBySql(str);
        }

        public DataTable PreSetPath(string string_0, string string_1, int int_0, int int_1, string string_2, double double_0, double double_1, string string_3, double double_2, double double_3)
        {
            string str = "WebGpsClient_PreSetPath";
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@pathStr", string_0), new SqlParameter("@pathName", string_1), new SqlParameter("@pathType", int_0), new SqlParameter("@region_Radius", int_1), new SqlParameter("@factoryName", string_2), new SqlParameter("@lon_Factory", double_0), new SqlParameter("@lat_Factory", double_1), new SqlParameter("@buildingSitName", string_3), new SqlParameter("@lon_BuildingSit", double_2), new SqlParameter("@lat_BuildingSit", double_3) };
            SqlDataAccess access = new SqlDataAccess();
            return access.getDataBySP(str, parameterArray);
        }

        public DataTable PreSetPath(string string_0, string string_1, int int_0, int int_1, string string_2, double double_0, double double_1, string string_3, double double_2, double double_3, string string_4)
        {
            string str = "WebGpsClient_PreSetPath";
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@pathStr", string_0), new SqlParameter("@pathName", string_1), new SqlParameter("@pathType", int_0), new SqlParameter("@region_Radius", int_1), new SqlParameter("@factoryName", string_2), new SqlParameter("@lon_Factory", double_0), new SqlParameter("@lat_Factory", double_1), new SqlParameter("@buildingSitName", string_3), new SqlParameter("@lon_BuildingSit", double_2), new SqlParameter("@lat_BuildingSit", double_3), new SqlParameter("@remark", string_4) };
            SqlDataAccess access = new SqlDataAccess();
            return access.getDataBySP(str, parameterArray);
        }

        public DataTable PreSetPathEx(string string_0, string string_1, int int_0, int int_1, string string_2, double double_0, double double_1, string string_3, double double_2, double double_3, string string_4, string[][] string_5)
        {
            DataTable table = this.PreSetPath(string_0, string_1, int_0, int_1, string_2, double_0, double_1, string_3, double_2, double_3, string_4);
            int num = int.Parse((table.Rows[0][0] == null) ? "-1" : table.Rows[0][0].ToString());
            string format = "insert into GpsPathSegment ([PathSegmentName], [PathID],[alarmSegmentDot]) values('{0}',{1},'{2}')";
            string str2 = "";
            SqlDataAccess access = new SqlDataAccess();
            int num2 = (string_5 == null) ? 0 : string_5.Length;
            for (int i = 0; i < num2; i++)
            {
                str2 = string.Format(format, string_5[i][0], num, string_5[i][1]);
                access.insertBySql(str2);
            }
            return table;
        }

        public DataTable PreSetRegion(string string_0, string string_1, int int_0)
        {
            string str = "AppSvr_PreSetRegion";
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@regionStr", string_0), new SqlParameter("@regionName", string_1), new SqlParameter("@regionFeature", int_0) };
            SqlDataAccess access = new SqlDataAccess();
            return access.getDataBySP(str, parameterArray);
        }

        public DataTable ShowGroupType(string string_0)
        {
            string str = " SELECT B.pathgroupName,A.pathgroupID FROM GpsPathGroupAuth A,GpsPathGroup B ";
            str = str + " WHERE A.USERID='" + string_0 + "' AND A.AUTH&4<>0 AND A.pathgroupID=B.pathgroupID";
            return new SqlDataAccess().getDataBySql(str);
        }

        public int UpdatePathEx(TrafficPath trafficPath_0)
        {
            string[][] pathSegments = trafficPath_0.PathSegments;
            int num = (pathSegments == null) ? 0 : pathSegments.Length;
            string str = "";
            bool flag = true;
            for (int i = 0; i < num; i++)
            {
                if (flag)
                {
                    str = string.Concat(new object[] { trafficPath_0.PathId, ",''", pathSegments[i][0], "'',''", pathSegments[i][1], "''" });
                    flag = false;
                }
                else
                {
                    object obj2 = str;
                    str = string.Concat(new object[] { obj2, "#", trafficPath_0.PathId, ",''", pathSegments[i][0], "'',''", pathSegments[i][1], "''" });
                }
            }
            string str2 = "WebGpsClientJTB_UpdatePath";
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@PathID", trafficPath_0.PathId), new SqlParameter("@pathStr", trafficPath_0.pathStr), new SqlParameter("@pathName", trafficPath_0.pathName), new SqlParameter("@pathType", trafficPath_0.pathType), new SqlParameter("@region_Radius", trafficPath_0.region_Radius), new SqlParameter("@factoryName", trafficPath_0.factoryName), new SqlParameter("@lon_Factory", trafficPath_0.lon_Factory), new SqlParameter("@lat_Factory", trafficPath_0.lat_Factory), new SqlParameter("@buildingSitName", trafficPath_0.buildingSitName), new SqlParameter("@lon_BuildingSit", trafficPath_0.lon_BuildingSit), new SqlParameter("@lat_BuildingSit", trafficPath_0.lat_BuildingSit), new SqlParameter("@remark", trafficPath_0.remark), new SqlParameter("@SegmentList", str), new SqlParameter("@iNewPath", trafficPath_0.isNewPath ? 1 : 0), new SqlParameter("@pathgroupID", trafficPath_0.pathgroupID) };
            DataTable table = new SqlDataAccess().getDataBySP(str2, parameterArray);
            return ((((table == null) || (table.Rows.Count == 0)) || (table.Rows[0][0] == DBNull.Value)) ? -3 : int.Parse(table.Rows[0][0].ToString()));
        }

        public int UpdateRegionType(string string_0, string string_1)
        {
            string format = "update gpsRegionType set regionDot = '{0}' where regionId = {1}";
            format = string.Format(format, string_1, string_0);
            SqlDataAccess access = new SqlDataAccess();
            return access.updateBySql(format);
        }

        public int UpdateRegionType(string string_0, string string_1, string string_2, int int_0)
        {
            int num = 0;
            string format = "update gpsRegionType set regionDot = '{0}', RegionName='{2}' where regionId = {1}";
            format = string.Format(format, string_1, string_0, string_2);
            SqlDataAccess access = new SqlDataAccess();
            num = access.updateBySql(format);
            if (num > 0)
            {
                format = string.Concat(new object[] { "update GpsPathInGroup set pathgroupID=", int_0, " where regionID=", string_0 });
                access.updateBySql(format);
            }
            return num;
        }
    }
}


using GisServices.PlatformAlarmWebReference;
using GisServices.WebGISservice;
using Library;
using System;
using System.Net;

namespace GisServices
{
	public class ReadDataFromGis
	{
		private static WebgisService ServerGetAddress;

		private static AlarmService platformalarmService;

		public static WebgisService GisWebService
		{
			get
			{
				WebgisService serverGetAddress;
				try
				{
					if (ReadDataFromGis.ServerGetAddress == null)
					{
						ReadDataFromGis.ServerGetAddress = new WebgisService();
						ReadDataFromGis.GisWebService.Url = string.Concat("http://", ReadDataFromXml.MapUrl, "/StarGIS/OpenLayers/WebgisService.asmx");
					}
					serverGetAddress = ReadDataFromGis.ServerGetAddress;
				}
				catch
				{
					serverGetAddress = null;
				}
				return serverGetAddress;
			}
		}

		public static AlarmService PlatformAlarmService
		{
			get
			{
				AlarmService alarmService;
				try
				{
					if (ReadDataFromGis.platformalarmService == null)
					{
						ReadDataFromGis.platformalarmService = new AlarmService();
						ReadDataFromGis.PlatformAlarmService.Url = string.Concat("http://", ReadDataFromXml.MapUrl, "/StarGIS/Service/AlarmService.asmx");
					}
					alarmService = ReadDataFromGis.platformalarmService;
				}
				catch
				{
					alarmService = null;
				}
				return alarmService;
			}
		}

		public ReadDataFromGis()
		{
		}

		public static string ChackRoadSegAlarmCustom(string inputXML)
		{
			string str;
			try
			{
				str = ReadDataFromGis.PlatformAlarmService.ChackRoadSegAlarmCustom(inputXML);
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				LogHelper logHelper = new LogHelper();
				ErrorMsg errorMsg = new ErrorMsg("ReadDataFromGis", "ChackRoadSegAlarmCustom", string.Concat("调用webservice判断车自定义分段超速报警错误：", exception.Message));
				logHelper.WriteError(errorMsg);
				str = "";
			}
			return str;
		}

		public static string CheckRoadSegAlarm(string inputXML)
		{
			string str;
			try
			{
				str = ReadDataFromGis.PlatformAlarmService.ChackRoadSegAlarm(inputXML);
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				LogHelper logHelper = new LogHelper();
				ErrorMsg errorMsg = new ErrorMsg("ReadDataFromGis", "CheckRoadSegAlarm", string.Concat("调用webservice判断分路段超速报警错误：", exception.Message));
				logHelper.WriteError(errorMsg);
				str = "";
			}
			return str;
		}

		public static string checkRoadSpeedAndRank(string inputXML)
		{
			string str;
			try
			{
                str = "";
				//str = ReadDataFromGis.PlatformAlarmService.checkRoadSpeedAndRank(inputXML);
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				LogHelper logHelper = new LogHelper();
				ErrorMsg errorMsg = new ErrorMsg("ReadDataFromGis", "checkRoadSpeedAndRank", string.Concat("调用webservice判断车自定义分段超速报警和道路等级错误：", exception.Message));
				logHelper.WriteError(errorMsg);
				str = "";
			}
			return str;
		}

		public static string GetAlarmState(string inputXml)
		{
			string alarmState;
			LogHelper logHelper = null;
			try
			{
				alarmState = ReadDataFromGis.GisWebService.GetAlarmState(inputXml);
			}
			catch (WebException webException1)
			{
				WebException webException = webException1;
				logHelper = new LogHelper();
				ErrorMsg errorMsg = new ErrorMsg()
				{
					ClassName = "ReadDataFromGis",
					FunctionName = "GetAlarmState",
					ErrorText = "判断三级路面报警发生WebException错误"
				};
				logHelper.WriteError(errorMsg, webException);
				alarmState = "";
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				logHelper = new LogHelper();
				ErrorMsg errorMsg1 = new ErrorMsg()
				{
					ClassName = "ReadDataFromGis",
					FunctionName = "GetAlarmState",
					ErrorText = "判断三级路面报警发生错误"
				};
				logHelper.WriteError(errorMsg1, exception);
				alarmState = "";
			}
			return alarmState;
		}

		public static string[] GetBillCarAddress(string[] pointList)
		{
			string[] cityDistrictInfo;
			LogHelper logHelper = null;
			try
			{
				if (pointList == null)
				{
					cityDistrictInfo = null;
				}
				else
				{
					cityDistrictInfo = ReadDataFromGis.GisWebService.GetCityDistrictInfo(pointList);
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				logHelper = new LogHelper();
				ErrorMsg errorMsg = new ErrorMsg()
				{
					ClassName = "ReadDataFromGis",
					FunctionName = "GetBillCarAddress",
					ErrorText = "批量解析地理位置发生错误"
				};
				logHelper.WriteError(errorMsg, exception);
				cityDistrictInfo = null;
			}
			return cityDistrictInfo;
		}

		public static string GetSeparateAndStickyCars(string inputXml)
		{
			string separateAndStickyCars;
			LogHelper logHelper = null;
			try
			{
				separateAndStickyCars = ReadDataFromGis.PlatformAlarmService.GetSeparateAndStickyCars(inputXml);
			}
			catch (WebException webException1)
			{
				WebException webException = webException1;
				logHelper = new LogHelper();
				ErrorMsg errorMsg = new ErrorMsg()
				{
					ClassName = "ReadDataFromGis",
					FunctionName = "GetSeparateAndStickyCars",
					ErrorText = "取得脱车粘车报警信息发生WebException错误"
				};
				logHelper.WriteError(errorMsg, webException);
				separateAndStickyCars = "";
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				logHelper = new LogHelper();
				ErrorMsg errorMsg1 = new ErrorMsg()
				{
					ClassName = "ReadDataFromGis",
					FunctionName = "GetSeparateAndStickyCars",
					ErrorText = "取得脱车粘车报警信息发生错误"
				};
				logHelper.WriteError(errorMsg1, exception);
				separateAndStickyCars = "";
			}
			return separateAndStickyCars;
		}

		public static string IsCarsOnRoad(string inputXML)
		{
			string str;
			try
			{
				str = ReadDataFromGis.PlatformAlarmService.IsCarsOnRoad(inputXML);
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				LogHelper logHelper = new LogHelper();
				ErrorMsg errorMsg = new ErrorMsg("ReadDataFromGis", "IsCarsOnRoad", string.Concat("调用webservice判断车路线偏移报警错误：", exception.Message));
				logHelper.WriteError(errorMsg);
				str = "";
			}
			return str;
		}

		public static string IsInArea(string inputXML)
		{
			string str;
			try
			{
				str = ReadDataFromGis.PlatformAlarmService.IsInArea(inputXML);
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				LogHelper logHelper = new LogHelper();
				ErrorMsg errorMsg = new ErrorMsg("ReadDataFromGis", "IsInArea", string.Concat("调用webservice判断车路线偏移报警错误：", exception.Message));
				logHelper.WriteError(errorMsg);
				str = "";
			}
			return str;
		}

		public static string QueryAllLayerByPoint(string sLon, string sLat)
		{
			string str;
			LogHelper logHelper = null;
			try
			{
				double num = double.Parse(sLon);
				double num1 = double.Parse(sLat);
				string str1 = ReadDataFromGis.GisWebService.QueryAllLayerByPoint(num, num1);
				string[] strArrays = new string[] { ":::" };
				string[] strArrays1 = str1.Split(strArrays, StringSplitOptions.RemoveEmptyEntries);
				str = ((int)strArrays1.Length > 1 ? strArrays1[1] : "");
			}
			catch (WebException webException1)
			{
				WebException webException = webException1;
				logHelper = new LogHelper();
				ErrorMsg errorMsg = new ErrorMsg()
				{
					ClassName = "ReadDataFromGis",
					FunctionName = "QueryAllLayerByPoint",
					ErrorText = "取得详细位置信息发生WebException错误"
				};
				logHelper.WriteError(errorMsg, webException);
				str = "";
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				logHelper = new LogHelper();
				ErrorMsg errorMsg1 = new ErrorMsg()
				{
					ClassName = "ReadDataFromGis",
					FunctionName = "QueryAllLayerByPoint",
					ErrorText = "取得详细位置信息发生错误"
				};
				logHelper.WriteError(errorMsg1, exception);
				str = "";
			}
			return str;
		}

		public static bool servicerIsInRegion(double lon, double lat, string regionId)
		{
			bool flag;
			try
			{
				flag = ReadDataFromGis.GisWebService.IsInRegion(lon, lat, regionId);
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				LogHelper logHelper = new LogHelper();
				ErrorMsg errorMsg = new ErrorMsg("ReadDataFromGis", "servicerIsInRegion", string.Concat("调用webservice判断车辆是否在行政区内例外：", exception.Message));
				logHelper.WriteError(errorMsg);
				flag = false;
			}
			return flag;
		}

		public static string[] servicerIsInRegions(string[] paramStr)
		{
			string[] strArrays;
			try
			{
				strArrays = ReadDataFromGis.GisWebService.IsInRegions(paramStr);
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				LogHelper logHelper = new LogHelper();
				ErrorMsg errorMsg = new ErrorMsg("ReadDataFromGis", "servicerIsInRegion", string.Concat("调用webservice判断车辆是否在行政区内例外：", exception.Message));
				logHelper.WriteError(errorMsg);
				strArrays = null;
			}
			return strArrays;
		}
	}
}
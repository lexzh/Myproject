using Library;
using System;
using System.Net;
using System.Text;

namespace GisServices
{
	public class ReadDataFromOtherGis
	{
		private static WebClient gisWebClient;

		public static WebClient GisWebClient
		{
			get
			{
				WebClient webClient;
				try
				{
					if (ReadDataFromOtherGis.gisWebClient == null)
					{
						ReadDataFromOtherGis.gisWebClient = new WebClient();
					}
					webClient = ReadDataFromOtherGis.gisWebClient;
				}
				catch
				{
					webClient = null;
				}
				return webClient;
			}
		}

		public ReadDataFromOtherGis()
		{
		}

		public static string checkRoadSpeedAndRank(string inputXML)
		{
			string str;
			try
			{
				string str1 = string.Format("Method={0}&PosMsgsXML={1}", "SpeedLimitAlarm", inputXML);
				byte[] bytes = Encoding.UTF8.GetBytes(str1);
				string roadSpeedAndRankOtherGisAddress = ReadDataFromXml.RoadSpeedAndRankOtherGisAddress;
				ReadDataFromOtherGis.GisWebClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
				byte[] numArray = ReadDataFromOtherGis.GisWebClient.UploadData(roadSpeedAndRankOtherGisAddress, "POST", bytes);
				str = Encoding.UTF8.GetString(numArray);
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				LogHelper logHelper = new LogHelper();
				ErrorMsg errorMsg = new ErrorMsg("ReadDataFromOtherGis", "checkRoadSpeedAndRank", string.Concat("调用webservice判断车自定义分段超速报警和道路等级错误：", exception.Message));
				logHelper.WriteError(errorMsg);
				str = "";
			}
			return str;
		}
	}
}
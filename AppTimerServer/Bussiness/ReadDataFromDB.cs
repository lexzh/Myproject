using System;
using System.Data;
using Library;

namespace Bussiness
{
	public class ReadDataFromDB
	{
		public ReadDataFromDB()
		{
		}

		public static void execSaveGpsLogTable(int sWrkId, int sOrderId, string sCarId, int sOptcode, string sMsgText)
		{
			string str = sMsgText.Replace("'", "‘");
			string str1 = "";
			try
			{
				str1 = string.Concat(str1, " insert into ");
				str1 = string.Concat(str1, " GpsLog(wrkid,orderid,userid,carid,optcode,OptCodeDetail,isSuccess,description,optTime) ");
				str1 = string.Concat(str1, " values({0},{1},'{2}',{3}, 13, {4},-1,'{5}',getDate())");
				object[] objArray = new object[] { sWrkId, sOrderId, ReadDataFromXml.ExecUserId, sCarId, sOptcode, str };
				str1 = string.Format(str1, objArray);
				SqlDataAccess.insertBySql(str1);
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg()
				{
					ClassName = "ReadDataFromDB",
					FunctionName = "execSaveGpsLogTable",
					ErrorText = string.Concat("保存下发内容到GpsLog时发生错误!", exception.Message, ",SQL:", str1)
				};
				(new LogHelper()).WriteError(errorMsg, exception);
			}
		}

		public static DateTime GetSvrTime()
		{
			DateTime now;
			try
			{
				DataTable dataBySql = SqlDataAccess.getDataBySql(" select getdate() ");
				now = (dataBySql.Rows.Count <= 0 || dataBySql == null ? DateTime.Now : DateTime.Parse(dataBySql.Rows[0][0].ToString()));
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				LogHelper logHelper = new LogHelper();
				ErrorMsg errorMsg = new ErrorMsg()
				{
					ClassName = "ReadDataFromDB",
					FunctionName = "GetSvrTime",
					ErrorText = string.Concat("获取数据库服务器时间发生错误!", exception.Message)
				};
				logHelper.WriteError(errorMsg, exception);
				now = DateTime.Now;
			}
			return now;
		}
	}
}
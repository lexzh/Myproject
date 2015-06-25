using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Library;

namespace Bussiness
{
	public class SqlDataAccess
	{
		private const int _DbOutTime = 4800;

		private static string m_ConnectionString;

		static SqlDataAccess()
		{
			SqlDataAccess.m_ConnectionString = FileHelper.ReadParamFromXml("ConnectionString");
		}

		public SqlDataAccess()
		{
		}

		public static void AddParam(SqlCommand pCmd, DbParameter[] pParam)
		{
			if (pParam != null)
			{
				DbParameter[] dbParameterArray = pParam;
				for (int i = 0; i < (int)dbParameterArray.Length; i++)
				{
					SqlParameter sqlParameter = (SqlParameter)((ICloneable)dbParameterArray[i]).Clone();
					pCmd.Parameters.Add(sqlParameter);
				}
			}
		}

		public static int ExecuteNoQueryBySp(string pSpName, SqlConnection connection, DbParameter[] pSpParam, int pTimeOut)
		{
			int num = 0;
			using (SqlCommand sqlCommand = new SqlCommand(pSpName, connection))
			{
				sqlCommand.CommandType = CommandType.StoredProcedure;
				sqlCommand.CommandTimeout = pTimeOut;
				SqlDataAccess.AddParam(sqlCommand, pSpParam);
				num = sqlCommand.ExecuteNonQuery();
			}
			return num;
		}

		private static int ExecuteNoQueryBySql(string pSql, SqlConnection connection, int pTimeOut)
		{
			int num = 0;
			using (SqlCommand sqlCommand = new SqlCommand(pSql, connection))
			{
				sqlCommand.CommandTimeout = pTimeOut;
				num = sqlCommand.ExecuteNonQuery();
			}
			return num;
		}

		public static SqlConnection GetConnection()
		{
			return new SqlConnection(SqlDataAccess.m_ConnectionString);
		}

		public static DataTable getDataBySP(string pSpName, DbParameter[] pSqlParam)
		{
			DataTable dataTable = new DataTable();
			using (SqlConnection connection = SqlDataAccess.GetConnection())
			{
				using (SqlCommand sqlCommand = new SqlCommand(pSpName, connection))
				{
					sqlCommand.CommandType = CommandType.StoredProcedure;
					SqlDataAccess.AddParam(sqlCommand, pSqlParam);
					connection.Open();
					using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
					{
						dataTable.Load(sqlDataReader);
					}
				}
			}
			return dataTable;
		}

		public static DataTable getDataBySql(string pStrSql)
		{
			DataTable dataTable = new DataTable();
			using (SqlConnection connection = SqlDataAccess.GetConnection())
			{
				using (SqlCommand sqlCommand = new SqlCommand(pStrSql, connection))
				{
					connection.Open();
					sqlCommand.CommandTimeout = 4800;
					using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
					{
						dataTable.Load(sqlDataReader);
					}
				}
			}
			return dataTable;
		}

		public static string getStringBySP(string pSpName, DbParameter[] pSqlParam)
		{
			DataTable dataTable = new DataTable();
			string str = "";
			using (SqlConnection connection = SqlDataAccess.GetConnection())
			{
				using (SqlCommand sqlCommand = new SqlCommand(pSpName, connection))
				{
					sqlCommand.CommandType = CommandType.StoredProcedure;
					SqlDataAccess.AddParam(sqlCommand, pSqlParam);
					connection.Open();
					object obj = sqlCommand.ExecuteScalar();
					if (obj != null)
					{
						str = obj.ToString();
					}
				}
			}
			return str;
		}

		public static int insertBySp(string pSpName, DbParameter[] pSpParam)
		{
			int num = 0;
			using (SqlConnection connection = SqlDataAccess.GetConnection())
			{
				connection.Open();
				num = SqlDataAccess.ExecuteNoQueryBySp(pSpName, connection, pSpParam, 4800);
			}
			return num;
		}

		public static int insertBySql(string pInsertSql)
		{
			int num = 0;
			using (SqlConnection connection = SqlDataAccess.GetConnection())
			{
				connection.Open();
				num = SqlDataAccess.ExecuteNoQueryBySql(pInsertSql, connection, 4800);
			}
			return num;
		}
	}
}
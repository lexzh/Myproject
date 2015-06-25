using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ParamLibrary.GpsEntity
{
	[DataContract]
	public class GpsDataTable
	{
		private const string OUTOFINDEX = "索引超出界限!";

		private const string OUTOFCOLUMNS = "不存在的列!";

		private const int MAXSIZE = 100;

		private Dictionary<string, int> m_Columns;

		[DataMember]
		public List<string[]> Rows;

		[DataMember]
		public Dictionary<string, int> Columns
		{
			get
			{
				return this.m_Columns;
			}
			set
			{
				this.m_Columns = value;
			}
		}

		public GpsDataTable(Dictionary<string, int> columns)
		{
			if (columns != null)
			{
				this.m_Columns = columns;
			}
			else
			{
				this.m_Columns = new Dictionary<string, int>();
			}
			this.Rows = new List<string[]>();
		}

		public object GetValues(int iRows, string strFieldName)
		{
			if (iRows > this.Rows.Count)
			{
				throw new Exception("索引超出界限!");
			}
			if (!this.m_Columns.ContainsKey(strFieldName))
			{
				throw new Exception("不存在的列!");
			}
			int item = this.m_Columns[strFieldName];
			return this.Rows[iRows][item];
		}

		public int InsertRows(object[] newrow)
		{
			int num;
			try
			{
				string[] str = new string[this.Columns.Count];
				for (int i = 0; i < this.Columns.Count; i++)
				{
					object obj = newrow[i];
					if (!(obj is DBNull))
					{
						str[i] = obj.ToString();
					}
					else
					{
						str[i] = "";
					}
				}
				this.Rows.Add(str);
				num = 0;
			}
			catch
			{
				num = -1;
			}
			return num;
		}
	}
}
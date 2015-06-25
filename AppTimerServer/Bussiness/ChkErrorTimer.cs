using PublicClass;
using Remoting;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Timers;
using Library;
using ParamLibrary.Entity;
using ParamLibrary.CmdParamInfo;
using ParamLibrary.Application;

namespace Bussiness
{
	public class ChkErrorTimer : ProcessBase
	{
		private DownData myDownData = new DownData(0);

		private System.Timers.Timer tChkErrorTimer;

		private int iChkErrTime = 60000 * ReadDataFromXml.ChkErrDiff;

		private bool bIsAppOk = true;

		private bool bIsLinkOk = true;

		private bool bIsM2MOk = true;

		private bool bIsProxyOk = true;

		private bool bAllOk = true;

		public ChkErrorTimer()
		{
		}

		private void ChkErrorMain()
		{
			if (this.Test())
			{
				DataTable dataTable = this.execChkError();
				if (dataTable != null && dataTable.Rows.Count > 0)
				{
					int num = Convert.ToInt32(dataTable.Rows[0]["Link"]);
					int num1 = Convert.ToInt32(dataTable.Rows[0]["M2M"]);
					int num2 = Convert.ToInt32(dataTable.Rows[0]["Proxy"]);
					Record.execFileRecord("StarG5故障检测", string.Format("星网非标准终端在线数：{0}；M2M标准终端在线数：{1}；原物流通终端在线数：{2}", num, num1, num2));
					if (num < 20)
					{
						if (this.bIsLinkOk)
						{
							this.bAllOk = false;
							this.bIsLinkOk = false;
							this.execSendErrMsg("车务通平台故障：星网非标准终端大量掉线，请尽快排查！");
							return;
						}
					}
					else if (num1 < 5)
					{
						if (this.bIsM2MOk)
						{
							this.bAllOk = false;
							this.bIsM2MOk = false;
							this.execSendErrMsg("车务通平台故障：M2M标准终端大量掉线，请尽快排查！");
							return;
						}
					}
					else if (num2 < 20)
					{
						if (this.bIsProxyOk)
						{
							this.bAllOk = false;
							this.bIsProxyOk = false;
							this.execSendErrMsg("车务通平台故障：原物流通终端大量掉线，请尽快排查！");
							return;
						}
					}
					else if (!this.bAllOk)
					{
						this.execSendErrMsg("车务通平台故障已恢复！");
						this.bAllOk = true;
						this.bIsProxyOk = true;
						this.bIsM2MOk = true;
						this.bIsLinkOk = true;
						this.bIsAppOk = true;
					}
					dataTable.Dispose();
					dataTable = null;
				}
			}
			else if (this.bIsAppOk)
			{
				this.bAllOk = false;
				this.bIsAppOk = false;
				this.execSendErrMsg("车务通平台故障：业务处理服务器无法正常登录，请尽快排查！");
				return;
			}
		}

		private DataTable execChkError()
		{
			DataTable dataTable = null;
			try
			{
				string str = string.Concat("", " select ISNULL(SUM(case when b.TerminalTypeId in (40,41) then 1 else 0 end), 0) as Proxy, ");
				str = string.Concat(str, " ISNULL(SUM(case when (c.AreaCode like '01124W%'or c.AreaCode like '0131010501%') then 1 else 0 end), 0) as M2M , ");
				str = string.Concat(str, " ISNULL(SUM(case when b.TerminalTypeId = 3 then 1 else 0 end), 0) as Link ");
				str = string.Concat(str, " from GpsCarCurrentPosInfo a  ");
				str = string.Concat(str, " inner join GisCar b on a.telephone = b.SimNum ");
				str = string.Concat(str, " inner join GpsArea c on b.AreaId = c.AreaID ");
				str = string.Concat(str, " where a.LastUpdateTime > dateadd(mi, -{0}, getDate()) ");
				str = string.Format(str, ReadDataFromXml.DelayTime);
				dataTable = RemotingClient.ExecSql(str);
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg()
				{
					ClassName = "ChkErrorTimer",
					FunctionName = "execChkError",
					ErrorText = "获取故障检测数据时发生错误!"
				};
				(new LogHelper()).WriteError(errorMsg, exception);
			}
			return dataTable;
		}

		private bool execConnection()
		{
			Response response = RemotingClient.LoginSys_Login(false, true);
			if (response.ResultCode != (long)0)
			{
				Record.execFileRecord("用户登录", string.Format("{0}登录失败：{1}", Variable.sUserId, response.ErrorMsg));
				return false;
			}
			LogHelper logHelper = new LogHelper();
			LogMsg logMsg = new LogMsg()
			{
				Msg = string.Concat("用户登录", string.Format("{0}登录成功：{1}", Variable.sUserId, response.ErrorMsg))
			};
			logHelper.WriteLog(logMsg);
			return true;
		}

		private void execSendErrMsg(string sErrMsg)
		{
			TxtMsg txtMsg = new TxtMsg()
			{
				MsgType = CmdParam.MsgType.UCS2手机短信,
				OrderCode = CmdParam.OrderCode.调度,
				strMsg = sErrMsg
			};
			Record.execFileRecord("发送故障报警", sErrMsg);
			string[] strArrays = ReadDataFromXml.Linkman.Split(new char[] { ',' });
			for (int i = 0; i < (int)strArrays.Length; i++)
			{
				string str = strArrays[i];
				if (!string.IsNullOrEmpty(str))
				{
					this.myDownData.icar_SendTxtMsg(CmdParam.ParamType.SimNum, str, "", CmdParam.CommMode.短信, txtMsg, 0, "故障检测通知");
				}
			}
			MailMessage mailMessage = new MailMessage();
			if (string.IsNullOrEmpty(ReadDataFromXml.sLinkmanMailTo))
			{
				return;
			}
			string[] strArrays1 = ReadDataFromXml.sLinkmanMailTo.Split(new char[] { ';' });
			for (int j = 0; j < (int)strArrays1.Length; j++)
			{
				string str1 = strArrays1[j];
				if (!string.IsNullOrEmpty(str1))
				{
					string[] strArrays2 = str1.Split(new char[] { '(' });
					if ((int)strArrays2.Length == 2)
					{
						MailAddressCollection to = mailMessage.To;
						string str2 = strArrays2[1];
						char[] chrArray = new char[] { ')' };
						((Collection<MailAddress>)to).Add(new MailAddress(str2.Trim(chrArray), strArrays2[0]));
					}
					else if ((int)strArrays2.Length == 1)
					{
						mailMessage.To.Add(strArrays2[0]);
					}
				}
			}
			string[] strArrays3 = ReadDataFromXml.sLinkmanMailCC.Split(new char[] { ';' });
			for (int k = 0; k < (int)strArrays3.Length; k++)
			{
				string str3 = strArrays3[k];
				if (!string.IsNullOrEmpty(str3))
				{
					string[] strArrays4 = str3.Split(new char[] { '(' });
					if ((int)strArrays4.Length == 2)
					{
						MailAddressCollection cC = mailMessage.CC;
						string str4 = strArrays4[1];
						char[] chrArray1 = new char[] { ')' };
						((Collection<MailAddress>)cC).Add(new MailAddress(str4.Trim(chrArray1), strArrays4[0]));
					}
					else if ((int)strArrays4.Length == 1)
					{
						mailMessage.CC.Add(strArrays4[0]);
					}
				}
			}
			mailMessage.From = new MailAddress("GpsSystemCheck@star-net.cn", "车务通检测平台", Encoding.Default);
			mailMessage.Subject = "故障检测通知";
			DateTime now = DateTime.Now;
			mailMessage.Body = string.Format("时间：{0}\r\n{1}", now.ToString("yyyy-MM-dd HH:mm:ss"), sErrMsg);
			mailMessage.SubjectEncoding = Encoding.GetEncoding(936);
			mailMessage.BodyEncoding = Encoding.GetEncoding(936);
			SmtpClient smtpClient = new SmtpClient("mail.star-net.cn")
			{
				Credentials = new NetworkCredential("zhangbin@star-net.cn", "zb1234")
			};
			try
			{
				smtpClient.UseDefaultCredentials = false;
				smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
				smtpClient.Send(mailMessage);
				(new LogHelper()).WriteText("故障邮件发送成功");
			}
			catch (Exception exception)
			{
				ErrorMsg errorMsg = new ErrorMsg("故障检测通知", "发送故障邮件", exception.Message)
				{
					ClassName = "execSendErrMsg"
				};
				(new LogHelper()).WriteError(errorMsg);
			}
		}

		private void onChkErrorMain(object sender, ElapsedEventArgs e)
		{
			this.tChkErrorTimer.Enabled = false;
			try
			{
				try
				{
					this.ChkErrorMain();
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					ErrorMsg errorMsg = new ErrorMsg("ChkErrorTimer", "onChkErrorMain", string.Concat("StarG5故障检测定时器", exception.Message));
					this.logHelper.WriteError(errorMsg);
				}
			}
			finally
			{
				this.tChkErrorTimer.Enabled = true;
			}
		}

		public override void start()
		{
			try
			{
				try
				{
					Variable.sServerIp = ReadDataFromXml.AppIp;
					Variable.sPort = ReadDataFromXml.AppPort;
					Variable.sUserId = ReadDataFromXml.AppUser;
					Variable.sPassword = ReadDataFromXml.AppPwd;
					this.execConnection();
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					ErrorMsg errorMsg = new ErrorMsg("ChkErrorTimer", "start", string.Concat("启动故障检测失败", exception.Message));
					this.logHelper.WriteError(errorMsg);
				}
				this.tChkErrorTimer = new System.Timers.Timer((double)this.iChkErrTime);
				this.tChkErrorTimer.Elapsed += new ElapsedEventHandler(this.onChkErrorMain);
				this.tChkErrorTimer.Enabled = true;
			}
			catch (Exception exception3)
			{
				Exception exception2 = exception3;
				this.tChkErrorTimer.Enabled = true;
				ErrorMsg errorMsg1 = new ErrorMsg("ChkErrorTimer", "start", string.Concat("启动故障检测失败Ex", exception2.Message));
				this.logHelper.WriteError(errorMsg1);
			}
		}

		public override void stop()
		{
			this.tChkErrorTimer.Stop();
		}

		private bool Test()
		{
			bool flag;
			int num = 0;
			bool flag1 = false;
			while (num < 3)
			{
				flag1 = RemotingClient.Test();
				num++;
				if (flag1)
				{
					break;
				}
				try
				{
					Thread.Sleep(10000);
					flag1 = this.execConnection();
					continue;
				}
				catch (InvalidOperationException invalidOperationException1)
				{
					InvalidOperationException invalidOperationException = invalidOperationException1;
					if (Variable.bLogin)
					{
						Record.execFileRecord("心跳", invalidOperationException.ToString());
					}
					flag = false;
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					if (Variable.bLogin)
					{
						Record.execFileRecord("心跳", exception.ToString());
					}
					flag = false;
				}
				return flag;
			}
			return flag1;
		}
	}
}
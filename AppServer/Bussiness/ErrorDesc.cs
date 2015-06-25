namespace Bussiness
{
    using System;
    using System.Collections;

    public class ErrorDesc
    {
        private static Hashtable _ErrorMsgDetailDesc;
        private static Hashtable ErrorMsgList;

        static ErrorDesc()
        {
            old_acctor_mc();
        }

        public static string GetDetailErrorMessage(int int_0)
        {
            if (_ErrorMsgDetailDesc.Count <= 0)
            {
                _ErrorMsgDetailDesc.Add(0, "重新交换密钥成功");
                _ErrorMsgDetailDesc.Add(0xf021, "无工作密钥");
                _ErrorMsgDetailDesc.Add(0xf015, "解密失败");
                _ErrorMsgDetailDesc.Add(0xf011, "解压失败");
                _ErrorMsgDetailDesc.Add(0xf022, "终端未注册");
                _ErrorMsgDetailDesc.Add(0xf016, "CRC校验失败");
            }
            string str = string.Empty;
            if (_ErrorMsgDetailDesc.ContainsKey(int_0))
            {
                str = (string) _ErrorMsgDetailDesc[int_0];
            }
            return str;
        }

        public static string GetErrorMessage(int int_0)
        {
            if (ErrorMsgList.Count <= 0)
            {
                ErrorMsgList.Add(0xf047, "下行命令拆分成过多短信。");
                ErrorMsgList.Add(0xf121, "vip客户编码不存在。");
                ErrorMsgList.Add(0xf105, "非法调用接口。");
                ErrorMsgList.Add(0xf107, "数据库访问或者SQL语句出错。");
                ErrorMsgList.Add(0xf109, "命令参数类型错误。");
                ErrorMsgList.Add(0xf116, "该车辆的油箱参考值信息不完整。");
                ErrorMsgList.Add(0xf117, "组包错误。");
                ErrorMsgList.Add(0xf115, "该用户没有登陆此模块的权限 。");
                ErrorMsgList.Add(0xf114, "没有任何轨迹 。");
                ErrorMsgList.Add(0xf110, "载入HuahanLED.dll失败。");
                ErrorMsgList.Add(0xf111, "预设路线名称重复。");
                ErrorMsgList.Add(0xf112, "预设路线个数超过最大值。");
                ErrorMsgList.Add(0xf113, "预设路线的经纬度报文全部为0。");
                ErrorMsgList.Add(0xf104, "命令频度限制,请与管理员联系。");
                ErrorMsgList.Add(0xf044, "通讯服务器自动升级模式下不支持手动升级。");
                ErrorMsgList.Add(0xf049, "当下发多种条件监控后实时监控所选监控次数不能为0");
                ErrorMsgList.Add(0xf103, "上次多种条件图像监控未完成，'+#13+'请等待监控完成，或者停止图像监控。");
                ErrorMsgList.Add(0xf102, "该车辆服务已到期。");
                ErrorMsgList.Add(10, "用户名或密码不正确，请重新输入！");
                ErrorMsgList.Add(100, "用户密码错误。");
                ErrorMsgList.Add(0x65, "没有找到符合条件的数据。");
                ErrorMsgList.Add(0x67, "保存配置文件出错。");
                ErrorMsgList.Add(0x66, "访问数据库出错。");
                ErrorMsgList.Add(0x68, "与服务器通讯失败。");
                ErrorMsgList.Add(200, "连接应用服务器失败。");
                ErrorMsgList.Add(0xff, "选择区域个数不能超过255个!");
                ErrorMsgList.Add(0x100, "区域点数不能超过5000个!");
                ErrorMsgList.Add(0x3e9, "监控工作站版本太低，请与管理员联系!");
                ErrorMsgList.Add(-1, "服务器未知错误。");
                ErrorMsgList.Add(0xf000, "通讯服务器未知的异常。");
                ErrorMsgList.Add(0xf001, "通讯服务器内存分配或访问出错。");
                ErrorMsgList.Add(0xf002, "函数调用参数错误。");
                ErrorMsgList.Add(0xf003, "电话号码格式有误。");
                ErrorMsgList.Add(0xf004, "通讯服务器已达到最大连接数。");
                ErrorMsgList.Add(0xf010, "通讯服务器zlib压缩出错。");
                ErrorMsgList.Add(0xf011, "通讯服务器zlib解压缩出错。");
                ErrorMsgList.Add(0xf012, "车台未交换密钥。");
                ErrorMsgList.Add(0xf013, "获取车台密钥失败。");
                ErrorMsgList.Add(0xf014, "3des加密失败。");
                ErrorMsgList.Add(0xf015, "3des解密失败。");
                ErrorMsgList.Add(0xf016, "crc检验错误。");
                ErrorMsgList.Add(0xf017, "车台报文解析错误。");
                ErrorMsgList.Add(0xf020, "操作key文件错误。");
                ErrorMsgList.Add(0xf021, "该车无工作密钥。");
                ErrorMsgList.Add(0xf022, "尚未设置终端ID（车台本机号）。");
                ErrorMsgList.Add(0xf023, "未设置汇报中心。");
                ErrorMsgList.Add(0xf031, "网络错误。");
                ErrorMsgList.Add(0xf032, "命令发送失败。");
                ErrorMsgList.Add(0xf033, "车台响应超时。");
                ErrorMsgList.Add(0xf034, "通讯服务器命令队列已满。");
                ErrorMsgList.Add(0xf035, "通讯服务器未启动或已停止。");
                ErrorMsgList.Add(0xf036, "系统通讯失败。");
                ErrorMsgList.Add(0xf005, "通讯服务器注册表信息丢失。");
                ErrorMsgList.Add(0xf037, "启动通讯服务器失败。");
                ErrorMsgList.Add(0xf038, "停止通讯服务器失败。");
                ErrorMsgList.Add(0xf039, "通讯服务器找不到无线猫。");
                ErrorMsgList.Add(0xf100, "服务器未找到升级文件的注册表信息。");
                ErrorMsgList.Add(0xf040, "通讯服务器读取升级文件失败。");
                ErrorMsgList.Add(0xf041, "车台不在线或者通信网络暂时无信号，可能是无GPRS/CDMA连接，短信服务未启动或已禁用。请尝试重新发送");
                ErrorMsgList.Add(0xf042, "该车辆终端不支持此操作。");
                ErrorMsgList.Add(0xf119, "预设路线时混凝土区域名称重复");
            }
            if (ErrorMsgList.ContainsKey(int_0))
            {
                return (string) ErrorMsgList[int_0];
            }
            return "";
        }

        private static void old_acctor_mc()
        {
            _ErrorMsgDetailDesc = new Hashtable(0x11);
            ErrorMsgList = new Hashtable(0x4f);
        }
    }
}


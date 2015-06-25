namespace Bussiness
{
    using ParamLibrary.Application;
    using ParamLibrary.CmdParamInfo;
    using Protocol;
    using System;

    public class UpdataIODeviceHandler
    {
        private DownDataIODevice downDataIODevice_0;

        public void HandleIODeviceAttachInfo(IODeviceAttachInfo iodeviceAttachInfo_0)
        {
            if (iodeviceAttachInfo_0 != null)
            {
                try
                {
                    if (iodeviceAttachInfo_0.InfoID == 1)
                    {
                        this.method_0(iodeviceAttachInfo_0);
                    }
                }
                catch
                {
                }
            }
        }

        private void method_0(IODeviceAttachInfo iodeviceAttachInfo_0)
        {
            IODeviceTextMsg msg = iodeviceAttachInfo_0 as IODeviceTextMsg;
            TxtMsg msg2 = new TxtMsg {
                OrderCode = CmdParam.OrderCode.调度,
                MsgType = CmdParam.MsgType.详细调度信息,
                strMsg = msg.Message
            };
            if (this.downDataIODevice_0 == null)
            {
                this.downDataIODevice_0 = new DownDataIODevice(3, false, true, true);
            }
            this.downDataIODevice_0.icar_SendTxtMsg(CmdParam.ParamType.SimNum, msg.SimNum, "", CmdParam.CommMode.未知方式, msg2);
        }
    }
}


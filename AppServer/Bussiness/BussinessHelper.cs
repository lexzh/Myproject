namespace Bussiness
{
    using System;
    using System.Threading;
    using Library;

    public class BussinessHelper
    {
        public static UpdataNewPosition upNewPosition;
        public static UpDataOtherData upOtherData;
        public static UpDataIODeviceData upOutEquipmentData;
        public static UpdataReachCar upReachTime;
        public static UpdataResponseData upResponse;

        static BussinessHelper()
        {
            upNewPosition = new UpdataNewPosition();
            upReachTime = new UpdataReachCar();
            upResponse = new UpdataResponseData();
            upOutEquipmentData = new UpDataIODeviceData();
            upOtherData = new UpDataOtherData();
        }

        /// <summary>
        /// 初始化程序数据，开启线程
        /// </summary>
        public static void AppInitDataAndStarRun()
        {
            try
            {
                CarDataInfoBuffer.LoadAllCarInfoList();
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("BussinessHelper", "StartRun", "车辆缓存信息加载错误:" + exception.Message);
                new LogHelper().WriteError(msg);
                
                Thread.Sleep(0x1388);
            }
            try
            {
                CarAlarmType.LoadAllCarAlarmTypeList();
            }
            catch (Exception exception2)
            {
                ErrorMsg msg2 = new ErrorMsg("BussinessHelper", "StartRun", "报警类型数据加载错误:" + exception2.Message);
                new LogHelper().WriteError(msg2);
                Thread.Sleep(0x1388);
            }
            try
            {
                AlamStatus.LoadAllAlarmStatu();
            }
            catch (Exception exception3)
            {
                ErrorMsg msg3 = new ErrorMsg("BussinessHelper", "StartRun", "报警状态数据加载错误:" + exception3.Message);
                new LogHelper().WriteError(msg3);
                Thread.Sleep(0x1388);
            }
            upNewPosition.Start();
            upResponse.Start();
            upOutEquipmentData.Start();
            upOtherData.Start();
        }
    }
}


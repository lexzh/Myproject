namespace Bussiness
{
    using DataAccess;
    using System;
    using System.Data;
    using System.Data.SqlClient;

    public sealed class Menu
    {
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="moduleId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public DataTable GetAllMenu(string userId, int moduleId, int groupId)
        {
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@vchrUserId", userId), new SqlParameter("@viModuleId", moduleId), new SqlParameter("@iGroupId", groupId) };
            string str = "WebGpsClient_GetMenuInfoAll";
            return new SqlDataAccess().getDataBySP(str, parameterArray);
        }
    }
}


using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;

namespace ClientInterface
{
    /// <summary>
    ///     ServiceClient 的摘要说明
    ///     管理系统上传Teetime以及下载预约时间接口
    /// </summary>
    [WebService(Namespace = "http://192.168.0.247/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class ServiceClient : WebService
    {
        /// <summary>
        /// 上传TeeTime
        /// </summary>
        /// <param name="teetimeDataSet">上传所需TeeTime数据集</param>
        [WebMethod(Description = "上传Teetime接口")]
        public void UpdateTeetimeMethod(DataSet teetimeDataSet)
        {
            try
            {
                var insertSql =
                    "INSERT INTO [GroupSource_TeeTime] ([TeeTime], [MaxNum], [UseNum], [ResID], [Create_day]) VALUES (@p1, @p2, @p3, @p4, @p5)";
                List<CommandInfo> cmdList = new List<CommandInfo>();
                foreach (DataRow dr in teetimeDataSet.Tables["ds"].Rows)
                {
                    var sqlParameters = new SqlParameter[5];
                    sqlParameters[0] = new SqlParameter("@p1", dr["TeeTime"].ToString());
                    sqlParameters[1] = new SqlParameter("@p2", dr["MaxNum"].ToString());
                    sqlParameters[2] = new SqlParameter("@p3", dr["UseNum"].ToString());
                    sqlParameters[3] = new SqlParameter("@p4", dr["ResID"].ToString());
                    sqlParameters[4] = new SqlParameter("@p5", dr["Create_day"].ToString());

                    cmdList.Add(new CommandInfo(insertSql, sqlParameters));
                }
                DbHelperSQL.ExecuteSqlTran(cmdList);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        /// <summary>
        /// 获取预订时间接口
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "获取预订时间接口")]
        public DataSet GetAppointmentMethod()
        {
            DataSet resultDataSet = null;
            try
            {
                var strSql = @"SELECT   [ORDER_NUMBER] ,
                                        [FULL_NAME] ,
                                        [COURT_ID] ,
                                        [COURT_NAME] ,
                                        [TEETIME] ,
                                        [NUMBER_OF_PEOPLE] ,
                                        [HOLE_NUMBER] ,
                                        [CONTACT_NUMBER] ,
                                        [WHETHER_TO_PAY] ,
                                        [PAYMENT] ,
                                        [SERIAL_NUMBER] ,
                                        [PAYMENT_AMOUNT] ,
                                        [REFUND_AMOUNT] ,
                                        [CANCELL]
                                FROM    [GroupSource_TeeTime_Appointment]";
                resultDataSet = DbHelperSQL.Query(strSql);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
            return resultDataSet;
        }
    }
}
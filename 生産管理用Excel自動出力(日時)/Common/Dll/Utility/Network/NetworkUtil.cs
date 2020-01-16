using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using System.IO;
using System.Text;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Net.NetworkInformation;
//*************************************************************************************
//
//   ネットワークユーティリティ
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************

namespace Utility.Common.Network
{
    /// <summary>
    /// ネットワークユーティリティ
    /// </summary>
    public class NetworkUtil
    {
        #region メソッド

        /// <summary>
        /// Pingの実行
        /// </summary>
        /// <param name="ipAddress">IPアドレス</param>
        /// <param name="timeOutPeriod">タイムアウト時間</param>
        /// <param name="retryCount">リトライ回数</param>
        /// <returns>結果</returns>
        public static bool ExcutePing(string ipAddress, int timeOutPeriod, int retryCount)
        {
            bool ret = false;

            try
            {
                for (int i = 0; i < retryCount; i++)
                {
                    Ping ping = new Ping();

                    // タイムアウト時間を指定してPing実行する
                    PingReply pingReply = ping.Send(ipAddress, timeOutPeriod);
                    if (pingReply.Status == IPStatus.Success)
                    {
                        ret = true;
                        break;
                    }
                }

                return ret;
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
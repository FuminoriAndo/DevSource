//*************************************************************************************
//
//   Log4Net用のクリーナー
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ   担当者    修正内容
//   18.09.01             DSK吉武   新規作成
//
//*************************************************************************************
using System;
using System.IO;
using System.Linq;
using log4net;
using log4net.Appender;
using Utility.Core;

namespace Cleaner.Log4Net
{
    /// <summary>
    /// Log4Net用のクリーナー
    /// </summary>
    public class Log4NetCleaner
    {
        /// <summary>
        /// ログの世代管理の削除
        /// </summary>
        public void CleanUp()
        {
            try
            {
                var repo = LogManager.GetAllRepositories().FirstOrDefault();

                if (repo == null)
                {
                    throw new NotSupportedException("Log4Netが設定されていません");
                }

                var app = repo.GetAppenders().FirstOrDefault(x => x.GetType() == typeof(RollingFileAppender));

                if (app != null)
                {
                    var appender = app as RollingFileAppender;

                    var directory = Path.GetDirectoryName(appender.File);
                    var filePrefix = Path.GetFileName(appender.File);
                    filePrefix = filePrefix.Substring(0, filePrefix.Length - appender.DatePattern.Replace("\"", "").Length);

                    // ログの最終保持日付
                    var targetDate = DateTime.Now.Date.AddDays(-appender.MaxSizeRollBackups);

                    // 削除の実行
                    CleanUp(directory, filePrefix, targetDate, appender);
                }
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ログの世代管理の削除の実行
        /// </summary>
        /// <param name="logDirectory">削除対象のディレクトリ.</param>
        /// <param name="logPrefix">ファイルのフォーマット名</param>
        /// <param name="lastDate">世代管理の開始日</param>
        /// <param name="appender">RollingFileAppender</param>
        private void CleanUp(string logDirectory, string logPrefix, DateTime lastDate, RollingFileAppender appender)
        {
            try
            {
                if (string.IsNullOrEmpty(logDirectory))
                {
                    throw new ArgumentException("ログのディレクトリが存在しません");
                }

                if (string.IsNullOrEmpty(logDirectory))
                {
                    throw new ArgumentException("ログのファイルフォーマット名の形式が存在しません");
                }

                var dirInfo = new DirectoryInfo(logDirectory);
                if (!dirInfo.Exists)
                {
                    return;
                }

                // 削除
                dirInfo.GetFiles(String.Format("{0}*.*", logPrefix))
                    .Where(info => GetLogDate(info, logPrefix, appender) < lastDate)
                    .ForEach(info =>
                    {
                        if ((info.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                            info.Attributes = FileAttributes.Normal;
                        // 削除
                        info.Delete();
                    });
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ログの作成日付の取得
        /// </summary>
        /// <param name="fileInfo">ファイル情報オブジェクト</param>
        /// <param name="logPrefix">ログファイル名のフォーマット</param>
        /// <param name="app">ログのアペンダー</param>
        /// <returns>ログの作成日付</returns>
        private DateTime GetLogDate(FileInfo fileInfo, string logPrefix, RollingFileAppender app)
        {
            try
            {
                var year = fileInfo.Name.Substring(logPrefix.Length + app.DatePattern.IndexOf("yyyy"), 4);
                var month = fileInfo.Name.Substring(logPrefix.Length + app.DatePattern.IndexOf("MM"), 2);
                var day = fileInfo.Name.Substring(logPrefix.Length + app.DatePattern.IndexOf("dd"), 2);

                return new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), Convert.ToInt32(day));
            }

            catch (Exception)
            {
                throw;
            }
        }
    }
}

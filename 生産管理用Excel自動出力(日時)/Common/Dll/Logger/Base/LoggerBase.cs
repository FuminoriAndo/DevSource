//*************************************************************************************
//
//   ロガーの基底
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ   担当者    修正内容
//   18.09.01             DSK吉武   新規作成
//
//*************************************************************************************
using System;
using System.Collections.Generic;
using System.IO;

namespace Logger.Base
{
    /// <summary>
    /// ロガーの基底
    /// </summary>
    public class LoggerBase : IDisposable
    {
        #region フィールド

        /// <summary>
        /// ログライター
        /// </summary>
        private static StreamWriter logWriter = null;

        /// <summary>
        /// ログフォルダ名
        /// </summary>
        private readonly string logFolderName = null;

        /// <summary>
        /// ログファイル名(デフォルト)
        /// </summary>
        private readonly string logDefaultFileName = null;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="logFolderName">ログフォルダ名</param>
        /// <param name="logDefaultFlieName">ログファイル名(デフォルト)</param>
        protected LoggerBase(string logFolderName, string logDefaultFileName)
        {
            this.logFolderName = logFolderName;
            this.logDefaultFileName = logDefaultFileName;
            Initialize();
        }

        #endregion

        #region メソッド

        /// <summary>
        /// 初期処理
        /// </summary>
        /// <param name="logMessage">メッセージ</param>
        public virtual void Initialize()
        {
            try
            {
                // ログフォルダが存在するか
                if (!Directory.Exists(logFolderName))
                {
                    // 存在しない場合、ログフォルダを作成する。
                    Directory.CreateDirectory(logFolderName);
                }

                var logFilePath = logFolderName + @"\" + DateTime.Now.ToString("yyyyMMdd") + "_" + logDefaultFileName;
                logWriter = new StreamWriter(logFilePath, true, System.Text.Encoding.GetEncoding("Shift_JIS"));
                logWriter.AutoFlush = true;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ログ書込み
        /// </summary>
        /// <param name="message">ログ出力メッセージ</param>
        public virtual void Write(string message)
        {
            try
            {
                var dateTimeNow = DateTime.Now;
                var ymd = dateTimeNow.ToString("yyyyMMdd");
                var hms = dateTimeNow.ToString("HH:mm:ss");

                logWriter.Write(ymd + "," + hms + "," + message + "\r\n");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ログの世代管理の削除
        /// </summary>
        public virtual void CleanUp()
        {
            try
            {
                // ログフォルダが存在するか
                if (Directory.Exists(logFolderName))
                {
                    var fileList = new List<String>();
                    var directoryInfo = new DirectoryInfo(logFolderName);
                    var files = directoryInfo.GetFiles("*.log");
                    var deleteTarget = DateTime.Now.AddMonths(-1).ToString("yyyyMMdd");

                    foreach (FileInfo file in files)
                    {
                        if (string.Compare(file.Name.Substring(0, 8), deleteTarget) < 0)
                        {
                            file.Delete();
                        }
                    }
                }
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public virtual void Terminate()
        {
            logWriter.Close();
            logWriter.Dispose();
            logWriter = null;
        }

        #region Disposeパターン

        /// <summary>
        /// 破棄
        /// </summary>
        /// <param name="disposing">破棄する/しない</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~LoggerBase()
        {
            Dispose(false);
        }

        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        #endregion

        #endregion
    }
}

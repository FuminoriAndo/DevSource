using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Windows.Forms;
using Logger.Base;
//*************************************************************************************
//
//   カスタムロガー
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ   担当者    修正内容
//   18.09.01             DSK吉武   新規作成
//
//*************************************************************************************
namespace ProductionControlDailyExcelCreator.Log
{
    /// <summary>
    /// カスタムロガー
    /// </summary>
    internal class CustomLogger : LoggerBase
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="logFolderName">ログフォルダ名</param>
        /// <param name="logDefaultFlieName">ログファイル名(デフォルト)</param>
        internal CustomLogger(string logFolderName, string logDefaultFileName)
            : base(logFolderName, logDefaultFileName)
        {
        }
    }
}

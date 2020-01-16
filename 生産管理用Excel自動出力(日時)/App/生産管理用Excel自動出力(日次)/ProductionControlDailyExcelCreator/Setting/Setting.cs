//*************************************************************************************
//
//   設定情報
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************
using ProductionControlDailyExcelCreator.Types;

namespace ProductionControlDailyExcelCreator.Setting
{
    /// <summary>
    /// 読込みファイル情報
    /// </summary>
    internal class ReadFileSetting
    {
        /// <summary>
        /// モデルの種類
        /// </summary>
        internal ModelType ModelType { get; set; }

        /// <summary>
        /// ファイル名
        /// </summary>
        internal string FileName { get; set; }

        /// <summary>
        /// 設定ファイル名
        /// </summary>
        internal string SettingFileName { get; set; }

        /// <summary>
        /// ファイル1行あたりの文字数
        /// </summary>
        internal int LengthPerLine { get; set; }
    }

    /// <summary>
    /// Excel情報
    /// </summary>
    internal class ExcelSetting
    {
        /// <summary>
        /// モデルの種類
        /// </summary>
        internal ModelType ModelType { get; set; }

        /// <summary>
        /// テンプレートブックのパス
        /// </summary>
        internal string TemplateBookPath { get; set; }

        /// <summary>
        /// 出力ブック名
        /// </summary>
        internal string OutputBookName { get; set; }

        /// <summary>
        /// 出力ブックのフォルダ
        /// </summary>
        internal string OutputBookFolder { get; set; }

        /// <summary>
        /// バックアップフォルダ
        /// </summary>
        internal string BackupFolder { get; set; }

    }

    /// <summary>
    /// Ping情報
    /// </summary>
    internal class PingSetting
    {
        /// <summary>
        /// モデルの種類
        /// </summary>
        internal string IPAddress { get; set; }

        /// <summary>
        /// タイムアウト時間
        /// </summary>
        internal int TimeoutPeriod { get; set; }

        /// <summary>
        /// リトライ回数
        /// </summary>
        internal int RetryCount { get; set; }
    }
}


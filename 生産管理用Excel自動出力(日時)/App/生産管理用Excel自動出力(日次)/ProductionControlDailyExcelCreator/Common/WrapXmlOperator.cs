//*************************************************************************************
//
//   XML操作(Utility)のラッパー
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************
using System;
using System.Collections.Generic;
using Utility.Core;
using ProductionControlDailyExcelCreator.Setting;
using ProductionControlDailyExcelCreator.Types;
using Utility.Xml;
using System.IO;
using System.Reflection;

namespace ProductionControlDailyExcelCreator.Common
{
    /// <summary>
    /// XML操作(Utility)のラッパー
    /// </summary>
    internal class WrapXmlOperator
    {
        #region フィールド

        /// <summary>
        /// シングルトン
        /// </summary>
        private static WrapXmlOperator singleton = null;

        /// <summary>
        /// XML操作オブジェクト
        /// </summary>
        private XmlOperator xmlOperator = null;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        private WrapXmlOperator()
        {
            // XMLファイルのパスの取得
            string xmlPath = Path.Combine(Path.GetDirectoryName(
            Assembly.GetExecutingAssembly().Location) ?? string.Empty, "ProductionControlDailyExcelCreatorSettings.xml");

            // XMLファイルを読込む
            xmlOperator = new XmlOperator(xmlPath);
        }

        #endregion

        #region メソッド

        /// <summary>
        /// インスタンスの取得
        /// </summary>
        /// <returns>WrapXMLOperatorのインスタンス</returns>
        internal static WrapXmlOperator GetInstance()
        {
            if (singleton == null) singleton = new WrapXmlOperator();
            return singleton;
        }

        /// <summary>
        /// 読込みフォルダの取得
        /// </summary>
        /// <returns>読込みフォルダ</returns>
        internal string GetReadFolderPath()
        {
            string path = string.Empty;

            try
            {
                var elem = xmlOperator.GetElement("/Settings/Input");
                path = xmlOperator.GetAttlibuteValue(elem, "Folder");
                return path;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 読込みフォルダのホスト名の取得
        /// </summary>
        /// <returns>読込みフォルダ</returns>
        internal string GetReadFolderHostName()
        {
            string path = string.Empty;

            try
            {
                var elem = xmlOperator.GetElement("/Settings/Input");
                path = xmlOperator.GetAttlibuteValue(elem, "HostName");
                return path;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 読込みファイル情報一覧の取得
        /// </summary>
        /// <returns>読込みファイル情報一覧</returns>
        internal IList<ReadFileSetting> GetReadFileSettings()
        {
            string path = string.Empty;
            var readFileProperties = new List<ReadFileSetting>();

            try
            {
                var elements = xmlOperator.GetElements("/Settings/Text/Input");
                elements.ForEach(item =>
                {
                    readFileProperties.Add(new ReadFileSetting()
                    {
                        // 読込みファイル種類
                        ModelType = (ModelType)(int.Parse(item.AttributeValue("FileNo"))),
                        // ファイル名
                        FileName = item.AttributeValue("FileName"),
                        // 設定ファイル名
                        SettingFileName = item.AttributeValue("SettingFileName"),
                        // ファイル1行あたりの文字数
                        LengthPerLine = int.Parse(item.AttributeValue("LengthPerLine"))
                    });
                });

                return readFileProperties;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 書込みフォルダの取得
        /// </summary>
        /// <returns>書込みフォルダ</returns>
        internal string GetWriteFolderPath()
        {
            string path = string.Empty;

            try
            {
                var elem = xmlOperator.GetElement("/Settings/Output");
                path = xmlOperator.GetAttlibuteValue(elem, "Folder");
                return path;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// バックアップフォルダの取得
        /// </summary>
        /// <returns>バックアップフォルダ</returns>
        internal string GetBackupFolderPath()
        {
            string path = string.Empty;

            try
            {
                var elem = xmlOperator.GetElement("/Settings/Backup");
                path = xmlOperator.GetAttlibuteValue(elem, "Folder");
                return path;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 読込みフォルダの保存
        /// </summary>
        /// <param name="folderPath">フォルダのパス</param>
        internal void SaveReadFolderPath(string folderPath)
        {
            string path = string.Empty;
            string operationType = string.Empty;

            try
            {
                xmlOperator.SetAttlibuteValueWithUpdate("/Settings/Input", "Folder", folderPath);
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 書込みフォルダの保存
        /// </summary>
        /// <param name="folderPath">フォルダのパス</param>
        internal void SaveWriteFolderPath(string folderPath)
        {
            string path = string.Empty;

            try
            {
                xmlOperator.SetAttlibuteValueWithUpdate("/Settings/Output", "Folder", folderPath);
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Excelテンプレートブックのパスの取得
        /// </summary>
        /// <returns>Excelテンプレートブックのパス</returns>
        internal string GetExcelTemplateBookPath()
        {
            string folder = string.Empty;
            string fileName = string.Empty;

            try
            {
                var elem = xmlOperator.GetElement("/Settings/Excel/Template");
                folder = xmlOperator.GetAttlibuteValue(elem, "Folder");
                fileName = xmlOperator.GetAttlibuteValue(elem, "Name");

                return folder + fileName;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Excel情報の一覧の取得
        /// </summary>
        /// <returns>Excel情報の一覧</returns>
        internal IList<ExcelSetting> GetExcelProperties()
        {
            var performanceExcelProperties = new List<ExcelSetting>();

            try
            {
                var elements = xmlOperator.GetElements("/Settings/Excels/Excel");
                elements.ForEach(item =>
                {
                    performanceExcelProperties.Add(new ExcelSetting()
                    {
                        // 読込みファイル種類
                        ModelType = (ModelType)(int.Parse(item.AttributeValue("FileNo"))),
                        // テンプレートブックのパス
                        TemplateBookPath = item.AttributeValue("TemplateBookFolder") + item.AttributeValue("TemplateBookName"),
                        // 出力ブック名
                        OutputBookName = item.AttributeValue("OutputBookName"),
                        // 出力ブックフォルダ
                        OutputBookFolder = item.AttributeValue("OutputBookFolder"),
                        // バックアップフォルダ
                        BackupFolder = item.AttributeValue("BackupFolder")
                    });
                });

                return performanceExcelProperties;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Ping情報の取得
        /// </summary>
        /// <returns>Ping情報</returns>
        internal PingSetting GetPingProperty()
        {
            try
            {
                var elem = xmlOperator.GetElement("/Settings/Ping/Output");
                var setting = new PingSetting()
                {
                    IPAddress = xmlOperator.GetAttlibuteValue(elem, "IPAddress"),
                    TimeoutPeriod = int.Parse(xmlOperator.GetAttlibuteValue(elem, "TimeoutPeriod")),
                    RetryCount = int.Parse(xmlOperator.GetAttlibuteValue(elem, "RetryCount"))
                };
                return setting;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// カスタムログのフォルダパスの取得
        /// </summary>
        /// <returns>カスタムログのフォルダパス</returns>
        internal string GetCustomLogFolderPath()
        {
            string ret = string.Empty;

            try
            {
                var elem = xmlOperator.GetElement("/Settings/CustomLog/Folder");
                return ret = xmlOperator.GetAttlibuteValue(elem, "Folder");
            }

            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// カスタムログのファイル名の取得
        /// </summary>
        /// <returns>カスタムログのファイル名</returns>
        internal string GetCustomLogFileName()
        {
            string ret = string.Empty;

            try
            {
                var elem = xmlOperator.GetElement("/Settings/CustomLog/File");
                return ret = xmlOperator.GetAttlibuteValue(elem, "Name");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ログ出力用フォルダを作成する
        /// </summary>
        internal void CreateOutputLogFolder()
        {
            string folder = string.Empty;

            try
            {
                var elem = xmlOperator.GetElement("/Settings/OutputLog/Folder");
                folder = xmlOperator.GetAttlibuteValue(elem, "Folder");

                if (!Directory.Exists(folder)) 
                    Directory.CreateDirectory(folder);
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ログ出力用フォルダの存在チェック
        /// </summary>
        /// <returns>結果</returns>
        internal bool ExistOutputLogFolder()
        {
            bool ret = false;
            string folder = string.Empty;

            try
            {
                var elem = xmlOperator.GetElement("/Settings/OutputLog/Folder");
                folder = xmlOperator.GetAttlibuteValue(elem, "Folder");

                if (Directory.Exists(folder)) ret = true;
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
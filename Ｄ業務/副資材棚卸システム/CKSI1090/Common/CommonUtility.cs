using System;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.XPath;
//*************************************************************************************
//
//   ユーティリティークラス
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   17.04.28              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1090.Common
{
    /// <summary>
    /// ユーティリティークラス
    /// </summary>
    internal class CommonUtility
    {
        #region メソッド

        /// <summary>
        /// ログ出力用のフォルダを作成する
        /// </summary>
        internal static void CreateOutputLogFolder()
        {
            string path = string.Empty;

            try
            {
                // XMLファイルのパスの取得
                string xmlPath = Path.Combine(Path.GetDirectoryName(
                Assembly.GetExecutingAssembly().Location) ?? string.Empty, "WorkNoteCheckListSettings.xml");

                // XMLファイルを読込む
                var doc = XDocument.Load(xmlPath);
                var elem = doc.XPathSelectElement($"/Settings/ExceptionSQL/File");
                path = (string)elem.Attribute("Path");

                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ログ出力用のフォルダの存在チェック
        /// </summary>
        /// <returns>結果</returns>
        internal static bool ExistOutputLogFolder()
        {
            bool ret = false;
            string path = string.Empty;

            try
            {
                // XMLファイルのパスの取得
                string xmlPath = Path.Combine(Path.GetDirectoryName(
                Assembly.GetExecutingAssembly().Location) ?? string.Empty, "WorkNoteCheckListSettings.xml");

                // XMLファイルを読込む
                var doc = XDocument.Load(xmlPath);
                var elem = doc.XPathSelectElement($"/Settings/ExceptionSQL/File");
                path = (string)elem.Attribute("Path");

                if (Directory.Exists(path)) ret = true;
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
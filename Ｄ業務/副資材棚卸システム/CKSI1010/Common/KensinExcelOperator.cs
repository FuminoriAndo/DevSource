using System;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.XPath;
using static CKSI1010.Common.Constants;
//*************************************************************************************
//
//   検針データ(EXCEl)操作用クラス
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1010.Common
{
    /// <summary>
    /// 検針データ(EXCEl)操作用クラス
    /// </summary>
    internal class KensinExcelOperator
    {
        #region メソッド

        /// <summary>
        /// 検針データ(EXCEL)のファイルパスを設定ファイル(XMLファイル)に書込む
        /// </summary>
        /// <param name="dataType">検針データ(EXCEL)のデータの種類</param> 
        /// <param name="filePath">ファイルパス</param>
        internal static void WriteExcelFilePath(KensinDataType dataType, string filePath)
        {
            try
            {
                // XMLファイルのパスの取得
                string xmlPath = Path.Combine(Path.GetDirectoryName(
                    Assembly.GetExecutingAssembly().Location) ?? string.Empty, "Settings.xml");

                // XMLファイルを読込む
                var doc = XDocument.Load(xmlPath);

                // 検針データ(EXCEL)のファイルパスを書込む
                var elem = doc.XPathSelectElement($"/Settings/Kensin/File");
                elem.Attribute("Path").SetValue(filePath);

                // 上書き保存
                doc.Save(xmlPath);
            }

            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// 検針データ(EXCEL)のファイルパスを設定ファイル(XMLファイル)から読込む
        /// </summary>
        /// <param name="dataType">検針データ(EXCEL)のデータの種類</param> 
        internal static string ReadExcelFilePath(KensinDataType dataType)
        {
            string ret = null;

            try
            {
                // XMLファイルのパスの取得
                string xmlPath = Path.Combine(Path.GetDirectoryName(
                Assembly.GetExecutingAssembly().Location) ?? string.Empty, "Settings.xml");

                // XMLファイルを読込む
                var doc = XDocument.Load(xmlPath);

                // 検針データ(EXCEL)のファイルパスを読込む
                var elem = doc.XPathSelectElement($"/Settings/Kensin/File");
                ret = (string)elem.Attribute("Path");
            }

            catch (Exception)
            {
                throw;
            }

            return ret;

        }

        #endregion
    }
}
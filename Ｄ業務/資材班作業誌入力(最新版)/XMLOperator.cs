using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.XPath;
//*************************************************************************************
//
//   XML操作クラス
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   17.04.28              DSK   　     新規作成
//
//*************************************************************************************
namespace Common
{
    /// <summary>
    /// XML操作
    /// </summary>
    internal class XMLOperator
    {
        #region フィールド
        /// <summary>
        /// XML操作オブジェクト(Singleton)
        /// </summary>
        private static XMLOperator singleton = null;

        /// <summary>
        /// XDocumentオブジェクト
        /// </summary>
        private static XDocument xDocument = null;
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        private XMLOperator()
        {
            string xmlPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "CKSI0010Settings.xml");
            xDocument = XDocument.Load(xmlPath);
        }
        #endregion

        #region メソッド
        /// <summary>
        /// インスタンスの取得
        /// </summary>
        /// <returns></returns>
        internal static XMLOperator GetInstance()
        {
            singleton = new XMLOperator();
            return singleton;
        }

        /// <summary>
        /// 検針データ(EXCEL)出力用のシート名の取得
        /// </summary>
        internal string GetSizaiInOutSheetName()
        {
            string ret = string.Empty;

            var element = xDocument.XPathSelectElement("/Settings/Kensin/Excel");

            ret = element.Attribute("SheetName").Value;

            return ret;
        }

        #endregion
    }
}
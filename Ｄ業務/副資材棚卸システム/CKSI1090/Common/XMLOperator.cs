using CKSI1090.Core;
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
            string xmlPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "WorkNoteCheckListSettings.xml");
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
        /// 作業誌区分の取得
        /// </summary>
        /// <returns>作業誌区分</returns>
        internal IDictionary<string, string> GetWorkNoteKbn()
        {
            IDictionary<string, string> dicitionnary = new Dictionary<string, string>();

            IEnumerable<XElement> elemSelect = from item in xDocument.XPathSelectElements("//Settings/WorkNoteType/WorkNote") select item;

            foreach (XElement elem in elemSelect)
            {
                dicitionnary.Add(elem.AttributeValue("Key"), elem.AttributeValue("Value"));
            }

            return dicitionnary;
        }

        /// <summary>
        /// 向先の取得
        /// </summary>
        /// <returns>向先</returns>
        internal IDictionary<string, string> GetMukesakiType()
        {
            IDictionary<string, string> dicitionnary = new Dictionary<string, string>();

            IEnumerable<XElement> elemSelect = from item in xDocument.XPathSelectElements("//Settings/MukesakiType/Mukesaki") select item;

            foreach (XElement elem in elemSelect)
            {
                dicitionnary.Add(elem.AttributeValue("Key"), elem.AttributeValue("Value"));
            }

            return dicitionnary;
        }

        #endregion
    }
}
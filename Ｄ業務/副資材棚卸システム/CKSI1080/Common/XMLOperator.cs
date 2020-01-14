using Core;
using Shared;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.XPath;
using CKSI1080.Types;
//*************************************************************************************
//
//   XML操作クラス
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK       　 新規作成
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
        private static XMLOperator _singleton = null;

        /// <summary>
        /// XDocumentオブジェクト
        /// </summary>
        private static XDocument _xDocument = null;
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        private XMLOperator()
        {
            string xmlPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "LogSettings.xml");
            _xDocument = XDocument.Load(xmlPath);
        }
        #endregion

        #region メソッド
        /// <summary>
        /// インスタンスの取得
        /// </summary>
        /// <returns></returns>
        internal static XMLOperator GetInstance()
        {
            _singleton = new XMLOperator();
            return _singleton;
        }

        /// <summary>
        /// 作業区分の取得
        /// </summary>
        /// <returns>作業区分</returns>
        internal IDictionary<string, string> GetWorkCategory()
        {
            IDictionary<string, string> dicitionnary = new Dictionary<string, string>();

            IEnumerable<XElement> elemSelect = from item in _xDocument.XPathSelectElements("//Settings/WorkCategorys/WorkCategory") select item;

            foreach (XElement elem in elemSelect)
            {
                dicitionnary.Add(elem.AttributeValue("No"), elem.AttributeValue("Category"));
            }

            return dicitionnary;
        }

        /// <summary>
        /// 操作種別の取得
        /// </summary>
        /// <returns>操作種別</returns>
        internal IDictionary<string, string> GetOperationType(InventoryCategory category)
        {
            IDictionary<string, string> dicitionnary = new Dictionary<string,string>();

            var inventoryCategory = category == InventoryCategory.SIZAI ? "SIZAI" : "BS";

            IEnumerable<XElement> elemSelect = from item in _xDocument.XPathSelectElements("//OperationTypes/" + inventoryCategory + "/OperationType") select item;

            foreach (XElement elem in elemSelect)
            {
                dicitionnary.Add(elem.AttributeValue("No"), elem.AttributeValue("Type"));
            }

            return dicitionnary;
        }

        /// <summary>
        /// 操作内容の取得
        /// </summary>
        /// <returns>操作内容</returns>
        internal IDictionary<string, string> GetOperationContent(InventoryCategory category)
        {
            IDictionary<string, string> dicitionnary = new Dictionary<string, string>();

            var inventoryCategory = category == InventoryCategory.SIZAI ? "SIZAI" : "BS";

            IEnumerable<XElement> elemSelect = from item in _xDocument.XPathSelectElements("//OperationContents/" + inventoryCategory + "/OperationContent") select item;

            foreach (XElement elem in elemSelect)
            {
                dicitionnary.Add(elem.AttributeValue("No"), elem.AttributeValue("Content"));
            }

            return dicitionnary;
        }

        #endregion
    }
}
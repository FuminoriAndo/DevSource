using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.XPath;
using CKSI1020.ViewModel;
using Core;
//*************************************************************************************
//
//   XML操作クラス
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   17.02.01              DSK       　 新規作成
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
            string xmlPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "MasterMaintenanceSettings.xml");
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
        /// メニューボタン設定
        /// </summary>
        /// <returns>メニューボタン情報</returns>
        internal MenuButtonSettingInfo GetMenuButtonSettingInfo()
        {
            MenuButtonSettingInfo menuButtonSettingInfo = new MenuButtonSettingInfo();
            IEnumerable<XElement> elemSelect = from item in xDocument.XPathSelectElements("//Settings/MenuButton/Setting") select item;

            foreach (XElement elem in elemSelect)
            {
                menuButtonSettingInfo.Width = elem.AttributeValue("Width");
                menuButtonSettingInfo.Height = elem.AttributeValue("Height");
                menuButtonSettingInfo.Margin = elem.AttributeValue("Margin");
            }
            return menuButtonSettingInfo;
        }

        /// <summary>
        /// メニュー設定情報取得
        /// </summary>
        /// <returns>メニュー情報</returns>
        internal IList<MenuSettingInfo> GetMenuSettingInfo()
        {
            IList<MenuSettingInfo> menuList = new List<MenuSettingInfo>();
            IEnumerable<XElement> elemSelect = from item in xDocument.XPathSelectElements("//Settings/MenuList/Menu") select item;

            foreach (XElement elem in elemSelect)
            {
                MenuSettingInfo menuSettingInfo = new MenuSettingInfo();
                menuSettingInfo.MenuName = elem.AttributeValue("Content");
                menuSettingInfo.Execution = elem.AttributeValue("Execution");
                menuSettingInfo.ButtonName = elem.AttributeValue("ButtonName");
                menuList.Add(menuSettingInfo);
            }
            return menuList;
        }

        #endregion
    }
}
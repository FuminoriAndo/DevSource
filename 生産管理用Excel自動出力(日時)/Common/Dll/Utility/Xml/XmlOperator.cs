using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
//*************************************************************************************
//
//   XML操作クラス
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************
namespace Utility.Xml
{
    /// <summary>
    /// XML操作
    /// </summary>
    public class XmlOperator
    {
        #region フィールド

        /// <summary>
        /// XDocumentオブジェクト
        /// </summary>
        private XDocument xDocument = null;

        /// <summary>
        /// XMLファイルのパス
        /// </summary>
        private readonly string xmlFilePath = null;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        public XmlOperator(string filePath)
        {
            try
            {
                // XMLファイルを指定
                this.xmlFilePath = filePath;

                // XMLファイルを読込む
                this.xDocument = XDocument.Load(xmlFilePath);
            }

            catch (Exception)
            {
                
                throw;
            }
        }

        #endregion

        #region メソッド

        /// <summary>
        /// エレメントの取得
        /// </summary>
        /// <param name="xmlPath">XMLパス</param>
        /// <returns>エレメント</returns>
        public XElement GetElement(string xmlPath)
        {
            try
            {
                return this.xDocument.XPathSelectElement(xmlPath);
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// エレメントのコレクションの取得
        /// </summary>
        /// <param name="xmlPath">XMLパス</param>
        /// <returns>エレメント</returns>
        public IEnumerable<XElement> GetElements(string xmlPath)
        {
            try
            {
                IEnumerable<XElement> elemSelect
                    = from item in this.xDocument.XPathSelectElements(xmlPath) select item;
                return elemSelect;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 属性値の取得
        /// </summary>
        /// <param name="xmlPath">XMLパス</param>
        /// <param name="attributeName">属性名</param>
        /// <returns>属性値</returns>
        public string GetAttlibuteValue(string xmlPath, string attributeName)
        {
            try
            {
                var element = this.xDocument.XPathSelectElement(xmlPath);
                return (string)element.Attribute(attributeName);
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 属性値の取得
        /// </summary>
        /// <param name="element">エレメント</param>
        /// <param name="attributeName">属性名</param>
        /// <returns>属性値</returns>
        public string GetAttlibuteValue(XElement element, string attributeName)
        {
            try
            {
                return (string)element.Attribute(attributeName);
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 属性値の設定
        /// </summary>
        /// <param name="xmlPath">XMLパス</param>
        /// <param name="attributeName">属性名</param>
        /// <param name="attributeValue">設定する属性値</param>
        public void SetAttlibuteValue(string xmlPath, string attributeName, string attributeValue)
        {
            try
            {
                var element = this.xDocument.XPathSelectElement(xmlPath);
                element.Attribute(attributeName).SetValue(attributeValue);
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 属性値の設定
        /// </summary>
        /// <param name="element">エレメント</param>
        /// <param name="attributeName">属性名</param>
        /// <param name="attributeValue">設定する属性値</param>
        public void SetAttlibuteValue(XElement element, string attributeName, string attributeValue)
        {
            try
            {
                element.Attribute(attributeName).SetValue(attributeValue);
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 属性値の設定および設定ファイルの上書き保存を行う
        /// </summary>
        /// <param name="xmlPath">XMLパス</param>
        /// <param name="attributeName">属性名</param>
        /// <param name="attributeValue">設定する属性値</param>
        public void SetAttlibuteValueWithUpdate(string xmlPath, string attributeName, string attributeValue)
        {
            try
            {
                SetAttlibuteValue(xmlPath, attributeName, attributeValue);
                this.xDocument.Save(this.xmlFilePath);
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 属性値の設定および設定ファイルの上書き保存を行う
        /// </summary>
        /// <param name="element">エレメント</param>
        /// <param name="attributeName">属性名</param>
        /// <param name="attributeValue">設定する属性値</param>
        public void SetAttlibuteValueWithUpdate(XElement element, string attributeName, string attributeValue)
        {
            try
            {
                SetAttlibuteValue(element, attributeName, attributeValue);
                this.xDocument.Save(this.xmlFilePath);
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
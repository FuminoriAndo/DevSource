//*************************************************************************************
//
//   MessageBox操作クラス
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************
using System.IO;
using System.Reflection;
using System.Windows;
using System.Xml.Linq;
using System.Xml.XPath;

namespace ProductionControlDailyExcelCreator.Common
{
    /// <summary>
    /// MessageBox操作
    /// </summary>
    internal class MessageOperator
    {
        #region フィールド

        /// <summary>
        /// メッセージオブジェクト(Singleton)
        /// </summary>
        private static MessageOperator singleton = null;

        /// <summary>
        /// XDocumentオブジェクト
        /// </summary>
        private static XDocument xDocument = null;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        private MessageOperator()
        {
            string xmlPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Message.xml");
            xDocument = XDocument.Load(xmlPath);
        }

        #endregion

        #region メソッド

        /// <summary>
        /// Messageオブジェクトのインスタンスの取得
        /// </summary>
        /// <returns>Messageオブジェクトのインスタンス</returns>
        internal static MessageOperator GetInstance()
        {
            singleton = new MessageOperator();
            return singleton;
        }

        /// <summary>
        /// メッセージの取得
        /// </summary>
        /// <param name="systemID">システムID</param>
        /// <param name="messageName">メッセージ名</param>
        /// <returns>メッセージ</returns>
        internal string GetMessage(MessageManager.SystemID systemID, string messageName)
        {
            string element = null;
            string message = null;

            switch (systemID)
            {
                // 生産管理用Excel自動出力(日次)
                case MessageManager.SystemID.ProductionControlDailyExcelCreator:
                    element = "//Message/ProductionControlDailyExcelCreator/Setting[@Name='" + messageName + "']";
                    break;
                default:
                    break;
               
            }

            message = xDocument.XPathSelectElement(element).Attribute("value").Value;
            return message;
        }

        /// <summary>
        /// タイトルの取得
        /// </summary>
        /// <param name="systemID">システムID</param>
        /// <returns>タイトル</returns>
        internal string GetTitle(MessageManager.SystemID systemID)
        {
            string element = string.Empty;
            string title = string.Empty;

            switch (systemID)
            {
                // 生産管理用Excel自動出力(日次)
                case MessageManager.SystemID.ProductionControlDailyExcelCreator:
                    element = "//Message/ProductionControlDailyExcelCreator/Setting[@Name='ApplicationTitle']";
                    break;            
                default:
                    break;
            }

            title = xDocument.XPathSelectElement(element).Attribute("value").Value;
            return title;
        }

        /// <summary>
        /// MessageBoxの出力
        /// </summary>
        /// <param name="msg">出力するメッセージ</param>
        /// <param name="caption">タイトル</param>
        /// <param name="btnType">ボタン形式</param>
        /// <param name="imageType">イメージ形式</param>
        /// <returns>出力結果</returns>
        internal MessageManager.ResultType ShowMessage(string msg, string caption,
                                                       MessageManager.ButtonType btnType, 
                                                       MessageManager.ImageType imageType)
        {
            MessageBoxButton button = getMessageBoxButton(btnType);
            MessageBoxImage image = getMessageBoxImage(imageType);
            MessageBoxResult result = MessageBox.Show(msg, caption, button, image);
            return getResultType(result);
        }

        /// <summary>
        /// MessageBoxの出力
        /// </summary>
        /// <param name="msg">出力するメッセージ</param>
        /// <param name="caption">タイトル</param>
        /// <param name="btnType">ボタン形式</param>
        /// <param name="imageType">イメージ形式</param>
        /// <param name="resultType">デフォルトのボタンフォーカス</param>
        /// <returns>出力結果</returns>
        internal MessageManager.ResultType ShowMessage(string msg, string caption,
                                                       MessageManager.ButtonType btnType, 
                                                       MessageManager.ImageType imageType,
                                                       MessageManager.ResultType resultType)
        {
            MessageBoxButton button = getMessageBoxButton(btnType);
            MessageBoxImage image = getMessageBoxImage(imageType);
            MessageBoxResult defaultResult = getMessageBoxResult(resultType);
            MessageBoxResult result = MessageBox.Show(msg, caption, button, image, defaultResult);
            return getResultType(result);
        }

        /// <summary>
        /// MessageBoxButtonオブジェクトの取得
        /// </summary>
        /// <param name="buttonType">MessageBoxのボタン種類</param>
        /// <returns>MessageBoxButtonオブジェクト</returns>
        private MessageBoxButton getMessageBoxButton(MessageManager.ButtonType buttonType)
        {
            MessageBoxButton button = MessageBoxButton.OK;

            switch (buttonType)
            {
                case MessageManager.ButtonType.OK:
                    button = MessageBoxButton.OK;
                    break;
                case MessageManager.ButtonType.OKCancel:
                    button = MessageBoxButton.OKCancel;
                    break;
                case MessageManager.ButtonType.YesNo:
                    button = MessageBoxButton.YesNo;
                    break;
                case MessageManager.ButtonType.YesNoCancel:
                    button = MessageBoxButton.YesNoCancel;
                    break;
                default:
                    break;
            }

            return button;
        }

        /// <summary>
        /// MessageBoxImageオブジェクトの取得
        /// </summary>
        /// <param name="imageType">MessageBoxのイメージ種類</param>
        /// <returns>essageBoxImageオブジェクト</returns>
        private MessageBoxImage getMessageBoxImage(MessageManager.ImageType imageType)
        {
            MessageBoxImage image = MessageBoxImage.Information;

            switch (imageType)
            {
                case MessageManager.ImageType.Asterisk:
                    image = MessageBoxImage.Asterisk;
                    break;
                case MessageManager.ImageType.Error:
                    image = MessageBoxImage.Error;
                    break;
                case MessageManager.ImageType.Exclamation:
                    image = MessageBoxImage.Exclamation;
                    break;
                case MessageManager.ImageType.Hand:
                    image = MessageBoxImage.Hand;
                    break;
                case MessageManager.ImageType.Information:
                    image = MessageBoxImage.Information;
                    break;
                case MessageManager.ImageType.None:
                    image = MessageBoxImage.None;
                    break;
                case MessageManager.ImageType.Question:
                    image = MessageBoxImage.Question;
                    break;
                case MessageManager.ImageType.Stop:
                    image = MessageBoxImage.Stop;
                    break;
                case MessageManager.ImageType.Warning:
                    image = MessageBoxImage.Warning;
                    break;
                default:
                    break;
            }
            return image;
        }

        /// <summary>
        /// MessageBoxResultオブジェクトの取得
        /// </summary>
        /// <param name="resultType">MessageBoxの結果種類</param>
        /// <returns>MessageBoxResultオブジェクト</returns>
        private MessageBoxResult getMessageBoxResult(MessageManager.ResultType resultType)
        {
            MessageBoxResult result = MessageBoxResult.OK;

            switch (resultType)
            {
                case MessageManager.ResultType.Cancel:
                    result = MessageBoxResult.Cancel;
                    break;
                case MessageManager.ResultType.No:
                    result = MessageBoxResult.No;
                    break;
                case MessageManager.ResultType.None:
                    result = MessageBoxResult.None;
                    break;
                case MessageManager.ResultType.OK:
                    result = MessageBoxResult.OK;
                    break;
                case MessageManager.ResultType.Yes:
                    result = MessageBoxResult.Yes;
                    break;
                default:
                    break;
            }

            return result;
        }

        /// <summary>
        /// MessageBoxの結果種類の取得
        /// </summary>
        /// <param name="result">MessageBoxResultオブジェクト</param>
        /// <returns>MessageBoxの結果種類</returns>
        private MessageManager.ResultType getResultType(MessageBoxResult result)
        {
            MessageManager.ResultType resultType = MessageManager.ResultType.OK;

            switch (result)
            {
                case MessageBoxResult.Cancel:
                    resultType = MessageManager.ResultType.Cancel;
                    break;
                case MessageBoxResult.No:
                    resultType = MessageManager.ResultType.No;
                    break;
                case MessageBoxResult.None:
                    resultType = MessageManager.ResultType.None;
                    break;
                case MessageBoxResult.OK:
                    resultType = MessageManager.ResultType.OK;
                    break;
                case MessageBoxResult.Yes:
                    resultType = MessageManager.ResultType.Yes;
                    break;
                default:
                    break;
            }

            return resultType;
        }

        #endregion
    }
}
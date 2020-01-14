using System.IO;
using System.Reflection;
using System.Windows;
using System.Xml.Linq;
using System.Xml.XPath;
//*************************************************************************************
//
//   MessageBox操作クラス
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
    /// MessageBox操作
    /// </summary>
    internal class MessageOperator
    {
        #region フィールド
        /// <summary>
        /// メッセージオブジェクト(Singleton)
        /// </summary>
        private static MessageOperator singletonMessage = null;

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
            singletonMessage = new MessageOperator();
            return singletonMessage;
        }

        /// <summary>
        /// メッセージの取得
        /// </summary>
        /// <param name="systemID">システムID</param>
        /// <param name="messageName">メッセージ名</param>
        /// <returns>メッセージ</returns>
        internal string GetMessage(Common.MessageManager.SystemID systemID, string messageName)
        {
            string element = null;
            string message = null;

            switch (systemID)
            {
                // CKSI1010
                case Common.MessageManager.SystemID.CKSI1010:
                    element = "//Message/CKSI1010/Setting[@Name='" + messageName + "']";
                    break;
                // CKSI1020
                case Common.MessageManager.SystemID.CKSI1020:
                    element = "//Message/CKSI1020/Setting[@Name='" + messageName + "']";
                    break;
                // CKSI1030
                case Common.MessageManager.SystemID.CKSI1030:
                    element = "//Message/CKSI1030/Setting[@Name='" + messageName + "']";
                    break;
                // CKSI1040
                case Common.MessageManager.SystemID.CKSI1040:
                    element = "//Message/CKSI1040/Setting[@Name='" + messageName + "']";
                    break;
                // CKSI1050
                case Common.MessageManager.SystemID.CKSI1050:
                    element = "//Message/CKSI1050/Setting[@Name='" + messageName + "']";
                    break;
                // CKSI1060
                case Common.MessageManager.SystemID.CKSI1060:
                    element = "//Message/CKSI1060/Setting[@Name='" + messageName + "']";
                    break;
                // CKSI1070
                case Common.MessageManager.SystemID.CKSI1070:
                    element = "//Message/CKSI1070/Setting[@Name='" + messageName + "']";
                    break;
                // CKSI1080
                case Common.MessageManager.SystemID.CKSI1080:
                    element = "//Message/CKSI1080/Setting[@Name='" + messageName + "']";
                    break;
                // CKSI1090
                case Common.MessageManager.SystemID.CKSI1090:
                    element = "//Message/CKSI1090/Setting[@Name='" + messageName + "']";
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
        /// <param name="messageName">メッセージ名</param> 
        /// <returns>タイトル</returns>
        internal string GetTitle(Common.MessageManager.SystemID systemID)
        {
            string element = string.Empty;
            string title = string.Empty;

            switch (systemID)
            {
                // CKSI1010
                case Common.MessageManager.SystemID.CKSI1010:
                    element = "//Message/CKSI1010/Setting[@Name='ApplicationTitle']";
                    break;
                // CKSI1020
                case Common.MessageManager.SystemID.CKSI1020:
                   element = "//Message/CKSI1020/Setting[@Name='ApplicationTitle']";
                    break;
                 // CKSI1030
                case Common.MessageManager.SystemID.CKSI1030:
                    element = "//Message/CKSI1030/Setting[@Name='ApplicationTitle']";
                    break;                   
                // CKSI1040
                case Common.MessageManager.SystemID.CKSI1040:
                    element = "//Message/CKSI1040/Setting[@Name='ApplicationTitle']";
                    break;
                // CKSI1050
                case Common.MessageManager.SystemID.CKSI1050:
                    element = "//Message/CKSI1050/Setting[@Name='ApplicationTitle']";
                    break;
                // CKSI1060
                case Common.MessageManager.SystemID.CKSI1060:
                    element = "//Message/CKSI1060/Setting[@Name='ApplicationTitle']";
                    break;
                // CKSI1070
                case Common.MessageManager.SystemID.CKSI1070:
                    element = "//Message/CKSI1070/Setting[@Name='ApplicationTitle']";
                    break;
                // CKSI1080
                case Common.MessageManager.SystemID.CKSI1080:
                    element = "//Message/CKSI1080/Setting[@Name='ApplicationTitle']";
                    break;
                // CKSI1090
                case Common.MessageManager.SystemID.CKSI1090:
                    element = "//Message/CKSI1090/Setting[@Name='ApplicationTitle']";
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
        internal Common.MessageManager.ResultType ShowMessage(string msg, string caption,
                                             Common.MessageManager.ButtonType btnType, Common.MessageManager.ImageType imageType)
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
        /// <param name="defaultResult">デフォルトのボタンフォーカス</param>
        /// <returns>出力結果</returns>
        internal Common.MessageManager.ResultType ShowMessage(string msg, string caption,
                                             Common.MessageManager.ButtonType btnType, Common.MessageManager.ImageType imageType,
                                             Common.MessageManager.ResultType resultType)
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
        private MessageBoxButton getMessageBoxButton(Common.MessageManager.ButtonType buttonType)
        {
            MessageBoxButton button = MessageBoxButton.OK;

            switch (buttonType)
            {
                case Common.MessageManager.ButtonType.OK:
                    button = MessageBoxButton.OK;
                    break;
                case Common.MessageManager.ButtonType.OKCancel:
                    button = MessageBoxButton.OKCancel;
                    break;
                case Common.MessageManager.ButtonType.YesNo:
                    button = MessageBoxButton.YesNo;
                    break;
                case Common.MessageManager.ButtonType.YesNoCancel:
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
        private MessageBoxImage getMessageBoxImage(Common.MessageManager.ImageType imageType)
        {
            MessageBoxImage image = MessageBoxImage.Information;

            switch (imageType)
            {
                case Common.MessageManager.ImageType.Asterisk:
                    image = MessageBoxImage.Asterisk;
                    break;
                case Common.MessageManager.ImageType.Error:
                    image = MessageBoxImage.Error;
                    break;
                case Common.MessageManager.ImageType.Exclamation:
                    image = MessageBoxImage.Exclamation;
                    break;
                case Common.MessageManager.ImageType.Hand:
                    image = MessageBoxImage.Hand;
                    break;
                case Common.MessageManager.ImageType.Information:
                    image = MessageBoxImage.Information;
                    break;
                case Common.MessageManager.ImageType.None:
                    image = MessageBoxImage.None;
                    break;
                case Common.MessageManager.ImageType.Question:
                    image = MessageBoxImage.Question;
                    break;
                case Common.MessageManager.ImageType.Stop:
                    image = MessageBoxImage.Stop;
                    break;
                case Common.MessageManager.ImageType.Warning:
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
        private MessageBoxResult getMessageBoxResult(Common.MessageManager.ResultType resultType)
        {
            MessageBoxResult result = MessageBoxResult.OK;

            switch (resultType)
            {
                case Common.MessageManager.ResultType.Cancel:
                    result = MessageBoxResult.Cancel;
                    break;
                case Common.MessageManager.ResultType.No:
                    result = MessageBoxResult.No;
                    break;
                case Common.MessageManager.ResultType.None:
                    result = MessageBoxResult.None;
                    break;
                case Common.MessageManager.ResultType.OK:
                    result = MessageBoxResult.OK;
                    break;
                case Common.MessageManager.ResultType.Yes:
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
        private Common.MessageManager.ResultType getResultType(MessageBoxResult result)
        {
            Common.MessageManager.ResultType resultType = Common.MessageManager.ResultType.OK;

            switch (result)
            {
                case MessageBoxResult.Cancel:
                    resultType = Common.MessageManager.ResultType.Cancel;
                    break;
                case MessageBoxResult.No:
                    resultType = Common.MessageManager.ResultType.No;
                    break;
                case MessageBoxResult.None:
                    resultType = Common.MessageManager.ResultType.None;
                    break;
                case MessageBoxResult.OK:
                    resultType = Common.MessageManager.ResultType.OK;
                    break;
                case MessageBoxResult.Yes:
                    resultType = Common.MessageManager.ResultType.Yes;
                    break;
                default:
                    break;
            }

            return resultType;
        }
        #endregion
    }
}
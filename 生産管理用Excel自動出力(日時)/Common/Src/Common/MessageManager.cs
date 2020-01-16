//*************************************************************************************
//
//   MessageBox出力制御クラス
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************
using System;

namespace ProductionControlDailyExcelCreator.Common
{
    /// <summary>
    /// MessageBox出力制御
    /// </summary>
    public class MessageManager
    {
        #region フィールド

        /// <summary>
        /// メッセージオブジェクト
        /// </summary>
        private static MessageOperator messageOperator = MessageOperator.GetInstance();

        #endregion

        #region 列挙型

        /// <summary>
        /// MessageBoxのボタン種類
        /// </summary>
        internal enum ButtonType
        {
            OK,
            OKCancel,
            YesNo,
            YesNoCancel
        }

        /// <summary>
        /// MessageBoxのイメージ種類
        /// </summary>
        internal enum ImageType
        {
            Asterisk,
            Error,
            Exclamation,
            Hand,
            Information,
            None,
            Question,
            Stop,
            Warning
        }

        /// <summary>
        /// MessageBoxの結果種類
        /// </summary>
        internal enum ResultType
        {
            Cancel,
            No,
            None,
            OK,
            Yes
        }

        /// <summary>
        /// システムID
        /// </summary>
        public enum SystemID
        {
            /// <summary>
            /// 生産管理用Excel自動出力(日次)
            /// </summary>
            ProductionControlDailyExcelCreator
        }

        #endregion

        #region メソッド

        /// <summary>
        /// メッセージを取得する
        /// </summary>
        /// <param name="systemID">システムID</param>
        /// <param name="messageName">メッセージ名</param>
        /// <returns>メッセージ</returns>
        internal static string GetMessage(SystemID systemID, string messageName)
        {
            string message = null;
            
            try
            {
                return message = messageOperator.GetMessage(systemID, messageName);
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Informationメッセージを出力する
        /// </summary>
        /// <param name="systemID">システムID</param>
        /// <param name="messageName">メッセージ名</param>
        /// <returns>MessageBoxの結果種類</returns>
        internal static ResultType ShowInformation(SystemID systemID, string messageName)
        {
            try
            {
                string message = messageOperator.GetMessage(systemID, messageName);
                string title = messageOperator.GetTitle(systemID);
                ResultType result = messageOperator.ShowMessage(message, title, ButtonType.OK, ImageType.Information);
                return result;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Warningメッセージを出力する
        /// </summary>
        /// <param name="systemID">システムID</param>
        /// <param name="messageName">メッセージ名</param>
        /// <returns>MessageBoxの結果種類</returns>
        internal static ResultType ShowWarning(SystemID systemID, string messageName)
        {
            try
            {
                string message = messageOperator.GetMessage(systemID, messageName);
                string title = messageOperator.GetTitle(systemID);
                ResultType result = messageOperator.ShowMessage(message, title, ButtonType.OK, ImageType.Warning);
                return result;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Warningメッセージを出力する
        /// </summary>
        /// <param name="systemID">システムID</param>
        /// <param name="messageName">メッセージ名</param>
        /// <param name="messageExt">拡張用メッセージ</param>
        /// <returns>MessageBoxの結果種類</returns>
        internal static ResultType ShowWarning(SystemID systemID, string messageName, string messageExt)
        {
            try
            {
                string message = messageOperator.GetMessage(systemID, messageName);
                message = message.Replace("%s", messageExt);
                string title = messageOperator.GetTitle(systemID);
                ResultType result = messageOperator.ShowMessage(message, title, ButtonType.OK, ImageType.Warning);
                return result;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Errorメッセージを出力する
        /// </summary>
        /// <param name="systemID">システムID</param>
        /// <param name="messageName">メッセージ名</param>
        /// <returns>MessageBoxの結果種類</returns>
        internal static ResultType ShowError(SystemID systemID, string messageName)
        {
            try
            {
                string message = messageOperator.GetMessage(systemID, messageName);
                string title = messageOperator.GetTitle(systemID);
                ResultType result = messageOperator.ShowMessage(message, title, ButtonType.OK, ImageType.Error);
                return result;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Errorメッセージを出力する
        /// </summary>
        /// <param name="systemID">システムID</param>
        /// <param name="messageName">メッセージ名</param>
        /// <param name="messageExt">拡張用メッセージ</param>
        /// <returns>MessageBoxの結果種類</returns>
        internal static ResultType ShowError(SystemID systemID, string messageName, string messageExt)
        {
            try
            {
                string message = messageOperator.GetMessage(systemID, messageName);
                message = message.Replace("%s", messageExt);
                string title = messageOperator.GetTitle(systemID);
                ResultType result = messageOperator.ShowMessage(message, title, ButtonType.OK, ImageType.Error);
                return result;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Questionメッセージを出力する
        /// </summary>
        /// <param name="systemID">システムID</param>
        /// <param name="messageName">メッセージ名</param>
        /// <returns>MessageBoxの結果種類</returns>
        internal static ResultType ShowQuestion(SystemID systemID, string messageName)
        {
            try
            {
                string message = messageOperator.GetMessage(systemID, messageName);
                string title = messageOperator.GetTitle(systemID);
                ResultType result = messageOperator.ShowMessage(message, title, ButtonType.YesNo, ImageType.Question, ResultType.No);
                return result;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Exclamationメッセージを出力する
        /// </summary>
        /// <param name="systemID">システムID</param>
        /// <param name="messageName">メッセージ名</param>
        /// <returns>MessageBoxの結果種類</returns>
        internal static ResultType ShowExclamation(SystemID systemID, string messageName)
        {
            try
            {
                string message = messageOperator.GetMessage(systemID, messageName);
                string title = messageOperator.GetTitle(systemID);
                ResultType result = messageOperator.ShowMessage(message, title, ButtonType.OK, ImageType.Exclamation);
                return result;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Exclamationメッセージを出力する（拡張用）
        /// </summary>
        /// <param name="systemID">システムID</param>
        /// <param name="messageName">メッセージ名</param>
        /// <param name="messageExt">拡張用メッセージ</param>
        /// <returns>MessageBoxの結果種類</returns>
        internal static ResultType ShowExclamation(SystemID systemID, string messageName, string messageExt)
        {
            try
            {
                string message = messageOperator.GetMessage(systemID, messageName);
                message = message.Replace("%s", messageExt);
                string title = messageOperator.GetTitle(systemID);
                ResultType result = messageOperator.ShowMessage(message, title, ButtonType.OK, ImageType.Exclamation);
                return result;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Exclamationメッセージを出力する（拡張用）
        /// </summary>
        /// <param name="systemID">システムID</param>
        /// <param name="messageName">メッセージ名</param>
        /// <param name="messageExt1">拡張用メッセージ</param>
        /// <param name="messageExt2">拡張用メッセージ</param>
        /// <returns>MessageBoxの結果種類</returns>
        internal static ResultType ShowExclamation(SystemID systemID, string messageName, string messageExt1, string messageExt2)
        {
            try
            {
                string message = messageOperator.GetMessage(systemID, messageName);
                message = message.Replace("%1", messageExt1);
                message = message.Replace("%2", messageExt2);
                string title = messageOperator.GetTitle(systemID);
                ResultType result = messageOperator.ShowMessage(message, title, ButtonType.OK, ImageType.Exclamation);
                return result;
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
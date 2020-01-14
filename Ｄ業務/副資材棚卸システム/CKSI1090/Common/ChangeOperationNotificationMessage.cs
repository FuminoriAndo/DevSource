using CKSI1090.Properties;
using GalaSoft.MvvmLight.Messaging;
using System.Windows;
//*************************************************************************************
//
//   操作変更のメッセージ
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
    /// 操作変更のメッセージ
    /// </summary>
    internal class ChangeOperationNotificationMessage : NotificationMessage
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        internal ChangeOperationNotificationMessage()
            : base(Resources.ChangeOperationNotification)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="sender">送信元</param>
        internal ChangeOperationNotificationMessage(object sender)
            : base(sender, Resources.ChangeOperationNotification)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="target">送信先</param>
        internal ChangeOperationNotificationMessage(object sender, object target)
            : base(sender, target, Resources.ChangeOperationNotification)
        {
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 棚卸操作用のU/I
        /// </summary>
        internal UIElement Operation { get; set; }

        #endregion
    }
}

using CKSI1010.Properties;
using GalaSoft.MvvmLight.Messaging;
using System.Windows;
//*************************************************************************************
//
//   モーダル画面を表示するメッセージ
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//*************************************************************************************

namespace CKSI1010.Common
{
    /// <summary>
    /// モーダル画面を表示するメッセージ
    /// </summary>
    internal class ShowModalWindowMessage : NotificationMessage
    {
        #region コンストラクタ
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        internal ShowModalWindowMessage()
            : base(Resources.ChangeOperationNotification)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="sender">送信元</param>
        internal ShowModalWindowMessage(object sender)
            : base(sender, Resources.ChangeOperationNotification)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="target">送信先</param>
        internal ShowModalWindowMessage(object sender, object target)
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

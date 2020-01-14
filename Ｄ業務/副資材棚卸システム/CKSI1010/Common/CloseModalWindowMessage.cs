using CKSI1010.Properties;
using GalaSoft.MvvmLight.Messaging;
//*************************************************************************************
//
//   モーダル画面を閉じるメッセージ
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//*************************************************************************************

namespace CKSI1010.Common
{
    /// <summary>
    /// モーダル画面を閉じるメッセージ
    /// </summary>
    internal class CloseModalWindowMessage : NotificationMessage
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        internal CloseModalWindowMessage()
            : base(Resources.ChangeOperationNotification)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="sender">送信元</param>
        internal CloseModalWindowMessage(object sender)
            : base(sender, Resources.ChangeOperationNotification)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="target">送信先</param>
        internal CloseModalWindowMessage(object sender, object target)
            : base(sender, target, Resources.ChangeOperationNotification)
        {
        }

        #endregion
    }
}

using CKSI1090.Common;
using GalaSoft.MvvmLight.Messaging;
//*************************************************************************************
//
//   ベース画面
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   17.04.28              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1090.Base.View
{
    /// <summary>
    /// ベース画面
    /// </summary>
    public partial class BaseWindow
    {
        
        #region コンストラクタ

        /// <summary>
        ///コンストラクタ
        /// </summary>
        public BaseWindow()
        {
            InitializeComponent();

            MouseLeftButtonDown += (_, __) => DragMove();
            Closing += (_, __) => ViewModelLocator.Cleanup();

            this.ResizeMode = System.Windows.ResizeMode.NoResize;

            Messenger.Default.Register<ChangeOperationNotificationMessage>(this, message =>
            {
                if (message.Sender != DataContext)
                {
                    return;
                }

                OperationGrid.Children.Clear();
                OperationGrid.Children.Add(message.Operation);
            });

        }

        #endregion
    }
}
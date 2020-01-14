using CKSI1010.Base.ViewModel;
using CKSI1010.Common;
using GalaSoft.MvvmLight.Messaging;
//*************************************************************************************
//
//   ベース画面
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1010.Base.View
{
    /// <summary>
    /// ベース画面
    /// </summary>
    public partial class BaseWindow
    {
        #region コンストラクタ

        #region プロパティ
        
        /// <summary>
        /// 表示中のモーダル画面
        /// </summary>
        private System.Windows.UIElement modalWindow;
        
        #endregion

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

            Messenger.Default.Register<ShowModalWindowMessage>(this, message =>
            {
                for(int i = 0 ; i < OperationGrid.Children.Count; i++)
                {
                    OperationGrid.Children[i].Visibility = System.Windows.Visibility.Hidden;
                }
                OperationGrid.Children.Add(message.Operation);
                modalWindow = message.Operation;

                ((BaseViewModel)DataContext).IsShowModal = true;
            });

            Messenger.Default.Register<CloseModalWindowMessage>(this, message =>
            {
                ((BaseViewModel)DataContext).IsShowModal = false;

                modalWindow.Visibility = System.Windows.Visibility.Collapsed;
                OperationGrid.Children.Remove(modalWindow);
                modalWindow = null;
                for (int i = 0; i < OperationGrid.Children.Count; i++)
                {
                    OperationGrid.Children[i].Visibility = System.Windows.Visibility.Visible;
                }
            });
        }

        #endregion
    }
}
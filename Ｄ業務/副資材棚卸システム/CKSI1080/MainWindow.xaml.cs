using System;
using System.ComponentModel;
using System.Windows;
using CKSI1080.ViewModel;
using Common;
using System.Windows.Controls;

namespace CKSI1080
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            TabItem tabItemWorkLog = new TabItem();
            tabItemWorkLog.Header = "資材棚卸作業ログ";
            ucWorkLog workLog = new ucWorkLog();
            workLog.LogScreenDisplay = ucWorkLog.DisplayModeType.WorkLog;
            tabItemWorkLog.Content = workLog;
            _tabControl.Items.Add(tabItemWorkLog);

            TabItem tabItemWorkLogDetails = new TabItem();
            tabItemWorkLogDetails.Header = "資材棚卸作業ログ(詳細)";
            ucWorkLog workLogDetails = new ucWorkLog();
            workLogDetails.LogScreenDisplay = ucWorkLog.DisplayModeType.WorkLogDetails;
            tabItemWorkLogDetails.Content = workLogDetails;
            _tabControl.Items.Add(tabItemWorkLogDetails);

            TabItem tabItemWorkErrorLog = new TabItem();
            tabItemWorkErrorLog.Header = "資材棚卸作業エラーログ";
            ucWorkLog workErrorLog = new ucWorkLog();
            workErrorLog.LogScreenDisplay = ucWorkLog.DisplayModeType.WorkErrorLog;
            tabItemWorkErrorLog.Content = workErrorLog;
            _tabControl.Items.Add(tabItemWorkErrorLog);

            TabItem tabItemWorkErrorLogDetails = new TabItem();
            tabItemWorkErrorLogDetails.Header = "資材棚卸作業エラーログ（詳細）";
            ucWorkLog workErrorLogDetails = new ucWorkLog();
            workErrorLogDetails.LogScreenDisplay = ucWorkLog.DisplayModeType.WorkErrorLogDetails;
            tabItemWorkErrorLogDetails.Content = workErrorLogDetails;
            _tabControl.Items.Add(tabItemWorkErrorLogDetails);

            TabItem tabItemInventoryWorkLog = new TabItem();
            tabItemInventoryWorkLog.Header = "部品倉庫棚卸作業ログ";
            ucWorkLog inventoryWorkLog = new ucWorkLog();
            inventoryWorkLog.LogScreenDisplay = ucWorkLog.DisplayModeType.BSInventoryWorkLog;
            tabItemInventoryWorkLog.Content = inventoryWorkLog;
            _tabControl.Items.Add(tabItemInventoryWorkLog);

            TabItem tabItemInventoryWorkErrorLog = new TabItem();
            tabItemInventoryWorkErrorLog.Header = "部品倉庫棚卸作業エラーログ";
            ucWorkLog inventoryWorkErrorLog = new ucWorkLog();
            inventoryWorkErrorLog.LogScreenDisplay = ucWorkLog.DisplayModeType.BSInventoryWorkErrorLog;
            tabItemInventoryWorkErrorLog.Content = inventoryWorkErrorLog;
            _tabControl.Items.Add(tabItemInventoryWorkErrorLog);

        }
        #endregion

        #region メソッド

        /// <summary>
        /// 閉じる「×」ボタン押下時のイベント
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベントデータ</param>
        private void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            if (MessageManager.ShowQuestion(MessageManager.SystemID.CKSI1080, "ConfirmExit") == MessageManager.ResultType.No)
            {
                e.Cancel = true;
            }
        }

        #endregion
    }
}

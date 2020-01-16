using System;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using log4net;
using ProductionControlDailyExcelCreator.Common;

namespace ProductionControlDailyExcelCreator
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Mutex mutex = new Mutex(false, "生産管理用Excel自動出力(日次)");
                WrapXmlOperator xmlOperator = null;

                if (!mutex.WaitOne(0, false))
                {
                    // 既に起動しているため終了させる
                    MessageManager.ShowExclamation(MessageManager.SystemID.ProductionControlDailyExcelCreator, "AlreadyRun");
                    mutex.Close();
                    mutex = null;
                    Environment.Exit(0);
                }


                // XML操作(Utility)のラッパーのインスタンスの取得
                xmlOperator = WrapXmlOperator.GetInstance();

                // ログ出力用フォルダの存在チェック(なければフォルダを作成する)
                if (!xmlOperator.ExistOutputLogFolder())
                {
                    xmlOperator.CreateOutputLogFolder();
                }
            }

            catch (Exception ex)
            {
                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(ex.ToString());
                // システムエラー
                MessageManager.ShowError(MessageManager.SystemID.ProductionControlDailyExcelCreator,
                                        "Exception", ex.Message.ToString());
            }
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }
    }
}

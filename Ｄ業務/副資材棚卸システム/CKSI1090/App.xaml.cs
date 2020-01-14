using GalaSoft.MvvmLight.Threading;
using log4net;
using System.Reflection;
using System.Windows.Threading;
//*************************************************************************************
//
//   アプリケーション
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1090
{
    /// <summary>
    /// アプリケーション
    /// </summary>
    public partial class App
    {
        static App()
        {
            DispatcherHelper.Initialize();
        }

        /// <summary>
        /// アプリ起動時の処理
        /// </summary>
        /// <param name="sender">イベント送信元</param>
        /// <param name="e">イベントデータ</param>
        private void OnStartup(object sender, System.Windows.StartupEventArgs e)
        {
            if (e.Args != null&& e.Args.Length >= 1)
            {
                string[] args = e.Args[0].Split(',');
                Shared.SharedModel.Instance.StartupArgs.EmployeeCode = args[0];
                Shared.SharedModel.Instance.StartupArgs.PositionCode = args[1];
                Shared.SharedModel.Instance.StartupArgs.BelongingCode = args[2];
            }
        }

        /// <summary>
        /// 未処理例外の捕縛
        /// </summary>
        /// <param name="sender">イベント送信元</param>
        /// <param name="e">イベントデータ</param>
        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.Exception.ToString());
            e.Handled = true;
        }
    }
}

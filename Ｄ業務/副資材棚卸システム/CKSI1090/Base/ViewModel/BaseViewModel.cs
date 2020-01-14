using CKSI1090.Common;
using CKSI1090.Core;
using CKSI1090.Operation.Common.View;
using CKSI1090.Shared;
using CKSI1090.Shared.Model;
using CKSI1090.Shared.ViewModel;
using Common;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;
using System.Xml.XPath;
using static CKSI1090.Common.Constants;
using static Common.MessageManager;
//*************************************************************************************
//
//   ベース画面のビューモデル
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   17.04.28              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1090.Base.ViewModel
{
    /// <summary>
    /// ベース画面のビューモデル
    /// </summary>
    public class BaseViewModel : ViewModelBase
    {
        #region フィールド

        /// <summary>
        /// データサービス
        /// </summary>
        private readonly IDataService dataService;

        /// <summary>
        /// 現在の棚卸操作
        /// </summary>
        private Common.Operation currentOperation;

        /// <summary>
        /// 棚卸操作用ユーザーコントロールを操作するためのインターフェース
        /// </summary>
        private IOperationViewBase operation;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dataService">データサービス</param>
        public BaseViewModel(IDataService dataService)
        {
            this.dataService = dataService;
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// アプリケーションの状態
        /// </summary>
        public BusyStatus BusyStatus => SharedViewModel.Instance.BusyStatus;

        /// <summary>
        /// 現在の操作
        /// </summary>
        public Common.Operation CurrentOperation
        {
            get { return currentOperation; }
            set
            {
                var lastOperation = currentOperation;

                if (!Set(ref currentOperation, value))
                {
                    return;
                }

                var userControl = (UserControl)Activator.CreateInstance(CurrentOperation.UserControl);
                operation = (IOperationViewBase)userControl;
                operation.InitializeAsync();

                Messenger.Default.Send(new ChangeOperationNotificationMessage(this)
                {
                    Operation = userControl
                });

            }
        }

        #endregion

        #region コマンド

        /// <summary>
        /// ロードイベントの発火
        /// </summary>
        public ICommand FireLoaded => new RelayCommand(async () =>
        {
            try
            {
                BusyStatus.IsBusy = true;

                // ログ出力用フォルダの存在チェック(なければフォルダを作成する)
                if (!CommonUtility.ExistOutputLogFolder()) CommonUtility.CreateOutputLogFolder();

                // 棚卸操作グループマスタに存在する資材ユーザであるかどうかをチェックする
                SharedViewModel.Instance.IsSizaiOfficeStaff 
                    = await dataService.CheckUserInTanaorosiOperationGroupMst(SharedModel.Instance.StartupArgs.EmployeeCode, SharedModel.Instance.StartupArgs.BelongingCode);

                // 期数の算出
                IEnumerable<InventoryTerm> periods = await dataService.GetInventoryPeriodsAsync(SystemCategory.Sizai.GetStringValue());

                // 定義ファイルのオープン
                var settings = Path.Combine(Path.GetDirectoryName(
                    Assembly.GetExecutingAssembly().Location) ?? string.Empty, "WorkNoteCheckListSettings.xml");
                var doc = XDocument.Load(settings);

                // 期末月の読込み
                SharedViewModel.Instance.OperationYearMonth.TermEndMonths.AddRange(
                    doc.XPathSelectElements("/Settings/EndTerms/EndTerm").Select(element => (int)element.Attribute("Month")));

                // 当月年
                SharedViewModel.Instance.OperationYearMonth.YearMonth = int.Parse(periods.First().YearMonth);
                // 当年
                SharedViewModel.Instance.OperationYearMonth.Year = int.Parse(periods.First().YearMonth.Substring(0, 4));
                // 当月
                SharedViewModel.Instance.OperationYearMonth.Month = int.Parse(periods.First().YearMonth.Substring(4, 2));
                // 期数
                SharedViewModel.Instance.OperationYearMonth.Term = periods.First().Term;
                // 上期/下期
                SharedViewModel.Instance.OperationYearMonth.Half = periods.First().Half;

                var elem = doc.XPathSelectElement($"/Settings/OperationDefinitions/Tanaorosi/OperationDefinition[@Title='作業誌チェックリスト']");
                var userControl = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(
                                    eachType => eachType.Name == (string)elem.Attribute("UserControlName"));
                CurrentOperation = new Common.Operation() { UserControl = userControl };

            }

            catch (Exception e)
            {
                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.ToString());
                MessageManager.ShowError(SystemID.CKSI1090, "FatalError");
                Application.Current.Shutdown(0);
            }

            finally
            {
                BusyStatus.IsBusy = false;
            }
        });

        /// <summary>
        /// [×]イベントの発火
        /// </summary>
        public ICommand FireClose => new RelayCommand(async () =>
        {
            if (operation != null)
            {
                bool result = await operation.CloseAsync();
                if (!result) return;
            }

            if (MessageManager.ShowQuestion(SystemID.CKSI1090, "ConfirmExit") == ResultType.No)
            {
                return;
            }

            Application.Current.Shutdown();
        });

        #endregion
    }
}
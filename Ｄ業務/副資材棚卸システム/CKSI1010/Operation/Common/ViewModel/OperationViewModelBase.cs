using AutoMapper;
using CKSI1010.Common;
using CKSI1010.Core;
using CKSI1010.Operation.Common.View;
using CKSI1010.Shared;
using CKSI1010.Shared.Model;
using CKSI1010.Shared.ViewModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Controls;
using static CKSI1010.Common.Constants;
using Microsoft.VisualBasic;
//*************************************************************************************
//
//   各棚卸操作の基底となるビューモデル
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1010.Operation.Common.ViewModel
{
    /// <summary>
    /// 各棚卸操作の基底となるビューモデル
    /// </summary>
    public abstract class OperationViewModelBase : ViewModelBase
    {

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dataService">データサービス</param>
        internal OperationViewModelBase(IDataService dataService)
        {
            DataService = dataService;
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// データサービス
        /// </summary>
        protected IDataService DataService { get; private set; }

        /// <summary>
        /// ビジー状態
        /// </summary>
        public BusyStatus BusyStatus => SharedViewModel.Instance.BusyStatus;

        #endregion

        #region メソッド

        /// <summary>
        /// 初期化
        /// </summary>
        /// <returns>非同期タスク</returns>
        internal virtual Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 確定
        /// </summary>
        /// <returns>非同期タスク</returns>
        internal virtual Task<bool> CommitAsync()
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// 修正
        /// </summary>
        /// <returns>非同期タスク</returns>
        internal virtual Task<bool> ModifyAsync()
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// 印刷
        /// </summary>
        /// <returns>非同期タスク</returns>
        internal virtual Task<bool> PrintAsync()
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <returns>非同期タスク</returns>
        internal virtual Task<bool> UpdateAsync()
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// 次へ
        /// </summary>
        /// <returns>非同期タスク</returns>
        internal virtual Task<bool> GoNextAsync()
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// 前へ
        /// </summary>
        /// <returns>非同期タスク</returns>
        internal virtual Task<bool> BackPreviousAsync()
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// 終了
        /// </summary>
        /// <returns>非同期タスク</returns>
        internal virtual Task<bool> ExitAsync()
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// キャンセル
        /// </summary>
        internal virtual void Cancel()
        {
        }

        /// <summary>
        /// ×ボタン
        /// </summary>
        /// <returns>非同期タスク</returns>
        internal virtual Task<bool> CloseAsync()
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// ぱんくずリスト選択
        /// </summary>
        /// <returns>非同期タスク</returns>
        internal virtual Task<bool> SelecOperationAsync()
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// ぱんくずリスト選択(前処理)
        /// </summary>
        /// <returns>非同期タスク</returns>
        internal virtual Task<bool> BeforeSelecOperationAsync()
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// 画面を生成する
        /// </summary>
        public UserControl CreateWindow(string windowName)
        {
            var tempControl = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(
                eachType => eachType.Name == windowName);

            var userControl = (UserControl)Activator.CreateInstance(tempControl);
            return userControl;
        }

        /// <summary>
        /// 画面をモーダル表示する
        /// </summary>
        public void ShowModalWindow(UserControl userControl)
        {
            ((IOperationViewBase)userControl).InitializeAsync();

            Messenger.Default.Send(new ShowModalWindowMessage(this)
            {
                Operation = (UserControl)userControl
            });
        }

        /// <summary>
        /// モーダル画面を閉じる
        /// </summary>
        public void CloseModalWindow()
        {
            Messenger.Default.Send(new CloseModalWindowMessage(this)
            {
            });
        }

        /// <summary>
        /// 操作状況の更新
        /// </summary>
        /// <param name="condition">操作状況</param>
        /// <returns>結果</returns>
        protected async Task<bool> UpdateOperationCondition(OperationCondition condition)
        {
            bool result = false;

            try
            {
                // 作業区分
                string workKbn = CommonUtility.GetCurrentOperationWorkKbn();

                // 操作順
                int order = CommonUtility.GetCurrentOperaionOrder();

                // 操作状況
                string conditionValue = string.Empty;

                if (condition == OperationCondition.Fix) // 確定にする場合
                {
                    // 現在の操作を確定状態にする
                    CommonUtility.ExcuteCurrentOperaionFixed();
                    conditionValue = CommonUtility.GetCurrentOperationStatusValue();
                }

                else if (condition == OperationCondition.Modify) // 修正にする場合
                {
                    // 現在の操作を修正状態にする
                    CommonUtility.ExcuteCurrentOperaionModify();
                    conditionValue = CommonUtility.GetCurrentOperationStatusValue();
                }

                // 棚卸進捗状況マスタの更新
                await DataService.UpdateTanaorosiProgressMst(workKbn, order, conditionValue);
                result = true;

            }

            catch (Exception)
            {
                throw;
            }

            return result;
        }

        /// <summary>
        /// 操作状況の更新
        /// </summary>
        /// <param name="condition">操作状況</param>
        /// <param name="isUpdateProgressMst">棚卸進捗マスタを更新するか</param>
        /// <returns>結果</returns>
        protected async Task<bool> UpdateOperationCondition(OperationCondition condition, bool isUpdateProgressMst)
        {
            bool result = false;

            try
            {
                // 作業区分
                string workKbn = CommonUtility.GetCurrentOperationWorkKbn();

                // 操作順
                int order = CommonUtility.GetCurrentOperaionOrder();

                // 操作状況
                string conditionValue = string.Empty;

                if (condition == OperationCondition.Fix) // 確定にする場合
                {
                    // 現在の操作を確定状態にする
                    CommonUtility.ExcuteCurrentOperaionFixed();
                    conditionValue = CommonUtility.GetCurrentOperationStatusValue();
                }

                else if (condition == OperationCondition.Modify) // 修正にする場合
                {
                    // 現在の操作を修正状態にする
                    CommonUtility.ExcuteCurrentOperaionModify();
                    conditionValue = CommonUtility.GetCurrentOperationStatusValue();
                }
                if(isUpdateProgressMst)
                {                
                    // 棚卸進捗状況マスタの更新
                    await DataService.UpdateTanaorosiProgressMst(workKbn, order, conditionValue);
                }

                result = true;

            }

            catch (Exception)
            {
                throw;
            }

            return result;
        }

        /// <summary>
        /// 棚卸ログの挿入
        /// </summary>
        /// <param name="logType">ログ種類</param>
        /// <param name="workKbn">作業区分</param>
        /// <param name="operationType">操作種類</param>
        /// <param name="operationContent">操作内容</param>
        /// <param name="bikou">備考</param>
        /// <param name="ErrorContent">エラー内容</param>
        /// <param name="ErrorCode">エラーコード</param>
        /// <returns>非同期タスク</returns>
        protected async Task<bool> InsertTanaorosiLog(LogType logType, SizaiWorkCategory workKbn, 
                                                     LogOperationType operationType, LogOperationContent operationContent,
                                                     string bikou = null, string errorContent = null, string errorCode = null)
        {
            bool ret = false;

            try
            {
                TanaorosiLog tanaorosiLog = new TanaorosiLog();
                tanaorosiLog.SystemCategory = ((int)SystemCategory.Sizai).ToString();
                tanaorosiLog.LogType = ((int)logType).ToString();
                tanaorosiLog.SyainCode = SharedModel.Instance.StartupArgs.EmployeeCode;
                tanaorosiLog.WorkKbn = ((int)workKbn).ToString();
                tanaorosiLog.OperationType = ((int)operationType).ToString();
                tanaorosiLog.OperationContent = ((int)operationContent).ToString();
                tanaorosiLog.Bikou = bikou;
                tanaorosiLog.ErrorContent = errorContent;
                tanaorosiLog.ErrorCode = errorCode;
                await DataService.InsertTanaorosiLog(tanaorosiLog);
                ret = true;
            }

            catch (Exception)
            {
                throw;
            }

            return ret;
        }

        /// <summary>
        /// 棚卸詳細ログの挿入
        /// </summary>
        /// <param name="logType">ログ種類</param>
        /// <param name="workKbn">作業区分</param>
        /// <param name="operationType">操作種類</param>
        /// <param name="operationContent">操作内容</param>
        /// <param name="sizaiKbn">資材区分</param>
        /// <param name="hinmokuCode">品目コード</param>
        /// <param name="gyosyaCode">業者コード</param>
        /// <param name="hinmokuNM">品目名</param>
        /// <param name="gyosyaNM">業者名</param>
        /// <param name="bikou">備考</param>
        /// <param name="updateContent1">変更内容1</param>
        /// <param name="updateContent2">変更内容2</param>
        /// <param name="updateContent3">変更内容3</param>
        /// <param name="errorContent">エラー内容</param>
        /// <returns>非同期タスク</returns>
        protected async Task<bool> InsertTanaorosiDetailLog(LogType logType, SizaiWorkCategory workKbn,
                                                           LogOperationType operationType, LogOperationContent operationContent,
                                                           SizaiCategory sizaiKbn, string hinmokuCode, string gyosyaCode, string hinmokuNM, string gyosyaNM,
                                                           string bikou = null, string updateContent1 = null, string updateContent2 = null, string updateContent3 = null, string errorContent = null)
        {
            bool ret = false;

            try
            {
                TanaorosiDetailLog tanaorosiDetailLog = new TanaorosiDetailLog();
                tanaorosiDetailLog.SystemCategory = ((int)SystemCategory.Sizai).ToString();
                tanaorosiDetailLog.LogType = ((int)logType).ToString();
                tanaorosiDetailLog.SyainCode = SharedModel.Instance.StartupArgs.EmployeeCode;
                tanaorosiDetailLog.WorkKbn = ((int)workKbn).ToString();
                tanaorosiDetailLog.OperationType = ((int)operationType).ToString();
                tanaorosiDetailLog.OperationContent = ((int)operationContent).ToString();
                tanaorosiDetailLog.SizaiKbn = ((int)sizaiKbn).ToString();
                tanaorosiDetailLog.HinmokuCode = hinmokuCode;
                tanaorosiDetailLog.GyosyaCode = gyosyaCode;
                tanaorosiDetailLog.HinmokuNM = hinmokuNM;
                tanaorosiDetailLog.GyosyaNM = gyosyaNM;
                tanaorosiDetailLog.Bikou = bikou;
                tanaorosiDetailLog.UpdateContent1 = updateContent1;
                tanaorosiDetailLog.UpdateContent2 = updateContent2;
                tanaorosiDetailLog.UpdateContent3 = updateContent3;
                tanaorosiDetailLog.ErrorContent = errorContent;
                await DataService.InsertTanaorosiDetailLog(tanaorosiDetailLog);
                ret = true;
            }

            catch (Exception)
            {
                throw;
            }

            return ret;
        }

        /// <summary>
        /// プロパティの移送
        /// </summary>
        /// <typeparam name="TSource">対象となるオブジェクト</typeparam>
        /// <typeparam name="TDistination">対象となるオブジェクト</typeparam>
        /// <param name="source">コピー元</param>
        /// <param name="distination">コピー先</param>
        protected Task<bool> CopyProperties<TSource, TDistination>(TSource source, out TDistination distination)
        {
            try
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<TSource, TDistination>();
                });

                var mapper = config.CreateMapper();
                distination = mapper.Map<TDistination>(source);
            }

            catch (Exception)
            {
                throw;
            }

            return Task.FromResult(true);
        }

        /// <summary>
        /// 作業誌モデルのリフレッシュ
        /// </summary>
        protected async Task RefreshWorkNoteModel()
        {
            try
            {
                // 入庫データ
                SharedModel.Instance.Receivings.Clear();
                SharedModel.Instance.Receivings.AddRange
                    (await DataService.GetWorkNotesAsync(SharedViewModel.Instance.OperationYearMonth.YearMonth.ToString(), SagyosiKbn.Nyuko.GetStringValue()));

                // 出庫データ
                SharedModel.Instance.Leavings.Clear();
                SharedModel.Instance.Leavings.AddRange
                    (await DataService.GetOutWorkNotesAsync(SharedViewModel.Instance.OperationYearMonth.YearMonth.ToString(), SagyosiKbn.Shukko.GetStringValue()));

                // 直送データ
                SharedModel.Instance.DirectDeliverys.Clear();
                SharedModel.Instance.DirectDeliverys.AddRange
                    (await DataService.GetDirectOutWorkNotesAsync(SharedViewModel.Instance.OperationYearMonth.YearMonth.ToString(), SagyosiKbn.Chokusou.GetStringValue()));

                // 返品データ
                SharedModel.Instance.Returns.Clear();
                SharedModel.Instance.Returns.AddRange
                    (await DataService.GetWorkNotesAsync(SharedViewModel.Instance.OperationYearMonth.YearMonth.ToString(), SagyosiKbn.Henpin.GetStringValue()));
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
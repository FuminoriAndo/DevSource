using CKSI1010.Common;
using CKSI1010.Core;
using CKSI1010.Operation.Common.ViewModel;
using CKSI1010.Shared;
using CKSI1010.Shared.Model;
using CKSI1010.Shared.ViewModel;
using Common;
using GalaSoft.MvvmLight.CommandWpf;
using log4net;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using static CKSI1010.Common.Constants;
using static Common.MessageManager;
//*************************************************************************************
//
//   資材班作業誌入力画面(液酸入庫)のビューモデル
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1010.Operation.InputInputWorkNote.ViewModel
{
    /// <summary>
    /// 資材班作業誌入力画面(液酸入庫)のビューモデル
    /// </summary>
    public class InputInputWorkNoteViewModel : OperationViewModelBase
    {
        #region フィールド

        /// <summary>
        /// ファイルダイアログのタイトル
        /// </summary>
        private readonly string FileDialog_Title = "ファイルを開く";

        /// <summary>
        /// ファイルダイアログのフィルタ
        /// </summary>
        private readonly string FileDialog_Filter = "全てのファイル(*.*)|*.*";

        /// <summary>
        /// 検針データExcelのファイル名として妥当な文字列
        /// </summary>
        private readonly string KensinExcel_FileName = "月末検針データ";

        /// <summary>
        /// 確定状態か
        /// </summary>
        private bool isFixed = false;

        /// <summary>
        /// 実行されたか
        /// </summary>
        private bool isExcute = false;

        /// <summary>
        /// ファイル名
        /// </summary>
        private string fileName = string.Empty;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dataService"></param>
        public InputInputWorkNoteViewModel(IDataService dataService)
            : base(dataService)
        {
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 確定状態か
        /// </summary>
        public bool IsFixed
        {
            get { return isFixed; }
            set { Set(ref isFixed, value); }
        }

        /// <summary>
        /// ファイル名
        /// </summary>
        public string FileName
        {
            get { return fileName; }
            set { Set(ref fileName, value); }
        }

        /// <summary>
        /// 実施年月
        /// </summary>
        public OperationYearMonth OperationYearMonth => SharedViewModel.Instance.OperationYearMonth;

        #endregion

        #region メソッド

        /// <summary>
        /// 初期化
        /// </summary>
        /// <returns>非同期タスク</returns>
        internal override async Task InitializeAsync()
        {
            try
            {
                IsFixed = CommonUtility.IsCurrentOperationFixed();
                FileName = KensinExcelOperator.ReadExcelFilePath(KensinDataType.Nyuko);
                await base.InitializeAsync();
            }

            catch (Exception e)
            {
                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.ToString());
                MessageManager.ShowError(SystemID.CKSI1010, "FatalError");
            }
        }

        /// <summary>
        /// 確定
        /// </summary>
        internal override async Task<bool> CommitAsync()
        {
            bool hasException = false;
            string errorContent = string.Empty;
            string errorCode = string.Empty;
            string sql = string.Empty;
            string sqlParam = string.Empty;
            string timestamp = string.Empty;

            try
            {
                if (isExcute)
                {
                    BusyStatus.IsBusy = true;

                    // 作業誌モデルのリフレッシュ
                    await RefreshWorkNoteModel();
                    
                    // 資材作業誌トランから当月の検針データ(入庫/直送)を取得する
                    SharedModel.Instance.MeterDataWorkNoteData.Clear();
                    SharedModel.Instance.MeterDataWorkNoteData.AddRange
                        (await DataService.GetMeterDataWorkNotesAsync
                            (SharedViewModel.Instance.OperationYearMonth.YearMonth.ToString(), 
                                SagyosiKbn.Nyuko.GetStringValue(), SagyosiKbn.Chokusou.GetStringValue()));

                    if (SharedModel.Instance.MeterDataWorkNoteData.Count > 0)
                    {
                        IList<MeterDataWorkNoteItem> itemList = new List<MeterDataWorkNoteItem>();
                        foreach (var workNoteItem in SharedModel.Instance.MeterDataWorkNoteData)
                        {
                            bool found = false;
                            foreach (var list in itemList)
                            {
                                if (workNoteItem.Mukesaki == list.Mukesaki &&
                                    workNoteItem.HinmokuCD == list.HinmokuCD &&
                                    workNoteItem.GyosyaCD == list.GyosyaCD)
                                {
                                    list.Amount += workNoteItem.Amount;
                                    if (workNoteItem.Kubun == SagyosiKbn.Chokusou.GetStringValue())
                                        list.Kubun = workNoteItem.Kubun;
                                    found = true;
                                    break;
                                }
                            }

                            if (found)
                                continue;
                            itemList.Add(workNoteItem);
                        }

                        // 資材棚卸ワークの検針データ(入庫)の数量を更新する
                        await DataService.UpdateSizaiTanaorosiWorkInOut
                                (itemList, SharedModel.Instance.WorkKbn.GetStringValue());
                    }

                    // 操作状況の更新
                    await UpdateOperationCondition(OperationCondition.Fix);
                    IsFixed = true;

                    return await base.CommitAsync();
                }

                else
                {
                    MessageManager.ShowExclamation(SystemID.CKSI1010, "NotExcute");
                    return false;
                }
            }

            catch (Exception e)
            {
                hasException = true;

                if (e is TanaorosiLogException)
                {
                    TanaorosiLogException logException = e as TanaorosiLogException;
                    timestamp = logException.TimeStamp;
                    sql = logException.SQL;
                    sqlParam = logException.SqlParam;
                    errorContent = logException.Message;
                    errorCode = logException.ErrorCode;
                    CommonUtility.WriteLogExceptionSQL(timestamp, sql, errorContent, errorCode, sqlParam);
                }
                else
                {
                    errorContent = e.Message;
                }

                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.ToString());
                return false;
            }

            finally
            {
                if (isExcute)
                {
                    // 棚卸ログの挿入
                    LogType logType = hasException ? LogType.Error : LogType.Normal;
                    await InsertTanaorosiLog(logType, SharedModel.Instance.WorkKbn,
                                             LogOperationType.InputInputWorkNote, 
                                             LogOperationContent.Fix, null, errorContent, errorCode);
                    if (!hasException)
                    {
                        MessageManager.ShowInformation(SystemID.CKSI1010, "CommitCompleted");
                    }
                    else
                    {
                        MessageManager.ShowError(SystemID.CKSI1010, "FatalError");
                    }
                }

                BusyStatus.IsBusy = false;
            }
        }

        /// <summary>
        /// 修正
        /// </summary>
        internal override async Task<bool> ModifyAsync()
        {
            bool hasException = false;
            string errorContent = string.Empty;
            string errorCode = string.Empty;
            string sql = string.Empty;
            string sqlParam = string.Empty;
            string timestamp = string.Empty;

            try
            {
                BusyStatus.IsBusy = true;

                // 共有データで保持している印刷対象外受払い情報の初期化
                CommonUtility.InitializeExcludeInputPaymentInfo();

                // 操作状況の更新
                await UpdateOperationCondition(OperationCondition.Modify);
                IsFixed = false;

                return await base.ModifyAsync();
            }

            catch (Exception e)
            {
                hasException = true;

                if (e is TanaorosiLogException)
                {
                    TanaorosiLogException logException = e as TanaorosiLogException;
                    timestamp = logException.TimeStamp;
                    sql = logException.SQL;
                    sqlParam = logException.SqlParam;
                    errorContent = logException.Message;
                    errorCode = logException.ErrorCode;
                    CommonUtility.WriteLogExceptionSQL(timestamp, sql, errorContent, errorCode, sqlParam);
                }
                else
                {
                    errorContent = e.Message;
                }

                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.ToString());
                return false;
            }

            finally
            {
                // 棚卸ログの挿入
                LogType logType = hasException ? LogType.Error : LogType.Normal;
                await InsertTanaorosiLog(logType, SharedModel.Instance.WorkKbn,
                                         LogOperationType.InputInputWorkNote, 
                                         LogOperationContent.Modify, null, errorContent, errorCode);
                if (hasException)
                {
                    MessageManager.ShowError(SystemID.CKSI1010, "FatalError");
                }

                BusyStatus.IsBusy = false;
            }
        }

        /// <summary>
        /// 次へ
        /// </summary>
        internal override async Task<bool> GoNextAsync()
        {
            try
            {
                BusyStatus.IsBusy = true;

                if (!IsFixed)
                {
                    MessageManager.ShowExclamation(SystemID.CKSI1010, "CanNotGoNext");
                    return false;
                }
                else
                {
                    return await base.GoNextAsync();
                }
            }

            catch (Exception e)
            {
                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.ToString());
                MessageManager.ShowError(SystemID.CKSI1010, "FatalError");
                return false;
            }

            finally
            {
                BusyStatus.IsBusy = false;
            }
        }

        #endregion

        #region コマンド

        /// <summary>
        /// ファイル選択
        /// </summary>
        public ICommand FileSelect => new RelayCommand(() =>
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Title = FileDialog_Title;
                dialog.Filter = FileDialog_Filter;

                if ((bool)dialog.ShowDialog())
                {
                    if (dialog.SafeFileName.IndexOf(KensinExcel_FileName, 0) < 0)
                    {
                        MessageManager.ShowExclamation(SystemID.CKSI1010, "NotFindKensinExcel");
                        return;
                    }

                    FileName = dialog.FileName;
                    KensinExcelOperator.WriteExcelFilePath(KensinDataType.Nyuko, FileName);
                }
            }

            catch (Exception e)
            {
                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.ToString());
                MessageManager.ShowError(SystemID.CKSI1010, "FatalError");
            }
        });

        /// <summary>
        /// 実行
        /// </summary>
        public ICommand ExecuteCKSI0010 => new RelayCommand(async () =>
        {
            bool hasException = false;
            string errorContent = string.Empty;
            string errorCode = string.Empty;
            string sql = string.Empty;
            string sqlParam = string.Empty;
            string timestamp = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(FileName))
                {
                    BusyStatus.IsBusy = true;
                    
                    // 資材作業誌入力を起動する
                    ExternalProcessAccessor.StartSizaiSagyosiNyuryoku(KensinDataType.Nyuko, FileName);
                    isExcute = true;
                }
                else
                {
                    MessageManager.ShowExclamation(SystemID.CKSI1010, "NotSelectedKensinExcelPath");
                }
            }

            catch (Exception e)
            {
                hasException = true;

                if (e is TanaorosiLogException)
                {
                    TanaorosiLogException logException = e as TanaorosiLogException;
                    timestamp = logException.TimeStamp;
                    sql = logException.SQL;
                    sqlParam = logException.SqlParam;
                    errorContent = logException.Message;
                    errorCode = logException.ErrorCode;
                    CommonUtility.WriteLogExceptionSQL(timestamp, sql, errorContent, errorCode, sqlParam);
                }
                else
                {
                    errorContent = e.Message;
                }

                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.ToString());
            }

            finally
            {
                if (!string.IsNullOrEmpty(FileName))
                {
                    // 棚卸ログの挿入
                    LogType logType = hasException ? LogType.Error : LogType.Normal;
                    await InsertTanaorosiLog(logType, SharedModel.Instance.WorkKbn,
                                             LogOperationType.InputInputWorkNote,
                                             LogOperationContent.Excute, null, errorContent, errorCode);
                    if (hasException)
                    {
                        MessageManager.ShowError(SystemID.CKSI1010, "FatalError");
                    }
                }

                BusyStatus.IsBusy = false;
            }
        });

        #endregion
    }
}

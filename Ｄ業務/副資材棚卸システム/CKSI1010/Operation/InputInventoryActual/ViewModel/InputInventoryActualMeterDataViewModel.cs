using CKSI1010.Common;
using CKSI1010.Core;
using CKSI1010.Shared;
using CKSI1010.Shared.Model;
using CKSI1010.Shared.ViewModel;
using CKSI1010.Types;
using Common;
using GalaSoft.MvvmLight.CommandWpf;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using static CKSI1010.Common.Constants;
using static Common.MessageManager;
//*************************************************************************************
//
//   棚卸実績値入力画面(検針データ入力作業)のビューモデル
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1010.Operation.InputInventoryActual.ViewModel
{
    /// <summary>
    /// 棚卸実績値入力画面(検針データ入力作業)のビューモデル
    /// </summary>
    public class InputInventoryActualMeterDataViewModel : InputInventoryActualBaseViewModel
    {
        #region フィールド

        /// <summary>
        /// 編集モード
        /// </summary>
        private bool isEditMode = false;

        /// <summary>
        /// 編集モード入力欄(品目CD)
        /// </summary>
        private string hinmokuCD = string.Empty;

        /// <summary>
        /// 編集モード入力欄(業者CD)
        /// </summary>
        private string gyosyaCD = string.Empty;

        /// <summary>
        /// 編集モード入力欄(品目名)
        /// </summary>
        private string hinmokuName = string.Empty;

        /// <summary>
        /// 編集モード入力欄(業者CD)
        /// </summary>
        private string gyosyaName = string.Empty;

        /// <summary>
        /// 編集フォームが表示されているか
        /// </summary>
        private bool isShowEditForm = false;

        /// <summary>
        /// SK一覧(削除対象)
        /// </summary>
        protected IList<InputInventoryActualRecordViewModel> DeleteSkList
            = new List<InputInventoryActualRecordViewModel>();

        /// <summary>
        /// EF一覧(削除対象)
        /// </summary>
        protected IList<InputInventoryActualRecordViewModel> DeleteEfList
            = new List<InputInventoryActualRecordViewModel>();

        /// <summary>
        /// LF一覧(削除対象)
        /// </summary>
        protected IList<InputInventoryActualRecordViewModel> DeleteLfList
            = new List<InputInventoryActualRecordViewModel>();

        /// <summary>
        /// 築炉一覧(削除対象)
        /// </summary>
        protected IList<InputInventoryActualRecordViewModel> DeleteBuildingList
            = new List<InputInventoryActualRecordViewModel>();

        /// <summary>
        /// LD一覧(削除対象)
        /// </summary>
        protected IList<InputInventoryActualRecordViewModel> DeleteLdList
            = new List<InputInventoryActualRecordViewModel>();

        /// <summary>
        /// TD一覧(削除対象)
        /// </summary>
        protected IList<InputInventoryActualRecordViewModel> DeleteTdList
            = new List<InputInventoryActualRecordViewModel>();

        /// <summary>
        /// CC一覧(削除対象)
        /// </summary>
        protected IList<InputInventoryActualRecordViewModel> DeleteCcList
            = new List<InputInventoryActualRecordViewModel>();

        /// <summary>
        /// その他一覧(削除対象)
        /// </summary>
        protected IList<InputInventoryActualRecordViewModel> DeleteOthersList
            = new List<InputInventoryActualRecordViewModel>();

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public InputInventoryActualMeterDataViewModel(IDataService dataService) : base(dataService)
        {
            ShowSK = false;
            ShowEF = true;
            ShowLF = false;
            ShowBuilding = false;
            ShowLD = false;
            ShowTD = false;
            ShowCC = false;
            ShowOthers = false;

            SelectShizaiKbn = ShizaiKbnTypes.EF;
            SelectIndex = -1;

            IsEditMode = false;
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 編集モード
        /// </summary>
        public bool IsEditMode
        {
            get { return isEditMode; }
            set { Set(ref isEditMode, value); }
        }

        /// <summary>
        /// 品目コード
        /// </summary>
        public string HinmokuCD
        {
            get { return hinmokuCD; }
            set { Set(ref hinmokuCD, value); }
        }

        /// <summary>
        /// 業者コード
        /// </summary>
        public string GyosyaCD
        {
            get { return gyosyaCD; }
            set { Set(ref gyosyaCD, value); }
        }

        /// <summary>
        /// 品目名
        /// </summary>
        public string HinmokuName
        {
            get { return hinmokuName; }
            set { Set(ref hinmokuName, value); }
        }

        /// <summary>
        /// 業者名
        /// </summary>
        public string GyosyaName
        {
            get { return gyosyaName; }
            set { Set(ref gyosyaName, value); }
        }


        /// <summary>
        /// 編集フォームが表示されているか
        /// </summary>
        public bool IsShowEditForm
        {
            get { return isShowEditForm; }
            set { Set(ref isShowEditForm, value); }
        }

        #endregion

        #region コマンド

        /// <summary>
        /// 追加ボタン押下
        /// </summary>
        public ICommand FireUpdateEditMode => new RelayCommand(() =>
        {
            IsEditMode = true;
            IsShowEditForm = true;
            clearEditItem();
        });

        /// <summary>
        /// 上下への行移動
        /// </summary>
        public ICommand FireReOrder => new RelayCommand<int>(async direction =>
        {
            int oldIndex = -1;
            int toIndex = -1;
            bool hasException = false;
            LogOperationContent operationContent;
            string beforeHinmokuCD = string.Empty;
            string beforeGyosyaCD = string.Empty;
            string beforeHinmokuName = string.Empty;
            string beforeGyosyaName = string.Empty;
            string errorContent = null;
            string errorCode = null;
            SizaiCategory sizaiCategory = SizaiCategory.None;

            try
            {
                if (-1 == SelectIndex)
                {
                    MessageManager.ShowExclamation(SystemID.CKSI1010, "SelectSortItem");
                    return;
                }

                oldIndex = SelectIndex;
                toIndex = (0 == direction) ? SelectIndex + 1 : SelectIndex - 1;

                switch (SelectShizaiKbn)
                {
                    case ShizaiKbnTypes.SK:
                        beforeHinmokuCD = SkList[oldIndex].HinmokuCd;
                        beforeGyosyaCD = SkList[oldIndex].GyosyaCd;
                        beforeHinmokuName = SkList[oldIndex].HinmokuName;
                        beforeGyosyaName = SkList[oldIndex].GyosyaName;
                        sizaiCategory = SizaiCategory.SK;
                        toIndex = moveItem(ref skList, SelectIndex, toIndex);
                        SelectItem = SkList[toIndex];
                        break;
                    case ShizaiKbnTypes.EF:
                        beforeHinmokuCD = EfList[oldIndex].HinmokuCd;
                        beforeGyosyaCD = EfList[oldIndex].GyosyaCd;
                        beforeHinmokuName = EfList[oldIndex].HinmokuName;
                        beforeGyosyaName = EfList[oldIndex].GyosyaName;
                        sizaiCategory = SizaiCategory.EF;
                        toIndex = moveItem(ref efList, SelectIndex, toIndex);
                        SelectItem = EfList[toIndex];
                        break;
                    case ShizaiKbnTypes.LF:
                        beforeHinmokuCD = LfList[oldIndex].HinmokuCd;
                        beforeGyosyaCD = LfList[oldIndex].GyosyaCd;
                        beforeHinmokuName = LfList[oldIndex].HinmokuName;
                        beforeGyosyaName = LfList[oldIndex].GyosyaName;
                        sizaiCategory = SizaiCategory.LF;
                        toIndex = moveItem(ref lfList, SelectIndex, toIndex);
                        SelectItem = LfList[toIndex];
                        break;
                    case ShizaiKbnTypes.Building:
                        beforeHinmokuCD = BuildingList[oldIndex].HinmokuCd;
                        beforeGyosyaCD = BuildingList[oldIndex].GyosyaCd;
                        beforeHinmokuName = BuildingList[oldIndex].HinmokuName;
                        beforeGyosyaName = BuildingList[oldIndex].GyosyaName;
                        sizaiCategory = SizaiCategory.Building;
                        toIndex = moveItem(ref buildingList, SelectIndex, toIndex);
                        SelectItem = BuildingList[toIndex];
                        break;
                    case ShizaiKbnTypes.LD:
                        beforeHinmokuCD = LdList[oldIndex].HinmokuCd;
                        beforeGyosyaCD = LdList[oldIndex].GyosyaCd;
                        beforeHinmokuName = LdList[oldIndex].HinmokuName;
                        beforeGyosyaName = LdList[oldIndex].GyosyaName;
                        sizaiCategory = SizaiCategory.LD;
                        toIndex = moveItem(ref ldList, SelectIndex, toIndex);
                        SelectItem = LdList[toIndex];
                        break;
                    case ShizaiKbnTypes.TD:
                        beforeHinmokuCD = TdList[oldIndex].HinmokuCd;
                        beforeGyosyaCD = TdList[oldIndex].GyosyaCd;
                        beforeHinmokuName = TdList[oldIndex].HinmokuName;
                        beforeGyosyaName = TdList[oldIndex].GyosyaName;
                        sizaiCategory = SizaiCategory.TD;
                        toIndex = moveItem(ref tdList, SelectIndex, toIndex);
                        SelectItem = TdList[toIndex];
                        break;
                    case ShizaiKbnTypes.CC:
                        beforeHinmokuCD = CcList[oldIndex].HinmokuCd;
                        beforeGyosyaCD = CcList[oldIndex].GyosyaCd;
                        beforeHinmokuName = CcList[oldIndex].HinmokuName;
                        beforeGyosyaName = CcList[oldIndex].GyosyaName;
                        sizaiCategory = SizaiCategory.CC;
                        toIndex = moveItem(ref ccList, SelectIndex, toIndex);
                        SelectItem = CcList[toIndex];
                        break;
                    case ShizaiKbnTypes.Others:
                        beforeHinmokuCD = OthersList[oldIndex].HinmokuCd;
                        beforeGyosyaCD = OthersList[oldIndex].GyosyaCd;
                        beforeHinmokuName = OthersList[oldIndex].HinmokuName;
                        beforeGyosyaName = OthersList[oldIndex].GyosyaName;
                        sizaiCategory = SizaiCategory.Others;
                        toIndex = moveItem(ref othersList, SelectIndex, toIndex);
                        SelectItem = OthersList[toIndex];
                        break;
                    default:
                        return;
                }
            }

            catch (Exception e)
            {
                hasException = true;
                errorContent = e.Message;
                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.ToString());
                MessageManager.ShowError(SystemID.CKSI1010, "FatalError");
            }

            finally
            {
                if (SelectIndex != -1)
                {
                    LogType logType = hasException ? LogType.Error : LogType.Normal;
                    operationContent = direction == 0 ? LogOperationContent.Down : LogOperationContent.Up;

                    // 棚卸ログの挿入
                    await InsertTanaorosiLog(logType, SizaiWorkCategory.KensinWork, 
                                             LogOperationType.InputInventoryActual, 
                                             operationContent, null, errorContent, errorCode);
                    // 棚卸詳細ログの挿入
                    if (oldIndex != -1)
                    {
                        await InsertTanaorosiDetailLog
                            (logType, SizaiWorkCategory.KensinWork,
                             LogOperationType.InputInventoryActual, operationContent, sizaiCategory,
                             beforeHinmokuCD.PadRight(4),
                             beforeGyosyaCD.PadRight(4),
                             CommonUtility.PadRightSJIS(beforeHinmokuName, 40),
                             CommonUtility.PadRightSJIS(beforeGyosyaName, 24), null,
                             Constants.SortOrder_StringDefine + (toIndex + 1).ToString(), 
                             null, null, errorContent
                        );
                    }
                }
            }
        });

        /// <summary>
        /// 変更ボタン押下
        /// </summary>
        public ICommand FireUpdate => new RelayCommand(() =>
        {
            if (null == SelectItem)
            {
                return;
            }

            IsEditMode = true;

            // 編集領域に選択項目を表示する
            HinmokuCD = SelectItem.HinmokuCd;
            GyosyaCD = SelectItem.GyosyaCd;
            HinmokuName = SelectItem.HinmokuName;
            GyosyaName = SelectItem.GyosyaName;
        });

        /// <summary>
        /// 削除ボタン押下
        /// </summary>
        public ICommand FireDelete => new RelayCommand(async () =>
        {
            string targetHinmokuCD = string.Empty;
            string targetGyosyaCD = string.Empty;
            string targetHinmokuName = string.Empty;
            string targetGyosyaName = string.Empty;
            SizaiCategory sizaiCategory = SizaiCategory.None;

            bool hasException = false;
            bool hasError = false;
            string errorContent = null;
            string errorCode = null;
            bool isLogExcluded = false;

            try
            {
                if (-1 == SelectIndex)
                {
                    MessageManager.ShowExclamation(SystemID.CKSI1010, "SelectDeleteItem");
                    hasError = true;
                    return;
                }

                if (null == SelectItem)
                {
                    MessageManager.ShowExclamation(SystemID.CKSI1010, "SelectDeleteItem");
                    hasError = true;
                    return;
                }

                if (MessageManager.ShowQuestion(SystemID.CKSI1010, "ConfirmDelete") == ResultType.Yes)
                {
                    int index = 0;

                    targetHinmokuCD = SelectItem.HinmokuCd;
                    targetGyosyaCD = SelectItem.GyosyaCd;
                    targetHinmokuName = SelectItem.HinmokuName;
                    targetGyosyaName = SelectItem.GyosyaName;

                    switch (SelectShizaiKbn)
                    {
                        case ShizaiKbnTypes.SK:
                            index = removeItem(ref skList, SelectIndex);
                            if (SkList.Count() > 0)
                            {
                                SelectItem = SkList[(index < 0) ? 0 : index];
                            }
                            sizaiCategory = SizaiCategory.SK;
                            break;
                        case ShizaiKbnTypes.EF:
                            index = removeItem(ref efList, SelectIndex);
                            if (EfList.Count() > 0)
                            {
                                SelectItem = EfList[(index < 0) ? 0 : index];
                            }
                            sizaiCategory = SizaiCategory.EF;
                            break;
                        case ShizaiKbnTypes.LF:
                            index = removeItem(ref lfList, SelectIndex);
                            if (LfList.Count() > 0)
                            {
                                SelectItem = LfList[(index < 0) ? 0 : index];
                            }
                            sizaiCategory = SizaiCategory.LF;
                            break;
                        case ShizaiKbnTypes.Building:
                            index = removeItem(ref buildingList, SelectIndex);
                            if (BuildingList.Count() > 0)
                            {
                                SelectItem = BuildingList[(index < 0) ? 0 : index];
                            }
                            sizaiCategory = SizaiCategory.Building;
                            break;
                        case ShizaiKbnTypes.LD:
                            index = removeItem(ref ldList, SelectIndex);
                            if (LdList.Count() > 0)
                            {
                                SelectItem = LdList[(index < 0) ? 0 : index];
                            }
                            sizaiCategory = SizaiCategory.LD;
                            break;
                        case ShizaiKbnTypes.TD:
                            index = removeItem(ref tdList, SelectIndex);
                            if (TdList.Count() > 0)
                            {
                                SelectItem = TdList[(index < 0) ? 0 : index];
                            }
                            sizaiCategory = SizaiCategory.TD;
                            break;
                        case ShizaiKbnTypes.CC:
                            index = removeItem(ref ccList, SelectIndex);
                            if (CcList.Count() > 0)
                            {
                                SelectItem = CcList[(index < 0) ? 0 : index];
                            }
                            sizaiCategory = SizaiCategory.CC;
                            break;
                        case ShizaiKbnTypes.Others:
                            index = removeItem(ref othersList, SelectIndex);
                            if (OthersList.Count() > 0)
                            {
                                SelectItem = OthersList[(index < 0) ? 0 : index];
                            }
                            sizaiCategory = SizaiCategory.Others;
                            break;
                        default:
                            return;
                    }
                }
                else
                {
                    isLogExcluded = true;
                }
            }

            catch (Exception e)
            {
                hasException = true;
                errorContent = e.Message;
                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.ToString());
                MessageManager.ShowError(SystemID.CKSI1010, "FatalError");
            }

            finally
            {
                if (!hasError && !isLogExcluded)
                {
                    LogType logType = hasException ? LogType.Error : LogType.Normal;

                    // 棚卸ログの挿入
                    await InsertTanaorosiLog(logType, SizaiWorkCategory.KensinWork, 
                                             LogOperationType.InputInventoryActual, 
                                             LogOperationContent.Delete,null,errorContent, errorCode);
                    // 棚卸詳細ログの挿入
                    await InsertTanaorosiDetailLog
                        (logType, SizaiWorkCategory.KensinWork,
                         LogOperationType.InputInventoryActual, 
                         LogOperationContent.Delete, sizaiCategory,
                         targetHinmokuCD.PadRight(4),
                         targetGyosyaCD.PadRight(4),
                         CommonUtility.PadRightSJIS(targetHinmokuName, 40),
                         CommonUtility.PadRightSJIS(targetGyosyaName, 24),
                         null, null, null, null, errorContent);
                }
            }

        });

        /// <summary>
        /// 保存ボタン押下
        /// </summary>
        public new ICommand FireSave => new RelayCommand(async () =>
        {
            bool hasException = false;
            string errorContent = null;
            string errorCode = null;
            string sql = null;
            string sqlParam = null;
            string timestamp = null;
            bool isCompleted = false;

            try
            {
                BusyStatus.IsBusy = true;
                if (await SaveInventoriesWorkAsync()) isCompleted = true;
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
                if (isCompleted)
                {
                    // 棚卸ログの挿入
                    LogType logType = hasException ? LogType.Error : LogType.Normal;
                    await InsertTanaorosiLog(logType, SizaiWorkCategory.KensinWork, 
                                             LogOperationType.InputInventoryActual, 
                                             LogOperationContent.Save, null, errorContent, errorCode);
                    if (!hasException)
                    {
                        MessageManager.ShowInformation(SystemID.CKSI1010, "SaveCompleted");
                    }
                }

                else
                {
                    MessageManager.ShowError(SystemID.CKSI1010, "FatalError");
                }

                BusyStatus.IsBusy = false;
            }
        });

        /// <summary>
        /// ロストフォーカス
        /// </summary>
        public ICommand FireLostFocus => new RelayCommand<string>(type =>
        {
            string code = string.Empty;

            try
            {
                // 品目コード
                if (type == InputChecker.Hinmoku_StringDefine)
                {
                    // 空文字の場合は何もしない
                    if (HinmokuCD.Length == 0)
                    {
                        HinmokuName = string.Empty;
                        return;
                    }

                    code = HinmokuCD;

                    var check = InputChecker.CheckString(HinmokuCD);
                    if (InputChecker.Err_Check_Str_Unmatch == check)
                    {
                        MessageManager.ShowExclamation(SystemID.CKSI1010, "UnmatchCharsType");
                        HinmokuCD = string.Empty;
                        return;
                    }
                    else if (InputChecker.Err_Check_Str_Over == check)
                    {
                        MessageManager.ShowExclamation(SystemID.CKSI1010, "UnmatchCharsCountHINMOKU");
                        HinmokuCD = string.Empty;
                        return;
                    }
                }
                // 業者コード
                else if (type == InputChecker.Gyosya_StringDefine)
                {
                    //空文字の場合は何もしない
                    if (GyosyaCD.Length == 0)
                    {
                        GyosyaName = string.Empty;
                        return;
                    }

                    code = GyosyaCD;

                    var check = InputChecker.CheckString(GyosyaCD);
                    if (InputChecker.Err_Check_Str_Unmatch == check)
                    {
                        MessageManager.ShowExclamation(SystemID.CKSI1010, "UnmatchCharsType");
                        GyosyaCD = string.Empty;
                        return;
                    }
                    else if (InputChecker.Err_Check_Str_Over == check)
                    {
                        MessageManager.ShowExclamation(SystemID.CKSI1010, "UnmatchCharsCountGYOSYA");
                        GyosyaCD = string.Empty;
                        return;
                    }
                }

                // コード(品目コード or 業者コード)に紐付く名称を取得する
                var ret = DataService.CodeToName(type, code);

                // 取得できない場合
                if ((ret == null) || (ret.Result.Length == 0))
                {
                    MessageManager.ShowExclamation(SystemID.CKSI1010, "InputNotExistCode");

                    if (type == InputChecker.Hinmoku_StringDefine)
                    {
                        HinmokuCD = string.Empty;
                    }
                    else
                    {
                        GyosyaCD = string.Empty;
                    }

                    return;
                }

                // 取得できたので更新する
                if (type == InputChecker.Hinmoku_StringDefine)
                {
                    HinmokuName = ret.Result;
                }
                else
                {
                    GyosyaName = ret.Result;
                }

                return;
            }

            catch (Exception e)
            {
                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.ToString());
                MessageManager.ShowError(SystemID.CKSI1010, "FatalError");
            }

        });

        /// <summary>
        /// 更新ボタン押下
        /// </summary>
        public ICommand FireUpdateData => new RelayCommand(async () =>
        {
            bool hasException = false;
            bool hasError = false;
            string errorContent = null;
            string errorCode = null;
            string sql = string.Empty;
            string sqlParam = string.Empty;
            string timestamp = string.Empty;

            string targetHinmokuCode = string.Empty;
            string targetGyosyaCode = string.Empty;
            string targetHinmokuName = string.Empty;
            string targetGyosyaName = string.Empty;
            SizaiCategory sizaiCategory = SizaiCategory.None;

            try
            {
                if (HinmokuCD == string.Empty)
                {
                    MessageManager.ShowExclamation(SystemID.CKSI1010, "InputIncomplete");
                    hasError = true;
                    return;
                }

                var hinmokuCode = HinmokuCD.PadRight(4);
                // 品目コードの受払い種別を取得
                var isSiyoudakaBarai = await DataService.GetUkebaraiSyubetuAsync(hinmokuCode) == UkebaraiType.SiyoudakaBarai ? true : false;
                if (isSiyoudakaBarai)
                {
                    if (string.IsNullOrEmpty(GyosyaCD.Trim()))
                    {
                        MessageManager.ShowExclamation(SystemID.CKSI1010, "NotInputGyosyaCode");
                        hasError = true;
                        return;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(GyosyaCD.Trim()))
                    {
                        // 入力した品目コードの受払い種別が入庫払いで、
                        // 業者コードが入力されている場合は、入力エラーとする。
                        MessageManager.ShowExclamation(SystemID.CKSI1010, "DoNotInputGyosyaCode");
                        hasError = true;
                        return;
                    }
                }

                // 業者CDは任意入力のため、NULL文字なら半角スペースをＤＢ桁数に設定する
                string gyosyaCD = (GyosyaCD != string.Empty) ? GyosyaCD : GyosyaCD.PadRight(4);
                string gyosyaName = (GyosyaName != string.Empty) ? GyosyaName : GyosyaName.PadRight(40);

                int shizaiKbn = (int)SelectShizaiKbn;
                int workKbn = SizaiWorkCategory.KensinWork.GetIntValue();

                bool ret = false;
                switch (SelectShizaiKbn)
                {
                    case ShizaiKbnTypes.SK:
                        ret = isExist(ref skList, HinmokuCD, gyosyaCD);
                        break;
                    case ShizaiKbnTypes.EF:
                        ret = isExist(ref efList, HinmokuCD, gyosyaCD);
                        break;
                    case ShizaiKbnTypes.LF:
                        ret = isExist(ref lfList, HinmokuCD, gyosyaCD);
                        break;
                    case ShizaiKbnTypes.Building:
                        ret = isExist(ref buildingList, HinmokuCD, gyosyaCD);
                        break;
                    case ShizaiKbnTypes.LD:
                        ret = isExist(ref ldList, HinmokuCD, gyosyaCD);
                        break;
                    case ShizaiKbnTypes.TD:
                        ret = isExist(ref tdList, HinmokuCD, gyosyaCD);
                        break;
                    case ShizaiKbnTypes.CC:
                        ret = isExist(ref ccList, HinmokuCD, gyosyaCD);
                        break;
                    case ShizaiKbnTypes.Others:
                        ret = isExist(ref othersList, HinmokuCD, gyosyaCD);
                        break;
                    default:
                        return;
                }

                if (ret)
                {
                    MessageManager.ShowInformation(SystemID.CKSI1010, "InputDuplexdata");
                    hasError = true;
                    return;
                }

                List<InventoryMaster> masterList = new List<InventoryMaster>();
                masterList.AddRange(await DataService.GetInventoryMasterAsync(shizaiKbn, workKbn, HinmokuCD, gyosyaCD));

                if (masterList.Count > 0)
                {
                    MessageManager.ShowExclamation(SystemID.CKSI1010, "InputDuplexdata");
                    hasError = true;
                    return;
                }

                if (SelectIndex == -1)
                {
                    SelectIndex = 0;
                }

                // 入庫情報を設定(無い場合は初期値のまま)
                WorkNoteItem receiving = null;
                int nyukoValue = 0;
                if (isSiyoudakaBarai)
                {
                    receiving = SharedModel.Instance.Receivings.FirstOrDefault(e => (e.ItemCode == HinmokuCD) && (e.GyosyaCD == gyosyaCD));
                    if (null != receiving)
                    {
                        nyukoValue = (int)receiving.Amount;
                    }
                }
                else
                {
                    receiving = SharedModel.Instance.Receivings.FirstOrDefault(e => e.ItemCode == HinmokuCD);
                    if (null != receiving)
                    {
                        // 入庫払いは業者コードを指定しないため、
                        // 取得した入庫情報のサマリを入庫量に設定
                        nyukoValue = (int)SharedModel.Instance.Receivings.Where(n => n.ItemCode == HinmokuCD).Select(x => x.Amount).Sum();
                    }
                }
                
                // 出庫情報を設定(無い場合は初期値のまま)
                int haraiValue = 0;
                switch ((ShizaiKbnTypes)SelectShizaiKbn)
                {
                    case ShizaiKbnTypes.SK:
                        var leaveingSK = SharedModel.Instance.Leavings
                            .FirstOrDefault(n => n.ItemCode == HinmokuCD);
                        if (null != leaveingSK)
                        {
                            haraiValue += (int)leaveingSK.Amount;
                        }
                        break;

                    case ShizaiKbnTypes.EF:
                        var leaveingEF = SharedModel.Instance.Leavings
                            .FirstOrDefault(n => (n.ItemCode == HinmokuCD) 
                                && (n.Mukesaki == Mukesaki.EF.GetStringValue()));
                        if (null != leaveingEF)
                        {
                            haraiValue += (int)leaveingEF.Amount;
                        }
                        break;

                    case ShizaiKbnTypes.LF:
                        var leaveingLF = SharedModel.Instance.Leavings
                            .FirstOrDefault(n => (n.ItemCode == HinmokuCD) 
                                && (n.Mukesaki == Mukesaki.LF.GetStringValue()));
                        if (null != leaveingLF)
                        {
                            haraiValue += (int)leaveingLF.Amount;
                        }
                        break;

                    case ShizaiKbnTypes.Building:
                        var leaveingBuilding = SharedModel.Instance.Leavings
                            .FirstOrDefault(n => (n.ItemCode == HinmokuCD) 
                                && (n.Mukesaki == Mukesaki.Building.GetStringValue()));
                        if (null != leaveingBuilding)
                        {
                            haraiValue += (int)leaveingBuilding.Amount;
                        }
                        break;

                    case ShizaiKbnTypes.LD:
                        var leaveingLD = SharedModel.Instance.Leavings
                            .FirstOrDefault(n => (n.ItemCode == HinmokuCD) 
                                && (n.Mukesaki == Mukesaki.LD.GetStringValue()));
                        if (null != leaveingLD)
                        {
                            haraiValue += (int)leaveingLD.Amount;
                        }
                        break;

                    case ShizaiKbnTypes.CC:
                        var leaveingCC = SharedModel.Instance.Leavings
                            .FirstOrDefault(n => (n.ItemCode == HinmokuCD) 
                                && (n.Mukesaki == Mukesaki.CC.GetStringValue()));
                        if (null != leaveingCC)
                        {
                            haraiValue += (int)leaveingCC.Amount;
                        }
                        break;

                    case ShizaiKbnTypes.TD:
                        var leaveingTD = SharedModel.Instance.Leavings
                            .FirstOrDefault(n => (n.ItemCode == HinmokuCD) 
                                && (n.Mukesaki == Mukesaki.TD.GetStringValue()));
                        if (null != leaveingTD)
                        {
                            haraiValue += (int)leaveingTD.Amount;
                        }
                        break;

                    case ShizaiKbnTypes.Others:
                        var leaveingETC = SharedModel.Instance.Leavings
                            .FirstOrDefault(n => (n.ItemCode == HinmokuCD) 
                                && (n.Mukesaki == Mukesaki.Others.GetStringValue()));
                        if (null != leaveingETC)
                        {
                            haraiValue += (int)leaveingETC.Amount;
                        }
                        break;

                    default:
                        break;
                }

                // 直送情報を設定(無い場合は初期値のまま)
                switch ((ShizaiKbnTypes)SelectShizaiKbn)
                {
                    case ShizaiKbnTypes.SK:
                        OutWorkNoteItem directDeliverysSK = null;
                        if (isSiyoudakaBarai)
                        {
                            // 品目コード、業者コードをキーにして直送(SK)情報を取得
                            directDeliverysSK = SharedModel.Instance.DirectDeliverys
                                                    .FirstOrDefault(n => (n.ItemCode == HinmokuCD)
                                                    && (n.GyosyaCD == gyosyaCD));

                            if (null != directDeliverysSK)
                            {
                                nyukoValue += (int)directDeliverysSK.Amount;
                                haraiValue += (int)directDeliverysSK.Amount;
                            }
                        }
                        else
                        {
                            // 品目コードをキーにして直送(SK)情報を取得
                            directDeliverysSK = SharedModel.Instance.DirectDeliverys
                                                   .FirstOrDefault(n => n.ItemCode == HinmokuCD);

                            if (null != directDeliverysSK)
                            {
                                // 入庫払いは業者コードを指定しないため、
                                // 取得した直送(SK)情報のサマリを入庫量/出庫量に設定
                                var amount = (int)SharedModel.Instance.DirectDeliverys.Where(n => n.ItemCode == HinmokuCD).Select(x => x.Amount).Sum();
                                nyukoValue += amount;
                                haraiValue += amount;
                            }
                        }
                        break;

                    case ShizaiKbnTypes.EF:
                        OutWorkNoteItem directDeliverysEF = null;
                        if (isSiyoudakaBarai)
                        {
                            // 品目コード、業者コード、向先をキーにして直送(EF)情報を取得
                            directDeliverysEF = SharedModel.Instance.DirectDeliverys
                                                    .FirstOrDefault(n => (n.ItemCode == HinmokuCD)
                                                    && (n.GyosyaCD == gyosyaCD)
                                                    && (n.Mukesaki == Mukesaki.EF.GetStringValue()));

                            if (null != directDeliverysEF)
                            {
                                nyukoValue += (int)directDeliverysEF.Amount;
                                haraiValue += (int)directDeliverysEF.Amount;
                            }
                        }
                        else
                        {
                            // 品目コード、向先をキーにして直送(EF)情報を取得
                            directDeliverysEF = SharedModel.Instance.DirectDeliverys
                                                    .FirstOrDefault(n => (n.ItemCode == HinmokuCD)
                                                    && (n.Mukesaki == Mukesaki.EF.GetStringValue()));

                            if (null != directDeliverysEF)
                            {
                                // 入庫払いは業者コードを指定しないため、
                                // 取得した直送(EF)情報のサマリを入庫量/出庫量に設定
                                var amount = (int)SharedModel.Instance.DirectDeliverys
                                    .Where(n => (n.ItemCode == HinmokuCD)
                                    && (n.Mukesaki == Mukesaki.EF.GetStringValue())).Select(x => x.Amount).Sum();
                                nyukoValue += amount;
                                haraiValue += amount;
                            }
                        }
                        break;

                    case ShizaiKbnTypes.LF:
                        OutWorkNoteItem directDeliverysLF = null;
                        if (isSiyoudakaBarai)
                        {
                            // 品目コード、業者コード、向先をキーにして直送(LF)情報を取得
                            directDeliverysLF = SharedModel.Instance.DirectDeliverys
                                                    .FirstOrDefault(n => (n.ItemCode == HinmokuCD)
                                                    && (n.GyosyaCD == gyosyaCD)
                                                    && (n.Mukesaki == Mukesaki.LF.GetStringValue()));

                            if (null != directDeliverysLF)
                            {
                                nyukoValue += (int)directDeliverysLF.Amount;
                                haraiValue += (int)directDeliverysLF.Amount;
                            }
                        }
                        else
                        {
                            // 品目コード、向先をキーにして直送(LF)情報を取得
                            directDeliverysLF = SharedModel.Instance.DirectDeliverys
                                                    .FirstOrDefault(n => (n.ItemCode == HinmokuCD)
                                                    && (n.Mukesaki == Mukesaki.LF.GetStringValue()));


                            if (null != directDeliverysLF)
                            {
                                // 入庫払いは業者コードを指定しないため、
                                // 取得した直送(LF)情報のサマリを入庫量/出庫量に設定
                                var amount = (int)SharedModel.Instance.DirectDeliverys
                                    .Where(n => (n.ItemCode == HinmokuCD)
                                    && (n.Mukesaki == Mukesaki.LF.GetStringValue())).Select(x => x.Amount).Sum();
                                nyukoValue += amount;
                                haraiValue += amount;
                            }
                        }
                        break;

                    case ShizaiKbnTypes.Building:
                        OutWorkNoteItem directDeliverysBuilding = null;
                        if (isSiyoudakaBarai)
                        {
                            // 品目コード、業者コード、向先をキーにして直送(築炉)情報を取得
                            directDeliverysBuilding = SharedModel.Instance.DirectDeliverys
                                                        .FirstOrDefault(n => (n.ItemCode == HinmokuCD)
                                                        && (n.GyosyaCD == gyosyaCD)
                                                        && (n.Mukesaki == Mukesaki.Building.GetStringValue()));

                            if (null != directDeliverysBuilding)
                            {
                                nyukoValue += (int)directDeliverysBuilding.Amount;
                                haraiValue += (int)directDeliverysBuilding.Amount;
                            }
                        }
                        else
                        {
                            // 品目コード、向先をキーにして直送(築炉)情報を取得
                            directDeliverysBuilding = SharedModel.Instance.DirectDeliverys
                                                        .FirstOrDefault(n => (n.ItemCode == HinmokuCD)
                                                        && (n.Mukesaki == Mukesaki.Building.GetStringValue()));

                            {
                                // 入庫払いは業者コードを指定しないため、
                                // 取得した直送(築炉)情報のサマリを入庫量/出庫量に設定
                                var amount = (int)SharedModel.Instance.DirectDeliverys
                                    .Where(n => (n.ItemCode == HinmokuCD)
                                    && (n.Mukesaki == Mukesaki.Building.GetStringValue())).Select(x => x.Amount).Sum();
                                nyukoValue += amount;
                                haraiValue += amount;
                            }
                        }
                        break;

                    case ShizaiKbnTypes.LD:
                        OutWorkNoteItem directDeliverysLD = null;
                        if (isSiyoudakaBarai)
                        {
                            // 品目コード、業者コード、向先をキーにして直送(LD)情報を取得
                            directDeliverysLD = SharedModel.Instance.DirectDeliverys
                                                    .FirstOrDefault(n => (n.ItemCode == HinmokuCD)
                                                    && (n.GyosyaCD == gyosyaCD)
                                                    && (n.Mukesaki == Mukesaki.LD.GetStringValue()));

                            if (null != directDeliverysLD)
                            {
                                nyukoValue += (int)directDeliverysLD.Amount;
                                haraiValue += (int)directDeliverysLD.Amount;
                            }
                        }
                        else
                        {
                            // 品目コード、向先をキーにして直送(LD)情報を取得
                            directDeliverysLD = SharedModel.Instance.DirectDeliverys
                                                    .FirstOrDefault(n => (n.ItemCode == HinmokuCD)
                                                    && (n.Mukesaki == Mukesaki.LD.GetStringValue()));

                            if (null != directDeliverysLD)
                            {
                                // 入庫払いは業者コードを指定しないため、
                                // 取得した直送(LD)情報のサマリを入庫量/出庫量に設定
                                var amount = (int)SharedModel.Instance.DirectDeliverys
                                    .Where(n => (n.ItemCode == HinmokuCD)
                                    && (n.Mukesaki == Mukesaki.LD.GetStringValue())).Select(x => x.Amount).Sum();
                                nyukoValue += amount;
                                haraiValue += amount;
                            }
                        }
                        break;

                    case ShizaiKbnTypes.CC:
                        OutWorkNoteItem directDeliverysCC = null;
                        if (isSiyoudakaBarai)
                        {
                            // 品目コード、業者コード、向先をキーにして直送(CC)情報を取得
                            directDeliverysCC = SharedModel.Instance.DirectDeliverys
                                                    .FirstOrDefault(n => (n.ItemCode == HinmokuCD)
                                                    && (n.GyosyaCD == gyosyaCD)
                                                    && (n.Mukesaki == Mukesaki.CC.GetStringValue()));

                            if (null != directDeliverysCC)
                            {
                                nyukoValue += (int)directDeliverysCC.Amount;
                                haraiValue += (int)directDeliverysCC.Amount;
                            }
                        }
                        else
                        {
                            // 品目コード、向先をキーにして直送(CC)情報を取得
                            directDeliverysCC = SharedModel.Instance.DirectDeliverys
                                                    .FirstOrDefault(n => (n.ItemCode == HinmokuCD)
                                                    && (n.Mukesaki == Mukesaki.CC.GetStringValue()));

                            if (null != directDeliverysCC)
                            {
                                // 入庫払いは業者コードを指定しないため、
                                // 取得した直送(CC)情報のサマリを入庫量/出庫量に設定
                                var amount = (int)SharedModel.Instance.DirectDeliverys
                                    .Where(n => (n.ItemCode == HinmokuCD)
                                    && (n.Mukesaki == Mukesaki.CC.GetStringValue())).Select(x => x.Amount).Sum();
                                nyukoValue += amount;
                                haraiValue += amount;
                            }
                        }
                        break;

                    case ShizaiKbnTypes.TD:
                        OutWorkNoteItem directDeliverysTD = null;
                        if (isSiyoudakaBarai)
                        {
                            // 品目コード、業者コード、向先をキーにして直送(TD)情報を取得
                            directDeliverysTD = SharedModel.Instance.DirectDeliverys
                                                    .FirstOrDefault(n => (n.ItemCode == HinmokuCD)
                                                    && (n.GyosyaCD == gyosyaCD)
                                                    && (n.Mukesaki == Mukesaki.TD.GetStringValue()));

                            if (null != directDeliverysTD)
                            {
                                nyukoValue += (int)directDeliverysTD.Amount;
                                haraiValue += (int)directDeliverysTD.Amount;
                            }
                        }
                        else
                        {
                            // 品目コード、向先をキーにして直送(TD)情報を取得
                            directDeliverysTD = SharedModel.Instance.DirectDeliverys
                                                    .FirstOrDefault(n => (n.ItemCode == HinmokuCD)
                                                    && (n.Mukesaki == Mukesaki.TD.GetStringValue()));

                            if (null != directDeliverysTD)
                            {
                                // 入庫払いは業者コードを指定しないため、
                                // 取得した直送(TD)情報のサマリを入庫量/出庫量に設定
                                var amount = (int)SharedModel.Instance.DirectDeliverys
                                    .Where(n => (n.ItemCode == HinmokuCD)
                                    && (n.Mukesaki == Mukesaki.TD.GetStringValue())).Select(x => x.Amount).Sum();
                                nyukoValue += amount;
                                haraiValue += amount;
                            }
                        }
                        break;

                    case ShizaiKbnTypes.Others:
                        OutWorkNoteItem directDeliverysETC = null;
                        if (isSiyoudakaBarai)
                        {
                            // 品目コード、業者コード、向先をキーにして直送(その他)情報を取得
                            directDeliverysETC = SharedModel.Instance.DirectDeliverys
                                                    .FirstOrDefault(n => (n.ItemCode == HinmokuCD)
                                                        && (n.GyosyaCD == gyosyaCD)
                                                        && (n.Mukesaki == Mukesaki.Others.GetStringValue()));

                            if (null != directDeliverysETC)
                            {
                                nyukoValue += (int)directDeliverysETC.Amount;
                                haraiValue += (int)directDeliverysETC.Amount;
                            }
                        }
                        else
                        {
                            // 品目コード、向先をキーにして直送(その他)情報を取得
                            directDeliverysETC = SharedModel.Instance.DirectDeliverys
                                                    .FirstOrDefault(n => (n.ItemCode == HinmokuCD)
                                                        && (n.Mukesaki == Mukesaki.Others.GetStringValue()));

                            if (null != directDeliverysETC)
                            {
                                // 入庫払いは業者コードを指定しないため、
                                // 取得した直送(その他)情報のサマリを入庫量/出庫量に設定
                                var amount = (int)SharedModel.Instance.DirectDeliverys
                                    .Where(n => (n.ItemCode == HinmokuCD)
                                    && (n.Mukesaki == Mukesaki.Others.GetStringValue())).Select(x => x.Amount).Sum();
                                nyukoValue += amount;
                                haraiValue += amount;
                            }
                        }
                        break;

                    default:
                        break;
                }

                // 返品情報を設定(無い場合は初期値のまま)
                int returnValue = 0;
                WorkNoteItem returns = null;
                if (isSiyoudakaBarai)
                {
                    returns = SharedModel.Instance.Returns.FirstOrDefault(e => (e.ItemCode == HinmokuCD) && (e.GyosyaCD == gyosyaCD));

                    if (null != returns)
                    {
                        returnValue = (int)returns.Amount;
                    }
                }
                else
                {
                    returns = SharedModel.Instance.Returns.FirstOrDefault(e => e.ItemCode == HinmokuCD);

                    if (null != returns)
                    {
                        // 入庫払いは業者コードを指定しないため、
                        // 取得した返品報のサマリを入庫量 /出庫量に設定
                        returnValue = (int)SharedModel.Instance.Receivings.Where(n => n.ItemCode == HinmokuCD).Select(x => x.Amount).Sum();
                    }
                }
                
                // 前月在庫情報
                Inventory item = null;
                if (isSiyoudakaBarai)
                {
                    item = SharedModel.Instance.LastInventories
                                .FirstOrDefault(e => (e.ItemCode == HinmokuCD) && (e.SupplierCode == gyosyaCD));
                }
                else
                {
                    item = SharedModel.Instance.LastInventories
                                .FirstOrDefault(e => e.ItemCode == HinmokuCD);
                }

                SizaiCategoryTiePlaceDefinition defItem 
                    = SizaiCategoryTiePlaceList.Find(x => (x.ShizaiKbnTypes == SelectShizaiKbn));

                string workkbn = this.WorkKbn.Equals(SizaiWorkCategory.TanaoroshiWork) ? 
                    SizaiWorkCategory.TanaoroshiWork.GetStringValue() : SizaiWorkCategory.KensinWork.GetStringValue();

                InputInventoryActualRecordViewModel addRecord = new InputInventoryActualRecordViewModel()
                {
                    SIZAI_KBN = ((int)SelectShizaiKbn).ToString(),
                    WORK_KBN = workkbn,
                    ITEM_ORDER = SelectIndex + 1,
                    HINMOKUCD = HinmokuCD,
                    GYOSYACD = gyosyaCD,
                    ShizaiKbn = SelectShizaiKbn,
                    WorkKbn = workkbn,
                    Order = SelectIndex,
                    HinmokuCd = HinmokuCD,
                    GyosyaCd = gyosyaCD,
                    HinmokuName = HinmokuName,
                    GyosyaName = gyosyaName,
                    TogetuValueToNumber = string.Empty,
                    NyukoValue = nyukoValue,
                    HaraiValue = haraiValue,
                    HenpinValue = returnValue,
                    SoukoValue = (item == null) ? 0 : (long)item.StockInWarehouse,
                    EfValue = (item == null) ? 0 : (long)item.StockEF,
                    LfValue = (item == null) ? 0 : (long)item.StockLF,
                    CcValue = (item == null) ? 0 : (long)item.StockCC,
                    OthersValue = (item == null) ? 0 : (long)item.StockOther,
                    MeterValue = (item == null) ? 0 : (long)item.StockMeter,
                    Yobi1Value = (item == null) ? 0 : (long)item.StockReserve1,
                    Yobi2Value = (item == null) ? 0 : (long)item.StockReserve2,
                    PlaceCode = (defItem != null) ? defItem.PlaceCode : string.Empty,
                    IsError = false
                };

                int addIndex = SelectIndex;
                switch (SelectShizaiKbn)
                {
                    case ShizaiKbnTypes.SK:
                        addIndex = addItem(ref skList, SelectIndex, addRecord);
                        SelectItem = SkList[addIndex];
                        sizaiCategory = SizaiCategory.SK;
                        break;
                    case ShizaiKbnTypes.EF:
                        addIndex = addItem(ref efList, SelectIndex, addRecord);
                        SelectItem = EfList[addIndex];
                        sizaiCategory = SizaiCategory.EF;
                        break;
                    case ShizaiKbnTypes.LF:
                        addIndex = addItem(ref lfList, SelectIndex, addRecord);
                        SelectItem = LfList[addIndex];
                        sizaiCategory = SizaiCategory.LF;
                        break;
                    case ShizaiKbnTypes.Building:
                        addIndex = addItem(ref buildingList, SelectIndex, addRecord);
                        SelectItem = BuildingList[addIndex];
                        sizaiCategory = SizaiCategory.Building;
                        break;
                    case ShizaiKbnTypes.LD:
                        addIndex = addItem(ref ldList, SelectIndex, addRecord);
                        SelectItem = LdList[addIndex];
                        sizaiCategory = SizaiCategory.LD;
                        break;
                    case ShizaiKbnTypes.TD:
                        addIndex = addItem(ref tdList, SelectIndex, addRecord);
                        SelectItem = TdList[addIndex];
                        sizaiCategory = SizaiCategory.TD;
                        break;
                    case ShizaiKbnTypes.CC:
                        addIndex = addItem(ref ccList, SelectIndex, addRecord);
                        SelectItem = CcList[addIndex];
                        sizaiCategory = SizaiCategory.CC;
                        break;
                    case ShizaiKbnTypes.Others:
                        addIndex = addItem(ref othersList, SelectIndex, addRecord);
                        SelectItem = OthersList[addIndex];
                        sizaiCategory = SizaiCategory.Others;
                        break;
                    default:
                        break;
                }

                targetHinmokuCode = addRecord.HinmokuCd;
                targetGyosyaCode = addRecord.GyosyaCd;
                targetHinmokuName = addRecord.HinmokuName;
                targetGyosyaName = addRecord.GyosyaName;
                SelectIndex = addIndex;
                SelectShizaiKbn = SelectShizaiKbn;

                clearEditItem();
                IsEditMode = false;
                IsShowEditForm = false;

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
                if (!hasError)
                {
                    // 棚卸ログの挿入
                    LogType logType = hasException ? LogType.Error : LogType.Normal;
                    await InsertTanaorosiLog(logType, SizaiWorkCategory.KensinWork, 
                                             LogOperationType.InputInventoryActual, 
                                             LogOperationContent.Add, null, errorContent, errorCode);
                    // 棚卸詳細ログの挿入
                    await InsertTanaorosiDetailLog(logType, SizaiWorkCategory.KensinWork,
                    LogOperationType.InputInventoryActual, LogOperationContent.Add, sizaiCategory,
                    targetHinmokuCode, targetGyosyaCode, targetHinmokuName, targetGyosyaName,
                    null, null, null, errorContent);
                }
            }

        });

        /// <summary>
        /// キャンセルボタン押下
        /// </summary>
        public ICommand FireCancelEditMode => new RelayCommand(() =>
        {
            IsEditMode = false;
            IsShowEditForm = false;
            clearEditItem();
        });

        #endregion

        #region 抽象メソッド
        /// <summary>
        /// 棚卸ワークデータの取得
        /// </summary>
        /// <returns>棚卸ワークデータ</returns>
        protected override async Task<IEnumerable<InventoryWork>> GetInventoryWork()
        {
            try
            {
                return await DataService.GetInventoriesKensinWorkAsync
                    (SizaiWorkCategory.KensinWork.GetStringValue());
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 棚卸ワークデータの取得(棚卸マスタのデータをもとにする)
        /// </summary>
        /// <returns>棚卸ワークデータ</returns>
        protected override async Task<IEnumerable<InventoryWork>> GetInventoryWorkFromMst()
        {
            try
            {
                return await DataService.GetInventoriesWorkFromMsterAsync
                    (SizaiWorkCategory.KensinWork.GetStringValue());
            }

            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// 単価入力チェック
        /// </summary>
        protected override async Task<bool> CheckTankaInput()
        {
            return await Task.FromResult(false);
        }

        #endregion

        #region メソッド

        /// <summary>
        /// データ保存
        /// </summary>
        private async Task<bool> SaveInventoriesWorkAsync()
        {
            try
            {
                //削除データがあれば削除する
                if (DeleteSkList.Count() > 0)
                    await DataService.DeletetInventoriesWorkData(DeleteSkList);
                if (DeleteEfList.Count() > 0)
                    await DataService.DeletetInventoriesWorkData(DeleteEfList);
                if (DeleteLfList.Count() > 0)
                    await DataService.DeletetInventoriesWorkData(DeleteLfList);
                if (DeleteBuildingList.Count() > 0)
                    await DataService.DeletetInventoriesWorkData(DeleteBuildingList);
                if (DeleteLdList.Count() > 0)
                    await DataService.DeletetInventoriesWorkData(DeleteLdList);
                if (DeleteTdList.Count() > 0)
                    await DataService.DeletetInventoriesWorkData(DeleteTdList);
                if (DeleteCcList.Count() > 0)
                    await DataService.DeletetInventoriesWorkData(DeleteCcList);
                if (DeleteOthersList.Count() > 0)
                    await DataService.DeletetInventoriesWorkData(DeleteOthersList);

                InventoryWork.Clear();

                addInventoriesWorkItem(SkList);
                addInventoriesWorkItem(EfList);
                addInventoriesWorkItem(LfList);
                addInventoriesWorkItem(BuildingList);
                addInventoriesWorkItem(LdList);
                addInventoriesWorkItem(TdList);
                addInventoriesWorkItem(CcList);
                addInventoriesWorkItem(OthersList);

                await DataService.UpdateSizaiKensinData(InventoryWork);

                return true;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 確定
        /// </summary>
        internal override async Task<bool> CommitAsync()
        {
            bool hasException = false;
            bool hasError = false;
            string errorContent = null;
            string errorCode = null;
            string sql = string.Empty;
            string sqlParam = string.Empty;
            string timestamp = string.Empty;

            try
            {
                BusyStatus.IsBusy = true;

                if (!CheckValue())
                {
                    hasError = true;
                    return false;
                }

                //削除データがあれば削除する
                if(DeleteSkList.Count() > 0)
                    await DataService.DeletetInventoriesWorkData(DeleteSkList);
                if(DeleteEfList.Count() > 0)
                    await DataService.DeletetInventoriesWorkData(DeleteEfList);
                if(DeleteLfList.Count() > 0)
                    await DataService.DeletetInventoriesWorkData(DeleteLfList);
                if(DeleteBuildingList.Count() > 0)
                    await DataService.DeletetInventoriesWorkData(DeleteBuildingList);
                if(DeleteLdList.Count() > 0)
                    await DataService.DeletetInventoriesWorkData(DeleteLdList);
                if(DeleteTdList.Count() > 0)
                    await DataService.DeletetInventoriesWorkData(DeleteTdList);
                if(DeleteCcList.Count() > 0)
                    await DataService.DeletetInventoriesWorkData(DeleteCcList);
                if(DeleteOthersList.Count() > 0)
                    await DataService.DeletetInventoriesWorkData(DeleteOthersList);

                InventoryWork.Clear();

                addInventoriesWorkItem(SkList);
                addInventoriesWorkItem(EfList);
                addInventoriesWorkItem(LfList);
                addInventoriesWorkItem(BuildingList);
                addInventoriesWorkItem(LdList);
                addInventoriesWorkItem(TdList);
                addInventoriesWorkItem(CcList);
                addInventoriesWorkItem(OthersList);

                if (InventoryWork.Count == 0)
                {
                    MessageManager.ShowExclamation(SystemID.CKSI1010, "NotInputKensinData");
                    return false;
                }

                await DataService.UpdateSizaiKensinData(InventoryWork);

                // 操作状況の更新
                await UpdateOperationCondition(OperationCondition.Fix);
                IsFixed = true;

                return true;
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
                await InsertTanaorosiLog(logType, SizaiWorkCategory.KensinWork, 
                                         LogOperationType.InputInventoryActual, 
                                         LogOperationContent.Fix, null, errorContent, errorCode);
                if (!hasException)
                { 
                    if (!hasError)
                    {
                        MessageManager.ShowInformation(SystemID.CKSI1010, "CommitCompleted");
                    }
                }
                else
                {
                    MessageManager.ShowError(SystemID.CKSI1010, "FatalError");
                }

                BusyStatus.IsBusy = false;
            }

        }

        /// <summary>
        /// 修正
        /// </summary>
        internal override async Task<bool> ModifyAsync()
        {
            string errorContent = null;
            string errorCode = null;
            string sql = null;
            string sqlParam = null;
            string timestamp = null;
            bool hasException = false;

            try
            {
                BusyStatus.IsBusy = true;

                // 共有データで保持している印刷対象外受払い情報の初期化
                CommonUtility.InitializeExcludeInputPaymentInfo();

                // 操作状況の更新
                await UpdateOperationCondition(OperationCondition.Modify);
                IsFixed = false;

                return true;
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
                await InsertTanaorosiLog(logType, SizaiWorkCategory.KensinWork, 
                                         LogOperationType.InputInventoryActual, 
                                         LogOperationContent.Modify, null, errorContent, errorCode);
                BusyStatus.IsBusy = false;
            }

        }

        /// <summary>
        /// 次へ
        /// </summary>
        internal override async Task<bool> GoNextAsync()
        {
            bool hasException = false;
            string errorContent = null;
            string errorCode = null;
            string sql = null;
            string sqlParam = null;
            string timestamp = null;

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
                    return true;
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
                // 棚卸ログの挿入
                LogType logType = hasException ? LogType.Error : LogType.Normal;
                await InsertTanaorosiLog(logType, SizaiWorkCategory.KensinWork, 
                                         LogOperationType.InputInventoryActual, 
                                         LogOperationContent.Next, null, errorContent, errorCode);
                BusyStatus.IsBusy = false;
            }
        }

        /// <summary>
        /// 前へ
        /// </summary>
        internal override async Task<bool> BackPreviousAsync()
        {
            return await base.BackPreviousAsync();
        }

        /// <summary>
        /// ぱんくずリストの選択
        /// </summary>
        internal override async Task<bool> SelecOperationAsync()
        {
            return await base.SelecOperationAsync();
        }

        /// <summary>
        /// 編集項目をクリアする
        /// </summary>
        private void clearEditItem()
        {
            HinmokuCD = string.Empty;
            HinmokuName = string.Empty;
            GyosyaCD = string.Empty;
            GyosyaName = string.Empty;
        }

        /// <summary>
        /// インデックスを指定して項目を移動する
        /// </summary>
        /// <param name="list">対象のリスト</param>
        /// <param name="fromIndex">移動元の項目Index</param>
        /// <param name="toIndex">移動先の項目Index</param>
        private int moveItem(ref ObservableCollection<InputInventoryActualRecordViewModel> list, 
            int fromIndex, int toIndex)
        {
            try
            {
                if (0 > fromIndex || list.Count <= fromIndex)
                {
                    return 0;
                }
                if (0 > toIndex)
                {
                    toIndex = 0;
                }
                if (list.Count <= toIndex)
                {
                    toIndex = list.Count - 1;
                }

                if (fromIndex > toIndex)
                {
                    // 上方向へ移動
                    InputInventoryActualRecordViewModel item = list[fromIndex];
                    list.RemoveAt(fromIndex);
                    list.Insert(toIndex, item);
                }
                else if (fromIndex < toIndex)
                {
                    // 下方向へ移動
                    InputInventoryActualRecordViewModel item = list[fromIndex];
                    if (toIndex < list.Count - 1)
                    {
                        list.Insert(toIndex + 1, item);
                    }
                    else
                    {
                        list.Add(item);

                    }
                    list.RemoveAt(fromIndex);
                }

                return toIndex;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// インデックスを指定して項目を削除する
        /// </summary>
        /// <param name="list">対象リスト</param>
        /// <param name="index">削除する項目Index</param>
        /// <returns>削除後の選択項目Index</returns>
        private int removeItem(ref ObservableCollection<InputInventoryActualRecordViewModel> list, int index)
        {
            int nextIndex = -1;

            try
            {
                if (0 > index || list.Count <= index)
                {
                    return -1;
                }
                nextIndex = ((list.Count - 1) == index) ? index - 1 : index;

                InputInventoryActualRecordViewModel deleteItem = new InputInventoryActualRecordViewModel();
                deleteItem.SIZAI_KBN = list.ElementAt(index).SIZAI_KBN;
                deleteItem.WorkKbn = list.ElementAt(index).WORK_KBN;
                deleteItem.HINMOKUCD = list.ElementAt(index).HINMOKUCD;
                deleteItem.GYOSYACD = list.ElementAt(index).GYOSYACD;

                switch (int.Parse(deleteItem.SIZAI_KBN.Trim(' ')))
                {
                    case (int)ShizaiKbnTypes.SK:
                        if (!DeleteSkList.Contains(deleteItem)) DeleteSkList.Add(deleteItem);
                        break;
                    case (int)ShizaiKbnTypes.EF:
                        if (!DeleteEfList.Contains(deleteItem)) DeleteEfList.Add(deleteItem);
                        break;
                    case (int)ShizaiKbnTypes.LF:
                        if (!DeleteLfList.Contains(deleteItem)) DeleteLfList.Add(deleteItem);
                        break;
                    case (int)ShizaiKbnTypes.Building:
                        if (!DeleteBuildingList.Contains(deleteItem)) DeleteBuildingList.Add(deleteItem);
                        break;
                    case (int)ShizaiKbnTypes.LD:
                        if (!DeleteLdList.Contains(deleteItem)) DeleteLdList.Add(deleteItem);
                        break;
                    case (int)ShizaiKbnTypes.TD:
                        if (!DeleteTdList.Contains(deleteItem)) DeleteTdList.Add(deleteItem);
                        break;
                    case (int)ShizaiKbnTypes.CC:
                        if (!DeleteCcList.Contains(deleteItem)) DeleteCcList.Add(deleteItem);
                        break;
                    case (int)ShizaiKbnTypes.Others:
                        if (!DeleteOthersList.Contains(deleteItem)) DeleteOthersList.Add(deleteItem);
                        break;
                    default:
                        break;
                }

                list.RemoveAt(index);

                return nextIndex;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// データ存在チェック
        /// </summary>
        /// <param name="list">対象リスト</param>
        /// <param name="hinmokuCd">品目CD</param>
        /// <param name="gyosyaCd">業者CD</param>
        /// <returns>結果</returns>
        private bool isExist(ref ObservableCollection<InputInventoryActualRecordViewModel> list, 
            string hinmokuCd, string gyosyaCd)
        {
            bool isExist = false;
            if(list.Count() > 0)
            {
                List<InputInventoryActualRecordViewModel> existList 
                    = list.Where(item => (item.HinmokuCd?.Trim() == hinmokuCD?.Trim() 
                                && item.GyosyaCd?.Trim() == gyosyaCD?.Trim())).ToList();
                if (existList.Count > 0)
                {
                    isExist = true;
                }
            }

            return isExist;
        }

        /// <summary>
        /// インデックスを指定して項目を追加する
        /// </summary>
        /// <param name="list">対象リスト</param>
        /// <param name="index">追加するIndex位置</param>
        /// <param name="item">追加する項目</param>
        /// <returns>追加した項目のIndex</returns>
        private int addItem(ref ObservableCollection<InputInventoryActualRecordViewModel> list, 
            int index, InputInventoryActualRecordViewModel item)
        {
            try
            {
                if (0 > index || list.Count <= index)
                {
                    list.Add(item);
                    return list.Count - 1;
                }
                else
                {
                    list.Insert(index + 1, item);
                    return index + 1;
                }
            }

            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// インデックスを指定して項目の値を変更する
        /// </summary>
        /// <param name="list">対象リスト</param>
        /// <param name="index">変更する項目Index</param>
        /// <param name="hinmokuCd">品目コード</param>
        /// <param name="gyosyaCd">業者コード</param>
        /// <param name="hinmokuName">品目名</param>
        /// <param name="gyosyaName">業者名</param>
        private void modifyItem(ref ObservableCollection<InputInventoryActualRecordViewModel> list, 
            int index, string hinmokuCd, string gyosyaCd, string hinmokuName, string gyosyaName)
        {
            try
            {
                if (0 > index || list.Count <= index)
                {
                    return;
                }

                list[index].HinmokuCd = hinmokuCd;
                list[index].GyosyaCd = gyosyaCd;
                list[index].HinmokuName = hinmokuName;
                list[index].GyosyaName = gyosyaName;

                list[index].HINMOKUCD = hinmokuCd;
                list[index].GYOSYACD = gyosyaCd;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 画面で変更した項目をDB更新用の一時データに追加する
        /// </summary>
        /// <param name="viewList">追加する画面のリスト</param>
        private void addInventoriesWorkItem(ObservableCollection<InputInventoryActualRecordViewModel> viewList)
        {
            int skItemOrder = 0;
            int efItemOrder = 0;
            int lfItemOrder = 0;
            int buildingItemOrder = 0;
            int ldItemOrder = 0;
            int tdItemOrder = 0;
            int ccItemOrder = 0;
            int otherItemOrder = 0;
            int order = 0;

            try
            {
                foreach (InputInventoryActualRecordViewModel item in viewList)
                {
                    switch (int.Parse(item.SIZAI_KBN.Trim(' ')))
                    {
                        case (int)ShizaiKbnTypes.SK:
                            skItemOrder = ++skItemOrder;
                            order = skItemOrder;
                            break;
                        case (int)ShizaiKbnTypes.EF:
                            efItemOrder = ++efItemOrder;
                            order = efItemOrder;
                            break;
                        case (int)ShizaiKbnTypes.LF:
                            lfItemOrder = ++lfItemOrder;
                            order = lfItemOrder;
                            break;
                        case (int)ShizaiKbnTypes.Building:
                            buildingItemOrder = ++buildingItemOrder;
                            order = buildingItemOrder;
                            break;
                        case (int)ShizaiKbnTypes.LD:
                            ldItemOrder = ++ldItemOrder;
                            order = ldItemOrder;
                            break;
                        case (int)ShizaiKbnTypes.TD:
                            tdItemOrder = ++tdItemOrder;
                            order = tdItemOrder;
                            break;
                        case (int)ShizaiKbnTypes.CC:
                            ccItemOrder = ++ccItemOrder;
                            order = ccItemOrder;
                            break;
                        case (int)ShizaiKbnTypes.Others:
                            otherItemOrder = ++otherItemOrder;
                            order = otherItemOrder;
                            break;
                        default:
                            break;
                    }

                    InventoryWork.Add(new InventoryWork()
                    {
                        ShizaiKbn = item.SIZAI_KBN,
                        WorkKbn = item.WORK_KBN,
                        ItemOrder = order,
                        ItemCode = item.HINMOKUCD,
                        SupplierCode = item.GYOSYACD,
                        ItemName = item.HinmokuName,
                        SupplierName = item.GyosyaName,
                        CurrentValue = item.TogetuValueToNumber,
                        CurrentExpValue = item.TogetuYosouValue,
                        InputValue = item.NyukoValue,
                        OutputValue = item.HaraiValue,
                        ReturnValue = item.HenpinValue,
                        StockInWarehouse = item.SoukoValue,
                        StockEF = item.EfValue,
                        StockLF = item.LfValue,
                        StockCC = item.CcValue,
                        StockOthers = item.OthersValue,
                        StockMeter = item.MeterValue,
                        StockYobi1 = item.Yobi1Value,
                        StockYobi2 = item.Yobi2Value
                    });
                }
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
using GalaSoft.MvvmLight;
using System;
//*************************************************************************************
//
//   棚卸操作情報
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1010.Common
{
    /// <summary>
    /// 棚卸操作情報
    /// </summary>
    public class Operation : ViewModelBase
    {
        #region フィールド

        /// <summary>
        /// インデックス
        /// </summary>
        private int index = 0;

        /// <summary>
        /// 作業区分
        /// </summary>
        private string workType = string.Empty;

        /// <summary>
        /// 操作コード
        /// </summary>
        private int code = 0;

        /// <summary>
        /// タイトル
        /// </summary>
        private string title = string.Empty;

        /// <summary>
        /// 説明
        /// </summary>
        private string note = string.Empty;

        /// <summary>
        /// 選択中か
        /// </summary>
        private bool isSelected = false;

        /// <summary>
        /// 最初の操作か
        /// </summary>
        private bool isStartOperation = false;

        /// <summary>
        /// 最後の操作か
        /// </summary>
        private bool isLastOperation = false;

        /// <summary>
        /// 中間の操作か
        /// </summary>
        private bool isMiddleOperation = false;

        /// <summary>
        /// 印刷ボタンを表示する対象の操作か
        /// </summary>
        private bool isPrintVisibleOperation = false;

        /// <summary>
        /// 確定ボタンを表示する対象の操作か
        /// </summary>
        private bool isFixVisibleOperation = false;

        /// <summary>
        /// 更新ボタンを表示する対象の操作か
        /// </summary>
        private bool isUpdateVisibleOperation = false;

        /// <summary>
        /// 修正ボタンを表示する対象の操作か
        /// </summary>
        private bool isModifyVisibleOperation = false;

        /// <summary>
        /// 修正ボタンを表示する対象の操作か
        /// </summary>
        private bool isConditionExcludedOperation = false;

        /// <summary>
        /// 確定状態か
        /// </summary>
        private bool isFixed = false;

        /// <summary>
        /// 修正状態か
        /// </summary>
        private bool isModified = false;

        /// <summary>
        /// 操作可能か
        /// </summary>
        private bool canOperate = true;

        #endregion

        #region プロパティ

        /// <summary>
        /// インデックス
        /// </summary>
        public int Index
        {
            get { return index; }
            set { Set(ref index, value); }
        }

        /// <summary>
        /// 作業区分
        /// </summary>
        public string WorkType
        {
            get { return workType; }
            set { Set(ref workType, value); }

        }

        /// <summary>
        /// 操作コード
        /// </summary>
        public int Code
        {
            get { return code; }
            set { Set(ref code, value); }
        }

        /// <summary>
        /// タイトル
        /// </summary>
        public string Title
        {
            get { return title; }
            set { Set(ref title, value); }
        }

        /// <summary>
        /// ノート
        /// </summary>
        public string Note
        {
            get { return note; }
            set { Set(ref note, value); }
        }

        /// <summary>
        /// ユーザーコントロール
        /// </summary>
        internal Type UserControl { get; set; }

        /// <summary>
        /// 選択中か
        /// </summary>
        public bool IsSelected
        {
            get { return isSelected; }
            set { Set(ref isSelected, value); }
        }

        /// <summary>
        /// 最初の操作か
        /// </summary>
        public bool IsStartOperation
        {
            get { return isStartOperation; }
            set { Set(ref isStartOperation, value); }
        }

        /// <summary>
        /// 最後の操作か
        /// </summary>
        public bool IsLastOperation
        {
            get { return isLastOperation; }
            set { Set(ref isLastOperation, value); }
        }

        /// <summary>
        /// 中間の操作か
        /// </summary>
        public bool IsMiddleOperation
        {
            get { return isMiddleOperation; }
            set { Set(ref isMiddleOperation, value); }
        }

        /// <summary>
        /// 印刷ボタンを表示する対象の操作か
        /// </summary>
        public bool IsPrintVisibleOperation
        {
            get { return isPrintVisibleOperation; }
            set { Set(ref isPrintVisibleOperation, value); }
        }

        /// <summary>
        /// 確定ボタンを表示する対象の操作か
        /// </summary>
        public bool IsFixVisibleOperation
        {
            get { return isFixVisibleOperation; }
            set { Set(ref isFixVisibleOperation, value); }
        }

        /// <summary>
        /// 更新ボタンを表示する対象の操作か
        /// </summary>
        public bool IsUpdateVisibleOperation
        {
            get { return isUpdateVisibleOperation; }
            set { Set(ref isUpdateVisibleOperation, value); }
        }

        /// <summary>
        /// 修正ボタンを表示する対象の操作か
        /// </summary>
        public bool IsModifyVisibleOperation
        {
            get { return isModifyVisibleOperation; }
            set { Set(ref isModifyVisibleOperation, value); }
        }

        /// <summary>
        /// 作業状況対象外のの操作か
        /// </summary>
        public bool IsConditionExcludedOperation
        {
            get { return isConditionExcludedOperation; }
            set { Set(ref isConditionExcludedOperation, value); }
        }

        /// <summary>
        /// 確定状態か
        /// </summary>
        public bool IsFixed
        {
            get { return isFixed; }
            set { Set(ref isFixed, value); }
        }

        /// <summary>
        /// 修正状態か
        /// </summary>
        public bool IsModified
        {
            get { return isModified; }
            set { Set(ref isModified, value); }
        }

        /// <summary>
        /// 操作可能か
        /// </summary>
        public bool CanOperate
        {
            get { return canOperate; }
            set { Set(ref canOperate, value); }
        }

        #endregion
    }
}

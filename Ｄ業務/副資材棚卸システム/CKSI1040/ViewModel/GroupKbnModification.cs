﻿using ViewModel;
//*************************************************************************************
//
//   棚卸グループ区分の変更状態管理クラス
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   17.02.01              DSK       　 新規作成
//
//*************************************************************************************
namespace CKSI1040.ViewModel
{
    /// <summary>
    /// 棚卸グループ区分の変更状態管理
    /// </summary>
    public class GroupKbnModification : ViewModelBase
    {
        #region フィールド

        /// <summary>
        /// 変更されているか
        /// </summary>
        private bool isDirty;

        #endregion

        #region プロパティ

        /// <summary>
        /// 変更されているか
        /// </summary>
        public bool IsDirty
        {
            get
            {
                return isDirty;
            }
            set
            {
                if (isDirty != value)
                {
                    isDirty = value;
                    OnPropertyChanged("IsDirty");
                }
            }
        }

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GroupKbnModification()
        {
        }

        #endregion
    }
}

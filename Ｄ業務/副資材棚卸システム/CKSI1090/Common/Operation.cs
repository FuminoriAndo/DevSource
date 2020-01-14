using GalaSoft.MvvmLight;
using System;
//*************************************************************************************
//
//   棚卸操作情報
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   17.04.28              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1090.Common
{
    /// <summary>
    /// 棚卸操作情報
    /// </summary>
    public class Operation : ViewModelBase
    {
        #region プロパティ

        /// <summary>
        /// ユーザーコントロール
        /// </summary>
        internal Type UserControl { get; set; }

        #endregion
    }
}

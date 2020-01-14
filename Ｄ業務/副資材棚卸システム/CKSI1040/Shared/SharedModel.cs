using Core;
using System.Collections.Generic;
//*************************************************************************************
//
//   共有データ
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************

namespace Shared
{
    /// <summary>
    /// 共有データ
    /// </summary>
    public class SharedModel : Singleton<SharedModel>
    {
        #region プロパティ

        /// <summary>
        /// システム分類
        /// </summary>
        public IDictionary<string, string> SystemCategory { get; set; }

        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SharedModel()
        {
        }
    }
}

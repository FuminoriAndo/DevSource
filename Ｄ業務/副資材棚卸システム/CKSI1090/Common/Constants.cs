//*************************************************************************************
//
//   定数クラス
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
    /// 定数クラス
    /// </summary>
    public static class Constants
    {
        #region 定数

        /// <summary>
        /// SQL結果(OK)
        /// </summary>
        public const int SQL_RESULT_OK = 0;

        /// <summary>
        /// SQL結果(キー重複)
        /// </summary>
        public const int SQL_RESULT_ALREADY_EXISTS = 1;

        /// <summary>
        /// SQL結果(NG)
        /// </summary>
        public const int SQL_RESULT_NG = -1;

        /// <summary>
        /// 作業日
        /// </summary>
        public const string OperationDate_StringDefine = "作業日";

        /// <summary>
        /// 作業誌区分
        /// </summary>
        public const string WorkNoteKbn_StringDefine = "作業誌区分";

        /// <summary>
        /// 品目コード
        /// </summary>
        public const string HinmokuCode_StringDefine = "品目CD";

        /// <summary>
        /// 業者コード
        /// </summary>
        public const string GyosyaCode_StringDefine = "業者CD";

        /// <summary>
        /// 向先
        /// </summary>
        public const string Mukesaki_StringDefine = "向先";

        #endregion

        #region 列挙型

        /// <summary>
        /// 作業日の検索種類
        /// </summary>
        public enum OperationDateSearchTypes
        {
            /// <summary>
            /// 最小
            /// </summary>
            Min,
            /// <summary>
            /// 最大
            /// </summary>
            Max,
            /// <summary>
            /// 最小～最大
            /// </summary>
            MinMax,
        }

        /// <summary>
        /// 品目コードの検索種類
        /// </summary>
        public enum HinmokuCodeSearchTypes
        {
            /// <summary>
            /// 最小
            /// </summary>
            Min,
            /// <summary>
            /// 最大
            /// </summary>
            Max,
            /// <summary>
            /// 最小～最大
            /// </summary>
            MinMax,
        }

        /// <summary>
        /// 業者コードの検索種類
        /// </summary>
        public enum GyosyaCodeSearchTypes
        {
            /// <summary>
            /// 最小
            /// </summary>
            Min,
            /// <summary>
            /// 最大
            /// </summary>
            Max,
            /// <summary>
            /// 最小～最大
            /// </summary>
            MinMax,
            /// <summary>
            /// 空白
            /// </summary>
            Blank,
        }

        /// <summary>
        /// システム分類
        /// </summary>
        public enum SystemCategory
        {
            Sizai = 1, // 資材
            Buhin = 2  // 部品
        }

        #endregion
    }
}
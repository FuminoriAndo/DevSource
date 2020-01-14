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

using System;

namespace Project1
{
    /// <summary>
    /// 定数クラス
    /// </summary>
    public static class SizaiConstants
    {
        #region 定数

        /// <summary>
        /// 資材区分(SK)
        /// </summary>
        public const string SizaiCategory_SK_StringDefine = "1";
        /// <summary>
        /// 資材区分(EF)
        /// </summary>
        public const string SizaiCategory_EF_StringDefine = "2";
        /// <summary>
        /// 資材区分(LF)
        /// </summary>
        public const string SizaiCategory_LF_StringDefine = "3";
        /// <summary>
        /// 資材区分(築炉)
        /// </summary>
        public const string SizaiCategory_Chikuro_StringDefine = "4";
        /// <summary>
        /// 資材区分(LD)
        /// </summary>
        public const string SizaiCategory_LD_StringDefine = "5";
        /// <summary>
        /// 資材区分(TD)
        /// </summary>
        public const string SizaiCategory_TD_StringDefine = "6";
        /// <summary>
        /// 資材区分(CC)
        /// </summary>
        public const string SizaiCategory_CC_StringDefine = "7";
        /// <summary>
        /// 資材区分(その他)
        /// </summary>
        public const string SizaiCategory_Others_StringDefine = "8";
        /// <summary>
        /// 資材区分(メータ)
        /// </summary>
        public const string SizaiCategory_Meter_StringDefine = "9";
        /// <summary>
        /// 資材区分(予備1)
        /// </summary>
        public const string SizaiCategory_Yobi1 = "10";
        /// <summary>
        /// 資材区分(予備2)
        /// </summary>
        public const string SizaiCategory_Yobi2 = "11";

        /// <summary>
        /// 向先(EF)
        /// </summary>
        public const string Mukesaki_EF_StringDefine = "1";
        /// <summary>
        /// 向先(LF)
        /// </summary>
        public const string Mukesaki_LF_StringDefine = "2";
        /// <summary>
        /// 向先(築炉)
        /// </summary>
        public const string Mukesaki_Chikuro_StringDefine = "2";
        /// <summary>
        /// 向先(LD)
        /// </summary>
        public const string Mukesaki_LD_StringDefine = "2";
        /// <summary>
        /// 向先(TD)
        /// </summary>
        public const string Mukesaki_TD_StringDefine = "7";
        /// <summary>
        /// 向先(CC)
        /// </summary>
        public const string Mukesaki_CC_StringDefine = "3";
        /// <summary>
        /// 向先(その他)
        /// </summary>
        public const string Mukesaki_Others_StringDefine = "4";

        #endregion

        #region 列挙型

        /// <summary>
        /// 資材区分
        /// </summary>
        public enum SizaiCategory
        {
            /// <summary>
            /// 未設定
            /// </summary>
            None = 0,
            /// <summary>
            /// SK
            /// </summary>
            SK = 1,
            /// <summary>
            /// EF
            /// </summary>
            EF = 2,
            /// <summary>
            /// LF
            /// </summary>
            LF = 3,
            /// <summary>
            /// 築炉
            /// </summary>
            Chikuro = 4,
            /// <summary>
            /// LD
            /// </summary>
            LD = 5,
            /// <summary>
            /// TD
            /// </summary>
            TD = 6,
            /// <summary>
            /// CC
            /// </summary>
            CC = 7,
            /// <summary>
            /// その他
            /// </summary>
            Others = 8,
            /// <summary>
            /// メータ
            /// </summary>
            Meter = 9,
            /// <summary>
            /// 予備1
            /// </summary>
            Yobi1 = 10,
            /// <summary>
            /// 予備2
            /// </summary>
            Yobi2 = 11
        }

        /// <summary>
        /// 向先
        /// </summary>
        public enum Mukesaki
        {
            /// <summary>
            /// EF
            /// </summary>
            EF = 1,
            /// <summary>
            /// LF
            /// </summary>
            LF = 2,
            /// <summary>
            /// 築炉
            /// </summary>
            Chikuro = 2,
            /// <summary>
            /// LD
            /// </summary>
            LD = 2,
            /// <summary>
            /// CC
            /// </summary>
            CC = 3,
            /// <summary>
            /// TD
            /// </summary>
            TD = 7,
            /// <summary>
            /// その他
            /// </summary>
            Others = 4
        }

        #endregion
    }
}
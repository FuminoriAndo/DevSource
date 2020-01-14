//*************************************************************************************
//
//   資材区分
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1010.Types
{
    #region 列挙型

    /// <summary>
    /// 資材区分
    /// </summary>
    public enum ShizaiKbnTypes
    {
        /// <summary>
        /// 未設定
        /// </summary>
        None = -1,
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
        Building = 4,
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
        /// 他
        /// </summary>
        Others = 8,
        /// <summary>
        /// メーター
        /// /// </summary>
        Meter = 9,
        /// <summary>
        /// 予備１
        /// /// </summary>
        Yobi1 = 10,
        /// <summary>
        /// 予備２
        /// /// </summary>
        Yobi2 = 11
    }

    #endregion
}

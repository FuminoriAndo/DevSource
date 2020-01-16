//*************************************************************************************
//
//   SSYM050Bのモデル
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************
using Model.Base;

namespace Model
{
    /// <summary>
    /// SSYM050Bのモデル
    /// </summary>
    public class SSYM050BModel : ModelBase
    {
        #region プロパティ

        /// <summary>
        /// 年月日
        /// </summary>
        public string YYYYMMDD { get; set; }

        /// <summary>
        /// エンドシャー屑
        /// </summary>
        public string EndShearWaste { get; set; }

        /// <summary>
        /// プレーナ屑
        /// </summary>
        public string PlanerWaste { get; set; }

        /// <summary>
        /// トリーマ屑
        /// </summary>
        public string TrimmerWaste { get; set; }

        /// <summary>
        /// レーザー屑
        /// </summary>
        public string LaserWaste { get; set; }

        /// <summary>
        /// その他屑
        /// </summary>
        public string OtherWaste { get; set; }

        /// <summary>
        /// 合計
        /// </summary>
        public string Total { get; set; }

        #endregion
    }
}

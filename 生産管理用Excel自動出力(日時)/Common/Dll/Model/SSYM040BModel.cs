//*************************************************************************************
//
//   SSYM040Bのモデル
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
    /// SSYM040Bのモデル
    /// </summary>
    public class SSYM040BModel : ModelBase
    {
        #region プロパティ

        /// <summary>
        /// 年月日
        /// </summary>
        public string YYYYMMDD { get; set; }

        /// <summary>
        /// 中板屑
        /// </summary>
        public string MiddlePlateWaste { get; set; }

        /// <summary>
        /// 日清リターン屑
        /// </summary>
        public string NissinReturnWaste { get; set; }

        /// <summary>
        /// トリーマ屑
        /// </summary>
        public string TrimmerWaste { get; set; }

        /// <summary>
        /// 厚板ライン屑
        /// </summary>
        public string AtuItaLineWaste { get; set; }

        /// <summary>
        /// プレーナ屑
        /// </summary>
        public string PlanerWaste { get; set; }

        /// <summary>
        /// レーザー屑
        /// </summary>
        public string LaserWaste { get; set; }

        /// <summary>
        /// プレーナ知多屑
        /// </summary>
        public string PlanerChitaWaste { get; set; }

        /// <summary>
        /// ミスロール屑
        /// </summary>
        public string MissRollWaste { get; set; }

        /// <summary>
        /// コラム返品屑
        /// </summary>
        public string ColumuReturnWaste { get; set; }

        /// <summary>
        /// 合計
        /// </summary>
        public string Total { get; set; }

        #endregion
    }
}

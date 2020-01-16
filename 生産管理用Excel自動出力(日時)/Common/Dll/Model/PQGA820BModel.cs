//*************************************************************************************
//
//   PQGA820Bのモデル
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
    /// PQGA820Bのモデル
    /// </summary>
    public class PQGA820BModel : ModelBase
    {
        #region プロパティ

        /// <summary>
        /// 出荷日
        /// </summary>
        public string ShipmentYYMMDD { get; set; }

        /// <summary>
        /// ZK081本数
        /// </summary>
        public string ZK081Honsuu { get; set; }

        /// <summary>
        /// ZK081重量
        /// </summary>
        public string ZK081Weight { get; set; }

        /// <summary>
        /// ZK085本数
        /// </summary>
        public string ZK085Honsuu { get; set; }

        /// <summary>
        /// ZK085重量
        /// </summary>
        public string ZK085Weight { get; set; }

        /// <summary>
        /// ZK121本数
        /// </summary>
        public string ZK121Honsuu { get; set; }

        /// <summary>
        /// ZK121重量
        /// </summary>
        public string ZK121Weight { get; set; }

        /// <summary>
        /// ZK172本数
        /// </summary>
        public string ZK172Honsuu { get; set; }

        /// <summary>
        /// ZK172重量
        /// </summary>
        public string ZK172Weight { get; set; }

        /// <summary>
        /// ＳＰＨＣ本数
        /// </summary>
        public string SPHCHonsuu { get; set; }

        /// <summary>
        /// ＳＰＨＣ重量
        /// </summary>
        public string SPHCWeight { get; set; }

        /// <summary>
        /// ＳＳ400本数
        /// </summary>
        public string SS400Honsuu { get; set; }

        /// <summary>
        /// ＳＳ400重量
        /// </summary>
        public string SS400Weight { get; set; }

        /// <summary>
        /// その他本数
        /// </summary>
        public string OtherHonsuu { get; set; }

        /// <summary>
        /// その他重量
        /// </summary>
        public string OtherWeight { get; set; }

        /// <summary>
        /// 合計本数
        /// </summary>
        public string TotalHonsuu { get; set; }

        /// <summary>
        /// 合計重量
        /// </summary>
        public string TotalWeight { get; set; }

        /// <summary>
        /// 抽出日
        /// </summary>
        public string ExtractionYMD { get; set; }

        #endregion
    }
}

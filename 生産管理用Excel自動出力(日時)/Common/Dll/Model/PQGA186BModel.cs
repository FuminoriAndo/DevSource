//*************************************************************************************
//
//   PQGA186Bのモデル
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
    /// PQGA186Bのモデル
    /// </summary>
    public class PQGA186BModel : ModelBase
    {
        #region プロパティ

        /// <summary>
        /// 発行№+日付
        /// </summary>
        public string IssuNoAndDate { get; set; }

        /// <summary>
        /// チャージ№
        /// </summary>
        public string CHNo { get; set; }

        /// <summary>
        /// 規格名
        /// </summary>
        public string StandardName { get; set; }

        /// <summary>
        /// スラブサイズ
        /// </summary>
        public string SlabSize { get; set; }

        /// <summary>
        /// スラブサイズ(厚)
        /// </summary>
        public string SlabAtu { get; set; }

        /// <summary>
        /// スラブサイズ(巾)
        /// </summary>
        public string SlabHaba { get; set; }

        /// <summary>
        /// スラブサイズ(長)
        /// </summary>
        public string SlabNaga { get; set; }

        /// <summary>
        /// 単重
        /// </summary>
        public string UnitWeight { get; set; }

        /// <summary>
        /// 受注№
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 計画№
        /// </summary>
        public string PlanNo { get; set; }

        /// <summary>
        /// 抽出日
        /// </summary>
        public string ExtractionYMD { get; set; }

        #endregion
    }
}

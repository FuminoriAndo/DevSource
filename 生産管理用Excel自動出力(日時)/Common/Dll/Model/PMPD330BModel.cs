//*************************************************************************************
//
//   PMPD330Bのモデル
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
    /// PMPD330Bのモデル
    /// </summary>
    public class PMPD330BModel : ModelBase
    {
        #region プロパティ

        /// <summary>
        /// 鋼種名
        /// </summary>
        public string SteelsName { get; set; }

        /// <summary>
        /// 鋼種別
        /// </summary>
        public string SteelsType { get; set; }

        /// <summary>
        /// 規格名
        /// </summary>
        public string StandardName { get; set; }

        /// <summary>
        /// 製品サイズ
        /// </summary>
        public string ProductSize { get; set; }

        /// <summary>
        /// 製品厚
        /// </summary>
        public string ProductAtu { get; set; }

        /// <summary>
        /// 製品巾
        /// </summary>
        public string ProductHaba { get; set; }

        /// <summary>
        /// 製品長
        /// </summary>
        public string ProductNaga { get; set; }

        /// <summary>
        /// 定耳
        /// </summary>
        public string TM { get; set; }

        /// <summary>
        /// 製品コード
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 受注№
        /// </summary>
        public string OrdersNo { get; set; }

        /// <summary>
        /// 需要家名
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 納期月日
        /// </summary>
        public string DeliveryDate { get; set; }

        /// <summary>
        /// 納期ランク
        /// </summary>
        public string DeliveryDateRank { get; set; }

        /// <summary>
        /// 枚数
        /// </summary>
        public string Maisuu { get; set; }

        /// <summary>
        /// ロールサイズ
        /// </summary>
        public string RollSize { get; set; }

        /// <summary>
        /// ロール巾
        /// </summary>
        public string RollHaba { get; set; }

        /// <summary>
        /// ロール長
        /// </summary>
        public string RollNaga { get; set; }

        /// <summary>
        /// 保温区分
        /// </summary>
        public string HeatInsulation { get; set; }

        /// <summary>
        /// スラブサイズ
        /// </summary>
        public string SlabSize { get; set; }

        /// <summary>
        /// スラブ厚
        /// </summary>
        public string SlabAtu { get; set; }

        /// <summary>
        /// スラブ巾
        /// </summary>
        public string SlabHaba { get; set; }

        /// <summary>
        /// スラブ長
        /// </summary>
        public string SlabNaga { get; set; }

        /// <summary>
        /// スラブ重量
        /// </summary>
        public string SlabWeight { get; set; }

        /// <summary>
        /// スラブ本数
        /// </summary>
        public string SlabCount { get; set; }

        /// <summary>
        /// 急ぎ区分 
        /// </summary>
        public string HasteDivision { get; set; }

        /// <summary>
        /// 特殊区分
        /// </summary>
        public string SpecialDivision { get; set; }

        /// <summary>
        /// 輸出区分
        /// </summary>
        public string ExportDivision { get; set; }

        #endregion
    }
}

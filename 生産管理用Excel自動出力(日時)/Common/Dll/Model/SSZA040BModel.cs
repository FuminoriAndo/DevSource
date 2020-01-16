//*************************************************************************************
//
//   SSZA400Bのモデル
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
    /// SSZA400Bのモデル
    /// </summary>
    public class SSZA040BModel : ModelBase
    {
        #region プロパティ

        /// <summary>
        /// 受注№
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 規格名
        /// </summary>
        public string StandardName { get; set; }

        /// <summary>
        /// 製品サイズ
        /// </summary>
        public string ProductSize { get; set; }

        /// <summary>
        /// 製品サイズ(厚)
        /// </summary>
        public string ProductAtu { get; set; }

        /// <summary>
        /// 製品サイズ(巾)
        /// </summary>
        public string ProductHaba { get; set; }

        /// <summary>
        /// 製品サイズ(長)
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
        /// 需要家名
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 受注枚数
        /// </summary>
        public string OrderCount { get; set; }

        /// <summary>
        /// 受注重量
        /// </summary>
        public string OrderWeight { get; set; }

        /// <summary>
        /// 出荷枚数
        /// </summary>
        public string ShipmentCount { get; set; }

        /// <summary>
        /// 受渡場所名
        /// </summary>
        public string TransferPlaceName { get; set; }

        /// <summary>
        /// 受渡条件
        /// </summary>
        public string TransferCondition { get; set; }

        /// <summary>
        /// 納期
        /// </summary>
        public string DeliveryDate { get; set; }

        /// <summary>
        /// 納期ランク
        /// </summary>
        public string DeliveryDateRank { get; set; }

        /// <summary>
        /// 引当
        /// </summary>
        public string ProvisionClassfication { get; set; }

        /// <summary>
        /// 抽出日
        /// </summary>
        public string ExtractionYMD { get; set; }

        #endregion
    }
}

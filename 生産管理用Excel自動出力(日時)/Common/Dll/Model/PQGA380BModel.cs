//*************************************************************************************
//
//   PQGA380Bのモデル
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
    /// PQGA380Bのモデル
    /// </summary>
    public class PQGA380BModel : ModelBase
    {
        #region プロパティ

        /// <summary>
        /// 受注№
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 需要家コード
        /// </summary>
        public string CustomerCode { get; set; }

        /// <summary>
        /// 需要家名
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 商社コード
        /// </summary>
        public string DisutributorCode { get; set; }

        /// <summary>
        /// 都市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 回収
        /// </summary>
        public string Collectiion { get; set; }

        /// <summary>
        /// 品種
        /// </summary>
        public string Kind { get; set; }

        /// <summary>
        /// 規格コード
        /// </summary>
        public string StandardCode { get; set; }

        /// <summary>
        /// 決済条件
        /// </summary>
        public string SettlementCondition { get; set; }

        /// <summary>
        /// 商社名
        /// </summary>
        public string DisutributorName { get; set; }

        /// <summary>
        /// 規格名
        /// </summary>
        public string StandardName { get; set; }

        /// <summary>
        /// 製品サイズ
        /// </summary>
        public string ProductSize { get; set; }

        /// <summary>
        /// 製品(厚)
        /// </summary>
        public string ProductAtu { get; set; }

        /// <summary>
        /// 製品(巾)
        /// </summary>
        public string ProductHaba { get; set; }

        /// <summary>
        /// 製品(長)
        /// </summary>
        public string ProductNaga { get; set; }

        /// <summary>
        /// 出荷枚数
        /// </summary>
        public string ShipmentHonsuu { get; set; }

        /// <summary>
        /// 出荷重量
        /// </summary>
        public string ShipmentWeight { get; set; }

        /// <summary>
        /// 単価
        /// </summary>
        public string UnitPrice { get; set; }

        /// <summary>
        /// 受渡条件
        /// </summary>
        public string TransferCondition { get; set; }

        /// <summary>
        /// チャージ№
        /// </summary>
        public string CHNo { get; set; }

        /// <summary>
        /// １次№①
        /// </summary>
        public string Slab1No1 { get; set; }

        /// <summary>
        /// １次№②
        /// </summary>
        public string Slab1No2 { get; set; }

        /// <summary>
        /// １次№③
        /// </summary>
        public string Slab1No3 { get; set; }

        /// <summary>
        /// １次№④
        /// </summary>
        public string Slab1No4 { get; set; }

        /// <summary>
        /// １次№⑤
        /// </summary>
        public string Slab1No5 { get; set; }

        /// <summary>
        /// １次№⑥
        /// </summary>
        public string Slab1No6 { get; set; }

        /// <summary>
        /// １次№⑦
        /// </summary>
        public string Slab1No7 { get; set; }

        /// <summary>
        /// １次№⑧
        /// </summary>
        public string Slab1No8 { get; set; }

        /// <summary>
        /// １次№⑨
        /// </summary>
        public string Slab1No9 { get; set; }

        /// <summary>
        /// １次№⑩
        /// </summary>
        public string Slab1No10 { get; set; }

        /// <summary>
        /// チャージスラブ本数
        /// </summary>
        public string SlabHonsuu { get; set; }

        /// <summary>
        /// チャージスラブ重量
        /// </summary>
        public string SlabWeight { get; set; }

        /// <summary>
        /// 抽出日
        /// </summary>
        public string ExtractionYMD { get; set; }

        #endregion
    }
}

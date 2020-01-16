//*************************************************************************************
//
//   PQGA420Bのモデル
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
    /// PQGA420Bのモデル
    /// </summary>
    public class PQGA420BModel : ModelBase
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
        public string Collection { get; set; }

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
        /// 本数
        /// </summary>
        public string Honsuu { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        public string Weight { get; set; }

        /// <summary>
        /// 納期
        /// </summary>
        public string DeliveryDate { get; set; }

        /// <summary>
        /// ランク
        /// </summary>
        public string DeliveryDateRank { get; set; }

        /// <summary>
        /// 受渡し条件
        /// </summary>
        public string TransferCondition { get; set; }

        /// <summary>
        /// 抽出日
        /// </summary>
        public string ExtractionYMD { get; set; }

        #endregion
    }
}

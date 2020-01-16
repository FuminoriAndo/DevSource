//*************************************************************************************
//
//   PMPF090Bのモデル
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
    /// PMPF090Bのモデル
    /// </summary>
    public class PMPF090BModel : ModelBase
    {
        #region プロパティ

        /// <summary>
        /// 未採取区分
        /// </summary>
        public string NotCollectClassfication { get; set; }

        /// <summary>
        /// 受注№
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 受注№1
        /// </summary>
        public string OrderNo1 { get; set; }

        /// <summary>
        /// 受注№2
        /// </summary>
        public string OrderNo2 { get; set; }

        /// <summary>
        /// 受注№3
        /// </summary>
        public string OrderNo3 { get; set; }

        /// <summary>
        /// 需要家名
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 未計画
        /// </summary>
        public string UnPlan { get; set; }

        /// <summary>
        /// 納期
        /// </summary>
        public string DeliveryDate { get; set; }

        /// <summary>
        /// 納期ランク
        /// </summary>
        public string DeliveryDateRank { get; set; }

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
        /// 定耳
        /// </summary>
        public string TM { get; set; }

        /// <summary>
        /// 製品コード
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 置場
        /// </summary>
        public string Yard { get; set; }

        /// <summary>
        /// 置場コード
        /// </summary>
        public string YardCode { get; set; }

        /// <summary>
        /// 置場X
        /// </summary>
        public string YardX { get; set; }

        /// <summary>
        /// 置場Y
        /// </summary>
        public string YardY { get; set; }

        /// <summary>
        /// 枚数
        /// </summary>
        public string Maisuu { get; set; }

        /// <summary>
        /// 紐付枚数
        /// </summary>
        public string TieMaisuu { get; set; }

        /// <summary>
        /// 予約区分
        /// </summary>
        public string ReservationClassfication { get; set; }

        /// <summary>
        /// 発生品区分1
        /// </summary>
        public string GeneratingProductClassfication1 { get; set; }

        /// <summary>
        /// 板№1
        /// </summary>
        public string ItaNo1 { get; set; }

        /// <summary>
        /// 試験合否1
        /// </summary>
        public string TestAcceptance1 { get; set; }

        /// <summary>
        /// 積み上げ1
        /// </summary>
        public string PileUp1 { get; set; }

        /// <summary>
        /// 発生品区分2
        /// </summary>
        public string GeneratingProductClassfication2 { get; set; }

        /// <summary>
        /// 板№2
        /// </summary>
        public string ItaNo2 { get; set; }

        /// <summary>
        /// 試験合否2
        /// </summary>
        public string TestAcceptance2 { get; set; }

        /// <summary>
        /// 積み上げ2
        /// </summary>
        public string PileUp2 { get; set; }

        /// <summary>
        /// 抽出日
        /// </summary>
        public string ExtractionYMD { get; set; }

        #endregion
    }
}

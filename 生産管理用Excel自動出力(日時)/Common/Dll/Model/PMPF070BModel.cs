//*************************************************************************************
//
//   PMPF070Bのモデル
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
    /// PMPF070Bのモデル
    /// </summary>
    public class PMPF070BModel : ModelBase
    {
        #region プロパティ

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
        /// 納期
        /// </summary>
        public string DeliveryDate { get; set; }

        /// <summary>
        /// 納期(MM)
        /// </summary>
        public string DeliveryDateMM { get; set; }

        /// <summary>
        /// 納期(DD)
        /// </summary>
        public string DeliveryDateDD { get; set; }

        /// <summary>
        /// 納期ランク
        /// </summary>
        public string DeliveryDateRank { get; set; }

        /// <summary>
        /// 需要家名
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// ロットNo
        /// </summary>
        public string LotNo { get; set; }

        /// <summary>
        /// ロット1
        /// </summary>
        public string Lot1 { get; set; }

        /// <summary>
        /// ロット2
        /// </summary>
        public string Lot2 { get; set; }

        /// <summary>
        /// ロール№
        /// </summary>
        public string RollNo { get; set; }

        /// <summary>
        /// 未採取ロット
        /// </summary>
        public string NotCollectedLot { get; set; }

        /// <summary>
        /// 未採取ロット1
        /// </summary>
        public string NotCollectedLotNo1 { get; set; }

        /// <summary>
        /// 未採取ロット2
        /// </summary>
        public string NotCollectedLotNo2 { get; set; }

        /// <summary>
        /// 状態
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 計画規格コード
        /// </summary>
        public string PlanStandardCode { get; set; }

        /// <summary>
        /// 計画サイズ
        /// </summary>
        public string PlanSize { get; set; }

        /// <summary>
        /// 計画厚
        /// </summary>
        public string PlanAtu { get; set; }

        /// <summary>
        /// 計画巾
        /// </summary>
        public string PlanHaba { get; set; }

        /// <summary>
        /// 計画長
        /// </summary>
        public string PlanNaga { get; set; }

        /// <summary>
        /// 計画耳
        /// </summary>
        public string PlanTM { get; set; }

        /// <summary>
        /// 計画製品コード
        /// </summary>
        public string PlanProductCode { get; set; }

        /// <summary>
        /// 計画枚数
        /// </summary>
        public string PlanMaisuu { get; set; }

        /// <summary>
        /// 実績規格コード
        /// </summary>
        public string PerformanceStandardCode { get; set; }

        /// <summary>
        /// 実績サイズ
        /// </summary>
        public string PerformanceSize { get; set; }

        /// <summary>
        /// 実績厚
        /// </summary>
        public string PerformanceAtu { get; set; }

        /// <summary>
        /// 実績巾
        /// </summary>
        public string PerformanceHaba { get; set; }

        /// <summary>
        /// 実績長
        /// </summary>
        public string PerformanceNaga { get; set; }

        /// <summary>
        /// 実績耳
        /// </summary>
        public string PerformanceTM { get; set; }

        /// <summary>
        /// 実績製品コード
        /// </summary>
        public string PerformanceProdutCode { get; set; }

        /// <summary>
        /// 実績枚数
        /// </summary>
        public string PerformanceMaisuu { get; set; }

        /// <summary>
        /// 発生工程
        /// </summary>
        public string GeneratingStep { get; set; }

        /// <summary>
        /// 特殊
        /// </summary>
        public string Special { get; set; }

        /// <summary>
        /// 抽出日
        /// </summary>
        public string ExtractionYMD { get; set; }

        #endregion
    }
}

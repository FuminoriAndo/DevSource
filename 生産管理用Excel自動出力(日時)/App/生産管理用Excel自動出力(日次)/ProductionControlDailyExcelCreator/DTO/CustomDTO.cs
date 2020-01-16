//*************************************************************************************
//
//   カスタムDTO
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************
using DTO.Base;

namespace ProductionControlDailyExcelCreator.DTO
{
    /// <summary>
    /// PMGQ010BのDTO
    /// </summary>
    public class PMGQ010BDTO : DTOBase
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PMGQ010BDTO()
        {
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 受注№
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 需要家名
        /// </summary>
        public string CustomerName { get; set; }

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
        /// ロールサイズ
        /// </summary>
        public string RollSize { get; set; }

        /// <summary>
        /// ロールサイズ(巾)
        /// </summary>
        public string RollHaba { get; set; }

        /// <summary>
        /// ロールサイズ(長)
        /// </summary>
        public string RollNaga { get; set; }

        /// <summary>
        /// TP区分
        /// </summary>
        public string TPClassfication { get; set; }

        /// <summary>
        /// ロット
        /// </summary>
        public string Lot { get; set; }

        /// <summary>
        /// 圧延予定
        /// </summary>
        public string AtuenYMD { get; set; }

        /// <summary>
        /// スラブ№
        /// </summary>
        public string SlabNo { get; set; }

        /// <summary>
        /// ロール№
        /// </summary>
        public string RollNo { get; set; }

        #endregion

    }

    /// <summary>
    /// PMPA260BのDTO
    /// </summary>
    public class PMPA260BDTO : DTOBase
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PMPA260BDTO()
        {
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// RCNO
        /// </summary>
        public string RCNO { get; set; }

        /// <summary>
        /// 勤務
        /// </summary>
        public string Work { get; set; }

        /// <summary>
        /// 圧延順
        /// </summary>
        public string AtuenOrder { get; set; }

        /// <summary>
        /// ロット№
        /// </summary>
        public string LotNo { get; set; }

        /// <summary>
        /// 1次鋳片№
        /// </summary>
        public string Slab1No { get; set; }

        /// <summary>
        /// 材料識別
        /// </summary>
        public string MaterialsIdentification { get; set; }

        /// <summary>
        /// ＴＰ管理
        /// </summary>
        public string TPManagement { get; set; }

        /// <summary>
        /// 厚テーバー
        /// </summary>
        public string AtuTapera { get; set; }

        /// <summary>
        /// 巾テーパー
        /// </summary>
        public string HabaTapera { get; set; }

        /// <summary>
        /// ＨＣ区分
        /// </summary>
        public string HCDivision { get; set; }

        /// <summary>
        /// 抽出時分
        /// </summary>
        public string ExtractionHHMM { get; set; }

        /// <summary>
        /// 規格
        /// </summary>
        public string Standard { get; set; }

        /// <summary>
        /// ロールサイズ
        /// </summary>
        public string RollSize { get; set; }

        /// <summary>
        /// ロールサイズ(厚)
        /// </summary>
        public string RollAtu { get; set; }

        /// <summary>
        /// ロールサイズ(巾)
        /// </summary>
        public string RollHaba { get; set; }

        /// <summary>
        /// ロールサイズ(長)
        /// </summary>
        public string RollNaga { get; set; }

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
        /// スラブ重量
        /// </summary>
        public int SlabWeight { get; set; }

        /// <summary>
        /// 本数
        /// </summary>
        public int SlabCount { get; set; }

        /// <summary>
        /// スラブ合計重量(スラブ重量　* 本数)
        /// </summary>
        public int SlabTotalWeight
        {
            get
            {
                return SlabWeight * SlabCount;
            }
        }

        /// <summary>
        /// 再計画
        /// </summary>
        public string RePlan { get; set; }

        /// <summary>
        /// 急ぎ
        /// </summary>
        public string Haste { get; set; }

        /// <summary>
        /// 輸出
        /// </summary>
        public string Export { get; set; }

        /// <summary>
        /// 抽出温度
        /// </summary>
        public int ExtractionTemperature { get; set; }

        /// <summary>
        /// 試圧
        /// </summary>
        public string TestPressure { get; set; }

        /// <summary>
        /// 板巾
        /// </summary>
        public string PlateHaba { get; set; }

        /// <summary>
        /// 歪
        /// </summary>
        public string Strain { get; set; }

        #endregion

    }

    /// <summary>
    /// PMPD330BのDTO
    /// </summary>
    public class PMPD330BDTO : DTOBase
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PMPD330BDTO()
        {
        }

        #endregion

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
        public int Maisuu { get; set; }

        /// <summary>
        /// ロールサイズ
        /// </summary>
        public string RollSize { get; set; }

        /// <summary>
        /// ロールサイズ(巾)
        /// </summary>
        public string RollHaba { get; set; }

        /// <summary>
        /// ロールサイズ(長)
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
        /// スラブ重量
        /// </summary>
        public int SlabWeight { get; set; }

        /// <summary>
        /// スラブ本数
        /// </summary>
        public int SlabCount { get; set; }

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

    /// <summary>
    /// PMPF070BのDTO
    /// </summary>
    public class PMPF070BDTO : DTOBase
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PMPF070BDTO()
        {
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 受注№
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 納期
        /// </summary>
        public string DeliveryDate { get; set; }

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
        /// ロール№
        /// </summary>
        public string RollNo { get; set; }

        /// <summary>
        /// 未採取ロット
        /// </summary>
        public string NotCollectedLot { get; set; }

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
        /// 計画サイズ(厚)
        /// </summary>
        public string PlanAtu { get; set; }

        /// <summary>
        /// 計画サイズ(巾)
        /// </summary>
        public string PlanHaba { get; set; }

        /// <summary>
        /// 計画サイズ(長)
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
        public int PlanMaisuu { get; set; }

        /// <summary>
        /// 実績規格コード
        /// </summary>
        public string PerformanceStandardCode { get; set; }

        /// <summary>
        /// 実績サイズ
        /// </summary>
        public string PerformanceSize { get; set; }

        /// <summary>
        /// 実績サイズ(厚)
        /// </summary>
        public string PerformanceAtu { get; set; }

        /// <summary>
        /// 実績サイズ(巾)
        /// </summary>
        public string PerformanceHaba { get; set; }

        /// <summary>
        /// 実績サイズ(長)
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
        public int PerformanceMaisuu { get; set; }

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

    /// <summary>
    /// PMPF090BのDTO
    /// </summary>
    public class PMPF090BDTO : DTOBase
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PMPF090BDTO()
        {
        }

        #endregion

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
        /// 需要家名
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 未計画
        /// </summary>
        public int UnPlan { get; set; }

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
        /// 置場
        /// </summary>
        public string Yard { get; set; }

        /// <summary>
        /// 枚数
        /// </summary>
        public int Maisuu { get; set; }

        /// <summary>
        /// 紐付枚数
        /// </summary>
        public int TieMaisuu { get; set; }

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

        #endregion

    }

    /// <summary>
    /// PQGA186BのDTO
    /// </summary>
    public class PQGA186BDTO : DTOBase
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PQGA186BDTO()
        {
        }

        #endregion

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
        public int UnitWeight { get; set; }

        /// <summary>
        /// 受注№
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 計画№
        /// </summary>
        public string PlanNo { get; set; }

        #endregion

    }

    /// <summary>
    /// PQGA380BのDTO
    /// </summary>
    public class PQGA380BDTO : DTOBase
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PQGA380BDTO()
        {
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 受注№
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 需要家名
        /// </summary>
        public string CustomerName { get; set; }

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
        /// 出荷本数
        /// </summary>
        public int ShipmentHonsuu { get; set; }

        /// <summary>
        /// 出荷重量
        /// </summary>
        public int ShipmentWeight { get; set; }

        /// <summary>
        /// 単価
        /// </summary>
        public int UnitPrice { get; set; }

        /// <summary>
        /// 受渡条件
        /// </summary>
        public string TransferCondition { get; set; }

        /// <summary>
        /// 抽出日
        /// </summary>
        public string ExtractionYMD { get; set; }

        #endregion

    }

    /// <summary>
    /// PQGA420BのDTO
    /// </summary>
    public class PQGA420BDTO : DTOBase
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PQGA420BDTO()
        {
        }

        #endregion

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
        public int Honsuu { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        public int Weight { get; set; }

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

        #endregion

    }

    /// <summary>
    /// PQGA820BのDTO
    /// </summary>
    public class PQGA820BDTO : DTOBase
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PQGA820BDTO()
        {
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 出荷日
        /// </summary>
        public string ShipmentYYMMDD { get; set; }

        /// <summary>
        /// ZK081本数
        /// </summary>
        public int ZK081Honsuu { get; set; }

        /// <summary>
        /// ZK081重量
        /// </summary>
        public int ZK081Weight { get; set; }

        /// <summary>
        /// ZK085本数
        /// </summary>
        public int ZK085Honsuu { get; set; }

        /// <summary>
        /// ZK085重量
        /// </summary>
        public int ZK085Weight { get; set; }

        /// <summary>
        /// ZK121本数
        /// </summary>
        public int ZK121Honsuu { get; set; }

        /// <summary>
        /// ZK121重量
        /// </summary>
        public int ZK121Weight { get; set; }

        /// <summary>
        /// ZK172本数
        /// </summary>
        public int ZK172Honsuu { get; set; }

        /// <summary>
        /// ZK172重量
        /// </summary>
        public int ZK172Weight { get; set; }

        /// <summary>
        /// ＳＰＨＣ本数
        /// </summary>
        public int SPHCHonsuu { get; set; }

        /// <summary>
        /// ＳＰＨＣ重量
        /// </summary>
        public int SPHCWeight { get; set; }

        /// <summary>
        /// ＳＳ400本数
        /// </summary>
        public int SS400Honsuu { get; set; }

        /// <summary>
        /// ＳＳ400重量
        /// </summary>
        public int SS400Weight { get; set; }

        /// <summary>
        /// その他本数
        /// </summary>
        public int OtherHonsuu { get; set; }

        /// <summary>
        /// その他重量
        /// </summary>
        public int OtherWeight { get; set; }

        /// <summary>
        /// 合計本数
        /// </summary>
        public int TotalHonsuu { get; set; }

        /// <summary>
        /// 合計重量
        /// </summary>
        public int TotalWeight { get; set; }

        #endregion

    }

    /// <summary>
    /// SSYM040BのDTO
    /// </summary>
    public class SSYM040BDTO : DTOBase
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SSYM040BDTO()
        {
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 年月日
        /// </summary>
        public string YYYYMMDD { get; set; }

        /// <summary>
        /// 中板屑
        /// </summary>
        public int MiddlePlateWaste { get; set; }

        /// <summary>
        /// 日清リターン屑
        /// </summary>
        public int NissinReturnWaste { get; set; }

        /// <summary>
        /// トリーマ屑
        /// </summary>
        public int TrimmerWaste { get; set; }

        /// <summary>
        /// 厚板ライン屑
        /// </summary>
        public int AtuItaLineWaste { get; set; }

        /// <summary>
        /// プレーナ屑
        /// </summary>
        public int PlanerWaste { get; set; }

        /// <summary>
        /// レーザー屑
        /// </summary>
        public int LaserWaste { get; set; }

        /// <summary>
        /// プレーナ知多屑
        /// </summary>
        public int PlanerChitaWaste { get; set; }

        /// <summary>
        /// ミスロール屑
        /// </summary>
        public int MissRollWaste { get; set; }

        /// <summary>
        /// コラム返品屑
        /// </summary>
        public int ColumuReturnWaste { get; set; }

        /// <summary>
        /// 合計
        /// </summary>
        public int Total { get; set; }

        #endregion

    }

    /// <summary>
    /// SSYM050BのDTO
    /// </summary>
    public class SSYM050BDTO : DTOBase
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SSYM050BDTO()
        {
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 年月日
        /// </summary>
        public string YYYYMMDD { get; set; }

        /// <summary>
        /// エンドシャー屑
        /// </summary>
        public int EndShearWaste { get; set; }

        /// <summary>
        /// プレーナ屑
        /// </summary>
        public int PlanerWaste { get; set; }

        /// <summary>
        /// トリーマ屑
        /// </summary>
        public int TrimmerWaste { get; set; }

        /// <summary>
        /// レーザー屑
        /// </summary>
        public int LaserWaste { get; set; }

        /// <summary>
        /// その他屑
        /// </summary>
        public int OtherWaste { get; set; }

        /// <summary>
        /// 合計
        /// </summary>
        public int Total { get; set; }

        #endregion

    }

    /// <summary>
    /// SSZA040BのDTO
    /// </summary>
    public class SSZA040BDTO : DTOBase
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SSZA040BDTO()
        {
        }

        #endregion

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
        public int OrderCount { get; set; }

        /// <summary>
        /// 受注重量
        /// </summary>
        public int OrderWeight { get; set; }

        /// <summary>
        /// 出荷枚数
        /// </summary>
        public int ShipmentCount { get; set; }

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

        #endregion

    }
}

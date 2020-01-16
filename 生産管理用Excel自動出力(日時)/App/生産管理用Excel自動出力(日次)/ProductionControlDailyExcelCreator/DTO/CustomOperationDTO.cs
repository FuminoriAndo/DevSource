//*************************************************************************************
//
//   各操作のカスタムDTO
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************
using System.Collections.Generic;
using DTO.Base;

namespace ProductionControlDailyExcelCreator.DTO
{
    /// <summary>
    /// 技術特殊引合品のDTO
    /// </summary>
    public class TechnologySpecialInquiriesGoodsDTO<T> : FileBaseDTO<T> where T : FileDetailDTO
    {
    }

    /// <summary>
    /// 技術特殊引合品のヘッダー
    /// </summary>
    public class TechnologySpecialInquiriesGoodsHeader : FileHeaderDTO
    {
        #region フィールド

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TechnologySpecialInquiriesGoodsHeader()
        {
        }

        #endregion

        #region プロパティ

        #endregion
    }

    /// <summary>
    /// 技術特殊引合品の詳細
    /// </summary>
    public class TechnologySpecialInquiriesGoodsDetail : FileDetailDTO
    {
        #region フィールド

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TechnologySpecialInquiriesGoodsDetail()
        {
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 技術特殊引合品のリスト
        /// </summary>
        public IList<PMGQ010BDTO> PMGQ010BDTOList { get; set; }

        #endregion
    }

    /// <summary>
    /// 技術特殊引合品のフッター
    /// </summary>
    public class TechnologySpecialInquiriesGoodsFooter : FileFooterDTO
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TechnologySpecialInquiriesGoodsFooter()
        {
        }

        #endregion

        #region プロパティ

        #endregion
    }

    /// <summary>
    /// 圧延計画のDTO
    /// </summary>
    public class RollingPlanDTO<T> : FileBaseDTO<T> where T : FileDetailDTO
    {
    }

    /// <summary>
    /// 圧延計画のヘッダー
    /// </summary>
    public class RollingPlanHeader : FileHeaderDTO
    {
        #region フィールド

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RollingPlanHeader()
        {
        }

        #endregion

        #region プロパティ

        #endregion
    }

    /// <summary>
    /// 圧延計画の詳細
    /// </summary>
    public class RollingPlanDetail : FileDetailDTO
    {
        #region フィールド

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RollingPlanDetail()
        {
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 圧延計画のリスト
        /// </summary>
        public IList<PMPA260BDTO> PMPA260BDTOList { get; set; }

        #endregion
    }

    /// <summary>
    /// 圧延計画のフッター
    /// </summary>
    public class RollingPlanFooter : FileFooterDTO
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RollingPlanFooter()
        {
        }

        #endregion

        #region プロパティ

        #endregion
    }

    /// <summary>
    /// 未計画受注のDTO
    /// </summary>
    public class UnPlannedOrdersDTO<T> : FileBaseDTO<T> where T : FileDetailDTO
    {
    }

    /// <summary>
    /// 未計画受注のヘッダー
    /// </summary>
    public class UnPlannedOrdersHeader : FileHeaderDTO
    {
        #region フィールド

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UnPlannedOrdersHeader()
        {
        }

        #endregion

        #region プロパティ

        #endregion
    }

    /// <summary>
    /// 未計画受注の詳細
    /// </summary>
    public class UnPlannedOrdersDetail : FileDetailDTO
    {
        #region フィールド

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UnPlannedOrdersDetail()
        {
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 未計画受注のリスト
        /// </summary>
        public IList<PMPD330BDTO> PMPD330BDTOList { get; set; }

        #endregion
    }

    /// <summary>
    /// 未計画受注のフッター
    /// </summary>
    public class UnPlannedOrdersFooter : FileFooterDTO
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UnPlannedOrdersFooter()
        {
        }

        #endregion

        #region プロパティ

        #endregion
    }

    /// <summary>
    /// 未採取のDTO
    /// </summary>
    public class UnHarvestingDTO<T> : FileBaseDTO<T> where T : FileDetailDTO
    {
    }

    /// <summary>
    /// 未採取のヘッダー
    /// </summary>
    public class UnHarvestingHeader : FileHeaderDTO
    {
        #region フィールド

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UnHarvestingHeader()
        {
        }

        #endregion

        #region プロパティ

        #endregion
    }

    /// <summary>
    /// 未採取の詳細
    /// </summary>
    public class UnHarvestingDetail : FileDetailDTO
    {
        #region フィールド

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UnHarvestingDetail()
        {
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 未採取のリスト
        /// </summary>
        public IList<PMPF070BDTO> PMPF070BDTOList { get; set; }

        #endregion
    }

    /// <summary>
    /// 未採取のフッター
    /// </summary>
    public class UnHarvestingsFooter : FileFooterDTO
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UnHarvestingsFooter()
        {
        }

        #endregion

        #region プロパティ

        #endregion
    }

    /// <summary>
    /// 余剰品引当可能在庫のDTO
    /// </summary>
    public class SurplusGoodsProvisionpPossibleStockDTO<T> : FileBaseDTO<T> where T : FileDetailDTO
    {
    }

    /// <summary>
    /// 余剰品引当可能在庫のヘッダー
    /// </summary>
    public class SurplusGoodsProvisionpPossibleStockHeader : FileHeaderDTO
    {
        #region フィールド

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SurplusGoodsProvisionpPossibleStockHeader()
        {
        }

        #endregion

        #region プロパティ

        #endregion
    }

    /// <summary>
    /// 余剰品引当可能在庫の詳細
    /// </summary>
    public class SurplusGoodsProvisionpPossibleStockDetail : FileDetailDTO
    {
        #region フィールド

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SurplusGoodsProvisionpPossibleStockDetail()
        {
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 余剰品引当可能在庫のリスト
        /// </summary>
        public IList<PMPF090BDTO> PMPF090BDTOList { get; set; }

        #endregion
    }

    /// <summary>
    /// 余剰品引当可能在庫のフッター
    /// </summary>
    public class SurplusGoodsProvisionpPossibleStockFooter : FileFooterDTO
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SurplusGoodsProvisionpPossibleStockFooter()
        {
        }

        #endregion

        #region プロパティ

        #endregion
    }

    /// <summary>
    /// 外販出荷計画のDTO
    /// </summary>
    public class ExternalSalesShipmentPlanDTO<T> : FileBaseDTO<T> where T : FileDetailDTO
    {
    }

    /// <summary>
    /// 外販出荷計画のヘッダー
    /// </summary>
    public class ExternalSalesShipmentPlanHeader : FileHeaderDTO
    {
        #region フィールド

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ExternalSalesShipmentPlanHeader()
        {
        }

        #endregion

        #region プロパティ

        #endregion
    }

    /// <summary>
    /// 外販出荷計画の詳細
    /// </summary>
    public class ExternalSalesShipmentPlanDetail : FileDetailDTO
    {
        #region フィールド

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ExternalSalesShipmentPlanDetail()
        {
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 外販出荷計画のリスト
        /// </summary>
        public IList<PQGA186BDTO> PQGA186BDTOList { get; set; }

        #endregion
    }

    /// <summary>
    /// 外販出荷計画のフッター
    /// </summary>
    public class ExternalSalesShipmentPlanFooter : FileFooterDTO
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ExternalSalesShipmentPlanFooter()
        {
        }

        #endregion

        #region プロパティ

        #endregion
    }

    /// <summary>
    /// 外販出荷実績のDTO
    /// </summary>
    public class ExternalSalesShipmentPerformanceDTO<T> : FileBaseDTO<T> where T : FileDetailDTO
    {
    }

    /// <summary>
    /// 外販出荷実績のヘッダー
    /// </summary>
    public class ExternalSalesShipmentPerformanceHeader : FileHeaderDTO
    {
        #region フィールド

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ExternalSalesShipmentPerformanceHeader()
        {
        }

        #endregion

        #region プロパティ

        #endregion
    }

    /// <summary>
    /// 外販出荷実績の詳細
    /// </summary>
    public class ExternalSalesShipmentPerformanceDetail : FileDetailDTO
    {
        #region フィールド

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ExternalSalesShipmentPerformanceDetail()
        {
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 外販出荷計画のリスト
        /// </summary>
        public IList<PQGA380BDTO> PQGA380BDTOList { get; set; }

        #endregion
    }

    /// <summary>
    /// 外販出荷実績のフッター
    /// </summary>
    public class ExternalSalesShipmentPerformanceFooter : FileFooterDTO
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ExternalSalesShipmentPerformanceFooter()
        {
        }

        #endregion

        #region プロパティ

        #endregion
    }

    /// <summary>
    /// 外販受注残のDTO
    /// </summary>
    public class ExternalSalesBacklogDTO<T> : FileBaseDTO<T> where T : FileDetailDTO
    {
    }

    /// <summary>
    /// 外販受注残のヘッダー
    /// </summary>
    public class ExternalSalesBacklogHeader : FileHeaderDTO
    {
        #region フィールド

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ExternalSalesBacklogHeader()
        {
        }

        #endregion

        #region プロパティ

        #endregion
    }

    /// <summary>
    /// 外販受注残の詳細
    /// </summary>
    public class ExternalSalesBacklogDetail : FileDetailDTO
    {
        #region フィールド

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ExternalSalesBacklogDetail()
        {
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 外販受注残のリスト
        /// </summary>
        public IList<PQGA420BDTO> PQGA420BDTOList { get; set; }

        #endregion
    }

    /// <summary>
    /// 外販受注残のフッター
    /// </summary>
    public class ExternalSalesBacklogFooter : FileFooterDTO
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ExternalSalesBacklogFooter()
        {
        }

        #endregion

        #region プロパティ

        #endregion
    }

    /// <summary>
    /// 外販規格別集計のDTO
    /// </summary>
    public class ExternalStandardByAggregateDTO<T> : FileBaseDTO<T> where T : FileDetailDTO
    {
    }

    /// <summary>
    /// 外販規格別集計のヘッダー
    /// </summary>
    public class ExternalStandardByAggregateHeader : FileHeaderDTO
    {
        #region フィールド

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ExternalStandardByAggregateHeader()
        {
        }

        #endregion

        #region プロパティ

        #endregion
    }

    /// <summary>
    /// 外販規格別集計の詳細
    /// </summary>
    public class ExternalStandardByAggregateDetail : FileDetailDTO
    {
        #region フィールド

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ExternalStandardByAggregateDetail()
        {
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 外販規格別集計のリスト
        /// </summary>
        public IList<PQGA820BDTO> PQGA820BDTOList { get; set; }

        #endregion
    }

    /// <summary>
    /// 外販規格別集計のフッター
    /// </summary>
    public class ExternalStandardByAggregateFooter : FileFooterDTO
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ExternalStandardByAggregateFooter()
        {
        }

        #endregion

        #region プロパティ

        #endregion
    }

    /// <summary>
    /// 発生屑・端板・伸鉄払出のDTO
    /// </summary>
    public class GeneratedWaste_SinglePlate_SinTetsuPayoutDTO<T> : FileBaseDTO<T> where T : FileDetailDTO
    {
    }

    /// <summary>
    /// 発生屑・端板・伸鉄払出のヘッダー
    /// </summary>
    public class GeneratedWaste_SinglePlate_SinTetsuPayoutHeader : FileHeaderDTO
    {
        #region フィールド

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GeneratedWaste_SinglePlate_SinTetsuPayoutHeader()
        {
        }

        #endregion

        #region プロパティ

        #endregion
    }

    /// <summary>
    /// 発生屑・端板・伸鉄払出の詳細
    /// </summary>
    public class GeneratedWaste_SinglePlate_SinTetsuPayoutDetail : FileDetailDTO
    {
        #region フィールド

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GeneratedWaste_SinglePlate_SinTetsuPayoutDetail()
        {
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 発生屑・単板・神鉄払出明細のリスト
        /// </summary>
        public IList<SSYM040BDTO> SSYM040BDTOList { get; set; }

        #endregion
    }

    /// <summary>
    /// 発生屑・端板・伸鉄払出のフッター
    /// </summary>
    public class GeneratedWaste_SinglePlate_SinTetsuPayoutFooter : FileFooterDTO
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GeneratedWaste_SinglePlate_SinTetsuPayoutFooter()
        {
        }

        #endregion

        #region プロパティ

        #endregion
    }

    /// <summary>
    /// スクラップ外販のDTO
    /// </summary>
    public class ScrapExternalSalesDTO<T> : FileBaseDTO<T> where T : FileDetailDTO
    {
    }

    /// <summary>
    /// スクラップ外販のヘッダー
    /// </summary>
    public class ScrapExternalSalesHeader : FileHeaderDTO
    {
        #region フィールド

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ScrapExternalSalesHeader()
        {
        }

        #endregion

        #region プロパティ

        #endregion
    }

    /// <summary>
    /// スクラップ外販の詳細
    /// </summary>
    public class ScrapExternalSalesDetail : FileDetailDTO
    {
        #region フィールド

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ScrapExternalSalesDetail()
        {
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// スクラップ外販のリスト
        /// </summary>
        public IList<SSYM050BDTO> SSYM050BDTOList { get; set; }

        #endregion
    }

    /// <summary>
    /// スクラップ外販のフッター
    /// </summary>
    public class ScrapExternalSalesFooter : FileFooterDTO
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ScrapExternalSalesFooter()
        {
        }

        #endregion

        #region プロパティ

        #endregion
    }

    /// <summary>
    /// 在庫売り受注のDTO
    /// </summary>
    public class StockSellOrdersDTO<T> : FileBaseDTO<T> where T : FileDetailDTO
    {
    }

    /// <summary>
    /// 在庫売り受注のヘッダー
    /// </summary>
    public class StockSellOrdersHeader : FileHeaderDTO
    {
        #region フィールド

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public StockSellOrdersHeader()
        {
        }

        #endregion

        #region プロパティ

        #endregion
    }

    /// <summary>
    /// 在庫売り受注の詳細
    /// </summary>
    public class StockSellOrdersDetail : FileDetailDTO
    {
        #region フィールド

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public StockSellOrdersDetail()
        {
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 在庫売り受注のリスト
        /// </summary>
        public IList<SSZA040BDTO> SSZA040BDTOList { get; set; }

        #endregion
    }

    /// <summary>
    /// 在庫売り受注のフッター
    /// </summary>
    public class StockSellOrdersFooter : FileFooterDTO
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public StockSellOrdersFooter()
        {
        }

        #endregion

        #region プロパティ

        #endregion
    }
}

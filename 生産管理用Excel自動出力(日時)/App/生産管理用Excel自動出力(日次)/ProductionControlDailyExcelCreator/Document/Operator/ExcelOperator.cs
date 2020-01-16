//*************************************************************************************
//
//   Excel操作
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************
using System;
using System.IO;
using ProductionControlDailyExcelCreator.Document.Operator;
using ProductionControlDailyExcelCreator.Document.Strategy;
using ProductionControlDailyExcelCreator.DTO;
using ProductionControlDailyExcelCreator.Log;
using ProductionControlDailyExcelCreator.Shared;
using Utility.Core;

namespace ProductionControlDocumentOperator.Document.Operator
{
    /// <summary>
    /// Excel操作
    /// </summary>
    internal class ExcelOperator : AbstractOperatorBase
    {
        #region フィールド

        /// <summary>
        /// カスタムログのマネージャ
        /// </summary>
        private readonly CustomLogManager logManager = null;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        internal ExcelOperator()
        {
            logManager = AppSharedModel.Instance.LogManager;
        }

        #endregion

        #region メソッド

        /// <summary>
        /// 技術特殊引合品チェックリスト(PMGQ010F)の出力
        /// </summary>
        /// <param name="document">ドキュメントのDTO</param>
        /// <returns>結果</returns>
        protected internal override bool OutputPMGQ010FList<T>(T document)
        {
            string fileName = null;

            try
            {
                if (document is PMGQ010FExcelOutputDocument<TechnologySpecialInquiriesGoodsDTO<TechnologySpecialInquiriesGoodsDetail>>)
                {
                    // 技術特殊引合品チェックリスト(PMGQ010F)のExcel出力ドキュメントDTOへのキャスト
                    var outputDocument
                        = document as PMGQ010FExcelOutputDocument<TechnologySpecialInquiriesGoodsDTO<TechnologySpecialInquiriesGoodsDetail>>;
                    // 技術特殊引合品チェックリスト(PMGQ010F)のストラテジの生成
                    var writer = new PMGQ010FExcelWriter(outputDocument.DocumentOperationContainer)
                                    .Chain(x => x.TemplateWorkBookPath = outputDocument.TemplateBookPath)
                                    .Chain(x => x.OutputWorkBookPath = outputDocument.OutputBookPath)
                                    .Chain(x => x.Year = outputDocument.Year)
                                    .Chain(x => x.Month = outputDocument.Month);

                    var item = outputDocument.OutputData
                        as TechnologySpecialInquiriesGoodsDTO<TechnologySpecialInquiriesGoodsDetail>;

                    fileName = Path.GetFileName(outputDocument.OutputBookPath);
                    logManager.Write("Excel出力開始(" + "ファイル名:" + fileName + ")");

                    // 初期化処理
                    writer.Initialize();
                    // 書込み前処理
                    writer.PreWrite();
                    // ヘッダーの書込み
                    writer.WriteHeader(item.FileHeaderInfo);
                    // 詳細の書込み
                    writer.WriteDetail(item.FileDetailInfo);
                    // フッターの書込み
                    writer.WriteFooter(item.FileFooterInfo);
                    // 書込み後処理
                    writer.PostWrite();
                    // 終了処理
                    writer.Terminate();
                    logManager.Write("Excel出力終了(" + "ファイル名:" + fileName + ")");
                }
            }

            catch (Exception ex)
            {
                logManager.Write("Excel出力異常(" + "ファイル名:" + fileName + "," + "エラーメッセージ" + ex.Message + ")");
                throw;
            }

            return true;
        }

        /// <summary>
        /// 圧延計画リスト(加熱炉用）(PMPA280F)の出力
        /// </summary>
        /// <param name="document">ドキュメントのDTO</param>
        /// <returns>結果</returns>
        protected internal override bool OutputPMPA280FList<T>(T document)
        {
            string fileName = null;
            
            try
            {
                if (document is PMPA280FExcelOutputDocument<RollingPlanDTO<RollingPlanDetail>>)
                {
                    // 圧延計画リスト(加熱炉用）(PMPA280F)のExcel出力ドキュメントDTOへのキャスト
                    var outputDocument
                        = document as PMPA280FExcelOutputDocument<RollingPlanDTO<RollingPlanDetail>>;
                    // 圧延計画リスト(加熱炉用）(PMPA280F)のストラテジの生成
                    var writer = new PMPA280FExcelWriter(outputDocument.DocumentOperationContainer)
                                    .Chain(x => x.TemplateWorkBookPath = outputDocument.TemplateBookPath)
                                    .Chain(x => x.OutputWorkBookPath = outputDocument.OutputBookPath)
                                    .Chain(x => x.Year = outputDocument.Year)
                                    .Chain(x => x.Month = outputDocument.Month)
                                    .Chain(x => x.RCNo = outputDocument.RCNo)
                                    .Chain(x => x.DateTimeNow = outputDocument.DateTimeNow);

                    var item = outputDocument.OutputData
                        as RollingPlanDTO<RollingPlanDetail>;

                    fileName = Path.GetFileName(outputDocument.OutputBookPath);
                    logManager.Write("Excel出力開始(" + "ファイル名:" + fileName + ")");

                    // 初期化処理
                    writer.Initialize();
                    // 書込み前処理
                    writer.PreWrite();
                    // ヘッダーの書込み
                    writer.WriteHeader(item.FileHeaderInfo);
                    // 詳細の書込み
                    writer.WriteDetail(item.FileDetailInfo);
                    // フッターの書込み
                    writer.WriteFooter(item.FileFooterInfo);
                    // 書込み後処理
                    writer.PostWrite();
                    // 終了処理
                    writer.Terminate();
                    logManager.Write("Excel出力終了(" + "ファイル名:" + fileName + ")");
                }
            }

            catch (Exception ex)
            {
                logManager.Write("Excel出力異常(" + "ファイル名:" + fileName + "," + "エラーメッセージ" + ex.Message + ")");
                throw;
            }

            return true;
        }

        /// <summary>
        /// 未計画受注リスト(PMPD610F)の出力
        /// </summary>
        /// <param name="document">ドキュメントのDTO</param>
        /// <returns>結果</returns>
        protected internal override bool OutputPMPD610FList<T>(T document)
        {
            string fileName = null;
            
            try
            {
                if (document is PMPD610FExcelOutputDocument<UnPlannedOrdersDTO<UnPlannedOrdersDetail>>)
                {
                    // 未計画受注リスト(PMPD610F)のExcel出力ドキュメントDTOへのキャスト
                    var outputDocument
                        = document as PMPD610FExcelOutputDocument<UnPlannedOrdersDTO<UnPlannedOrdersDetail>>;
                    // 未計画受注リスト(PMPD610F)のストラテジの生成
                    var writer = new PMPD610FExcelWriter(outputDocument.DocumentOperationContainer)
                                    .Chain(x => x.TemplateWorkBookPath = outputDocument.TemplateBookPath)
                                    .Chain(x => x.OutputWorkBookPath = outputDocument.OutputBookPath)
                                    .Chain(x => x.Year = outputDocument.Year)
                                    .Chain(x => x.Month = outputDocument.Month);

                    var item = outputDocument.OutputData
                        as UnPlannedOrdersDTO<UnPlannedOrdersDetail>;

                    fileName = Path.GetFileName(outputDocument.OutputBookPath);
                    logManager.Write("Excel出力開始(" + "ファイル名:" + fileName + ")");

                    // 初期化処理
                    writer.Initialize();
                    // 書込み前処理
                    writer.PreWrite();
                    // ヘッダーの書込み
                    writer.WriteHeader(item.FileHeaderInfo);
                    // 詳細の書込み
                    writer.WriteDetail(item.FileDetailInfo);
                    // フッターの書込み
                    writer.WriteFooter(item.FileFooterInfo);
                    // 書込み後処理
                    writer.PostWrite();
                    // 終了処理
                    writer.Terminate();
                    logManager.Write("Excel出力終了(" + "ファイル名:" + fileName + ")");
                }
            }

            catch (Exception ex)
            {
                logManager.Write("Excel出力異常(" + "ファイル名:" + fileName + "," + "エラーメッセージ" + ex.Message + ")");
                throw;
            }

            return true;
        }

        /// <summary>
        /// 未採取リスト(PMPF070F)の出力
        /// </summary>
        /// <param name="document">ドキュメントのDTO</param>
        /// <returns>結果</returns>
        protected internal override bool OutputPMPF070FList<T>(T document)
        {
            string fileName = null;
            
            try
            {
                if (document is PMPF070FExcelOutputDocument<UnHarvestingDTO<UnHarvestingDetail>>)
                {
                    // 未採取リスト(PMPF070F)のExcel出力ドキュメントDTOへのキャスト
                    var outputDocument
                        = document as PMPF070FExcelOutputDocument<UnHarvestingDTO<UnHarvestingDetail>>;
                    // 未採取リスト(PMPF070F)のストラテジの生成
                    var writer = new PMPF070FExcelWriter(outputDocument.DocumentOperationContainer)
                                    .Chain(x => x.TemplateWorkBookPath = outputDocument.TemplateBookPath)
                                    .Chain(x => x.OutputWorkBookPath = outputDocument.OutputBookPath)
                                    .Chain(x => x.Year = outputDocument.Year)
                                    .Chain(x => x.Month = outputDocument.Month);

                    var item = outputDocument.OutputData
                        as UnHarvestingDTO<UnHarvestingDetail>;

                    fileName = Path.GetFileName(outputDocument.OutputBookPath);
                    logManager.Write("Excel出力開始(" + "ファイル名:" + fileName + ")");

                    // 初期化処理
                    writer.Initialize();
                    // 書込み前処理
                    writer.PreWrite();
                    // ヘッダーの書込み
                    writer.WriteHeader(item.FileHeaderInfo);
                    // 詳細の書込み
                    writer.WriteDetail(item.FileDetailInfo);
                    // フッターの書込み
                    writer.WriteFooter(item.FileFooterInfo);
                    // 書込み後処理
                    writer.PostWrite();
                    // 終了処理
                    writer.Terminate();
                    logManager.Write("Excel出力終了(" + "ファイル名:" + fileName + ")");
                }
            }

            catch (Exception ex)
            {
                logManager.Write("Excel出力異常(" + "ファイル名:" + fileName + "," + "エラーメッセージ" + ex.Message + ")");
                throw;
            }

            return true;
        }

        /// <summary>
        /// 余剰品引当可能在庫リスト(PMPF090F)の出力
        /// </summary>
        /// <param name="document">ドキュメントのDTO</param>
        /// <returns>結果</returns>
        protected internal override bool OutputPMPF090FList<T>(T document)
        {
            string fileName = null;
            
            try
            {
                if (document is PMPF090FExcelOutputDocument<SurplusGoodsProvisionpPossibleStockDTO<SurplusGoodsProvisionpPossibleStockDetail>>)
                {
                    // 余剰品引当可能在庫リスト(PMPF090F)のExcel出力ドキュメントDTOへのキャスト
                    var outputDocument
                        = document as PMPF090FExcelOutputDocument<SurplusGoodsProvisionpPossibleStockDTO<SurplusGoodsProvisionpPossibleStockDetail>>;
                    // 余剰品引当可能在庫リスト(PMPF090F)のストラテジの生成
                    var writer = new PMPF090FExcelWriter(outputDocument.DocumentOperationContainer)
                                    .Chain(x => x.TemplateWorkBookPath = outputDocument.TemplateBookPath)
                                    .Chain(x => x.OutputWorkBookPath = outputDocument.OutputBookPath)
                                    .Chain(x => x.Year = outputDocument.Year)
                                    .Chain(x => x.Month = outputDocument.Month);

                    var item = outputDocument.OutputData
                        as SurplusGoodsProvisionpPossibleStockDTO<SurplusGoodsProvisionpPossibleStockDetail>;

                    fileName = Path.GetFileName(outputDocument.OutputBookPath);
                    logManager.Write("Excel出力開始(" + "ファイル名:" + fileName + ")");

                    // 初期化処理
                    writer.Initialize();
                    // 書込み前処理
                    writer.PreWrite();
                    // ヘッダーの書込み
                    writer.WriteHeader(item.FileHeaderInfo);
                    // 詳細の書込み
                    writer.WriteDetail(item.FileDetailInfo);
                    // フッターの書込み
                    writer.WriteFooter(item.FileFooterInfo);
                    // 書込み後処理
                    writer.PostWrite();
                    // 終了処理
                    writer.Terminate();
                    logManager.Write("Excel出力終了(" + "ファイル名:" + fileName + ")");
                }
            }

            catch (Exception ex)
            {
                logManager.Write("Excel出力異常(" + "ファイル名:" + fileName + "," + "エラーメッセージ" + ex.Message + ")");
                throw;
            }

            return true;
        }

        /// <summary>
        /// 外販出荷計画リスト(PQGA186F)の出力
        /// </summary>
        /// <param name="document">ドキュメントのDTO</param>
        /// <returns>結果</returns>
        protected internal override bool OutputPQGA186FList<T>(T document)
        {
            string fileName = null;
            
            try
            {
                if (document is PQGA186FExcelOutputDocument<ExternalSalesShipmentPlanDTO<ExternalSalesShipmentPlanDetail>>)
                {
                    // 外販出荷計画リスト(PQGA186F)のExcel出力ドキュメントDTOへのキャスト
                    var outputDocument
                        = document as PQGA186FExcelOutputDocument<ExternalSalesShipmentPlanDTO<ExternalSalesShipmentPlanDetail>>;
                    // 外販出荷計画リスト(PQGA186F)のストラテジの生成
                    var writer = new PQGA186FExcelWriter(outputDocument.DocumentOperationContainer)
                                    .Chain(x => x.TemplateWorkBookPath = outputDocument.TemplateBookPath)
                                    .Chain(x => x.OutputWorkBookPath = outputDocument.OutputBookPath)
                                    .Chain(x => x.Year = outputDocument.Year)
                                    .Chain(x => x.Month = outputDocument.Month);

                    var item = outputDocument.OutputData
                        as ExternalSalesShipmentPlanDTO<ExternalSalesShipmentPlanDetail>;

                    fileName = Path.GetFileName(outputDocument.OutputBookPath);
                    logManager.Write("Excel出力開始(" + "ファイル名:" + fileName + ")");

                    // 初期化処理
                    writer.Initialize();
                    // 書込み前処理
                    writer.PreWrite();
                    // ヘッダーの書込み
                    writer.WriteHeader(item.FileHeaderInfo);
                    // 詳細の書込み
                    writer.WriteDetail(item.FileDetailInfo);
                    // フッターの書込み
                    writer.WriteFooter(item.FileFooterInfo);
                    // 書込み後処理
                    writer.PostWrite();
                    // 終了処理
                    writer.Terminate();
                    logManager.Write("Excel出力終了(" + "ファイル名:" + fileName + ")");
                }
            }

            catch (Exception ex)
            {
                logManager.Write("Excel出力異常(" + "ファイル名:" + fileName + "," + "エラーメッセージ" + ex.Message + ")");
                throw;
            }

            return true;
        }

        /// <summary>
        /// 外販出荷実績報告書(PQGA380F)の出力
        /// </summary>
        /// <param name="document">ドキュメントのDTO</param>
        /// <returns>結果</returns>
        protected internal override bool OutputPQGA380FList<T>(T document)
        {
            string fileName = null;
            
            try
            {
                if (document is PQGA380FExcelOutputDocument<ExternalSalesShipmentPerformanceDTO<ExternalSalesShipmentPerformanceDetail>>)
                {
                    // 外販出荷実績報告書(PQGA380F)のExcel出力ドキュメントDTOへのキャスト
                    var outputDocument
                        = document as PQGA380FExcelOutputDocument<ExternalSalesShipmentPerformanceDTO<ExternalSalesShipmentPerformanceDetail>>;
                    // 外販出荷実績報告書(PQGA380F)のストラテジの生成
                    var writer = new PQGA380FExcelWriter(outputDocument.DocumentOperationContainer)
                                    .Chain(x => x.TemplateWorkBookPath = outputDocument.TemplateBookPath)
                                    .Chain(x => x.OutputWorkBookPath = outputDocument.OutputBookPath)
                                    .Chain(x => x.Year = outputDocument.Year)
                                    .Chain(x => x.Month = outputDocument.Month);

                    var item = outputDocument.OutputData
                        as ExternalSalesShipmentPerformanceDTO<ExternalSalesShipmentPerformanceDetail>;

                    fileName = Path.GetFileName(outputDocument.OutputBookPath);
                    logManager.Write("Excel出力開始(" + "ファイル名:" + fileName + ")");

                    // 初期化処理
                    writer.Initialize();
                    // 書込み前処理
                    writer.PreWrite();
                    // ヘッダーの書込み
                    writer.WriteHeader(item.FileHeaderInfo);
                    // 詳細の書込み
                    writer.WriteDetail(item.FileDetailInfo);
                    // フッターの書込み
                    writer.WriteFooter(item.FileFooterInfo);
                    // 書込み後処理
                    writer.PostWrite();
                    // 終了処理
                    writer.Terminate();
                    logManager.Write("Excel出力終了(" + "ファイル名:" + fileName + ")");
                }
            }

            catch (Exception ex)
            {
                logManager.Write("Excel出力異常(" + "ファイル名:" + fileName + "," + "エラーメッセージ" + ex.Message + ")");
                throw;
            }

            return true;
        }

        /// <summary>
        /// 外販受注残リスト(PQGA420F)の出力
        /// </summary>
        /// <param name="document">ドキュメントのDTO</param>
        /// <returns>結果</returns>
        protected internal override bool OutputPQGA420FList<T>(T document)
        {
            string fileName = null;
            
            try
            {
                if (document is PQGA420FExcelOutputDocument<ExternalSalesBacklogDTO<ExternalSalesBacklogDetail>>)
                {
                    // 外販受注残リスト(PQGA420F)のExcel出力ドキュメントDTOへのキャスト
                    var outputDocument
                        = document as PQGA420FExcelOutputDocument<ExternalSalesBacklogDTO<ExternalSalesBacklogDetail>>;
                    // 外販受注残リスト(PQGA420F)のストラテジの生成
                    var writer = new PQGA420FExcelWriter(outputDocument.DocumentOperationContainer)
                                    .Chain(x => x.TemplateWorkBookPath = outputDocument.TemplateBookPath)
                                    .Chain(x => x.OutputWorkBookPath = outputDocument.OutputBookPath)
                                    .Chain(x => x.Year = outputDocument.Year)
                                    .Chain(x => x.Month = outputDocument.Month);

                    var item = outputDocument.OutputData
                        as ExternalSalesBacklogDTO<ExternalSalesBacklogDetail>;

                    fileName = Path.GetFileName(outputDocument.OutputBookPath);
                    logManager.Write("Excel出力開始(" + "ファイル名:" + fileName + ")");

                    // 初期化処理
                    writer.Initialize();
                    // 書込み前処理
                    writer.PreWrite();
                    // ヘッダーの書込み
                    writer.WriteHeader(item.FileHeaderInfo);
                    // 詳細の書込み
                    writer.WriteDetail(item.FileDetailInfo);
                    // フッターの書込み
                    writer.WriteFooter(item.FileFooterInfo);
                    // 書込み後処理
                    writer.PostWrite();
                    // 終了処理
                    writer.Terminate();
                    logManager.Write("Excel出力終了(" + "ファイル名:" + fileName + ")");
                }
            }

            catch (Exception ex)
            {
                logManager.Write("Excel出力異常(" + "ファイル名:" + fileName + "," + "エラーメッセージ" + ex.Message + ")");
                throw;
            }

            return true;
        }

        /// <summary>
        /// 外販出荷計画リスト(PQGA820F)の出力
        /// </summary>
        /// <param name="document">ドキュメントのDTO</param>
        /// <returns>結果</returns>
        protected internal override bool OutputPQGA820FList<T>(T document)
        {
            string fileName = null;
            
            try
            {
                if (document is PQGA820FExcelOutputDocument<ExternalStandardByAggregateDTO<ExternalStandardByAggregateDetail>>)
                {
                    // 外販出荷計画リスト(PQGA820F)のExcel出力ドキュメントDTOへのキャスト
                    var outputDocument
                        = document as PQGA820FExcelOutputDocument<ExternalStandardByAggregateDTO<ExternalStandardByAggregateDetail>>;
                    // 外販出荷計画リスト(PQGA820F)のストラテジの生成
                    var writer = new PQGA820FExcelWriter(outputDocument.DocumentOperationContainer)
                                    .Chain(x => x.TemplateWorkBookPath = outputDocument.TemplateBookPath)
                                    .Chain(x => x.OutputWorkBookPath = outputDocument.OutputBookPath)
                                    .Chain(x => x.Year = outputDocument.Year)
                                    .Chain(x => x.Month = outputDocument.Month);

                    var item = outputDocument.OutputData
                        as ExternalStandardByAggregateDTO<ExternalStandardByAggregateDetail>;

                    fileName = Path.GetFileName(outputDocument.OutputBookPath);
                    logManager.Write("Excel出力開始(" + "ファイル名:" + fileName + ")");

                    // 初期化処理
                    writer.Initialize();
                    // 書込み前処理
                    writer.PreWrite();
                    // ヘッダーの書込み
                    writer.WriteHeader(item.FileHeaderInfo);
                    // 詳細の書込み
                    writer.WriteDetail(item.FileDetailInfo);
                    // フッターの書込み
                    writer.WriteFooter(item.FileFooterInfo);
                    // 書込み後処理
                    writer.PostWrite();
                    // 終了処理
                    writer.Terminate();
                    logManager.Write("Excel出力終了(" + "ファイル名:" + fileName + ")");
                }
            }

            catch (Exception ex)
            {
                logManager.Write("Excel出力異常(" + "ファイル名:" + fileName + "," + "エラーメッセージ" + ex.Message + ")");
                throw;
            }

            return true;
        }

        /// <summary>
        /// 発生屑・単板・伸鉄払出明細(SSYM040F)の出力
        /// </summary>
        /// <param name="document">ドキュメントのDTO</param>
        /// <returns>結果</returns>
        protected internal override bool OutputSSYM040FList<T>(T document)
        {
            string fileName = null;
            
            try
            {
                if (document is SSYM040FExcelOutputDocument<GeneratedWaste_SinglePlate_SinTetsuPayoutDTO<GeneratedWaste_SinglePlate_SinTetsuPayoutDetail>>)
                {
                    // 発生屑・単板・伸鉄払出明細(SSYM040F)のExcel出力ドキュメントDTOへのキャスト
                    var outputDocument
                        = document as SSYM040FExcelOutputDocument<GeneratedWaste_SinglePlate_SinTetsuPayoutDTO<GeneratedWaste_SinglePlate_SinTetsuPayoutDetail>>;
                    // 発生屑・単板・伸鉄払出明細(SSYM040F)のストラテジの生成
                    var writer = new SSYM040FExcelWriter(outputDocument.DocumentOperationContainer)
                                    .Chain(x => x.TemplateWorkBookPath = outputDocument.TemplateBookPath)
                                    .Chain(x => x.OutputWorkBookPath = outputDocument.OutputBookPath)
                                    .Chain(x => x.Year = outputDocument.Year)
                                    .Chain(x => x.Month = outputDocument.Month);

                    var item = outputDocument.OutputData
                        as GeneratedWaste_SinglePlate_SinTetsuPayoutDTO<GeneratedWaste_SinglePlate_SinTetsuPayoutDetail>;

                    fileName = Path.GetFileName(outputDocument.OutputBookPath);
                    logManager.Write("Excel出力開始(" + "ファイル名:" + fileName + ")");
                    
                    // 初期化処理
                    writer.Initialize();
                    // 書込み前処理
                    writer.PreWrite();
                    // ヘッダーの書込み
                    writer.WriteHeader(item.FileHeaderInfo);
                    // 詳細の書込み
                    writer.WriteDetail(item.FileDetailInfo);
                    // フッターの書込み
                    writer.WriteFooter(item.FileFooterInfo);
                    // 書込み後処理
                    writer.PostWrite();
                    // 終了処理
                    writer.Terminate();
                    logManager.Write("Excel出力終了(" + "ファイル名:" + fileName + ")");
                }
            }

            catch (Exception ex)
            {
                logManager.Write("Excel出力異常(" + "ファイル名:" + fileName + "," + "エラーメッセージ" + ex.Message + ")");
                throw;
            }

            return true;
        }

        /// <summary>
        /// スクラップ外販明細(SSYM050F)の出力
        /// </summary>
        /// <param name="document">ドキュメントのDTO</param>
        /// <returns>結果</returns>
        protected internal override bool OutputSSYM050FList<T>(T document)
        {
            string fileName = null;
            
            try
            {
                if (document is SSYM050FExcelOutputDocument<ScrapExternalSalesDTO<ScrapExternalSalesDetail>>)
                {
                    // スクラップ外販明細(SSYM050F)のExcel出力ドキュメントDTOへのキャスト
                    var outputDocument
                        = document as SSYM050FExcelOutputDocument<ScrapExternalSalesDTO<ScrapExternalSalesDetail>>;
                    // スクラップ外販明細(SSYM050F)のストラテジの生成
                    var writer = new SSYM050FExcelWriter(outputDocument.DocumentOperationContainer)
                                    .Chain(x => x.TemplateWorkBookPath = outputDocument.TemplateBookPath)
                                    .Chain(x => x.OutputWorkBookPath = outputDocument.OutputBookPath)
                                    .Chain(x => x.Year = outputDocument.Year)
                                    .Chain(x => x.Month = outputDocument.Month);

                    var item = outputDocument.OutputData
                        as ScrapExternalSalesDTO<ScrapExternalSalesDetail>;

                    fileName = Path.GetFileName(outputDocument.OutputBookPath);
                    logManager.Write("Excel出力開始(" + "ファイル名:" + fileName + ")");
                    
                    // 初期化処理
                    writer.Initialize();
                    // 書込み前処理
                    writer.PreWrite();
                    // ヘッダーの書込み
                    writer.WriteHeader(item.FileHeaderInfo);
                    // 詳細の書込み
                    writer.WriteDetail(item.FileDetailInfo);
                    // フッターの書込み
                    writer.WriteFooter(item.FileFooterInfo);
                    // 書込み後処理
                    writer.PostWrite();
                    // 終了処理
                    writer.Terminate();
                    logManager.Write("Excel出力終了(" + "ファイル名:" + fileName + ")");
                }
            }

            catch (Exception ex)
            {
                logManager.Write("Excel出力異常(" + "ファイル名:" + fileName + "," + "エラーメッセージ" + ex.Message + ")");
                throw;
            }

            return true;
        }

        /// <summary>
        /// 在庫売り受注一覧表(SSZA400B)の出力
        /// </summary>
        /// <param name="document">ドキュメントのDTO</param>
        /// <returns>結果</returns>
        protected internal override bool OutputSSZA400BList<T>(T document)
        {
            string fileName = null;
            
            try
            {
                if (document is SSZA400BExcelOutputDocument<StockSellOrdersDTO<StockSellOrdersDetail>>)
                {
                    // 在庫売り受注一覧表(SSZA400B)のExcel出力ドキュメントDTOへのキャスト
                    var outputDocument
                        = document as SSZA400BExcelOutputDocument<StockSellOrdersDTO<StockSellOrdersDetail>>;
                    // 在庫売り受注一覧表(SSZA400B)のストラテジの生成
                    var writer = new SSZA400BExcelWriter(outputDocument.DocumentOperationContainer)
                                    .Chain(x => x.TemplateWorkBookPath = outputDocument.TemplateBookPath)
                                    .Chain(x => x.OutputWorkBookPath = outputDocument.OutputBookPath)
                                    .Chain(x => x.Year = outputDocument.Year)
                                    .Chain(x => x.Month = outputDocument.Month);

                    var item = outputDocument.OutputData
                        as StockSellOrdersDTO<StockSellOrdersDetail>;

                    fileName = Path.GetFileName(outputDocument.OutputBookPath);
                    logManager.Write("Excel出力開始(" + "ファイル名:" + fileName + ")");
                    
                    // 初期化処理
                    writer.Initialize();
                    // 書込み前処理
                    writer.PreWrite();
                    // ヘッダーの書込み
                    writer.WriteHeader(item.FileHeaderInfo);
                    // 詳細の書込み
                    writer.WriteDetail(item.FileDetailInfo);
                    // フッターの書込み
                    writer.WriteFooter(item.FileFooterInfo);
                    // 書込み後処理
                    writer.PostWrite();
                    // 終了処理
                    writer.Terminate();
                    logManager.Write("Excel出力終了(" + "ファイル名:" + fileName + ")");
                }
            }

            catch (Exception ex)
            {
                logManager.Write("Excel出力異常(" + "ファイル名:" + fileName + "," + "エラーメッセージ" + ex.Message + ")");
                throw;
            }

            return true;
        }

        #endregion
    }
}
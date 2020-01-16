//*************************************************************************************
//
//   操作のユーティリティークラス
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using Document.Types;
using FileManager.Reader.Interface;
using Generator.Interface;
using Model;
using Model.Base;
using Utility.Core;
using Utility.Types;
using ProductionControlDailyExcelCreator.Shared;
using ProductionControlDailyExcelCreator.Document;
using ProductionControlDailyExcelCreator.DTO;
using ProductionControlDailyExcelCreator.FileReader;
using ProductionControlDailyExcelCreator.Generator.DTO;
using ProductionControlDailyExcelCreator.Generator.Model;
using ProductionControlDailyExcelCreator.Types;
using System.IO;
using ProductionControlDailyExcelCreator.Common.Container;

namespace ProductionControlDailyExcelCreator.Operation
{
    /// <summary>
    /// 操作のユーティリティークラス
    /// </summary>
    internal class OperationUtil
    {
        #region メソッド

        /// <summary>
        /// ファイルの読込み
        /// </summary>
        /// <param name="modelType">モデルの種類(月報)</param>
        /// <param name="extensionType">拡張子の種類</param>
        /// <param name="filePath">読込みファイルパス</param>
        /// <param name="settingFilePath">読込み用設定ファイルパス</param>
        internal static IList<T> ReadFile<T>(ModelType modelType,
                                             ExtensionType extensionType,
                                             string readFilePath, 
                                             string readSettingFilePath) where T : ModelBase
        {
            IFileReader fileReader = null;
            IModelGenerator modelGenerator = null;

            try
            {
                fileReader = FileReadProvidor.GetFileReader(modelType, extensionType);
                fileReader.Read(readFilePath, readSettingFilePath);
                fileReader.Validate();
                modelGenerator = GenerateModelProvidor.GetModelGenerator(modelType, fileReader);
                return modelGenerator.GenerateList<T>();
            }

            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Excelの出力
        /// </summary>
        /// <param name="modelType">モデルの種類</param>
        /// <param name="templateBookPath">テンプレートExcelブックのパス</param>
        /// <param name="outputBookPath">出力Excelブックのパス</param>
        /// <param name="container">ドキュメントに対する操作完了時のコンテナ</param>
        /// <returns>結果</returns>
        internal static bool OutputExcel(ModelType modelType,
                                         string templateBookPath,
                                         string outputBookPath,
                                         IDocumentOperationContainer container)
        {
             
            try
            {
                switch(modelType)
                {
                    case ModelType.PMGQ010F:
                        // 技術特殊引合品チェックリスト(PMGQ010F)の出力
                        return OutputPMGQ010FList(templateBookPath, outputBookPath, container);
                    case ModelType.PMPA280F:
                        // 圧延計画リスト(加熱炉用）(PMPA280F)の出力
                        return OutputPMPA280FList(templateBookPath, outputBookPath, container);
                    case ModelType.PMPD610F:
                        // 未計画受注リスト(PMPD610F)の出力
                        return OutputPMPD610FList(templateBookPath, outputBookPath, container);
                    case ModelType.PMPF070F:
                        // 未採取リスト(PMPF070F)の出力
                        return OutputPMPF070FList(templateBookPath, outputBookPath, container);
                    case ModelType.PMPF090F:
                        // 製鋼月報作成用リスト(PMPF090F)の出力
                        return OutputPMPF090FList(templateBookPath, outputBookPath, container);
                    case ModelType.PQGA186F:
                        // 外販出荷計画リスト(PQGA186F)の出力
                        return OutputPQGA186FList(templateBookPath, outputBookPath, container);
                    case ModelType.PQGA380F:
                        // 外販出荷実績報告書(PQGA380F)の出力
                        return OutputPQGA380FList(templateBookPath, outputBookPath, container);
                    case ModelType.PQGA420F:
                        // 外販受注残リスト(PQGA420F)の出力
                        return OutputPQGA420FList(templateBookPath, outputBookPath, container);
                    case ModelType.PQGA820F:
                        // 外販出荷計画リスト(PQGA820F)の出力
                        return OutputPQGA820FList(templateBookPath, outputBookPath, container);
                    case ModelType.SSYM040F:
                        // 発生屑・単板・伸鉄払出明細(SSYM040F)の出力
                        return OutputSSYM040FList(templateBookPath, outputBookPath, container);
                    case ModelType.SSYM050F:
                        // スクラップ外販明細(SSYM050F)の出力
                        return OutputSSYM050FList(templateBookPath, outputBookPath, container);
                    case ModelType.SSZA400B:
                        // 在庫売り受注一覧表(SSZA400B)の出力
                        return OutputSSZA400BList(templateBookPath, outputBookPath, container);
                    default:
                        return false;
                }
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 技術特殊引合品チェックリスト(PMGQ010F)の出力
        /// </summary>
        /// <param name="templateBookPath">テンプレートExcelブックのパス</param>
        /// <param name="outputBookPath">出力Excelブックのパス</param>
        /// <param name="container">ドキュメントに対する操作完了時のコンテナ</param>
        /// <returns>結果</returns>
        internal static bool OutputPMGQ010FList(string templateBookPath,
                                                string outputBookPath,
                                                IDocumentOperationContainer container)
        {
            try
            {
                var target = new TechnologySpecialInquiriesGoodsDTO<TechnologySpecialInquiriesGoodsDetail>();
                var details = new List<TechnologySpecialInquiriesGoodsDetail>();
                var detail = new TechnologySpecialInquiriesGoodsDetail();
                // 技術特殊引合品のリスト
                detail.PMGQ010BDTOList
                    = new PMGQ010BDTOGenerator()
                        .GenerateList<PMGQ010BModel, PMGQ010BDTO>(AppSharedModel.Instance.PMGQ010BModels);

                if (detail.PMGQ010BDTOList.Count > 0)
                {
                    details.Add(detail);
                    target.FileDetailInfo = details;
                    // Excel出力ドキュメントDTOの生成
                    var outputDocument
                        = new PMGQ010FExcelOutputDocument
                            <TechnologySpecialInquiriesGoodsDTO<TechnologySpecialInquiriesGoodsDetail>>(target)
                                .Chain(x => x.TemplateBookPath = templateBookPath)
                                .Chain(x => x.OutputBookPath = outputBookPath)
                                .Chain(x => x.Year = AppSharedModel.Instance.Year)
                                .Chain(x => x.Month = AppSharedModel.Instance.Month)
                                .Chain(x => x.DocumentOperationContainer = container);
                    // ドキュメントマネージャ(Excel操作用)の生成
                    DocumentManager docManager
                        = new DocumentManager(DocumentType.Excel);
                    // 技術特殊引合品チェックリスト(PMGQ010F)の出力
                    return docManager.OutputPMGQ010FList(outputDocument);
                }
                else
                {
                    return false;
                }
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 圧延計画リスト(加熱炉用）(PMPA280F)の出力
        /// </summary>
        /// <param name="templateBookPath">テンプレートExcelブックのパス</param>
        /// <param name="outputBookPath">出力Excelブックのパス</param>
        /// <param name="container">ドキュメントに対する操作完了時のコンテナ</param>
        /// <returns>結果</returns>
        internal static bool OutputPMPA280FList(string templateBookPath,
                                                string outputBookPath,
                                                IDocumentOperationContainer container)
        {
            try
            {
                // 圧延計画のリスト
                var pmpa260BTODList
                    = new PMPA260BDTOGenerator().GenerateList<PMPA260BModel, PMPA260BDTO>(AppSharedModel.Instance.PMPA260BModels);

                if (pmpa260BTODList.Count > 0)
                {
                    var groupList = pmpa260BTODList.GroupBy(item => item.RCNO);
                    var groupCount = groupList.Count();
                    if (groupCount >= 1)
                    {
                        var dateTimeNow = DateTime.Now;
                        foreach (var group in groupList)
                        {
                            var target = new RollingPlanDTO<RollingPlanDetail>();
                            var details = new List<RollingPlanDetail>();
                            var detail = new RollingPlanDetail();
                            detail.PMPA260BDTOList = new List<PMPA260BDTO>();
                            detail.PMPA260BDTOList.AddRange(group.ToList());
                            details.Add(detail);

                            target.FileDetailInfo = details;
                            var outputDocument
                            = new PMPA280FExcelOutputDocument
                                <RollingPlanDTO<RollingPlanDetail>>(target)
                                    .Chain(x => x.TemplateBookPath = templateBookPath)
                                    .Chain(x => x.OutputBookPath = outputBookPath)
                                    .Chain(x => x.Year = AppSharedModel.Instance.Year)
                                    .Chain(x => x.Month = AppSharedModel.Instance.Month)
                                    .Chain(x => x.RCNo = group.Key)
                                    .Chain(x => x.DateTimeNow = dateTimeNow)
                                    .Chain(x => x.DocumentOperationContainer = container);
                            // ドキュメントマネージャ(Excel操作用)の生成
                            DocumentManager docManager
                                = new DocumentManager(DocumentType.Excel);
                            // 圧延計画リスト(加熱炉用）(PMPA280F)の出力
                            docManager.OutputPMPA280FList(outputDocument);
                        }

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 未計画受注リスト(PMPD610F)の出力
        /// </summary>
        /// <param name="templateBookPath">テンプレートExcelブックのパス</param>
        /// <param name="outputBookPath">出力Excelブックのパス</param>
        /// <param name="container">ドキュメントに対する操作完了時のコンテナ</param>
        /// <returns>結果</returns>
        internal static bool OutputPMPD610FList(string templateBookPath,
                                                string outputBookPath,
                                                IDocumentOperationContainer container)
        {
            try
            {
                var target = new UnPlannedOrdersDTO<UnPlannedOrdersDetail>();
                var details = new List<UnPlannedOrdersDetail>();
                var detail = new UnPlannedOrdersDetail();
                // 未計画受注のリスト
                detail.PMPD330BDTOList
                    = new PMPD330BDTOGenerator()
                        .GenerateList<PMPD330BModel, PMPD330BDTO>(AppSharedModel.Instance.PMPD330BModels);

                if (detail.PMPD330BDTOList.Count > 0)
                {
                    details.Add(detail);
                    target.FileDetailInfo = details;
                    // Excel出力ドキュメントDTOの生成
                    var outputDocument
                        = new PMPD610FExcelOutputDocument
                            <UnPlannedOrdersDTO<UnPlannedOrdersDetail>>(target)
                                .Chain(x => x.TemplateBookPath = templateBookPath)
                                .Chain(x => x.OutputBookPath = outputBookPath)
                                .Chain(x => x.Year = AppSharedModel.Instance.Year)
                                .Chain(x => x.Month = AppSharedModel.Instance.Month)
                                .Chain(x => x.DocumentOperationContainer = container);
                    // ドキュメントマネージャ(Excel操作用)の生成
                    DocumentManager docManager
                        = new DocumentManager(DocumentType.Excel);
                    // 未計画受注リスト(PMPD610F)の出力
                    return docManager.OutputPMPD610FList(outputDocument);
                }
                else
                {
                    return false;
                }
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 未採取リスト(PMPF070F)の出力
        /// </summary>
        /// <param name="templateBookPath">テンプレートExcelブックのパス</param>
        /// <param name="outputBookPath">出力Excelブックのパス</param>
        /// <param name="container">ドキュメントに対する操作完了時のコンテナ</param>
        /// <returns>結果</returns>
        internal static bool OutputPMPF070FList(string templateBookPath,
                                                string outputBookPath,
                                                IDocumentOperationContainer container)
        {
            try
            {
                var target = new UnHarvestingDTO<UnHarvestingDetail>();
                var details = new List<UnHarvestingDetail>();
                var detail = new UnHarvestingDetail();
                // 未採取リスト
                detail.PMPF070BDTOList
                    = new PMPF070BDTOGenerator()
                        .GenerateList<PMPF070BModel, PMPF070BDTO>(AppSharedModel.Instance.PMPF070BModels);

                if (detail.PMPF070BDTOList.Count > 0)
                {
                    details.Add(detail);
                    target.FileDetailInfo = details;
                    // Excel出力ドキュメントDTOの生成
                    var outputDocument
                        = new PMPF070FExcelOutputDocument
                            <UnHarvestingDTO<UnHarvestingDetail>>(target)
                                .Chain(x => x.TemplateBookPath = templateBookPath)
                                .Chain(x => x.OutputBookPath = outputBookPath)
                                .Chain(x => x.Year = AppSharedModel.Instance.Year)
                                .Chain(x => x.Month = AppSharedModel.Instance.Month)
                                .Chain(x => x.DocumentOperationContainer = container);
                    // ドキュメントマネージャ(Excel操作用)の生成
                    DocumentManager docManager
                        = new DocumentManager(DocumentType.Excel);
                    // 未採取リスト(PMPF070F)の出力
                    return docManager.OutputPMPF070FList(outputDocument);
                }
                else
                {
                    return false;
                }
            }

            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// 余剰品引当可能在庫リスト(PMPF090F)の出力
        /// </summary>
        /// <param name="templateBookPath">テンプレートExcelブックのパス</param>
        /// <param name="outputBookPath">出力Excelブックのパス</param>
        /// <param name="container">ドキュメントに対する操作完了時のコンテナ</param>
        /// <returns>結果</returns>
        internal static bool OutputPMPF090FList(string templateBookPath,
                                                string outputBookPath,
                                                IDocumentOperationContainer container)
        {
            try
            {
                var target = new SurplusGoodsProvisionpPossibleStockDTO<SurplusGoodsProvisionpPossibleStockDetail>();
                var details = new List<SurplusGoodsProvisionpPossibleStockDetail>();
                var detail = new SurplusGoodsProvisionpPossibleStockDetail();
                // 余剰品引当可能在庫のリスト
                detail.PMPF090BDTOList
                    = new PMPF090BDTOGenerator()
                        .GenerateList<PMPF090BModel, PMPF090BDTO>(AppSharedModel.Instance.PMPF090BModels);

                if (detail.PMPF090BDTOList.Count > 0)
                {
                    details.Add(detail);
                    target.FileDetailInfo = details;
                    // Excel出力ドキュメントDTOの生成
                    var outputDocument
                        = new PMPF090FExcelOutputDocument
                            <SurplusGoodsProvisionpPossibleStockDTO<SurplusGoodsProvisionpPossibleStockDetail>>(target)
                                .Chain(x => x.TemplateBookPath = templateBookPath)
                                .Chain(x => x.OutputBookPath = outputBookPath)
                                .Chain(x => x.Year = AppSharedModel.Instance.Year)
                                .Chain(x => x.Month = AppSharedModel.Instance.Month)
                                .Chain(x => x.DocumentOperationContainer = container);
                    // ドキュメントマネージャ(Excel操作用)の生成
                    DocumentManager docManager
                        = new DocumentManager(DocumentType.Excel);
                    // 余剰品引当可能在庫リスト(PMPF090F)の出力
                    return docManager.OutputPMPF090FList(outputDocument);
                }
                else
                {
                    return false;
                }
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 外販出荷計画リスト(PQGA186F)の出力
        /// </summary>
        /// <param name="templateBookPath">テンプレートExcelブックのパス</param>
        /// <param name="outputBookPath">出力Excelブックのパス</param>
        /// <param name="container">ドキュメントに対する操作完了時のコンテナ</param>
        /// <returns>結果</returns>
        internal static bool OutputPQGA186FList(string templateBookPath,
                                                string outputBookPath,
                                                IDocumentOperationContainer container)
        {
            try
            {
                var target = new ExternalSalesShipmentPlanDTO<ExternalSalesShipmentPlanDetail>();
                var details = new List<ExternalSalesShipmentPlanDetail>();
                var detail = new ExternalSalesShipmentPlanDetail();
                // 外販出荷計画のリスト
                detail.PQGA186BDTOList
                    = new PQGA186BDTOGenerator()
                        .GenerateList<PQGA186BModel, PQGA186BDTO>(AppSharedModel.Instance.PQGA186BModels);

                if (detail.PQGA186BDTOList.Count > 0)
                {
                    details.Add(detail);
                    target.FileDetailInfo = details;
                    // Excel出力ドキュメントDTOの生成
                    var outputDocument
                        = new PQGA186FExcelOutputDocument
                            <ExternalSalesShipmentPlanDTO<ExternalSalesShipmentPlanDetail>>(target)
                                .Chain(x => x.TemplateBookPath = templateBookPath)
                                .Chain(x => x.OutputBookPath = outputBookPath)
                                .Chain(x => x.Year = AppSharedModel.Instance.Year)
                                .Chain(x => x.Month = AppSharedModel.Instance.Month)
                                .Chain(x => x.DocumentOperationContainer = container);
                    // ドキュメントマネージャ(Excel操作用)の生成
                    DocumentManager docManager
                        = new DocumentManager(DocumentType.Excel);
                    // 外販出荷計画リスト(PQGA186F)の出力
                    return docManager.OutputPQGA186FList(outputDocument);
                }
                else
                {
                    return false;
                }
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 外販出荷実績報告書(PQGA380F)の出力
        /// </summary>
        /// <param name="templateBookPath">テンプレートExcelブックのパス</param>
        /// <param name="outputBookPath">出力Excelブックのパス</param>
        /// <param name="container">ドキュメントに対する操作完了時のコンテナ</param>
        /// <returns>結果</returns>
        internal static bool OutputPQGA380FList(string templateBookPath,
                                                string outputBookPath,
                                                IDocumentOperationContainer container)
        {
            try
            {
                var target = new ExternalSalesShipmentPerformanceDTO<ExternalSalesShipmentPerformanceDetail>();
                var details = new List<ExternalSalesShipmentPerformanceDetail>();
                var detail = new ExternalSalesShipmentPerformanceDetail();
                // 外販出荷計画のリスト
                detail.PQGA380BDTOList
                    = new PQGA380BDTOGenerator()
                        .GenerateList<PQGA380BModel, PQGA380BDTO>(AppSharedModel.Instance.PQGA380BModels);

                if (detail.PQGA380BDTOList.Count > 0)
                {
                    details.Add(detail);
                    target.FileDetailInfo = details;
                    // Excel出力ドキュメントDTOの生成
                    var outputDocument
                        = new PQGA380FExcelOutputDocument
                            <ExternalSalesShipmentPerformanceDTO<ExternalSalesShipmentPerformanceDetail>>(target)
                                .Chain(x => x.TemplateBookPath = templateBookPath)
                                .Chain(x => x.OutputBookPath = outputBookPath)
                                .Chain(x => x.Year = AppSharedModel.Instance.Year)
                                .Chain(x => x.Month = AppSharedModel.Instance.Month)
                                .Chain(x => x.DocumentOperationContainer = container);
                    // ドキュメントマネージャ(Excel操作用)の生成
                    DocumentManager docManager
                        = new DocumentManager(DocumentType.Excel);
                    // 外販出荷実績報告書(PQGA380F)の出力
                    return docManager.OutputPQGA380FList(outputDocument);
                }
                else
                {
                    return false;
                }
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 外販受注残リスト(PQGA420F)の出力
        /// </summary>
        /// <param name="templateBookPath">テンプレートExcelブックのパス</param>
        /// <param name="outputBookPath">出力Excelブックのパス</param>
        /// <param name="container">ドキュメントに対する操作完了時のコンテナ</param>
        /// <returns>結果</returns>
        internal static bool OutputPQGA420FList(string templateBookPath,
                                                string outputBookPath,
                                                IDocumentOperationContainer container)
        {
            try
            {
                var target = new ExternalSalesBacklogDTO<ExternalSalesBacklogDetail>();
                var details = new List<ExternalSalesBacklogDetail>();
                var detail = new ExternalSalesBacklogDetail();
                // 外販受注残のリスト
                detail.PQGA420BDTOList
                    = new PQGA420BDTOGenerator()
                        .GenerateList<PQGA420BModel, PQGA420BDTO>(AppSharedModel.Instance.PQGA420BModels);

                if (detail.PQGA420BDTOList.Count > 0)
                {
                    details.Add(detail);
                    target.FileDetailInfo = details;
                    // Excel出力ドキュメントDTOの生成
                    var outputDocument
                        = new PQGA420FExcelOutputDocument
                            <ExternalSalesBacklogDTO<ExternalSalesBacklogDetail>>(target)
                                .Chain(x => x.TemplateBookPath = templateBookPath)
                                .Chain(x => x.OutputBookPath = outputBookPath)
                                .Chain(x => x.Year = AppSharedModel.Instance.Year)
                                .Chain(x => x.Month = AppSharedModel.Instance.Month)
                                .Chain(x => x.DocumentOperationContainer = container);
                    // ドキュメントマネージャ(Excel操作用)の生成
                    DocumentManager docManager
                        = new DocumentManager(DocumentType.Excel);
                    // 外販受注残リスト(PQGA420F)の出力
                    return docManager.OutputPQGA420FList(outputDocument);
                }
                else
                {
                    return false;
                }
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 外販出荷計画リスト(PQGA820F)の出力
        /// </summary>
        /// <param name="templateBookPath">テンプレートExcelブックのパス</param>
        /// <param name="outputBookPath">出力Excelブックのパス</param>
        /// <param name="container">ドキュメントに対する操作完了時のコンテナ</param>
        /// <returns>結果</returns>
        internal static bool OutputPQGA820FList(string templateBookPath,
                                                string outputBookPath,
                                                IDocumentOperationContainer container)
        {
            try
            {
                var target = new ExternalStandardByAggregateDTO<ExternalStandardByAggregateDetail>();
                var details = new List<ExternalStandardByAggregateDetail>();
                var detail = new ExternalStandardByAggregateDetail();
                // 外販規格別集計のリスト
                detail.PQGA820BDTOList
                    = new PQGA820BDTOGenerator()
                        .GenerateList<PQGA820BModel, PQGA820BDTO>(AppSharedModel.Instance.PQGA820BModels);

                if (detail.PQGA820BDTOList.Count > 0)
                {
                    details.Add(detail);
                    target.FileDetailInfo = details;
                    // Excel出力ドキュメントDTOの生成
                    var outputDocument
                        = new PQGA820FExcelOutputDocument
                            <ExternalStandardByAggregateDTO<ExternalStandardByAggregateDetail>>(target)
                                .Chain(x => x.TemplateBookPath = templateBookPath)
                                .Chain(x => x.OutputBookPath = outputBookPath)
                                .Chain(x => x.Year = AppSharedModel.Instance.Year)
                                .Chain(x => x.Month = AppSharedModel.Instance.Month)
                                .Chain(x => x.DocumentOperationContainer = container);
                    // ドキュメントマネージャ(Excel操作用)の生成
                    DocumentManager docManager
                        = new DocumentManager(DocumentType.Excel);
                    // 外販出荷計画リスト(PQGA820F)の出力
                    return docManager.OutputPQGA820FList(outputDocument);
                }
                else
                {
                    return false;
                }
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 発生屑・単板・神鉄払出明細(SSYM040F)の出力
        /// </summary>
        /// <param name="templateBookPath">テンプレートExcelブックのパス</param>
        /// <param name="outputBookPath">出力Excelブックのパス</param>
        /// <param name="container">ドキュメントに対する操作完了時のコンテナ</param>
        /// <returns>結果</returns>
        internal static bool OutputSSYM040FList(string templateBookPath,
                                                string outputBookPath,
                                                IDocumentOperationContainer container)
        {
            try
            {
                var target = new GeneratedWaste_SinglePlate_SinTetsuPayoutDTO<GeneratedWaste_SinglePlate_SinTetsuPayoutDetail>();
                var details = new List<GeneratedWaste_SinglePlate_SinTetsuPayoutDetail>();
                var detail = new GeneratedWaste_SinglePlate_SinTetsuPayoutDetail();
                // 発生屑・単板・神鉄払出明細のリスト
                detail.SSYM040BDTOList
                    = new SSYM040BDTOGenerator()
                        .GenerateList<SSYM040BModel, SSYM040BDTO>(AppSharedModel.Instance.SSYM040BModels);

                if (detail.SSYM040BDTOList.Count > 0)
                {
                    details.Add(detail);
                    target.FileDetailInfo = details;
                    // Excel出力ドキュメントDTOの生成
                    var outputDocument
                        = new SSYM040FExcelOutputDocument
                            <GeneratedWaste_SinglePlate_SinTetsuPayoutDTO<GeneratedWaste_SinglePlate_SinTetsuPayoutDetail>>(target)
                                .Chain(x => x.TemplateBookPath = templateBookPath)
                                .Chain(x => x.OutputBookPath = outputBookPath)
                                .Chain(x => x.Year = AppSharedModel.Instance.Year)
                                .Chain(x => x.Month = AppSharedModel.Instance.Month)
                                .Chain(x => x.DocumentOperationContainer = container);
                    // ドキュメントマネージャ(Excel操作用)の生成
                    DocumentManager docManager
                        = new DocumentManager(DocumentType.Excel);
                    // 発生屑・単板・伸鉄払出明細(SSYM040F)の出力
                    return docManager.OutputSSYM040FList(outputDocument);
                }
                else
                {
                    return false;
                }
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// スクラップ外販明細(SSYM050F)の出力
        /// </summary>
        /// <param name="templateBookPath">テンプレートExcelブックのパス</param>
        /// <param name="outputBookPath">出力Excelブックのパス</param>
        /// <param name="container">ドキュメントに対する操作完了時のコンテナ</param>
        /// <returns>結果</returns>
        internal static bool OutputSSYM050FList(string templateBookPath,
                                                string outputBookPath,
                                                IDocumentOperationContainer container)
        {
            try
            {
                var target = new ScrapExternalSalesDTO<ScrapExternalSalesDetail>();
                var details = new List<ScrapExternalSalesDetail>();
                var detail = new ScrapExternalSalesDetail();
                // スクラップ外販明細のリスト
                detail.SSYM050BDTOList
                    = new SSYM050BDTOGenerator()
                        .GenerateList<SSYM050BModel, SSYM050BDTO>(AppSharedModel.Instance.SSYM050BModels);

                if (detail.SSYM050BDTOList.Count > 0)
                {
                    details.Add(detail);
                    target.FileDetailInfo = details;
                    // Excel出力ドキュメントDTOの生成
                    var outputDocument
                        = new SSYM050FExcelOutputDocument
                            <ScrapExternalSalesDTO<ScrapExternalSalesDetail>>(target)
                                .Chain(x => x.TemplateBookPath = templateBookPath)
                                .Chain(x => x.OutputBookPath = outputBookPath)
                                .Chain(x => x.Year = AppSharedModel.Instance.Year)
                                .Chain(x => x.Month = AppSharedModel.Instance.Month)
                                .Chain(x => x.DocumentOperationContainer = container);
                    // ドキュメントマネージャ(Excel操作用)の生成
                    DocumentManager docManager
                        = new DocumentManager(DocumentType.Excel);
                    // スクラップ外販明細(SSYM050F)の出力
                    return docManager.OutputSSYM050FList(outputDocument);
                }
                else
                {
                    return false;
                }
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 在庫売り受注一覧表(SSZA400B)の出力
        /// </summary>
        /// <param name="templateBookPath">テンプレートExcelブックのパス</param>
        /// <param name="outputBookPath">出力Excelブックのパス</param>
        /// <param name="container">ドキュメントに対する操作完了時のコンテナ</param>
        /// <returns>結果</returns>
        internal static bool OutputSSZA400BList(string templateBookPath,
                                                string outputBookPath,
                                                IDocumentOperationContainer container)
        {
            try
            {
                var target = new StockSellOrdersDTO<StockSellOrdersDetail>();
                var details = new List<StockSellOrdersDetail>();
                var detail = new StockSellOrdersDetail();
                // 在庫売り受注のリスト
                detail.SSZA040BDTOList
                    = new SSZA040BDTOGenerator()
                        .GenerateList<SSZA040BModel, SSZA040BDTO>(AppSharedModel.Instance.SSZA040BModels);

                if (detail.SSZA040BDTOList.Count > 0)
                {
                    details.Add(detail);
                    target.FileDetailInfo = details;
                    // Excel出力ドキュメントDTOの生成
                    var outputDocument
                        = new SSZA400BExcelOutputDocument
                            <StockSellOrdersDTO<StockSellOrdersDetail>>(target)
                                .Chain(x => x.TemplateBookPath = templateBookPath)
                                .Chain(x => x.OutputBookPath = outputBookPath)
                                .Chain(x => x.Year = AppSharedModel.Instance.Year)
                                .Chain(x => x.Month = AppSharedModel.Instance.Month)
                                .Chain(x => x.DocumentOperationContainer = container);
                    // ドキュメントマネージャ(Excel操作用)の生成
                    DocumentManager docManager
                        = new DocumentManager(DocumentType.Excel);
                    // 在庫売り受注一覧表(SSZA400B)の出力
                    return docManager.OutputSSZA400BList(outputDocument);
                }
                else
                {
                    return false;
                }
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}

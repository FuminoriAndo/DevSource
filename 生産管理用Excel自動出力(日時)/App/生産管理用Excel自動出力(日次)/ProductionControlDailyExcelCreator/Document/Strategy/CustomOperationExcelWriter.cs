//*************************************************************************************
//
//   各操作のカスタムExcelライター
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Document.Excel;
using Document.Strategy.Excel;
using DTO.Base;
using Microsoft.Office.Interop.Excel;
using ProductionControlDailyExcelCreator.Core;
using ProductionControlDailyExcelCreator.DTO;
using ProductionControlDailyExcelCreator.Types;
using Utility.Core;
using Utility.Types;
using ProductionControlDailyExcelCreator.Common.Container;

namespace ProductionControlDailyExcelCreator.Document.Strategy
{
    /// <summary>
    /// 技術特殊引合品チェックリスト(PMGQ010F)のExcelライター
    /// </summary>
    internal class PMGQ010FExcelWriter : AbstractExcelWriter
    {
        #region フィールド

        /// <summary>
        /// ExcelApplication
        /// </summary>
        private Application application = null;

        /// <summary>
        /// テンプレート用のワークブック
        /// </summary>
        private Workbook templateWorkBook = null;

        /// <summary>
        /// テンプレート用のワークシート
        /// </summary>
        private Worksheet templateWorkSheet = null;

        /// <summary>
        /// 出力用のワークブック
        /// </summary>
        private Workbook outputWorkBook = null;

        /// <summary>
        /// 出力用のワークシート
        /// </summary>
        private Worksheet outputWorkSheet = null;

        /// <summary>
        /// ドキュメントに対する操作完了時のコンテナ
        /// </summary>
        private IDocumentOperationContainer container;

        #endregion

        #region プロパティ

        /// <summary>
        /// 年
        /// </summary>
        internal string Year { get; set; }

        /// <summary>
        /// 月
        /// </summary>
        internal string Month { get; set; }

        /// <summary>
        /// テンプレートワークブックのパス
        /// </summary>
        internal string TemplateWorkBookPath { get; set; }

        /// <summary>
        /// 出力ワークブックのパス
        /// </summary>
        internal string OutputWorkBookPath { get; set; }

        /// <summary>
        /// 出力ワークブックのフォルダ
        /// </summary>
        internal string OutputWorkBookFolder { get; set; }

        /// <summary>
        /// 出力ワークブック名
        /// </summary>
        internal string OutputWorkBookName { get; set; }

        /// <summary>
        /// プレビューを表示するか
        /// </summary>
        internal bool IsShowPreview { get; set; }

        /// <summary>
        /// Excelシート名のマップ
        /// </summary>
        private static readonly IDictionary<PMGQ010BExcelSheetType, string> excelSheetNameMap = null;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// 静的コンストラクタ
        /// </summary>
        static PMGQ010FExcelWriter()
        {
            excelSheetNameMap
                = new Dictionary<PMGQ010BExcelSheetType, string>();
            excelSheetNameMap.Add(PMGQ010BExcelSheetType.Detail, PMGQ010BExcelSheetType.Detail.GetValue());
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        internal PMGQ010FExcelWriter()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="container">ドキュメントに対する操作完了時のコンテナ</param>
        internal PMGQ010FExcelWriter(IDocumentOperationContainer container)
        {
            this.container = container;
        }

        #endregion

        #region メソッド

        /// <summary>
        /// 初期化処理
        /// </summary>
        public override void Initialize()
        {
            try
            {
                // ExcelApplicationの取得
                application = ExcelUtils.GetExcelApplication();
                application.DisplayAlerts = false;

                // Excelテンプレート用のExcelワークブックの取得
                templateWorkBook = ExcelUtils.GetExcelTemplateWorkBook(application, TemplateWorkBookPath);

                // 出力用のExcelワークブックのフォルダ
                OutputWorkBookFolder = OutputWorkBookFolder ??
                    Path.GetDirectoryName(OutputWorkBookPath) + "\\";
                // 現在時刻の取得
                DateTimeNow = DateTime.Now;
                // 出力用のExcelワークブック名
                OutputWorkBookName = OutputWorkBookName ??
                    (Path.GetFileName(OutputWorkBookPath)).ConvertExcelWorkBookName(DateTimeNow);
                // 出力用のExcelワークブックの取得
                outputWorkBook = ExcelUtils.GetExcelOutputWorkBook(application,
                                                                   OutputWorkBookFolder,
                                                                   OutputWorkBookName,
                                                                   TemplateWorkBookPath, true, true);
            }

            catch (Exception)
            {
                // COMオブジェクトの解放
                ReleaseComObject();
                throw;
            }
        }

        /// <summary>
        /// 書込み前処理
        /// </summary>
        public override void PreWrite()
        {
        }

        /// <summary>
        /// ヘッダーの書込み
        /// </summary>
        /// <typeparam name="T">ヘッダーを継承した任意のオブジェクトの型</typeparam>
        /// <param name="header">ファイルのヘッダー情報</param>
        public override void WriteHeader<T>(T header)
        {
        }

        /// <summary>
        /// 詳細の書込み
        /// </summary>
        /// <typeparam name="T">詳細を継承した任意のオブジェクトの型</typeparam>
        /// <param name="detail">ファイルの詳細情報</param>
        public override void WriteDetail<T>(IList<T> detail)
        {
            try
            {
                // 詳細
                var detailData = new List<PMGQ010BDTO>();
                var targets = detail[0] as TechnologySpecialInquiriesGoodsDetail;
                var items = targets.PMGQ010BDTOList;
                items.ForEach(item =>
                {
                    detailData.Add(item);
                });
                // ワークシートの切替え(詳細)
                SwitchOutputWorkBookSheet(PMGQ010BExcelSheetType.Detail);
                // 年月日の書込み(タイトルの右端)
                WriteYearMonth(outputWorkSheet.Range("$M$1"), DateTimeNow);
                // 詳細データの書込み
                WriteDetailData(detailData);
            }

            catch (Exception)
            {
                // COMオブジェクトの解放
                ReleaseComObject();
                throw;
            }
        }

        /// <summary>
        /// フッターの書込み
        /// </summary>
        /// <typeparam name="T">フッターを継承した任意のオブジェクトの型</typeparam>
        /// <param name="footer">ファイルのフッター情報</param>
        public override void WriteFooter<T>(T footer)
        {
            // 処理不要
        }

        /// <summary>
        /// 書込み後処理
        /// </summary>
        public override void PostWrite()
        {
            // 処理不要
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public override void Terminate()
        {
            try
            {
                // ワークブックの保存
                outputWorkBook.Save(OutputWorkBookFolder, OutputWorkBookName);
                if (container != null)
                {
                    container.NotifyOutputExcel(OutputWorkBookFolder + OutputWorkBookName);
                }
            }

            catch (Exception)
            {
                throw;
            }

            finally
            {
                // COMオブジェクトの解放
                ReleaseComObject();
            }
        }

        /// <summary>
        /// COMオブジェクトの解放
        /// </summary>
        protected override void ReleaseComObject()
        {
            if (outputWorkBook != null)
            {
                outputWorkBook.Close(false, false, false);
            }

            if (templateWorkBook != null)
            {
                templateWorkBook.Close(false, false, false);
            }

            if (application != null)
            {
                application.Visible = false;
                application.DisplayAlerts = true;
                application.Quit();
                Marshal.ReleaseComObject(application);
                application = null;
            }

            if (outputWorkBook != null)
            {
                Marshal.ReleaseComObject(outputWorkBook);
                outputWorkBook = null;
            }

            if (templateWorkBook != null)
            {
                Marshal.ReleaseComObject(templateWorkBook);
                templateWorkBook = null;
            }

            if (templateWorkSheet != null)
            {
                Marshal.ReleaseComObject(templateWorkSheet);
                templateWorkSheet = null;
            }

            if (outputWorkSheet != null)
            {
                Marshal.ReleaseComObject(outputWorkSheet);
                outputWorkSheet = null;
            }
        }

        /// <summary>
        /// 出力用のExcelワークブックのシートの切替え
        /// </summary>
        /// <param name="excelSheetType">Excelシートの種類</param>
        private void SwitchOutputWorkBookSheet(PMGQ010BExcelSheetType excelSheetType)
        {
            try
            {
                // 対象のワークシートの検索
                outputWorkSheet = ExcelUtils.SearchSheet(outputWorkBook, excelSheetNameMap[excelSheetType]);
                outputWorkSheet.Select(true);
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 詳細データの書込み
        /// </summary>
        /// <param name="list">詳細データ</param>
        private void WriteDetailData(IList<PMGQ010BDTO> list)
        {
            Range range = null;
            int listCount = list.Count;

            // 罫線
            int startLeftRowPosition = 5;               // 左上の行番号
            int endRightRowPosition = listCount + 4;    // 右下の行番号

            try
            {
                // 印刷範囲の設定
                outputWorkSheet.PageSetup.PrintArea = @"$A$1:$N$" + (endRightRowPosition).ToString();
                // 罫線および値を出力するレンジの設定
                range = outputWorkSheet.Range("$A$" + (startLeftRowPosition).ToString(),
                                              "$N$" + (endRightRowPosition).ToString());

                range.Borders[XlBordersIndex.xlInsideHorizontal].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlInsideHorizontal].ColorIndex = 1;
                range.Borders[XlBordersIndex.xlInsideVertical].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlInsideVertical].ColorIndex = 1;
                range.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlEdgeTop].ColorIndex = 1;
                range.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlEdgeBottom].ColorIndex = 1;
                range.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlEdgeLeft].ColorIndex = 1;
                range.Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlEdgeRight].ColorIndex = 1;

                var outputData = new object[listCount, 14];
                for (int i = 0; i < listCount; ++i)
                {
                    // 受注№
                    outputData[i, 0] = list[i].OrderNo;
                    // 需要家名
                    outputData[i, 1] = list[i].CustomerName;
                    // 規格名
                    outputData[i, 2] = list[i].StandardName;
                    // 製品サイズ(厚)
                    outputData[i, 3] = list[i].ProductAtu;
                    // 製品サイズ(巾)
                    outputData[i, 4] = list[i].ProductHaba;
                    // 製品サイズ(長)
                    outputData[i, 5] = list[i].ProductNaga;
                    // 定耳
                    outputData[i, 6] = list[i].TM;
                    // ロールサイズ(巾)
                    outputData[i, 7] = list[i].RollHaba;
                    // ロールサイズ(長)
                    outputData[i, 8] = list[i].RollNaga;
                    // 試圧区分
                    outputData[i, 9] = list[i].TPClassfication;
                    // ロット№
                    outputData[i, 10] = list[i].Lot;
                    // 圧延予定
                    outputData[i, 11] = list[i].AtuenYMD;
                    // スラブ№
                    outputData[i, 12] = list[i].SlabNo;
                    // ロール№
                    outputData[i, 13] = list[i].RollNo;
                }

                range.Value2 = outputData;
            }

            catch (Exception)
            {
                throw;
            }

            finally
            {
                if (range != null)
                {
                    Marshal.ReleaseComObject(range);
                    range = null;
                }
            }
        }

        /// <summary>
        /// 年月日の書込み
        /// </summary>
        /// <param name="range">書込み範囲</param>
        /// <param name="datetime">現在時刻</param>
        protected override void WriteYearMonth(Range range, DateTime datetime)
        {
            range.Value2 = "           " + datetime.ToString("G");
        }

        #endregion

    }

    /// <summary>
    /// 圧延計画リスト(加熱炉用）(PMPA280F)のExcelライター
    /// </summary>
    internal class PMPA280FExcelWriter : AbstractExcelWriter
    {
        #region フィールド

        /// <summary>
        /// ExcelApplication
        /// </summary>
        private Application application = null;

        /// <summary>
        /// テンプレート用のワークブック
        /// </summary>
        private Workbook templateWorkBook = null;

        /// <summary>
        /// テンプレート用のワークシート
        /// </summary>
        private Worksheet templateWorkSheet = null;

        /// <summary>
        /// 出力用のワークブック
        /// </summary>
        private Workbook outputWorkBook = null;

        /// <summary>
        /// 出力用のワークシート
        /// </summary>
        private Worksheet outputWorkSheet = null;

        /// <summary>
        /// Excelシート名のマップ
        /// </summary>
        private static readonly IDictionary<PMPA260BExcelSheetType, string> excelSheetNameMap = null;

        /// <summary>
        /// ドキュメントに対する操作完了時のコンテナ
        /// </summary>
        private IDocumentOperationContainer container;

        #endregion

        #region プロパティ

        /// <summary>
        /// 年
        /// </summary>
        internal string Year { get; set; }

        /// <summary>
        /// 月
        /// </summary>
        internal string Month { get; set; }

        /// <summary>
        /// RC№
        /// </summary>
        internal string RCNo { get; set; }

        /// <summary>
        /// テンプレートワークブックのパス
        /// </summary>
        internal string TemplateWorkBookPath { get; set; }

        /// <summary>
        /// 出力ワークブックのパス
        /// </summary>
        internal string OutputWorkBookPath { get; set; }

        /// <summary>
        /// 出力ワークブックのフォルダ
        /// </summary>
        internal string OutputWorkBookFolder { get; set; }

        /// <summary>
        /// 出力ワークブック名
        /// </summary>
        internal string OutputWorkBookName { get; set; }

        /// <summary>
        /// プレビューを表示するか
        /// </summary>
        internal bool IsShowPreview { get; set; }

        #endregion

        #region コンストラクタ

        /// <summary>
        /// 静的コンストラクタ
        /// </summary>
        static PMPA280FExcelWriter()
        {
            excelSheetNameMap
                = new Dictionary<PMPA260BExcelSheetType, string>();
            excelSheetNameMap.Add(PMPA260BExcelSheetType.Kinmu1, PMPA260BExcelSheetType.Kinmu1.GetValue());
            excelSheetNameMap.Add(PMPA260BExcelSheetType.Kinmu2, PMPA260BExcelSheetType.Kinmu2.GetValue());
            excelSheetNameMap.Add(PMPA260BExcelSheetType.Kinmu3, PMPA260BExcelSheetType.Kinmu3.GetValue());
            excelSheetNameMap.Add(PMPA260BExcelSheetType.Kinmu4, PMPA260BExcelSheetType.Kinmu4.GetValue());
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        internal PMPA280FExcelWriter()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="container">ドキュメントに対する操作完了時のコンテナ</param>
        internal PMPA280FExcelWriter(IDocumentOperationContainer container)
        {
            this.container = container;
        }

        #endregion

        #region メソッド

        /// <summary>
        /// 初期化処理
        /// </summary>
        public override void Initialize()
        {
            try
            {
                // ExcelApplicationの取得
                application = ExcelUtils.GetExcelApplication();
                application.DisplayAlerts = false;

                // Excelテンプレート用のExcelワークブックの取得
                templateWorkBook = ExcelUtils.GetExcelTemplateWorkBook(application, TemplateWorkBookPath);

                // 出力用のExcelワークブックのフォルダ
                OutputWorkBookFolder = OutputWorkBookFolder ??
                    Path.GetDirectoryName(OutputWorkBookPath) + "\\";
                // 出力用のExcelワークブック名
                OutputWorkBookName = OutputWorkBookName ?? BuildExcelWorkBookName(DateTimeNow);
                // 出力用のExcelワークブックの取得
                outputWorkBook = ExcelUtils.GetExcelOutputWorkBook(application,
                                                                   OutputWorkBookFolder,
                                                                   OutputWorkBookName,
                                                                   TemplateWorkBookPath, true, true);
            }

            catch (Exception)
            {
                // COMオブジェクトの解放
                ReleaseComObject();
                throw;
            }
        }

        /// <summary>
        /// 書込み前処理
        /// </summary>
        public override void PreWrite()
        {
        }

        /// <summary>
        /// ヘッダーの書込み
        /// </summary>
        /// <typeparam name="T">ヘッダーを継承した任意のオブジェクトの型</typeparam>
        /// <param name="header">ファイルのヘッダー情報</param>
        public override void WriteHeader<T>(T header)
        {
        }

        /// <summary>
        /// 詳細の書込み
        /// </summary>
        /// <typeparam name="T">詳細を継承した任意のオブジェクトの型</typeparam>
        /// <param name="detail">ファイルの詳細情報</param>
        public override void WriteDetail<T>(IList<T> detail)
        {
            try
            {
                // 詳細
                var detailDataKinmu1 = new List<PMPA260BDTO>();
                var detailDataKinmu2 = new List<PMPA260BDTO>();
                var detailDataKinmu3 = new List<PMPA260BDTO>();
                var detailDataKinmu4 = new List<PMPA260BDTO>();
                var targets = detail[0] as RollingPlanDetail;
                var items = targets.PMPA260BDTOList;

                foreach (var sheetType in EnumExtensions<PMPA260BExcelSheetType>.Enumerate())
                {
                    var target = targets.PMPA260BDTOList
                                        .Where(item => item.Work == ((int)sheetType).ToString());
                    var slabCountSum = target.Sum(countSum => countSum.SlabCount);
                    var slabTotalWeightSum = target.Sum(weightSum => weightSum.SlabTotalWeight);

                    switch (sheetType)
                    {
                        case PMPA260BExcelSheetType.Kinmu1:
                            detailDataKinmu1.AddRange(target);
                            // ワークシートの切替え(詳細)
                            SwitchOutputWorkBookSheet(PMPA260BExcelSheetType.Kinmu1);
                            // 詳細データの書込み
                            WriteDetailData(detailDataKinmu1, slabCountSum, slabTotalWeightSum);
                            break;
                        case PMPA260BExcelSheetType.Kinmu2:
                            detailDataKinmu2.AddRange(target);
                            // ワークシートの切替え(詳細)
                            SwitchOutputWorkBookSheet(PMPA260BExcelSheetType.Kinmu2);
                            // 詳細データの書込み
                            WriteDetailData(detailDataKinmu2, slabCountSum, slabTotalWeightSum);
                            break;
                        case PMPA260BExcelSheetType.Kinmu3:
                            detailDataKinmu3.AddRange(target);
                            // ワークシートの切替え(詳細)
                            SwitchOutputWorkBookSheet(PMPA260BExcelSheetType.Kinmu3);
                            // 詳細データの書込み
                            WriteDetailData(detailDataKinmu3, slabCountSum, slabTotalWeightSum);
                            break;
                        case PMPA260BExcelSheetType.Kinmu4:
                            detailDataKinmu4.AddRange(target);
                            // ワークシートの切替え(詳細)
                            SwitchOutputWorkBookSheet(PMPA260BExcelSheetType.Kinmu4);
                            // 詳細データの書込み
                            WriteDetailData(detailDataKinmu4, slabCountSum, slabTotalWeightSum);
                            break;
                        default:
                            break;
                    }
                    // 年月日の書込み(タイトルの右端)
                    WriteYearMonth(outputWorkSheet.Range("$X$1"), DateTimeNow);

                }               

                // 勤務1のシートに切替える
                SwitchOutputWorkBookSheet(PMPA260BExcelSheetType.Kinmu1);
            }
            catch (Exception)
            {
                // COMオブジェクトの解放
                ReleaseComObject();
                throw;
            }
        }      

        /// <summary>
        /// フッターの書込み
        /// </summary>
        /// <typeparam name="T">フッターを継承した任意のオブジェクトの型</typeparam>
        /// <param name="footer">ファイルのフッター情報</param>
        public override void WriteFooter<T>(T footer)
        {
            // 処理不要
        }

        /// <summary>
        /// 書込み後処理
        /// </summary>
        public override void PostWrite()
        {
            // 処理不要
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public override void Terminate()
        {
            try
            {
                // ワークブックの保存
                outputWorkBook.Save(OutputWorkBookFolder, OutputWorkBookName);
                if (container != null)
                {
                    container.NotifyOutputExcel(OutputWorkBookFolder + OutputWorkBookName);
                }
            }

            catch (Exception)
            {
                throw;
            }

            finally
            {
                // COMオブジェクトの解放
                ReleaseComObject();
            }
        }

        /// <summary>
        /// COMオブジェクトの解放
        /// </summary>
        protected override void ReleaseComObject()
        {
            if (outputWorkBook != null)
            {
                outputWorkBook.Close(false, false, false);
            }

            if (templateWorkBook != null)
            {
                templateWorkBook.Close(false, false, false);
            }

            if (application != null)
            {
                application.Visible = false;
                application.DisplayAlerts = true;
                application.Quit();
                Marshal.ReleaseComObject(application);
                application = null;
            }

            if (outputWorkBook != null)
            {
                Marshal.ReleaseComObject(outputWorkBook);
                outputWorkBook = null;
            }

            if (templateWorkBook != null)
            {
                Marshal.ReleaseComObject(templateWorkBook);
                templateWorkBook = null;
            }

            if (templateWorkSheet != null)
            {
                Marshal.ReleaseComObject(templateWorkSheet);
                templateWorkSheet = null;
            }

            if (outputWorkSheet != null)
            {
                Marshal.ReleaseComObject(outputWorkSheet);
                outputWorkSheet = null;
            }
        }

        /// <summary>
        /// Excelワークブック名の作成
        /// </summary>
        /// <param name="dateTimeNow">現在時刻</param>
        /// <returns>Excelワークブック名</returns>
        private string BuildExcelWorkBookName(DateTime dateTimeNow)
        {
            try
            {
                var rcNoPart = RCNo.Substring(0, 2)
                             + SymbolType.Dash.GetValue()  
                             + RCNo.Substring(2, 2)
                             + SymbolType.Dash.GetValue()
                             + RCNo.Substring(4, 2);

                var datePart = dateTimeNow.ToString("MMdd");
                var dateTimePart = dateTimeNow.ToString("HHmmss");

                return rcNoPart + SymbolType.Underscore.GetValue()
                     + datePart + SymbolType.Underscore.GetValue()
                     + dateTimePart + ExtensionType.XLSX.GetValue();
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 出力用のExcelワークブックのシートの切替え
        /// </summary>
        /// <param name="excelSheetType">Excelシートの種類</param>
        private void SwitchOutputWorkBookSheet(PMPA260BExcelSheetType excelSheetType)
        {
            try
            {
                // 対象のワークシートの検索
                outputWorkSheet = ExcelUtils.SearchSheet(outputWorkBook, excelSheetNameMap[excelSheetType]);
                outputWorkSheet.Select(true);
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 詳細データの書込み
        /// </summary>
        /// <param name="list">詳細データ</param>
        /// <param name="slabCountSum">スラブ本数の合計</param>
        /// <param name="slabWeightSum">スラブ重量の合計</param>
        private void WriteDetailData(IList<PMPA260BDTO> list, int slabCountSum, int slabTotalWeightSum)
        {
            Range rangeDetail = null;
            Range rangeSum = null;
            int listCount = list.Count;

            // 罫線(詳細)
            int detailStartLeftRowPosition = 5;               // 左上の行番号
            int detailEndRightRowPosition = listCount + 4;    // 右下の行番号

            try
            {
                // 印刷範囲の設定
                outputWorkSheet.PageSetup.PrintArea = @"$A$1:$AA$" + (detailEndRightRowPosition + 4).ToString();
                // 罫線および値を出力するレンジの設定(詳細)
                rangeDetail = outputWorkSheet.Range("$A$" + (detailStartLeftRowPosition).ToString(),
                                                    "$AA$" + (detailEndRightRowPosition).ToString());

                rangeDetail.Borders[XlBordersIndex.xlInsideHorizontal].LineStyle = XlLineStyle.xlContinuous;
                rangeDetail.Borders[XlBordersIndex.xlInsideHorizontal].ColorIndex = 1;
                rangeDetail.Borders[XlBordersIndex.xlInsideVertical].LineStyle = XlLineStyle.xlContinuous;
                rangeDetail.Borders[XlBordersIndex.xlInsideVertical].ColorIndex = 1;
                rangeDetail.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
                rangeDetail.Borders[XlBordersIndex.xlEdgeTop].ColorIndex = 1;
                rangeDetail.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
                rangeDetail.Borders[XlBordersIndex.xlEdgeBottom].ColorIndex = 1;
                rangeDetail.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
                rangeDetail.Borders[XlBordersIndex.xlEdgeLeft].ColorIndex = 1;
                rangeDetail.Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;
                rangeDetail.Borders[XlBordersIndex.xlEdgeRight].ColorIndex = 1;

                var outputData = new object[listCount, 27];
                for (int i = 0; i < listCount; ++i)
                {
                    // RCNO
                    outputData[i, 0] = list[i].RCNO;
                    // 勤務
                    outputData[i, 1] = list[i].Work;
                    // 圧延順
                    outputData[i, 2] = list[i].AtuenOrder;
                    // ロット№
                    outputData[i, 3] = list[i].LotNo;
                    // 1次鋳片
                    outputData[i, 4] = list[i].Slab1No;
                    // 材料識別
                    outputData[i, 5] = list[i].MaterialsIdentification;
                    // TP管理
                    outputData[i, 6] = list[i].TPManagement;
                    // 厚テーパー
                    outputData[i, 7] = list[i].AtuTapera;
                    // 巾テーパー
                    outputData[i, 8] = list[i].HabaTapera;
                    // ＨＣ区分
                    outputData[i, 9] = list[i].HCDivision;
                    // 抽出時間
                    outputData[i, 10] = list[i].ExtractionHHMM;
                    // 規格
                    outputData[i, 11] = list[i].Standard;
                    // ロールサイズ(厚)
                    outputData[i, 12] = list[i].RollAtu;
                    // ロールサイズ(巾)
                    outputData[i, 13] = list[i].RollHaba;
                    // ロールサイズ(長)
                    outputData[i, 14] = list[i].RollNaga;
                    // スラブサイズ(厚)
                    outputData[i, 15] = list[i].SlabAtu;
                    // スラブサイズ(巾)
                    outputData[i, 16] = list[i].SlabHaba;
                    // スラブサイズ(長)
                    outputData[i, 17] = list[i].SlabNaga;
                    // 重量(kg)
                    outputData[i, 18] = list[i].SlabWeight;
                    // 本数
                    outputData[i, 19] = list[i].SlabCount;
                    // 再計画
                    outputData[i, 20] = list[i].RePlan;
                    // 急ぎ
                    outputData[i, 21] = list[i].Haste;
                    // 輸出
                    outputData[i, 22] = list[i].Export;
                    // 抽出温度
                    outputData[i, 23] = list[i].ExtractionTemperature;
                    // 試圧
                    outputData[i, 24] = list[i].TestPressure;
                    // 板巾
                    outputData[i, 25] = list[i].PlateHaba;
                    // 歪
                    outputData[i, 26] = list[i].Strain;
                }

                rangeDetail.Value2 = outputData;

                // 罫線(合計)
                int sumStartLeftRowPosition = detailEndRightRowPosition + 2;    // 左上の行番号
                int sumEndRightRowPosition = sumStartLeftRowPosition + 2;       // 右下の行番号

                // 罫線および値を出力するレンジの設定(合計)
                rangeSum = outputWorkSheet.Range("$T$" + (sumStartLeftRowPosition).ToString(),
                                                 "$AA$" + (sumEndRightRowPosition).ToString());

                rangeSum.Borders[XlBordersIndex.xlInsideHorizontal].LineStyle = XlLineStyle.xlContinuous;
                rangeSum.Borders[XlBordersIndex.xlInsideHorizontal].ColorIndex = 1;
                rangeSum.Borders[XlBordersIndex.xlInsideVertical].LineStyle = XlLineStyle.xlContinuous;
                rangeSum.Borders[XlBordersIndex.xlInsideVertical].ColorIndex = 1;
                rangeSum.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
                rangeSum.Borders[XlBordersIndex.xlEdgeTop].ColorIndex = 1;
                rangeSum.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
                rangeSum.Borders[XlBordersIndex.xlEdgeBottom].ColorIndex = 1;
                rangeSum.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
                rangeSum.Borders[XlBordersIndex.xlEdgeLeft].ColorIndex = 1;
                rangeSum.Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;
                rangeSum.Borders[XlBordersIndex.xlEdgeRight].ColorIndex = 1;

                // 勤務計
                outputWorkSheet.CellMerge("$T$" + sumStartLeftRowPosition, "$AA$" + sumStartLeftRowPosition);
                outputWorkSheet.Range("$T$" + sumStartLeftRowPosition.ToString()).Value2 = "(勤務計)";
                outputWorkSheet.Range("$T$" + sumStartLeftRowPosition.ToString()).HorizontalAlignment = Constants.xlLeft;

                // 本数
                outputWorkSheet.CellMerge("$T$" + (sumStartLeftRowPosition + 1).ToString(), "$W$" + (sumStartLeftRowPosition + 1).ToString());
                outputWorkSheet.Range("$T$" + (sumStartLeftRowPosition + 1).ToString()).Value2 = "本数";
                outputWorkSheet.Range("$T$" + (sumStartLeftRowPosition + 1).ToString()).HorizontalAlignment = Constants.xlCenter;
                
                // 重量
                outputWorkSheet.CellMerge("$X$" + (sumStartLeftRowPosition + 1).ToString(), "$AA$" + (sumStartLeftRowPosition + 1).ToString());
                outputWorkSheet.Range("$X$" + (sumStartLeftRowPosition + 1).ToString()).Value2 = "重量(kg)";
                outputWorkSheet.Range("$X$" + (sumStartLeftRowPosition + 1).ToString()).HorizontalAlignment = Constants.xlCenter;

                // 本数(値)
                outputWorkSheet.CellMerge("$T$" + (sumStartLeftRowPosition + 2).ToString(), "$W$" + (sumStartLeftRowPosition + 2).ToString());
                outputWorkSheet.Range("$T$" + (sumStartLeftRowPosition + 2).ToString()).Value2 = slabCountSum;
                outputWorkSheet.Range("$T$" + (sumStartLeftRowPosition + 2).ToString()).HorizontalAlignment = Constants.xlRight;
                outputWorkSheet.Range("$T$" + (sumStartLeftRowPosition + 2).ToString()).NumberFormatLocal = "#,###";

                // 重量(値)
                outputWorkSheet.CellMerge("$X$" + (sumStartLeftRowPosition + 2).ToString(), "$AA$" + (sumStartLeftRowPosition + 2).ToString());
                outputWorkSheet.Range("$X$" + (sumStartLeftRowPosition + 2).ToString()).Value2 = slabTotalWeightSum;
                outputWorkSheet.Range("$X$" + (sumStartLeftRowPosition + 2).ToString()).HorizontalAlignment = Constants.xlRight;
                outputWorkSheet.Range("$X$" + (sumStartLeftRowPosition + 2).ToString()).NumberFormatLocal = "#,###";
            }

            catch (Exception)
            {
                throw;
            }

            finally
            {
                if (rangeDetail != null)
                {
                    Marshal.ReleaseComObject(rangeDetail);
                    rangeDetail = null;
                }

                if (rangeSum != null)
                {
                    Marshal.ReleaseComObject(rangeSum);
                    rangeSum = null;
                }
            }
        }

        /// <summary>
        /// 年月日の書込み
        /// </summary>
        /// <param name="range">書込み範囲</param>
        /// <param name="datetime">現在時刻</param>
        protected override void WriteYearMonth(Range range, DateTime datetime)
        {
            range.Value2 = "   " + datetime.ToString("G");
        }

        #endregion

    }

    /// <summary>
    /// 未計画受注リスト(PMPD610F)のExcelライター
    /// </summary>
    internal class PMPD610FExcelWriter : AbstractExcelWriter
    {
        #region フィールド

        /// <summary>
        /// ExcelApplication
        /// </summary>
        private Application application = null;

        /// <summary>
        /// テンプレート用のワークブック
        /// </summary>
        private Workbook templateWorkBook = null;

        /// <summary>
        /// テンプレート用のワークシート
        /// </summary>
        private Worksheet templateWorkSheet = null;

        /// <summary>
        /// 出力用のワークブック
        /// </summary>
        private Workbook outputWorkBook = null;

        /// <summary>
        /// 出力用のワークシート
        /// </summary>
        private Worksheet outputWorkSheet = null;

        /// <summary>
        /// ドキュメントに対する操作完了時のコンテナ
        /// </summary>
        private IDocumentOperationContainer container;

        #endregion

        #region プロパティ

        /// <summary>
        /// 年
        /// </summary>
        internal string Year { get; set; }

        /// <summary>
        /// 月
        /// </summary>
        internal string Month { get; set; }

        /// <summary>
        /// テンプレートワークブックのパス
        /// </summary>
        internal string TemplateWorkBookPath { get; set; }

        /// <summary>
        /// 出力ワークブックのパス
        /// </summary>
        internal string OutputWorkBookPath { get; set; }

        /// <summary>
        /// 出力ワークブックのフォルダ
        /// </summary>
        internal string OutputWorkBookFolder { get; set; }

        /// <summary>
        /// 出力ワークブック名
        /// </summary>
        internal string OutputWorkBookName { get; set; }

        /// <summary>
        /// プレビューを表示するか
        /// </summary>
        internal bool IsShowPreview { get; set; }

        /// <summary>
        /// Excelシート名のマップ
        /// </summary>
        private static readonly IDictionary<PMPD330BExcelSheetType, string> excelSheetNameMap = null;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// 静的コンストラクタ
        /// </summary>
        static PMPD610FExcelWriter()
        {
            excelSheetNameMap
                = new Dictionary<PMPD330BExcelSheetType, string>();
            excelSheetNameMap.Add(PMPD330BExcelSheetType.Detail, PMPD330BExcelSheetType.Detail.GetValue());
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        internal PMPD610FExcelWriter()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="container">ドキュメントに対する操作完了時のコンテナ</param>
        internal PMPD610FExcelWriter(IDocumentOperationContainer container)
        {
            this.container = container;
        }

        #endregion

        #region メソッド

        /// <summary>
        /// 初期化処理
        /// </summary>
        public override void Initialize()
        {
            try
            {
                // ExcelApplicationの取得
                application = ExcelUtils.GetExcelApplication();
                application.DisplayAlerts = false;

                // Excelテンプレート用のExcelワークブックの取得
                templateWorkBook = ExcelUtils.GetExcelTemplateWorkBook(application, TemplateWorkBookPath);

                // 出力用のExcelワークブックのフォルダ
                OutputWorkBookFolder = OutputWorkBookFolder ??
                    Path.GetDirectoryName(OutputWorkBookPath) + "\\";
                // 現在時刻の取得
                DateTimeNow = DateTime.Now;
                // 出力用のExcelワークブック名
                OutputWorkBookName = OutputWorkBookName ??
                    (Path.GetFileName(OutputWorkBookPath)).ConvertExcelWorkBookName(DateTimeNow);
                // 出力用のExcelワークブックの取得
                outputWorkBook = ExcelUtils.GetExcelOutputWorkBook(application,
                                                                   OutputWorkBookFolder,
                                                                   OutputWorkBookName,
                                                                   TemplateWorkBookPath, true, true);
            }

            catch (Exception)
            {
                // COMオブジェクトの解放
                ReleaseComObject();
                throw;
            }
        }

        /// <summary>
        /// 書込み前処理
        /// </summary>
        public override void PreWrite()
        {
        }

        /// <summary>
        /// ヘッダーの書込み
        /// </summary>
        /// <typeparam name="T">ヘッダーを継承した任意のオブジェクトの型</typeparam>
        /// <param name="header">ファイルのヘッダー情報</param>
        public override void WriteHeader<T>(T header)
        {
        }

        /// <summary>
        /// 詳細の書込み
        /// </summary>
        /// <typeparam name="T">詳細を継承した任意のオブジェクトの型</typeparam>
        /// <param name="detail">ファイルの詳細情報</param>
        public override void WriteDetail<T>(IList<T> detail)
        {
            try
            {
                // 詳細
                var detailData = new List<PMPD330BDTO>();
                var targets = detail[0] as UnPlannedOrdersDetail;
                var items = targets.PMPD330BDTOList;
                items.ForEach(item =>
                {
                    detailData.Add(item);
                });
                // ワークシートの切替え(詳細)
                SwitchOutputWorkBookSheet(PMPD330BExcelSheetType.Detail);
                // 年月日の書込み(タイトルの右端)
                WriteYearMonth(outputWorkSheet.Range("$T$1"), DateTimeNow);
                // 詳細データの書込み
                WriteDetailData(detailData);
            }

            catch (Exception)
            {
                // COMオブジェクトの解放
                ReleaseComObject();
                throw;
            }
        }

        /// <summary>
        /// フッターの書込み
        /// </summary>
        /// <typeparam name="T">フッターを継承した任意のオブジェクトの型</typeparam>
        /// <param name="footer">ファイルのフッター情報</param>
        public override void WriteFooter<T>(T footer)
        {
            // 処理不要
        }

        /// <summary>
        /// 書込み後処理
        /// </summary>
        public override void PostWrite()
        {
            // 処理不要
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public override void Terminate()
        {
            try
            {
                // ワークブックの保存
                outputWorkBook.Save(OutputWorkBookFolder, OutputWorkBookName);
                if (container != null)
                {
                    container.NotifyOutputExcel(OutputWorkBookFolder + OutputWorkBookName);
                }
            }

            catch (Exception)
            {
                throw;
            }

            finally
            {
                // COMオブジェクトの解放
                ReleaseComObject();
            }
        }

        /// <summary>
        /// COMオブジェクトの解放
        /// </summary>
        protected override void ReleaseComObject()
        {
            if (outputWorkBook != null)
            {
                outputWorkBook.Close(false, false, false);
            }

            if (templateWorkBook != null)
            {
                templateWorkBook.Close(false, false, false);
            }

            if (application != null)
            {
                application.Visible = false;
                application.DisplayAlerts = true;
                application.Quit();
                Marshal.ReleaseComObject(application);
                application = null;
            }

            if (outputWorkBook != null)
            {
                Marshal.ReleaseComObject(outputWorkBook);
                outputWorkBook = null;
            }

            if (templateWorkBook != null)
            {
                Marshal.ReleaseComObject(templateWorkBook);
                templateWorkBook = null;
            }

            if (templateWorkSheet != null)
            {
                Marshal.ReleaseComObject(templateWorkSheet);
                templateWorkSheet = null;
            }

            if (outputWorkSheet != null)
            {
                Marshal.ReleaseComObject(outputWorkSheet);
                outputWorkSheet = null;
            }
        }

        /// <summary>
        /// 出力用のExcelワークブックのシートの切替え
        /// </summary>
        /// <param name="excelSheetType">Excelシートの種類</param>
        private void SwitchOutputWorkBookSheet(PMPD330BExcelSheetType excelSheetType)
        {
            try
            {
                // 対象のワークシートの検索
                outputWorkSheet = ExcelUtils.SearchSheet(outputWorkBook, excelSheetNameMap[excelSheetType]);
                outputWorkSheet.Select(true);
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 詳細データの書込み
        /// </summary>
        /// <param name="list">詳細データ</param>
        private void WriteDetailData(IList<PMPD330BDTO> list)
        {
            Range range = null;
            int listCount = list.Count;

            // 罫線
            int startLeftRowPosition = 5;               // 左上の行番号
            int endRightRowPosition = listCount + 4;    // 右下の行番号

            try
            {
                // 印刷範囲の設定
                outputWorkSheet.PageSetup.PrintArea = @"$A$1:$W$" + (endRightRowPosition).ToString();
                // 罫線および値を出力するレンジの設定
                range = outputWorkSheet.Range("$A$" + (startLeftRowPosition).ToString(),
                                              "$W$" + (endRightRowPosition).ToString());

                range.Borders[XlBordersIndex.xlInsideHorizontal].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlInsideHorizontal].ColorIndex = 1;
                range.Borders[XlBordersIndex.xlInsideVertical].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlInsideVertical].ColorIndex = 1;
                range.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlEdgeTop].ColorIndex = 1;
                range.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlEdgeBottom].ColorIndex = 1;
                range.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlEdgeLeft].ColorIndex = 1;
                range.Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlEdgeRight].ColorIndex = 1;

                var outputData = new object[listCount, 23];
                for (int i = 0; i < listCount; ++i)
                {
                    // 鋼種名
                    outputData[i, 0] = list[i].SteelsName;
                    // 鋼種別
                    outputData[i, 1] = list[i].SteelsType;
                    // 規格名
                    outputData[i, 2] = list[i].StandardName;
                    // 製品サイズ(厚)
                    outputData[i, 3] = list[i].ProductAtu;
                    // 製品サイズ(巾)
                    outputData[i, 4] = list[i].ProductHaba;
                    // 製品サイズ(長)
                    outputData[i, 5] = list[i].ProductNaga;
                    // 定耳
                    outputData[i, 6] = list[i].TM;
                    // 製品コード
                    outputData[i, 7] = list[i].ProductCode;
                    // 受注№
                    outputData[i, 8] = list[i].OrdersNo;
                    // 需要家名
                    outputData[i, 9] = list[i].CustomerName;
                    // 納期
                    outputData[i, 10] = list[i].DeliveryDate;
                    // 納期ランク
                    outputData[i, 11] = list[i].DeliveryDateRank;
                    // 枚数
                    outputData[i, 12] = list[i].Maisuu;
                    // ロールサイズ(巾)
                    outputData[i, 13] = list[i].RollHaba;
                    // ロールサイズ(長)
                    outputData[i, 14] = list[i].RollNaga;
                    // スラブサイズ(厚)
                    outputData[i, 15] = list[i].SlabAtu;
                    // スラブサイズ(巾)
                    outputData[i, 16] = list[i].SlabHaba;
                    // スラブサイズ(長)
                    outputData[i, 17] = list[i].SlabNaga;
                    // スラブ重量
                    outputData[i, 18] = list[i].SlabWeight;
                    // スラブ本数
                    outputData[i, 19] = list[i].SlabCount;
                    // 急ぎ区分
                    outputData[i, 20] = list[i].HasteDivision;
                    // 特殊区分
                    outputData[i, 21] = list[i].SpecialDivision;
                    // 輸出区分
                    outputData[i, 22] = list[i].ExportDivision;
                }

                range.Value2 = outputData;
            }

            catch (Exception)
            {
                throw;
            }

            finally
            {
                if (range != null)
                {
                    Marshal.ReleaseComObject(range);
                    range = null;
                }
            }
        }

        /// <summary>
        /// 年月日の書込み
        /// </summary>
        /// <param name="range">書込み範囲</param>
        /// <param name="datetime">現在時刻</param>
        protected override void WriteYearMonth(Range range, DateTime datetime)
        {
            range.Value2 = "      " + datetime.ToString("G");
        }

        #endregion

    }

    /// <summary>
    /// 未採取リスト(PMPF070F)のExcelライター
    /// </summary>
    internal class PMPF070FExcelWriter : AbstractExcelWriter
    {
        #region フィールド

        /// <summary>
        /// ExcelApplication
        /// </summary>
        private Application application = null;

        /// <summary>
        /// テンプレート用のワークブック
        /// </summary>
        private Workbook templateWorkBook = null;

        /// <summary>
        /// テンプレート用のワークシート
        /// </summary>
        private Worksheet templateWorkSheet = null;

        /// <summary>
        /// 出力用のワークブック
        /// </summary>
        private Workbook outputWorkBook = null;

        /// <summary>
        /// 出力用のワークシート
        /// </summary>
        private Worksheet outputWorkSheet = null;

        /// <summary>
        /// 抽出日
        /// </summary>
        private string extractionYMD = null;

        /// <summary>
        /// ドキュメントに対する操作完了時のコンテナ
        /// </summary>
        private IDocumentOperationContainer container;

        #endregion

        #region プロパティ

        /// <summary>
        /// 年
        /// </summary>
        internal string Year { get; set; }

        /// <summary>
        /// 月
        /// </summary>
        internal string Month { get; set; }

        /// <summary>
        /// テンプレートワークブックのパス
        /// </summary>
        internal string TemplateWorkBookPath { get; set; }

        /// <summary>
        /// 出力ワークブックのパス
        /// </summary>
        internal string OutputWorkBookPath { get; set; }

        /// <summary>
        /// 出力ワークブックのフォルダ
        /// </summary>
        internal string OutputWorkBookFolder { get; set; }

        /// <summary>
        /// 出力ワークブック名
        /// </summary>
        internal string OutputWorkBookName { get; set; }

        /// <summary>
        /// プレビューを表示するか
        /// </summary>
        internal bool IsShowPreview { get; set; }

        /// <summary>
        /// Excelシート名のマップ
        /// </summary>
        private static readonly IDictionary<PMPF070BExcelSheetType, string> excelSheetNameMap = null;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// 静的コンストラクタ
        /// </summary>
        static PMPF070FExcelWriter()
        {
            excelSheetNameMap
                = new Dictionary<PMPF070BExcelSheetType, string>();
            excelSheetNameMap.Add(PMPF070BExcelSheetType.Detail, PMGQ010BExcelSheetType.Detail.GetValue());
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        internal PMPF070FExcelWriter()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="container">ドキュメントに対する操作完了時のコンテナ</param>
        internal PMPF070FExcelWriter(IDocumentOperationContainer container)
        {
            this.container = container;
        }

        #endregion

        #region メソッド

        /// <summary>
        /// 初期化処理
        /// </summary>
        public override void Initialize()
        {
            try
            {
                // ExcelApplicationの取得
                application = ExcelUtils.GetExcelApplication();
                application.DisplayAlerts = false;

                // Excelテンプレート用のExcelワークブックの取得
                templateWorkBook = ExcelUtils.GetExcelTemplateWorkBook(application, TemplateWorkBookPath);

                // 出力用のExcelワークブックのフォルダ
                OutputWorkBookFolder = OutputWorkBookFolder ??
                    Path.GetDirectoryName(OutputWorkBookPath) + "\\";
                // 現在時刻の取得
                DateTimeNow = DateTime.Now;
                // 出力用のExcelワークブック名
                OutputWorkBookName = OutputWorkBookName ??
                    (Path.GetFileName(OutputWorkBookPath)).ConvertExcelWorkBookName(DateTimeNow);
                // 出力用のExcelワークブックの取得
                outputWorkBook = ExcelUtils.GetExcelOutputWorkBook(application,
                                                                   OutputWorkBookFolder,
                                                                   OutputWorkBookName,
                                                                   TemplateWorkBookPath, true, true);
            }

            catch (Exception)
            {
                // COMオブジェクトの解放
                ReleaseComObject();
                throw;
            }
        }

        /// <summary>
        /// 書込み前処理
        /// </summary>
        public override void PreWrite()
        {
        }

        /// <summary>
        /// ヘッダーの書込み
        /// </summary>
        /// <typeparam name="T">ヘッダーを継承した任意のオブジェクトの型</typeparam>
        /// <param name="header">ファイルのヘッダー情報</param>
        public override void WriteHeader<T>(T header)
        {
        }

        /// <summary>
        /// 詳細の書込み
        /// </summary>
        /// <typeparam name="T">詳細を継承した任意のオブジェクトの型</typeparam>
        /// <param name="detail">ファイルの詳細情報</param>
        public override void WriteDetail<T>(IList<T> detail)
        {
            try
            {
                // 詳細
                var detailData = new List<PMPF070BDTO>();
                var targets = detail[0] as UnHarvestingDetail;
                var items = targets.PMPF070BDTOList;
                items.ForEach(item =>
                {
                    detailData.Add(item);
                });
                // ワークシートの切替え(詳細)
                SwitchOutputWorkBookSheet(PMPF070BExcelSheetType.Detail);
                // 年月日の書込み(タイトルの右端)
                WriteYearMonth(outputWorkSheet.Range("$V$1"), DateTimeNow);
                // 詳細データの書込み
                WriteDetailData(detailData);
                var title = extractionYMD.Substring(0,2) + "/" + extractionYMD.Substring(2,2) + "/" + extractionYMD.Substring(4,2) + "_未採取リスト(PMPF070B)";
                // タイトルの書込み
                WriteTitle(outputWorkSheet.Range("$I$1"), title);
            }

            catch (Exception)
            {
                // COMオブジェクトの解放
                ReleaseComObject();
                throw;
            }
        }

        /// <summary>
        /// フッターの書込み
        /// </summary>
        /// <typeparam name="T">フッターを継承した任意のオブジェクトの型</typeparam>
        /// <param name="footer">ファイルのフッター情報</param>
        public override void WriteFooter<T>(T footer)
        {
            // 処理不要
        }

        /// <summary>
        /// 書込み後処理
        /// </summary>
        public override void PostWrite()
        {
            // 処理不要
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public override void Terminate()
        {
            try
            {
                var prefix = extractionYMD.Substring(0, 2)
                           + extractionYMD.Substring(2, 2)
                           + extractionYMD.Substring(4, 2)
                           + SymbolType.Underscore.GetValue();
                // ワークブックの保存
                outputWorkBook.SaveCopyAs(OutputWorkBookFolder + prefix + OutputWorkBookName);
                if (container != null)
                {
                    container.NotifyOutputExcel(OutputWorkBookFolder + prefix + OutputWorkBookName);
                }
            }

            catch (Exception)
            {
                throw;
            }

            finally
            {
                // COMオブジェクトの解放
                ReleaseComObject();
            }
        }

        /// <summary>
        /// COMオブジェクトの解放
        /// </summary>
        protected override void ReleaseComObject()
        {
            if (outputWorkBook != null)
            {
                outputWorkBook.Close(false, false, false);
            }

            if (templateWorkBook != null)
            {
                templateWorkBook.Close(false, false, false);
            }

            if (application != null)
            {
                application.Visible = false;
                application.DisplayAlerts = true;
                application.Quit();
                Marshal.ReleaseComObject(application);
                application = null;
            }

            if (outputWorkBook != null)
            {
                Marshal.ReleaseComObject(outputWorkBook);
                outputWorkBook = null;
            }

            if (templateWorkBook != null)
            {
                Marshal.ReleaseComObject(templateWorkBook);
                templateWorkBook = null;
            }

            if (templateWorkSheet != null)
            {
                Marshal.ReleaseComObject(templateWorkSheet);
                templateWorkSheet = null;
            }

            if (outputWorkSheet != null)
            {
                Marshal.ReleaseComObject(outputWorkSheet);
                outputWorkSheet = null;
            }
        }

        /// <summary>
        /// 出力用のExcelワークブックのシートの切替え
        /// </summary>
        /// <param name="excelSheetType">Excelシートの種類</param>
        private void SwitchOutputWorkBookSheet(PMPF070BExcelSheetType excelSheetType)
        {
            try
            {
                // 対象のワークシートの検索
                outputWorkSheet = ExcelUtils.SearchSheet(outputWorkBook, excelSheetNameMap[excelSheetType]);
                outputWorkSheet.Select(true);
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 詳細データの書込み
        /// </summary>
        /// <param name="list">詳細データ</param>
        private void WriteDetailData(IList<PMPF070BDTO> list)
        {
            Range range = null;
            int listCount = list.Count;

            // 罫線
            int startLeftRowPosition = 5;               // 左上の行番号
            int endRightRowPosition = listCount + 4;    // 右下の行番号

            try
            {
                // 印刷範囲の設定
                outputWorkSheet.PageSetup.PrintArea = @"$A$1:$X$" + (endRightRowPosition).ToString();

                // 罫線および値を出力するレンジの設定
                range = outputWorkSheet.Range("$A$" + (startLeftRowPosition).ToString(),
                                              "$X$" + (endRightRowPosition).ToString());

                range.Borders[XlBordersIndex.xlInsideHorizontal].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlInsideHorizontal].ColorIndex = 1;
                range.Borders[XlBordersIndex.xlInsideVertical].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlInsideVertical].ColorIndex = 1;
                range.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlEdgeTop].ColorIndex = 1;
                range.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlEdgeBottom].ColorIndex = 1;
                range.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlEdgeLeft].ColorIndex = 1;
                range.Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlEdgeRight].ColorIndex = 1;

                var outputData = new object[listCount, 24];
                for (int i = 0; i < listCount; ++i)
                {
                    // 受注№
                    outputData[i, 0] = list[i].OrderNo;
                    // 納期
                    outputData[i, 1] = list[i].DeliveryDate;
                    // 納期ランク
                    outputData[i, 2] = list[i].DeliveryDateRank;
                    // 需要家
                    outputData[i, 3] = list[i].CustomerName;
                    // ロット№
                    outputData[i, 4] = list[i].LotNo;
                    // ロール№
                    outputData[i, 5] = list[i].RollNo;
                    // 未ロット№
                    outputData[i, 6] = list[i].NotCollectedLot;
                    // 状態
                    outputData[i, 7] = list[i].Status;
                    // 計画規格コード
                    outputData[i, 8] = list[i].PlanStandardCode;
                    // 計画サイズ(厚)
                    outputData[i, 9] = list[i].PlanAtu;
                    // 計画サイズ(巾)
                    outputData[i, 10] = list[i].PlanHaba;
                    // 計画サイズ(長)
                    outputData[i, 11] = list[i].PlanNaga;
                    // 計画耳
                    outputData[i, 12] = list[i].PlanTM;
                    // 計画製品コード
                    outputData[i, 13] = list[i].PlanProductCode;
                    // 計画枚数
                    outputData[i, 14] = list[i].PlanMaisuu;
                    // 実績規格コード
                    outputData[i, 15] = list[i].PerformanceStandardCode;
                    // 実績サイズ(厚)
                    outputData[i, 16] = list[i].PerformanceAtu;
                    // 実績サイズ(巾)
                    outputData[i, 17] = list[i].PerformanceHaba;
                    // 実績サイズ(長)
                    outputData[i, 18] = list[i].PerformanceNaga;
                    // 実績耳
                    outputData[i, 19] = list[i].PerformanceTM;
                    // 実績製品コード
                    outputData[i, 20] = list[i].PerformanceProdutCode;
                    // 実績枚数
                    outputData[i, 21] = list[i].PerformanceMaisuu;
                    // 発生工程
                    outputData[i, 22] = list[i].GeneratingStep;
                    // 特殊
                    outputData[i, 23] = list[i].Special;
                    if (extractionYMD == null)
                    {
                        extractionYMD = list[i].ExtractionYMD;
                    }
                }

                range.Value2 = outputData;
            }

            catch (Exception)
            {
                throw;
            }

            finally
            {
                if (range != null)
                {
                    Marshal.ReleaseComObject(range);
                    range = null;
                }
            }
        }

        /// <summary>
        /// 年月日の書込み
        /// </summary>
        /// <param name="range">書込み範囲</param>
        /// <param name="datetime">現在時刻</param>
        protected override void WriteYearMonth(Range range, DateTime datetime)
        {
            range.Value2 = "       " + datetime.ToString("G");
        }

        #endregion
    }

    /// <summary>
    /// 余剰品引当可能在庫リスト(PMPF090F)のExcelライター
    /// </summary>
    internal class PMPF090FExcelWriter : AbstractExcelWriter
    {
        #region フィールド

        /// <summary>
        /// ExcelApplication
        /// </summary>
        private Application application = null;

        /// <summary>
        /// テンプレート用のワークブック
        /// </summary>
        private Workbook templateWorkBook = null;

        /// <summary>
        /// テンプレート用のワークシート
        /// </summary>
        private Worksheet templateWorkSheet = null;

        /// <summary>
        /// 出力用のワークブック
        /// </summary>
        private Workbook outputWorkBook = null;

        /// <summary>
        /// 出力用のワークシート
        /// </summary>
        private Worksheet outputWorkSheet = null;

        /// <summary>
        /// ドキュメントに対する操作完了時のコンテナ
        /// </summary>
        private IDocumentOperationContainer container;

        #endregion

        #region プロパティ

        /// <summary>
        /// 年
        /// </summary>
        internal string Year { get; set; }

        /// <summary>
        /// 月
        /// </summary>
        internal string Month { get; set; }

        /// <summary>
        /// テンプレートワークブックのパス
        /// </summary>
        internal string TemplateWorkBookPath { get; set; }

        /// <summary>
        /// 出力ワークブックのパス
        /// </summary>
        internal string OutputWorkBookPath { get; set; }

        /// <summary>
        /// 出力ワークブックのフォルダ
        /// </summary>
        internal string OutputWorkBookFolder { get; set; }

        /// <summary>
        /// 出力ワークブック名
        /// </summary>
        internal string OutputWorkBookName { get; set; }

        /// <summary>
        /// プレビューを表示するか
        /// </summary>
        internal bool IsShowPreview { get; set; }

        /// <summary>
        /// Excelシート名のマップ
        /// </summary>
        private static readonly IDictionary<PMPF090BExcelSheetType, string> excelSheetNameMap = null;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// 静的コンストラクタ
        /// </summary>
        static PMPF090FExcelWriter()
        {
            excelSheetNameMap
                = new Dictionary<PMPF090BExcelSheetType, string>();
            excelSheetNameMap.Add(PMPF090BExcelSheetType.Detail, PMPF090BExcelSheetType.Detail.GetValue());
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        internal PMPF090FExcelWriter()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="container">ドキュメントに対する操作完了時のコンテナ</param>
        internal PMPF090FExcelWriter(IDocumentOperationContainer container)
        {
            this.container = container;
        }

        #endregion

        #region メソッド

        /// <summary>
        /// 初期化処理
        /// </summary>
        public override void Initialize()
        {
            try
            {
                // ExcelApplicationの取得
                application = ExcelUtils.GetExcelApplication();
                application.DisplayAlerts = false;

                // Excelテンプレート用のExcelワークブックの取得
                templateWorkBook = ExcelUtils.GetExcelTemplateWorkBook(application, TemplateWorkBookPath);

                // 出力用のExcelワークブックのフォルダ
                OutputWorkBookFolder = OutputWorkBookFolder ??
                    Path.GetDirectoryName(OutputWorkBookPath) + "\\";
                // 現在時刻の取得
                DateTimeNow = DateTime.Now;
                // 出力用のExcelワークブック名
                OutputWorkBookName = OutputWorkBookName ??
                    (Path.GetFileName(OutputWorkBookPath)).ConvertExcelWorkBookName(DateTimeNow);
                // 出力用のExcelワークブックの取得
                outputWorkBook = ExcelUtils.GetExcelOutputWorkBook(application,
                                                                   OutputWorkBookFolder,
                                                                   OutputWorkBookName,
                                                                   TemplateWorkBookPath, true, true);
            }

            catch (Exception)
            {
                // COMオブジェクトの解放
                ReleaseComObject();
                throw;
            }
        }

        /// <summary>
        /// 書込み前処理
        /// </summary>
        public override void PreWrite()
        {
        }

        /// <summary>
        /// ヘッダーの書込み
        /// </summary>
        /// <typeparam name="T">ヘッダーを継承した任意のオブジェクトの型</typeparam>
        /// <param name="header">ファイルのヘッダー情報</param>
        public override void WriteHeader<T>(T header)
        {
        }

        /// <summary>
        /// 詳細の書込み
        /// </summary>
        /// <typeparam name="T">詳細を継承した任意のオブジェクトの型</typeparam>
        /// <param name="detail">ファイルの詳細情報</param>
        public override void WriteDetail<T>(IList<T> detail)
        {
            try
            {
                // 詳細
                var detailData = new List<PMPF090BDTO>();
                var targets = detail[0] as SurplusGoodsProvisionpPossibleStockDetail;
                var items = targets.PMPF090BDTOList;
                items.ForEach(item =>
                {
                    detailData.Add(item);
                });
                // ワークシートの切替え(詳細)
                SwitchOutputWorkBookSheet(PMPF090BExcelSheetType.Detail);
                // 年月日の書込み(タイトルの右端)
                WriteYearMonth(outputWorkSheet.Range("$O$1"), DateTimeNow);
                // 詳細データの書込み
                WriteDetailData(detailData);
            }

            catch (Exception)
            {
                // COMオブジェクトの解放
                ReleaseComObject();
                throw;
            }
        }

        /// <summary>
        /// フッターの書込み
        /// </summary>
        /// <typeparam name="T">フッターを継承した任意のオブジェクトの型</typeparam>
        /// <param name="footer">ファイルのフッター情報</param>
        public override void WriteFooter<T>(T footer)
        {
            // 処理不要
        }

        /// <summary>
        /// 書込み後処理
        /// </summary>
        public override void PostWrite()
        {
            // 処理不要
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public override void Terminate()
        {
            try
            {
                // ワークブックの保存
                outputWorkBook.Save(OutputWorkBookFolder, OutputWorkBookName);
                if (container != null)
                {
                    container.NotifyOutputExcel(OutputWorkBookFolder + OutputWorkBookName);
                }
            }

            catch (Exception)
            {
                throw;
            }

            finally
            {
                // COMオブジェクトの解放
                ReleaseComObject();
            }
        }

        /// <summary>
        /// COMオブジェクトの解放
        /// </summary>
        protected override void ReleaseComObject()
        {
            if (outputWorkBook != null)
            {
                outputWorkBook.Close(false, false, false);
            }

            if (templateWorkBook != null)
            {
                templateWorkBook.Close(false, false, false);
            }

            if (application != null)
            {
                application.Visible = false;
                application.DisplayAlerts = true;
                application.Quit();
                Marshal.ReleaseComObject(application);
                application = null;
            }

            if (outputWorkBook != null)
            {
                Marshal.ReleaseComObject(outputWorkBook);
                outputWorkBook = null;
            }

            if (templateWorkBook != null)
            {
                Marshal.ReleaseComObject(templateWorkBook);
                templateWorkBook = null;
            }

            if (templateWorkSheet != null)
            {
                Marshal.ReleaseComObject(templateWorkSheet);
                templateWorkSheet = null;
            }

            if (outputWorkSheet != null)
            {
                Marshal.ReleaseComObject(outputWorkSheet);
                outputWorkSheet = null;
            }
        }

        /// <summary>
        /// 出力用のExcelワークブックのシートの切替え
        /// </summary>
        /// <param name="excelSheetType">Excelシートの種類</param>
        private void SwitchOutputWorkBookSheet(PMPF090BExcelSheetType excelSheetType)
        {
            try
            {
                // 対象のワークシートの検索
                outputWorkSheet = ExcelUtils.SearchSheet(outputWorkBook, excelSheetNameMap[excelSheetType]);
                outputWorkSheet.Select(true);
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 詳細データの書込み
        /// </summary>
        /// <param name="list">詳細データ</param>
        private void WriteDetailData(IList<PMPF090BDTO> list)
        {
            Range range = null;
            int listCount = list.Count;

            // 罫線
            int startLeftRowPosition = 5;               // 左上の行番号
            int endRightRowPosition = listCount + 4;    // 右下の行番号

            try
            {
                // 印刷範囲の設定
                outputWorkSheet.PageSetup.PrintArea = @"$A$1:$P$" + (endRightRowPosition).ToString();
                // 罫線および値を出力するレンジの設定
                range = outputWorkSheet.Range("$A$" + (startLeftRowPosition).ToString(),
                                              "$P$" + (endRightRowPosition).ToString());

                range.Borders[XlBordersIndex.xlInsideHorizontal].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlInsideHorizontal].ColorIndex = 1;
                range.Borders[XlBordersIndex.xlInsideVertical].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlInsideVertical].ColorIndex = 1;
                range.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlEdgeTop].ColorIndex = 1;
                range.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlEdgeBottom].ColorIndex = 1;
                range.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlEdgeLeft].ColorIndex = 1;
                range.Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlEdgeRight].ColorIndex = 1;

                var outputData = new object[listCount, 16];
                for (int i = 0; i < listCount; ++i)
                {                  
                    // 規格名
                    outputData[i, 0] = list[i].StandardName;
                    // 製品サイズ(厚)
                    outputData[i, 1] = list[i].ProductAtu;
                    // 製品サイズ(巾)
                    outputData[i, 2] = list[i].ProductHaba;
                    // 製品サイズ(長)
                    outputData[i, 3] = list[i].ProductNaga;
                    // 定耳
                    outputData[i, 4] = list[i].TM;
                    // 製品コード
                    outputData[i, 5] = list[i].ProductCode;
                    // 置場
                    outputData[i, 6] = list[i].Yard;
                    if(!string.IsNullOrEmpty(list[i].Yard.Trim()))
                    {
                        // 枚数
                        outputData[i, 7] = list[i].Maisuu;
                    }
                    if(!string.IsNullOrEmpty(list[i].Yard.Trim()))
                    {
                        // 紐付枚数
                        outputData[i, 8] = list[i].TieMaisuu;
                    }
                    // 予約区分
                    outputData[i, 9] = list[i].ReservationClassfication;
                    // 未採取区分
                    outputData[i, 10] = list[i].NotCollectClassfication;
                    // 受注№
                    outputData[i, 11] = list[i].OrderNo;
                    // 需要家名
                    outputData[i, 12] = list[i].CustomerName;
                    // 未計画
                    outputData[i, 13] = list[i].UnPlan;
                    // 希望納期
                    outputData[i, 14] = list[i].DeliveryDate;
                    // 希望納期ランク
                    outputData[i, 15] = list[i].DeliveryDateRank;
                }

                range.Value2 = outputData;
            }

            catch (Exception)
            {
                throw;
            }

            finally
            {
                if (range != null)
                {
                    Marshal.ReleaseComObject(range);
                    range = null;
                }
            }
        }

        /// <summary>
        /// 年月日の書込み
        /// </summary>
        /// <param name="range">書込み範囲</param>
        /// <param name="datetime">現在時刻</param>
        protected override void WriteYearMonth(Range range, DateTime datetime)
        {
            range.Value2 = "  " + datetime.ToString("G");
        }

        #endregion
    }

    /// <summary>
    /// 外販出荷計画リスト(PQGA186F)のExcelライター
    /// </summary>
    internal class PQGA186FExcelWriter : AbstractExcelWriter
    {
        #region フィールド

        /// <summary>
        /// ExcelApplication
        /// </summary>
        private Application application = null;

        /// <summary>
        /// テンプレート用のワークブック
        /// </summary>
        private Workbook templateWorkBook = null;

        /// <summary>
        /// テンプレート用のワークシート
        /// </summary>
        private Worksheet templateWorkSheet = null;

        /// <summary>
        /// 出力用のワークブック
        /// </summary>
        private Workbook outputWorkBook = null;

        /// <summary>
        /// 出力用のワークシート
        /// </summary>
        private Worksheet outputWorkSheet = null;

        /// <summary>
        /// ドキュメントに対する操作完了時のコンテナ
        /// </summary>
        private IDocumentOperationContainer container;

        #endregion

        #region プロパティ

        /// <summary>
        /// 年
        /// </summary>
        internal string Year { get; set; }

        /// <summary>
        /// 月
        /// </summary>
        internal string Month { get; set; }

        /// <summary>
        /// テンプレートワークブックのパス
        /// </summary>
        internal string TemplateWorkBookPath { get; set; }

        /// <summary>
        /// 出力ワークブックのパス
        /// </summary>
        internal string OutputWorkBookPath { get; set; }

        /// <summary>
        /// 出力ワークブックのフォルダ
        /// </summary>
        internal string OutputWorkBookFolder { get; set; }

        /// <summary>
        /// 出力ワークブック名
        /// </summary>
        internal string OutputWorkBookName { get; set; }

        /// <summary>
        /// プレビューを表示するか
        /// </summary>
        internal bool IsShowPreview { get; set; }

        /// <summary>
        /// Excelシート名のマップ
        /// </summary>
        private static readonly IDictionary<PQGA186BExcelSheetType, string> excelSheetNameMap = null;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// 静的コンストラクタ
        /// </summary>
        static PQGA186FExcelWriter()
        {
            excelSheetNameMap
                = new Dictionary<PQGA186BExcelSheetType, string>();
            excelSheetNameMap.Add(PQGA186BExcelSheetType.Detail, PQGA186BExcelSheetType.Detail.GetValue());
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        internal PQGA186FExcelWriter()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="container">ドキュメントに対する操作完了時のコンテナ</param>
        internal PQGA186FExcelWriter(IDocumentOperationContainer container)
        {
            this.container = container;
        }

        #endregion

        #region メソッド

        /// <summary>
        /// 初期化処理
        /// </summary>
        public override void Initialize()
        {
            try
            {
                // ExcelApplicationの取得
                application = ExcelUtils.GetExcelApplication();
                application.DisplayAlerts = false;

                // Excelテンプレート用のExcelワークブックの取得
                templateWorkBook = ExcelUtils.GetExcelTemplateWorkBook(application, TemplateWorkBookPath);

                // 出力用のExcelワークブックのフォルダ
                OutputWorkBookFolder = OutputWorkBookFolder ??
                    Path.GetDirectoryName(OutputWorkBookPath) + "\\";
                // 現在時刻の取得
                DateTimeNow = DateTime.Now;
                // 出力用のExcelワークブック名
                OutputWorkBookName = OutputWorkBookName ??
                    (Path.GetFileName(OutputWorkBookPath)).ConvertExcelWorkBookName(DateTimeNow);
                // 出力用のExcelワークブックの取得
                outputWorkBook = ExcelUtils.GetExcelOutputWorkBook(application,
                                                                   OutputWorkBookFolder,
                                                                   OutputWorkBookName,
                                                                   TemplateWorkBookPath, true, true);
            }

            catch (Exception)
            {
                // COMオブジェクトの解放
                ReleaseComObject();
                throw;
            }
        }

        /// <summary>
        /// 書込み前処理
        /// </summary>
        public override void PreWrite()
        {
        }

        /// <summary>
        /// ヘッダーの書込み
        /// </summary>
        /// <typeparam name="T">ヘッダーを継承した任意のオブジェクトの型</typeparam>
        /// <param name="header">ファイルのヘッダー情報</param>
        public override void WriteHeader<T>(T header)
        {
        }

        /// <summary>
        /// 詳細の書込み
        /// </summary>
        /// <typeparam name="T">詳細を継承した任意のオブジェクトの型</typeparam>
        /// <param name="detail">ファイルの詳細情報</param>
        public override void WriteDetail<T>(IList<T> detail)
        {
            try
            {
                // 詳細
                var detailData = new List<PQGA186BDTO>();
                var targets = detail[0] as ExternalSalesShipmentPlanDetail;
                var items = targets.PQGA186BDTOList;
                var groupingSlabList = new List<SlabDTO>();

                items.ForEach(item =>
                {
                    detailData.Add(item);
                });

                var groupList = detailData.GroupBy(item => item.StandardName);
                var count = groupList.Count();

                foreach (var group in groupList)
                {
                    var slabDTO = new SlabDTO();
                    slabDTO.StandardName = group.Key;
                    slabDTO.SlabCount = group.Count();
                    slabDTO.SlabWeight = group.Sum(item => item.UnitWeight);
                    groupingSlabList.Add(slabDTO);
                }

                // ワークシートの切替え(詳細)
                SwitchOutputWorkBookSheet(PQGA186BExcelSheetType.Detail);
                // 年月日の書込み(タイトルの右端)
                WriteYearMonth(outputWorkSheet.Range("$I$1"), DateTimeNow);
                // 詳細データの書込み
                WriteDetailData(detailData, groupingSlabList);
            }

            catch (Exception)
            {
                // COMオブジェクトの解放
                ReleaseComObject();
                throw;
            }
        }

        /// <summary>
        /// フッターの書込み
        /// </summary>
        /// <typeparam name="T">フッターを継承した任意のオブジェクトの型</typeparam>
        /// <param name="footer">ファイルのフッター情報</param>
        public override void WriteFooter<T>(T footer)
        {
            // 処理不要
        }

        /// <summary>
        /// 書込み後処理
        /// </summary>
        public override void PostWrite()
        {
            // 処理不要
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public override void Terminate()
        {
            try
            {
                // ワークブックの保存
                outputWorkBook.Save(OutputWorkBookFolder, OutputWorkBookName);
                if (container != null)
                {
                    container.NotifyOutputExcel(OutputWorkBookFolder + OutputWorkBookName);
                }
            }

            catch (Exception)
            {
                throw;
            }

            finally
            {
                // COMオブジェクトの解放
                ReleaseComObject();
            }
        }

        /// <summary>
        /// COMオブジェクトの解放
        /// </summary>
        protected override void ReleaseComObject()
        {
            if (outputWorkBook != null)
            {
                outputWorkBook.Close(false, false, false);
            }

            if (templateWorkBook != null)
            {
                templateWorkBook.Close(false, false, false);
            }

            if (application != null)
            {
                application.Visible = false;
                application.DisplayAlerts = true;
                application.Quit();
                Marshal.ReleaseComObject(application);
                application = null;
            }

            if (outputWorkBook != null)
            {
                Marshal.ReleaseComObject(outputWorkBook);
                outputWorkBook = null;
            }

            if (templateWorkBook != null)
            {
                Marshal.ReleaseComObject(templateWorkBook);
                templateWorkBook = null;
            }

            if (templateWorkSheet != null)
            {
                Marshal.ReleaseComObject(templateWorkSheet);
                templateWorkSheet = null;
            }

            if (outputWorkSheet != null)
            {
                Marshal.ReleaseComObject(outputWorkSheet);
                outputWorkSheet = null;
            }
        }

        /// <summary>
        /// 出力用のExcelワークブックのシートの切替え
        /// </summary>
        /// <param name="excelSheetType">Excelシートの種類</param>
        private void SwitchOutputWorkBookSheet(PQGA186BExcelSheetType excelSheetType)
        {
            try
            {
                // 対象のワークシートの検索
                outputWorkSheet = ExcelUtils.SearchSheet(outputWorkBook, excelSheetNameMap[excelSheetType]);
                outputWorkSheet.Select(true);
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 詳細データの書込み
        /// </summary>
        /// <param name="list">詳細データ</param>
        /// <param name="groupingInfo">規格別計画量</param>
        private void WriteDetailData(IList<PQGA186BDTO> list, IList<SlabDTO> slabInfo)
        {
            Range rangeDetail = null;
            Range rangeGrouping = null;
            Range rangeAllTotal = null;
            int listCount = list.Count;
            int groupingInfoCount = slabInfo.Count;

            // 罫線
            int detailStartLeftRowPosition = 5;               // 左上の行番号
            int detailEndRightRowPosition = listCount + 4;    // 右下の行番号

            try
            {
                // 印刷範囲の設定
                outputWorkSheet.PageSetup.PrintArea = @"$A$1:$I$" + (detailEndRightRowPosition + 2 + groupingInfoCount + 1).ToString();
                // 罫線および値を出力するレンジの設定
                rangeDetail = outputWorkSheet.Range("$A$" + (detailStartLeftRowPosition).ToString(),
                                                    "$I$" + (detailEndRightRowPosition).ToString());

                rangeDetail.Borders[XlBordersIndex.xlInsideHorizontal].LineStyle = XlLineStyle.xlContinuous;
                rangeDetail.Borders[XlBordersIndex.xlInsideHorizontal].ColorIndex = 1;
                rangeDetail.Borders[XlBordersIndex.xlInsideVertical].LineStyle = XlLineStyle.xlContinuous;
                rangeDetail.Borders[XlBordersIndex.xlInsideVertical].ColorIndex = 1;
                rangeDetail.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
                rangeDetail.Borders[XlBordersIndex.xlEdgeTop].ColorIndex = 1;
                rangeDetail.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
                rangeDetail.Borders[XlBordersIndex.xlEdgeBottom].ColorIndex = 1;
                rangeDetail.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
                rangeDetail.Borders[XlBordersIndex.xlEdgeLeft].ColorIndex = 1;
                rangeDetail.Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;
                rangeDetail.Borders[XlBordersIndex.xlEdgeRight].ColorIndex = 1;

                var outputData = new object[listCount, 9];
                for (int i = 0; i < listCount; ++i)
                {
                    // 発行№
                    outputData[i, 0] = list[i].IssuNoAndDate;
                    // チャージ№
                    outputData[i, 1] = list[i].CHNo;
                    // 規格名
                    outputData[i, 2] = list[i].StandardName;
                    // スラブサイズ(厚)
                    outputData[i, 3] = list[i].SlabAtu;
                    // スラブサイズ(巾)
                    outputData[i, 4] = list[i].SlabHaba;
                    // スラブサイズ(長)
                    outputData[i, 5] = list[i].SlabNaga;
                    // 重量
                    outputData[i, 6] = list[i].UnitWeight;
                    // 受注№
                    outputData[i, 7] = list[i].OrderNo;
                    // 計画№
                    outputData[i, 8] = list[i].PlanNo;
                }

                rangeDetail.Value2 = outputData;

                // 罫線(規格別計画量)
                int groupingStartLeftRowPosition 
                    = detailEndRightRowPosition + 2;                           // 左上の行番号
                int groupingEndRightRowPosition 
                    = groupingStartLeftRowPosition + groupingInfoCount + 1;    // 右下の行番号

                // 罫線および値を出力するレンジの設定(規格別計画量)
                rangeGrouping = outputWorkSheet.Range("$C$" + (groupingStartLeftRowPosition).ToString(),
                                                      "$H$" + (groupingEndRightRowPosition).ToString());

                rangeGrouping.Borders[XlBordersIndex.xlInsideHorizontal].LineStyle = XlLineStyle.xlContinuous;
                rangeGrouping.Borders[XlBordersIndex.xlInsideHorizontal].ColorIndex = 1;
                rangeGrouping.Borders[XlBordersIndex.xlInsideVertical].LineStyle = XlLineStyle.xlContinuous;
                rangeGrouping.Borders[XlBordersIndex.xlInsideVertical].ColorIndex = 1;
                rangeGrouping.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
                rangeGrouping.Borders[XlBordersIndex.xlEdgeTop].ColorIndex = 1;
                rangeGrouping.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
                rangeGrouping.Borders[XlBordersIndex.xlEdgeBottom].ColorIndex = 1;
                rangeGrouping.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
                rangeGrouping.Borders[XlBordersIndex.xlEdgeLeft].ColorIndex = 1;
                rangeGrouping.Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;
                rangeGrouping.Borders[XlBordersIndex.xlEdgeRight].ColorIndex = 1;

                // 規格別計画量
                outputWorkSheet.Range("$C$" + (groupingStartLeftRowPosition).ToString()).Value2 = "規格別計画量";
                outputWorkSheet.Range("$C$" + (groupingStartLeftRowPosition).ToString()).HorizontalAlignment = Constants.xlCenter;

                // 本数
                outputWorkSheet.CellMerge("$D$" + groupingStartLeftRowPosition.ToString(),"$F$" + groupingStartLeftRowPosition.ToString());
                outputWorkSheet.Range("$D$" + (groupingStartLeftRowPosition).ToString()).Value2 = "本数";
                outputWorkSheet.Range("$D$" + (groupingStartLeftRowPosition).ToString()).HorizontalAlignment = Constants.xlCenter;

                // 重量
                outputWorkSheet.CellMerge("$G$" + groupingStartLeftRowPosition.ToString(), "$H$" + groupingStartLeftRowPosition.ToString());
                outputWorkSheet.Range("$G$" + (groupingStartLeftRowPosition).ToString()).Value2 = "重量(kg)";
                outputWorkSheet.Range("$G$" + (groupingStartLeftRowPosition).ToString()).HorizontalAlignment = Constants.xlCenter;

                int offset = 1;
                int count = 0;
                int weight = 0;
                int countSum = 0;
                int countWeight = 0;
                foreach (var group in slabInfo)
                {
                    // 規格別計画量(値)
                    outputWorkSheet.Range("$C$" + (groupingStartLeftRowPosition + offset).ToString()).Value2 = group.StandardName;
                    outputWorkSheet.Range("$C$" + (groupingStartLeftRowPosition + offset).ToString()).HorizontalAlignment = Constants.xlLeft;

                    // 本数(値)
                    count = group.SlabCount;
                    countSum = countSum + count;
                    outputWorkSheet.CellMerge("$D$" + (groupingStartLeftRowPosition + offset).ToString(), "$F$" + (groupingStartLeftRowPosition + offset).ToString());
                    outputWorkSheet.Range("$D$" + (groupingStartLeftRowPosition + offset).ToString()).Value2 = count;
                    outputWorkSheet.Range("$D$" + (groupingStartLeftRowPosition + offset).ToString()).HorizontalAlignment = Constants.xlRight;
                    outputWorkSheet.Range("$D$" + (groupingStartLeftRowPosition + offset).ToString()).NumberFormatLocal = "#,###";

                    // 重量(値)
                    weight = group.SlabWeight;
                    countWeight = countWeight + weight;
                    outputWorkSheet.CellMerge("$G$" + (groupingStartLeftRowPosition + offset).ToString(), "$H$" + (groupingStartLeftRowPosition + offset).ToString());
                    outputWorkSheet.Range("$G$" + (groupingStartLeftRowPosition + offset).ToString()).Value2 = weight;
                    outputWorkSheet.Range("$G$" + (groupingStartLeftRowPosition + offset).ToString()).HorizontalAlignment = Constants.xlRight;
                    outputWorkSheet.Range("$G$" + (groupingStartLeftRowPosition + offset).ToString()).NumberFormatLocal = "#,###";
                    offset++;
                }

                // 規格別計画量(総合計)
                outputWorkSheet.Range("$C$" + (groupingStartLeftRowPosition + offset).ToString()).Value2 = "総合計";
                outputWorkSheet.Range("$C$" + (groupingStartLeftRowPosition + offset).ToString()).HorizontalAlignment = Constants.xlLeft;

                // 規格別計画量(本数)
                outputWorkSheet.CellMerge("$D$" + (groupingStartLeftRowPosition + offset).ToString(), "$F$" + (groupingStartLeftRowPosition + offset).ToString());
                outputWorkSheet.Range("$D$" + (groupingStartLeftRowPosition + offset).ToString()).Value2 = countSum;
                outputWorkSheet.Range("$D$" + (groupingStartLeftRowPosition + offset).ToString()).HorizontalAlignment = Constants.xlRight;
                outputWorkSheet.Range("$D$" + (groupingStartLeftRowPosition + offset).ToString()).NumberFormatLocal = "#,###";

                // 規格別計画量(重量)
                outputWorkSheet.CellMerge("$G$" + (groupingStartLeftRowPosition + offset).ToString(), "$H$" + (groupingStartLeftRowPosition + offset).ToString());
                outputWorkSheet.Range("$G$" + (groupingStartLeftRowPosition + offset).ToString()).Value2 = countWeight;
                outputWorkSheet.Range("$G$" + (groupingStartLeftRowPosition + offset).ToString()).HorizontalAlignment = Constants.xlRight;
                outputWorkSheet.Range("$G$" + (groupingStartLeftRowPosition + offset).ToString()).NumberFormatLocal = "#,###";

                rangeAllTotal = outputWorkSheet.Range("$C$" + (groupingStartLeftRowPosition + offset).ToString(),
                                                      "$H$" + (groupingStartLeftRowPosition + offset).ToString());

                rangeAllTotal.Borders[XlBordersIndex.xlInsideHorizontal].LineStyle = XlLineStyle.xlContinuous;
                rangeAllTotal.Borders[XlBordersIndex.xlInsideHorizontal].ColorIndex = 1;
                rangeAllTotal.Borders[XlBordersIndex.xlInsideVertical].LineStyle = XlLineStyle.xlContinuous;
                rangeAllTotal.Borders[XlBordersIndex.xlInsideVertical].ColorIndex = 1;
                rangeAllTotal.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlDouble;
                rangeAllTotal.Borders[XlBordersIndex.xlEdgeTop].ColorIndex = 1;
                rangeAllTotal.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
                rangeAllTotal.Borders[XlBordersIndex.xlEdgeBottom].ColorIndex = 1;
                rangeAllTotal.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
                rangeAllTotal.Borders[XlBordersIndex.xlEdgeLeft].ColorIndex = 1;
                rangeAllTotal.Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;
                rangeAllTotal.Borders[XlBordersIndex.xlEdgeRight].ColorIndex = 1;
            }

            catch (Exception)
            {
                throw;
            }

            finally
            {
                if (rangeDetail != null)
                {
                    Marshal.ReleaseComObject(rangeDetail);
                    rangeDetail = null;
                }

                if (rangeGrouping != null)
                {
                    Marshal.ReleaseComObject(rangeGrouping);
                    rangeGrouping = null;
                }
            }
        }

        #endregion

    }

    /// <summary>
    /// スラブのDTO
    /// </summary>
    internal class SlabDTO : DTOBase
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        internal SlabDTO()
        {
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 規格名
        /// </summary>
        internal string StandardName { get; set; }

        /// <summary>
        /// スラブ本数
        /// </summary>
        internal int SlabCount { get; set; }

        /// <summary>
        /// スラブ重量
        /// </summary>
        internal int SlabWeight { get; set; }

        #endregion
    }

    /// <summary>
    /// 外販出荷実績報告書(PQGA380F)のExcelライター
    /// </summary>
    internal class PQGA380FExcelWriter : AbstractExcelWriter
    {
        #region フィールド

        /// <summary>
        /// ExcelApplication
        /// </summary>
        private Application application = null;

        /// <summary>
        /// テンプレート用のワークブック
        /// </summary>
        private Workbook templateWorkBook = null;

        /// <summary>
        /// テンプレート用のワークシート
        /// </summary>
        private Worksheet templateWorkSheet = null;

        /// <summary>
        /// 出力用のワークブック
        /// </summary>
        private Workbook outputWorkBook = null;

        /// <summary>
        /// 出力用のワークシート
        /// </summary>
        private Worksheet outputWorkSheet = null;

        /// <summary>
        /// ドキュメントに対する操作完了時のコンテナ
        /// </summary>
        private IDocumentOperationContainer container;

        #endregion

        #region プロパティ

        /// <summary>
        /// 年
        /// </summary>
        internal string Year { get; set; }

        /// <summary>
        /// 月
        /// </summary>
        internal string Month { get; set; }

        /// <summary>
        /// テンプレートワークブックのパス
        /// </summary>
        internal string TemplateWorkBookPath { get; set; }

        /// <summary>
        /// 出力ワークブックのパス
        /// </summary>
        internal string OutputWorkBookPath { get; set; }

        /// <summary>
        /// 出力ワークブックのフォルダ
        /// </summary>
        internal string OutputWorkBookFolder { get; set; }

        /// <summary>
        /// 出力ワークブック名
        /// </summary>
        internal string OutputWorkBookName { get; set; }

        /// <summary>
        /// プレビューを表示するか
        /// </summary>
        internal bool IsShowPreview { get; set; }

        /// <summary>
        /// Excelシート名のマップ
        /// </summary>
        private static readonly IDictionary<PQGA380BExcelSheetType, string> excelSheetNameMap = null;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// 静的コンストラクタ
        /// </summary>
        static PQGA380FExcelWriter()
        {
            excelSheetNameMap
                = new Dictionary<PQGA380BExcelSheetType, string>();
            excelSheetNameMap.Add(PQGA380BExcelSheetType.Detail, PQGA380BExcelSheetType.Detail.GetValue());
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        internal PQGA380FExcelWriter()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="container">ドキュメントに対する操作完了時のコンテナ</param>
        internal PQGA380FExcelWriter(IDocumentOperationContainer container)
        {
            this.container = container;
        }

        #endregion

        #region メソッド

        /// <summary>
        /// 初期化処理
        /// </summary>
        public override void Initialize()
        {
            try
            {
                // ExcelApplicationの取得
                application = ExcelUtils.GetExcelApplication();
                application.DisplayAlerts = false;

                // Excelテンプレート用のExcelワークブックの取得
                templateWorkBook = ExcelUtils.GetExcelTemplateWorkBook(application, TemplateWorkBookPath);

                // 出力用のExcelワークブックのフォルダ
                OutputWorkBookFolder = OutputWorkBookFolder ??
                    Path.GetDirectoryName(OutputWorkBookPath) + "\\";
                // 現在時刻の取得
                DateTimeNow = DateTime.Now;
                // 出力用のExcelワークブック名
                OutputWorkBookName = OutputWorkBookName ??
                    (Path.GetFileName(OutputWorkBookPath)).ConvertExcelWorkBookName(DateTimeNow);
                // 出力用のExcelワークブックの取得
                outputWorkBook = ExcelUtils.GetExcelOutputWorkBook(application,
                                                                   OutputWorkBookFolder,
                                                                   OutputWorkBookName,
                                                                   TemplateWorkBookPath, true, true);
            }

            catch (Exception)
            {
                // COMオブジェクトの解放
                ReleaseComObject();
                throw;
            }
        }

        /// <summary>
        /// 書込み前処理
        /// </summary>
        public override void PreWrite()
        {
        }

        /// <summary>
        /// ヘッダーの書込み
        /// </summary>
        /// <typeparam name="T">ヘッダーを継承した任意のオブジェクトの型</typeparam>
        /// <param name="header">ファイルのヘッダー情報</param>
        public override void WriteHeader<T>(T header)
        {
        }

        /// <summary>
        /// 詳細の書込み
        /// </summary>
        /// <typeparam name="T">詳細を継承した任意のオブジェクトの型</typeparam>
        /// <param name="detail">ファイルの詳細情報</param>
        public override void WriteDetail<T>(IList<T> detail)
        {
            try
            {
                // 詳細
                var detailData = new List<PQGA380BDTO>();
                var groupingInfo = new List<ShipmentDTO>();
                var targets = detail[0] as ExternalSalesShipmentPerformanceDetail;
                var items = targets.PQGA380BDTOList;
                items.ForEach(item =>
                {
                    detailData.Add(item);
                });

                var groupList = detailData.GroupBy(item => item.OrderNo);
                var count = groupList.Count();

                foreach (var group in groupList)
                {
                    var shipmentDTO = new ShipmentDTO();
                    shipmentDTO.OrderNo = group.Key;
                    shipmentDTO.ShipmentHonsuu = group.Sum(item => item.ShipmentHonsuu);
                    shipmentDTO.ShipmentWeight = group.Sum(item => item.ShipmentWeight);
                    groupingInfo.Add(shipmentDTO);
                }

                // ワークシートの切替え(詳細)
                SwitchOutputWorkBookSheet(PQGA380BExcelSheetType.Detail);
                // 年月日の書込み(タイトルの右端)
                WriteYearMonth(outputWorkSheet.Range("$Y$1"), DateTimeNow);
                // 詳細データの書込み
                WriteDetailData(detailData, groupingInfo);
            }

            catch (Exception)
            {
                // COMオブジェクトの解放
                ReleaseComObject();
                throw;
            }
        }

        /// <summary>
        /// フッターの書込み
        /// </summary>
        /// <typeparam name="T">フッターを継承した任意のオブジェクトの型</typeparam>
        /// <param name="footer">ファイルのフッター情報</param>
        public override void WriteFooter<T>(T footer)
        {
            // 処理不要
        }

        /// <summary>
        /// 書込み後処理
        /// </summary>
        public override void PostWrite()
        {
            // 処理不要
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public override void Terminate()
        {
            try
            {
                // ワークブックの保存
                outputWorkBook.Save(OutputWorkBookFolder, OutputWorkBookName);
                if (container != null)
                {
                    container.NotifyOutputExcel(OutputWorkBookFolder + OutputWorkBookName);
                }
            }

            catch (Exception)
            {
                throw;
            }

            finally
            {
                // COMオブジェクトの解放
                ReleaseComObject();
            }
        }

        /// <summary>
        /// COMオブジェクトの解放
        /// </summary>
        protected override void ReleaseComObject()
        {
            if (outputWorkBook != null)
            {
                outputWorkBook.Close(false, false, false);
            }

            if (templateWorkBook != null)
            {
                templateWorkBook.Close(false, false, false);
            }

            if (application != null)
            {
                application.Visible = false;
                application.DisplayAlerts = true;
                application.Quit();
                Marshal.ReleaseComObject(application);
                application = null;
            }

            if (outputWorkBook != null)
            {
                Marshal.ReleaseComObject(outputWorkBook);
                outputWorkBook = null;
            }

            if (templateWorkBook != null)
            {
                Marshal.ReleaseComObject(templateWorkBook);
                templateWorkBook = null;
            }

            if (templateWorkSheet != null)
            {
                Marshal.ReleaseComObject(templateWorkSheet);
                templateWorkSheet = null;
            }

            if (outputWorkSheet != null)
            {
                Marshal.ReleaseComObject(outputWorkSheet);
                outputWorkSheet = null;
            }
        }

        /// <summary>
        /// 出力用のExcelワークブックのシートの切替え
        /// </summary>
        /// <param name="excelSheetType">Excelシートの種類</param>
        private void SwitchOutputWorkBookSheet(PQGA380BExcelSheetType excelSheetType)
        {
            try
            {
                // 対象のワークシートの検索
                outputWorkSheet = ExcelUtils.SearchSheet(outputWorkBook, excelSheetNameMap[excelSheetType]);
                outputWorkSheet.Select(true);
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 詳細データの書込み
        /// </summary>
        /// <param name="list">詳細データ</param>
        private void WriteDetailData(IList<PQGA380BDTO> list, IList<ShipmentDTO> groupingInfo)
        {
            Range rangeDetail = null;
            Range rangeGrouping = null;
            Range rangeAllTotal = null;
            int listCount = list.Count;
            int groupingInfoCount = groupingInfo.Count;

            // 罫線
            int detailStartLeftRowPosition = 6;               // 左上の行番号
            int detailEndRightRowPosition = listCount + 5;    // 右下の行番号

            try
            {
                // 印刷範囲の設定
                outputWorkSheet.PageSetup.PrintArea = "$A$1:$Z$" + (detailEndRightRowPosition + 5 + groupingInfoCount + 1).ToString();
                // 罫線および値を出力するレンジの設定
                rangeDetail = outputWorkSheet.Range("$A$" + (detailStartLeftRowPosition).ToString(),
                                                    "$Z$" + (detailEndRightRowPosition).ToString());
                rangeDetail.Borders[XlBordersIndex.xlInsideHorizontal].LineStyle = XlLineStyle.xlContinuous;
                rangeDetail.Borders[XlBordersIndex.xlInsideHorizontal].ColorIndex = 1;
                rangeDetail.Borders[XlBordersIndex.xlInsideVertical].LineStyle = XlLineStyle.xlContinuous;
                rangeDetail.Borders[XlBordersIndex.xlInsideVertical].ColorIndex = 1;
                rangeDetail.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
                rangeDetail.Borders[XlBordersIndex.xlEdgeTop].ColorIndex = 1;
                rangeDetail.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
                rangeDetail.Borders[XlBordersIndex.xlEdgeBottom].ColorIndex = 1;
                rangeDetail.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
                rangeDetail.Borders[XlBordersIndex.xlEdgeLeft].ColorIndex = 1;
                rangeDetail.Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;
                rangeDetail.Borders[XlBordersIndex.xlEdgeRight].ColorIndex = 1;

                string extractionYMD = string.Empty;
                var outputData = new object[listCount, 26];
                for (int i = 0; i < listCount; ++i)
                {
                    // 受注№
                    outputData[i, 0] = list[i].OrderNo;                    
                    // 需要家名
                    outputData[i, 1] = list[i].CustomerName;                    
                    // 都市
                    outputData[i, 2] = list[i].City;
                    // 回収
                    outputData[i, 3] = list[i].Collectiion;
                    // 品種
                    outputData[i, 4] = list[i].Kind;
                    // チャージ№
                    outputData[i, 5] = list[i].CHNo;
                    // １次№①
                    outputData[i, 6] = list[i].Slab1No1;
                    // １次№②
                    outputData[i, 7] = list[i].Slab1No2;
                    // １次№③
                    outputData[i, 8] = list[i].Slab1No3;
                    // １次№④
                    outputData[i, 9] = list[i].Slab1No4;
                    // １次№⑤
                    outputData[i, 10] = list[i].Slab1No5;
                    // １次№⑥
                    outputData[i, 11] = list[i].Slab1No6;
                    // １次№⑦
                    outputData[i, 12] = list[i].Slab1No7;
                    // １次№⑧
                    outputData[i, 13] = list[i].Slab1No8;
                    // １次№⑨
                    outputData[i, 14] = list[i].Slab1No9;
                    // １次№⑩
                    outputData[i, 15] = list[i].Slab1No10;                   
                    // 決済
                    outputData[i, 16] = list[i].SettlementCondition;
                    // 商社名
                    outputData[i, 17] = list[i].DisutributorName;
                    // 規格名
                    outputData[i, 18] = list[i].StandardName;
                    // 製品サイズ(厚)
                    outputData[i, 19] = list[i].ProductAtu;
                    // 製品サイズ(巾)
                    outputData[i, 20] = list[i].ProductHaba;
                    // 製品サイズ(長)
                    outputData[i, 21] = list[i].ProductNaga;
                    // 出荷本数
                    outputData[i, 22] = list[i].ShipmentHonsuu;
                    // 出荷重量
                    outputData[i, 23] = list[i].ShipmentWeight;
                    // 単価
                    outputData[i, 24] = list[i].UnitPrice;
                    // 受渡条件
                    outputData[i, 25] = list[i].TransferCondition;
                    if(i == 0)
                    {
                        extractionYMD = list[i].ExtractionYMD;
                    }
                }

                rangeDetail.Value2 = outputData;
                // 出荷日
                outputWorkSheet.Range("$B$2").Value2 = extractionYMD.Substring(0, 2)
                                                    + SymbolType.Slash.GetValue()
                                                    + extractionYMD.Substring(2, 2)
                                                    + SymbolType.Slash.GetValue()
                                                    + extractionYMD.Substring(4, 2);

                // 罫線(受注№小計)
                int groupingStartLeftRowPosition
                    = detailEndRightRowPosition + 3;                            // 左上の行番号
                int groupingEndRightRowPosition
                    = groupingStartLeftRowPosition + groupingInfoCount;         // 右下の行番号

                // 罫線および値を出力するレンジの設定
                rangeGrouping = outputWorkSheet.Range("$T$" + (groupingStartLeftRowPosition).ToString(),
                                                      "$X$" + (groupingEndRightRowPosition).ToString());

                rangeGrouping.Borders[XlBordersIndex.xlInsideHorizontal].LineStyle = XlLineStyle.xlContinuous;
                rangeGrouping.Borders[XlBordersIndex.xlInsideHorizontal].ColorIndex = 1;
                rangeGrouping.Borders[XlBordersIndex.xlInsideVertical].LineStyle = XlLineStyle.xlContinuous;
                rangeGrouping.Borders[XlBordersIndex.xlInsideVertical].ColorIndex = 1;
                rangeGrouping.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
                rangeGrouping.Borders[XlBordersIndex.xlEdgeTop].ColorIndex = 1;
                rangeGrouping.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
                rangeGrouping.Borders[XlBordersIndex.xlEdgeBottom].ColorIndex = 1;
                rangeGrouping.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
                rangeGrouping.Borders[XlBordersIndex.xlEdgeLeft].ColorIndex = 1;
                rangeGrouping.Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;
                rangeGrouping.Borders[XlBordersIndex.xlEdgeRight].ColorIndex = 1;

                // 受注No.合計
                outputWorkSheet.CellMerge("$T$" + groupingStartLeftRowPosition.ToString(), "$V$" + groupingStartLeftRowPosition.ToString());
                outputWorkSheet.Range("$T$" + (groupingStartLeftRowPosition).ToString()).Value2 = "受注№小計";
                outputWorkSheet.Range("$T$" + (groupingStartLeftRowPosition).ToString()).HorizontalAlignment = Constants.xlLeft;

                // 本数
                outputWorkSheet.Range("$W$" + (groupingStartLeftRowPosition).ToString()).Value2 = "本数";
                outputWorkSheet.Range("$W$" + (groupingStartLeftRowPosition).ToString()).HorizontalAlignment = Constants.xlCenter;

                // 重量
                outputWorkSheet.Range("$X$" + (groupingStartLeftRowPosition).ToString()).Value2 = "重量(kg)";
                outputWorkSheet.Range("$X$" + (groupingStartLeftRowPosition).ToString()).HorizontalAlignment = Constants.xlCenter;

                int offset = 1;
                int count = 0;
                int weight = 0;
                int countSum = 0;
                int countWeight = 0;
                foreach (var group in groupingInfo)
                {
                    // 受注No.合計(値)
                    outputWorkSheet.CellMerge("$T$" + (groupingStartLeftRowPosition + offset).ToString(), "$V$" + (groupingStartLeftRowPosition + offset).ToString());
                    outputWorkSheet.Range("$T$" + (groupingStartLeftRowPosition + offset).ToString()).Value2 = group.OrderNo;
                    outputWorkSheet.Range("$T$" + (groupingStartLeftRowPosition + offset).ToString()).HorizontalAlignment = Constants.xlLeft;

                    // 本数(値)
                    count = group.ShipmentHonsuu;
                    countSum = countSum + count;
                    outputWorkSheet.Range("$W$" + (groupingStartLeftRowPosition + offset).ToString()).Value2 = count;
                    outputWorkSheet.Range("$W$" + (groupingStartLeftRowPosition + offset).ToString()).HorizontalAlignment = Constants.xlRight;
                    outputWorkSheet.Range("$W$" + (groupingStartLeftRowPosition + offset).ToString()).NumberFormatLocal = "#,###";

                    // 重量(値)
                    weight = group.ShipmentWeight;
                    countWeight = countWeight + weight;
                    outputWorkSheet.Range("$X$" + (groupingStartLeftRowPosition + offset).ToString()).Value2 = weight;
                    outputWorkSheet.Range("$X$" + (groupingStartLeftRowPosition + offset).ToString()).HorizontalAlignment = Constants.xlRight;
                    outputWorkSheet.Range("$X$" + (groupingStartLeftRowPosition + offset).ToString()).NumberFormatLocal = "#,###";
                    offset++;
                }

                // 総合計
                outputWorkSheet.CellMerge("$T$" + (groupingStartLeftRowPosition + offset + 1).ToString(), "$V$" + (groupingStartLeftRowPosition + offset + 1).ToString());
                outputWorkSheet.Range("$T$" + (groupingStartLeftRowPosition + offset + 1).ToString()).Value2 = "出荷合計";
                outputWorkSheet.Range("$T$" + (groupingStartLeftRowPosition + offset + 1).ToString()).HorizontalAlignment = Constants.xlLeft;

                // 総合計(本数)
                outputWorkSheet.Range("$W$" + (groupingStartLeftRowPosition + offset + 1).ToString()).Value2 = countSum;
                outputWorkSheet.Range("$W$" + (groupingStartLeftRowPosition + offset + 1).ToString()).HorizontalAlignment = Constants.xlRight;
                outputWorkSheet.Range("$W$" + (groupingStartLeftRowPosition + offset + 1).ToString()).NumberFormatLocal = "#,###";

                // 総合計(重量)
                outputWorkSheet.Range("$X$" + (groupingStartLeftRowPosition + offset + 1).ToString()).Value2 = countWeight;
                outputWorkSheet.Range("$X$" + (groupingStartLeftRowPosition + offset + 1).ToString()).HorizontalAlignment = Constants.xlRight;
                outputWorkSheet.Range("$X$" + (groupingStartLeftRowPosition + offset + 1).ToString()).NumberFormatLocal = "#,###";

                rangeAllTotal = outputWorkSheet.Range("$T$" + (groupingStartLeftRowPosition + offset + 1).ToString(),
                                                      "$X$" + (groupingStartLeftRowPosition + offset + 1).ToString());
                rangeAllTotal.Borders[XlBordersIndex.xlInsideHorizontal].LineStyle = XlLineStyle.xlContinuous;
                rangeAllTotal.Borders[XlBordersIndex.xlInsideHorizontal].ColorIndex = 1;
                rangeAllTotal.Borders[XlBordersIndex.xlInsideVertical].LineStyle = XlLineStyle.xlContinuous;
                rangeAllTotal.Borders[XlBordersIndex.xlInsideVertical].ColorIndex = 1;
                rangeAllTotal.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
                rangeAllTotal.Borders[XlBordersIndex.xlEdgeTop].ColorIndex = 1;
                rangeAllTotal.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
                rangeAllTotal.Borders[XlBordersIndex.xlEdgeBottom].ColorIndex = 1;
                rangeAllTotal.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
                rangeAllTotal.Borders[XlBordersIndex.xlEdgeLeft].ColorIndex = 1;
                rangeAllTotal.Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;
                rangeAllTotal.Borders[XlBordersIndex.xlEdgeRight].ColorIndex = 1;
            }

            catch (Exception)
            {
                throw;
            }

            finally
            {
                if (rangeDetail != null)
                {
                    Marshal.ReleaseComObject(rangeDetail);
                    rangeDetail = null;
                }

                if (rangeGrouping != null)
                {
                    Marshal.ReleaseComObject(rangeGrouping);
                    rangeGrouping = null;
                }

                if (rangeAllTotal != null)
                {
                    Marshal.ReleaseComObject(rangeAllTotal);
                    rangeAllTotal = null;
                }
            }
        }

        /// <summary>
        /// 年月日の書込み
        /// </summary>
        /// <param name="range">書込み範囲</param>
        /// <param name="datetime">現在時刻</param>
        protected override void WriteYearMonth(Range range, DateTime datetime)
        {
            range.Value2 = "  " + datetime.ToString("G");
        }

        #endregion

    }

    /// <summary>
    /// 出荷のDTO
    /// </summary>
    internal class ShipmentDTO : DTOBase
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        internal ShipmentDTO()
        {
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 受注№
        /// </summary>
        internal string OrderNo { get; set; }

        /// <summary>
        /// 出荷本数
        /// </summary>
        internal int ShipmentHonsuu { get; set; }

        /// <summary>
        /// 出荷重量
        /// </summary>
        internal int ShipmentWeight { get; set; }

        #endregion
    }

    /// <summary>
    /// 外販受注残リスト(PQGA420F)のExcelライター
    /// </summary>
    internal class PQGA420FExcelWriter : AbstractExcelWriter
    {
        #region フィールド

        /// <summary>
        /// ExcelApplication
        /// </summary>
        private Application application = null;

        /// <summary>
        /// テンプレート用のワークブック
        /// </summary>
        private Workbook templateWorkBook = null;

        /// <summary>
        /// テンプレート用のワークシート
        /// </summary>
        private Worksheet templateWorkSheet = null;

        /// <summary>
        /// 出力用のワークブック
        /// </summary>
        private Workbook outputWorkBook = null;

        /// <summary>
        /// 出力用のワークシート
        /// </summary>
        private Worksheet outputWorkSheet = null;

        /// <summary>
        /// ドキュメントに対する操作完了時のコンテナ
        /// </summary>
        private IDocumentOperationContainer container;

        #endregion

        #region プロパティ

        /// <summary>
        /// 年
        /// </summary>
        internal string Year { get; set; }

        /// <summary>
        /// 月
        /// </summary>
        internal string Month { get; set; }

        /// <summary>
        /// テンプレートワークブックのパス
        /// </summary>
        internal string TemplateWorkBookPath { get; set; }

        /// <summary>
        /// 出力ワークブックのパス
        /// </summary>
        internal string OutputWorkBookPath { get; set; }

        /// <summary>
        /// 出力ワークブックのフォルダ
        /// </summary>
        internal string OutputWorkBookFolder { get; set; }

        /// <summary>
        /// 出力ワークブック名
        /// </summary>
        internal string OutputWorkBookName { get; set; }

        /// <summary>
        /// プレビューを表示するか
        /// </summary>
        internal bool IsShowPreview { get; set; }

        /// <summary>
        /// Excelシート名のマップ
        /// </summary>
        private static readonly IDictionary<PQGA420BExcelSheetType, string> excelSheetNameMap = null;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// 静的コンストラクタ
        /// </summary>
        static PQGA420FExcelWriter()
        {
            excelSheetNameMap
                = new Dictionary<PQGA420BExcelSheetType, string>();
            excelSheetNameMap.Add(PQGA420BExcelSheetType.Detail, PQGA420BExcelSheetType.Detail.GetValue());
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        internal PQGA420FExcelWriter()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="container">ドキュメントに対する操作完了時のコンテナ</param>
        internal PQGA420FExcelWriter(IDocumentOperationContainer container)
        {
            this.container = container;
        }

        #endregion

        #region メソッド

        /// <summary>
        /// 初期化処理
        /// </summary>
        public override void Initialize()
        {
            try
            {
                // ExcelApplicationの取得
                application = ExcelUtils.GetExcelApplication();
                application.DisplayAlerts = false;

                // Excelテンプレート用のExcelワークブックの取得
                templateWorkBook = ExcelUtils.GetExcelTemplateWorkBook(application, TemplateWorkBookPath);

                // 出力用のExcelワークブックのフォルダ
                OutputWorkBookFolder = OutputWorkBookFolder ??
                    Path.GetDirectoryName(OutputWorkBookPath) + "\\";
                // 現在時刻の取得
                DateTimeNow = DateTime.Now;
                // 出力用のExcelワークブック名
                OutputWorkBookName = OutputWorkBookName ??
                    (Path.GetFileName(OutputWorkBookPath)).ConvertExcelWorkBookName(DateTimeNow);
                // 出力用のExcelワークブックの取得
                outputWorkBook = ExcelUtils.GetExcelOutputWorkBook(application,
                                                                   OutputWorkBookFolder,
                                                                   OutputWorkBookName,
                                                                   TemplateWorkBookPath, true, true);
            }

            catch (Exception)
            {
                // COMオブジェクトの解放
                ReleaseComObject();
                throw;
            }
        }

        /// <summary>
        /// 書込み前処理
        /// </summary>
        public override void PreWrite()
        {
        }

        /// <summary>
        /// ヘッダーの書込み
        /// </summary>
        /// <typeparam name="T">ヘッダーを継承した任意のオブジェクトの型</typeparam>
        /// <param name="header">ファイルのヘッダー情報</param>
        public override void WriteHeader<T>(T header)
        {
        }

        /// <summary>
        /// 詳細の書込み
        /// </summary>
        /// <typeparam name="T">詳細を継承した任意のオブジェクトの型</typeparam>
        /// <param name="detail">ファイルの詳細情報</param>
        public override void WriteDetail<T>(IList<T> detail)
        {
            try
            {
                // 詳細
                var detailData = new List<PQGA420BDTO>();
                var targets = detail[0] as ExternalSalesBacklogDetail;
                var items = targets.PQGA420BDTOList;
                items.ForEach(item =>
                {
                    detailData.Add(item);
                });
                
                    var target = targets.PQGA420BDTOList;                             
                    var targetHonsuuSum = target.Sum(honsuuSum => honsuuSum.Honsuu);
                    var targetWeightSum = target.Sum(weightSum => weightSum.Weight);

                // ワークシートの切替え(詳細)
                SwitchOutputWorkBookSheet(PQGA420BExcelSheetType.Detail);
                // 年月日の書込み(タイトルの右端)
                WriteYearMonth(outputWorkSheet.Range("$Q$1"), DateTimeNow);
                // 詳細データの書込み
                WriteDetailData(detailData, targetHonsuuSum, targetWeightSum);
                                   
            }
            catch (Exception)
            {
                // COMオブジェクトの解放
                ReleaseComObject();
                throw;
            }
        }

        /// <summary>
        /// フッターの書込み
        /// </summary>
        /// <typeparam name="T">フッターを継承した任意のオブジェクトの型</typeparam>
        /// <param name="footer">ファイルのフッター情報</param>
        public override void WriteFooter<T>(T footer)
        {
            // 処理不要
        }

        /// <summary>
        /// 書込み後処理
        /// </summary>
        public override void PostWrite()
        {
            // 処理不要
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public override void Terminate()
        {
            try
            {
                // ワークブックの保存
                outputWorkBook.Save(OutputWorkBookFolder, OutputWorkBookName);
                if (container != null)
                {
                    container.NotifyOutputExcel(OutputWorkBookFolder + OutputWorkBookName);
                }
            }

            catch (Exception)
            {
                throw;
            }

            finally
            {
                // COMオブジェクトの解放
                ReleaseComObject();
            }
        }

        /// <summary>
        /// COMオブジェクトの解放
        /// </summary>
        protected override void ReleaseComObject()
        {
            if (outputWorkBook != null)
            {
                outputWorkBook.Close(false, false, false);
            }

            if (templateWorkBook != null)
            {
                templateWorkBook.Close(false, false, false);
            }

            if (application != null)
            {
                application.Visible = false;
                application.DisplayAlerts = true;
                application.Quit();
                Marshal.ReleaseComObject(application);
                application = null;
            }

            if (outputWorkBook != null)
            {
                Marshal.ReleaseComObject(outputWorkBook);
                outputWorkBook = null;
            }

            if (templateWorkBook != null)
            {
                Marshal.ReleaseComObject(templateWorkBook);
                templateWorkBook = null;
            }

            if (templateWorkSheet != null)
            {
                Marshal.ReleaseComObject(templateWorkSheet);
                templateWorkSheet = null;
            }

            if (outputWorkSheet != null)
            {
                Marshal.ReleaseComObject(outputWorkSheet);
                outputWorkSheet = null;
            }
        }

        /// <summary>
        /// 出力用のExcelワークブックのシートの切替え
        /// </summary>
        /// <param name="excelSheetType">Excelシートの種類</param>
        private void SwitchOutputWorkBookSheet(PQGA420BExcelSheetType excelSheetType)
        {
            try
            {
                // 対象のワークシートの検索
                outputWorkSheet = ExcelUtils.SearchSheet(outputWorkBook, excelSheetNameMap[excelSheetType]);
                outputWorkSheet.Select(true);
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 詳細データの書込み
        /// </summary>
        /// <param name="list">詳細データ</param>
        private void WriteDetailData(IList<PQGA420BDTO> list, int HonsuuSum, int WeightSum)
        {
            Range rangeDetail = null;
            Range rangeSum = null;
            int listCount = list.Count;

            // 罫線
            int startLeftRowPosition = 5;               // 左上の行番号
            int endRightRowPosition = listCount + 4;    // 右下の行番号

            try
            {
                // 印刷範囲の設定
                outputWorkSheet.PageSetup.PrintArea = @"$A$1:$R$" + (endRightRowPosition + 4).ToString();
                // 罫線および値を出力するレンジの設定
                rangeDetail = outputWorkSheet.Range("$A$" + (startLeftRowPosition).ToString(),
                                                    "$R$" + (endRightRowPosition).ToString());

                rangeDetail.Borders[XlBordersIndex.xlInsideHorizontal].LineStyle = XlLineStyle.xlContinuous;
                rangeDetail.Borders[XlBordersIndex.xlInsideHorizontal].ColorIndex = 1;
                rangeDetail.Borders[XlBordersIndex.xlInsideVertical].LineStyle = XlLineStyle.xlContinuous;
                rangeDetail.Borders[XlBordersIndex.xlInsideVertical].ColorIndex = 1;
                rangeDetail.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
                rangeDetail.Borders[XlBordersIndex.xlEdgeTop].ColorIndex = 1;
                rangeDetail.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
                rangeDetail.Borders[XlBordersIndex.xlEdgeBottom].ColorIndex = 1;
                rangeDetail.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
                rangeDetail.Borders[XlBordersIndex.xlEdgeLeft].ColorIndex = 1;
                rangeDetail.Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;
                rangeDetail.Borders[XlBordersIndex.xlEdgeRight].ColorIndex = 1;

                var outputData = new object[listCount, 18];
                for (int i = 0; i < listCount; ++i)
                {
                    // 受注№
                    outputData[i, 0] = list[i].OrderNo;
                    // 需要家コード
                    outputData[i, 1] = list[i].CustomerCode;
                    // 需要家名
                    outputData[i, 2] = list[i].CustomerName;
                    // 商社
                    outputData[i, 3] = list[i].DisutributorCode;
                    // 都市
                    outputData[i, 4] = list[i].City;
                    // 回収
                    outputData[i, 5] = list[i].Collection;
                    // 品種
                    outputData[i, 6] = list[i].Kind;
                    // 規格
                    outputData[i, 7] = list[i].StandardCode;
                    // 決済
                    outputData[i, 8] = list[i].SettlementCondition;
                    // 商社名
                    outputData[i, 9] = list[i].DisutributorName;
                    // 規格名
                    outputData[i, 10] = list[i].StandardName;
                    // 製品サイズ(厚)
                    outputData[i, 11] = list[i].SlabAtu;
                    // 製品サイズ(巾)
                    outputData[i, 12] = list[i].SlabHaba;
                    // 製品サイズ(長)
                    outputData[i, 13] = list[i].SlabNaga;
                    // 出荷枚数
                    outputData[i, 14] = list[i].Honsuu;
                    // 出荷重量
                    outputData[i, 15] = list[i].Weight;
                    // 納期
                    outputData[i, 16] = list[i].DeliveryDate;
                    // 受渡条件
                    outputData[i, 17] = list[i].TransferCondition;
                }

                rangeDetail.Value2 = outputData;

                // 罫線(合計)
                int sumStartLeftRowPosition = endRightRowPosition + 2;    // 左上の行番号
                int sumEndRightRowPosition = sumStartLeftRowPosition + 2; // 右下の行番号

                // 罫線および値を出力するレンジの設定(合計)
                rangeSum = outputWorkSheet.Range("$O$" + (sumStartLeftRowPosition).ToString(),
                                                 "$R$" + (sumEndRightRowPosition).ToString());
                rangeSum.Borders[XlBordersIndex.xlInsideHorizontal].LineStyle = XlLineStyle.xlContinuous;
                rangeSum.Borders[XlBordersIndex.xlInsideHorizontal].ColorIndex = 1;
                rangeSum.Borders[XlBordersIndex.xlInsideVertical].LineStyle = XlLineStyle.xlContinuous;
                rangeSum.Borders[XlBordersIndex.xlInsideVertical].ColorIndex = 1;
                rangeSum.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
                rangeSum.Borders[XlBordersIndex.xlEdgeTop].ColorIndex = 1;
                rangeSum.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
                rangeSum.Borders[XlBordersIndex.xlEdgeBottom].ColorIndex = 1;
                rangeSum.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
                rangeSum.Borders[XlBordersIndex.xlEdgeLeft].ColorIndex = 1;
                rangeSum.Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;
                rangeSum.Borders[XlBordersIndex.xlEdgeRight].ColorIndex = 1;

                // 勤務計
                outputWorkSheet.CellMerge("$O$" + sumStartLeftRowPosition, "$R$" + sumStartLeftRowPosition);
                outputWorkSheet.Range("$O$" + sumStartLeftRowPosition.ToString()).Value2 = "(合計)";
                outputWorkSheet.Range("$O$" + sumStartLeftRowPosition.ToString()).HorizontalAlignment = Constants.xlLeft;

                // 本数
                outputWorkSheet.CellMerge("$O$" + (sumStartLeftRowPosition + 1).ToString(), "$P$" + (sumStartLeftRowPosition + 1).ToString());
                outputWorkSheet.Range("$O$" + (sumStartLeftRowPosition + 1).ToString()).Value2 = "本数";
                outputWorkSheet.Range("$O$" + (sumStartLeftRowPosition + 1).ToString()).HorizontalAlignment = Constants.xlCenter;

                // 重量
                outputWorkSheet.CellMerge("$Q$" + (sumStartLeftRowPosition + 1).ToString(), "$R$" + (sumStartLeftRowPosition + 1).ToString());
                outputWorkSheet.Range("$Q$" + (sumStartLeftRowPosition + 1).ToString()).Value2 = "重量(kg)";
                outputWorkSheet.Range("$Q$" + (sumStartLeftRowPosition + 1).ToString()).HorizontalAlignment = Constants.xlCenter;

                // 本数(値)
                outputWorkSheet.CellMerge("$O$" + (sumStartLeftRowPosition + 2).ToString(), "$P$" + (sumStartLeftRowPosition + 2).ToString());
                outputWorkSheet.Range("$O$" + (sumStartLeftRowPosition + 2).ToString()).Value2 = HonsuuSum;
                outputWorkSheet.Range("$O$" + (sumStartLeftRowPosition + 2).ToString()).HorizontalAlignment = Constants.xlRight;
                outputWorkSheet.Range("$O$" + (sumStartLeftRowPosition + 2).ToString()).NumberFormatLocal = "#,###";

                // 重量(値)
                outputWorkSheet.CellMerge("$Q$" + (sumStartLeftRowPosition + 2).ToString(), "$R$" + (sumStartLeftRowPosition + 2).ToString());
                outputWorkSheet.Range("$Q$" + (sumStartLeftRowPosition + 2).ToString()).Value2 = WeightSum;
                outputWorkSheet.Range("$Q$" + (sumStartLeftRowPosition + 2).ToString()).HorizontalAlignment = Constants.xlRight;
                outputWorkSheet.Range("$Q$" + (sumStartLeftRowPosition + 2).ToString()).NumberFormatLocal = "#,###";

            
            }

            catch (Exception)
            {
                throw;
            }

            finally
            {
                if (rangeDetail != null)
                {
                    Marshal.ReleaseComObject(rangeDetail);
                    rangeDetail = null;
                }

                if (rangeSum != null)
                {
                    Marshal.ReleaseComObject(rangeSum);
                    rangeSum = null;
                }
            }
        }

        /// <summary>
        /// 年月日の書込み
        /// </summary>
        /// <param name="range">書込み範囲</param>
        /// <param name="datetime">現在時刻</param>
        protected override void WriteYearMonth(Range range, DateTime datetime)
        {
            range.Value2 = "  " + datetime.ToString("G");
        }

        #endregion

    }

    /// <summary>
    /// 外販出荷計画リスト(PQGA820F)のExcelライター
    /// </summary>
    internal class PQGA820FExcelWriter : AbstractExcelWriter
    {
        #region フィールド

        /// <summary>
        /// 合計を意味する値
        /// </summary>
        private static readonly string SummaryMeanValue = "GOKEI";

        /// <summary>
        /// 合計のキーワード
        /// </summary>
        private static readonly string SummaryKeyWord = "規格別合計";

        /// <summary>
        /// ExcelApplication
        /// </summary>
        private Application application = null;

        /// <summary>
        /// テンプレート用のワークブック
        /// </summary>
        private Workbook templateWorkBook = null;

        /// <summary>
        /// テンプレート用のワークシート
        /// </summary>
        private Worksheet templateWorkSheet = null;

        /// <summary>
        /// 出力用のワークブック
        /// </summary>
        private Workbook outputWorkBook = null;

        /// <summary>
        /// 出力用のワークシート
        /// </summary>
        private Worksheet outputWorkSheet = null;

        /// <summary>
        /// ドキュメントに対する操作完了時のコンテナ
        /// </summary>
        private IDocumentOperationContainer container;

        #endregion

        #region プロパティ

        /// <summary>
        /// 年
        /// </summary>
        internal string Year { get; set; }

        /// <summary>
        /// 月
        /// </summary>
        internal string Month { get; set; }

        /// <summary>
        /// テンプレートワークブックのパス
        /// </summary>
        internal string TemplateWorkBookPath { get; set; }

        /// <summary>
        /// 出力ワークブックのパス
        /// </summary>
        internal string OutputWorkBookPath { get; set; }

        /// <summary>
        /// 出力ワークブックのフォルダ
        /// </summary>
        internal string OutputWorkBookFolder { get; set; }

        /// <summary>
        /// 出力ワークブック名
        /// </summary>
        internal string OutputWorkBookName { get; set; }

        /// <summary>
        /// プレビューを表示するか
        /// </summary>
        internal bool IsShowPreview { get; set; }

        /// <summary>
        /// Excelシート名のマップ
        /// </summary>
        private static readonly IDictionary<PQGA820BExcelSheetType, string> excelSheetNameMap = null;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// 静的コンストラクタ
        /// </summary>
        static PQGA820FExcelWriter()
        {
            excelSheetNameMap
                = new Dictionary<PQGA820BExcelSheetType, string>();
            excelSheetNameMap.Add(PQGA820BExcelSheetType.Detail, PQGA820BExcelSheetType.Detail.GetValue());
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        internal PQGA820FExcelWriter()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="container">ドキュメントに対する操作完了時のコンテナ</param>
        internal PQGA820FExcelWriter(IDocumentOperationContainer container)
        {
            this.container = container;
        }

        #endregion

        #region メソッド

        /// <summary>
        /// 初期化処理
        /// </summary>
        public override void Initialize()
        {
            try
            {
                // ExcelApplicationの取得
                application = ExcelUtils.GetExcelApplication();
                application.DisplayAlerts = false;

                // Excelテンプレート用のExcelワークブックの取得
                templateWorkBook = ExcelUtils.GetExcelTemplateWorkBook(application, TemplateWorkBookPath);

                // 出力用のExcelワークブックのフォルダ
                OutputWorkBookFolder = OutputWorkBookFolder ??
                    Path.GetDirectoryName(OutputWorkBookPath) + "\\";
                // 現在時刻の取得
                DateTimeNow = DateTime.Now;
                // 出力用のExcelワークブック名
                OutputWorkBookName = OutputWorkBookName ??
                    (Path.GetFileName(OutputWorkBookPath)).ConvertExcelWorkBookName(DateTimeNow);
                // 出力用のExcelワークブックの取得
                outputWorkBook = ExcelUtils.GetExcelOutputWorkBook(application,
                                                                   OutputWorkBookFolder,
                                                                   OutputWorkBookName,
                                                                   TemplateWorkBookPath, true, true);
            }

            catch (Exception)
            {
                // COMオブジェクトの解放
                ReleaseComObject();
                throw;
            }
        }

        /// <summary>
        /// 書込み前処理
        /// </summary>
        public override void PreWrite()
        {
        }

        /// <summary>
        /// ヘッダーの書込み
        /// </summary>
        /// <typeparam name="T">ヘッダーを継承した任意のオブジェクトの型</typeparam>
        /// <param name="header">ファイルのヘッダー情報</param>
        public override void WriteHeader<T>(T header)
        {
        }

        /// <summary>
        /// 詳細の書込み
        /// </summary>
        /// <typeparam name="T">詳細を継承した任意のオブジェクトの型</typeparam>
        /// <param name="detail">ファイルの詳細情報</param>
        public override void WriteDetail<T>(IList<T> detail)
        {
            try
            {
                // 詳細
                var detailData = new List<PQGA820BDTO>();
                var targets = detail[0] as ExternalStandardByAggregateDetail;
                var items = targets.PQGA820BDTOList;
                items.ForEach(item =>
                {
                    detailData.Add(item);
                });
                // ワークシートの切替え(詳細)
                SwitchOutputWorkBookSheet(PQGA820BExcelSheetType.Detail);
                // 年月日の書込み(タイトルの右端)
                WriteYearMonth(outputWorkSheet.Range("$O$1"), DateTimeNow);
                // 詳細データの書込み
                WriteDetailData(detailData);
            }

            catch (Exception)
            {
                // COMオブジェクトの解放
                ReleaseComObject();
                throw;
            }
        }

        /// <summary>
        /// フッターの書込み
        /// </summary>
        /// <typeparam name="T">フッターを継承した任意のオブジェクトの型</typeparam>
        /// <param name="footer">ファイルのフッター情報</param>
        public override void WriteFooter<T>(T footer)
        {
            // 処理不要
        }

        /// <summary>
        /// 書込み後処理
        /// </summary>
        public override void PostWrite()
        {
            // 処理不要
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public override void Terminate()
        {
            try
            {
                // ワークブックの保存
                outputWorkBook.Save(OutputWorkBookFolder, OutputWorkBookName);
                if (container != null)
                {
                    container.NotifyOutputExcel(OutputWorkBookFolder + OutputWorkBookName);
                }
            }

            catch (Exception)
            {
                throw;
            }

            finally
            {
                // COMオブジェクトの解放
                ReleaseComObject();
            }
        }

        /// <summary>
        /// COMオブジェクトの解放
        /// </summary>
        protected override void ReleaseComObject()
        {
            if (outputWorkBook != null)
            {
                outputWorkBook.Close(false, false, false);
            }

            if (templateWorkBook != null)
            {
                templateWorkBook.Close(false, false, false);
            }

            if (application != null)
            {
                application.Visible = false;
                application.DisplayAlerts = true;
                application.Quit();
                Marshal.ReleaseComObject(application);
                application = null;
            }

            if (outputWorkBook != null)
            {
                Marshal.ReleaseComObject(outputWorkBook);
                outputWorkBook = null;
            }

            if (templateWorkBook != null)
            {
                Marshal.ReleaseComObject(templateWorkBook);
                templateWorkBook = null;
            }

            if (templateWorkSheet != null)
            {
                Marshal.ReleaseComObject(templateWorkSheet);
                templateWorkSheet = null;
            }

            if (outputWorkSheet != null)
            {
                Marshal.ReleaseComObject(outputWorkSheet);
                outputWorkSheet = null;
            }
        }

        /// <summary>
        /// 出力用のExcelワークブックのシートの切替え
        /// </summary>
        /// <param name="excelSheetType">Excelシートの種類</param>
        private void SwitchOutputWorkBookSheet(PQGA820BExcelSheetType excelSheetType)
        {
            try
            {
                // 対象のワークシートの検索
                outputWorkSheet = ExcelUtils.SearchSheet(outputWorkBook, excelSheetNameMap[excelSheetType]);
                outputWorkSheet.Select(true);
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 詳細データの書込み
        /// </summary>
        /// <param name="list">詳細データ</param>
        private void WriteDetailData(IList<PQGA820BDTO> list)
        {
            Range range = null;
            int listCount = list.Count;

            // 罫線
            int startLeftRowPosition = 5;               // 左上の行番号
            int endRightRowPosition = listCount + 4;    // 右下の行番号

            try
            {
                // 印刷範囲の設定
                outputWorkSheet.PageSetup.PrintArea = @"$A$1:$Q$" + (endRightRowPosition).ToString();
                // 罫線および値を出力するレンジの設定
                range = outputWorkSheet.Range("$A$" + (startLeftRowPosition).ToString(),
                                              "$Q$" + (endRightRowPosition).ToString());

                range.Borders[XlBordersIndex.xlInsideHorizontal].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlInsideHorizontal].ColorIndex = 1;
                range.Borders[XlBordersIndex.xlInsideVertical].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlInsideVertical].ColorIndex = 1;
                range.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlEdgeTop].ColorIndex = 1;
                range.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlEdgeBottom].ColorIndex = 1;
                range.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlEdgeLeft].ColorIndex = 1;
                range.Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlEdgeRight].ColorIndex = 1;

                var outputData = new object[listCount, 17];
                for (int i = 0; i < listCount; ++i)
                {
                    // 出荷日
                    outputData[i, 0] = list[i].ShipmentYYMMDD.Trim() != SummaryMeanValue ?
                                       list[i].ShipmentYYMMDD : SummaryKeyWord;
                    // ＺＫ０８１(本数)
                    outputData[i, 1] = list[i].ZK081Honsuu;
                    // ＺＫ０８１(重量)
                    outputData[i, 2] = list[i].ZK081Weight;
                    // ＺＫ０８５(本数)
                    outputData[i, 3] = list[i].ZK085Honsuu;
                    // ＺＫ０８５(重量)
                    outputData[i, 4] = list[i].ZK085Weight;
                    // ＺＫ１２１(本数)
                    outputData[i, 5] = list[i].ZK121Honsuu;
                    // ＺＫ１２１(重量)
                    outputData[i, 6] = list[i].ZK121Weight;
                    // ＺＫ１７２(本数)
                    outputData[i, 7] = list[i].ZK172Honsuu;
                    // ＺＫ１７２(重量)
                    outputData[i, 8] = list[i].ZK172Weight;
                    // ＳＰＨＣ(本数)
                    outputData[i, 9] = list[i].SPHCHonsuu;
                    // ＳＰＨＣ(重量)
                    outputData[i, 10] = list[i].SPHCWeight;
                    // ＳＳ４００(本数)
                    outputData[i, 11] = list[i].SS400Honsuu;
                    // ＳＳ４００(重量)
                    outputData[i, 12] = list[i].SS400Weight;
                    // その他(本数)
                    outputData[i, 13] = list[i].OtherHonsuu;
                    // その他(重量)
                    outputData[i, 14] = list[i].OtherWeight;
                    // 出荷日合計(本数)
                    outputData[i, 15] = list[i].TotalHonsuu;
                    // 出荷日合計(重量)
                    outputData[i, 16] = list[i].TotalWeight;
                }

                range.Value2 = outputData;
            }

            catch (Exception)
            {
                throw;
            }

            finally
            {
                if (range != null)
                {
                    Marshal.ReleaseComObject(range);
                    range = null;
                }
            }
        }

        /// <summary>
        /// 年月日の書込み
        /// </summary>
        /// <param name="range">書込み範囲</param>
        /// <param name="datetime">現在時刻</param>
        protected override void WriteYearMonth(Range range, DateTime datetime)
        {
            range.Value2 = "               " + datetime.ToString("G");
        }

        #endregion

    }

    /// <summary>
    /// 発生屑・端板・伸鉄払出明細(SSYM040F)のExcelライター
    /// </summary>
    internal class SSYM040FExcelWriter : AbstractExcelWriter
    {
        #region フィールド

        /// <summary>
        /// 合計を意味する値
        /// </summary>
        private static readonly string SummaryMeanValue = "GOKEI";

        /// <summary>
        /// 圧延を意味する値
        /// </summary>
        private static readonly string AtuenMeanValue = "ATUEN";

        /// <summary>
        /// 合計のキーワード
        /// </summary>
        private static readonly string SummaryKeyWord = "計";

        /// <summary>
        /// ExcelApplication
        /// </summary>
        private Application application = null;

        /// <summary>
        /// テンプレート用のワークブック
        /// </summary>
        private Workbook templateWorkBook = null;

        /// <summary>
        /// テンプレート用のワークシート
        /// </summary>
        private Worksheet templateWorkSheet = null;

        /// <summary>
        /// 出力用のワークブック
        /// </summary>
        private Workbook outputWorkBook = null;

        /// <summary>
        /// 出力用のワークシート
        /// </summary>
        private Worksheet outputWorkSheet = null;

        /// <summary>
        /// ドキュメントに対する操作完了時のコンテナ
        /// </summary>
        private IDocumentOperationContainer container;

        #endregion

        #region プロパティ

        /// <summary>
        /// 年
        /// </summary>
        internal string Year { get; set; }

        /// <summary>
        /// 月
        /// </summary>
        internal string Month { get; set; }

        /// <summary>
        /// テンプレートワークブックのパス
        /// </summary>
        internal string TemplateWorkBookPath { get; set; }

        /// <summary>
        /// 出力ワークブックのパス
        /// </summary>
        internal string OutputWorkBookPath { get; set; }

        /// <summary>
        /// 出力ワークブックのフォルダ
        /// </summary>
        internal string OutputWorkBookFolder { get; set; }

        /// <summary>
        /// 出力ワークブック名
        /// </summary>
        internal string OutputWorkBookName { get; set; }

        /// <summary>
        /// プレビューを表示するか
        /// </summary>
        internal bool IsShowPreview { get; set; }

        /// <summary>
        /// Excelシート名のマップ
        /// </summary>
        private static readonly IDictionary<SSYM040BExcelSheetType, string> excelSheetNameMap = null;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// 静的コンストラクタ
        /// </summary>
        static SSYM040FExcelWriter()
        {
            excelSheetNameMap
                = new Dictionary<SSYM040BExcelSheetType, string>();
            excelSheetNameMap.Add(SSYM040BExcelSheetType.Detail, SSYM040BExcelSheetType.Detail.GetValue());
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        internal SSYM040FExcelWriter()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="container">ドキュメントに対する操作完了時のコンテナ</param>
        internal SSYM040FExcelWriter(IDocumentOperationContainer container)
        {
            this.container = container;
        }

        #endregion

        #region メソッド

        /// <summary>
        /// 初期化処理
        /// </summary>
        public override void Initialize()
        {
            try
            {
                // ExcelApplicationの取得
                application = ExcelUtils.GetExcelApplication();
                application.DisplayAlerts = false;

                // Excelテンプレート用のExcelワークブックの取得
                templateWorkBook = ExcelUtils.GetExcelTemplateWorkBook(application, TemplateWorkBookPath);

                // 出力用のExcelワークブックのフォルダ
                OutputWorkBookFolder = OutputWorkBookFolder ??
                    Path.GetDirectoryName(OutputWorkBookPath) + "\\";
                // 現在時刻の取得
                DateTimeNow = DateTime.Now;
                // 出力用のExcelワークブック名
                OutputWorkBookName = OutputWorkBookName ??
                    (Path.GetFileName(OutputWorkBookPath)).ConvertExcelWorkBookName(DateTimeNow);
                // 出力用のExcelワークブックの取得
                outputWorkBook = ExcelUtils.GetExcelOutputWorkBook(application,
                                                                   OutputWorkBookFolder,
                                                                   OutputWorkBookName,
                                                                   TemplateWorkBookPath, true, true);
            }

            catch (Exception)
            {
                // COMオブジェクトの解放
                ReleaseComObject();
                throw;
            }
        }

        /// <summary>
        /// 書込み前処理
        /// </summary>
        public override void PreWrite()
        {
        }

        /// <summary>
        /// ヘッダーの書込み
        /// </summary>
        /// <typeparam name="T">ヘッダーを継承した任意のオブジェクトの型</typeparam>
        /// <param name="header">ファイルのヘッダー情報</param>
        public override void WriteHeader<T>(T header)
        {
        }

        /// <summary>
        /// 詳細の書込み
        /// </summary>
        /// <typeparam name="T">詳細を継承した任意のオブジェクトの型</typeparam>
        /// <param name="detail">ファイルの詳細情報</param>
        public override void WriteDetail<T>(IList<T> detail)
        {
            try
            {
                // 詳細
                var detailData = new List<SSYM040BDTO>();
                var atuenData = new List<AtuenDTO>();
                var targets = detail[0] as GeneratedWaste_SinglePlate_SinTetsuPayoutDetail;
                var items = targets.SSYM040BDTOList;
                items.ForEach(item =>
                {
                    if(item.YYYYMMDD.Trim() != AtuenMeanValue)
                    {
                        detailData.Add(item);
                    }
                    else
                    {
                        var atuenDTO = new AtuenDTO()
                        {
                            EndPlate = item.MiddlePlateWaste,
                            BusheledIronMaterial = item.NissinReturnWaste,
                            Total = item.Total,
                        };
                        atuenData.Add(atuenDTO);
                    }

                });
                // ワークシートの切替え(詳細)
                SwitchOutputWorkBookSheet(SSYM040BExcelSheetType.Detail);
                // 年月日の書込み(タイトルの右端)
                WriteYearMonth(outputWorkSheet.Range("$J$1"), DateTimeNow);
                // 詳細データの書込み
                WriteDetailData(detailData, atuenData);
            }

            catch (Exception)
            {
                // COMオブジェクトの解放
                ReleaseComObject();
                throw;
            }
        }

        /// <summary>
        /// フッターの書込み
        /// </summary>
        /// <typeparam name="T">フッターを継承した任意のオブジェクトの型</typeparam>
        /// <param name="footer">ファイルのフッター情報</param>
        public override void WriteFooter<T>(T footer)
        {
            // 処理不要
        }

        /// <summary>
        /// 書込み後処理
        /// </summary>
        public override void PostWrite()
        {
            // 処理不要
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public override void Terminate()
        {
            try
            {
                // ワークブックの保存
                outputWorkBook.Save(OutputWorkBookFolder, OutputWorkBookName);
                if (container != null)
                {
                    container.NotifyOutputExcel(OutputWorkBookFolder + OutputWorkBookName);
                }
            }

            catch (Exception)
            {
                throw;
            }

            finally
            {
                // COMオブジェクトの解放
                ReleaseComObject();
            }
        }

        /// <summary>
        /// COMオブジェクトの解放
        /// </summary>
        protected override void ReleaseComObject()
        {
            if (outputWorkBook != null)
            {
                outputWorkBook.Close(false, false, false);
            }

            if (templateWorkBook != null)
            {
                templateWorkBook.Close(false, false, false);
            }

            if (application != null)
            {
                application.Visible = false;
                application.DisplayAlerts = true;
                application.Quit();
                Marshal.ReleaseComObject(application);
                application = null;
            }

            if (outputWorkBook != null)
            {
                Marshal.ReleaseComObject(outputWorkBook);
                outputWorkBook = null;
            }

            if (templateWorkBook != null)
            {
                Marshal.ReleaseComObject(templateWorkBook);
                templateWorkBook = null;
            }

            if (templateWorkSheet != null)
            {
                Marshal.ReleaseComObject(templateWorkSheet);
                templateWorkSheet = null;
            }

            if (outputWorkSheet != null)
            {
                Marshal.ReleaseComObject(outputWorkSheet);
                outputWorkSheet = null;
            }
        }

        /// <summary>
        /// 出力用のExcelワークブックのシートの切替え
        /// </summary>
        /// <param name="excelSheetType">Excelシートの種類</param>
        private void SwitchOutputWorkBookSheet(SSYM040BExcelSheetType excelSheetType)
        {
            try
            {
                // 対象のワークシートの検索
                outputWorkSheet = ExcelUtils.SearchSheet(outputWorkBook, excelSheetNameMap[excelSheetType]);
                outputWorkSheet.Select(true);
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 詳細データの書込み
        /// </summary>
        /// <param name="list">詳細データ</param>
        /// <param name="atuenData">圧延データ</param>
        private void WriteDetailData(IList<SSYM040BDTO> list, IList<AtuenDTO> atuenData)
        {
            Range rangeDetail = null;
            Range rangeAtuen = null;
            int listCount = list.Count;
            int atuenDataCount = atuenData.Count;

            // 罫線(詳細)
            int detailStartLeftRowPosition = 4;               // 左上の行番号
            int detailEndRightRowPosition = listCount + 3;    // 右下の行番号

            try
            {
                // 印刷範囲の設定
                outputWorkSheet.PageSetup.PrintArea = @"$A$1:$K$" + (detailEndRightRowPosition + 3 + atuenDataCount).ToString();
                // 罫線および値を出力するレンジの設定(詳細)
                rangeDetail = outputWorkSheet.Range("$A$" + (detailStartLeftRowPosition).ToString(),
                                                    "$K$" + (detailEndRightRowPosition).ToString());
                rangeDetail.Borders[XlBordersIndex.xlInsideHorizontal].LineStyle = XlLineStyle.xlContinuous;
                rangeDetail.Borders[XlBordersIndex.xlInsideHorizontal].ColorIndex = 1;
                rangeDetail.Borders[XlBordersIndex.xlInsideVertical].LineStyle = XlLineStyle.xlContinuous;
                rangeDetail.Borders[XlBordersIndex.xlInsideVertical].ColorIndex = 1;
                rangeDetail.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
                rangeDetail.Borders[XlBordersIndex.xlEdgeTop].ColorIndex = 1;
                rangeDetail.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
                rangeDetail.Borders[XlBordersIndex.xlEdgeBottom].ColorIndex = 1;
                rangeDetail.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
                rangeDetail.Borders[XlBordersIndex.xlEdgeLeft].ColorIndex = 1;
                rangeDetail.Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;
                rangeDetail.Borders[XlBordersIndex.xlEdgeRight].ColorIndex = 1;

                var outputData = new object[listCount, 11];
                for (int i = 0; i < listCount; ++i)
                {
                    // 年月日
                    outputData[i, 0] = list[i].YYYYMMDD.Trim() != SummaryMeanValue ?
                                       list[i].YYYYMMDD : SummaryKeyWord;
                    // 中板ﾗｲﾝ屑
                    outputData[i, 1] = list[i].MiddlePlateWaste;
                    // 日清ﾘﾀｰﾝ屑
                    outputData[i, 2] = list[i].NissinReturnWaste;
                    // ﾄﾘｰﾏｰ屑
                    outputData[i, 3] = list[i].TrimmerWaste;
                    // 厚板ﾗｲﾝD1屑
                    outputData[i, 4] = list[i].AtuItaLineWaste;
                    // ﾌﾟﾚｰﾅ屑他
                    outputData[i, 5] = list[i].PlanerWaste;
                    // ﾚｰｻﾞｰ屑
                    outputData[i, 6] = list[i].LaserWaste;
                    // ﾌﾟﾚｰﾅ屑知多
                    outputData[i, 7] = list[i].PlanerChitaWaste;
                    // ﾐｽﾛｰﾙ
                    outputData[i, 8] = list[i].MissRollWaste;
                    // ｺﾗﾑ返品屑
                    outputData[i, 9] = list[i].ColumuReturnWaste;
                    // 合計
                    outputData[i, 10] = list[i].Total;
                }

                rangeDetail.Value2 = outputData;

                // 罫線(圧延)
                int atuenStartLeftRowPosition 
                    = detailEndRightRowPosition + 2;                     // 左上の行番号
                int atuenEndRightRowPosition 
                    = atuenStartLeftRowPosition + atuenDataCount + 1;    // 右下の行番号

                // 罫線および値を出力するレンジの設定(圧延)
                rangeAtuen = outputWorkSheet.Range("$A$" + (atuenStartLeftRowPosition + 1).ToString(),
                                                   "$C$" + (atuenEndRightRowPosition).ToString());
                rangeAtuen.Borders[XlBordersIndex.xlInsideHorizontal].LineStyle = XlLineStyle.xlContinuous;
                rangeAtuen.Borders[XlBordersIndex.xlInsideHorizontal].ColorIndex = 1;
                rangeAtuen.Borders[XlBordersIndex.xlInsideVertical].LineStyle = XlLineStyle.xlContinuous;
                rangeAtuen.Borders[XlBordersIndex.xlInsideVertical].ColorIndex = 1;
                rangeAtuen.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
                rangeAtuen.Borders[XlBordersIndex.xlEdgeTop].ColorIndex = 1;
                rangeAtuen.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
                rangeAtuen.Borders[XlBordersIndex.xlEdgeBottom].ColorIndex = 1;
                rangeAtuen.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
                rangeAtuen.Borders[XlBordersIndex.xlEdgeLeft].ColorIndex = 1;
                rangeAtuen.Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;
                rangeAtuen.Borders[XlBordersIndex.xlEdgeRight].ColorIndex = 1;

                // 圧延
                outputWorkSheet.Range("$A$" + atuenStartLeftRowPosition.ToString()).Value2 = "(圧延)";
                outputWorkSheet.Range("$A$" + atuenStartLeftRowPosition.ToString()).HorizontalAlignment = Constants.xlLeft;
                outputWorkSheet.Range("$A$" + atuenStartLeftRowPosition.ToString()).Font.Bold = true;
                outputWorkSheet.Range("$A$" + atuenStartLeftRowPosition.ToString()).Font.Size = 16;

                // 端板
                outputWorkSheet.Range("$A$" + (atuenStartLeftRowPosition + 1).ToString()).Value2 = "端板";
                outputWorkSheet.Range("$A$" + (atuenStartLeftRowPosition + 1).ToString()).HorizontalAlignment = Constants.xlCenter;

                // 伸鉄材
                outputWorkSheet.Range("$B$" + (atuenStartLeftRowPosition + 1).ToString()).Value2 = "伸鉄材";
                outputWorkSheet.Range("$B$" + (atuenStartLeftRowPosition + 1).ToString()).HorizontalAlignment = Constants.xlCenter;

                // 合計
                outputWorkSheet.Range("$C$" + (atuenStartLeftRowPosition + 1).ToString()).Value2 = "合計";
                outputWorkSheet.Range("$C$" + (atuenStartLeftRowPosition + 1).ToString()).HorizontalAlignment = Constants.xlCenter;

                int offset = 2;
                foreach (var item in atuenData)
                {
                    // 端板
                    outputWorkSheet.Range("$A$" + (atuenStartLeftRowPosition + offset).ToString()).Value2 = item.EndPlate;
                    outputWorkSheet.Range("$A$" + (atuenStartLeftRowPosition + offset).ToString()).HorizontalAlignment = Constants.xlRight;
                    outputWorkSheet.Range("$A$" + (atuenStartLeftRowPosition + offset).ToString()).NumberFormatLocal = "#,###";

                    // 伸鉄材
                    outputWorkSheet.Range("$B$" + (atuenStartLeftRowPosition + offset).ToString()).Value2 = item.BusheledIronMaterial;
                    outputWorkSheet.Range("$B$" + (atuenStartLeftRowPosition + offset).ToString()).HorizontalAlignment = Constants.xlRight;
                    if(item.BusheledIronMaterial != 0)
                       outputWorkSheet.Range("$B$" + (atuenStartLeftRowPosition + offset).ToString()).NumberFormatLocal = "#,###";
                    
                    // 合計
                    outputWorkSheet.Range("$C$" + (atuenStartLeftRowPosition + offset).ToString()).Value2 = item.Total;
                    outputWorkSheet.Range("$C$" + (atuenStartLeftRowPosition + offset).ToString()).HorizontalAlignment = Constants.xlRight;
                    outputWorkSheet.Range("$C$" + (atuenStartLeftRowPosition + offset).ToString()).NumberFormatLocal = "#,###";
                    offset++;
                }
            }

            catch (Exception)
            {
                throw;
            }

            finally
            {
                if (rangeDetail != null)
                {
                    Marshal.ReleaseComObject(rangeDetail);
                    rangeDetail = null;
                }

                if (rangeAtuen != null)
                {
                    Marshal.ReleaseComObject(rangeAtuen);
                    rangeAtuen = null;
                }
            }
        }

        /// <summary>
        /// 圧延データの書込み
        /// </summary>
        /// <param name="list">圧延データ</param>
        private void WriteAtuenData(IList<AtuenDTO> list)
        {
            Range range = null;
            int listCount = list.Count;

            // 罫線
            int startLeftRowPosition = 4;               // 左上の行番号
            int endRightRowPosition = listCount + 3;    // 右下の行番号

            try
            {
                // 印刷範囲の設定
                outputWorkSheet.PageSetup.PrintArea = @"$A$1:$C$" + (endRightRowPosition).ToString();
                // 罫線および値を出力するレンジの設定
                range = outputWorkSheet.Range("$A$" + (startLeftRowPosition).ToString(),
                                              "$C$" + (endRightRowPosition).ToString());

                range.Borders[XlBordersIndex.xlInsideHorizontal].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlInsideHorizontal].ColorIndex = 1;
                range.Borders[XlBordersIndex.xlInsideVertical].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlInsideVertical].ColorIndex = 1;
                range.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlEdgeTop].ColorIndex = 1;
                range.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlEdgeBottom].ColorIndex = 1;
                range.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlEdgeLeft].ColorIndex = 1;
                range.Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlEdgeRight].ColorIndex = 1;

                var outputData = new object[listCount, 3];
                for (int i = 0; i < listCount; ++i)
                {
                    // 端板
                    outputData[i, 0] = list[i].EndPlate;
                    // 伸鉄材
                    outputData[i, 1] = list[i].BusheledIronMaterial;
                    // 合計
                    outputData[i, 2] = list[i].Total;
                }

                range.Value2 = outputData;
            }

            catch (Exception)
            {
                throw;
            }

            finally
            {
                if (range != null)
                {
                    Marshal.ReleaseComObject(range);
                    range = null;
                }
            }

        }

        /// <summary>
        /// 年月日の書込み
        /// </summary>
        /// <param name="range">書込み範囲</param>
        /// <param name="datetime">現在時刻</param>
        protected override void WriteYearMonth(Range range, DateTime datetime)
        {
            range.Value2 = "                 " + datetime.ToString("G");
        }

        /// <summary>
        /// 圧延のDTO
        /// </summary>
        internal class AtuenDTO : DTOBase
        {
            #region コンストラクタ

            /// <summary>
            /// コンストラクタ
            /// </summary>
            internal AtuenDTO()
            {
            }

            #endregion

            #region プロパティ

            /// <summary>
            /// 端板
            /// </summary>
            internal int EndPlate { get; set; }

            /// <summary>
            /// 伸鉄材
            /// </summary>
            internal int BusheledIronMaterial { get; set; }

            /// <summary>
            /// 合計
            /// </summary>
            internal int Total { get; set; }

            #endregion

        }

        #endregion

    }

    /// <summary>
    /// スクラップ外販明細(SSYM050F)のExcelライター
    /// </summary>
    internal class SSYM050FExcelWriter : AbstractExcelWriter
    {
        #region フィールド

        /// <summary>
        /// 合計を意味する値
        /// </summary>
        private static readonly string SummaryMeanValue = "GOUKEI";

        /// <summary>
        /// 合計のキーワード
        /// </summary>
        private static readonly string SummaryKeyWord = "計";

        /// <summary>
        /// ExcelApplication
        /// </summary>
        private Application application = null;

        /// <summary>
        /// テンプレート用のワークブック
        /// </summary>
        private Workbook templateWorkBook = null;

        /// <summary>
        /// テンプレート用のワークシート
        /// </summary>
        private Worksheet templateWorkSheet = null;

        /// <summary>
        /// 出力用のワークブック
        /// </summary>
        private Workbook outputWorkBook = null;

        /// <summary>
        /// 出力用のワークシート
        /// </summary>
        private Worksheet outputWorkSheet = null;

        /// <summary>
        /// ドキュメントに対する操作完了時のコンテナ
        /// </summary>
        private IDocumentOperationContainer container;

        #endregion

        #region プロパティ

        /// <summary>
        /// 年
        /// </summary>
        internal string Year { get; set; }

        /// <summary>
        /// 月
        /// </summary>
        internal string Month { get; set; }

        /// <summary>
        /// テンプレートワークブックのパス
        /// </summary>
        internal string TemplateWorkBookPath { get; set; }

        /// <summary>
        /// 出力ワークブックのパス
        /// </summary>
        internal string OutputWorkBookPath { get; set; }

        /// <summary>
        /// 出力ワークブックのフォルダ
        /// </summary>
        internal string OutputWorkBookFolder { get; set; }

        /// <summary>
        /// 出力ワークブック名
        /// </summary>
        internal string OutputWorkBookName { get; set; }

        /// <summary>
        /// プレビューを表示するか
        /// </summary>
        internal bool IsShowPreview { get; set; }

        /// <summary>
        /// Excelシート名のマップ
        /// </summary>
        private static readonly IDictionary<SSYM050BExcelSheetType, string> excelSheetNameMap = null;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// 静的コンストラクタ
        /// </summary>
        static SSYM050FExcelWriter()
        {
            excelSheetNameMap
                = new Dictionary<SSYM050BExcelSheetType, string>();
            excelSheetNameMap.Add(SSYM050BExcelSheetType.Detail, SSYM050BExcelSheetType.Detail.GetValue());
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        internal SSYM050FExcelWriter()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="container">ドキュメントに対する操作完了時のコンテナ</param>
        internal SSYM050FExcelWriter(IDocumentOperationContainer container)
        {
            this.container = container;
        }

        #endregion

        #region メソッド

        /// <summary>
        /// 初期化処理
        /// </summary>
        public override void Initialize()
        {
            try
            {
                // ExcelApplicationの取得
                application = ExcelUtils.GetExcelApplication();
                application.DisplayAlerts = false;

                // Excelテンプレート用のExcelワークブックの取得
                templateWorkBook = ExcelUtils.GetExcelTemplateWorkBook(application, TemplateWorkBookPath);

                // 出力用のExcelワークブックのフォルダ
                OutputWorkBookFolder = OutputWorkBookFolder ??
                    Path.GetDirectoryName(OutputWorkBookPath) + "\\";
                // 現在時刻の取得
                DateTimeNow = DateTime.Now;
                // 出力用のExcelワークブック名
                OutputWorkBookName = OutputWorkBookName ??
                    (Path.GetFileName(OutputWorkBookPath)).ConvertExcelWorkBookName(DateTimeNow);
                // 出力用のExcelワークブックの取得
                outputWorkBook = ExcelUtils.GetExcelOutputWorkBook(application,
                                                                   OutputWorkBookFolder,
                                                                   OutputWorkBookName,
                                                                   TemplateWorkBookPath, true, true);
            }

            catch (Exception)
            {
                // COMオブジェクトの解放
                ReleaseComObject();
                throw;
            }
        }

        /// <summary>
        /// 書込み前処理
        /// </summary>
        public override void PreWrite()
        {
        }

        /// <summary>
        /// ヘッダーの書込み
        /// </summary>
        /// <typeparam name="T">ヘッダーを継承した任意のオブジェクトの型</typeparam>
        /// <param name="header">ファイルのヘッダー情報</param>
        public override void WriteHeader<T>(T header)
        {
        }

        /// <summary>
        /// 詳細の書込み
        /// </summary>
        /// <typeparam name="T">詳細を継承した任意のオブジェクトの型</typeparam>
        /// <param name="detail">ファイルの詳細情報</param>
        public override void WriteDetail<T>(IList<T> detail)
        {
            try
            {
                // 詳細
                var detailData = new List<SSYM050BDTO>();
                var targets = detail[0] as ScrapExternalSalesDetail;
                var items = targets.SSYM050BDTOList;
                items.ForEach(item =>
                {
                    detailData.Add(item);
                });
                // ワークシートの切替え(詳細)
                SwitchOutputWorkBookSheet(SSYM050BExcelSheetType.Detail);
                // 年月日の書込み(タイトルの右端)
                WriteYearMonth(outputWorkSheet.Range("$F$1"), DateTimeNow);
                // 詳細データの書込み
                WriteDetailData(detailData);
            }

            catch (Exception)
            {
                // COMオブジェクトの解放
                ReleaseComObject();
                throw;
            }
        }

        /// <summary>
        /// フッターの書込み
        /// </summary>
        /// <typeparam name="T">フッターを継承した任意のオブジェクトの型</typeparam>
        /// <param name="footer">ファイルのフッター情報</param>
        public override void WriteFooter<T>(T footer)
        {
            // 処理不要
        }

        /// <summary>
        /// 書込み後処理
        /// </summary>
        public override void PostWrite()
        {
            // 処理不要
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public override void Terminate()
        {
            try
            {
                // ワークブックの保存
                outputWorkBook.Save(OutputWorkBookFolder, OutputWorkBookName);
                if (container != null)
                {
                    container.NotifyOutputExcel(OutputWorkBookFolder + OutputWorkBookName);
                }
            }

            catch (Exception)
            {
                throw;
            }

            finally
            {
                // COMオブジェクトの解放
                ReleaseComObject();
            }
        }

        /// <summary>
        /// COMオブジェクトの解放
        /// </summary>
        protected override void ReleaseComObject()
        {
            if (outputWorkBook != null)
            {
                outputWorkBook.Close(false, false, false);
            }

            if (templateWorkBook != null)
            {
                templateWorkBook.Close(false, false, false);
            }

            if (application != null)
            {
                application.Visible = false;
                application.DisplayAlerts = true;
                application.Quit();
                Marshal.ReleaseComObject(application);
                application = null;
            }

            if (outputWorkBook != null)
            {
                Marshal.ReleaseComObject(outputWorkBook);
                outputWorkBook = null;
            }

            if (templateWorkBook != null)
            {
                Marshal.ReleaseComObject(templateWorkBook);
                templateWorkBook = null;
            }

            if (templateWorkSheet != null)
            {
                Marshal.ReleaseComObject(templateWorkSheet);
                templateWorkSheet = null;
            }

            if (outputWorkSheet != null)
            {
                Marshal.ReleaseComObject(outputWorkSheet);
                outputWorkSheet = null;
            }
        }

        /// <summary>
        /// 出力用のExcelワークブックのシートの切替え
        /// </summary>
        /// <param name="excelSheetType">Excelシートの種類</param>
        private void SwitchOutputWorkBookSheet(SSYM050BExcelSheetType excelSheetType)
        {
            try
            {
                // 対象のワークシートの検索
                outputWorkSheet = ExcelUtils.SearchSheet(outputWorkBook, excelSheetNameMap[excelSheetType]);
                outputWorkSheet.Select(true);
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 詳細データの書込み
        /// </summary>
        /// <param name="list">詳細データ</param>
        private void WriteDetailData(IList<SSYM050BDTO> list)
        {
            Range range = null;
            int listCount = list.Count;

            // 罫線
            int startLeftRowPosition = 4;               // 左上の行番号
            int endRightRowPosition = listCount + 3;    // 右下の行番号

            try
            {
                // 印刷範囲の設定
                outputWorkSheet.PageSetup.PrintArea = @"$A$1:$G$" + (endRightRowPosition);
                // 罫線および値を出力するレンジの設定
                range = outputWorkSheet.Range("$A$" + (startLeftRowPosition).ToString(),
                                              "$G$" + (endRightRowPosition).ToString());
                range.Borders[XlBordersIndex.xlInsideHorizontal].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlInsideHorizontal].ColorIndex = 1;
                range.Borders[XlBordersIndex.xlInsideVertical].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlInsideVertical].ColorIndex = 1;
                range.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlEdgeTop].ColorIndex = 1;
                range.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlEdgeBottom].ColorIndex = 1;
                range.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlEdgeLeft].ColorIndex = 1;
                range.Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlEdgeRight].ColorIndex = 1;

                var outputData = new object[listCount, 7];
                for (int i = 0; i < listCount; ++i)
                {
                    // 年月日
                    outputData[i, 0] = list[i].YYYYMMDD.Trim() != SummaryMeanValue ?
                                       list[i].YYYYMMDD : SummaryKeyWord;
                    // ｴﾝﾄﾞｼﾔｰ屑
                    outputData[i, 1] = list[i].EndShearWaste;
                    // ﾌﾟﾚｰﾅｰ屑
                    outputData[i, 2] = list[i].PlanerWaste;
                    // ﾄﾘｰﾏｰ屑
                    outputData[i, 3] = list[i].TrimmerWaste;
                    // ﾚｰｻﾞｰ屑
                    outputData[i, 4] = list[i].LaserWaste;
                    // その他
                    outputData[i, 5] = list[i].OtherWaste;
                    // 合計
                    outputData[i, 6] = list[i].Total;
                }

                range.Value2 = outputData;
            }

            catch (Exception)
            {
                throw;
            }

            finally
            {
                if (range != null)
                {
                    Marshal.ReleaseComObject(range);
                    range = null;
                }
            }
        }

        /// <summary>
        /// 年月日の書込み
        /// </summary>
        /// <param name="range">書込み範囲</param>
        /// <param name="datetime">現在時刻</param>
        protected override void WriteYearMonth(Range range, DateTime datetime)
        {
            range.Value2 = "           " + datetime.ToString("G");
        }

        #endregion

    }

    /// <summary>
    /// 在庫売り受注一覧表(SSZA400B)のExcelライター
    /// </summary>
    internal class SSZA400BExcelWriter : AbstractExcelWriter
    {
        #region フィールド

        /// <summary>
        /// ExcelApplication
        /// </summary>
        private Application application = null;

        /// <summary>
        /// テンプレート用のワークブック
        /// </summary>
        private Workbook templateWorkBook = null;

        /// <summary>
        /// テンプレート用のワークシート
        /// </summary>
        private Worksheet templateWorkSheet = null;

        /// <summary>
        /// 出力用のワークブック
        /// </summary>
        private Workbook outputWorkBook = null;

        /// <summary>
        /// 出力用のワークシート
        /// </summary>
        private Worksheet outputWorkSheet = null;

        /// <summary>
        /// ドキュメントに対する操作完了時のコンテナ
        /// </summary>
        private IDocumentOperationContainer container;

        #endregion

        #region プロパティ

        /// <summary>
        /// 年
        /// </summary>
        internal string Year { get; set; }

        /// <summary>
        /// 月
        /// </summary>
        internal string Month { get; set; }

        /// <summary>
        /// テンプレートワークブックのパス
        /// </summary>
        internal string TemplateWorkBookPath { get; set; }

        /// <summary>
        /// 出力ワークブックのパス
        /// </summary>
        internal string OutputWorkBookPath { get; set; }

        /// <summary>
        /// 出力ワークブックのフォルダ
        /// </summary>
        internal string OutputWorkBookFolder { get; set; }

        /// <summary>
        /// 出力ワークブック名
        /// </summary>
        internal string OutputWorkBookName { get; set; }

        /// <summary>
        /// プレビューを表示するか
        /// </summary>
        internal bool IsShowPreview { get; set; }

        /// <summary>
        /// Excelシート名のマップ
        /// </summary>
        private static readonly IDictionary<SSZA040BExcelSheetType, string> excelSheetNameMap = null;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// 静的コンストラクタ
        /// </summary>
        static SSZA400BExcelWriter()
        {
            excelSheetNameMap
                = new Dictionary<SSZA040BExcelSheetType, string>();
            excelSheetNameMap.Add(SSZA040BExcelSheetType.Detail, SSZA040BExcelSheetType.Detail.GetValue());
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        internal SSZA400BExcelWriter()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="container">ドキュメントに対する操作完了時のコンテナ</param>
        internal SSZA400BExcelWriter(IDocumentOperationContainer container)
        {
            this.container = container;
        }

        #endregion

        #region メソッド

        /// <summary>
        /// 初期化処理
        /// </summary>
        public override void Initialize()
        {
            try
            {
                // ExcelApplicationの取得
                application = ExcelUtils.GetExcelApplication();
                application.DisplayAlerts = false;

                // Excelテンプレート用のExcelワークブックの取得
                templateWorkBook = ExcelUtils.GetExcelTemplateWorkBook(application, TemplateWorkBookPath);

                // 出力用のExcelワークブックのフォルダ
                OutputWorkBookFolder = OutputWorkBookFolder ??
                    Path.GetDirectoryName(OutputWorkBookPath) + "\\";
                // 現在時刻の取得
                DateTimeNow = DateTime.Now;
                // 出力用のExcelワークブック名
                OutputWorkBookName = OutputWorkBookName ??
                    (Path.GetFileName(OutputWorkBookPath)).ConvertExcelWorkBookName(DateTimeNow);
                // 出力用のExcelワークブックの取得
                outputWorkBook = ExcelUtils.GetExcelOutputWorkBook(application,
                                                                   OutputWorkBookFolder,
                                                                   OutputWorkBookName,
                                                                   TemplateWorkBookPath, true, true);
            }

            catch (Exception)
            {
                // COMオブジェクトの解放
                ReleaseComObject();
                throw;
            }
        }

        /// <summary>
        /// 書込み前処理
        /// </summary>
        public override void PreWrite()
        {
        }

        /// <summary>
        /// ヘッダーの書込み
        /// </summary>
        /// <typeparam name="T">ヘッダーを継承した任意のオブジェクトの型</typeparam>
        /// <param name="header">ファイルのヘッダー情報</param>
        public override void WriteHeader<T>(T header)
        {
        }

        /// <summary>
        /// 詳細の書込み
        /// </summary>
        /// <typeparam name="T">詳細を継承した任意のオブジェクトの型</typeparam>
        /// <param name="detail">ファイルの詳細情報</param>
        public override void WriteDetail<T>(IList<T> detail)
        {
            try
            {
                // 詳細
                var detailData = new List<SSZA040BDTO>();
                var targets = detail[0] as StockSellOrdersDetail;
                var items = targets.SSZA040BDTOList;
                items.ForEach(item =>
                {
                    detailData.Add(item);
                });
                // ワークシートの切替え(詳細)
                SwitchOutputWorkBookSheet(SSZA040BExcelSheetType.Detail);
                // 年月日の書込み(タイトルの右端)
                WriteYearMonth(outputWorkSheet.Range("$N$1"), DateTimeNow);
                // 詳細データの書込み
                WriteDetailData(detailData);
            }

            catch (Exception)
            {
                // COMオブジェクトの解放
                ReleaseComObject();
                throw;
            }
        }

        /// <summary>
        /// フッターの書込み
        /// </summary>
        /// <typeparam name="T">フッターを継承した任意のオブジェクトの型</typeparam>
        /// <param name="footer">ファイルのフッター情報</param>
        public override void WriteFooter<T>(T footer)
        {
            // 処理不要
        }

        /// <summary>
        /// 書込み後処理
        /// </summary>
        public override void PostWrite()
        {
            // 処理不要
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public override void Terminate()
        {
            try
            {
                // ワークブックの保存
                outputWorkBook.Save(OutputWorkBookFolder, OutputWorkBookName);
                if (container != null)
                {
                    container.NotifyOutputExcel(OutputWorkBookFolder + OutputWorkBookName);
                }
            }

            catch (Exception)
            {
                throw;
            }

            finally
            {
                // COMオブジェクトの解放
                ReleaseComObject();
            }
        }

        /// <summary>
        /// COMオブジェクトの解放
        /// </summary>
        protected override void ReleaseComObject()
        {
            if (outputWorkBook != null)
            {
                outputWorkBook.Close(false, false, false);
            }

            if (templateWorkBook != null)
            {
                templateWorkBook.Close(false, false, false);
            }

            if (application != null)
            {
                application.Visible = false;
                application.DisplayAlerts = true;
                application.Quit();
                Marshal.ReleaseComObject(application);
                application = null;
            }

            if (outputWorkBook != null)
            {
                Marshal.ReleaseComObject(outputWorkBook);
                outputWorkBook = null;
            }

            if (templateWorkBook != null)
            {
                Marshal.ReleaseComObject(templateWorkBook);
                templateWorkBook = null;
            }

            if (templateWorkSheet != null)
            {
                Marshal.ReleaseComObject(templateWorkSheet);
                templateWorkSheet = null;
            }

            if (outputWorkSheet != null)
            {
                Marshal.ReleaseComObject(outputWorkSheet);
                outputWorkSheet = null;
            }
        }

        /// <summary>
        /// 出力用のExcelワークブックのシートの切替え
        /// </summary>
        /// <param name="excelSheetType">月報作成用リストのExcelシートの種類</param>
        private void SwitchOutputWorkBookSheet(SSZA040BExcelSheetType excelSheetType)
        {
            try
            {
                // 対象のワークシートの検索
                outputWorkSheet = ExcelUtils.SearchSheet(outputWorkBook, excelSheetNameMap[excelSheetType]);
                outputWorkSheet.Select(true);
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 詳細データの書込み
        /// </summary>
        /// <param name="list">詳細データ</param>
        private void WriteDetailData(IList<SSZA040BDTO> list)
        {
            Range range = null;
            int listCount = list.Count;

            // 罫線
            int startLeftRowPosition = 5;               // 左上の行番号
            int endRightRowPosition = listCount + 4;    // 右下の行番号

            try
            {
                // 印刷範囲の設定
                outputWorkSheet.PageSetup.PrintArea = @"$A$1:$P$" + (endRightRowPosition).ToString();
                // 罫線および値を出力するレンジの設定
                range = outputWorkSheet.Range("$A$" + (startLeftRowPosition).ToString(),
                                              "$P$" + (endRightRowPosition).ToString());
                range.Borders[XlBordersIndex.xlInsideHorizontal].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlInsideHorizontal].ColorIndex = 1;
                range.Borders[XlBordersIndex.xlInsideVertical].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlInsideVertical].ColorIndex = 1;
                range.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlEdgeTop].ColorIndex = 1;
                range.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlEdgeBottom].ColorIndex = 1;
                range.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlEdgeLeft].ColorIndex = 1;
                range.Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlEdgeRight].ColorIndex = 1;

                var outputData = new object[listCount, 16];
                for (int i = 0; i < listCount; ++i)
                {
                    // 受注№
                    outputData[i, 0] = list[i].OrderNo;
                    // 規格名
                    outputData[i, 1] = list[i].StandardName;
                    // 製品サイズ(厚)
                    outputData[i, 2] = list[i].ProductAtu;
                    // 製品サイズ(巾)
                    outputData[i, 3] = list[i].ProductHaba;
                    // 製品サイズ(長)
                    outputData[i, 4] = list[i].ProductNaga;
                    // 定耳
                    outputData[i, 5] = list[i].TM;
                    // 製品コード
                    outputData[i, 6] = list[i].ProductCode;
                    // 需要家
                    outputData[i, 7] = list[i].CustomerName;
                    // 枚数
                    outputData[i, 8] = list[i].OrderCount;
                    // 重量
                    outputData[i, 9] = list[i].OrderWeight;
                    // 出荷枚数
                    outputData[i, 10] = list[i].ShipmentCount;
                    // 受渡場所
                    outputData[i, 11] = list[i].TransferPlaceName;
                    // 受渡条件
                    outputData[i, 12] = list[i].TransferCondition;
                    // 納期
                    outputData[i, 13] = list[i].DeliveryDate;
                    // 納期ランク
                    outputData[i, 14] = list[i].DeliveryDateRank;
                    // 引当
                    outputData[i, 15] = list[i].ProvisionClassfication;
                }

                range.Value2 = outputData;
            }

            catch (Exception)
            {
                throw;
            }

            finally
            {
                if (range != null)
                {
                    Marshal.ReleaseComObject(range);
                    range = null;
                }
            }
        }

        /// <summary>
        /// 年月日の書込み
        /// </summary>
        /// <param name="range">書込み範囲</param>
        /// <param name="datetime">現在時刻</param>
        protected override void WriteYearMonth(Range range, DateTime datetime)
        {
            range.Value2 = "          " + datetime.ToString("G");
        }

        #endregion
    }
}

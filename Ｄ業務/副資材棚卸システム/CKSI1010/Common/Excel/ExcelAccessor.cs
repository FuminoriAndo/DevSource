using CKSI1010.Common.Excel.DTO;
using CKSI1010.Operation.Print.ViewModel;
using CKSI1010.Shared;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using static CKSI1010.Common.Constants;
//*************************************************************************************
//
//   Excelアクセッサー
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************
namespace CKSI1010.Common
{
    /// <summary>
    /// Excelアクセッサー
    /// </summary>
    internal class ExcelAccessor
    {

        #region フィールド

        /// <summary>
        /// 実行するかどうか
        /// </summary>
        private readonly string Do_Action = "1";

        /// <summary>
        /// 実行する
        /// </summary>
        private readonly string Action_Value = "する";

        /// <summary>
        /// 実行しない
        /// </summary>
        private readonly string Not_Action_Value = "しない";

        /// <summary>
        /// 入庫払い
        /// </summary>
        private readonly string Nyukobarai_StringDefine = "入庫払い";

        /// <summary>
        /// 使用高払い
        /// </summary>
        private readonly string Siyoudakabarai_StringDefine = "使用高払い";

        /// <summary>
        /// 受払い種別("1")
        /// </summary>
        private readonly string NyukoBarai = "1";

        /// <summary>
        /// 上(上期)
        /// </summary>
        private readonly string Kamiki_StringDefine = "上";

        /// <summary>
        /// 下(下期)
        /// </summary>
        private readonly string Simoki_StringDefine = "下";

        /// <summary>
        /// 資材棚卸し調査表のタイトル表示用の定型フォーマット
        /// </summary>
        private readonly string InventoryPrint_Title_Format = "{0}/{1}(第{2}{3}期)";

        /// <summary>
        /// 単価未入力一覧のシート名
        /// </summary>
        private readonly string UnInputTankaList_SheetName = "単価未入力リスト";

        /// <summary>
        /// 使用高払い受払い計算書のシート名
        /// </summary>
        private readonly string Siyoudakabarai_SheetName = "使用高払い受払い計算書";

        /// <summary>
        /// 入庫払い受払い計算書のシート名
        /// </summary>
        private readonly string Nyukobarai_SheetName = "入庫払い受払い計算書";

        /// <summary>
        /// 立会い用棚卸表のシート名
        /// </summary>
        private readonly string WitnessInventory_SheetName = "立会い用棚卸表";

        /// <summary>
        /// 財務提出用棚卸表のシート名
        /// </summary>
        private readonly string FinancingPresetationInventory_SheetName = "財務提出用棚卸表";

        /// <summary>
        /// 資材品目マスタ変更履歴のシート名
        /// </summary>
        private readonly string SizaiHinmoku_ChangeList_SheetName = "資材品目マスタ変更履歴";

        /// <summary>
        /// 立会い用棚卸表のタイトル
        /// </summary>
        private readonly string WitnessInventory_Title = "期製鋼工場副資材棚卸立会い表";

        /// <summary>
        /// 財務提出用棚卸表のタイトル
        /// </summary>
        private readonly string FinancingPresetationInventory_Title = "期製鋼工場副資材棚卸表";

        /// <summary>
        /// ヘッダ(内訳)
        /// </summary>
        private readonly string Header_Utiwake = "内訳";

        /// <summary>
        /// ヘッダ(内訳名)
        /// </summary>
        private readonly string Header_UtiwakeName = "内訳名";

        /// <summary>
        /// ヘッダ(棚番)
        /// </summary>
        private readonly string Header_Tanaban = "棚番";

        /// <summary>
        /// ヘッダ(品目名)
        /// </summary>
        private readonly string Header_HinmokuName = "品目名";

        /// <summary>
        /// ヘッダ(品目名２)
        /// </summary>
        private readonly string Header_HinmokuName2 = "品目名２";

        /// <summary>
        /// ヘッダ(口座名２)
        /// </summary>
        private readonly string Header_KouzaName = "口座名２";

        /// <summary>
        /// ヘッダ(単位)
        /// </summary>
        private readonly string Header_Tani = "単位";

        /// <summary>
        /// ヘッダ(月初在庫)
        /// </summary>
        private readonly string Header_SZaiko = "月初在庫";

        /// <summary>
        /// ヘッダ(当月入庫)
        /// </summary>
        private readonly string Header_Nyuko = "当月入庫";

        /// <summary>
        /// ヘッダ(ＥＦ出庫)
        /// </summary>
        private readonly string Header_EFShukko = "ＥＦ出庫";

        /// <summary>
        /// ヘッダ(ＬＦ出庫)
        /// </summary>
        private readonly string Header_LFShukko = "ＬＦ出庫";

        /// <summary>
        /// ヘッダ(ＣＣ出庫)
        /// </summary>
        private readonly string Header_CCShukko = "ＣＣ出庫";

        /// <summary>
        /// ヘッダ(その他)
        /// </summary>
        private readonly string Header_OtherShukko = "その他";

        /// <summary>
        /// ヘッダ(事業)
        /// </summary>
        private readonly string Header_BusinessDevelopment = "事業";

        /// <summary>
        /// ヘッダ(１次)
        /// </summary>
        private readonly string Header_PrimaryCutting = "１次";

        /// <summary>
        /// ヘッダ(ＴＤ出庫)
        /// </summary>
        private readonly string Header_TDShukko = "ＴＤ出庫";

        /// <summary>
        /// ヘッダ(２次)
        /// </summary>
        private readonly string Header_SecondarycCtting = "２次";

        /// <summary>
        /// ヘッダ(月末在庫)
        /// </summary>
        private readonly string Header_EZaiko = "月末在庫";

        /// <summary>
        /// ExcelAccessor(シングルトン)
        /// </summary>
        private static ExcelAccessor excelAccessor = null;

        /// <summary>
        /// XDocumentオブジェクト
        /// </summary>
        private static XDocument xDocument = null;

        /// <summary>
        /// 帳票出力用のExcelテンプレートブックのファイルパス
        /// </summary>
        private string excelTemplateBookPath = null;

        /// <summary>
        /// 帳票出力用のExcelのパス
        /// </summary>
        private string outputBookPath = null;

        /// <summary>
        /// 帳票出力用のExcelのフォルダ
        /// </summary>
        private string outputBookFolder = null;

        /// <summary>
        /// 帳票出力用のExcelのブック名
        /// </summary>
        private string outputBookName = null;

        #endregion

        #region 列挙型

        /// <summary>
        /// 会計期間
        /// </summary>
        private enum KaikeiKikan
        {
            // 上期開始月
            KamikiStartMonth = 4,
            // 上期終了月　
            KamikiEndMonth = 9,
            // 下期開始月
            SimokiStartMonth = 10,
            // 下期終了月　
            SimokiEndMonth = 3
        }

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        internal ExcelAccessor()
        {
            try
            {
                // XMLファイルのパスの取得
                string xmlPath = Path.Combine(Path.GetDirectoryName(
                Assembly.GetExecutingAssembly().Location) ?? string.Empty, "Settings.xml");

                // XMLファイルを読込む
                xDocument = XDocument.Load(xmlPath);

                // 帳票出力用のExcelテンプレートブックのファイルパスの取得
                excelTemplateBookPath = getExcelTemplatBookPath();

                // 帳票出力用のExcelのパスの取得
                outputBookPath = getOutputBookPath();

                // 帳票出力用のExcelのフォルダの取得
                outputBookFolder = getOutputBookFolder();

                // 帳票出力用のExcelのブック名の取得
                outputBookName = getOutputBookName();
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region メソッド

        /// <summary>
        /// ExcelAccessorのインスタンスの取得
        /// </summary>
        /// <returns>ExcelAccessorのインスタンス</returns>
        internal static ExcelAccessor GetInstance()
        {
            try
            {
                excelAccessor = new ExcelAccessor();
            }

            catch (Exception)
            {
                throw;
            }

            return excelAccessor;
        }

        /// <summary>
        /// ExcelApplicationの取得
        /// </summary>
        /// <returns>ExcelApplication</returns>
        private Application getExcelApplication()
        {
            Application app = null;

            try
            {
                app = new Application();
                app.Visible = false;
            }

            catch (Exception)
            {
                if (app != null)
                {
                    app.Quit();
                    Marshal.ReleaseComObject(app);
                    app = null;
                }

                throw;
            }

            return app;
        }

        /// <summary>
        /// ExcelWorkbookの取得
        /// </summary>
        /// <param name="application">ExcelApplication</param>
        /// <param name="filePath">ファイルパス</param>
        /// <returns>ExcelWorkbook</returns>
        private Workbook getWorkBook(Application application, string filePath)
        {
            Workbook book = null;

            try
            {
                book = application.Workbooks.Open(filePath);
            }

            catch (Exception)
            {
                if (book != null)
                {
                    book.Close(false);
                    Marshal.ReleaseComObject(book);
                    book = null;
                }
                throw;
            }

            return book;
        }


        /// <summary>
        /// Excelテンプレート用のExcelWorkbookの取得
        /// </summary>
        /// <param name="application">ExcelApplication</param>
        /// <returns>Excelテンプレート用のExcelWorkbook</returns>
        private Workbook getExcelTemplateWorkBook(Application application)
        {
            Workbook book = null;

            try
            {
                // ExcelWorkBookを取得する
                book = getWorkBook(application, excelTemplateBookPath);
            }

            catch (Exception)
            {
                throw;
            }

            return book;
        }

        /// <summary>
        /// 帳票出力用のExcelWorkbookの取得
        /// </summary>
        /// <param name="application">ExcelApplication</param>
        /// <returns>帳票出力用のExcelWorkbook</returns>
        private Workbook getOutputWorkBook(Application application)
        {
            Workbook book = null;

            try
            {
                if (!Directory.Exists(outputBookFolder))
                {
                    Directory.CreateDirectory(outputBookFolder);
                }

                // 帳票出力用のExcelブックが既に存在するか
                string[] fileList
                    = Directory.GetFileSystemEntries(outputBookFolder, outputBookName);

                // ファイルがなければ新規作成する。
                if (fileList.Count() == 0)
                {
                    File.Copy(excelTemplateBookPath, outputBookPath);
                }

                // ExcelWorkBookを取得する
                book = getWorkBook(application, outputBookPath);
            }

            catch (Exception)
            {
                throw;
            }

            return book;
        }

        /// <summary>
        /// 帳票出力用のExcelテンプレートブックのパスの取得
        /// </summary>
        /// <returns>帳票出力用のExcelテンプレートブックのパス</returns>
        private string getExcelTemplatBookPath()
        {
            string filePath = string.Empty;
            string fileName = string.Empty;

            try
            {
                var elem = xDocument.XPathSelectElement($"/Settings/Excel/Template");
                filePath = (string)elem.Attribute("Path");
                fileName = (string)elem.Attribute("Name");

                return filePath + fileName;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 帳票出力用のExcelテンプレートブックのフォルダの取得
        /// </summary>
        /// <returns>帳票出力用のExcelテンプレートブックのフォルダ</returns>
        private string getExcelTemplatBookFolder()
        {
            string folder = string.Empty;

            try
            {
                var elem = xDocument.XPathSelectElement($"/Settings/Excel/Template");
                folder = (string)elem.Attribute("Path");

                return folder;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 帳票出力用のExcelテンプレートブック名の取得
        /// </summary>
        /// <returns>帳票出力用のExcelテンプレートブック名</returns>
        private string getExcelTemplatBookName()
        {
            string name = string.Empty;

            try
            {
                var elem = xDocument.XPathSelectElement($"/Settings/Excel/Template");
                name = (string)elem.Attribute("Name");

                return name;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 帳票出力用のExcelのパスの取得
        /// </summary>
        /// <returns>帳票出力用のExcelのパス</returns>
        private string getOutputBookPath()
        {
            string filePath = string.Empty;
            string fileName = string.Empty;

            try
            {
                var elem = xDocument.XPathSelectElement($"/Settings/Excel/Output");
                filePath = (string)elem.Attribute("Path");
                fileName = (string)elem.Attribute("Name");

                return filePath + SharedViewModel.Instance.OperationYearMonth.YearMonth + "_" + fileName;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 帳票出力用のExcelのフォルダの取得
        /// </summary>
        /// <returns>帳票出力用のExcelのフォルダ</returns>
        private string getOutputBookFolder()
        {
            string folder = string.Empty;

            try
            {
                var elem = xDocument.XPathSelectElement($"/Settings/Excel/Output");
                folder = (string)elem.Attribute("Path");

                return folder;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 帳票出力用のExcelのブック名の取得
        /// </summary>
        /// <returns>帳票出力用のExcelのブック名の取得</returns>
        private string getOutputBookName()
        {
            string name = string.Empty;

            try
            {
                var elem = xDocument.XPathSelectElement($"/Settings/Excel/Output");
                name = (string)elem.Attribute("Name");

                return SharedViewModel.Instance.OperationYearMonth.YearMonth + "_" + name;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Excelワークシートの検索
        /// </summary>
        /// <param name="targetWorkbook">対象のWorkbook</param>
        /// <param name="targetSheetName">対象のシート名</param>
        /// <returns>Excel.Worksheetオブジェクト</returns>
        private Worksheet searchSheet(Workbook targetWorkbook, string targetSheetName)
        {

            Worksheet ret = null;

            try
            {
                foreach (Worksheet sheet in targetWorkbook.Worksheets)
                {
                    if (sheet.Name.Equals(targetSheetName))
                    {
                        ret = sheet;
                        break;
                    }
                }

                return ret;
            }

            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Excelワークシートの削除
        /// </summary>
        /// <param name="targetWorkbook">対象Workbook</param>
        /// <param name="targetSheetName">シート名</param>
        private void deleteWorksheet(Workbook targetWorkbook, string targetSheetName)
        {
            Worksheet targetSheet = null;

            try
            {
                targetSheet = searchSheet(targetWorkbook, targetSheetName);
                if (targetSheet != null)
                {
                    targetSheet.Delete();
                }
            }

            catch (Exception)
            {
                throw;
            }

            finally
            {
                if (targetSheet != null)
                {
                    Marshal.ReleaseComObject(targetSheet);
                    targetSheet = null;
                }
            }
        }

        /// <summary>
        /// 資材棚卸し調査表の出力
        /// </summary>
        /// <param name="printTarget">印刷対象の資材区部</param>
        /// <param name="printData">印刷データ</param>
        /// <param name="isTogetuClear">当月クリアかどうか</param>
        /// <returns>非同期タスク(結果)</returns>
        internal async Task<bool> OutputInventoryPrint(InventoryPrintTieSizaiCategory printTarget, PrintRecordViewModel[] printData, bool isTogetuClear)
        {
            Application application = null;
            Workbook templateWorkBook = null;
            Workbook outputWorkBook = null;
            Worksheet templateWorkSheet = null;
            Worksheet outputWorkSheet = null;
            Worksheet workSheet1 = null;
            Worksheet workSheet2= null;
            int outputWorkSheetIndex = 0;

            try
            {
                // ExcelApplicationの取得
                application = getExcelApplication();

                // Excelテンプレート用のExcelワークブックの取得
                templateWorkBook = getExcelTemplateWorkBook(application);

                var sheetNames = convertIndex2String(printTarget);
                var templateSheetName = sheetNames.Item1;　// テンプレートのワークシート名
                var sheet1Name = sheetNames.Item2;         // オリジナル
                var sheet2Name = sheetNames.Item3;         // 全て0含む

                // 帳票出力用のExcelワークブックの取得
                outputWorkBook = getOutputWorkBook(application);

                // 該当の資材区分の資材棚卸し調査表のワークシートの検索
                outputWorkSheet = searchSheet(outputWorkBook, templateSheetName);

                // コピー用にインデックスを退避する
                outputWorkSheetIndex = outputWorkSheet.Index;

                // 帳票出力用のExcelワークブックから
                // 該当の資材区分の資材棚卸し調査表のワークシートを削除する
                application.DisplayAlerts = false;
                deleteWorksheet(outputWorkBook, sheet1Name);
                deleteWorksheet(outputWorkBook, sheet2Name);
                application.DisplayAlerts = true;

                // 印刷対象データ(全て0含む)を作成
                workSheet2 = copyWorksheet(templateWorkBook, outputWorkBook, templateSheetName, sheet2Name, outputWorkSheetIndex);
                createWorksheet(workSheet2, printData.ToArray(), isTogetuClear);

                // 印刷対象データを作成
                workSheet1 = copyWorksheet(templateWorkBook, outputWorkBook, templateSheetName, sheet1Name, outputWorkSheetIndex);
                createWorksheet(workSheet1, printData.Where(t => t.IsPrintTarget).ToArray(), isTogetuClear);

                // 印刷用に選択
                workSheet1.Activate();
                outputWorkBook.Activate();

                // 印刷プレビューを表示する(画面前面表示)
                application.WindowState = XlWindowState.xlMaximized;
                WindowOperator.ForegroundWindow(application);
                application.Visible = true;
                outputWorkBook.PrintPreview();
                application.Visible = false;

                // 帳票出力用のExcelワークブックの保存
                outputWorkBook.Save();

                templateWorkBook.Close(false);
                outputWorkBook.Close(false);
            }

            catch (Exception)
            {
                throw;
            }

            finally
            {
                if (application != null)
                {
                    application.Quit();
                    Marshal.ReleaseComObject(application);
                    application = null;
                }

                if (templateWorkBook != null)
                {
                    Marshal.ReleaseComObject(templateWorkBook);
                    templateWorkBook = null;
                }

                if (outputWorkBook != null)
                {
                    Marshal.ReleaseComObject(outputWorkBook);
                    outputWorkBook = null;
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
                
                if (workSheet1 != null)
                {
                    Marshal.ReleaseComObject(workSheet1);
                    workSheet1 = null;
                }
                
                if (workSheet2 != null)
                {
                    Marshal.ReleaseComObject(workSheet2);
                    workSheet2 = null;
                }

            }

            return await Task.FromResult(true);
        }

        /// <summary>
        /// 単価未入力リストの出力
        /// </summary>
        /// <param name="list">単価未入力リスト</param>
        /// <returns>非同期タスク(結果)</returns>
        internal async Task<bool> OutputUnInputTankaList(IList<Tuple<string, string>> list)
        {
            Application application = null;
            Workbook templateWorkBook = null;
            Workbook outputWorkBook = null;
            Worksheet templateWorkSheet = null;
            Worksheet outputWorkSheet = null;
            Worksheet copyTargetWorkSheet = null;
            Range range = null;
            int outputWorkSheetIndex = 0;
            int listCount = list.Count;

            // 罫線
            int startLeftRowPosition = 3;            // 左上の行番号
            int startLeftColumnPosition = 1;         // 左上の列番号
            int endRightRowPosition = listCount + 2; // 右下の行番号
            int endRightColumnPosition = 2;          // 右下の列番号

            try
            {
                // ExcelApplicationの取得
                application = getExcelApplication();

                // Excelテンプレート用のExcelワークブックの取得
                templateWorkBook = getExcelTemplateWorkBook(application);

                // "単価未入力リスト"のワークシートの検索
                templateWorkSheet = searchSheet(templateWorkBook, UnInputTankaList_SheetName);
                templateWorkSheet.Select();

                // 印刷範囲の設定
                templateWorkSheet.PageSetup.PrintArea = $@"$A$1:$B${endRightRowPosition}";
                // 罫線および値を出力するレンジの設定
                range = templateWorkSheet
                            .Range[templateWorkSheet.Cells[startLeftRowPosition, startLeftColumnPosition],
                                    templateWorkSheet.Cells[endRightRowPosition, endRightColumnPosition]];

                range.Borders[XlBordersIndex.xlInsideHorizontal].LineStyle = XlLineStyle.xlContinuous;

                range.Borders[XlBordersIndex.xlInsideHorizontal].ColorIndex = 48;

                range.Borders[XlBordersIndex.xlInsideVertical].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlInsideVertical].ColorIndex = 48;

                range.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlDouble;
                range.Borders[XlBordersIndex.xlEdgeTop].ColorIndex = 1;

                range.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlEdgeBottom].ColorIndex = 1;
                range.Borders[XlBordersIndex.xlEdgeBottom].Weight = XlBorderWeight.xlMedium;

                range.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlEdgeLeft].ColorIndex = 1;
                range.Borders[XlBordersIndex.xlEdgeLeft].Weight = XlBorderWeight.xlMedium;

                range.Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlEdgeRight].ColorIndex = 1;
                range.Borders[XlBordersIndex.xlEdgeRight].Weight = XlBorderWeight.xlMedium;

                var outputData = new string[listCount, 2];
                for (int j = 0; j < listCount; ++j)
                {
                    outputData[j, 0] = list[j].Item1;
                    outputData[j, 1] = list[j].Item2;
                }
                range.Value2 = outputData;

                // 帳票出力用のExcelワークブックの取得
                outputWorkBook = getOutputWorkBook(application);

                // "単価未入力リスト"のワークシートの検索
                outputWorkSheet = searchSheet(outputWorkBook, UnInputTankaList_SheetName);

                // コピー用にインデックスを退避する
                outputWorkSheetIndex = outputWorkSheet.Index;

                // 帳票出力用のExcelワークブックから
                // "単価未入力リスト"のワークシートを削除する
                application.DisplayAlerts = false;
                deleteWorksheet(outputWorkBook, UnInputTankaList_SheetName);
                application.DisplayAlerts = true;

                //ファイルがなかった場合はそのままコピーする
                copyTargetWorkSheet = outputWorkBook.Worksheets[outputWorkSheetIndex - 1];

                // シートのコピー
                templateWorkSheet.Copy(Missing.Value, copyTargetWorkSheet);

                // 帳票出力用のExcelワークブックの保存
                outputWorkBook.Save();

                templateWorkBook.Close(false);
                outputWorkBook.Close(false);
            }

            catch (Exception)
            {
                throw;
            }

            finally
            {
                if (application != null)
                {
                    application.Quit();
                    Marshal.ReleaseComObject(application);
                    application = null;
                }

                if (templateWorkBook != null)
                {
                    Marshal.ReleaseComObject(templateWorkBook);
                    templateWorkBook = null;
                }

                if (outputWorkBook != null)
                {
                    Marshal.ReleaseComObject(outputWorkBook);
                    outputWorkBook = null;
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

                if (range != null)
                {
                    Marshal.ReleaseComObject(range);
                    range = null;
                }
            }

            return await Task.FromResult(true);
        }
            
        /// <summary>
        /// 使用高払い受払い計算書の出力
        /// </summary>
        /// <param name="list">使用高払い受払い計算書の一覧(Excel出力用)</param>
        /// <returns>非同期タスク(結果)</returns>
        internal async Task<bool> OutputSiyoudakabaraiUkebarai(IList<UkebaraiDTO> list)
        {
            Application application = null;
            Workbook templateWorkBook = null;
            Workbook outputWorkBook = null;
            Worksheet templateWorkSheet = null;
            Worksheet outputWorkSheet = null;
            Worksheet copyTargetWorkSheet = null;
            Range range = null;
            int outputWorkSheetIndex = 0;
            int keyCount = 0;
            int nextline = 0;
            int pageIndex = 1;

            // 罫線
            int startLeftColumnPosition = 1;           // 左上の列番号
            int endRightColumnPosition = 17;           // 右下の列番号

            try
            {
                // ExcelApplicationの取得
                application = getExcelApplication();

                // Excelテンプレート用のExcelワークブックの取得
                templateWorkBook = getExcelTemplateWorkBook(application);

                // "使用高払い受払い計算書"のワークシートの検索
                templateWorkSheet = searchSheet(templateWorkBook, Siyoudakabarai_SheetName);
                templateWorkSheet.Select();

                // 費目でグルーピングする
                var groupingList = list.GroupBy(a => a.Himoku);

                foreach (var oneGroup in groupingList)
                {
                    // キー(費目)が変わった場合、そこで改ページ入れる
                    // 但し、初回(1ページ目)は改ページしない
                    if (keyCount != 0)
                    {
                        // 改ページ挿入
                        insertKaiPage(templateWorkSheet, nextline + 1);
                    }

                    // 1番目   費目
                    // 2番目   費目名
                    // 12番目  年月
                    string himoku = oneGroup.Key;
                    string himokunam = oneGroup.ElementAt(0).HimokuName;
                    string yearmonth = SharedViewModel.Instance.OperationYearMonth.YearMonth.ToString();

                    // 1グループあたりの数
                    int count = oneGroup.Count();
                    // 1ページあたり40行(　1行目(費目)　+　2行目(ヘッダ)　+　38行(データ)　)となる
                    // 必要なページ数を算出
                    int page = count / 38;
                    page += (count % 38) == 0 ? 0 : 1;

                    // ヘッダ行等込々の行数
                    int lineCount = count + (page * 2);

                    var outputData = new object[lineCount, 17];
                    int counter = 0;    // カウンタ(ヘッダ込み)→40でリセット
                    int toosi = 0;      // 通し番号
                    int dataindex = 0;  // データ保持の配列インデックス

                    // データ+ヘッダ行(2行)*ページ数の行数ループ
                    for (int i = 0; i < lineCount; ++i)
                    {
                        // 40でリセット =　ヘッダ挿入
                        if (counter == 40)
                        {
                            counter = 0;
                            // 40行毎に改ページ挿入
                            insertKaiPage(templateWorkSheet, pageIndex * 40 + 1);
                            pageIndex++;
                        }

                        // 先頭行
                        if (counter == 0)
                        {
                            //品目名、日付、年月
                            outputData[toosi, 0] = oneGroup.Key;
                            outputData[toosi, 1] = himokunam;
                            outputData[toosi, 11] = yearmonth;
                        }

                        // 2行目
                        else if (counter == 1)
                        {
                            // ヘッダ行
                            outputData[toosi, 0] = Header_Utiwake;
                            outputData[toosi, 1] = Header_UtiwakeName;
                            outputData[toosi, 2] = Header_Tanaban;
                            outputData[toosi, 3] = Header_HinmokuName;
                            outputData[toosi, 4] = Header_KouzaName;
                            outputData[toosi, 5] = Header_Tani;
                            outputData[toosi, 6] = Header_SZaiko;
                            outputData[toosi, 7] = Header_Nyuko;
                            outputData[toosi, 8] = Header_EFShukko;
                            outputData[toosi, 9] = Header_LFShukko;
                            outputData[toosi, 10] = Header_CCShukko;
                            outputData[toosi, 11] = Header_OtherShukko;
                            outputData[toosi, 12] = Header_BusinessDevelopment;
                            outputData[toosi, 13] = Header_PrimaryCutting;
                            outputData[toosi, 14] = Header_TDShukko;
                            outputData[toosi, 15] = Header_SecondarycCtting;
                            outputData[toosi, 16] = Header_EZaiko;
                        }
                                             
                        else 
                        {
                            // 3行目～40行目までのデータ
                            outputData[toosi, 0] = oneGroup.ElementAt(dataindex).Utiwake;
                            outputData[toosi, 1] = oneGroup.ElementAt(dataindex).UtiwakeName;
                            outputData[toosi, 2] = oneGroup.ElementAt(dataindex).Tanaban;
                            outputData[toosi, 3] = oneGroup.ElementAt(dataindex).HinmokuName;
                            outputData[toosi, 4] = oneGroup.ElementAt(dataindex).KouzaName;
                            outputData[toosi, 5] = oneGroup.ElementAt(dataindex).Tani;
                            outputData[toosi, 6] = oneGroup.ElementAt(dataindex).SZaiko.ToString("#,0");
                            outputData[toosi, 7] = oneGroup.ElementAt(dataindex).Nyuko.ToString("#,0");
                            outputData[toosi, 8] = oneGroup.ElementAt(dataindex).EFShukko.ToString("#,0");
                            outputData[toosi, 9] = oneGroup.ElementAt(dataindex).LFShukko.ToString("#,0");
                            outputData[toosi, 10] = oneGroup.ElementAt(dataindex).CCShukko.ToString("#,0");
                            outputData[toosi, 11] = oneGroup.ElementAt(dataindex).OtherShukko.ToString("#,0");
                            outputData[toosi, 12] = oneGroup.ElementAt(dataindex).BusinessDevelopment.ToString("#,0");
                            outputData[toosi, 13] = oneGroup.ElementAt(dataindex).PrimaryCutting.ToString("#,0");
                            outputData[toosi, 14] = oneGroup.ElementAt(dataindex).TDShukko.ToString("#,0");
                            outputData[toosi, 15] = oneGroup.ElementAt(dataindex).SecondarycCutting.ToString("#,0");
                            outputData[toosi, 16] = oneGroup.ElementAt(dataindex).EZaiko.ToString("#,0");
                            dataindex++;
                        }
                        counter++;
                        toosi++;
                    }

                    // 範囲設定
                    var rangeEnd = toosi + nextline;
                    range = templateWorkSheet.Range[templateWorkSheet.Cells[nextline + 1, startLeftColumnPosition], 
                                                    templateWorkSheet.Cells[rangeEnd, endRightColumnPosition]];
                    //データ挿入
                    range.Value2 = outputData;

                    //次のデータ開始行
                    nextline += lineCount;

                    keyCount++;
                    pageIndex++;
                }

                // 印刷プレビューを表示する(画面前面表示)
                application.WindowState = XlWindowState.xlMaximized;
                WindowOperator.ForegroundWindow(application);
                application.Visible = true;
                templateWorkBook.PrintPreview();
                application.Visible = false;

                // 帳票出力用のExcelワークブックの取得
                outputWorkBook = getOutputWorkBook(application);

                // "使用高払い受払い計算書"のワークシートの検索
                outputWorkSheet = searchSheet(outputWorkBook, Siyoudakabarai_SheetName);

                // コピー用にインデックスを退避する
                outputWorkSheetIndex = outputWorkSheet.Index;

                // 帳票出力用のExcelワークブックから
                // "使用高払い受払い計算書"のワークシートを削除する
                application.DisplayAlerts = false;
                deleteWorksheet(outputWorkBook, Siyoudakabarai_SheetName);
                application.DisplayAlerts = true;

                //ファイルがなかった場合はそのままコピーする
                copyTargetWorkSheet = outputWorkBook.Worksheets[outputWorkSheetIndex - 1];

                // シートのコピー
                templateWorkSheet.Copy(Missing.Value, copyTargetWorkSheet);

                // 帳票出力用のExcelワークブックの保存
                outputWorkBook.Save();

                templateWorkBook.Close(false);
                outputWorkBook.Close(false);
            }

            catch (Exception)
            {
                throw;
            }

            finally
            {
                if (application != null)
                {
                    application.Quit();
                    Marshal.ReleaseComObject(application);
                    application = null;
                }

                if (templateWorkBook != null)
                {
                    Marshal.ReleaseComObject(templateWorkBook);
                    templateWorkBook = null;
                }

                if (outputWorkBook != null)
                {
                    Marshal.ReleaseComObject(outputWorkBook);
                    outputWorkBook = null;
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

                if (range != null)
                {
                    Marshal.ReleaseComObject(range);
                    range = null;
                }
            }

            return await Task.FromResult(true);
        }

        /// <summary>
        /// 入庫払い受払い算書の出力
        /// </summary>
        /// <param name="list">入庫払い受計算書の一覧(Excel出力用)</param>
        /// <returns>非同期タスク(結果)</returns>
        internal async Task<bool> OutputNyukobaraiUkebarai(IList<UkebaraiDTO> list)
        {
            Application application = null;
            Workbook templateWorkBook = null;
            Workbook outputWorkBook = null;
            Worksheet templateWorkSheet = null;
            Worksheet outputWorkSheet = null;
            Worksheet copyTargetWorkSheet = null;
            Range range = null;
            int outputWorkSheetIndex = 0;
            int keyCount = 0;
            var nextline = 0;
            int pageIndex = 1;

            // 罫線
            int startLeftColumnPosition = 1;           // 左上の列番号
            int endRightColumnPosition = 16;           // 右下の列番号

            try
            {
                // ExcelApplicationの取得
                application = getExcelApplication();

                // Excelテンプレート用のExcelワークブックの取得
                templateWorkBook = getExcelTemplateWorkBook(application);

                // "入庫払い受払い算書"のワークシートの検索
                templateWorkSheet = searchSheet(templateWorkBook, Nyukobarai_SheetName);
                templateWorkSheet.Select();

                // 費目でグルーピングする
                var groupedbyHimoku = list.GroupBy(a => a.Himoku);

                foreach (var oneGroup in groupedbyHimoku)
                {
                    // キー(費目)が変わった場合、そこで改ページ入れる
                    // 但し、初回(1ページ目)は改ページしない
                    if (keyCount != 0)
                    {
                        // 改ページの挿入
                        insertKaiPage(templateWorkSheet, nextline + 1);
                    }

                    // 1番目   費目
                    // 2番目   費目名
                    // 12番目  年月
                    string himoku = oneGroup.Key;
                    string himokunam = oneGroup.ElementAt(0).HimokuName;
                    string yearmonth = SharedViewModel.Instance.OperationYearMonth.YearMonth.ToString();

                    // 1グループあたりの数
                    int count = oneGroup.Count();
                    // 1ページあたり40行(　1行目(費目)　+　2行目(ヘッダ)　+　38行(データ)　)となる
                    // 必要なページ数を算出
                    int page = count / 38;
                    page += (count % 38) == 0 ? 0 : 1;

                    // ヘッダ行等込々の行数
                    int lineCount = count + (page * 2);

                    var outputData = new object[lineCount, 16];
                    int counter = 0;    //カウンタ(ヘッダ含む)→40でリセット
                    int toosi = 0;      //通し番号
                    int dataindex = 0;  //データ保持の配列インデックス

                    // データ+ヘッダ行(2行)*ページ数の行数ループ
                    for (int i = 0; i < lineCount; ++i)
                    {
                        // 40でリセット =　ヘッダ挿入
                        if (counter == 40)
                        {
                            counter = 0;
                            // 40行毎に改ページ挿入
                            insertKaiPage(templateWorkSheet, pageIndex * 40 + 1);
                            pageIndex++;
                        }
                        // 1ページの先頭行
                        if (counter == 0)
                        {
                            //品目名、日付、年月
                            outputData[toosi, 0] = oneGroup.Key;
                            outputData[toosi, 1] = himokunam;
                            outputData[toosi, 10] = yearmonth;

                        }
                        // 1ページの2行目
                        else if (1 == counter)
                        {
                            // ヘッダ行
                            outputData[toosi, 0] = Header_Utiwake;
                            outputData[toosi, 1] = Header_UtiwakeName;
                            outputData[toosi, 2] = Header_Tanaban;
                            outputData[toosi, 3] = Header_HinmokuName2;
                            outputData[toosi, 4] = Header_Tani;
                            outputData[toosi, 5] = Header_SZaiko;
                            outputData[toosi, 6] = Header_Nyuko;
                            outputData[toosi, 7] = Header_EFShukko;
                            outputData[toosi, 8] = Header_LFShukko;
                            outputData[toosi, 9] =  Header_CCShukko;
                            outputData[toosi, 10] = Header_OtherShukko;
                            outputData[toosi, 11] = Header_BusinessDevelopment;
                            outputData[toosi, 12] = Header_PrimaryCutting;
                            outputData[toosi, 13] = Header_TDShukko;
                            outputData[toosi, 14] = Header_SecondarycCtting;
                            outputData[toosi, 15] = Header_EZaiko;
                        }
                        else
                        {
                            // 3行目～40行目までのデータ
                            outputData[toosi, 0] = oneGroup.ElementAt(dataindex).Utiwake;
                            outputData[toosi, 1] = oneGroup.ElementAt(dataindex).UtiwakeName;
                            outputData[toosi, 2] = oneGroup.ElementAt(dataindex).Tanaban;
                            outputData[toosi, 3] = oneGroup.ElementAt(dataindex).HinmokuName;
                            outputData[toosi, 4] = oneGroup.ElementAt(dataindex).Tani;
                            outputData[toosi, 5] = oneGroup.ElementAt(dataindex).SZaiko.ToString("#,0");
                            outputData[toosi, 6] = oneGroup.ElementAt(dataindex).Nyuko.ToString("#,0");
                            outputData[toosi, 7] = oneGroup.ElementAt(dataindex).EFShukko.ToString("#,0");
                            outputData[toosi, 8] = oneGroup.ElementAt(dataindex).LFShukko.ToString("#,0");
                            outputData[toosi, 9] = oneGroup.ElementAt(dataindex).CCShukko.ToString("#,0");
                            outputData[toosi, 10] = oneGroup.ElementAt(dataindex).OtherShukko.ToString("#,0");
                            outputData[toosi, 11] = oneGroup.ElementAt(dataindex).BusinessDevelopment.ToString("#,0");
                            outputData[toosi, 12] = oneGroup.ElementAt(dataindex).PrimaryCutting.ToString("#,0");
                            outputData[toosi, 13] = oneGroup.ElementAt(dataindex).TDShukko.ToString("#,0");
                            outputData[toosi, 14] = oneGroup.ElementAt(dataindex).SecondarycCutting.ToString("#,0");
                            outputData[toosi, 15] = oneGroup.ElementAt(dataindex).EZaiko.ToString("#,0");
                            dataindex++;
                        }
                        counter++;
                        toosi++;
                    }
                    // 範囲設定
                    var rangeEnd = toosi + nextline;
                    range = templateWorkSheet.Range[templateWorkSheet.Cells[nextline + 1, startLeftColumnPosition], 
                                                    templateWorkSheet.Cells[rangeEnd, endRightColumnPosition]];

                    //データ挿入
                    range.Value2 = outputData;

                    //次のデータ開始行
                    nextline += lineCount;

                    keyCount++;
                    pageIndex++;
                }

                // 印刷プレビューを表示する(画面前面表示)
                application.WindowState = XlWindowState.xlMaximized;
                WindowOperator.ForegroundWindow(application);
                application.Visible = true;
                templateWorkBook.PrintPreview();
                application.Visible = false;

                // 帳票出力用のExcelワークブックの取得
                outputWorkBook = getOutputWorkBook(application);

                // "入庫払い受払い算書"のワークシートの検索
                outputWorkSheet = searchSheet(outputWorkBook, Nyukobarai_SheetName);

                // コピー用にインデックスを退避する
                outputWorkSheetIndex = outputWorkSheet.Index;

                // 帳票出力用のExcelワークブックから
                // "入庫払い受払い算書"のワークシートを削除する
                application.DisplayAlerts = false;
                deleteWorksheet(outputWorkBook, Nyukobarai_SheetName);
                application.DisplayAlerts = true;

                //ファイルがなかった場合はそのままコピーする
                copyTargetWorkSheet = outputWorkBook.Worksheets[outputWorkSheetIndex - 1];

                // シートのコピー
                templateWorkSheet.Copy(Missing.Value, copyTargetWorkSheet);

                // 帳票出力用のExcelワークブックの保存
                outputWorkBook.Save();

                templateWorkBook.Close(false);
                outputWorkBook.Close(false);
            }

            catch (Exception)
            {
                throw;
            }

            finally
            {
                if (application != null)
                {
                    application.Quit();
                    Marshal.ReleaseComObject(application);
                    application = null;
                }

                if (templateWorkBook != null)
                {
                    Marshal.ReleaseComObject(templateWorkBook);
                    templateWorkBook = null;
                }

                if (outputWorkBook != null)
                {
                    Marshal.ReleaseComObject(outputWorkBook);
                    outputWorkBook = null;
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

                if (range != null)
                {
                    Marshal.ReleaseComObject(range);
                    range = null;
                }

            }

            return await Task.FromResult(true);
        }

        /// <summary>
        /// 立会い用棚卸表の出力
        /// </summary>
        /// <param name="list">立会い用棚卸表の一覧(Excel出力用)</param>
        /// <returns>非同期タスク(結果)</returns>
        internal async Task<bool> OutputWitnessInventoryList(IList<InventoryWitnessDTO> list)
        {
            Application application = null;
            Workbook templateWorkBook = null;
            Workbook outputWorkBook = null;
            Worksheet templateWorkSheet = null;
            Worksheet outputWorkSheet = null;
            Worksheet copyTargetWorkSheet = null;
            Range range = null;
            int outputWorkSheetIndex = 0;
            int listCount = list.Count;

            // 罫線
            int startLeftRowPosition = 4;              // 左上の行番号
            int startLeftColumnPosition = 1;           // 左上の列番号
            int endRightRowPosition = listCount + 3;   // 右下の行番号
            int endRightColumnPosition = 10;           // 右下の列番号

            try
            {
                // ExcelApplicationの取得
                application = getExcelApplication();

                // Excelテンプレート用のExcelワークブックの取得
                templateWorkBook = getExcelTemplateWorkBook(application);

                // "立会い用棚卸表"のワークシートの検索
                templateWorkSheet = searchSheet(templateWorkBook, WitnessInventory_SheetName);
                templateWorkSheet.Select();

                // 印刷範囲の設定
                templateWorkSheet.PageSetup.PrintArea = $@"$A$1:$J${endRightRowPosition}";
                // 罫線および値を出力するレンジの設定
                range = templateWorkSheet
                           .Range[templateWorkSheet.Cells[startLeftRowPosition, startLeftColumnPosition],
                                  templateWorkSheet.Cells[endRightRowPosition, endRightColumnPosition]];

                int month = SharedViewModel.Instance.OperationYearMonth.Month;
                int term = SharedViewModel.Instance.OperationYearMonth.Term;

                string convertTerm = ((int)KaikeiKikan.KamikiStartMonth <= month
                                        && month < (int)KaikeiKikan.SimokiStartMonth ? Kamiki_StringDefine : Simoki_StringDefine);

                string title = term.ToString() + convertTerm + WitnessInventory_Title;
                ((Range)templateWorkSheet.Cells[1, 1]).Value2 = title;

                var outputData = new object[listCount, 10];
                for (int i = 0; i < listCount; ++i)
                {
                    if (list[i].HinmokuCD == "????")
                    {
                        outputData[i, 6] = list[i].ZSK;
                        outputData[i, 8] = list[i].GenbaSuryo;
                        outputData[i, 9] = list[i].Total;
                    }
                    else
                    {
                        outputData[i, 0] = list[i].HinmokuCD;
                        outputData[i, 1] = list[i].Himoku;
                        outputData[i, 2] = list[i].Utiwake;
                        outputData[i, 3] = list[i].Tanaban;
                        outputData[i, 4] = list[i].HinmokuName;
                        outputData[i, 5] = list[i].Tani;
                        outputData[i, 6] = list[i].ZSK;
                        outputData[i, 8] = list[i].GenbaSuryo;
                        outputData[i, 9] = list[i].Total;
                    }
                }
                range.Value2 = outputData;

                // 印刷プレビューを表示する(画面前面表示)
                application.WindowState = XlWindowState.xlMaximized;
                WindowOperator.ForegroundWindow(application);
                application.Visible = true;
                templateWorkBook.PrintPreview();
                application.Visible = false;

                // 帳票出力用のExcelワークブックの取得
                outputWorkBook = getOutputWorkBook(application);

                // "立会い用棚卸表"のワークシートの検索
                outputWorkSheet = searchSheet(outputWorkBook, WitnessInventory_SheetName);

                // コピー用にインデックスを退避する
                outputWorkSheetIndex = outputWorkSheet.Index;

                // 帳票出力用のExcelワークブックから
                // "立会い用棚卸表"のワークシートを削除する
                application.DisplayAlerts = false;
                deleteWorksheet(outputWorkBook, WitnessInventory_SheetName);
                application.DisplayAlerts = true;

                //ファイルがなかった場合はそのままコピーする
                copyTargetWorkSheet = outputWorkBook.Worksheets[outputWorkSheetIndex - 1];

                // シートのコピー
                templateWorkSheet.Copy(Missing.Value, copyTargetWorkSheet);

                // 帳票出力用のExcelワークブックの保存
                outputWorkBook.Save();

                templateWorkBook.Close(false);
                outputWorkBook.Close(false);
            }

            catch (Exception)
            {
                throw;
            }

            finally
            {
                if (application != null)
                {
                    application.Quit();
                    Marshal.ReleaseComObject(application);
                    application = null;
                }

                if (templateWorkBook != null)
                {
                    Marshal.ReleaseComObject(templateWorkBook);
                    templateWorkBook = null;
                }

                if (outputWorkBook != null)
                {
                    Marshal.ReleaseComObject(outputWorkBook);
                    outputWorkBook = null;
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

                if (range != null)
                {
                    Marshal.ReleaseComObject(range);
                    range = null;
                }

            }

            return await Task.FromResult(true);
        }

        /// <summary>
        /// 財務提出用棚卸表の出力
        /// </summary>
        /// <param name="list">財務提出用棚卸表の一覧(Excel出力用)</param>
        /// <returns>非同期タスク(結果)</returns>
        internal async Task<bool> OutputFinancingPresetationInventoryList
                                    (IList<InventoryFinancingPresetationDTO> list)
        {
            Application application = null;
            Workbook templateWorkBook = null;
            Workbook outputWorkBook = null;
            Worksheet templateWorkSheet = null;
            Worksheet outputWorkSheet = null;
            Worksheet copyTargetWorkSheet = null;
            Range range = null;
            Range otherRange = null;
            int outputWorkSheetIndex = 0;
            int listCount = list.Count;

            // 罫線
            int startLeftRowPosition = 5;              // 左上の行番号
            int startLeftColumnPosition = 1;           // 左上の列番号
            int endRightRowPosition = listCount + 4;   // 右下の行番号
            int endRightColumnPosition = 14;           // 右下の列番号

            try
            {
                // ExcelApplicationの取得
                application = getExcelApplication();

                // Excelテンプレート用のExcelワークブックの取得
                templateWorkBook = getExcelTemplateWorkBook(application);

                // "財務提出用棚卸表"のワークシートの検索
                templateWorkSheet = searchSheet(templateWorkBook, FinancingPresetationInventory_SheetName);
                templateWorkSheet.Select();

                // 印刷範囲の設定
                templateWorkSheet.PageSetup.PrintArea = $@"$A$1:$N${endRightRowPosition}";
                // 罫線および値を出力するレンジの設定
                range = templateWorkSheet
                           .Range[templateWorkSheet.Cells[startLeftRowPosition, startLeftColumnPosition],
                                  templateWorkSheet.Cells[endRightRowPosition, endRightColumnPosition]];

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

                otherRange = templateWorkSheet
                                 .Range[templateWorkSheet.Cells[5, 5], 
                                        templateWorkSheet.Cells[endRightRowPosition, 6]];

                otherRange.Borders[XlBordersIndex.xlInsideVertical].LineStyle = XlLineStyle.xlDouble;
                otherRange.Borders[XlBordersIndex.xlInsideVertical].ColorIndex = 1;

                int month = SharedViewModel.Instance.OperationYearMonth.Month;
                int term = SharedViewModel.Instance.OperationYearMonth.Term;

                string convertTerm = ((int)KaikeiKikan.KamikiStartMonth <= month
                                        && month < (int)KaikeiKikan.SimokiStartMonth ? Kamiki_StringDefine : Simoki_StringDefine);

                string title = term.ToString() + convertTerm + FinancingPresetationInventory_Title;
                ((Range)templateWorkSheet.Cells[2, 1]).Value2 = title;

                var outputData = new object[listCount, 14];
                for (int i = 0; i < listCount; ++i)
                {
                    outputData[i, 0] = list[i].Himoku;
                    outputData[i, 1] = list[i].Utiwake;
                    outputData[i, 2] = list[i].Tanaban;
                    outputData[i, 3] = list[i].HinmokuName;
                    outputData[i, 4] = list[i].Tani;
                    outputData[i, 5] = list[i].Kimatusuryo;
                    outputData[i, 8] = list[i].Kimatusuryo;
                }
                range.Value2 = outputData;

                // 印刷プレビューを表示する(画面前面表示)
                application.WindowState = XlWindowState.xlMaximized;
                WindowOperator.ForegroundWindow(application);
                application.Visible = true;
                templateWorkBook.PrintPreview();
                application.Visible = false;

                // 帳票出力用のExcelワークブックの取得
                outputWorkBook = getOutputWorkBook(application);

                // "財務提出用棚卸表"のワークシートの検索
                outputWorkSheet = searchSheet(outputWorkBook, FinancingPresetationInventory_SheetName);

                // コピー用にインデックスを退避する
                outputWorkSheetIndex = outputWorkSheet.Index;

                // 帳票出力用のExcelワークブックから
                // "財務提出用棚卸表"のワークシートを削除する
                application.DisplayAlerts = false;
                deleteWorksheet(outputWorkBook, FinancingPresetationInventory_SheetName);
                application.DisplayAlerts = true;

                //ファイルがなかった場合はそのままコピーする
                copyTargetWorkSheet = outputWorkBook.Worksheets[outputWorkSheetIndex - 1];

                // シートのコピー
                templateWorkSheet.Copy(Missing.Value, copyTargetWorkSheet);

                // 帳票出力用のExcelワークブックの保存
                outputWorkBook.Save();

                templateWorkBook.Close(false);
                outputWorkBook.Close(false);
            }

            catch (Exception)
            {
                throw;
            }

            finally
            {
                if (application != null)
                {
                    application.Quit();
                    Marshal.ReleaseComObject(application);
                    application = null;
                }

                if (templateWorkBook != null)
                {
                    Marshal.ReleaseComObject(templateWorkBook);
                    templateWorkBook = null;
                }

                if (outputWorkBook != null)
                {
                    Marshal.ReleaseComObject(outputWorkBook);
                    outputWorkBook = null;
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

                if (range != null)
                {
                    Marshal.ReleaseComObject(range);
                    range = null;
                }

                if (otherRange != null)
                {
                    Marshal.ReleaseComObject(otherRange);
                    otherRange = null;
                }

            }

            return await Task.FromResult(true);
        }

        /// <summary>
        /// 資材品目マスタの変更履歴の出力
        /// </summary>
        /// <param name="list">資材品目マスタの変更履歴の一覧(Excel出力用)</param>
        /// <returns>非同期タスク(結果)</returns>
        internal async Task<bool> OutputSizaiHinmokuChangeList(IList<SizaiHinmokuDTO> list)
        {
            Application application = null;
            Workbook templateWorkBook = null;
            Workbook outputWorkBook = null;
            Worksheet templateWorkSheet = null;
            Worksheet outputWorkSheet = null;
            Worksheet copyTargetWorkSheet = null;
            Range range = null;
            int outputWorkSheetIndex = 0;
            int listCount = list.Count;

            // 罫線
            int startLeftRowPosition = 3;            // 左上の行番号
            int startLeftColumnPosition = 1;         // 左上の列番号
            int endRightRowPosition = listCount + 2; // 右下の行番号
            int endRightColumnPosition = 14;     　　// 右下の列番号

            try
            {
                // ExcelApplicationの取得
                application = getExcelApplication();

                // Excelテンプレート用のExcelワークブックの取得
                templateWorkBook = getExcelTemplateWorkBook(application);

                // "資材品目マスタ変更利履歴"のワークシートの検索
                templateWorkSheet = searchSheet(templateWorkBook, SizaiHinmoku_ChangeList_SheetName);
                templateWorkSheet.Select();

                // 印刷範囲の設定
                templateWorkSheet.PageSetup.PrintArea = $@"$A$1:$N${endRightRowPosition}";
                // 罫線および値を出力するレンジの設定
                range = templateWorkSheet
                           .Range[templateWorkSheet.Cells[startLeftRowPosition, startLeftColumnPosition],
                                  templateWorkSheet.Cells[endRightRowPosition, endRightColumnPosition]];

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
                    // 品目コード
                    outputData[i, 0] = list[i].HinmokuCode;
                    // 品目名
                    outputData[i, 1] = list[i].HinmokuName;
                    // 種別
                    outputData[i, 2] = list[i].UkebaraiType.Trim()
                        .Equals(NyukoBarai) ? Nyukobarai_StringDefine : Siyoudakabarai_StringDefine;
                    // 費目
                    outputData[i, 3] = list[i].Himoku;
                    // 内訳
                    outputData[i, 4] = list[i].Utiwake;
                    // 棚番
                    outputData[i, 5] = list[i].Tanaban;
                    // 単位
                    outputData[i, 6] = list[i].Tani;
                    // 水分引き
                    outputData[i, 7] = list[i].SuibunKbn.Trim().Equals(Do_Action) ?
                        Action_Value : Not_Action_Value;
                    // 検収明細出力
                    outputData[i, 8] = list[i].KensyuKbn.Trim().Equals(Do_Action) ?
                        Action_Value : Not_Action_Value;
                    // 経理報告
                    outputData[i, 9] = list[i].HoukokuKbn.Trim().Equals(Do_Action) ?
                        Action_Value : Not_Action_Value;
                    // 向先
                    outputData[i, 10] = SharedViewModel.Instance.Mukesaki[list[i].Mukesaki];
                    // 出庫位置
                    outputData[i, 11] = SharedViewModel.Instance.Mukesaki[list[i].IchiKbn];
                    // 単価設定
                    outputData[i, 12] = list[i].TankaSetting;
                    // 更新日
                    outputData[i, 13] = list[i].UpdYMD;
                }
                range.Value2 = outputData;

                // 印刷プレビューを表示する(画面前面表示)
                application.WindowState = XlWindowState.xlMaximized;
                WindowOperator.ForegroundWindow(application);
                application.Visible = true;
                templateWorkBook.PrintPreview();
                application.Visible = false;

                // 帳票出力用のExcelワークブックの取得
                outputWorkBook = getOutputWorkBook(application);

                // "資材品目マスタ変更利履歴"のワークシートの検索
                outputWorkSheet = searchSheet(outputWorkBook, SizaiHinmoku_ChangeList_SheetName);

                // コピー用にインデックスを退避する
                outputWorkSheetIndex = outputWorkSheet.Index;

                // 帳票出力用のExcelワークブックから
                // "資材品目マスタ変更利履歴"のワークシートを削除する
                application.DisplayAlerts = false;
                deleteWorksheet(outputWorkBook, SizaiHinmoku_ChangeList_SheetName);
                application.DisplayAlerts = true;

                //ファイルがなかった場合はそのままコピーする
                copyTargetWorkSheet = outputWorkBook.Worksheets[outputWorkSheetIndex - 1];

                // シートのコピー
                templateWorkSheet.Copy(Missing.Value, copyTargetWorkSheet);

                // 帳票出力用のExcelワークブックの保存
                outputWorkBook.Save();

                templateWorkBook.Close(false);
                outputWorkBook.Close(false);
            }

            catch (Exception)
            {
                throw;
            }

            finally
            {
                if (application != null)
                {
                    application.Quit();
                    Marshal.ReleaseComObject(application);
                    application = null;
                }

                if (templateWorkBook != null)
                {
                    Marshal.ReleaseComObject(templateWorkBook);
                    templateWorkBook = null;
                }

                if (outputWorkBook != null)
                {
                    Marshal.ReleaseComObject(outputWorkBook);
                    outputWorkBook = null;
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

                if (range != null)
                {
                    Marshal.ReleaseComObject(range);
                    range = null;
                }

            }

            return await Task.FromResult(true);
        }

        /// <summary>
        /// 印刷対象 →　文字列変換
        /// </summary>
        /// <param name="target">取得対象のキー</param>
        /// <returns>テンプレートのシート名、コピー先のシート名１、コピー先のシート名２</returns>
        private Tuple<string, string, string> convertIndex2String(InventoryPrintTieSizaiCategory target)
        {
            Tuple<string, string, string> ret = null;

            try
            {
                // 定義ファイルのオープン
                var settings = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, "Settings.xml");
                var doc = XDocument.Load(settings);

                var element = doc.XPathSelectElement($"/Settings/Excel/SheetNames/SheetName[@Key='{target.ToString()}']");

                var templateSheetName = element.Attribute("Original").Value;
                var sheet1Name = templateSheetName;
                var sheet2Name = element.Attribute("IncludeZero").Value;
                ret = new Tuple<string, string, string>(templateSheetName, sheet1Name, sheet2Name);
            }

            catch
            {
                throw;
            }

            return ret;
        }

        /// <summary>
        /// ワークシートをコピーする
        /// </summary>
        /// <param name="copyFromWorkbook">コピー元ワークブック</param>
        /// <param name="copyToWorkbook">コピー先ワークブック</param>
        /// <param name="copyFromSheetName">コピー元シート名</param>
        /// <param name="copyToSheetName">コピー先シート名</param>
        /// <param name="copyToSheetIndex">コピー先のインデックス</param>
        /// <returns>新しく作成したワークシート</returns>
        private Worksheet copyWorksheet(Workbook copyFromWorkbook, Workbook copyToWorkbook, 
                                            string copyFromSheetName, string copyToSheetName, int copyToSheetIndex)
        {
            Worksheet copyFromWorksheet = null;
            Worksheet copyToWorksheet = null;

            try
            {
                // コピー元のシート・コピー先の一番最後のシートを取得
                copyFromWorksheet = searchSheet(copyFromWorkbook, copyFromSheetName);
                copyToWorksheet = copyToWorkbook.Worksheets[copyToSheetIndex];

                // シートをコピー先の一番最後にコピーする
                copyFromWorksheet.Copy(Before: copyToWorksheet);

                // コピーしたシートの名称を変更
                var targetWorksheet = copyToWorkbook.ActiveSheet as Worksheet;
                targetWorksheet.Name = copyToSheetName;
                return targetWorksheet;
            }

            catch (Exception)
            {
                throw;
            }

            finally
            {
                if (copyFromWorksheet != null)
                {
                    Marshal.ReleaseComObject(copyFromWorksheet);
                    copyFromWorksheet = null;
                }
                if (copyToWorksheet != null)
                {
                    Marshal.ReleaseComObject(copyToWorksheet);
                    copyToWorksheet = null;
                }
            }
        }

        /// <summary>
        /// 印刷データをワークシートに出力する
        /// </summary>
        /// <param name="workSheet">出力シート</param>
        /// <param name="printData">出力データ</param>
        /// <param name="isTogetuClear">当月クリアかどうか</param>
        private void createWorksheet(Worksheet workSheet, PrintRecordViewModel[] printData, bool isTogetuClear)
        {
            Range range = null;
            Range rangeGray = null;
            int count = printData.Length;

            // 罫線
            int startLeftRowPosition = 3;            // 左上の行番号
            int startLeftColumnPosition = 1;         // 左上の列番号
            int endRightRowPosition = count + 2;     // 右下の行番号
            int endRightColumnPosition = 14;         // 右下の列番号

            try
            {
                // 罫線および値を出力するレンジの設定
                range = workSheet
                            .Range[workSheet.Cells[startLeftRowPosition, startLeftColumnPosition],
                                    workSheet.Cells[endRightRowPosition, endRightColumnPosition]];

                range.Borders[XlBordersIndex.xlInsideHorizontal].LineStyle = XlLineStyle.xlContinuous;

                range.Borders[XlBordersIndex.xlInsideHorizontal].ColorIndex = 48;

                range.Borders[XlBordersIndex.xlInsideVertical].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlInsideVertical].ColorIndex = 48;

                range.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlDouble;
                range.Borders[XlBordersIndex.xlEdgeTop].ColorIndex = 1;

                range.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlEdgeBottom].ColorIndex = 1;
                range.Borders[XlBordersIndex.xlEdgeBottom].Weight = XlBorderWeight.xlMedium;

                range.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlEdgeLeft].ColorIndex = 1;
                range.Borders[XlBordersIndex.xlEdgeLeft].Weight = XlBorderWeight.xlMedium;

                range.Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlEdgeRight].ColorIndex = 1;
                range.Borders[XlBordersIndex.xlEdgeRight].Weight = XlBorderWeight.xlMedium;

                // 種別が1(入庫払い)の場合の灰色設定
                for (int loop = 0; loop < count; loop++)
                {
                    if ((printData[loop].Syubetu == NyukoBarai))
                    {
                        rangeGray = workSheet.Range[workSheet.Cells[loop + 3, 1], workSheet.Cells[loop + 3, 3]];
                        rangeGray.Interior.ColorIndex = 15;
                        rangeGray = workSheet.Range[workSheet.Cells[loop + 3, 7], workSheet.Cells[loop + 3, 13]];
                        rangeGray.Interior.ColorIndex = 15;
                    }
                }

                string format = InventoryPrint_Title_Format;
                string first = ((int)KaikeiKikan.KamikiStartMonth <= SharedViewModel.Instance.OperationYearMonth.Month
                                    && SharedViewModel.Instance.OperationYearMonth.Month < (int)KaikeiKikan.SimokiStartMonth ? Kamiki_StringDefine : Simoki_StringDefine);

                ((Range)workSheet.Cells[1, 2]).Value2
                    = string.Format(format,
                        SharedViewModel.Instance.OperationYearMonth.Year,
                        SharedViewModel.Instance.OperationYearMonth.Month,
                        SharedViewModel.Instance.OperationYearMonth.Term,
                        first);

                object[,] outputData = new object[count, 14];

                for (int i = 0; i < count; ++i)
                {
                    outputData[i, 0] = printData[i].HinmokuCD;
                    outputData[i, 1] = printData[i].GyosyaCD.TrimEnd();
                    outputData[i, 2] = printData[i].HinmokuName;
                    outputData[i, 3] = printData[i].GyosyaName.TrimEnd();
                    outputData[i, 4] = printData[i].Bikou.TrimEnd();
                    outputData[i, 5] = (isTogetuClear) ? string.Empty : printData[i].Togetsuryo;
                    outputData[i, 6] = printData[i].NyukoRyo;
                    outputData[i, 7] = printData[i].Henpin;
                    outputData[i, 8] = printData[i].LastMonth;
                    outputData[i, 9] = printData[i].SokoZaiko;
                    outputData[i, 10] = printData[i].EfZaiko;
                    outputData[i, 11] = printData[i].LfZaiko;
                    outputData[i, 12] = printData[i].CcZaiko;
                    outputData[i, 13] = string.Empty;
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

                if (rangeGray != null)
                {
                    Marshal.ReleaseComObject(rangeGray);
                    rangeGray = null;
                }
            }
        }

        /// <summary>
        /// 改ページの挿入
        /// </summary>
        /// <param name="wSheet">挿入対象のシート</param>
        /// <param name="end">挿入位置</param>
        private void insertKaiPage(Worksheet wSheet, int end)
        {
            Range range = null;

            try
            {
                //挿入位置の設定
                range = wSheet.Range[$"P{(end)}"];

                //改ページ設定
                wSheet.HPageBreaks.Add(range);
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

        #endregion
    }
}

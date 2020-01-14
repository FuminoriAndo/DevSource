using CKSI1090.Common.Excel.DTO;
using CKSI1090.Shared;
using Common;
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
//*************************************************************************************
//
//   Excelアクセッサー
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   17.04.28              DSK   　     新規作成
//
//*************************************************************************************
namespace CKSI1090.Common.Excel
{
    /// <summary>
    /// Excelアクセッサー
    /// </summary>
    internal class ExcelAccessor
    {
        #region フィールド

        /// <summary>
        /// 済
        /// </summary>
        private readonly string Approval_StringDefine = "済";

        /// <summary>
        /// 未
        /// </summary>
        private readonly string UnApproval_StringDefine = "未";

        /// <summary>
        /// 作業誌チェックリストのシート名
        /// </summary>
        private readonly string SagyosiCheckList_SheetName = "作業誌チェックリスト";

        /// <summary>
        /// テンプレートのシート名
        /// </summary>
        private readonly string Template_SheetName = "雛形";

        /// <summary>
        /// 改ページの制限値
        /// </summary>
        private readonly int InsertPageLimitValue = 1023;

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
        private readonly string excelTemplateBookPath = null;

        /// <summary>
        /// 作業誌チェックリストのパス
        /// </summary>
        private readonly string sagyosiCheckListPath = null;

        /// <summary>
        /// 作業誌チェックリストのフォルダ
        /// </summary>
        private readonly string sagyosiCheckListFolder = null;

        /// <summary>
        /// 作業誌チェックリストのブック名
        /// </summary>
        private readonly string sagyosiCheckListBookName = null;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        private ExcelAccessor()
        {
            try
            {
                // XMLファイルのパスの取得
                string xmlPath = Path.Combine(Path.GetDirectoryName(
                Assembly.GetExecutingAssembly().Location) ?? string.Empty, "WorkNoteCheckListSettings.xml");

                // XMLファイルを読込む
                xDocument = XDocument.Load(xmlPath);

                // 帳票出力用のExcelテンプレートブックのファイルパスの取得
                excelTemplateBookPath = getExcelTemplatBookPath();

                // 作業誌チェックリストのパスの取得
                sagyosiCheckListPath = getSagyosiCheckListPath();

                // 作業誌チェックリストのフォルダの取得
                sagyosiCheckListFolder = getSagyosiCheckListFolder();

                // 作業誌チェックリストのブック名の取得
                sagyosiCheckListBookName = getSagyosiCheckListBookName();
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
        /// 作業誌チェックリスト出力用のExcelWorkbookの取得
        /// </summary>
        /// <param name="application">ExcelApplication</param>
        /// <returns>作業誌チェックリスト出力用のExcelWorkbook</returns>
        private Workbook getSagyosiCheckListWorkBook(Application application)
        {
            Workbook book = null;

            try
            {
                if (!Directory.Exists(sagyosiCheckListFolder))
                { 
                    Directory.CreateDirectory(sagyosiCheckListFolder);
                }

                // 作業誌チェックリストのExcelブックが既に存在するか
                string[] fileList 
                    = Directory.GetFileSystemEntries(sagyosiCheckListFolder, sagyosiCheckListBookName);

                if (fileList.Count() > 0)
                {
                    File.Delete(sagyosiCheckListFolder + sagyosiCheckListBookName);
                }

                File.Copy(excelTemplateBookPath, sagyosiCheckListPath);

                // ExcelWorkBookを取得する
                book = getWorkBook(application, sagyosiCheckListPath);
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
        /// 作業誌チェックリストのパスの取得
        /// </summary>
        /// <returns>作業誌チェックリストのパス</returns>
        private string getSagyosiCheckListPath()
        {
            string filePath = string.Empty;
            string fileName = string.Empty;

            try
            {
                var elem = xDocument.XPathSelectElement($"/Settings/Excel/Output");
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
        /// 作業誌チェックリストのフォルダの取得
        /// </summary>
        /// <returns>作業誌チェックリストのフォルダ</returns>
        private string getSagyosiCheckListFolder()
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
        /// 作業誌チェックリストのブック名の取得
        /// </summary>
        /// <returns>作業誌チェックリストのブック名の取得</returns>
        private string getSagyosiCheckListBookName()
        {
            string name = string.Empty;

            try
            {
                var elem = xDocument.XPathSelectElement($"/Settings/Excel/Output");
                name = (string)elem.Attribute("Name");

                return name;
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
        /// 作業誌チェックリストの出力
        /// </summary>
        /// <param name="list">Excel出力用の作業誌チェックリストの一覧</param>
        /// <returns>非同期タスク(結果)</returns>
        internal async Task<bool> OutputSagyosiCheckList(IList<WorkNoteCheckListDTO> list)
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
            int keyCounter = 0;

            // 罫線
            int startLeftRowPosition = 3;        // 左上の行番号
            int startLeftColumnPosition = 1;     // 左上の列番号
            int endRightRowPosition = listCount + 2; // 右下の行番号
            int endRightColumnPosition = 12;     // 右下の列番号

            try
            {
                // ExcelApplicationの取得
                application = getExcelApplication();

                // Excelテンプレート用のExcelワークブックの取得
                templateWorkBook = getExcelTemplateWorkBook(application);

                // "作業誌チェックリスト"のワークシートの検索
                templateWorkSheet = searchSheet(templateWorkBook, SagyosiCheckList_SheetName);
                templateWorkSheet.Select();

                // 印刷範囲の設定
                templateWorkSheet.PageSetup.PrintArea = $@"$A$1:$L${endRightRowPosition}";
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

                var outputData = new object[listCount, 12];
                for (int i = 0; i < listCount; ++i)
                {
                    // 作業日
                    outputData[i, 0] = list[i].IsChecked ? Approval_StringDefine : UnApproval_StringDefine;

                    // 作業日
                    outputData[i, 1] = list[i].OperationDate.Substring(0, 4) + "/" +
                                       list[i].OperationDate.Substring(4, 2) + "/" +
                                       list[i].OperationDate.Substring(6, 2);
                    // 作業誌区分
                    outputData[i, 2] = SharedViewModel.Instance.WorkNoteKbn[list[i].WorkNoteType];
                    // 品目コード
                    outputData[i, 3] = list[i].HinmokuCode;
                    // 業者コード
                    outputData[i, 4] = list[i].GyosyaCode;
                    // 向先
                    outputData[i, 5] = SharedViewModel.Instance.Mukesaki.ContainsKey(list[i].Mukesaki) ?
                        SharedViewModel.Instance.Mukesaki[list[i].Mukesaki] : list[i].Mukesaki;
                    // 品目名
                    outputData[i, 6] = list[i].HinmokuName;
                    // 業者名
                    outputData[i, 7] = list[i].GyosyaName;
                    // 数量
                    outputData[i, 8] = list[i].Amount;
                    // 水分率
                    outputData[i, 9] = list[i].Suibunritu;
                    // 確認
                    outputData[i, 10] = list[i].ConfirmOne;
                    // 確認
                    outputData[i, 11] = list[i].ConfirmTwo;

                }
                range.Value2 = outputData;

                // 作業日でグルーピングする
                var groupedbyOperationDate = list.GroupBy(a => a.OperationDate);
                int oneGroupCount = 0;
                int totalCount = 3;

                foreach (var oneGroup in groupedbyOperationDate)
                {
                    oneGroupCount = oneGroup.Count();
                    totalCount += oneGroupCount;

                    // 改ページの挿入
                    insertKaiPage(templateWorkSheet, totalCount);
                    keyCounter++;
                }

                // 印刷プレビューを表示する(画面前面表示)
                application.WindowState = XlWindowState.xlMaximized;
                WindowOperator.ForegroundWindow(application);
                application.Visible = true;
                templateWorkBook.PrintPreview();
                application.Visible = false;

                // 作業誌チェックリスト用のExcelワークブックの取得
                outputWorkBook = getSagyosiCheckListWorkBook(application);

                // "作業誌チェックリスト"のワークシートの検索
                outputWorkSheet = searchSheet(outputWorkBook, SagyosiCheckList_SheetName);

                // コピー用にインデックスを退避する
                outputWorkSheetIndex = outputWorkSheet.Index;

                // 作業誌チェックリスト用のExcelワークブックから
                // "作業誌チェックリスト"のワークシートを削除する
                application.DisplayAlerts = false;
                deleteWorksheet(outputWorkBook, SagyosiCheckList_SheetName);
                application.DisplayAlerts = true;

                //ファイルがなかった場合はそのままコピーする
                copyTargetWorkSheet = outputWorkBook.Worksheets[outputWorkSheetIndex - 1];

                // シートのコピー
                templateWorkSheet.Copy(Missing.Value, copyTargetWorkSheet);

                // 作業誌チェックリスト用のExcelワークブックから
                // "雛形"のワークシートを削除する
                application.DisplayAlerts = false;
                deleteWorksheet(outputWorkBook, Template_SheetName);
                application.DisplayAlerts = true;

                // 作業誌チェックリスト用のExcelワークブックの保存
                outputWorkBook.Save();

            }

            catch (Exception)
            {
                if (keyCounter == InsertPageLimitValue)
                {
                    throw new ExcelInsertPageLimitOverException();
                }

                else
                {
                    throw;

                }
            }

            finally
            {
                if (templateWorkBook != null)
                {
                    templateWorkBook.Close(false);
                }

                if (outputWorkBook != null)
                {
                    outputWorkBook.Close(false);
                }

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
                range = wSheet.Range[$"L{(end)}"];

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

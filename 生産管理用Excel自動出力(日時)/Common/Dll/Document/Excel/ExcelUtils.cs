//*************************************************************************************
//
//   Excelユーティリティ
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;

namespace Document.Excel
{
    /// <summary>
    /// Excelユーティリティ
    /// </summary>
    public class ExcelUtils
    {
        /// <summary>
        /// 最終行セルの行の取得
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        /// <param name="sheetName">シート名</param>
        /// <returns>最終行セルの行</returns>
        public static int GetLastCellRow(string filePath,string sheetName)
        {
            Application app = null;
            Workbook workBook = null;
            Worksheet workSheet = null;

            try
            {
                app = new Application();
                app.Visible = false;
                // ワークブックの取得
                workBook = GetWorkBook(app, filePath);
                // ワークシートの検索
                workSheet = SearchSheet(workBook, sheetName);
                // 最終行セルの行の取得
                return workSheet.Cells.SpecialCells(XlCellType.xlCellTypeLastCell, Type.Missing).Row;

            }

            catch (Exception)
            {
                throw;
            }

            finally
            {
                if (workBook != null)
                {
                    workBook.Close(false, false, false);
                }

                if (app != null)
                {
                    app.Quit();
                    Marshal.ReleaseComObject(app);
                    app = null;
                }

                if (workBook != null)
                {
                    Marshal.ReleaseComObject(workBook);
                    workBook = null;
                }

                if (workSheet != null)
                {
                    Marshal.ReleaseComObject(workSheet);
                    workSheet = null;
                }

            }
        }


        /// <summary>
        /// ExcelApplicationの取得
        /// </summary>
        /// <returns>ExcelApplicationオブジェクト</returns>
        public static Application GetExcelApplication()
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
        /// <returns>ExcelWorkbookオブジェクト</returns>
        public static Workbook GetWorkBook(Application application, string filePath)
        {
            Workbook book = null;

            try
            {
                book = application.Workbooks.Open(filePath,
                                                  Type.Missing, Type.Missing, Type.Missing,
                                                  Type.Missing, Type.Missing, Type.Missing,
                                                  Type.Missing, Type.Missing, Type.Missing,
                                                  Type.Missing, Type.Missing, Type.Missing,
                                                  Type.Missing, Type.Missing);
            }

            catch (Exception)
            {
                throw;
            }

            return book;
        }

        /// <summary>
        /// テンプレート用のExcelWorkbookの取得
        /// </summary>
        /// <param name="application">ExcelApplication</param>
        /// <param name="templateBookPath">テンプレート用ブックのパス</param>
        /// <returns>テンプレート用のExcelWorkbook</returns>
        public static Workbook GetExcelTemplateWorkBook(Application application, string templateBookPath)
        {
            Workbook book = null;

            try
            {
                // ExcelWorkBookを取得する
                book = GetWorkBook(application, templateBookPath);
            }

            catch (Exception)
            {
                throw;
            }

            return book;
        }

        /// <summary>
        /// 出力用のExcelWorkbookの取得
        /// </summary>
        /// <param name="application">ExcelApplication</param>
        /// <param name="outputBookFolder">出力ブックのフォルダ</param>
        /// <param name="outputBookName">出力ブックの名前</param>
        /// <param name="templateBookPath">テンプレート用ブックのパス</param>
        /// <param name="createIfNotExists">存在しない場合、ワークブックを作成するか</param>
        /// <param name="deleteIfExists">存在する場合、既存のワークブックを削除するか</param>
        /// <returns>出力用のExcelWorkbook</returns>
        public static Workbook GetExcelOutputWorkBook(Application application,
                                                      string outputBookFolder,
                                                      string outputBookName,
                                                      string templateBookPath,
                                                      bool createIfNotExists, bool deleteIfExists)
        {
            Workbook book = null;

            try
            {
                // 出力ブックのフォルダが存在するか
                if (!Directory.Exists(outputBookFolder))
                {
                    if (createIfNotExists)
                        // 出力ブックのフォルダを作成する
                        Directory.CreateDirectory(outputBookFolder);
                    else
                        return book;
                }
                else
                {
                    // 出力用のExcelWorkbookが既に存在するか
                    string[] fileList
                        = Directory.GetFileSystemEntries(outputBookFolder, outputBookName);

                    if (fileList.Count() == 0)
                    {
                        if (createIfNotExists)
                            File.Copy(templateBookPath, outputBookFolder + outputBookName);
                        else
                            return book;
                    }
                    else
                    {
                        if (deleteIfExists)
                        {
                            File.Delete(outputBookFolder + outputBookName);
                            File.Copy(templateBookPath, outputBookFolder + outputBookName);
                        }
                    }

                    // ExcelWorkBookを取得する
                    book = GetWorkBook(application, outputBookFolder + outputBookName);
                }

            }

            catch (Exception)
            {
                throw;
            }

            return book;
        }

        /// <summary>
        /// Excelワークシートの検索
        /// </summary>
        /// <param name="targetWorkbook">対象のWorkbook</param>
        /// <param name="targetSheetName">対象のシート名</param>
        /// <returns>Excel.Worksheetオブジェクト</returns>
        public static Worksheet SearchSheet(Workbook targetWorkbook, string targetSheetName)
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
        public static void DeleteWorksheet(Workbook targetWorkbook, string targetSheetName)
        {
            Worksheet targetSheet = null;

            try
            {
                targetSheet = SearchSheet(targetWorkbook, targetSheetName);
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
        /// Excelファイルの削除
        /// </summary>
        /// <param name="folder">削除対象ファイルのフォルダ</param>
        /// <param name="fileName">ファイル名</param>
        /// <returns>結果</returns>
        public static bool DeleteOutputExcelFile(string folder, string fileName)
        {
            bool ret = false;

            try
            {
                // Excelファイルが存在するか
                string[] fileList
                    = Directory.GetFileSystemEntries(folder, fileName);

                if (fileList.Count() > 0)
                {
                    File.Delete(folder + fileName);
                    ret = true;
                }

            }

            catch (Exception)
            {
                throw;
            }

            return ret;
        }

        /// <summary>
        /// 改ページの挿入
        /// </summary>
        /// <param name="wSheet">挿入対象のシート</param>
        /// <param name="col">挿入列</param>
        /// <param name="end">挿入位置</param>
        public static void InsertKaiPage(Worksheet wSheet, string col, int end)
        {
            Range range = null;

            try
            {
                //挿入位置の設定
                range = wSheet.Range(col + end);

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
    }
}

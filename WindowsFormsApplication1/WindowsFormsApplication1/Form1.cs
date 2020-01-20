using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WindowsFormsApplication1.Model;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {

        /// <summary>
        /// 現在時刻
        /// </summary>
        public DateTime DateTimeNow { get; set; }

        /// <summary>
        /// ExcelApplication
        /// </summary>
        private Microsoft.Office.Interop.Excel.Application application = null;

        /// <summary>
        /// テンプレート用のワークブック
        /// </summary>
        private Workbook templateWorkBook = null;

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
        /// 出力ワークシート名
        /// </summary>
        internal string OutputWorkSheetName { get; set; }


        /// <summary>
        /// 出力用のワークブック
        /// </summary>
        private Workbook outputWorkBook = null;

        /// <summary>
        /// 出力用のワークシート
        /// </summary>
        private Worksheet outputWorkSheet = null;

        /// <summary>
        /// 入力データモデル
        /// </summary>
        private List<TestDataModel> test = new List<TestDataModel>();

        /// <summary>
        /// 
        /// </summary>
        public Form1()
        {
            InitializeComponent();

        }

        private void NewComandExcel_Click(object sender, EventArgs e)
        {

            test.Clear();
            test.Add(new TestDataModel { Field1 = "a", Field2 = "1", Field3 = "b", Field4 = "4" });
            test.Add(new TestDataModel { Field1 = "b", Field2 = "2", Field3 = "c", Field4 = "3" });
            test.Add(new TestDataModel { Field1 = "c", Field2 = "3", Field3 = "d", Field4 = "2" });
            test.Add(new TestDataModel { Field1 = "d", Field2 = "4", Field3 = "e", Field4 = "1" });

            Boolean ret = false;

            ret = DataTableExport2(test);

        }

        private Boolean DataTableExport2(List<TestDataModel> Dt)
        {

            outputWorkBook = null;
            outputWorkSheet = null;

            // ExcelApplicationの取得
            application = new Microsoft.Office.Interop.Excel.Application();
            application.DisplayAlerts = false;

            //テンプレートのパス
            TemplateWorkBookPath = "C:\\template\\test.xlsx";

            //出力Excelファイルのパス＋ファイル名
            OutputWorkBookPath = "C:\\temp\\test.xlsx";

            // 出力用のExcelワークブックのフォルダ
            OutputWorkBookFolder = OutputWorkBookFolder ?? Path.GetDirectoryName(OutputWorkBookPath) + "\\";
            
            // 現在時刻の取得
            DateTimeNow = DateTime.Now;

            // 出力用のExcelワークブック名
            OutputWorkBookName = "test2.xlsx";

            //ファイルの削除
            File.Delete(OutputWorkBookFolder + OutputWorkBookName);

            //ファイルのコピー
            File.Copy(TemplateWorkBookPath, OutputWorkBookFolder + OutputWorkBookName);

            //出力用Excelを開く
            outputWorkBook = application.Workbooks.Open(OutputWorkBookFolder + OutputWorkBookName,
                  Type.Missing, Type.Missing, Type.Missing,
                  Type.Missing, Type.Missing, Type.Missing,
                  Type.Missing, Type.Missing, Type.Missing,
                  Type.Missing, Type.Missing, Type.Missing,
                  Type.Missing, Type.Missing);

            //出力用ワークシート名
            OutputWorkSheetName = "Sheet2";

            //出力用Excelのシートを選択
            foreach (Worksheet sheet in outputWorkBook.Worksheets)
            {
                if (sheet.Name.Equals(OutputWorkSheetName))
                {
                    outputWorkSheet = sheet;
                    break;
                }
            }

            Range range = null;
            int listCount = Dt.Count;

            // 罫線
            int startLeftRowPosition = 5;               // 左上の行番号
            int endRightRowPosition = listCount + 4;    // 右下の行番号

            // 印刷範囲の設定
            outputWorkSheet.PageSetup.PrintArea = @"$A$1:$D$" + (endRightRowPosition).ToString();

            // 罫線および値を出力するレンジの設定
            range = outputWorkSheet.get_Range("$A$" + (startLeftRowPosition).ToString(),
                                          "$D$" + (endRightRowPosition).ToString());

            var outputData = new object[Dt.Count, 4];

            //Excel出力用に加工する
            int row = 0;
            foreach (TestDataModel DtRow in Dt)
            {
                outputData[row, 0] = DtRow.Field1;
                outputData[row, 1] = DtRow.Field2;
                outputData[row, 2] = DtRow.Field3;
                outputData[row, 3] = DtRow.Field4;
                row++;
            }

            //加工したデータを選択エリアへコピー
            range.Value2 = outputData;

            outputWorkBook.SaveAs(OutputWorkBookFolder + OutputWorkBookName,
                Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, XlSaveAsAccessMode.xlNoChange,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

            //ブックを閉じる
            outputWorkBook.Close(false, false, false);

            //Excel COMオブジェクトの開放
            if (outputWorkBook != null)
            {
                Marshal.ReleaseComObject(outputWorkBook);
                outputWorkBook = null;
            }

            if (outputWorkSheet != null)
            {
                Marshal.ReleaseComObject(outputWorkSheet);
                outputWorkSheet = null;
            }

            Marshal.ReleaseComObject(range);
            range = null;


            //オブジェクトの開放
            if (application != null)
            {
                application.Visible = false;
                application.DisplayAlerts = true;
                application.Quit();
                Marshal.ReleaseComObject(application);
                application = null;
            }

            //ガベレージコレクト
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            return true;

        }


        //private Boolean DataTableExport(System.Data.DataTable Dt)
        //{
        //    // Sheet1
        //    Excel.Worksheet ObjSheet = (Excel.Worksheet)ExcelWB.Sheets[1];

        //    // データを二次元配列に格納する
        //    String[,] pData = new String[Dt.Rows.Count, Dt.Columns.Count];

        //    // レコード分ループ
        //    for (int Row = 0; Row < Dt.Rows.Count; Row++)
        //    {
        //        // カラム分ループ
        //        for (int Column = 0; Column < Dt.Columns.Count; Column++)
        //        {
        //            pData[Row, Column] = Dt.Rows[Row][Column].ToString();
        //        }
        //    }

        //    // 二次元配列のデータをExcelに貼り付ける
        //    Excel.Range Rng = (Excel.Range)ObjSheet.Cells[1, 1];
        //    Rng = Rng.get_Resize(Dt.Rows.Count, Dt.Columns.Count);
        //    Rng.set_Item(1, 1, "1");//.Value = pData;
        //    return true;
        //}


    }
}

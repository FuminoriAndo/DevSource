using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {

        //>>>>>>>>>>
        //変数定義
        //>>>>>>>>>>
        //エクセルオブジェクト
        Microsoft.Office.Interop.Excel.Application m_MyExcel = new Microsoft.Office.Interop.Excel.Application();

        //ブックオブジェクト
        Microsoft.Office.Interop.Excel.Workbooks m_MyBook;


        //=======================================================================
        //コンストラクタ
        //=======================================================================
        public Form1()
        {
            InitializeComponent();



            //>>>>>>>>>>
            //エクセル初期化
            //>>>>>>>>>>
            //新規ブックを作成
            m_MyBook = m_MyExcel.Workbooks;
            m_MyBook.Add(string.Empty);

            //エクセルを表示
            m_MyExcel.Visible = true;
        }

        //=======================================================================
        //イベント
        //フォームを閉じる時に呼び出される。
        //=======================================================================
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

            //>>>>>>>>>>
            //エクセル終了時の処理
            //>>>>>>>>>>
            m_MyExcel.Quit();

            //ブックオブジェクト開放
            System.Runtime.InteropServices.Marshal.ReleaseComObject(m_MyBook);

            //エクセルオブジェクト開放
            System.Runtime.InteropServices.Marshal.ReleaseComObject(m_MyExcel);
        }
    }
}

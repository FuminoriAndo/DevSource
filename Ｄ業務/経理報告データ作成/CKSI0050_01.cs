using Microsoft.VisualBasic;
using System.Data;
using System.Data.Odbc;
using System;                           //2013.04.13 miura mori
namespace Project1
{
    static class CKSI0050_01
    {
        public static string G_NENGETU;

        public static string G_USERID;
        public static string G_OFFICEID;
        public static string G_SYOKUICD;
// 2016.12.12 yoshitake 副資材棚卸システム再構築 start.
        public static string G_START_SIZAI_TANAOROSI_SYSTEM;
        public static string G_YEARMONTH;
// 2016.12.12 yoshitake 副資材棚卸システム再構築 end.

        public static string G_MOTOYM;
        public static string[] G_HINMOKUCD = new string[51];
        public static decimal G_SZAIKO;
        public static decimal G_NYUKO;
        public static decimal G_SUIBUN;
        public static decimal G_EZAIKO;
        public static decimal[] G_SIYO = new decimal[11];
        public static decimal[] G_SIYORYO = new decimal[11];
        public static string G_HIMOKU;
        public static string G_UTIWAKE;
        public static string G_TANABAN;
        public static string[] G_HINMOKUNM = new string[51];
        public static string G_SYUBETU;
        public static string G_ICHIKBN;
        public static string G_MUKESAKI;

        //マスタエリア
        public static string[] G_HINMOKU_AREA = new string[11];

        //トランエリア
        //トランのワークエリア
        public static string[] G_TRAN_AREA = new string[81];
        public static string[] G_Sagyosi_Area = new string[21];
        public static string[] G_Tanaorosi_Area = new string[21];
        public static string[] G_Motocho_Area = new string[51];

        public static short Hinmoku_Kensaku(string L_Hinmokucd)
        {
            short functionReturnValue = 1;

            DataTable tbl = null;

            //品目マスタの検索
            IPPAN.G_SQL = "SELECT HIMOKU,UTIWAKE,TANABAN,MUKESAKI FROM SIZAI_HINMOKU_MST ";
            IPPAN.G_SQL = IPPAN.G_SQL + "WHERE HINMOKUCD = '" + L_Hinmokucd + "' ";

            using (C_ODBC db = new C_ODBC())
            {
                try
                {
                    // SQL実行
                    db.Connect();
                    tbl = db.ExecSQL(IPPAN.G_SQL);
                }
                catch (OdbcException)
                {
                    db.Error();
                }
                //2013.04.13 miura mori start
                catch (Exception)
                {
                    db.Error();
                }
                //2013.04.13 miura mori end
            }

            //結果の列数を取得する
            if (tbl.Columns.Count < 1)
            {
                return functionReturnValue;
            }

            //データがない
            if (tbl.Rows.Count == 0)
            {
                return functionReturnValue;
            }

            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                for (int j = 0; j < tbl.Columns.Count; j++)
                {
                    //項目セット
                    G_HINMOKU_AREA[j] = tbl.Rows[i][j].ToString();
                }
            }

            G_HIMOKU = Strings.Mid(G_HINMOKU_AREA[0], 1, 2);
            G_UTIWAKE = Strings.Mid(G_HINMOKU_AREA[1], 1, 2);
            G_TANABAN = Strings.Mid(G_HINMOKU_AREA[2], 1, 2);
            G_MUKESAKI = Strings.Mid(G_HINMOKU_AREA[3], 1, 1);

            functionReturnValue = 0;
            return functionReturnValue;
        }
    }
}

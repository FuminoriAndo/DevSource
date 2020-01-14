using System.Data;
using System.Data.Odbc;
using System;                           //2013.04.13 miura mori
namespace Project1
{
    static class FS_USER_CHECK
    {
        public static short User_Check(string L_UserID)
        {
            short functionReturnValue = 1;

            string[] L_ID_AREA = new string[2];
            DataTable tbl = null;

            //副資材コントロールマスタの検索

            IPPAN.G_SQL = "";
            IPPAN.G_SQL = "Select USERID from SIZAI_CONTROL_MST ";

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
                //副資材コントロールマスタが見つかりません。
                IPPAN.Error_Msg("E700", 0, " ");
                return functionReturnValue;
            }

            for (int j = 0; j < tbl.Columns.Count; j++)
            {
                //項目セット
                L_ID_AREA[j] = "";
                L_ID_AREA[j] = tbl.Rows[0][j].ToString();
            }

            if (L_UserID != L_ID_AREA[0])
            {
                //副資材ユーザ以外は使用できません。
                IPPAN.Error_Msg("E703", 0, " ");
                return functionReturnValue;
            }

            functionReturnValue = 0;
            return functionReturnValue;
        }
    }
}

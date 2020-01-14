using Microsoft.VisualBasic;
using System;
using System.Windows.Forms;
using System.Data;
using System.Text;
using System.Data.Odbc;

namespace Project1
{
    static class IPPAN
    {
        //*************************************************************************************
        //
        //
        //   プログラム名　　　一般関数
        //
        //
        //   修正履歴
        //
        //   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
        //*************************************************************************************
        public static System.Windows.Forms.Control CONTROL_NAME;

        private const int C_LCID_JPN = 0x0411;
        public const int C_CONTROL_COLOR_ENTER = 11;
        public const int C_CONTROL_COLOR_LEAVE = 15;
        public const string C_CURSOR_WAIT = "0";
        public const string C_CURSOR_DEFAULT = "1";

        //コマンドライン引数
        public static string G_COMMAND = string.Empty;
        //リターンコード
        public static int G_RET = 0;
        //日付のワーク
        public static string G_WYMD = string.Empty;
        public static string G_WYMD2 = string.Empty;
        //添字
        public static int G_I = 0;

        //SQL処理用ワーク
        public static string G_SQL = string.Empty;
        public static int G_rcols = 0;
        public static string G_svalue = string.Empty;
        public static int G_lvaluelen = 0;
        public static int G_larg2 = 0;
        public static int G_ivaluelen = 0;
        public static string G_sgridtext = string.Empty;
        public static int G_rcnt = 0;
        public static string G_datetime = string.Empty;

        //引用画面からのデータ引継パラメータ
        //0:引継なし　1:引継あり
        public static int G_PARAM = 0;

        //文字列の後ろの空白をカット
        public static string Space_Cut(string L_DATA)
        {
            string functionReturnValue = string.Empty;
            //L_Data:文字列
            int nLen = 0;
            int i = 0;
            functionReturnValue = "";
            nLen = Strings.Len(L_DATA);
            for (i = nLen; i >= 1; i += -1)
            {
                if (Strings.Mid(L_DATA, i, 1) == "　"
                 || Strings.Mid(L_DATA, i, 1) == " "
                 || string.IsNullOrEmpty(Strings.Mid(L_DATA, i, 1))
                 || (char)Strings.Mid(L_DATA, i, 1)[0] == Strings.Chr(0))
                {
                }
                else
                {
                    functionReturnValue = Strings.Mid(L_DATA, 1, i);
                    return functionReturnValue;
                }
            }
            return functionReturnValue;
        }

        //エラーメッセージを表示する(アイコン省略)
        public static DialogResult Error_Msg(string L_Err_CD, int L_Type, string L_NO)
        {
            return Error_Msg(L_Err_CD, L_Type, L_NO, MessageBoxIcon.None);
        }

        //エラーメッセージを表示する(アイコン指定)
        public static DialogResult Error_Msg(string L_Err_CD, int L_Type, string L_NO, MessageBoxIcon icnType)
        {
            //L_Err_CD:エラーコード L_Type:メッセージボックスのタイプ L_NO:表示する数字項目がある時使用。通常スペース
            string sMsg = string.Empty;
            string sSql = string.Empty;
            MessageBoxButtons btnType = GetBottunType(L_Type);
            DialogResult dlgResult = DialogResult.None;
            DataTable tbl = null;

            dlgResult = DialogResult.None;

            switch (L_Err_CD)
            {
                case "I007":
                    goto Noread;
                case "I009":
                    goto Noread2;
                case "I013":
                    goto Noread3;
                case "I501":
                    goto Noread4;
            }

            // SQL作成
            sSql = "Select ERRMSG from ERROR_MST WHERE ERRCD = '" + L_Err_CD + "'";

            // SQL実行
            using (C_ODBC db = new C_ODBC())
            {
                try
                {
                    // SQL実行
                    db.Connect();
                    tbl = db.ExecSQL(sSql);
                }
                catch (OdbcException)
                {
                    db.Error();
                }
                catch (Exception)
                {
                    return dlgResult;
                }
            }

            // 結果の列数を取得する
            if (tbl.Columns.Count < 1)
            {
                return dlgResult;
            }

            //データがない
            if (tbl.Rows.Count == 0)
            {
                return dlgResult;
            }

            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                for (int j = 0; j < tbl.Columns.Count; j++)
                {
                    //項目セット
                    sMsg = tbl.Rows[i][j].ToString();
                }
            }

            dlgResult = C_COMMON.Msg(sMsg, btnType);
            return dlgResult;

        Noread: ;
            //特殊な場合
            dlgResult = C_COMMON.Msg("申請№ = " + Strings.Mid(L_NO, 1, 2) + C_COMMON.FormatToNum(Strings.Mid(L_NO, 5, 8), "00000000") + "で申請されました。", MessageBoxButtons.OK);
            return dlgResult;
        Noread2: ;
            dlgResult = C_COMMON.Msg("リスト番号" + Strings.Mid(L_NO, 1, 2) + C_COMMON.FormatToNum(Strings.Mid(L_NO, 5, 8), "00000000") + "のリスト再出力を行います。" + Strings.Chr(10) + "プリンタの状態を確認してください。", MessageBoxButtons.YesNo);
            return dlgResult;
        Noread3: ;
            dlgResult = C_COMMON.Msg("申請№" + Strings.Mid(L_NO, 1, 2) + C_COMMON.FormatToNum(Strings.Mid(L_NO, 5, 8), "00000000") + "は承認されていません", MessageBoxButtons.OK);
            return dlgResult;
        Noread4: ;
            dlgResult = C_COMMON.Msg("対象件数は " + C_COMMON.FormatToNum(L_NO, "#######0") + "件です。表示しますか", MessageBoxButtons.YesNo);
            return dlgResult;
        }

        //VBのメッセージボタンの形式を変換する
        private static MessageBoxButtons GetBottunType(int nType)
        {
            MessageBoxButtons btnRet = MessageBoxButtons.OK;

            switch (nType)
            {
                case (int)MsgBoxStyle.AbortRetryIgnore:
                    btnRet = MessageBoxButtons.AbortRetryIgnore;
                    break;
                case (int)MsgBoxStyle.OkOnly:
                    btnRet = MessageBoxButtons.OK;
                    break;
                case (int)MsgBoxStyle.OkCancel:
                    btnRet = MessageBoxButtons.OKCancel;
                    break;
                case (int)MsgBoxStyle.RetryCancel:
                    btnRet = MessageBoxButtons.RetryCancel;
                    break;
                case (int)MsgBoxStyle.YesNo:
                    btnRet = MessageBoxButtons.YesNo;
                    break;
                case (int)MsgBoxStyle.YesNoCancel:
                    btnRet = MessageBoxButtons.YesNoCancel;
                    break;
                default:
                    break;
            }

            return btnRet;
        }

        //西暦より曜日を求める
        public static string YOBI_HENKAN(string MyDate)
        {
            string functionReturnValue = string.Empty;

            functionReturnValue = Strings.StrConv(" 曜日", VbStrConv.Wide, C_LCID_JPN);
            if (C_Allspace(MyDate) == 0)
            {
                return functionReturnValue;
            }

            DateTime dt = Convert.ToDateTime(Strings.Mid(MyDate, 1, 4) + "/" + Strings.Mid(MyDate, 5, 2) + "/" + Strings.Mid(MyDate, 7, 2));

            // 書式で曜日変換を行う
            functionReturnValue = dt.ToString("dddd");
            return functionReturnValue;
        }


        //文字列が全て空白もしくは空かどうかチェックする
        //空白、空　：０を返す
        //空白でない：１を返す
        public static int C_Allspace(string buf)
        {
            int functionReturnValue = 0;

            int n = 0;
            int m = 0;

            n = Strings.Len(buf);
            m = 0;

            for (int i = 1; i <= n; i++)
            {
                if (Strings.Mid(buf, i, 1) == "　"
                 || Strings.Mid(buf, i, 1) == " "
                 || string.IsNullOrEmpty(Strings.Mid(buf, i, 1))
                 || (char)Strings.Mid(buf, i, 1)[0] == Strings.Chr(0))
                {
                    m = m + 1;
                }
            }

            if (n == m)
            {
                functionReturnValue = 0;
                return functionReturnValue;
            }

            functionReturnValue = 1;
            return functionReturnValue;
        }

        //フォームのコントロールバーの表示を行う
        //各項目は全て左揃えで、オーバーした文字は切り捨てる。
        public static void Control_Init(Form actFrm, string PG_Name, string PG_ID, string SO_Uname, string SO_Oname, string R_NO)
        {
            StringBuilder L_PG_Name = new StringBuilder(10);
            StringBuilder L_SO_Uname = new StringBuilder(19);
            StringBuilder L_SO_Oname = new StringBuilder(19);
            string sTitle = string.Empty;
            string sTmp = string.Empty;

            //2013.09.19 DSK yoshida START
            //sTmp = Strings.StrConv(PG_Name + new string('_', 10), VbStrConv.Wide, C_LCID_JPN);
            sTmp = Strings.StrConv(Space_Cut(PG_Name) + new string('_', 10), VbStrConv.Wide, C_LCID_JPN);
            //2013.09.19 DAK yoshida END
            L_PG_Name.Insert(0, C_COMMON.LeftB(sTmp, 20));
            //2013.09.19 DSK yoshida START
            //sTmp = Strings.StrConv(SO_Uname + new string('_', 19), VbStrConv.Wide, C_LCID_JPN); //2013.04.12 miura mori "new string('_'"->"new string(' '"
            //L_SO_Uname.Insert(0, C_COMMON.LeftB(sTmp, 38));
            sTmp = Strings.StrConv(Space_Cut(SO_Uname) + new string('_', 18), VbStrConv.Wide, C_LCID_JPN);
            L_SO_Uname.Insert(0, C_COMMON.LeftB(sTmp, 36));
            //sTmp = Strings.StrConv(SO_Oname + new string('_', 19), VbStrConv.Wide, C_LCID_JPN); //2013.04.12 miura mori "new string('_'"->"new string(' '"
            sTmp = Strings.StrConv(Space_Cut(SO_Oname) + new string('_', 19), VbStrConv.Wide, C_LCID_JPN);
            //2013.09.19 DAK yoshida END
            L_SO_Oname.Insert(0, C_COMMON.LeftB(sTmp, 38));

            sTitle = "";
            sTitle += L_PG_Name.ToString();
            sTitle += "(" + PG_ID + ") ";
            sTitle += L_SO_Oname.ToString() + " ";
            sTitle += L_SO_Uname.ToString() + " ";
            //2013.09.19 DSK yoshida START
            //sTitle += String.Format("{0:yy-MM-dd HH:mm:ss}", DateTime.Now);
            sTitle += String.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);
            //2013.09.19 DSK yoshida END
            sTitle += " Rev" + R_NO;
            actFrm.Text = sTitle;
        }

        //社員マスタの生年月日用
        //日付の妥当性をチェックする
        //正常：０を返す
        //異常：１を返す
        public static int Date_Check2(string YMD)
        {
            int functionReturnValue = 0;

            string Wk_YMD = string.Empty;

            //2013.08.22 DSK yoshida start
            DateTime dt;
            if (YMD.Length != 8 || !DateTime.TryParse(Strings.Mid(YMD, 1, 4) + "/" + Strings.Mid(YMD, 5, 2) + "/" + Strings.Mid(YMD, 7, 2), out dt))
            {
                return 1;
            }
            //2013.08.22 DSK yoshida end

            Wk_YMD = String.Format("{0:yyyy/MM/dd}", DateAndTime.DateSerial(Convert.ToInt32(Strings.Mid(YMD, 1, 4)), Convert.ToInt32(Strings.Mid(YMD, 5, 2)), Convert.ToInt32(Strings.Mid(YMD, 7, 2))));
            if (Wk_YMD == Strings.Mid(YMD, 1, 4) + "/" + Strings.Mid(YMD, 5, 2) + "/" + Strings.Mid(YMD, 7, 2))
            {
                functionReturnValue = 0;
            }
            else
            {
                functionReturnValue = 1;
            }
            return functionReturnValue;
        }

        //システム日付＋－２以内
        //日付の妥当性をチェックする
        //正常：０を返す
        //異常：１を返す
        public static int Date_Check(string YMD)
        {
            int functionReturnValue = 0;

            string Wk_YMD = string.Empty;

            //2013.08.22 DSK yoshida start
            DateTime dt;
            if (YMD.Length != 8 || !DateTime.TryParse(Strings.Mid(YMD, 1, 4) + "/" + Strings.Mid(YMD, 5, 2) + "/" + Strings.Mid(YMD, 7, 2), out dt))
            {
                return 1;
            }
            //2013.08.22 DSK yoshida end

            Wk_YMD = String.Format("{0:yyyy/MM/dd}", DateAndTime.DateSerial(Convert.ToInt32(Strings.Mid(YMD, 1, 4)), Convert.ToInt32(Strings.Mid(YMD, 5, 2)), Convert.ToInt32(Strings.Mid(YMD, 7, 2))));
            if (Wk_YMD == Strings.Mid(YMD, 1, 4) + "/" + Strings.Mid(YMD, 5, 2) + "/" + Strings.Mid(YMD, 7, 2))
            {
                //入力年が前後３年以外はエラー
                if (Convert.ToInt32(String.Format("{0:yyyy}", DateTime.Now)) - 3 < Convert.ToInt32(Strings.Mid(YMD, 1, 4))
                 && Convert.ToInt32(String.Format("{0:yyyy}", DateTime.Now)) + 3 > Convert.ToInt32(Strings.Mid(YMD, 1, 4)))
                {
                    functionReturnValue = 0;
                }
                else
                {
                    functionReturnValue = 1;
                }
            }
            else
            {
                functionReturnValue = 1;
            }

            return functionReturnValue;
        }

        //日付の６桁<->８桁相互変換をする
        //正常：０を返す
        //異常：１を返す
        public static int Date_Henkan(ref string YMD)
        {
            int functionReturnValue = 0;
            string Wk_YMD = string.Empty;
            functionReturnValue = 1;

            //数値チェック関数呼出し追加
            if (Numeric_Check2(YMD) == 1)
            {
                return functionReturnValue;
            }

            if ((Strings.Len(YMD) == 6) || (Strings.Len(YMD) == 8))
            {
            }
            else
            {
                return functionReturnValue;
            }

            if (Strings.Len(YMD) == 6)
            {
                if (Convert.ToInt32(Strings.Mid(YMD, 1, 2)) >= 95)
                {
                    //型変換(文字->数値）
                    Wk_YMD = Convert.ToString(19000000 + Convert.ToInt32(YMD));
                }
                else
                {
                    //型変換(文字->数値）
                    Wk_YMD = Convert.ToString(20000000 + Convert.ToInt32(YMD));
                }
            }
            else
            {
                Wk_YMD = Strings.Mid(YMD, 3, 6);
            }

            YMD = Wk_YMD;

            functionReturnValue = 0;
            return functionReturnValue;
        }

        //フォーカス取得時のテキストの背景色を設定する
        public static void Got_Text(System.Windows.Forms.Control fCtrl, int Col)
        {
            CONTROL_NAME = fCtrl;
            CONTROL_NAME.BackColor = System.Drawing.ColorTranslator.FromOle(Information.QBColor(Col));
            if (CONTROL_NAME is TextBox)
            {
                //フォーカス取得時にテキストの全選択状態を解除する
                TextBox txtObj = (TextBox)CONTROL_NAME;
                txtObj.SelectionStart = 0;
            }
        }

        //データ内のシングルクォーテーションとカンマのチェックを行う
        //カンマ無し：０を返す
        //カンマあり：１を返す
        public static int Input_Check(string In_Data)
        {
            int functionReturnValue = 0;
            if (Strings.InStr(In_Data, ",", CompareMethod.Binary) != 0 || Strings.InStr(In_Data, "'", CompareMethod.Binary) != 0)
            {
                //カンマあり
                functionReturnValue = 1;
            }
            else
            {
                //カンマなし
                functionReturnValue = 0;
            }
            return functionReturnValue;

        }

        //フォーカス喪失時のテキストの背景色を設定する
        public static void Lost_Text(int Col)
        {
            if (CONTROL_NAME != null)
            {
                CONTROL_NAME.BackColor = System.Drawing.ColorTranslator.FromOle(Information.QBColor(Col));
            }
        }

        //入力データの四捨五入、切り上げ、切り捨てを行う
        //小数点３位まで対応
        //結果を返す
        public static double Marume_RTN(double In_Data, int M_Type, int Keta)
        {
            double functionReturnValue = 0;

            switch (Keta)
            {
                case 1:
                    //小数点１位
                    functionReturnValue = In_Data;
                    break;
                case 2:
                    //小数点２位
                    functionReturnValue = In_Data * 10;
                    break;
                case 3:
                    //小数点３位
                    functionReturnValue = In_Data * 100;
                    break;
                default:
                    functionReturnValue = In_Data;
                    break;
            }

            switch (M_Type)
            {
                case 0:
                    // 四捨五入
                    functionReturnValue = Math.Round(functionReturnValue, MidpointRounding.AwayFromZero);
                    break;
                case 1:
                    //切り上げ
                    if (functionReturnValue < 0)
                    {
                        functionReturnValue = functionReturnValue * -1;
                        functionReturnValue = Math.Ceiling(functionReturnValue);
                        functionReturnValue = functionReturnValue * -1;
                    }
                    else
                    {
                        functionReturnValue = Math.Ceiling(functionReturnValue);
                    }
                    break;
                case 2:
                    //切り捨て
                    functionReturnValue = Math.Truncate(functionReturnValue);
                    break;
                default:
                    break;
            }

            switch (Keta)
            {
                case 1:
                    //小数点１位
                    break;
                case 2:
                    //小数点２位
                    functionReturnValue = functionReturnValue / 10;
                    break;
                case 3:
                    //小数点３位
                    functionReturnValue = functionReturnValue / 100;
                    break;
                default:
                    break;
            }

            return functionReturnValue;
        }

        //金額データの符号とカンマの編集を行い右詰表示
        //結果を返す
        public static string Money_Hensyu(string Kingaku, string Hensyu)
        {
            if (string.IsNullOrEmpty(Kingaku))
            {
                return string.Empty;
            }
            return Strings.Right(Strings.Space(Strings.Len(Hensyu)) + String.Format("{0:" + Hensyu + "}", Convert.ToDecimal(Kingaku)), Strings.Len(Hensyu));
        }

        //次のオブジェクトにフォーカスを移す
        public static void Next_Focus(Form actForm)
        {
            //次のオブジェクトにフォーカスを移す
            int i = 0;

            for (i = 0; i < actForm.Controls.Count; i++)
            {
                if (Strings.Mid(actForm.Controls[i].Name, 1, 3) == "TMR"
                 || Strings.Mid(actForm.Controls[i].Name, 1, 3) == "FRM"
                 || Strings.Mid(actForm.Controls[i].Name, 1, 3) == "LBL")
                {
                    continue;
                }
                else
                {
                    if (actForm.Controls[i].TabIndex == actForm.ActiveControl.TabIndex + 1)
                    {
                        if (actForm.Controls[i].Enabled == true)
                        {
                            actForm.Controls[i].Focus();
                            return;
                        }
                    }
                }
            }

            for (i = 0; i < actForm.Controls.Count; i++)
            {
                if (Strings.Mid(actForm.Controls[i].Name, 1, 3) == "TMR"
                 || Strings.Mid(actForm.Controls[i].Name, 1, 3) == "FRM"
                 || Strings.Mid(actForm.Controls[i].Name, 1, 3) == "LBL")
                {
                    continue;
                }
                else
                {
                    if (actForm.Controls[i].TabIndex == 0)
                    {
                        if (actForm.Controls[i].Enabled == true)
                        {
                            actForm.Controls[i].Focus();
                            return;
                        }
                    }
                }
            }

        }

        public static void PG_Init(Form actForm)
        {
            actForm.WindowState = System.Windows.Forms.FormWindowState.Maximized;
        }

        //実行中のマウスポインタを砂時計および標準にする
        public static void Pointer_Change(Form actForm, string sType)
        {
            switch (sType)
            {
                case C_CURSOR_WAIT:
                    actForm.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                    break;
                case C_CURSOR_DEFAULT:
                    actForm.Cursor = System.Windows.Forms.Cursors.Default;
                    break;
                default:
                    break;
            }
        }

        //文字列の右側に空白を埋める
        public static string Space_Set(string L_DATA, int L_Byte, int L_Type)
        {
            string functionReturnValue = "";
            //L_Data:文字列 L_Byte:文字列の最大文字数(２バイトの場合は1/2)
            //L_Type:1->１バイトエリア 2->２バイトエリア
            //************注意事項受け側のエリアは固定でバイト数を指定しないこと**************
            //L_Byteの文字数だけ引き渡す
            string sWork = "";
            int nCnt = 0;

            switch (L_Type)
            {
                case 1:
                    nCnt = L_Byte - Strings.Len(Strings.Trim(L_DATA));
                    if (nCnt < 0)
                    {
                        nCnt = 0;
                    }
                    functionReturnValue = Strings.Mid(Strings.Trim(L_DATA) + Strings.Space(nCnt), 1, L_Byte);
                    break;
                case 2:
                    //"\"がある時は２バイトに変換する。
                    if (Strings.InStr(L_DATA, "\\", CompareMethod.Binary) != 0)
                    {
                        for (int i = 1; i <= Strings.Len(Strings.Trim(L_DATA)); i++)
                        {
                            if (Strings.Mid(Strings.Trim(L_DATA), i, 1) == "\\")
                            {
                                sWork = sWork + "￥";
                            }
                            else
                            {
                                sWork = sWork + Strings.StrConv(Strings.Mid(Strings.Trim(L_DATA), i, 1), VbStrConv.Wide, C_LCID_JPN);
                            }
                        }
                        functionReturnValue = Strings.StrConv(Strings.Trim(sWork) + Strings.Space(100), VbStrConv.Wide, C_LCID_JPN);
                        functionReturnValue = Strings.Mid(functionReturnValue, 1, L_Byte);
                    }
                    else
                    {
                        functionReturnValue = Strings.StrConv(Strings.Trim(L_DATA) + Strings.Space(100), VbStrConv.Wide, C_LCID_JPN);
                        functionReturnValue = Strings.Mid(functionReturnValue, 1, L_Byte);
                    }
                    break;
                default:
                    break;
            }
            return functionReturnValue;
        }

        //数値チェック関数
        public static int Numeric_Check(string L_DATA)
        {
            int functionReturnValue = 0;
            functionReturnValue = 1;
            if (Information.IsNumeric(L_DATA) == false)
            {
                return functionReturnValue;
            }
            functionReturnValue = 0;
            return functionReturnValue;
        }

        public static string Numeric_Hensyu(string L_DATE)
        {
            //※小数点は未対応
            int nLen = 0;
            string sStr1 = String.Empty;
            string sStr2 = String.Empty;
            // 先頭にマイナス符号がある時に１をたてる
            bool bMinusFlg = false;

            sStr1 = "";
            sStr2 = Strings.Trim(L_DATE);
            nLen = Strings.Len(sStr2);
            if (nLen <= 0)
            {
                return sStr1;
            }

            for (int i = 1; i <= nLen; i++)
            {
                switch (Strings.Mid(sStr2, i, 1))
                {
                    case "0":
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                    case "6":
                    case "7":
                    case "8":
                    case "9":
                        sStr1 = sStr1 + Strings.Mid(sStr2, i, 1);
                        break;
                    case "-":
                        if (i == 1)
                        {
                            bMinusFlg = true;
                        }
                        break;
                }
            }

            if (bMinusFlg && (0 < sStr1.Length))
            {
                sStr1 = Convert.ToString(Convert.ToInt64(sStr1) * -1);
            }
            return sStr1;
        }


        public static int Numeric_Check2(string L_DATA)
        {
            int functionReturnValue = 1;

            for (int i = 1; i <= Strings.Len(L_DATA); i++)
            {
                switch (Strings.Mid(L_DATA, i, 1))
                {
                    case "0":
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                    case "6":
                    case "7":
                    case "8":
                    case "9":
                        break;
                    default:
                        return functionReturnValue;
                }
            }
            functionReturnValue = 0;
            return functionReturnValue;
        }

        ///------------------------------------------------------------------------------------------------------------------
        /// 機能      :  入力税区分変換取得
        ///
        /// 戻り値    :  String型 税区分
        ///
        /// 引き数    :  L_InpKbn                    ：入力税区分
        ///              L_Kamoku                    ：科目コード
        ///              L_Bumon                     ：部門コード
        ///
        /// 機能説明  :  一般管理販売費の入力税区分を変換します。
        ///
        /// 備考      :  2012/04/03 依頼書№:10-136 全面修正
        ///
        ///------------------------------------------------------------------------------------------------------------------
        public static string Change_InputZeiKubun(string L_InpKbn, string L_Kamoku, string L_Bumon)
        {
            string functionReturnValue = string.Empty;

            string vlChange_InpKbn = string.Empty;

            //科目コードが802(一般管理販売費)以外の場合は処理を抜ける。
            if ((Strings.Mid(L_Kamoku, 1, 3) != "802"))
            {
                functionReturnValue = Strings.Mid(L_InpKbn, 1, 2);
                return functionReturnValue;
            }

            switch (Strings.Mid(L_InpKbn, 1, 2))
            {
                case "05":
                    vlChange_InpKbn = "07";
                    break;
                case "06":
                    vlChange_InpKbn = "08";
                    break;
                default:
                    vlChange_InpKbn = Constants.vbNullString;
                    break;
            }

            if (vlChange_InpKbn == Constants.vbNullString)
            {
                functionReturnValue = Strings.Mid(L_InpKbn, 1, 2);
                return functionReturnValue;
            }

            //部門コードを判定して変換税区分を入力税区分に戻す。
            switch (Strings.Mid(L_Bumon, 1, 2))
            {
                case "01":
                case "03":
                case "05":
                case "07":
                case "09":
                case "13":
                case "14":
                case "15":
                case "17":
                case "21":
                case "24":
                case "25":
                case "26":
                case "27":
                case "28":
                case "29":
                case "30":
                case "31":
                case "35":
                    vlChange_InpKbn = Strings.Mid(L_InpKbn, 1, 2);
                    break;
                default:
                    break;
            }

            functionReturnValue = vlChange_InpKbn;
            return functionReturnValue;

        }
    }
}

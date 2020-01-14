using Microsoft.VisualBasic;
using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Data.Odbc;

namespace Project1
{
    internal partial class FRM_CKSI0130S01 : System.Windows.Forms.Form
    {
        //**************************************************************'
        //                                                              '
        //   プログラム名　　　単価設定画面処理                           '
        //                                                              '
        //   更新履歴
        //   更新日付    Rev     修正者      内容
        //   03.01.15           NTIS浅田　　Ｓ／Ｏサーバ更新の為、VB4.0→VB6.0SP5へのバージョンアップ対応
        //   13.07.03           ISV-TRUC    資材単価設定画面　初期表示と資材単価設定画面　プロパティ
        //   13.07.03           ISV-TRUC    資材単価設定画面　業者コードのLeaveイベント
        //**************************************************************'

        //業者コード変更用ワーク
        string WK_GyosyaCd;
        //単価変更用ワーク
        string WK_Tanka;
        //支払条件コード変更用ワーク
        string WK_SiharaiCd;

        //   13.07.03   ISV-TRUC    資材単価設定画面　資材単価設定画面　プロパティ
        public string HinmokuCD;
        public string HinmokuNM;
        // End  13.07.03   ISV-TRUC 
        private void Work_Set()
        {
            short i = 0;

            for (i = 0; i <= 9; i += 1)
            {
                // ワーク品目コード
                CKSI0130.G_TANKA_AREA[i, 0] = IPPAN2.MixSpace_Set(LBL_HinCd.Text, 4);
                // ワーク業者コード
                CKSI0130.G_TANKA_AREA[i, 1] = IPPAN2.MixSpace_Set(TXT_GyoCd[i].Text, 4);
                // ワーク単価
                CKSI0130.G_TANKA_AREA[i, 2] = CKSI0130.G_Tanka[i];
                // ワーク支払条件コード
                CKSI0130.G_TANKA_AREA[i, 3] = IPPAN2.MixSpace_Set(TXT_SihaCd[i].Text, 2);
            }

        }
        private short Gamen_Check()
        {
            short functionReturnValue = 1;
            short i = 0;

            for (i = 0; i <= 9; i += 1)
            {
                //業者
                if (IPPAN.C_Allspace(TXT_GyoCd[i].Text) != 0)
                {
                    if (IPPAN.Input_Check(TXT_GyoCd[i].Text) == 1)
                    {
                        LBL_GyoNm[i].Text = "";
                        TXT_GyoCd[i].Focus();
                        TXT_GyoCd_Enter(TXT_GyoCd[i], new System.EventArgs());
                        //入力エラー
                        IPPAN.Error_Msg("E202", 0, " ");
                        return functionReturnValue;
                    }
                    IPPAN.G_RET = IPPAN.Numeric_Check2(TXT_GyoCd[i].Text);
                    if (IPPAN.G_RET == 1)
                    {
                        LBL_GyoNm[i].Text = "";
                        TXT_GyoCd[i].Focus();
                        TXT_GyoCd_Enter(TXT_GyoCd[i], new System.EventArgs());
                        //入力エラー
                        IPPAN.Error_Msg("E202", 0, " ");
                        IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                        return functionReturnValue;
                    }

                    if (CKSI0130.Gyosya_Kensaku(TXT_GyoCd[i].Text) != 0)
                    {
                        LBL_GyoNm[i].Text = "";
                        //フォーカスセット
                        TXT_GyoCd[i].Focus();
                        TXT_GyoCd_Enter(TXT_GyoCd[i], new System.EventArgs());
                        IPPAN.Error_Msg("E503", 0, " ");
                        return functionReturnValue;
                    }
                    LBL_GyoNm[i].Text = CKSI0130.G_GYOSYA_AREA[0];

                    //単価
                    if (IPPAN.C_Allspace(TXT_Tanka[i].Text) != 0)
                    {
                        if (IPPAN.Numeric_Check(TXT_Tanka[i].Text) == 1)
                        {
                            TXT_Tanka[i].Focus();
                            TXT_Tanka_Enter(TXT_Tanka[i], new System.EventArgs());
                            //入力エラー
                            IPPAN.Error_Msg("E202", 0, " ");
                            return functionReturnValue;
                        }
                    }
                    else
                    {
                        //フォーカスセット
                        TXT_Tanka[i].Focus();
                        TXT_Tanka_Enter(TXT_Tanka[i], new System.EventArgs());
                        //必須項目入力エラー
                        IPPAN.Error_Msg("E200", 0, "");
                        return functionReturnValue;
                    }

                    //支払条件
                    if (IPPAN.C_Allspace(TXT_SihaCd[i].Text) == 0)
                    {
                        LBL_SihaNm[i].Text = "";
                        TXT_SihaCd[i].Focus();
                        TXT_SihaCd_Enter(TXT_SihaCd[i], new System.EventArgs());
                        //必須項目入力エラー
                        IPPAN.Error_Msg("E200", 0, " ");
                        return functionReturnValue;
                    }
                    if (IPPAN.Input_Check(TXT_SihaCd[i].Text) == 1)
                    {
                        LBL_SihaNm[i].Text = "";
                        TXT_SihaCd[i].Focus();
                        TXT_SihaCd_Enter(TXT_SihaCd[i], new System.EventArgs());
                        //入力エラー
                        IPPAN.Error_Msg("E202", 0, " ");
                        return functionReturnValue;
                    }

                    if (CKSI0130.Jyoken_Kensaku(TXT_SihaCd[i].Text) != 0)
                    {
                        LBL_SihaNm[i].Text = "";
                        //フォーカスセット
                        TXT_SihaCd[i].Focus();
                        TXT_SihaCd_Enter(TXT_SihaCd[i], new System.EventArgs());
                        //支払条件マスタが見つかりません
                        IPPAN.Error_Msg("E108", 0, " ");
                        return functionReturnValue;
                    }
                    LBL_SihaNm[i].Text = CKSI0130.G_JYOKEN_AREA[0];
                }
                else
                {
                    LBL_GyoNm[i].Text = "";
                    TXT_Tanka[i].Enabled = false;
                    TXT_SihaCd[i].Enabled = false;
                    LBL_SihaNm[i].Text = "";
                }

            }
            functionReturnValue = 0;
            return functionReturnValue;
        }

        public void Gamen_Set()
        {
            short i = 0;

            for (i = 0; i <= 9; i += 1)
            {
                if (i == CKSI0130.G_N)
                {
                    break; // TODO: might not be correct. Was : Exit For
                }

                // 業者コード
                TXT_GyoCd[i].Text = Strings.Trim(CKSI0130.G_TANKA_AREA[i, 1]);
                if (CKSI0130.Gyosya_Kensaku(TXT_GyoCd[i].Text) == 0)
                {
                    // 業者名
                    LBL_GyoNm[i].Text = Strings.Trim(Strings.Mid(CKSI0130.G_GYOSYA_AREA[0], 1, 12));
                }
                // 数量                                       ' 品目コード
                TXT_Tanka[i].Text = IPPAN.Money_Hensyu(Strings.Trim(CKSI0130.G_TANKA_AREA[i, 2]), "##,###,##0.000");
                CKSI0130.G_Tanka[i] = IPPAN2.Numeric_Hensyu4(Strings.Trim(TXT_Tanka[i].Text));
                // 支払条件コード
                TXT_SihaCd[i].Text = Strings.Trim(CKSI0130.G_TANKA_AREA[i, 3]);
                if (CKSI0130.Jyoken_Kensaku(TXT_SihaCd[i].Text) == 0)
                {
                    // 支払条件名
                    LBL_SihaNm[i].Text = Strings.Trim(Strings.Mid(CKSI0130.G_JYOKEN_AREA[0], 1, 23));
                }
            }
        }
        private void Gamen_Clear()
        {
            short i = 0;

            for (i = 0; i <= 9; i++)
            {
                // 業者コード
                TXT_GyoCd[i].Text = "";
                // 業者名
                LBL_GyoNm[i].Text = "";
                // 単価
                TXT_Tanka[i].Text = "";
                // 支払条件コード
                TXT_SihaCd[i].Text = "";
                // 業者名
                LBL_SihaNm[i].Text = "";
                CKSI0130.G_Tanka[i] = "";
            }
        }

        private void Enabled_False()
        {
            short i = 0;

            for (i = 0; i <= 9; i++)
            {
                // 業者コード
                TXT_GyoCd[i].Enabled = false;
                // 単価
                TXT_Tanka[i].Enabled = false;
                // 支払条件コード
                TXT_SihaCd[i].Enabled = false;
            }
            // 戻る
            CMD_Button[0].Enabled = true;
            // 入力確定	
            CMD_Button[1].Enabled = false;
        }

        private void CMD_Button_Click(System.Object eventSender, System.EventArgs eventArgs)
        {
            short Index = CMD_Button.GetIndex((Button)eventSender);
            short i = 0;
            DialogResult dlgRslt = DialogResult.None;

            IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_WAIT);
            
            switch (Index)
            {
                case 0:
                    //戻る
                    CKSI0130.CKSI0130S01_RET = 0;
                    this.Close();
                    this.Dispose();
                    break;

                case 1:
                    //入力確定
                    if (Gamen_Check() != 0)
                    {
                        IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                        return;
                    }
                    //登録しますか
                    dlgRslt = IPPAN.Error_Msg("I607", 4, " ");
                    // [いいえ] ボタンを選択した場合
                    if (dlgRslt == DialogResult.No)
                    {
                        TXT_GyoCd[0].Focus();
                        TXT_GyoCd_Enter(TXT_GyoCd[0], new System.EventArgs());
                        IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                        return;
                    }

                    Work_Set();

                    //オーナーフォームを取得
                    FRM_CKSI0130M fmOwner = (FRM_CKSI0130M)this.Owner;

                    //ＳＱＬ文構築
                    using (C_ODBC db = new C_ODBC())
                    {
                        try
                        {
                            //DB接続
                            db.Connect();
                            //トランザクション開始
                            db.BeginTrans();
                            if (fmOwner.OPT_Henko.Checked == true)
                            {
                                IPPAN.G_SQL = "Update SIZAI_HINMOKU_MST ";
                                IPPAN.G_SQL = IPPAN.G_SQL + "SET HINMOKUCD = '" + CKSI0130.HINMOKU.HINMOKUCD + "', ";
                                IPPAN.G_SQL = IPPAN.G_SQL + "HINMOKUNM = '" + CKSI0130.HINMOKU.HINMOKUNM + "', ";
                                IPPAN.G_SQL = IPPAN.G_SQL + "HIMOKU = '" + CKSI0130.HINMOKU.HIMOKU + "', ";
                                IPPAN.G_SQL = IPPAN.G_SQL + "UTIWAKE = '" + CKSI0130.HINMOKU.UTIWAKE + "', ";
                                IPPAN.G_SQL = IPPAN.G_SQL + "TANABAN = '" + CKSI0130.HINMOKU.TANABAN + "', ";
                                IPPAN.G_SQL = IPPAN.G_SQL + "TANI = '" + CKSI0130.HINMOKU.TANI + "', ";
                                IPPAN.G_SQL = IPPAN.G_SQL + "SYUBETU = '" + CKSI0130.HINMOKU.SYUBETU + "', ";
                                IPPAN.G_SQL = IPPAN.G_SQL + "SUIBUNKBN = '" + CKSI0130.HINMOKU.SUIBUNKBN + "', ";
                                IPPAN.G_SQL = IPPAN.G_SQL + "KENSYUKBN = '" + CKSI0130.HINMOKU.KENSYUKBN + "', ";
                                IPPAN.G_SQL = IPPAN.G_SQL + "HOUKOKUKBN = '" + CKSI0130.HINMOKU.HOUKOKUKBN + "', ";
                                IPPAN.G_SQL = IPPAN.G_SQL + "ICHIKBN = '" + CKSI0130.HINMOKU.ICHIKBN + "', ";
                                IPPAN.G_SQL = IPPAN.G_SQL + "MUKESAKI = '" + CKSI0130.HINMOKU.MUKESAKI + "', ";
                                IPPAN.G_SQL = IPPAN.G_SQL + "UPDYMD = to_date('" + String.Format("{0:yyyy/MM/dd HH:mm:ss}", DateTime.Now) + "', 'YYYY/MM/DD HH24:MI:SS') ";
                                IPPAN.G_SQL = IPPAN.G_SQL + "WHERE HINMOKUCD = '" + CKSI0130.HINMOKU.HINMOKUCD + "'";
                            }
                            if (fmOwner.OPT_Sinki.Checked == true)
                            {
                                IPPAN.G_SQL = "insert into SIZAI_HINMOKU_MST values (";
                                IPPAN.G_SQL = IPPAN.G_SQL + "'" + CKSI0130.HINMOKU.HINMOKUCD + "'";
                                IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0130.HINMOKU.HINMOKUNM + "'";
                                IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0130.HINMOKU.HIMOKU + "'";
                                IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0130.HINMOKU.UTIWAKE + "'";
                                IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0130.HINMOKU.TANABAN + "'";
                                IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0130.HINMOKU.TANI + "'";
                                IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0130.HINMOKU.SYUBETU + "'";
                                IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0130.HINMOKU.SUIBUNKBN + "'";
                                IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0130.HINMOKU.KENSYUKBN + "'";
                                IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0130.HINMOKU.HOUKOKUKBN + "'";
                                IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0130.HINMOKU.ICHIKBN + "'";
                                IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0130.HINMOKU.MUKESAKI + "'";
                                IPPAN.G_SQL = IPPAN.G_SQL + ",to_date('" + String.Format("{0:yyyy/MM/dd HH:mm:ss}", DateTime.Now) + "', 'YYYY/MM/DD HH24:MI:SS'))";
                            }
                            //排他チェック（ＯＫｸﾘｯｸ時と現在の UPDYMD を比較）
                            IPPAN.G_RET = CKSI0130.SIZAI_HINMOKU_CHECK(db, fmOwner.OPT_Sakjyo.Checked, fmOwner.OPT_Sinki.Checked, fmOwner.TXT_HinCd.Text);
                            if (IPPAN.G_RET != 0)
                            {
                                if (IPPAN.G_RET == 1)
                                {
                                    IPPAN.Error_Msg("E501", 0, " ");
                                }
                                else
                                {
                                    IPPAN.Error_Msg("E500", 0, " ");
                                }
                                //コミット
                                db.Commit();
                                CKSI0130.CKSI0130S01_RET = 1;
                                this.Close();
                                this.Dispose();
                                return;
                            }
                            Debug.Print(IPPAN.G_SQL);

                            //SQL実行
                            db.ExecSQL(IPPAN.G_SQL);

                            if (fmOwner.OPT_Sakjyo.Checked == true)
                            {
                            }
                            else
                            {
                                IPPAN.G_SQL = "Delete FROM SIZAI_TANKA_MST ";
                                IPPAN.G_SQL = IPPAN.G_SQL + "WHERE HINMOKUCD ='" + fmOwner.TXT_HinCd.Text + "'";
                                Debug.Print(IPPAN.G_SQL);

                                //SQL実行
                                db.ExecSQL(IPPAN.G_SQL);

                                for (i = 0; i <= 9; i += 1)
                                {
                                    if (CKSI0130.G_TANKA_AREA[i, 1] != "    ")
                                    {
                                        IPPAN.G_SQL = "insert into SIZAI_TANKA_MST values (";
                                        IPPAN.G_SQL = IPPAN.G_SQL + "'" + CKSI0130.G_TANKA_AREA[i, 0] + "'";
                                        IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0130.G_TANKA_AREA[i, 1] + "'";
                                        IPPAN.G_SQL = IPPAN.G_SQL + "," + Convert.ToDecimal(CKSI0130.G_TANKA_AREA[i, 2]);
                                        IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0130.G_TANKA_AREA[i, 3] + "'";
                                        IPPAN.G_SQL = IPPAN.G_SQL + ",to_date('" + String.Format("{0:yyyy/MM/dd HH:mm:ss}", DateTime.Now) + "', 'YYYY/MM/DD HH24:MI:SS'))";
                                        Debug.Print(IPPAN.G_SQL);

                                        //SQL実行
                                        db.ExecSQL(IPPAN.G_SQL);
                                    }
                                }
                            }
                            //コミット
                            db.Commit();
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

                    Gamen_Clear();
                    CKSI0130.CKSI0130S01_RET = 1;
                    this.Close();
                    this.Dispose();
                    break;
            }
        }

        private void setEventCtrlAry()
        {
            //コントロール配列にイベントを設定
            //CMD_Button
            for (short i = 0; i < this.CMD_Button.Count(); i++)
            {
                this.CMD_Button[i].Click += new EventHandler(CMD_Button_Click);
            }
            //TXT_GyoCd
            for (short i = 0; i < this.TXT_GyoCd.Count(); i++)
            {
                this.TXT_GyoCd[i].Enter += new EventHandler(TXT_GyoCd_Enter);
                this.TXT_GyoCd[i].Leave += new EventHandler(TXT_GyoCd_Leave);
            }
            //TXT_SihaCd
            for (short i = 0; i < this.TXT_SihaCd.Count(); i++)
            {
                this.TXT_SihaCd[i].Enter += new EventHandler(TXT_SihaCd_Enter);
                this.TXT_SihaCd[i].Leave += new EventHandler(TXT_SihaCd_Leave);
            }
            //TXT_Tanka
            for (short i = 0; i < this.TXT_Tanka.Count(); i++)
            {
                this.TXT_Tanka[i].Enter += new EventHandler(TXT_Tanka_Enter);
                this.TXT_Tanka[i].Leave += new EventHandler(TXT_Tanka_Leave);
            }
        }

        private void FRM_0130S01_Load(System.Object eventSender, System.EventArgs eventArgs)
        {
            // 13.07.03     ISV-TRUC    現行のオーナーフォームから値を参照して設定する処理を、以下のように修正する。
            //オーナーフォームを取得
            //FRM_CKSI0130M fmOwner = (FRM_CKSI0130M)this.Owner;
            //LBL_HinCd.Text = fmOwner.TXT_HinCd.Text;
            //LBL_HinMei.Text = fmOwner.TXT_HinNm.Text;

            //HinmokuCDプロパティの値を、品目コードラベルへ設定する。
            LBL_HinCd.Text = HinmokuCD;
            //HinmokuNMプロパティの値を、品目名ラベルへ設定する。
            LBL_HinMei.Text = HinmokuNM;
            // End 13.07.03    ISV-TRUC

            //コントロール配列にイベントを設定
            setEventCtrlAry();

            //副資材単価マスタの検索
            if (CKSI0130.Fs_Tanka_Kensaku(LBL_HinCd.Text) != 0)
            {
                //データがなかったときは何も表示しない
                Gamen_Clear();
            }
            else
            {
                //データがある時は画面に表示する
                Gamen_Set();
            }
            if (CKSI0130.G_OfficeId != "KOUBAI")
            {
                //オフィスIDが"KOBAI"以外は入力不可
                Enabled_False();
            }
        }

        private void TMR_TIMER_Tick(System.Object eventSender, System.EventArgs eventArgs)
        {
            IPPAN.Control_Init(this, "単価設定入力", "CKSI0130", SO.SO_USERNAME, SO.SO_OFFICENAME, "1.00");
        }

        private void TXT_GyoCd_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            short Index = TXT_GyoCd.GetIndex((TextBox)eventSender);
            TXT_Tanka[Index].Enabled = true;
            TXT_SihaCd[Index].Enabled = true;
            WK_GyosyaCd = TXT_GyoCd[Index].Text;
            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
        }

        private void TXT_GyoCd_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {
            short Index = TXT_GyoCd.GetIndex((TextBox)eventSender);
            if (Strings.Trim(TXT_GyoCd[Index].Text) == WK_GyosyaCd)
            {
                IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
                return;
            }
            if (IPPAN.C_Allspace(TXT_GyoCd[Index].Text) == 0)
            {
                LBL_GyoNm[Index].Text = "";
                TXT_Tanka[Index].Enabled = false;
                TXT_SihaCd[Index].Enabled = false;
                LBL_SihaNm[Index].Text = "";
            }
            else
            {
                if (IPPAN.Input_Check(TXT_GyoCd[Index].Text) == 1)
                {
                    LBL_GyoNm[Index].Text = "";
                    TXT_GyoCd[Index].Focus();
                    TXT_GyoCd_Enter(TXT_GyoCd[Index], new System.EventArgs());
                    //入力エラー
                    IPPAN.Error_Msg("E202", 0, " ");
                    return;
                }
                IPPAN.G_RET = IPPAN.Numeric_Check2(TXT_GyoCd[Index].Text);
                if (IPPAN.G_RET == 1)
                {
                    LBL_GyoNm[Index].Text = "";
                    TXT_GyoCd[Index].Focus();
                    TXT_GyoCd_Enter(TXT_GyoCd[Index], new System.EventArgs());
                    //入力エラー
                    IPPAN.Error_Msg("E202", 0, " ");
                    IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                    return;
                }

                if (CKSI0130.Gyosya_Kensaku(TXT_GyoCd[Index].Text) != 0)
                {
                    LBL_GyoNm[Index].Text = "";
                    //フォーカスセット
                    TXT_GyoCd[Index].Focus();
                    TXT_GyoCd_Enter(TXT_GyoCd[Index], new System.EventArgs());
                    IPPAN.Error_Msg("E503", 0, " ");
                    return;
                }

                //  13.07.03    ISV-TRUC    支払条件マスタを検索して、支払条件ＣＤ項目へ支払条件ＣＤをセットする。
                if (CKSI0130.Jyoken_Gyosya_Kensaku(TXT_GyoCd[Index].Text) != 0)
                {
                    LBL_GyoNm[Index].Text = "";
                    //フォーカスセット
                    TXT_GyoCd[Index].Focus();
                    TXT_GyoCd_Enter(TXT_GyoCd[Index], new System.EventArgs());
                    IPPAN.Error_Msg("E108", 0, " ");
                    return;
                }
                else
                {
                    TXT_SihaCd[Index].Text = CKSI0130.G_JYOKEN_AREA[0];
                    LBL_SihaNm[Index].Text = CKSI0130.G_JYOKEN_AREA[1];
                }
                //  End  13.07.03    ISV-TRUC

                LBL_GyoNm[Index].Text = CKSI0130.G_GYOSYA_AREA[0];
            }
            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
        }

        private void TXT_SihaCd_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            short Index = TXT_SihaCd.GetIndex((TextBox)eventSender);
            WK_SiharaiCd = TXT_SihaCd[Index].Text;
            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
        }

        private void TXT_SihaCd_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {
            short Index = TXT_SihaCd.GetIndex((TextBox)eventSender);
            if (Strings.Trim(TXT_SihaCd[Index].Text) == WK_SiharaiCd)
            {
                IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
                return;
            }
            if (IPPAN.C_Allspace(TXT_SihaCd[Index].Text) == 0)
            {
                LBL_SihaNm[Index].Text = "";
                TXT_SihaCd[Index].Focus();
                TXT_SihaCd_Enter(TXT_SihaCd[Index], new System.EventArgs());
                //必須項目入力エラー
                IPPAN.Error_Msg("E200", 0, " ");
                return;
            }
            if (IPPAN.Input_Check(TXT_SihaCd[Index].Text) == 1)
            {
                LBL_SihaNm[Index].Text = "";
                TXT_SihaCd[Index].Focus();
                TXT_SihaCd_Enter(TXT_SihaCd[Index], new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                return;
            }
            if (CKSI0130.Jyoken_Kensaku(TXT_SihaCd[Index].Text) != 0)
            {
                LBL_SihaNm[Index].Text = "";
                //フォーカスセット
                TXT_SihaCd[Index].Focus();
                TXT_SihaCd_Enter(TXT_SihaCd[Index], new System.EventArgs());
                //支払条件マスタが見つかりません
                IPPAN.Error_Msg("E108", 0, " ");
                return;
            }
            LBL_SihaNm[Index].Text = CKSI0130.G_JYOKEN_AREA[0];
            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
        }

        private void TXT_Tanka_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            short Index = TXT_Tanka.GetIndex((TextBox)eventSender);
            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
            if (IPPAN.C_Allspace(TXT_Tanka[Index].Text) == 0)
            {
                TXT_Tanka[Index].Text = "";
            }
            else
            {
                TXT_Tanka[Index].Text = "";
                if (Conversion.Val(CKSI0130.G_Tanka[Index]) != 0)
                {
                    TXT_Tanka[Index].Text = Strings.Trim(CKSI0130.G_Tanka[Index]);
                }
            }
            WK_Tanka = Strings.Trim(TXT_Tanka[Index].Text);
        }

        private void TXT_Tanka_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {
            short Index = TXT_Tanka.GetIndex((TextBox)eventSender);
            if (WK_Tanka != Strings.Trim(TXT_Tanka[Index].Text))
            {
                if (IPPAN.C_Allspace(TXT_Tanka[Index].Text) != 0)
                {
                    if (IPPAN.Numeric_Check(TXT_Tanka[Index].Text) == 1)
                    {
                        TXT_Tanka[Index].Focus();
                        TXT_Tanka_Enter(TXT_Tanka[Index], new System.EventArgs());
                        //入力エラー
                        IPPAN.Error_Msg("E202", 0, " ");
                        return;
                    }
                    if (Conversion.Val(TXT_Tanka[Index].Text) >= 100000000)
                    {
                        TXT_Tanka[Index].Focus();
                        TXT_Tanka_Enter(TXT_Tanka[Index], new System.EventArgs());
                        //入力エラー
                        IPPAN.Error_Msg("E202", 0, " ");
                        return;
                    }
                    CKSI0130.G_Tanka[Index] = C_COMMON.FormatToNum(IPPAN2.Numeric_Hensyu4(Strings.Trim(TXT_Tanka[Index].Text)), "#######0.000");
                    if (Conversion.Val(CKSI0130.G_Tanka[Index]) < 0)
                    {
                        TXT_Tanka[Index].Focus();
                        TXT_Tanka_Enter(TXT_Tanka[Index], new System.EventArgs());
                        //入力エラー
                        IPPAN.Error_Msg("E202", 0, " ");
                        return;
                    }
                }
                else
                {
                    CKSI0130.G_Tanka[Index] = "";
                    //フォーカスセット
                    TXT_Tanka[Index].Focus();
                    TXT_Tanka_Enter(TXT_Tanka[Index], new System.EventArgs());
                    //必須項目入力エラー
                    IPPAN.Error_Msg("E200", 0, "");
                    return;
                }
            }
            //カンマ編集
            if (IPPAN.C_Allspace(TXT_Tanka[Index].Text) != 0)
            {
                TXT_Tanka[Index].Text = IPPAN.Money_Hensyu(CKSI0130.G_Tanka[Index], "##,###,##0.000");
            }
            else
            {
                CKSI0130.G_Tanka[Index] = "";
                TXT_Tanka[Index].Text = "";
            }
            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
        }
    }
}

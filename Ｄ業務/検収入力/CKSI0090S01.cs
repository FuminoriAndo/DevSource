using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using System.Data.Odbc;
using CKLib.Syohizei;
using CKLib.Syohizei.Types;
using Common;
using System.Collections.Generic;

namespace Project1
{
    internal partial class FRM_CKSI0090S01 : System.Windows.Forms.Form
    {
        //**************************************************************'
        //                                                              '
        //   プログラム名　　　検収入力－検収内容入力画面処理           '
        //                                                              '
        //   更新履歴
        //   更新日付    Rev     修正者      内容
        //   03.01.15           NTIS浅田　　Ｓ／Ｏサーバ更新の為、VB4.0→VB6.0SP5へのバージョンアップ対応
        //   13.06.12           HIT 綱本    消費税対応                  '
        //**************************************************************'

        string M_Simeyy;
        string M_Simemm;
        string M_Kubun;
        string M_Kensyuyy;
        string M_Kensyumm;
        string M_Kensyudd;
        string M_Gyosyacd;
        string M_Hinmokucd2;
        string M_Bumon;
        string M_Himoku;
        string M_Utiwake;
        string M_Tanaban;
        string M_Jyokencd;
        string M_Tani;
        string M_Zeikbn;

        // ComboBox用
        // ComboBox用ID：ID
        private string P_COMBO_ID = "ID";
        // ComboBox用ID：NAME
        private string P_COMBO_NM = "NAME";

// 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Add -start
        /// <summary>
        /// 消費税リゾルバー
        /// </summary>
        private SyohizeiResolver syohizeiResolver = null;
// 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Add -end

        //単位マスタをコンボボックスに展開
        private short Tani_Kensaku()
        {
            short functionReturnValue = 1;
            string[] L_DATA = new string[3];

            DataTable tbl = null;
            DataTable cmbTbl = null;

            IPPAN.G_SQL = "Select TANICD,TANINM from TANI_MST order by TANINM";

            using (C_ODBC db = new C_ODBC())
            {
                try
                {
                    //DB接続
                    db.Connect();
                    //SQL実行
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

            //ComboBox用の設定
            cmbTbl = new DataTable();
            cmbTbl.Columns.Add(P_COMBO_ID, typeof(string));
            cmbTbl.Columns.Add(P_COMBO_NM, typeof(string));

            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                for (int j = 0; j < tbl.Columns.Count; j++)
                {
                    //項目セット
                    L_DATA[j] = tbl.Rows[i][j].ToString();
                }
                DataRow row = cmbTbl.NewRow();
                row[P_COMBO_ID] = L_DATA[0];
                row[P_COMBO_NM] = IPPAN.Space_Set(Strings.Mid(L_DATA[1], 1, 2), 3, 2);
                cmbTbl.Rows.Add(row);
            }

            cmbTbl.AcceptChanges();

            //コンボボックスのDataSourceにDataTableを割り当てる
            this.Cbo_Tani.DataSource = cmbTbl;

            //表示される値はDataTableのNAME列
            this.Cbo_Tani.DisplayMember = P_COMBO_NM;

            //対応する値はDataTableのID列
            this.Cbo_Tani.ValueMember = P_COMBO_ID;

            //初期状態は未選択状態
            this.Cbo_Tani.SelectedIndex = -1;

            functionReturnValue = 0;
            return functionReturnValue;
        }

        private void Gamen_Disp_New()
        {
            using (C_ODBC db = new C_ODBC())
            {
                try
                {
                    //DB接続
                    db.Connect();
                    //トランザクション開始
                    db.BeginTrans();
                    IPPAN.G_RET = GYOMU.Hakko_Kensaku(db, "UU");
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

            Lbl_Kensyuno.Text = Strings.Mid(GYOMU.G_SEQNO, 1, 2) + Strings.Mid(GYOMU.G_SEQNO, 5, 8);
            if (CKSI0090.Hizuke_Kensaku() == 1)
            {
                //日付管理マスタがみつかりません
                IPPAN.Error_Msg("E119", 0, " ");
                return;
            }
            //2013.09.03 DSK yoshida start
            //Txt_Simeyy.Text = Strings.Mid(CKSI0090.G_Nen, 3, 2);
            Txt_Simeyy.Text = Strings.Mid(CKSI0090.G_Nen, 1, 4);
            Txt_Simemm.Text = Strings.Mid(CKSI0090.G_Tuki, 1, 2);
            //Txt_Kensyuyy.Text = Strings.Mid(CKSI0090.G_Nen, 3, 2);
            Txt_Kensyuyy.Text = Strings.Mid(CKSI0090.G_Nen, 1, 4);
            Txt_Kensyumm.Text = Strings.Mid(CKSI0090.G_Tuki, 1, 2);
            //2013.09.03 DSK yoshida end
            switch (Txt_Kensyumm.Text)
            {
                case "01":
                case "03":
                case "05":
                case "07":
                case "08":
                case "10":
                case "12":
                    Txt_Kensyudd.Text = "31";
                    break;
                case "04":
                case "06":
                case "09":
                case "11":
                    Txt_Kensyudd.Text = "30";
                    break;
                case "02":
                    if (Convert.ToInt16(CKSI0090.G_Nen) % 4 == 0)
                    {
                        Txt_Kensyudd.Text = "29";
                    }
                    else
                    {
                        Txt_Kensyudd.Text = "28";
                    }
                    break;
            }
            //Txt_Zeikbn.Text = "";                                                                                                     13.06.12 DELETE
            //Lbl_Zeiritu.Text = IPPAN2.NIBYTE_HENKAN(Convert.ToString(CKSI0090.Syohizei_Kensaku(Txt_Zeikbn.Text, "1")), 1) + "％";     13.06.12 DELETE

// 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Mod -start
            var target = syohizeiResolver.GetSyohizeiDTO(SyohizeiSearchConditionType.DispZeiKbn, " ");
            if (target != null)
            {
                Txt_Zeikbn.Text = target.DispZeiKbn;
                Lbl_Zeiritu.Text = target.DispSyohizei;
                if (target.ZeirituHojoMap != null)
                {
                    CMB_ZEIRITUHOJO.DataSource = null;
                    CMB_ZEIRITUHOJO.Items.Clear();
                    var comboBoxItems = new List<ComboBoxItem>();
                    foreach (var item in target.ZeirituHojoMap)
                    {
                        comboBoxItems.Add(new ComboBoxItem(item.Key, item.Key + ":" + item.Value));
                    }
                    if (target.ZeirituHojoMap.Count >= 1)
                    {
                        CMB_ZEIRITUHOJO.Enabled = true;
                    }
                    else
                    {
                        CMB_ZEIRITUHOJO.Enabled = false;
                    }
                    CMB_ZEIRITUHOJO.DataSource = comboBoxItems;
                    CMB_ZEIRITUHOJO.ValueMember = "Key";
                    CMB_ZEIRITUHOJO.DisplayMember = "Value";
                    if (!string.IsNullOrEmpty(target.InitDispZeirituHojoKbn))
                    {
                        CMB_ZEIRITUHOJO.SelectedValue = target.InitDispZeirituHojoKbn;
                    }
                }
                else
                {
                    CMB_ZEIRITUHOJO.DataSource = null;
                    CMB_ZEIRITUHOJO.Items.Clear();
                }
            }
            ////13.06.12 tsunamoto 消費税対応 start

            //IPPAN2.ZeiInfo zeiinfo = IPPAN2.Get_Syohizei("");
            //Txt_Zeikbn.Text  = zeiinfo.zeikbn1;
            //Lbl_Zeiritu.Text = zeiinfo.zeihyoji;

            ////13.06.12 tsunamoto 消費税対応 end
// 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Mod -end

            CKSI0090.G_Suryo = "0";
            CKSI0090.G_Tanka = "0.000";
            CKSI0090.G_Kingaku = "0";
            CKSI0090.G_Syohizei = "0";
            CKSI0090.G_Gkingaku = "0";
        }

        private void Gamen_Disp(int i)
        {
            Lbl_Kensyuno.Text = Strings.Mid(CKSI0090.G_Kensyu_Area[i, 0], 1, 2) + Strings.Mid(CKSI0090.G_Kensyu_Area[i, 0], 5, 8);
            //2013.09.03 DSK yoshida start
            //Txt_Simeyy.Text = Strings.Mid(CKSI0090.G_Kensyu_Area[i, 1], 3, 2);
            Txt_Simeyy.Text = Strings.Mid(CKSI0090.G_Kensyu_Area[i, 1], 1, 4);
            //2013.09.03 DSK yoshida end
            Txt_Simemm.Text = Strings.Mid(CKSI0090.G_Kensyu_Area[i, 1], 5, 2);
            Txt_Kubun.Text = Strings.Mid(CKSI0090.G_Kensyu_Area[i, 2], 1, 1);
            if (Txt_Kubun.Text == "1")
            {
                Txt_Bumon.Enabled = false;
            }
            else
            {
                Txt_Bumon.Enabled = true;
            }
            //2013.09.03 DSK yoshida start
            //Txt_Kensyuyy.Text = Strings.Mid(CKSI0090.G_Kensyu_Area[i, 3], 3, 2);
            Txt_Kensyuyy.Text = Strings.Mid(CKSI0090.G_Kensyu_Area[i, 3], 1, 4);
            //2013.09.03 DSK yoshida end
            Txt_Kensyumm.Text = Strings.Mid(CKSI0090.G_Kensyu_Area[i, 3], 5, 2);
            Txt_Kensyudd.Text = Strings.Mid(CKSI0090.G_Kensyu_Area[i, 3], 7, 2);
            Txt_Gyosyacd.Text = Strings.Mid(CKSI0090.G_Kensyu_Area[i, 4], 1, 4);
            Lbl_Gyosyanm.Text = Strings.Mid(CKSI0090.G_Kensyu_Area[i, 5], 1, 20);
            TXT_Hinmokucd2.Text = Strings.Mid(CKSI0090.G_Kensyu_Area[i, 27], 1, 4);
            Lbl_Hinmokunm2.Text = Strings.Mid(CKSI0090.G_Kensyu_Area[i, 28], 1, 20);
            Txt_Bumon.Text = Strings.Trim(Strings.Mid(CKSI0090.G_Kensyu_Area[i, 6], 1, 2));
            Lbl_Bumonnm.Text = Strings.Mid(CKSI0090.G_Kensyu_Area[i, 7], 1, 10);
            Txt_Himoku.Text = Strings.Mid(CKSI0090.G_Kensyu_Area[i, 8], 1, 2);
            Lbl_Himokunm.Text = Strings.Mid(CKSI0090.G_Kensyu_Area[i, 9], 1, 10);
            Txt_Utiwake.Text = Strings.Mid(CKSI0090.G_Kensyu_Area[i, 10], 1, 2);
            Lbl_Utiwakenm.Text = Strings.Mid(CKSI0090.G_Kensyu_Area[i, 11], 1, 10);
            Txt_Tanaban.Text = Strings.Mid(CKSI0090.G_Kensyu_Area[i, 12], 1, 2);
            Lbl_Hinmokunm.Text = Strings.Mid(CKSI0090.G_Kensyu_Area[i, 13], 1, 20);
            Txt_Jyokencd.Text = Strings.Trim(Strings.Mid(CKSI0090.G_Kensyu_Area[i, 14], 1, 2));
            Lbl_Jyokennm.Text = Strings.Mid(CKSI0090.G_Kensyu_Area[i, 15], 1, 23);

            Cbo_Tani.Text = Strings.Trim(Strings.Mid(CKSI0090.G_Kensyu_Area[i, 16], 1, 2));
            Txt_Suryo.Text = IPPAN.Money_Hensyu(CKSI0090.G_Kensyu_Area[i, 17], "##,###,##0");
            CKSI0090.G_Suryo = IPPAN.Money_Hensyu(CKSI0090.G_Kensyu_Area[i, 17], "#######0");
            Txt_Tanka.Text = IPPAN.Money_Hensyu(CKSI0090.G_Kensyu_Area[i, 18], "##,###,##0.000");
            CKSI0090.G_Tanka = IPPAN.Money_Hensyu(CKSI0090.G_Kensyu_Area[i, 18], "#######0.000");
            Txt_Kingaku.Text = IPPAN.Money_Hensyu(CKSI0090.G_Kensyu_Area[i, 19], "#,###,###,##0");
            CKSI0090.G_Kingaku = IPPAN.Money_Hensyu(CKSI0090.G_Kensyu_Area[i, 19], "#########0");

            //if (Strings.Mid(CKSI0090.G_Kensyu_Area[i, 20], 1, 1) == "1")                                                              13.06.12 DELETE
            //{                                                                                                                         13.06.12 DELETE
            //    Txt_Zeikbn.Text = "";                                                                                                 13.06.12 DELETE
            //}                                                                                                                         13.06.12 DELETE
            //else                                                                                                                      13.06.12 DELETE
            //{                                                                                                                         13.06.12 DELETE
            //    Txt_Zeikbn.Text = "1";                                                                                                13.06.12 DELETE
            //}                                                                                                                         13.06.12 DELETE
            //Lbl_Zeiritu.Text = IPPAN2.NIBYTE_HENKAN(Convert.ToString(CKSI0090.Syohizei_Kensaku(Txt_Zeikbn.Text, "1")), 1) + "％";     13.06.12 DELETE

// 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Mod -start
            var target = syohizeiResolver.GetSyohizeiDTO(SyohizeiSearchConditionType.InfoZeiKbn, Strings.Mid(CKSI0090.G_Kensyu_Area[i, 20], 1, 1));
            if (target != null)
            {
                Txt_Zeikbn.Text = target.DispZeiKbn;
                Lbl_Zeiritu.Text = target.DispSyohizei;
                if (target.ZeirituHojoMap != null)
                {
                    CMB_ZEIRITUHOJO.DataSource = null;
                    CMB_ZEIRITUHOJO.Items.Clear();
                    var comboBoxItems = new List<ComboBoxItem>();
                    foreach (var item in target.ZeirituHojoMap)
                    {
                        comboBoxItems.Add(new ComboBoxItem(item.Key, item.Key + ":" + item.Value));
                    }
                    if (target.ZeirituHojoMap.Count >= 1)
                    {
                        CMB_ZEIRITUHOJO.Enabled = true;
                    }
                    else
                    {
                        CMB_ZEIRITUHOJO.Enabled = false;
                    }
                    CMB_ZEIRITUHOJO.DataSource = comboBoxItems;
                    CMB_ZEIRITUHOJO.ValueMember = "Key";
                    CMB_ZEIRITUHOJO.DisplayMember = "Value";
                }
                else
                {
                    CMB_ZEIRITUHOJO.DataSource = null;
                    CMB_ZEIRITUHOJO.Items.Clear();
                }
            }
            ////13.06.12 tsunamoto 消費税対応 start
            //IPPAN2.ZeiInfo zeiinfo = IPPAN2.Get_Syohizei(2, Strings.Mid(CKSI0090.G_Kensyu_Area[i, 20], 1, 1));
            //Txt_Zeikbn.Text = zeiinfo.zeikbn1;
            //Lbl_Zeiritu.Text = zeiinfo.zeihyoji;
            ////13.06.12 tsunamoto 消費税対応 end
// 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Mod -end
            
            Txt_Syohizei.Text = IPPAN.Money_Hensyu(CKSI0090.G_Kensyu_Area[i, 21], "#,###,###,##0");
            CKSI0090.G_Syohizei = IPPAN.Money_Hensyu(CKSI0090.G_Kensyu_Area[i, 21], "#########0");
            Lbl_Gkingaku.Text = IPPAN.Money_Hensyu(CKSI0090.G_Kensyu_Area[i, 22], "#,###,###,##0");
            CKSI0090.G_Gkingaku = IPPAN.Money_Hensyu(CKSI0090.G_Kensyu_Area[i, 22], "#########0");
        }

        private short Gamen_Check()
        {
            short functionReturnValue = 1;
            string L_Kamoku = null;

            //締年月
            if (IPPAN.C_Allspace(Txt_Simeyy.Text) == 0)
            {
                Txt_Simeyy.Focus();
                Txt_Simeyy_Enter(Txt_Simeyy, new System.EventArgs());
                //必須項目入力エラー
                IPPAN.Error_Msg("E200", 0, " ");
                return functionReturnValue;
            }
            if (IPPAN.C_Allspace(Txt_Simemm.Text) == 0)
            {
                Txt_Simemm.Focus();
                Txt_Simemm_Enter(Txt_Simemm, new System.EventArgs());
                //必須項目入力エラー
                IPPAN.Error_Msg("E200", 0, " ");
                return functionReturnValue;
            }
            IPPAN.G_WYMD = Txt_Simeyy.Text + Txt_Simemm.Text + "01";
            //2013.09.03 DSK yoshida start
            //if (IPPAN.Date_Henkan(ref IPPAN.G_WYMD) == 1)
            //{
            //    Txt_Simemm.Focus();
            //    Txt_Simemm_Enter(Txt_Simemm, new System.EventArgs());
            //    //日付入力エラー
            //    IPPAN.Error_Msg("E201", 0, " ");
            //    return functionReturnValue;
            //}
            //2013.09.03 DSK yoshida end
            if (IPPAN.Date_Check(IPPAN.G_WYMD) == 1)
            {
                Txt_Simemm.Focus();
                Txt_Simemm_Enter(Txt_Simemm, new System.EventArgs());
                //日付入力エラー
                IPPAN.Error_Msg("E201", 0, " ");
                return functionReturnValue;
            }

            //区分
            if (IPPAN.C_Allspace(Txt_Kubun.Text) == 0)
            {
                Txt_Kubun.Focus();
                Txt_Kubun_Enter(Txt_Kubun, new System.EventArgs());
                //必須項目入力エラー
                IPPAN.Error_Msg("E200", 0, " ");
                return functionReturnValue;
            }
            if (IPPAN.Input_Check(Txt_Kubun.Text) == 1)
            {
                Txt_Kubun.Focus();
                Txt_Kubun_Enter(Txt_Kubun, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                return functionReturnValue;
            }

            if (Txt_Kubun.Text != "0" && Txt_Kubun.Text != "1")
            {
                Txt_Kubun.Focus();
                Txt_Kubun_Enter(Txt_Kubun, new System.EventArgs());
                //「０」か「１」を入力して下さい
                IPPAN.Error_Msg("E702", 0, " ");
                return functionReturnValue;
            }

            //検収日
            if (IPPAN.C_Allspace(Txt_Kensyuyy.Text) == 0)
            {
                Txt_Kensyuyy.Focus();
                Txt_Kensyuyy_Enter(Txt_Kensyuyy, new System.EventArgs());
                //必須項目入力エラー
                IPPAN.Error_Msg("E200", 0, " ");
                return functionReturnValue;
            }
            if (IPPAN.C_Allspace(Txt_Kensyumm.Text) == 0)
            {
                Txt_Kensyumm.Focus();
                Txt_Kensyumm_Enter(Txt_Kensyumm, new System.EventArgs());
                //必須項目入力エラー
                IPPAN.Error_Msg("E200", 0, " ");
                return functionReturnValue;
            }
            if (IPPAN.C_Allspace(Txt_Kensyudd.Text) == 0)
            {
                Txt_Kensyudd.Focus();
                Txt_Kensyudd_Enter(Txt_Kensyudd, new System.EventArgs());
                //必須項目入力エラー
                IPPAN.Error_Msg("E200", 0, " ");
                return functionReturnValue;
            }
            IPPAN.G_WYMD = Txt_Kensyuyy.Text + Txt_Kensyumm.Text + Txt_Kensyudd.Text;
            //2013.09.03 DSK yoshida start
            //if (IPPAN.Date_Henkan(ref IPPAN.G_WYMD) == 1)
            //{
            //    Txt_Kensyudd.Focus();
            //    Txt_Kensyudd_Enter(Txt_Kensyudd, new System.EventArgs());
            //    //日付入力エラー
            //    IPPAN.Error_Msg("E201", 0, " ");
            //    return functionReturnValue;
            //}
            //2013.09.03 DSK yoshida end
            if (IPPAN.Date_Check(IPPAN.G_WYMD) == 1)
            {
                Txt_Kensyudd.Focus();
                Txt_Kensyudd_Enter(Txt_Kensyudd, new System.EventArgs());
                //日付入力エラー
                IPPAN.Error_Msg("E201", 0, " ");
                return functionReturnValue;
            }

            //業者
            if (IPPAN.C_Allspace(Txt_Gyosyacd.Text) == 0)
            {
                Lbl_Gyosyanm.Text = "";
                Txt_Gyosyacd.Focus();
                Txt_Gyosyacd_Enter(Txt_Gyosyacd, new System.EventArgs());
                //必須項目入力エラー
                IPPAN.Error_Msg("E200", 0, " ");
                return functionReturnValue;
            }
            if (IPPAN.Input_Check(Txt_Gyosyacd.Text) == 1)
            {
                Lbl_Gyosyanm.Text = "";
                Txt_Gyosyacd.Focus();
                Txt_Gyosyacd_Enter(Txt_Gyosyacd, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                return functionReturnValue;
            }

            if (CKSI0090.Gyosya_Kensaku(Txt_Gyosyacd.Text) == 1)
            {
                Lbl_Gyosyanm.Text = "";
                Txt_Gyosyacd.Focus();
                Txt_Gyosyacd_Enter(Txt_Gyosyacd, new System.EventArgs());
                //業者マスタが見つかりません
                IPPAN.Error_Msg("E503", 0, " ");
                return functionReturnValue;

            }
            Lbl_Gyosyanm.Text = CKSI0090.G_Gyosyanm;

            //品目
            if (IPPAN.C_Allspace(TXT_Hinmokucd2.Text) == 0)
            {
                Lbl_Hinmokunm2.Text = "";
                TXT_Hinmokucd2.Focus();
                Txt_Hinmokucd2_Enter(TXT_Hinmokucd2, new System.EventArgs());
                //必須項目入力エラー
                IPPAN.Error_Msg("E200", 0, " ");
                return functionReturnValue;
            }
            if (IPPAN.Input_Check(TXT_Hinmokucd2.Text) == 1)
            {
                Lbl_Hinmokunm2.Text = "";
                TXT_Hinmokucd2.Focus();
                Txt_Hinmokucd2_Enter(TXT_Hinmokucd2, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                return functionReturnValue;
            }

            if (CKSI0090.SHinmoku_Kensaku(TXT_Hinmokucd2.Text) == 1)
            {
                Lbl_Hinmokunm2.Text = "";
                TXT_Hinmokucd2.Focus();
                Txt_Hinmokucd2_Enter(TXT_Hinmokucd2, new System.EventArgs());
                //副資材品目マスタが見つかりません
                IPPAN.Error_Msg("E701", 0, " ");
                return functionReturnValue;
            }
            Lbl_Hinmokunm2.Text = CKSI0090.G_Hinmokunm2;
            Txt_Himoku.Text = CKSI0090.G_Himoku;
            Txt_Utiwake.Text = CKSI0090.G_Utiwake;
            Txt_Tanaban.Text = CKSI0090.G_Tanaban;

            //部門
            if (IPPAN.C_Allspace(Txt_Bumon.Text) == 0)
            {
                Lbl_Bumonnm.Text = "";
                Txt_Bumon.Focus();
                Txt_Bumon_Enter(Txt_Bumon, new System.EventArgs());
                //必須項目入力エラー
                IPPAN.Error_Msg("E200", 0, " ");
                return functionReturnValue;
            }
            if (IPPAN.Input_Check(Txt_Bumon.Text) == 1)
            {
                Lbl_Bumonnm.Text = "";
                Txt_Bumon.Focus();
                Txt_Bumon_Enter(Txt_Bumon, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                return functionReturnValue;
            }

            if (Txt_Kubun.Text == "1")
            {
                //貯蔵品
                L_Kamoku = "111";
                Lbl_Bumonnm.Text = "製鋼共通　　　　　　";
            }
            else
            {
                //製造費
// 2015.01.21 yoshitake OBIC7仕訳連携対応(step3) start.
                if (C_COMMON.GetKouteiNM(Txt_Bumon.Text, out CKSI0090.G_Kamokunm2) == 1)
                //L_Kamoku = "110";
                //if (CKSI0090.Kamoku_Kensaku(L_Kamoku, Txt_Bumon.Text) == 1)
// 2015.01.21 yoshitake OBIC7仕訳連携対応(step3) end.
                {
                    Lbl_Bumonnm.Text = "";
                    Txt_Bumon.Focus();
                    Txt_Bumon_Enter(Txt_Bumon, new System.EventArgs());
// 2015.01.21 yoshitake OBIC7仕訳連携対応(step3) start.
                    //工程マスタが見つかりません
                    IPPAN.Error_Msg("E547", 0, " ");
                    //科目マスタが見つかりません
                    //IPPAN.Error_Msg("E520", 0, " ");
// 2015.01.21 yoshitake OBIC7仕訳連携対応(step3) end.
                    return functionReturnValue;
                }
                Lbl_Bumonnm.Text = CKSI0090.G_Kamokunm2;
            }

            //費目
            if (IPPAN.C_Allspace(Txt_Himoku.Text) == 0)
            {
                Lbl_Himokunm.Text = "";
                Txt_Himoku.Focus();
                Txt_Himoku_Enter(Txt_Himoku, new System.EventArgs());
                //必須項目入力エラー
                IPPAN.Error_Msg("E200", 0, " ");
                return functionReturnValue;
            }
            if (IPPAN.Input_Check(Txt_Himoku.Text) == 1)
            {
                Lbl_Himokunm.Text = "";
                Txt_Himoku.Focus();
                Txt_Himoku_Enter(Txt_Himoku, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                return functionReturnValue;
            }

            if (Txt_Kubun.Text == "0")
            {
// 2015.02.25 nishi OBIC7仕訳連携 Step3対応 start.
                if (CKSI0090.Kamoku_Kensaku("110", Txt_Himoku.Text) == 1)
                {
                    Lbl_Himokunm.Text = "";
                    Txt_Himoku.Focus();
                    Txt_Himoku_Enter(Txt_Himoku, new System.EventArgs());
                    //科目マスタが見つかりません
                    IPPAN.Error_Msg("E520", 0, " ");
                    return functionReturnValue;
                }
                Lbl_Himokunm.Text = CKSI0090.G_Kamokunm2;
                //if (CKSI0090.Himoku_Kensaku(Txt_Himoku.Text) == 1)
                //{
                //    Lbl_Himokunm.Text = "";
                //    Txt_Himoku.Focus();
                //    Txt_Himoku_Enter(Txt_Himoku, new System.EventArgs());
                //    //費目マスタが見つかりません
                //    IPPAN.Error_Msg("E521", 0, " ");
                //    return functionReturnValue;
                //}
                //Lbl_Himokunm.Text = CKSI0090.G_Himokunm;
// 2015.02.25 nishi OBIC7仕訳連携 Step3対応 end.
            }
            else
            {
                if (CKSI0090.Kamoku_Kensaku("111", Txt_Himoku.Text) == 1)
                {
                    Lbl_Himokunm.Text = "";
                    Txt_Himoku.Focus();
                    Txt_Himoku_Enter(Txt_Himoku, new System.EventArgs());
                    //科目マスタが見つかりません
                    IPPAN.Error_Msg("E520", 0, " ");
                    return functionReturnValue;
                }
                Lbl_Himokunm.Text = CKSI0090.G_Kamokunm2;
            }

            //内訳
            if (IPPAN.C_Allspace(Txt_Utiwake.Text) == 0)
            {
                Lbl_Utiwakenm.Text = "";
                Txt_Utiwake.Focus();
                Txt_Utiwake_Enter(Txt_Utiwake, new System.EventArgs());
                //必須項目入力エラー
                IPPAN.Error_Msg("E200", 0, " ");
                return functionReturnValue;
            }
            if (IPPAN.Input_Check(Txt_Utiwake.Text) == 1)
            {
                Lbl_Utiwakenm.Text = "";
                Txt_Utiwake.Focus();
                Txt_Utiwake_Enter(Txt_Utiwake, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                return functionReturnValue;
            }

            if (CKSI0090.Utiwake_Kensaku(Txt_Himoku.Text, Txt_Utiwake.Text) == 1)
            {
                Lbl_Utiwakenm.Text = "";
                Txt_Utiwake.Focus();
                Txt_Utiwake_Enter(Txt_Utiwake, new System.EventArgs());
                //製造費、費目内訳マスタが見つかりません
                IPPAN.Error_Msg("E522", 0, " ");
                return functionReturnValue;
            }
            Lbl_Utiwakenm.Text = CKSI0090.G_Utiwakenm;

            //棚番
            if (IPPAN.C_Allspace(Txt_Tanaban.Text) == 0)
            {
                Lbl_Hinmokunm.Text = "";
                Txt_Tanaban.Focus();
                Txt_Tanaban_Enter(Txt_Tanaban, new System.EventArgs());
                //必須項目入力エラー
                IPPAN.Error_Msg("E200", 0, " ");
                return functionReturnValue;
            }
            if (IPPAN.Input_Check(Txt_Tanaban.Text) == 1)
            {
                Lbl_Hinmokunm.Text = "";
                Txt_Tanaban.Focus();
                Txt_Tanaban_Enter(Txt_Tanaban, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                return functionReturnValue;
            }

            if (CKSI0090.Hinmoku_Kensaku(Txt_Himoku.Text, Txt_Utiwake.Text, Txt_Tanaban.Text) == 1)
            {
                Lbl_Hinmokunm.Text = "";
                Txt_Tanaban.Focus();
                Txt_Tanaban_Enter(Txt_Tanaban, new System.EventArgs());
                //品目マスタが見つかりません
                IPPAN.Error_Msg("E107", 0, " ");
                return functionReturnValue;
            }
            Lbl_Hinmokunm.Text = CKSI0090.G_Hinmokunm;

            //支払条件
            if (IPPAN.C_Allspace(Txt_Jyokencd.Text) == 0)
            {
                Lbl_Jyokennm.Text = "";
                Txt_Jyokencd.Focus();
                Txt_Jyokencd_Enter(Txt_Jyokencd, new System.EventArgs());
                //必須項目入力エラー
                IPPAN.Error_Msg("E200", 0, " ");
                return functionReturnValue;
            }
            if (IPPAN.Input_Check(Txt_Jyokencd.Text) == 1)
            {
                Lbl_Jyokennm.Text = "";
                Txt_Jyokencd.Focus();
                Txt_Jyokencd_Enter(Txt_Jyokencd, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                return functionReturnValue;
            }

            if (CKSI0090.Jyoken_Kensaku(Txt_Jyokencd.Text) == 1)
            {
                Lbl_Jyokennm.Text = "";
                Txt_Jyokencd.Focus();
                Txt_Jyokencd_Enter(Txt_Jyokencd, new System.EventArgs());
                //支払条件マスタが見つかりません
                IPPAN.Error_Msg("E108", 0, " ");
                return functionReturnValue;
            }
            Lbl_Jyokennm.Text = CKSI0090.G_Jyokennm;

            //単位
            if (IPPAN.C_Allspace(Cbo_Tani.Text) == 0)
            {
                Cbo_Tani.Focus();
                Cbo_Tani_Enter(Cbo_Tani, new System.EventArgs());
                //必須項目入力エラー
                IPPAN.Error_Msg("E200", 0, " ");
                return functionReturnValue;
            }
            if (IPPAN.Input_Check(Cbo_Tani.Text) == 1)
            {
                Cbo_Tani.Focus();
                Cbo_Tani_Enter(Cbo_Tani, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                return functionReturnValue;
            }

            //数量
            if (Conversion.Val(CKSI0090.G_Suryo) >= 100000000)
            {
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                Txt_Suryo.Focus();
                return functionReturnValue;
            }

            //単価
            if (Conversion.Val(CKSI0090.G_Tanka) >= 100000000)
            {
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                Txt_Tanka.Focus();
                return functionReturnValue;
            }

            //金額
            if (Conversion.Val(CKSI0090.G_Kingaku) >= 10000000000.0)
            {
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                Txt_Kingaku.Focus();
                return functionReturnValue;
            }

            //税区分
            if (IPPAN.Input_Check(Txt_Zeikbn.Text) == 1)
            {
                Lbl_Zeiritu.Text = "";
                Txt_Zeikbn.Focus();
                Txt_Zeikbn_Enter(Txt_Zeikbn, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                return functionReturnValue;
            }

            //13.06.12 DELETE start
            //if (IPPAN.C_Allspace(Txt_Zeikbn.Text) == 0)
            //{
            //    Lbl_Zeiritu.Text = "５％";
            //}
            //else
            //{
            //    Lbl_Zeiritu.Text = "３％";
            //}
            //13.06.12 DELETE end

// 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Mod -start
            var target = syohizeiResolver.GetSyohizeiDTO(SyohizeiSearchConditionType.DispZeiKbn, Txt_Zeikbn.Text);
            if (target != null)
            {
                Lbl_Zeiritu.Text = target.DispSyohizei;
                if (target.ZeirituHojoMap != null)
                {
                    CMB_ZEIRITUHOJO.DataSource = null;
                    CMB_ZEIRITUHOJO.Items.Clear();
                    var comboBoxItems = new List<ComboBoxItem>();
                    foreach (var item in target.ZeirituHojoMap)
                    {
                        comboBoxItems.Add(new ComboBoxItem(item.Key, item.Key + ":" + item.Value));
                    }
                    if (target.ZeirituHojoMap.Count >= 1)
                    {
                        CMB_ZEIRITUHOJO.Enabled = true;
                    }
                    else
                    {
                        CMB_ZEIRITUHOJO.Enabled = false;
                    }
                    CMB_ZEIRITUHOJO.DataSource = comboBoxItems;
                    CMB_ZEIRITUHOJO.ValueMember = "Key";
                    CMB_ZEIRITUHOJO.DisplayMember = "Value";
                }
                else
                {
                    CMB_ZEIRITUHOJO.DataSource = null;
                    CMB_ZEIRITUHOJO.Items.Clear();
                }
            }

            ////13.06.12 tsunamoto 消費税対応 start
            //IPPAN2.ZeiInfo zeiinfo = IPPAN2.Get_Syohizei(1, Txt_Zeikbn.Text);
            //Lbl_Zeiritu.Text = zeiinfo.zeihyoji;
            ////13.06.12 tsunamoto 消費税対応 end
// 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Mod -end

            //消費税
            if (Conversion.Val(CKSI0090.G_Syohizei) >= 10000000000.0)
            {
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                Txt_Syohizei.Focus();
                return functionReturnValue;
            }

            functionReturnValue = 0;
            return functionReturnValue;
        }

        private void Trn_Set(int i)
        {
            if (string.Compare(Strings.Mid(Lbl_Kensyuno.Text, 3, 2), "95") >= 0)
            {
                CKSI0090.G_Kensyu_Area[i, 0] = Strings.Mid(Lbl_Kensyuno.Text, 1, 2) + "19" + Strings.Mid(Lbl_Kensyuno.Text, 3, 8);
            }
            else
            {
                CKSI0090.G_Kensyu_Area[i, 0] = Strings.Mid(Lbl_Kensyuno.Text, 1, 2) + "20" + Strings.Mid(Lbl_Kensyuno.Text, 3, 8);
            }
            //2013.09.03 DSK yoshida start
            //if (string.Compare(Txt_Simeyy.Text, "95") >= 0)
            //{
            //    CKSI0090.G_Kensyu_Area[i, 1] = "19" + Txt_Simeyy.Text + Txt_Simemm.Text;
            //}
            //else
            //{
            //    CKSI0090.G_Kensyu_Area[i, 1] = "20" + Txt_Simeyy.Text + Txt_Simemm.Text;
            //}
            CKSI0090.G_Kensyu_Area[i, 1] = Txt_Simeyy.Text + Txt_Simemm.Text;
            CKSI0090.G_Kensyu_Area[i, 2] = IPPAN.Space_Set(Txt_Kubun.Text, 1, 1);
            //if (string.Compare(Txt_Kensyuyy.Text, "95") >= 0)
            //{
            //    CKSI0090.G_Kensyu_Area[i, 3] = "19" + Txt_Kensyuyy.Text + Txt_Kensyumm.Text + Txt_Kensyudd.Text;
            //}
            //else
            //{
            //    CKSI0090.G_Kensyu_Area[i, 3] = "20" + Txt_Kensyuyy.Text + Txt_Kensyumm.Text + Txt_Kensyudd.Text;
            //}
            CKSI0090.G_Kensyu_Area[i, 3] = Txt_Kensyuyy.Text + Txt_Kensyumm.Text + Txt_Kensyudd.Text;
            //2013.09.03 DSK yoshida end
            CKSI0090.G_Kensyu_Area[i, 4] = IPPAN.Space_Set(Txt_Gyosyacd.Text, 4, 1);
            CKSI0090.G_Kensyu_Area[i, 5] = IPPAN.Space_Set(Lbl_Gyosyanm.Text, 20, 2);
            CKSI0090.G_Kensyu_Area[i, 6] = IPPAN.Space_Set(Txt_Bumon.Text, 2, 1);
            CKSI0090.G_Kensyu_Area[i, 7] = IPPAN.Space_Set(Lbl_Bumonnm.Text, 10, 2);
            CKSI0090.G_Kensyu_Area[i, 8] = IPPAN.Space_Set(Txt_Himoku.Text, 2, 1);
            CKSI0090.G_Kensyu_Area[i, 9] = IPPAN.Space_Set(Lbl_Himokunm.Text, 10, 2);
            CKSI0090.G_Kensyu_Area[i, 10] = IPPAN.Space_Set(Txt_Utiwake.Text, 2, 1);
            CKSI0090.G_Kensyu_Area[i, 11] = IPPAN.Space_Set(Lbl_Utiwakenm.Text, 10, 2);
            CKSI0090.G_Kensyu_Area[i, 12] = IPPAN.Space_Set(Txt_Tanaban.Text, 2, 1);
            CKSI0090.G_Kensyu_Area[i, 13] = IPPAN.Space_Set(Lbl_Hinmokunm.Text, 20, 2);
            CKSI0090.G_Kensyu_Area[i, 14] = IPPAN.Space_Set(Txt_Jyokencd.Text, 2, 1);
            CKSI0090.G_Kensyu_Area[i, 15] = IPPAN.Space_Set(Lbl_Jyokennm.Text, 23, 2);
            CKSI0090.G_Kensyu_Area[i, 16] = IPPAN.Space_Set(Cbo_Tani.Text, 2, 2);
            CKSI0090.G_Kensyu_Area[i, 17] = CKSI0090.G_Suryo;
            CKSI0090.G_Kensyu_Area[i, 18] = CKSI0090.G_Tanka;
            CKSI0090.G_Kensyu_Area[i, 19] = CKSI0090.G_Kingaku;
            //13.06.14 delete start 
            //if (Txt_Zeikbn.Text == "1")
            //{
            //    CKSI0090.G_Kensyu_Area[i, 20] = " ";
            //}
            //else
            //{
            //    CKSI0090.G_Kensyu_Area[i, 20] = "1";
            //}
            //13.06.14 delete end

// 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Mod -start
            var target = syohizeiResolver.GetSyohizeiDTO(SyohizeiSearchConditionType.DispZeiKbn, Txt_Zeikbn.Text);
            if (target != null)
            {
                CKSI0090.G_Kensyu_Area[i, 20] = target.InfoZeiKbn;
            }
            ////13.06.14 tsunamoto 消費税対応 start
            //IPPAN2.ZeiInfo zeiinfo = IPPAN2.Get_Syohizei(1,Txt_Zeikbn.Text);
            //CKSI0090.G_Kensyu_Area[i, 20] = zeiinfo.zeikbn2;
            ////13.06.14 tsunamoto 消費税対応 end
// 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Mod -end

            CKSI0090.G_Kensyu_Area[i, 21] = CKSI0090.G_Syohizei;
            CKSI0090.G_Kensyu_Area[i, 22] = CKSI0090.G_Gkingaku;
            CKSI0090.G_Kensyu_Area[i, 23] = "1";
            CKSI0090.G_Kensyu_Area[i, 24] = "            ";
            CKSI0090.G_Kensyu_Area[i, 25] = Convert.ToString(0);
            CKSI0090.G_Kensyu_Area[i, 27] = IPPAN.Space_Set(TXT_Hinmokucd2.Text, 4, 1);
            CKSI0090.G_Kensyu_Area[i, 28] = IPPAN.Space_Set(Lbl_Hinmokunm2.Text, 20, 2);
            CKSI0090.G_Kensyu_Area[i, 29] = " ";
// 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Add -start
            if (CMB_ZEIRITUHOJO.SelectedValue != null)
            {
                CKSI0090.G_Kensyu_Area[i, 30] = CMB_ZEIRITUHOJO.SelectedValue.ToString();
            }
// 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Add -end

        }

        private void Enable_Control()
        {
            //オーナーフォームを取得
            FRM_CKSI0090M fmOwner = (FRM_CKSI0090M)this.Owner;
            if (fmOwner.Opt_Shori[0].Checked == true)
            {
                Txt_Gyosyacd.Enabled = false;
                TXT_Hinmokucd2.Enabled = false;
                Txt_Himoku.Enabled = false;
                Txt_Utiwake.Enabled = false;
                Txt_Tanaban.Enabled = false;
                CMD_Button[2].Visible = false;
            }
            else if (fmOwner.Opt_Shori[1].Checked == true)
            {
                Txt_Himoku.Enabled = false;
                Txt_Utiwake.Enabled = false;
                Txt_Tanaban.Enabled = false;
                CMD_Button[2].Visible = false;
            }
            else if (fmOwner.Opt_Shori[2].Checked == true)
            {
                Txt_Simeyy.Enabled = false;
                Txt_Simemm.Enabled = false;
                Txt_Kubun.Enabled = false;
                Txt_Kensyuyy.Enabled = false;
                Txt_Kensyumm.Enabled = false;
                Txt_Kensyudd.Enabled = false;
                Txt_Gyosyacd.Enabled = false;
                TXT_Hinmokucd2.Enabled = false;
                Txt_Bumon.Enabled = false;
                Txt_Himoku.Enabled = false;
                Txt_Utiwake.Enabled = false;
                Txt_Jyokencd.Enabled = false;
                Cbo_Tani.Enabled = false;
                Txt_Suryo.Enabled = false;
                Txt_Tanka.Enabled = false;
                Txt_Kingaku.Enabled = false;
                Txt_Zeikbn.Enabled = false;
                Txt_Syohizei.Enabled = false;
                Txt_Tanaban.Enabled = false;
                CMD_Button[1].Visible = false;
                CMD_Button[2].Visible = true;
// 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Add -start
                CMB_ZEIRITUHOJO.Enabled = false;
// 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Add -end
            }
        }

        private void Cbo_Tani_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            M_Tani = Cbo_Tani.Text;
            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
        }

        private void Cbo_Tani_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (Strings.Trim(Cbo_Tani.Text) == M_Tani)
            {
                IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
                return;
            }

            Cbo_Tani.Text = IPPAN2.NIBYTE_HENKAN(Cbo_Tani.Text, 3);
            if (IPPAN.C_Allspace(Cbo_Tani.Text) == 0)
            {
                Cbo_Tani.Focus();
                Cbo_Tani_Enter(Cbo_Tani, new System.EventArgs());
                //必須項目入力エラー
                IPPAN.Error_Msg("E200", 0, " ");
                return;
            }
            if (IPPAN.Input_Check(Cbo_Tani.Text) == 1)
            {
                Cbo_Tani.Focus();
                Cbo_Tani_Enter(Cbo_Tani, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                return;
            }
            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
        }

        private void CMD_Button_Click(System.Object eventSender, System.EventArgs eventArgs)
        {
            short Index = CMD_Button.GetIndex((Button)eventSender);
            DialogResult dlgRlt = DialogResult.None;
            switch (Index)
            {
                case 0:
                    //戻る
                    this.Close();
                    break;
                case 1:
                    //入力確定
                    if (Gamen_Check() == 1)
                    {
                        return;
                    }
                    dlgRlt = IPPAN.Error_Msg("I001", 4, " ");
                    //更新しますか？
                    if (dlgRlt != DialogResult.Yes)
                    {
                        return;
                    }

                    IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_WAIT);

                    using (C_ODBC db = new C_ODBC())
                    {
                        try
                        {
                            //オーナーフォームを取得
                            FRM_CKSI0090M fmOwner = (FRM_CKSI0090M)this.Owner;
                            //DB接続
                            db.Connect();
                            //トランザクション開始
                            db.BeginTrans();

                            Trn_Set(CKSI0090.G_Current_Data);
                            if (fmOwner.Opt_Shori[0].Checked == true)
                            {
                                if (Kensyu_Update(db, CKSI0090.G_Current_Data) == 1)
                                {
                                    //更新に失敗しました
                                    IPPAN.Error_Msg("E613", 0, " ");
                                    return;
                                }
                            }
                            else if (fmOwner.Opt_Shori[1].Checked == true)
                            {
                                if (Kensyu_Insert(db, CKSI0090.G_Current_Data) == 1)
                                {
                                    //更新に失敗しました
                                    IPPAN.Error_Msg("E613", 0, " ");
                                    return;
                                }
                            }
                            //SQLトランザクション終了
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

                    IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                    this.Close();
                    break;
                case 2:
                    //削除
                    dlgRlt = IPPAN.Error_Msg("I014", 4, " ");
                    //削除しますか？
                    if (dlgRlt != DialogResult.Yes)
                    {
                        return;
                    }

                    IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_WAIT);

                    using (C_ODBC db = new C_ODBC())
                    {
                        try
                        {
                            //DB接続
                            db.Connect();
                            //トランザクション開始
                            db.BeginTrans();

                            if (Kensyu_Delete(db, CKSI0090.G_Current_Data) == 1)
                            {
                                //更新に失敗しました
                                IPPAN.Error_Msg("E613", 0, " ");
                                return;
                            }

                            //SQLトランザクション終了
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

                    IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                    this.Close();
                    break;
            }
        }

        private void setContorolEvent()
        {
            //各数値設定後に一部イベントを登録する(初期設定時のイベント発生を避ける)
            // 
            // Txt_Kensyuyy
            // 
            this.Txt_Kensyuyy.TextChanged -= new System.EventHandler(this.Txt_Kensyuyy_TextChanged);
            this.Txt_Kensyuyy.TextChanged += new System.EventHandler(this.Txt_Kensyuyy_TextChanged);
            // 
            // Txt_Kensyumm
            // 
            this.Txt_Kensyumm.TextChanged -= new System.EventHandler(this.Txt_Kensyumm_TextChanged);
            this.Txt_Kensyumm.TextChanged += new System.EventHandler(this.Txt_Kensyumm_TextChanged);
            // 
            // Txt_Kensyudd
            // 
            this.Txt_Kensyudd.TextChanged -= new System.EventHandler(this.Txt_Kensyudd_TextChanged);
            this.Txt_Kensyudd.TextChanged += new System.EventHandler(this.Txt_Kensyudd_TextChanged);
            // 
            // Txt_Simeyy
            // 
            this.Txt_Simeyy.TextChanged -= new System.EventHandler(this.Txt_Simeyy_TextChanged);
            this.Txt_Simeyy.TextChanged += new System.EventHandler(this.Txt_Simeyy_TextChanged);
            // 
            // Txt_Simemm
            // 
            this.Txt_Simemm.TextChanged -= new System.EventHandler(this.Txt_Simemm_TextChanged);
            this.Txt_Simemm.TextChanged += new System.EventHandler(this.Txt_Simemm_TextChanged);
        }

        private void setEventCtrlAry()
        {
            //コントロール配列にイベントを設定
            //CMD_Button
            for (short i = 0; i < this.CMD_Button.Count(); i++)
            {
                this.CMD_Button[i].Click += new EventHandler(CMD_Button_Click);
            }
        }

        private void FRM_CKSI0090S01_Load(System.Object eventSender, System.EventArgs eventArgs)
        {
// 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Add -start
            syohizeiResolver = SyohizeiResolver.GetInstance();
// 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Add -end
            
            //コントロール配列にイベントを設定
            setEventCtrlAry();

            //初期設定前に単位コンボボックスを作成
            if (Tani_Kensaku() == 1)
            {
                //単位マスタが見つかりません
                IPPAN.Error_Msg("E106", 0, " ");
                return;
            }

            //オーナーフォームを取得
            FRM_CKSI0090M fmOwner = (FRM_CKSI0090M)this.Owner;
            if (fmOwner.Opt_Shori[1].Checked == true)
            {
                Gamen_Disp_New();
            }
            else
            {
                Gamen_Disp(CKSI0090.G_Current_Data);
            }

            //各数値設定後に一部イベントを登録する
            setContorolEvent();

            Enable_Control();
        }

        private void TMR_TIMER_Tick(System.Object eventSender, System.EventArgs eventArgs)
        {
            IPPAN.Control_Init(this, "検収内容入力", "CKSI0090S01", SO.SO_USERNAME, SO.SO_OFFICENAME, "1.01");
        }

        private void Txt_Bumon_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            M_Bumon = Txt_Bumon.Text;
            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
        }

        private void Txt_Bumon_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (Strings.Trim(Txt_Bumon.Text) == M_Bumon)
            {
                IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
                return;
            }
            if (IPPAN.C_Allspace(Txt_Bumon.Text) == 0)
            {
                Lbl_Bumonnm.Text = "";
                Txt_Bumon.Focus();
                Txt_Bumon_Enter(Txt_Bumon, new System.EventArgs());
                //必須項目入力エラー
                IPPAN.Error_Msg("E200", 0, " ");
                return;
            }
            if (IPPAN.Input_Check(Txt_Bumon.Text) == 1)
            {
                Lbl_Bumonnm.Text = "";
                Txt_Bumon.Focus();
                Txt_Bumon_Enter(Txt_Bumon, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                return;
            }

            if (Txt_Kubun.Text == "0")
            {
// 2015.01.21 yoshitake OBIC7仕訳連携対応(step3) start.
                if (C_COMMON.GetKouteiNM(Txt_Bumon.Text, out CKSI0090.G_Kamokunm2) == 1)
                //if (CKSI0090.Kamoku_Kensaku("110", Txt_Bumon.Text) == 1)
// 2015.01.21 yoshitake OBIC7仕訳連携対応(step3) end.
                {
                    Lbl_Bumonnm.Text = "";
                    Txt_Bumon.Focus();
                    Txt_Bumon_Enter(Txt_Bumon, new System.EventArgs());
// 2015.01.21 yoshitake OBIC7仕訳連携対応(step3) start.
                    //工程マスタが見つかりません
                    IPPAN.Error_Msg("E547", 0, " ");
                    //科目マスタが見つかりません
                    //IPPAN.Error_Msg("E520", 0, " ");
// 2015.01.21 yoshitake OBIC7仕訳連携対応(step3) end.
                    return;
                }
                Lbl_Bumonnm.Text = CKSI0090.G_Kamokunm2;
            }
            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
        }

        private void Txt_Gyosyacd_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            M_Gyosyacd = Txt_Gyosyacd.Text;
            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
        }

        private void Txt_Gyosyacd_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (Strings.Trim(Txt_Gyosyacd.Text) == M_Gyosyacd)
            {
                IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
                return;
            }
            if (IPPAN.C_Allspace(Txt_Gyosyacd.Text) == 0)
            {
                Lbl_Gyosyanm.Text = "";
                Txt_Gyosyacd.Focus();
                Txt_Gyosyacd_Enter(Txt_Gyosyacd, new System.EventArgs());
                //必須項目入力エラー
                IPPAN.Error_Msg("E200", 0, " ");
                return;
            }
            if (IPPAN.Input_Check(Txt_Gyosyacd.Text) == 1)
            {
                Lbl_Gyosyanm.Text = "";
                Txt_Gyosyacd.Focus();
                Txt_Gyosyacd_Enter(Txt_Gyosyacd, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                return;
            }

            if (CKSI0090.Gyosya_Kensaku(Txt_Gyosyacd.Text) == 1)
            {
                Lbl_Gyosyanm.Text = "";
                Txt_Gyosyacd.Focus();
                Txt_Gyosyacd_Enter(Txt_Gyosyacd, new System.EventArgs());
                //業者マスタが見つかりません
                IPPAN.Error_Msg("E503", 0, " ");
                return;
            }
            Lbl_Gyosyanm.Text = CKSI0090.G_Gyosyanm;
            M_Hinmokucd2 = "";
            Txt_Hinmokucd2_Leave(TXT_Hinmokucd2, new System.EventArgs());
            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
        }

        private void Txt_Hinmokucd2_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            M_Hinmokucd2 = TXT_Hinmokucd2.Text;
            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
        }

        private void Txt_Hinmokucd2_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (Strings.Trim(TXT_Hinmokucd2.Text) == M_Hinmokucd2)
            {
                IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
                return;
            }
            if (IPPAN.C_Allspace(TXT_Hinmokucd2.Text) == 0)
            {
                Lbl_Hinmokunm2.Text = "";
                TXT_Hinmokucd2.Focus();
                Txt_Hinmokucd2_Enter(TXT_Hinmokucd2, new System.EventArgs());
                //必須項目入力エラー
                IPPAN.Error_Msg("E200", 0, " ");
                return;
            }
            if (IPPAN.Input_Check(TXT_Hinmokucd2.Text) == 1)
            {
                Lbl_Hinmokunm2.Text = "";
                TXT_Hinmokucd2.Focus();
                Txt_Hinmokucd2_Enter(TXT_Hinmokucd2, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                return;
            }

            //副資材品目マスタ検索
            if (CKSI0090.SHinmoku_Kensaku(TXT_Hinmokucd2.Text) == 1)
            {
                Lbl_Hinmokunm2.Text = "";
                Txt_Bumon.Text = "";
                Txt_Himoku.Text = "";
                Txt_Utiwake.Text = "";
                Txt_Tanaban.Text = "";
                Lbl_Bumonnm.Text = "";
                Lbl_Himokunm.Text = "";
                Lbl_Utiwakenm.Text = "";
                Lbl_Hinmokunm.Text = "";
                TXT_Hinmokucd2.Focus();
                Txt_Hinmokucd2_Enter(TXT_Hinmokucd2, new System.EventArgs());
                //副資材品目マスタが見つかりません
                IPPAN.Error_Msg("E701", 0, " ");
                return;
            }
            Lbl_Hinmokunm2.Text = CKSI0090.G_Hinmokunm2;
            //製造費
            if (Txt_Kubun.Text == "0")
            {
                switch (CKSI0090.G_Mukesaki)
                {
                    case "1":
                        //EF
                        Txt_Bumon.Text = "01";
                        break;
                    case "2":
                        //LF
                        Txt_Bumon.Text = "02";
                        break;
                    case "3":
                        //CC
                        Txt_Bumon.Text = "03";
                        break;
                    case "7":
                        //TD
                        Txt_Bumon.Text = "03";
                        break;
                    default:
                        Txt_Bumon.Text = "";
                        break;
                }
            }
            else
            {
                Txt_Bumon.Text = "04";
            }
            Txt_Himoku.Text = CKSI0090.G_Himoku;
            Txt_Utiwake.Text = CKSI0090.G_Utiwake;
            Txt_Tanaban.Text = CKSI0090.G_Tanaban;
            M_Bumon = "";
            Txt_Bumon_Leave(Txt_Bumon, new System.EventArgs());
            Txt_Himoku_Leave(Txt_Himoku, new System.EventArgs());
            Txt_Utiwake_Leave(Txt_Utiwake, new System.EventArgs());
            Txt_Tanaban_Leave(Txt_Tanaban, new System.EventArgs());

            //副資材単価マスタ検索
            if (CKSI0090.STanka_Kensaku(Txt_Gyosyacd.Text, TXT_Hinmokucd2.Text) == 1)
            {
                Txt_Jyokencd.Text = "";
                Lbl_Jyokennm.Text = "";
                Txt_Tanka.Text = "";
                Txt_Tanka_Leave(Txt_Tanka, new System.EventArgs());
            }
            else
            {
                Txt_Jyokencd.Text = CKSI0090.G_Jyokencd;
                Txt_Tanka.Text = IPPAN.Money_Hensyu(CKSI0090.G_Tanka, "##,###,##0.000");
                Txt_Jyokencd_Leave(Txt_Jyokencd, new System.EventArgs());
                Txt_Tanka_Leave(Txt_Tanka, new System.EventArgs());
            }

            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
        }

        private void Txt_Himoku_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            M_Himoku = Txt_Himoku.Text;
            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
        }

        private void Txt_Himoku_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {

            if (IPPAN.C_Allspace(Txt_Himoku.Text) == 0)
            {
                Lbl_Himokunm.Text = "";
                Txt_Himoku.Focus();
                Txt_Himoku_Enter(Txt_Himoku, new System.EventArgs());
                //必須項目入力エラー
                IPPAN.Error_Msg("E200", 0, " ");
                return;
            }
            if (IPPAN.Input_Check(Txt_Himoku.Text) == 1)
            {
                Lbl_Himokunm.Text = "";
                Txt_Himoku.Focus();
                Txt_Himoku_Enter(Txt_Himoku, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                return;
            }

            if (Txt_Kubun.Text == "0")
            {
// 2015.02.25 nishi OBIC7仕訳連携 Step3対応 start.
                if (CKSI0090.Kamoku_Kensaku("110", Txt_Himoku.Text) == 1)
                {
                    Lbl_Himokunm.Text = "";
                    Txt_Himoku.Focus();
                    Txt_Himoku_Enter(Txt_Himoku, new System.EventArgs());
                    //科目マスタが見つかりません
                    IPPAN.Error_Msg("E520", 0, " ");
                    return;
                }
                Lbl_Himokunm.Text = CKSI0090.G_Kamokunm2;
                //if (CKSI0090.Himoku_Kensaku(Txt_Himoku.Text) == 1)
                //{
                //    Lbl_Himokunm.Text = "";
                //    Txt_Himoku.Focus();
                //    Txt_Himoku_Enter(Txt_Himoku, new System.EventArgs());
                //    //費目マスタが見つかりません
                //    IPPAN.Error_Msg("E521", 0, " ");
                //    return;
                //}
                //Lbl_Himokunm.Text = CKSI0090.G_Himokunm;
// 2015.02.25 nishi OBIC7仕訳連携 Step3対応 end.
            }
            else
            {
                if (CKSI0090.Kamoku_Kensaku("111", Txt_Himoku.Text) == 1)
                {
                    Lbl_Himokunm.Text = "";
                    Txt_Himoku.Focus();
                    Txt_Himoku_Enter(Txt_Himoku, new System.EventArgs());
                    //科目マスタが見つかりません
                    IPPAN.Error_Msg("E520", 0, " ");
                    return;
                }
                Lbl_Himokunm.Text = CKSI0090.G_Kamokunm2;
            }

            if (CKSI0090.Utiwake_Kensaku(Txt_Himoku.Text, Txt_Utiwake.Text) == 1)
            {
                Lbl_Utiwakenm.Text = "";
                IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
                return;
            }
            Lbl_Utiwakenm.Text = CKSI0090.G_Utiwakenm;

            if (CKSI0090.Hinmoku_Kensaku(Txt_Himoku.Text, Txt_Utiwake.Text, Txt_Tanaban.Text) == 1)
            {
                Lbl_Hinmokunm.Text = "";
                IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
                return;
            }
            Lbl_Hinmokunm.Text = CKSI0090.G_Hinmokunm;
            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
        }

        private void Txt_Jyokencd_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            M_Jyokencd = Txt_Jyokencd.Text;
            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
        }

        private void Txt_Jyokencd_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (Strings.Trim(Txt_Jyokencd.Text) == M_Jyokencd)
            {
                IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
                return;
            }
            if (IPPAN.C_Allspace(Txt_Jyokencd.Text) == 0)
            {
                Lbl_Jyokennm.Text = "";
                Txt_Jyokencd.Focus();
                Txt_Jyokencd_Enter(Txt_Jyokencd, new System.EventArgs());
                //必須項目入力エラー
                IPPAN.Error_Msg("E200", 0, " ");
                return;
            }
            if (IPPAN.Input_Check(Txt_Jyokencd.Text) == 1)
            {
                Lbl_Jyokennm.Text = "";
                Txt_Jyokencd.Focus();
                Txt_Jyokencd_Enter(Txt_Jyokencd, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                return;
            }

            if (CKSI0090.Jyoken_Kensaku(Txt_Jyokencd.Text) == 1)
            {
                Lbl_Jyokennm.Text = "";
                Txt_Jyokencd.Focus();
                Txt_Jyokencd_Enter(Txt_Jyokencd, new System.EventArgs());
                //支払条件マスタが見つかりません
                IPPAN.Error_Msg("E108", 0, " ");
                return;
            }
            Lbl_Jyokennm.Text = CKSI0090.G_Jyokennm;
            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
        }

        private void Txt_Kensyudd_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (this.Visible == false)
            {
                return;
            }

            if (Strings.Len(Txt_Kensyudd.Text) == 2)
            {
                if (Txt_Gyosyacd.Enabled == true)
                {
                    Txt_Gyosyacd.Focus();
                }
                else if (Txt_Bumon.Enabled == true)
                {
                    Txt_Bumon.Focus();
                }
                else if (Txt_Himoku.Enabled == true)
                {
                    Txt_Himoku.Focus();
                }
                else if (Txt_Jyokencd.Enabled == true)
                {
                    Txt_Jyokencd.Focus();
                }
            }
        }

        private void Txt_Kensyudd_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            M_Kensyudd = Txt_Kensyudd.Text;
            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
        }

        private void Txt_Kensyudd_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {
            Txt_Kensyudd.Text = C_COMMON.FormatToNum(Txt_Kensyudd.Text, "00");
            if (Strings.Trim(Txt_Kensyudd.Text) == M_Kensyudd)
            {
                IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
                return;
            }
            if (IPPAN.C_Allspace(Txt_Kensyudd.Text) == 0)
            {
                Txt_Kensyudd.Focus();
                Txt_Kensyudd_Enter(Txt_Kensyudd, new System.EventArgs());
                //必須項目入力エラー
                IPPAN.Error_Msg("E200", 0, " ");
                return;
            }
            IPPAN.G_WYMD = Txt_Kensyuyy.Text + Txt_Kensyumm.Text + Txt_Kensyudd.Text;
            //2013.09.03 DSK yoshida start
            //if (IPPAN.Date_Henkan(ref IPPAN.G_WYMD) == 1)
            //{
            //    Txt_Kensyudd.Focus();
            //    Txt_Kensyudd_Enter(Txt_Kensyudd, new System.EventArgs());
            //    //日付入力エラー
            //    IPPAN.Error_Msg("E201", 0, " ");
            //    return;
            //}
            //2013.09.03 DSK yoshida end
            if (IPPAN.Date_Check(IPPAN.G_WYMD) == 1)
            {
                Txt_Kensyudd.Focus();
                Txt_Kensyudd_Enter(Txt_Kensyudd, new System.EventArgs());
                //日付入力エラー
                IPPAN.Error_Msg("E201", 0, " ");
                return;
            }

            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
        }

        private void Txt_Kensyumm_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (this.Visible == false)
            {
                return;
            }

            if (Strings.Len(Txt_Kensyumm.Text) == 2)
            {
                Txt_Kensyudd.Focus();
            }
        }

        private void Txt_Kensyumm_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            M_Kensyumm = Txt_Kensyumm.Text;
            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
        }

        private void Txt_Kensyumm_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {
            Txt_Kensyumm.Text = C_COMMON.FormatToNum(Txt_Kensyumm.Text, "00");
            if (Strings.Trim(Txt_Kensyumm.Text) == M_Kensyumm)
            {
                IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
                return;
            }
            if (IPPAN.C_Allspace(Txt_Kensyumm.Text) == 0)
            {
                Txt_Kensyumm.Focus();
                Txt_Kensyumm_Enter(Txt_Kensyumm, new System.EventArgs());
                //必須項目入力エラー
                IPPAN.Error_Msg("E200", 0, " ");
                return;
            }

            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
        }

        private void Txt_Kensyuyy_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (this.Visible == false)
            {
                return;
            }
            //2013.09.06 DSK yoshida START
            //if (Strings.Len(Txt_Kensyuyy.Text) == 2)
            if (Strings.Len(Txt_Kensyuyy.Text) == 4)
            //2013.09.06 DSK yoshida END
            {
                Txt_Kensyumm.Focus();
            }
        }

        private void Txt_Kensyuyy_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            M_Kensyuyy = Txt_Kensyuyy.Text;
            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
        }

        private void Txt_Kensyuyy_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {
            //2013.09.06 DSK yoshida START
            //Txt_Kensyuyy.Text = C_COMMON.FormatToNum(Txt_Kensyuyy.Text, "00");
            Txt_Kensyuyy.Text = C_COMMON.DateYYChanged(Txt_Kensyuyy.Text);
            //2013.09.06 DSK yoshida END
            if (Strings.Trim(Txt_Kensyuyy.Text) == M_Kensyuyy)
            {
                IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
                return;
            }
            if (IPPAN.C_Allspace(Txt_Kensyuyy.Text) == 0)
            {
                Txt_Kensyuyy.Focus();
                Txt_Kensyuyy_Enter(Txt_Kensyuyy, new System.EventArgs());
                //必須項目入力エラー
                IPPAN.Error_Msg("E200", 0, " ");
                return;
            }

            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
        }

        private void Txt_Kingaku_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
            Txt_Kingaku.Text = "";
            if (Conversion.Val(CKSI0090.G_Kingaku) != 0)
            {
                Txt_Kingaku.Text = Strings.Trim(CKSI0090.G_Kingaku);
            }
        }

        private void Txt_Kingaku_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (Txt_Kingaku.Text == Strings.Trim(CKSI0090.G_Kingaku))
            {
                Txt_Kingaku.Text = IPPAN.Money_Hensyu(CKSI0090.G_Kingaku, "#,###,###,##0");
                IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
                return;
            }

            //数字、小数点以外はカット
            if (string.IsNullOrEmpty(IPPAN.Numeric_Hensyu(Strings.Trim(Txt_Kingaku.Text))))
            {
                CKSI0090.G_Kingaku = "0";
            }
            else
            {
                CKSI0090.G_Kingaku = C_COMMON.FormatToNum(IPPAN.Numeric_Hensyu(Strings.Trim(Txt_Kingaku.Text)), "#########0");
            }
            if (Conversion.Val(CKSI0090.G_Kingaku) >= 10000000000.0)
            {
                IPPAN.Error_Msg("E202", 0, " ");
                //入力エラー
                Txt_Kingaku.Focus();
                return;
            }
            //カンマ編集
            Txt_Kingaku.Text = IPPAN.Money_Hensyu(CKSI0090.G_Kingaku, "#,###,###,##0");

            //金額計算
            //CKSI0090.G_Syohizei = Convert.ToString(IPPAN.Marume_RTN((double)(Convert.ToDecimal(CKSI0090.G_Kingaku) * CKSI0090.Syohizei_Kensaku(Txt_Zeikbn.Text, "1") / 100), 1, 1));  13.06.12 DELETE

// 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Mod -start
            var target = syohizeiResolver.GetSyohizeiDTO(SyohizeiSearchConditionType.DispZeiKbn,
                                                         Txt_Zeikbn.Text, RoundingTyp.ZeikomiRound,
                                                         Convert.ToDouble(CKSI0090.G_Kingaku));
            if (target != null)
            {
                CKSI0090.G_Syohizei = Convert.ToString(target.Syohizei);
                if (target.ZeirituHojoMap != null)
                {
                    CMB_ZEIRITUHOJO.DataSource = null;
                    CMB_ZEIRITUHOJO.Items.Clear();
                    var comboBoxItems = new List<ComboBoxItem>();
                    foreach (var item in target.ZeirituHojoMap)
                    {
                        comboBoxItems.Add(new ComboBoxItem(item.Key, item.Key + ":" + item.Value));
                    }
                    if (target.ZeirituHojoMap.Count >= 1)
                    {
                        CMB_ZEIRITUHOJO.Enabled = true;
                    }
                    else
                    {
                        CMB_ZEIRITUHOJO.Enabled = false;
                    }
                    CMB_ZEIRITUHOJO.DataSource = comboBoxItems;
                    CMB_ZEIRITUHOJO.ValueMember = "Key";
                    CMB_ZEIRITUHOJO.DisplayMember = "Value";
                    if (!string.IsNullOrEmpty(target.InitDispZeirituHojoKbn))
                    {
                        CMB_ZEIRITUHOJO.SelectedValue = target.InitDispZeirituHojoKbn;
                    }
                }
                else
                {
                    CMB_ZEIRITUHOJO.DataSource = null;
                    CMB_ZEIRITUHOJO.Items.Clear();
                }
            }
            ////13.06.12 tsunamoto 消費税対応 start
            //IPPAN2.ZeiInfo zeiinfo = IPPAN2.Get_Syohizei(1,Txt_Zeikbn.Text,3,Convert.ToDouble(CKSI0090.G_Kingaku));
            //CKSI0090.G_Syohizei = Convert.ToString(zeiinfo.syohizei);
            ////13.06.12 tsunamoto 消費税対応 end
// 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Mod -end

            Txt_Syohizei.Text = IPPAN.Money_Hensyu(CKSI0090.G_Syohizei, "#,###,###,##0");
            CKSI0090.G_Gkingaku = Convert.ToString(Conversion.Val(CKSI0090.G_Kingaku) + Conversion.Val(CKSI0090.G_Syohizei));
            Lbl_Gkingaku.Text = IPPAN.Money_Hensyu(CKSI0090.G_Gkingaku, "#,###,###,##0");

            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
        }

        private void Txt_Kubun_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            M_Kubun = Txt_Kubun.Text;
            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
        }

        private void Txt_Kubun_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (Strings.Trim(Txt_Kubun.Text) == M_Kubun)
            {
                IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
                return;
            }
            if (IPPAN.C_Allspace(Txt_Kubun.Text) == 0)
            {
                Txt_Kubun.Focus();
                Txt_Kubun_Enter(Txt_Kubun, new System.EventArgs());
                //必須項目入力エラー
                IPPAN.Error_Msg("E200", 0, " ");
                return;
            }
            if (IPPAN.Input_Check(Txt_Kubun.Text) == 1)
            {
                Txt_Kubun.Focus();
                Txt_Kubun_Enter(Txt_Kubun, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                return;
            }

            if (Txt_Kubun.Text != "0" && Txt_Kubun.Text != "1")
            {
                Txt_Kubun.Focus();
                Txt_Kubun_Enter(Txt_Kubun, new System.EventArgs());
                //「０」か「１」を入力して下さい
                IPPAN.Error_Msg("E702", 0, " ");
                return;
            }
            if (Txt_Kubun.Text == "1")
            {
                Txt_Bumon.Enabled = false;
                Txt_Bumon.Text = "04";
                Lbl_Bumonnm.Text = "製鋼共通　　　　　　";
            }
            else
            {
                Txt_Bumon.Enabled = true;
                M_Hinmokucd2 = "";
                Txt_Hinmokucd2_Leave(TXT_Hinmokucd2, new System.EventArgs());
            }
            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
        }

        private void Txt_Simemm_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (this.Visible == false)
            {
                return;
            }

            if (Strings.Len(Txt_Simemm.Text) == 2)
            {
                Txt_Kubun.Focus();
            }
        }

        private void Txt_Simemm_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            M_Simemm = Txt_Simemm.Text;
            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
        }

        private void Txt_Simemm_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {
            Txt_Simemm.Text = C_COMMON.FormatToNum(Txt_Simemm.Text, "00");
            if (Strings.Trim(Txt_Simemm.Text) == M_Simemm)
            {
                IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
                return;
            }
            if (IPPAN.C_Allspace(Txt_Simemm.Text) == 0)
            {
                Txt_Simemm.Focus();
                Txt_Simemm_Enter(Txt_Simemm, new System.EventArgs());
                //必須項目入力エラー
                IPPAN.Error_Msg("E200", 0, " ");
                return;
            }
            IPPAN.G_WYMD = Txt_Simeyy.Text + Txt_Simemm.Text + "01";
            //2013.09.03 DSK yoshida start
            //if (IPPAN.Date_Henkan(ref IPPAN.G_WYMD) == 1)
            //{
            //    Txt_Simemm.Focus();
            //    Txt_Simemm_Enter(Txt_Simemm, new System.EventArgs());
            //    //日付入力エラー
            //    IPPAN.Error_Msg("E201", 0, " ");
            //    return;
            //}
            //2013.09.03 DSK yoshida end
            if (IPPAN.Date_Check(IPPAN.G_WYMD) == 1)
            {
                Txt_Simemm.Focus();
                Txt_Simemm_Enter(Txt_Simemm, new System.EventArgs());
                //日付入力エラー
                IPPAN.Error_Msg("E201", 0, " ");
                return;
            }

            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
        }

        private void Txt_Simeyy_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (this.Visible == false)
            {
                return;
            }
            //2013.09.06 DSK yoshida START
            //if (Strings.Len(Txt_Simeyy.Text) == 2)
            if (Strings.Len(Txt_Simeyy.Text) == 4)
            //2013.09.06 DSK yoshida END
            {
                Txt_Simemm.Focus();
            }
        }

        private void Txt_Simeyy_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            M_Simeyy = Txt_Simeyy.Text;
            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
        }

        private void Txt_Simeyy_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {
            //2013.09.06 DSK yoshida START
            //Txt_Simeyy.Text = C_COMMON.FormatToNum(Txt_Simeyy.Text, "00");
            Txt_Simeyy.Text = C_COMMON.DateYYChanged(Txt_Simeyy.Text);
            //2013.09.06 DSK yoshida END
            if (Strings.Trim(Txt_Simeyy.Text) == M_Simeyy)
            {
                IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
                return;
            }
            if (IPPAN.C_Allspace(Txt_Simeyy.Text) == 0)
            {
                Txt_Simeyy.Focus();
                Txt_Simeyy_Enter(Txt_Simeyy, new System.EventArgs());
                //必須項目入力エラー
                IPPAN.Error_Msg("E200", 0, " ");
                return;
            }

            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
        }

        private void Txt_Suryo_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
            Txt_Suryo.Text = "";
            if (Conversion.Val(CKSI0090.G_Suryo) != 0)
            {
                Txt_Suryo.Text = Strings.Trim(CKSI0090.G_Suryo);
            }
        }

        private void Txt_Suryo_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (Txt_Suryo.Text == Strings.Trim(CKSI0090.G_Suryo))
            {
                Txt_Suryo.Text = IPPAN.Money_Hensyu(CKSI0090.G_Suryo, "##,###,##0");
                IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
                return;
            }

            //数字、小数点以外はカット
            if (string.IsNullOrEmpty(IPPAN.Numeric_Hensyu(Strings.Trim(Txt_Suryo.Text))))
            {
                CKSI0090.G_Suryo = "0";
            }
            else
            {
                CKSI0090.G_Suryo = C_COMMON.FormatToNum(IPPAN.Numeric_Hensyu(Strings.Trim(Txt_Suryo.Text)), "#######0");
            }
            if (Conversion.Val(CKSI0090.G_Suryo) >= 100000000)
            {
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                Txt_Suryo.Focus();
                return;
            }
            //カンマ編集
            Txt_Suryo.Text = IPPAN.Money_Hensyu(CKSI0090.G_Suryo, "##,###,##0");

            //金額計算
            CKSI0090.G_Kingaku = Convert.ToString(IPPAN.Marume_RTN((double)(Convert.ToDecimal(CKSI0090.G_Suryo) * Convert.ToDecimal(CKSI0090.G_Tanka)), 0, 1));
            Txt_Kingaku.Text = IPPAN.Money_Hensyu(CKSI0090.G_Kingaku, "#,###,###,##0");
            //CKSI0090.G_Syohizei = Convert.ToString(IPPAN.Marume_RTN((double)(Convert.ToDecimal(CKSI0090.G_Kingaku) * CKSI0090.Syohizei_Kensaku(Txt_Zeikbn.Text, "1") / 100), 1, 1));  13.06.12 DELETE

// 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Mod -start
            var target = syohizeiResolver.GetSyohizeiDTO(SyohizeiSearchConditionType.DispZeiKbn,
                                                         Txt_Zeikbn.Text, RoundingTyp.ZeikomiRound,
                                                         Convert.ToDouble(CKSI0090.G_Kingaku));
            if (target != null)
            {
                CKSI0090.G_Syohizei = Convert.ToString(target.Syohizei);
                if (target.ZeirituHojoMap != null)
                {
                    CMB_ZEIRITUHOJO.DataSource = null;
                    CMB_ZEIRITUHOJO.Items.Clear();
                    var comboBoxItems = new List<ComboBoxItem>();
                    foreach (var item in target.ZeirituHojoMap)
                    {
                        comboBoxItems.Add(new ComboBoxItem(item.Key, item.Key + ":" + item.Value));
                    }
                    if (target.ZeirituHojoMap.Count >= 1)
                    {
                        CMB_ZEIRITUHOJO.Enabled = true;
                    }
                    else
                    {
                        CMB_ZEIRITUHOJO.Enabled = false;
                    }
                    CMB_ZEIRITUHOJO.DataSource = comboBoxItems;
                    CMB_ZEIRITUHOJO.ValueMember = "Key";
                    CMB_ZEIRITUHOJO.DisplayMember = "Value";
                    if (!string.IsNullOrEmpty(target.InitDispZeirituHojoKbn))
                    {
                        CMB_ZEIRITUHOJO.SelectedValue = target.InitDispZeirituHojoKbn;
                    }
                }
                else
                {
                    CMB_ZEIRITUHOJO.DataSource = null;
                    CMB_ZEIRITUHOJO.Items.Clear();
                }
            }
            ////13.06.12 tsunamoto 消費税対応 start
            //IPPAN2.ZeiInfo zeiinfo = IPPAN2.Get_Syohizei(1, Txt_Zeikbn.Text, 3, Convert.ToDouble(CKSI0090.G_Kingaku));
            //CKSI0090.G_Syohizei = Convert.ToString(zeiinfo.syohizei);
            ////13.06.12 tsunamoto 消費税対応 end
// 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Mod -end

            Txt_Syohizei.Text = IPPAN.Money_Hensyu(CKSI0090.G_Syohizei, "#,###,###,##0");
            CKSI0090.G_Gkingaku = Convert.ToString(Conversion.Val(CKSI0090.G_Kingaku) + Conversion.Val(CKSI0090.G_Syohizei));
            Lbl_Gkingaku.Text = IPPAN.Money_Hensyu(CKSI0090.G_Gkingaku, "#,###,###,##0");

            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
        }

        private void Txt_Syohizei_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
            Txt_Syohizei.Text = "";
            if (Conversion.Val(CKSI0090.G_Syohizei) != 0)
            {
                Txt_Syohizei.Text = Strings.Trim(CKSI0090.G_Syohizei);
            }
        }

        private void Txt_Syohizei_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (Txt_Syohizei.Text == Strings.Trim(CKSI0090.G_Syohizei))
            {
                Txt_Syohizei.Text = IPPAN.Money_Hensyu(CKSI0090.G_Syohizei, "#,###,###,##0");
                IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
                return;
            }

            //数字、小数点以外はカット
            if (string.IsNullOrEmpty(IPPAN.Numeric_Hensyu(Strings.Trim(Txt_Syohizei.Text))))
            {
                CKSI0090.G_Syohizei = "0";
            }
            else
            {
                CKSI0090.G_Syohizei = C_COMMON.FormatToNum(IPPAN.Numeric_Hensyu(Strings.Trim(Txt_Syohizei.Text)), "#########0");
            }
            if (Conversion.Val(CKSI0090.G_Syohizei) >= 10000000000.0)
            {
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                Txt_Syohizei.Focus();
                return;
            }
            //カンマ編集
            Txt_Syohizei.Text = IPPAN.Money_Hensyu(CKSI0090.G_Syohizei, "#,###,###,##0");

            //金額計算
            CKSI0090.G_Gkingaku = Convert.ToString(Convert.ToDecimal(CKSI0090.G_Kingaku) + Convert.ToDecimal(CKSI0090.G_Syohizei));
            Lbl_Gkingaku.Text = IPPAN.Money_Hensyu(CKSI0090.G_Gkingaku, "#,###,###,##0");

            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
        }

        private void Txt_Tanaban_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            M_Tanaban = Txt_Tanaban.Text;
            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
        }

        private void Txt_Tanaban_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {

            if (IPPAN.C_Allspace(Txt_Tanaban.Text) == 0)
            {
                Lbl_Hinmokunm.Text = "";
                Txt_Tanaban.Focus();
                Txt_Tanaban_Enter(Txt_Tanaban, new System.EventArgs());
                //必須項目入力エラー
                IPPAN.Error_Msg("E200", 0, " ");
                return;
            }
            if (IPPAN.Input_Check(Txt_Tanaban.Text) == 1)
            {
                Lbl_Hinmokunm.Text = "";
                Txt_Tanaban.Focus();
                Txt_Tanaban_Enter(Txt_Tanaban, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                return;
            }

            if (CKSI0090.Hinmoku_Kensaku(Txt_Himoku.Text, Txt_Utiwake.Text, Txt_Tanaban.Text) == 1)
            {
                Lbl_Hinmokunm.Text = "";
                Txt_Tanaban.Focus();
                Txt_Tanaban_Enter(Txt_Tanaban, new System.EventArgs());
                //品目マスタが見つかりません
                IPPAN.Error_Msg("E107", 0, " ");
                return;
            }
            Lbl_Hinmokunm.Text = CKSI0090.G_Hinmokunm;
            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
        }

        private void Txt_Tanka_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
            Txt_Tanka.Text = "";
            if (Conversion.Val(CKSI0090.G_Tanka) != 0)
            {
                Txt_Tanka.Text = Strings.Trim(CKSI0090.G_Tanka);
            }
        }

        private void Txt_Tanka_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (Txt_Tanka.Text == Strings.Trim(CKSI0090.G_Tanka))
            {
                Txt_Tanka.Text = IPPAN.Money_Hensyu(CKSI0090.G_Tanka, "##,###,##0.000");
                IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
                return;
            }

            //数字、小数点以外はカット
            if (string.IsNullOrEmpty(IPPAN2.Numeric_Hensyu4(Strings.Trim(Txt_Tanka.Text))))
            {
                CKSI0090.G_Tanka = "0";
            }
            else
            {
                CKSI0090.G_Tanka = C_COMMON.FormatToNum(IPPAN2.Numeric_Hensyu4(Strings.Trim(Txt_Tanka.Text)), "#######0.000");
            }
            if (Conversion.Val(CKSI0090.G_Tanka) >= 100000000)
            {
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                Txt_Tanka.Focus();
                return;
            }
            //カンマ編集
            Txt_Tanka.Text = IPPAN.Money_Hensyu(CKSI0090.G_Tanka,  "##,###,##0.000");

            //金額計算
            CKSI0090.G_Kingaku = Convert.ToString(IPPAN.Marume_RTN((double)(Convert.ToDecimal(CKSI0090.G_Suryo) * Convert.ToDecimal(CKSI0090.G_Tanka)), 0, 1));
            Txt_Kingaku.Text = IPPAN.Money_Hensyu(CKSI0090.G_Kingaku, "#,###,###,##0");
            //CKSI0090.G_Syohizei = Convert.ToString(IPPAN.Marume_RTN((double)(Convert.ToDecimal(CKSI0090.G_Kingaku) * CKSI0090.Syohizei_Kensaku(Txt_Zeikbn.Text, "1") / 100), 1, 1));  12.06.12 DELETE

// 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Mod -start
            var target = syohizeiResolver.GetSyohizeiDTO(SyohizeiSearchConditionType.DispZeiKbn,
                                                         Txt_Zeikbn.Text, RoundingTyp.ZeikomiRound,
                                                         Convert.ToDouble(CKSI0090.G_Kingaku));
            if (target != null)
            {
                CKSI0090.G_Syohizei = Convert.ToString(target.Syohizei);
                if (target.ZeirituHojoMap != null)
                {
                    CMB_ZEIRITUHOJO.DataSource = null;
                    CMB_ZEIRITUHOJO.Items.Clear();
                    var comboBoxItems = new List<ComboBoxItem>();
                    foreach (var item in target.ZeirituHojoMap)
                    {
                        comboBoxItems.Add(new ComboBoxItem(item.Key, item.Key + ":" + item.Value));
                    }
                    if (target.ZeirituHojoMap.Count >= 1)
                    {
                        CMB_ZEIRITUHOJO.Enabled = true;
                    }
                    else
                    {
                        CMB_ZEIRITUHOJO.Enabled = false;
                    }
                    CMB_ZEIRITUHOJO.DataSource = comboBoxItems;
                    CMB_ZEIRITUHOJO.ValueMember = "Key";
                    CMB_ZEIRITUHOJO.DisplayMember = "Value";
                    if (!string.IsNullOrEmpty(target.InitDispZeirituHojoKbn))
                    {
                        CMB_ZEIRITUHOJO.SelectedValue = target.InitDispZeirituHojoKbn;
                    }
                }
                else
                {
                    CMB_ZEIRITUHOJO.DataSource = null;
                    CMB_ZEIRITUHOJO.Items.Clear();
                }
            }
            ////13.06.12 tsunamoto 消費税対応 start
            //IPPAN2.ZeiInfo zeiinfo = IPPAN2.Get_Syohizei(1, Txt_Zeikbn.Text, 3, Convert.ToDouble(CKSI0090.G_Kingaku));
            //CKSI0090.G_Syohizei = Convert.ToString(zeiinfo.syohizei);
            ////13.06.12 tsunamoto 消費税対応 end
// 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Mod -end

            Txt_Syohizei.Text = IPPAN.Money_Hensyu(CKSI0090.G_Syohizei, "#,###,###,##0");
            CKSI0090.G_Gkingaku = Convert.ToString(Convert.ToDecimal(CKSI0090.G_Kingaku) + Convert.ToDecimal(CKSI0090.G_Syohizei));
            Lbl_Gkingaku.Text = IPPAN.Money_Hensyu(CKSI0090.G_Gkingaku, "#,###,###,##0");

            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
        }

        private void Txt_Utiwake_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            M_Utiwake = Txt_Utiwake.Text;
            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
        }

        private void Txt_Utiwake_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {

            if (IPPAN.C_Allspace(Txt_Utiwake.Text) == 0)
            {
                Lbl_Utiwakenm.Text = "";
                Txt_Utiwake.Focus();
                Txt_Utiwake_Enter(Txt_Utiwake, new System.EventArgs());
                //必須項目入力エラー
                IPPAN.Error_Msg("E200", 0, " ");
                return;
            }
            if (IPPAN.Input_Check(Txt_Utiwake.Text) == 1)
            {
                Lbl_Utiwakenm.Text = "";
                Txt_Utiwake.Focus();
                Txt_Utiwake_Enter(Txt_Utiwake, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                return;
            }

            if (CKSI0090.Utiwake_Kensaku(Txt_Himoku.Text, Txt_Utiwake.Text) == 1)
            {
                Lbl_Utiwakenm.Text = "";
                Txt_Utiwake.Focus();
                Txt_Utiwake_Enter(Txt_Utiwake, new System.EventArgs());
                //製造費、費目内訳マスタが見つかりません
                IPPAN.Error_Msg("E522", 0, " ");
                return;
            }
            Lbl_Utiwakenm.Text = CKSI0090.G_Utiwakenm;

            if (CKSI0090.Hinmoku_Kensaku(Txt_Himoku.Text, Txt_Utiwake.Text, Txt_Tanaban.Text) == 1)
            {
                Lbl_Hinmokunm.Text = "";
                IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
                return;
            }
            Lbl_Hinmokunm.Text = CKSI0090.G_Hinmokunm;
            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
        }

        private void Txt_Zeikbn_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            M_Zeikbn = Txt_Zeikbn.Text;
            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
        }

// 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Mod -start
        private void Txt_TextChanged(object sender, EventArgs e)
        {
            if (Strings.Trim(Txt_Zeikbn.Text) == M_Zeikbn)
            {
                IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
                return;
            }
            if (IPPAN.Input_Check(Txt_Zeikbn.Text) == 1)
            {
                Lbl_Zeiritu.Text = "";
                Txt_Zeikbn.Focus();
                Txt_Zeikbn_Enter(Txt_Zeikbn, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                return;
            }
            //13.06.12 DELETE start
            // if (IPPAN.C_Allspace(Txt_Zeikbn.Text) == 0)
            // {
            // }
            // else
            // {
            //     if (Txt_Zeikbn.Text == "1")
            //     {
            //     }
            //     else
            //     {
            //         Lbl_Zeiritu.Text = "";
            //         Txt_Zeikbn.Focus();
            //         Txt_Zeikbn_Enter(Txt_Zeikbn, new System.EventArgs());
            //         //入力エラー
            //         IPPAN.Error_Msg("E202", 0, " ");
            //         return;
            //     }
            // }
            //Lbl_Zeiritu.Text = IPPAN2.NIBYTE_HENKAN(Convert.ToString(CKSI0090.Syohizei_Kensaku(Txt_Zeikbn.Text, "1")), 1) + "％";                                                     

            //金額計算
            //CKSI0090.G_Syohizei = Convert.ToString(IPPAN.Marume_RTN((double)(Convert.ToDecimal(CKSI0090.G_Kingaku) * CKSI0090.Syohizei_Kensaku(Txt_Zeikbn.Text, "1") / 100), 1, 1));  
            //13.06.12 DELETE end

            // 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Mod -start
            var target = syohizeiResolver.GetSyohizeiDTO(SyohizeiSearchConditionType.DispZeiKbn,
                                                         Txt_Zeikbn.Text, RoundingTyp.ZeikomiRound,
                                                         Convert.ToDouble(CKSI0090.G_Kingaku));
            if (target != null)
            {
                Lbl_Zeiritu.Text = target.DispSyohizei;
                CKSI0090.G_Syohizei = Convert.ToString(target.Syohizei);
                if (target.ZeirituHojoMap != null)
                {
                    CMB_ZEIRITUHOJO.DataSource = null;
                    CMB_ZEIRITUHOJO.Items.Clear();
                    var comboBoxItems = new List<ComboBoxItem>();
                    foreach (var item in target.ZeirituHojoMap)
                    {
                        comboBoxItems.Add(new ComboBoxItem(item.Key, item.Key + ":" + item.Value));
                    }
                    if (target.ZeirituHojoMap.Count >= 1)
                    {
                        CMB_ZEIRITUHOJO.Enabled = true;
                    }
                    else
                    {
                        CMB_ZEIRITUHOJO.Enabled = false;
                    }
                    CMB_ZEIRITUHOJO.DataSource = comboBoxItems;
                    CMB_ZEIRITUHOJO.ValueMember = "Key";
                    CMB_ZEIRITUHOJO.DisplayMember = "Value";
                    if (!string.IsNullOrEmpty(target.InitDispZeirituHojoKbn))
                    {
                        CMB_ZEIRITUHOJO.SelectedValue = target.InitDispZeirituHojoKbn;
                    }
                }
                else
                {
                    CMB_ZEIRITUHOJO.DataSource = null;
                    CMB_ZEIRITUHOJO.Items.Clear();
                }
            }
            ////13.06.12 tsunamoto 消費税対応 start
            //IPPAN2.ZeiInfo zeiinfo = IPPAN2.Get_Syohizei(1, Txt_Zeikbn.Text, 3, Convert.ToDouble(CKSI0090.G_Kingaku));
            //Lbl_Zeiritu.Text = zeiinfo.zeihyoji;
            //CKSI0090.G_Syohizei = Convert.ToString(zeiinfo.syohizei);
            ////13.06.12 tsunamoto 消費税対応 end
            // 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Mod -end

            Txt_Syohizei.Text = IPPAN.Money_Hensyu(CKSI0090.G_Syohizei, "#,###,###,##0");
            CKSI0090.G_Gkingaku = Convert.ToString(Convert.ToDecimal(CKSI0090.G_Kingaku) + Convert.ToDecimal(CKSI0090.G_Syohizei));
            Lbl_Gkingaku.Text = IPPAN.Money_Hensyu(CKSI0090.G_Gkingaku, "#,###,###,##0");
        }
// 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Mod -end

        private void Txt_Zeikbn_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {
            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
            var targetZeirituKbn = Txt_Zeikbn.Text;
            if (string.IsNullOrEmpty(targetZeirituKbn))
            {
                Txt_Zeikbn.Text = " ";
                return;
            }
            var target = syohizeiResolver.GetSyohizeiDTO(SyohizeiSearchConditionType.DispZeiKbn, targetZeirituKbn);
            if (target == null)
            {
                Txt_Zeikbn.Text = " ";
                return;
            }
        }

        private short Kensyu_Insert(C_ODBC db, int i)
        {
            short functionReturnValue = 1;
            string L_datetime = null;

            L_datetime = String.Format("{0:yyyyMMdd HH:mm:ss}", DateTime.Now);

            IPPAN.G_SQL = "insert into SIZAI_KENSYU_TRN values (";
            IPPAN.G_SQL = IPPAN.G_SQL + "'" + CKSI0090.G_Kensyu_Area[i, 0] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0090.G_Kensyu_Area[i, 1] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0090.G_Kensyu_Area[i, 2] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0090.G_Kensyu_Area[i, 3] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0090.G_Kensyu_Area[i, 4] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0090.G_Kensyu_Area[i, 5] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0090.G_Kensyu_Area[i, 6] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0090.G_Kensyu_Area[i, 7] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0090.G_Kensyu_Area[i, 8] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0090.G_Kensyu_Area[i, 9] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0090.G_Kensyu_Area[i, 10] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0090.G_Kensyu_Area[i, 11] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0090.G_Kensyu_Area[i, 12] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0090.G_Kensyu_Area[i, 13] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0090.G_Kensyu_Area[i, 14] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0090.G_Kensyu_Area[i, 15] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0090.G_Kensyu_Area[i, 16] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + "," + CKSI0090.G_Kensyu_Area[i, 17];
            IPPAN.G_SQL = IPPAN.G_SQL + "," + CKSI0090.G_Kensyu_Area[i, 18];
            IPPAN.G_SQL = IPPAN.G_SQL + "," + CKSI0090.G_Kensyu_Area[i, 19];
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0090.G_Kensyu_Area[i, 20] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + "," + CKSI0090.G_Kensyu_Area[i, 21];
            IPPAN.G_SQL = IPPAN.G_SQL + "," + CKSI0090.G_Kensyu_Area[i, 22];
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0090.G_Kensyu_Area[i, 23] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0090.G_Kensyu_Area[i, 24] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + "," + CKSI0090.G_Kensyu_Area[i, 25];
            IPPAN.G_SQL = IPPAN.G_SQL + ",to_date('" + L_datetime + "', 'YYYY/MM/DD HH24:MI:SS')";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0090.G_Kensyu_Area[i, 27] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0090.G_Kensyu_Area[i, 28] + "'";
// 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Mod -start
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0090.G_Kensyu_Area[i, 29] + "'";
            if (!string.IsNullOrEmpty(CKSI0090.G_Kensyu_Area[i, 30]))
            {
                IPPAN.G_SQL += ",'" + CKSI0090.G_Kensyu_Area[i, 30] + "')";
            }
            else
            {
                IPPAN.G_SQL += ",NULL)";
            }
            //IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0090.G_Kensyu_Area[i, 29] + "')";
// 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Mod -end
            Debug.Print(IPPAN.G_SQL);

            try
            {
                //トランザクションは呼出し元で制御する
                //SQL実行
                db.ExecSQL(IPPAN.G_SQL);
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

            functionReturnValue = 0;
            return functionReturnValue;
        }

        private short Kensyu_Update(C_ODBC db, int i)
        {
            short functionReturnValue = 1;
            string L_datetime = null;

            L_datetime = String.Format("{0:yyyyMMdd HH:mm:ss}", DateTime.Now);

            IPPAN.G_SQL = "update SIZAI_KENSYU_TRN set ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIMEYM = '" + CKSI0090.G_Kensyu_Area[i, 1] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",KUBUN = '" + CKSI0090.G_Kensyu_Area[i, 2] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",KENSYUBI = '" + CKSI0090.G_Kensyu_Area[i, 3] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",GYOSYACD = '" + CKSI0090.G_Kensyu_Area[i, 4] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",GYOSYANM = '" + CKSI0090.G_Kensyu_Area[i, 5] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",BUMON = '" + CKSI0090.G_Kensyu_Area[i, 6] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",BUMONNM = '" + CKSI0090.G_Kensyu_Area[i, 7] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",HIMOKU = '" + CKSI0090.G_Kensyu_Area[i, 8] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",HIMOKUNM = '" + CKSI0090.G_Kensyu_Area[i, 9] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",UTIWAKE = '" + CKSI0090.G_Kensyu_Area[i, 10] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",UTIWAKENM = '" + CKSI0090.G_Kensyu_Area[i, 11] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",TANABAN = '" + CKSI0090.G_Kensyu_Area[i, 12] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",HINMOKUNM = '" + CKSI0090.G_Kensyu_Area[i, 13] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",JYOKENCD = '" + CKSI0090.G_Kensyu_Area[i, 14] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",JYOKENNM = '" + CKSI0090.G_Kensyu_Area[i, 15] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",TANI = '" + CKSI0090.G_Kensyu_Area[i, 16] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",SURYO = " + CKSI0090.G_Kensyu_Area[i, 17];
            IPPAN.G_SQL = IPPAN.G_SQL + ",TANKA = " + CKSI0090.G_Kensyu_Area[i, 18];
            IPPAN.G_SQL = IPPAN.G_SQL + ",KINGAKU = " + CKSI0090.G_Kensyu_Area[i, 19];
            IPPAN.G_SQL = IPPAN.G_SQL + ",ZEIKBN = '" + CKSI0090.G_Kensyu_Area[i, 20] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",SYOHIZEI = " + CKSI0090.G_Kensyu_Area[i, 21];
            IPPAN.G_SQL = IPPAN.G_SQL + ",GKINGAKU = " + CKSI0090.G_Kensyu_Area[i, 22];
            IPPAN.G_SQL = IPPAN.G_SQL + ",KENSYUFLG = '" + CKSI0090.G_Kensyu_Area[i, 23] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",HACHUNO = '" + CKSI0090.G_Kensyu_Area[i, 24] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",PAGENO = " + CKSI0090.G_Kensyu_Area[i, 25];
            IPPAN.G_SQL = IPPAN.G_SQL + ",UPDYMD = to_date('" + L_datetime + "', 'YYYY/MM/DD HH24:MI:SS')";
            IPPAN.G_SQL = IPPAN.G_SQL + ",HINMOKUCD2 = '" + CKSI0090.G_Kensyu_Area[i, 27] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",HINMOKUNM2 = '" + CKSI0090.G_Kensyu_Area[i, 28] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",DELFLG = '" + CKSI0090.G_Kensyu_Area[i, 29] + "'";
            // 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Mod -start
            if (!string.IsNullOrEmpty(CKSI0090.G_Kensyu_Area[i, 30]))
            {
                IPPAN.G_SQL += ",ZEIRITU_HOJO_KBN ='" + CKSI0090.G_Kensyu_Area[i, 30] + "'";
            }
            else
            {
                IPPAN.G_SQL += ",ZEIRITU_HOJO_KBN = NULL";
            }
            //IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0090.G_Kensyu_Area[i, 29] + "')";
            // 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Mod -end
            IPPAN.G_SQL = IPPAN.G_SQL + " where KENSYUNO = '" + CKSI0090.G_Kensyu_Area[i, 0] + "'";
            Debug.Print(IPPAN.G_SQL);

            try
            {
                //トランザクションは呼出し元で制御する
                //SQL実行
                db.ExecSQL(IPPAN.G_SQL);
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

            functionReturnValue = 0;
            return functionReturnValue;
        }

        private short Kensyu_Delete(C_ODBC db, int i)
        {
            short functionReturnValue = 1;
            string L_datetime = null;

            L_datetime = String.Format("{0:yyyyMMdd HH:mm:ss}", DateTime.Now);

            IPPAN.G_SQL = "update SIZAI_KENSYU_TRN set ";
            IPPAN.G_SQL = IPPAN.G_SQL + "UPDYMD = to_date('" + L_datetime + "', 'YYYY/MM/DD HH24:MI:SS')";
            IPPAN.G_SQL = IPPAN.G_SQL + ",DELFLG = '1'";
            IPPAN.G_SQL = IPPAN.G_SQL + " where KENSYUNO = '" + CKSI0090.G_Kensyu_Area[i, 0] + "'";
            Debug.Print(IPPAN.G_SQL);

            try
            {
                //トランザクションは呼出し元で制御する
                //SQL実行
                db.ExecSQL(IPPAN.G_SQL);
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

            functionReturnValue = 0;
            return functionReturnValue;
        }
    }
}

using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Windows.Forms;
using System.Data.Odbc;
using System.Drawing;

namespace Project1
{
    internal partial class FRM_CKSI0130M : System.Windows.Forms.Form
    {
        //**************************************************************'
        //                                                              '
        //   プログラム名　　　副資材品目マスタメンテ画面処理           '
        //                                                              '
        //   更新履歴
        //   更新日付    Rev     修正者      内容
        //   03.01.15           NTIS浅田　　Ｓ／Ｏサーバ更新の為、VB4.0→VB6.0SP5へのバージョンアップ対応
        //   03.03.25           NTIS浅田  　日本語項目のプロパティのMaxlengthを文字数に変更
        //   13.07.03           ISV-TRUC    資材品目メンテ画面　初期表示
        //   13.07.03           ISV-TRUC    資材品目メンテ画面　プロパティ
        //**************************************************************'

        //品目名変更用ワーク
        string WK_HinNm;
        //費目コード変更用ワーク
        string WK_Himoku;
        //内訳コード変更用ワーク
        string WK_Utiwake;
        //棚番変更用ワーク
        string WK_Tanaban;
        //単位変更用ワーク
        string WK_Tani;
        //種別変更用ワーク
        string WK_Syubetu;
        //水分引き区分変更用ワーク
        string WK_SuibunKbn;
        //検収明細出力区分変更用ワーク
        string WK_KensyuKbn;
        //経理報告区分変更用ワーク
        string WK_KeiriKbn;
        //向先区分変更用ワーク
        string WK_Mukesaki;
        //出庫位置区分変更用ワーク
        int WK_Syuko;

        //   13.07.03  ISV-TRUC    資材品目メンテ画面　プロパティ
        public string HinmokuCD;
        public short ButtonIndex;

        //資材品目メンテ画面　列挙型
        public CKSI0130.Mode ProcKBN = CKSI0130.Mode.INSERT_MODE;
        // End  13.07.03  ISV-TRUC  

        // ComboBox用
        // ComboBox用ID：ID
        private string P_COMBO_ID = "ID";
        // ComboBox用ID：NAME
        private string P_COMBO_NM = "NAME";

        //   13.07.03  ISV-TRUC 
        //[STAThread]
        //static void Main()
        //{
        //    Application.EnableVisualStyles();
        //    Application.SetCompatibleTextRenderingDefault(false);
        //    Application.Run(new FRM_CKSI0130M());
        //}
        //   13.07.03  ISV-TRUC end

        [System.Security.Permissions.SecurityPermission(
            System.Security.Permissions.SecurityAction.LinkDemand,
            Flags = System.Security.Permissions.SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            const int WM_SYSCOMMAND = 0x112;
            const long SC_CLOSE = 0xF060L;

            if (m.Msg == WM_SYSCOMMAND &&
               (m.WParam.ToInt64() & 0xFFF0L) == SC_CLOSE)
            {
                return;
            }

            base.WndProc(ref m);
        }

        private void Set_HinmokuPara()
        {
            // 品目コード
            CKSI0130.HINMOKU.HINMOKUCD = IPPAN2.MixSpace_Set(TXT_HinCd.Text, 4);
            // 品目名
            CKSI0130.HINMOKU.HINMOKUNM = IPPAN.Space_Set(TXT_HinNm.Text, 20, 2);
            // 費目
            CKSI0130.HINMOKU.HIMOKU = IPPAN2.MixSpace_Set(TXT_Himoku.Text, 2);
            // 内訳
            CKSI0130.HINMOKU.UTIWAKE = IPPAN2.MixSpace_Set(TXT_Utiwake.Text, 2);
            // 棚番
            CKSI0130.HINMOKU.TANABAN = IPPAN2.MixSpace_Set(TXT_Tanaban.Text, 2);
            // 単位
            CKSI0130.HINMOKU.TANI = IPPAN.Space_Set(CBO_Tani.Text, 2, 2);
            // 種別　１：入庫払い　２：使用高払い
            CKSI0130.HINMOKU.SYUBETU = IPPAN2.MixSpace_Set(TXT_Syubetu.Text, 1);
            // 水分引き区分　△：水分引き無し　１：水分引き可能
            CKSI0130.HINMOKU.SUIBUNKBN = IPPAN2.MixSpace_Set(TXT_SuibunKbn.Text, 1);
            // 検収明細出力区分　△：出力無し　１：検収明細出力
            CKSI0130.HINMOKU.KENSYUKBN = IPPAN2.MixSpace_Set(TXT_KensyuKbn.Text, 1);
            // 経理報告区分　△：出力無し　１：経理報告出力
            CKSI0130.HINMOKU.HOUKOKUKBN = IPPAN2.MixSpace_Set(TXT_KeiriKbn.Text, 1);
            // 出庫位置区分：１～８　経理報告データ作成時に使用
            CKSI0130.HINMOKU.ICHIKBN = IPPAN2.MixSpace_Set(Convert.ToString(CBO_Syuko.SelectedIndex + 1), 1);
            if (CBO_Mukesaki.SelectedIndex == -1 || CBO_Mukesaki.SelectedIndex == 8)
            {
                CKSI0130.HINMOKU.MUKESAKI = Strings.Space(1);
            }
            else
            {
                // 向先区分：１～８　資材班作業誌入力で使用
                CKSI0130.HINMOKU.MUKESAKI = IPPAN2.MixSpace_Set(Convert.ToString(CBO_Mukesaki.SelectedIndex + 1), 1);
            }
        }

        private short Gamen_Check()
        {
            short functionReturnValue = 1;

            //品目名
            if (IPPAN.C_Allspace(TXT_HinNm.Text) == 0)
            {
                TXT_HinNm.Focus();
                TXT_HinNm_Enter(TXT_HinNm, new System.EventArgs());
                //必須項目入力エラー
                IPPAN.Error_Msg("E200", 0, " ");
                return functionReturnValue;
            }
            if (IPPAN.Input_Check(TXT_HinNm.Text) == 1)
            {
                TXT_HinNm.Focus();
                TXT_HinNm_Enter(TXT_HinNm, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                return functionReturnValue;
            }

            //種別
            if (IPPAN.C_Allspace(TXT_Syubetu.Text) == 0)
            {
                TXT_Syubetu.Focus();
                TXT_Syubetu_Enter(TXT_Syubetu, new System.EventArgs());
                //必須項目入力エラー
                IPPAN.Error_Msg("E200", 0, " ");
                return functionReturnValue;
            }
            if (IPPAN.Input_Check(TXT_Syubetu.Text) == 1)
            {
                TXT_Syubetu.Focus();
                TXT_Syubetu_Enter(TXT_Syubetu, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                return functionReturnValue;
            }
            IPPAN.G_RET = IPPAN.Numeric_Check2(TXT_Syubetu.Text);
            if (IPPAN.G_RET == 1)
            {
                TXT_Syubetu.Focus();
                TXT_Syubetu_Enter(TXT_Syubetu, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                return functionReturnValue;
            }
            if (TXT_Syubetu.Text == "1" || TXT_Syubetu.Text == "2")
            {
            }
            else
            {
                TXT_Syubetu.Focus();
                TXT_Syubetu_Enter(TXT_Syubetu, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                return functionReturnValue;
            }

            //費目
            if (IPPAN.C_Allspace(TXT_Himoku.Text) == 0)
            {
                LBL_HimokuNm.Text = "";
                TXT_Himoku.Focus();
                TXT_Himoku_Enter(TXT_Himoku, new System.EventArgs());
                //必須項目入力エラー
                IPPAN.Error_Msg("E200", 0, " ");
                return functionReturnValue;
            }
            if (IPPAN.Input_Check(TXT_Himoku.Text) == 1)
            {
                LBL_HimokuNm.Text = "";
                TXT_Himoku.Focus();
                TXT_Himoku_Enter(TXT_Himoku, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                return functionReturnValue;
            }
            IPPAN.G_RET = IPPAN.Numeric_Check2(TXT_Himoku.Text);
            if (IPPAN.G_RET == 1)
            {
                TXT_Himoku.Focus();
                TXT_Himoku_Enter(TXT_Himoku, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                return functionReturnValue;
            }

            if (TXT_Syubetu.Text != "1")
            {
// 2015.02.25 nishi OBIC7仕訳連携 Step3対応 start.
                if (CKSI0130.Kamoku_Kensaku("110", TXT_Himoku.Text) == 1)
                {
                    LBL_HimokuNm.Text = "";
                    TXT_Himoku.Focus();
                    TXT_Himoku_Enter(TXT_Himoku, new System.EventArgs());
                    //科目マスタが見つかりません
                    IPPAN.Error_Msg("E520", 0, " ");
                    return functionReturnValue;
                }
                LBL_HimokuNm.Text = CKSI0130.G_Kamokunm;
                //if (CKSI0130.Himoku_Kensaku(TXT_Himoku.Text) == 1)
                //{
                //    LBL_HimokuNm.Text = "";
                //    TXT_Himoku.Focus();
                //    TXT_Himoku_Enter(TXT_Himoku, new System.EventArgs());
                //    //費目マスタが見つかりません
                //    IPPAN.Error_Msg("E521", 0, " ");
                //    return functionReturnValue;
                //}
                //LBL_HimokuNm.Text = CKSI0130.G_Himokunm;
// 2015.02.25 nishi OBIC7仕訳連携 Step3対応 end.
            }
            else
            {
                if (CKSI0130.Kamoku_Kensaku("111", TXT_Himoku.Text) == 1)
                {
                    LBL_HimokuNm.Text = "";
                    TXT_Himoku.Focus();
                    TXT_Himoku_Enter(TXT_Himoku, new System.EventArgs());
                    //科目マスタが見つかりません
                    IPPAN.Error_Msg("E520", 0, " ");
                    return functionReturnValue;
                }
                LBL_HimokuNm.Text = CKSI0130.G_Kamokunm;
            }

            //内訳
            if (IPPAN.C_Allspace(TXT_Utiwake.Text) == 0)
            {
                LBL_UtiwakeNm.Text = "";
                TXT_Utiwake.Focus();
                TXT_Utiwake_Enter(TXT_Utiwake, new System.EventArgs());
                //必須項目入力エラー
                IPPAN.Error_Msg("E200", 0, " ");
                return functionReturnValue;
            }
            if (IPPAN.Input_Check(TXT_Utiwake.Text) == 1)
            {
                LBL_UtiwakeNm.Text = "";
                TXT_Utiwake.Focus();
                TXT_Utiwake_Enter(TXT_Utiwake, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                return functionReturnValue;
            }
            IPPAN.G_RET = IPPAN.Numeric_Check2(TXT_Utiwake.Text);
            if (IPPAN.G_RET == 1)
            {
                LBL_UtiwakeNm.Text = "";
                TXT_Utiwake.Focus();
                TXT_Utiwake_Enter(TXT_Utiwake, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                return functionReturnValue;
            }

            if (CKSI0130.Utiwake_Kensaku(TXT_Himoku.Text, TXT_Utiwake.Text) == 1)
            {
                LBL_UtiwakeNm.Text = "";
                TXT_Utiwake.Focus();
                TXT_Utiwake_Enter(TXT_Utiwake, new System.EventArgs());
                //製造費、費目内訳マスタが見つかりません
                IPPAN.Error_Msg("E522", 0, " ");
                return functionReturnValue;
            }
            LBL_UtiwakeNm.Text = CKSI0130.G_Utiwakenm;

            //棚番
            if (IPPAN.C_Allspace(TXT_Tanaban.Text) == 0)
            {
                TXT_Tanaban.Focus();
                TXT_Tanaban_Enter(TXT_Tanaban, new System.EventArgs());
                //必須項目入力エラー
                IPPAN.Error_Msg("E200", 0, " ");
                return functionReturnValue;
            }
            if (IPPAN.Input_Check(TXT_Tanaban.Text) == 1)
            {
                TXT_Tanaban.Focus();
                TXT_Tanaban_Enter(TXT_Tanaban, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                return functionReturnValue;
            }
            IPPAN.G_RET = IPPAN.Numeric_Check2(TXT_Tanaban.Text);
            if (IPPAN.G_RET == 1)
            {
                TXT_Tanaban.Focus();
                TXT_Tanaban_Enter(TXT_Tanaban, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                return functionReturnValue;
            }

            //単位
            if (IPPAN.C_Allspace(CBO_Tani.Text) == 0)
            {
                CBO_Tani.Focus();
                CBO_Tani_Enter(CBO_Tani, new System.EventArgs());
                //必須項目入力エラー
                IPPAN.Error_Msg("E200", 0, " ");
                return functionReturnValue;
            }
            if (IPPAN.Input_Check(CBO_Tani.Text) == 1)
            {
                CBO_Tani.Focus();
                CBO_Tani_Enter(CBO_Tani, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                return functionReturnValue;
            }

            //水分引き区分
            if (IPPAN.Input_Check(TXT_SuibunKbn.Text) == 1)
            {
                TXT_SuibunKbn.Focus();
                TXT_SuibunKbn_Enter(TXT_SuibunKbn, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                return functionReturnValue;
            }

            if (TXT_SuibunKbn.Text == "1" || string.IsNullOrEmpty(TXT_SuibunKbn.Text) || TXT_SuibunKbn.Text == " ")
            {
            }
            else
            {
                TXT_SuibunKbn.Focus();
                TXT_SuibunKbn_Enter(TXT_SuibunKbn, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                return functionReturnValue;
            }

            //検収明細出力区分
            if (IPPAN.Input_Check(TXT_KensyuKbn.Text) == 1)
            {
                TXT_KensyuKbn.Focus();
                TXT_KensyuKbn_Enter(TXT_KensyuKbn, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                return functionReturnValue;
            }

            if (TXT_KensyuKbn.Text == "1" || string.IsNullOrEmpty(TXT_KensyuKbn.Text) || TXT_KensyuKbn.Text == " ")
            {
            }
            else
            {
                TXT_KensyuKbn.Focus();
                TXT_KensyuKbn_Enter(TXT_KensyuKbn, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                return functionReturnValue;
            }

            //経理報告区分
            if (IPPAN.Input_Check(TXT_KeiriKbn.Text) == 1)
            {
                TXT_KeiriKbn.Focus();
                TXT_KeiriKbn_Enter(TXT_KeiriKbn, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                return functionReturnValue;
            }

            if (TXT_KeiriKbn.Text == "1" || string.IsNullOrEmpty(TXT_KeiriKbn.Text) || TXT_KeiriKbn.Text == " ")
            {
            }
            else
            {
                TXT_KeiriKbn.Focus();
                TXT_KeiriKbn_Enter(TXT_KeiriKbn, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                return functionReturnValue;
            }

            //向先区分
            if (TXT_Syubetu.Text == "2")
            {
                if (CBO_Mukesaki.SelectedIndex == -1)
                {
                    CBO_Mukesaki.Focus();
                    CBO_Mukesaki_Enter(CBO_Mukesaki, new System.EventArgs());
                    //必須項目入力エラー
                    IPPAN.Error_Msg("E200", 0, " ");
                    return functionReturnValue;
                }
            }
            //出庫位置区分
            if (CBO_Syuko.SelectedIndex == -1)
            {
                C_COMMON.Msg("出庫位置区分が指定されてません");
            }

            functionReturnValue = 0;
            return functionReturnValue;
        }

        private void Gamen_Clear()
        {
            //マスタメンテ画面情報消去
            //   13.07.03  ISV-TRUC start
            if (CKSI0130.G_OfficeId.Equals("KOUBAI"))
            {
                TXT_HinCd.Text = "";
            }
            else
            {
                if (this.OPT_Sinki.Checked)
                {
                    TXT_HinCd.Text = "";
                }
            }
            //   13.07.03  ISV-TRUC end
                         
            TXT_HinNm.Text = "";
            TXT_Himoku.Text = "";
            TXT_Utiwake.Text = "";
            TXT_Tanaban.Text = "";
            CBO_Tani.Text = "";
            CBO_Tani.SelectedIndex = -1;
            TXT_Syubetu.Text = "";
            TXT_SuibunKbn.Text = "";
            TXT_KensyuKbn.Text = "";
            TXT_KeiriKbn.Text = "";
            CBO_Mukesaki.SelectedIndex = -1;
            CBO_Syuko.SelectedIndex = -1;
            LBL_HimokuNm.Text = "";
            LBL_UtiwakeNm.Text = "";
        }

        public short Tani_Kensaku()
        {
            short functionReturnValue = 1;
            //単位マスタをコンボボックスに展開
            string[] L_DATA = new string[2];

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
                catch (Exception)
                {
                    db.Error();
                }
            }

            //結果の列数を取得する
            if (tbl.Columns.Count < 1)
            {
                return functionReturnValue;
            }

            //ﾃﾞｰﾀがない
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
                row[P_COMBO_NM] = Strings.Left(L_DATA[1], 2) + Strings.StrConv(Strings.Space(1), VbStrConv.Wide, C_COMMON.G_CMN_LOCALID_JP);
                cmbTbl.Rows.Add(row);
            }

            cmbTbl.AcceptChanges();

            //コンボボックスのDataSourceにDataTableを割り当てる
            this.CBO_Tani.DataSource = cmbTbl;

            //表示される値はDataTableのNAME列
            this.CBO_Tani.DisplayMember = P_COMBO_NM;

            //対応する値はDataTableのID列
            this.CBO_Tani.ValueMember = P_COMBO_ID;

            //初期状態を未選択状態にする
            this.CBO_Tani.SelectedIndex = -1;

            functionReturnValue = 0;
            return functionReturnValue;
        }

        public void SYORI_C()
        {
            //フッタ部入力フェーズ
            OPT_Sinki.Enabled = false;
            OPT_Henko.Enabled = false;
            OPT_Sakjyo.Enabled = false;
            TXT_HinCd.Enabled = false;
            CMD_Button[0].Enabled = false;
            //明細部入力フェーズ
            TXT_HinNm.Enabled = false;
            TXT_Himoku.Enabled = false;
            TXT_Utiwake.Enabled = false;
            TXT_Tanaban.Enabled = false;
            CBO_Tani.Enabled = false;
            TXT_Syubetu.Enabled = false;
            TXT_SuibunKbn.Enabled = false;
            TXT_KensyuKbn.Enabled = false;
            TXT_KeiriKbn.Enabled = false;
            CBO_Mukesaki.Enabled = false;
            CBO_Syuko.Enabled = false;
            CMD_Button[1].Enabled = true;
            CMD_Button[2].Enabled = true;
            CMD_Button[3].Enabled = false;
            CMD_Button[4].Enabled = true;
        }

        public void SYORI_B()
        {
            //フッタ部入力フェーズ
            OPT_Sinki.Enabled = false;
            OPT_Henko.Enabled = false;
            OPT_Sakjyo.Enabled = false;
            TXT_HinCd.Enabled = false;
            CMD_Button[0].Enabled = false;
            //明細部入力フェーズ
            TXT_HinNm.Enabled = true;
            TXT_Himoku.Enabled = true;
            TXT_Utiwake.Enabled = true;
            TXT_Tanaban.Enabled = true;
            CBO_Tani.Enabled = true;
            TXT_Syubetu.Enabled = true;
            TXT_SuibunKbn.Enabled = true;
            TXT_KensyuKbn.Enabled = true;
            TXT_KeiriKbn.Enabled = true;
            CBO_Mukesaki.Enabled = true;
            CBO_Syuko.Enabled = true;
            CMD_Button[1].Enabled = true;
            CMD_Button[2].Enabled = true;
            CMD_Button[3].Enabled = true;
            CMD_Button[4].Enabled = true;
        }

        public void SYORI_A()
        {
            //フッタ部入力フェーズ
            OPT_Sinki.Enabled = true;
            OPT_Henko.Enabled = true;
            OPT_Sakjyo.Enabled = true;
            TXT_HinCd.Enabled = true;
            CMD_Button[0].Enabled = true;
            //明細部入力フェーズ
            TXT_HinNm.Enabled = false;
            TXT_Himoku.Enabled = false;
            TXT_Utiwake.Enabled = false;
            TXT_Tanaban.Enabled = false;
            CBO_Tani.Enabled = false;
            TXT_Syubetu.Enabled = false;
            TXT_SuibunKbn.Enabled = false;
            TXT_KensyuKbn.Enabled = false;
            TXT_KeiriKbn.Enabled = false;
            CBO_Mukesaki.Enabled = false;
            CBO_Syuko.Enabled = false;
            CMD_Button[1].Enabled = true;
            CMD_Button[2].Enabled = true;
            CMD_Button[3].Enabled = false;
            CMD_Button[4].Enabled = false;
        }

        private void CBO_Mukesaki_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            WK_Mukesaki = Convert.ToString(CBO_Mukesaki.SelectedIndex);
            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
        }

        private void CBO_Mukesaki_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (CBO_Mukesaki.SelectedIndex == Convert.ToDouble(WK_Mukesaki))
            {
                IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
                return;
            }
            if (TXT_Syubetu.Text == "2")
            {
                if (CBO_Mukesaki.SelectedIndex == -1)
                {
                    CBO_Mukesaki.Focus();
                    CBO_Mukesaki_Enter(CBO_Mukesaki, new System.EventArgs());
                    //必須項目入力エラー
                    IPPAN.Error_Msg("E200", 0, " ");
                    return;
                }
            }
            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
        }

        private void CBO_Syuko_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            WK_Syuko = CBO_Syuko.SelectedIndex;
            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
        }

        private void CBO_Syuko_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (CBO_Syuko.SelectedIndex == WK_Syuko)
            {
                IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
                return;
            }
            if (CBO_Syuko.SelectedIndex == -1)
            {
                CBO_Syuko.Focus();
                CBO_Syuko_Enter(CBO_Syuko, new System.EventArgs());
                //必須項目入力エラー
                IPPAN.Error_Msg("E200", 0, " ");
                return;
            }

            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
        }

        private void CBO_Tani_SelectedIndexChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            CBO_Tani.Text = Strings.Trim(CBO_Tani.Text);
        }

        private void CBO_Tani_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            WK_Tani = CBO_Tani.Text;
            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
            //ＩＭＥ全角ひらがな
            CBO_Tani.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
        }

        private void CBO_Tani_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (Strings.Trim(CBO_Tani.Text) == WK_Tani)
            {
                IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
                return;
            }

            CBO_Tani.Text = IPPAN2.NIBYTE_HENKAN(CBO_Tani.Text, 2);
            if (IPPAN.C_Allspace(CBO_Tani.Text) == 0)
            {
                CBO_Tani.Focus();
                CBO_Tani_Enter(CBO_Tani, new System.EventArgs());
                //必須項目入力エラー
                IPPAN.Error_Msg("E200", 0, " ");
                return;
            }
            if (IPPAN.Input_Check(CBO_Tani.Text) == 1)
            {
                CBO_Tani.Focus();
                CBO_Tani_Enter(CBO_Tani, new System.EventArgs());
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
            ButtonIndex = Index;
            IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_WAIT);

            switch (Index)
            {
                case 0:

                    //13.07.03  ISV-TRUC    Visible=Flase 既存項目で、不要になる項目改修工数削減の為、非表示に設定して利用します。
                    //ＯＫ
                    if (CKSI0130.G_OfficeId.Equals("KOUBAI"))
                    {
                        if (IPPAN.C_Allspace(TXT_HinCd.Text) == 0)
                        {
                            TXT_HinCd.Focus();
                            TXT_HinCd_Enter(TXT_HinCd, new System.EventArgs());
                            //必須項目入力エラー
                            IPPAN.Error_Msg("E200", 0, " ");
                            IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                            return;
                        }
                        else if (IPPAN.Input_Check(TXT_HinCd.Text) == 1)
                        {
                            TXT_HinCd.Focus();
                            TXT_HinCd_Enter(TXT_HinCd, new System.EventArgs());
                            //入力エラー
                            IPPAN.Error_Msg("E202", 0, " ");
                            IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                            return;
                        }
                        //棚番下４桁の入力チェック（新規時）
                        if (OPT_Sinki.Checked == true)
                        {
                            if (Strings.Len(TXT_HinCd.Text) < 4)
                            {
                                TXT_HinCd.Focus();
                                TXT_HinCd_Enter(TXT_HinCd, new System.EventArgs());
                                //入力エラー
                                IPPAN.Error_Msg("E202", 0, " ");
                                IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                                return;
                            }
                        }
                    }
                    //副資材品目マスタの存在チェック
                    IPPAN.G_RET = CKSI0130.Fs_Hinmoku_Kensaku(TXT_HinCd.Text);
                    //処理区分が新規かどうか
                    if (OPT_Sinki.Checked == true)
                    {
                        if (IPPAN.G_RET == 0)
                        {
                            TXT_HinCd.Focus();
                            TXT_HinCd_Enter(TXT_HinCd, new System.EventArgs());
                            //既に登録済みです
                            IPPAN.Error_Msg("E500", 0, " ");
                            IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                            return;
                        }
                    }
                    else
                    {
                        if (IPPAN.G_RET == 1)
                        {
                            TXT_HinCd.Focus();
                            TXT_HinCd_Enter(TXT_HinCd, new System.EventArgs());
                            //対象データがありません
                            IPPAN.Error_Msg("E536", 0, " ");
                            IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                            return;
                        }
                    }
                    //項目移送
                    if (OPT_Sinki.Checked != true)
                    {
                        TXT_HinNm.Text = Strings.Trim(Strings.Mid(CKSI0130.G_FS_HINMOKUNM, 1, 20));
                        TXT_Syubetu.Text = Strings.Mid(Strings.Trim(CKSI0130.G_FS_SYUBETU), 1, 1);
                        TXT_Himoku.Text = Strings.Trim(CKSI0130.G_FS_HIMOKU);
                        if (CKSI0130.G_FS_SYUBETU != "1")
                        {
// 2015.02.25 nishi OBIC7仕訳連携 Step3対応 start.
                            if (CKSI0130.Kamoku_Kensaku("110", CKSI0130.G_FS_HIMOKU) == 1)
                            {
                                LBL_HimokuNm.Text = "";
                                TXT_Himoku.Focus();
                                TXT_Himoku_Enter(TXT_Himoku, new System.EventArgs());
                                //科目マスタが見つかりません
                                IPPAN.Error_Msg("E520", 0, " ");
                                IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                                return;
                            }
                            LBL_HimokuNm.Text = CKSI0130.G_Kamokunm;
                            //if (CKSI0130.Himoku_Kensaku(CKSI0130.G_FS_HIMOKU) == 1)
                            //{
                            //    LBL_HimokuNm.Text = "";
                            //    TXT_Himoku.Focus();
                            //    TXT_Himoku_Enter(TXT_Himoku, new System.EventArgs());
                            //    //費目マスタが見つかりません
                            //    IPPAN.Error_Msg("E521", 0, " ");
                            //    IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT); //13.08.20 tsunamoto INSERT
                            //    return;
                            //}
                            //LBL_HimokuNm.Text = CKSI0130.G_Himokunm;
// 2015.02.25 nishi OBIC7仕訳連携 Step3対応 end.
                        }
                        else
                        {
                            if (CKSI0130.Kamoku_Kensaku("111", CKSI0130.G_FS_HIMOKU) == 1)
                            {
                                LBL_HimokuNm.Text = "";
                                TXT_Himoku.Focus();
                                TXT_Himoku_Enter(TXT_Himoku, new System.EventArgs());
                                //科目マスタが見つかりません
                                IPPAN.Error_Msg("E520", 0, " ");
                                IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT); //13.08.20 tsunamoto INSERT
                                return;
                            }
                            LBL_HimokuNm.Text = CKSI0130.G_Kamokunm;
                        }
                        TXT_Utiwake.Text = Strings.Trim(CKSI0130.G_FS_UTIWAKE);
                        if (CKSI0130.Utiwake_Kensaku(CKSI0130.G_FS_HIMOKU, CKSI0130.G_FS_UTIWAKE) == 1)
                        {
                            LBL_UtiwakeNm.Text = "";
                            TXT_Utiwake.Focus();
                            TXT_Utiwake_Enter(TXT_Utiwake, new System.EventArgs());
                            //製造費、費目内訳マスタが見つかりません
                            IPPAN.Error_Msg("E522", 0, " ");
                            IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT); //13.08.20 tsunamoto INSERT
                            return;
                        }
                        LBL_UtiwakeNm.Text = CKSI0130.G_Utiwakenm;
                        TXT_Tanaban.Text = Strings.Trim(CKSI0130.G_FS_TANABAN);
                        CBO_Tani.Text = Strings.Trim(CKSI0130.G_FS_TANI);
                        TXT_SuibunKbn.Text = Strings.Trim(CKSI0130.G_FS_SUIBUNKBN);
                        TXT_KensyuKbn.Text = Strings.Trim(CKSI0130.G_FS_KENSYUKBN);
                        TXT_KeiriKbn.Text = Strings.Trim(CKSI0130.G_FS_HOUKOKUKBN);
                        switch (CKSI0130.G_FS_MUKESAKI)
                        {
                            case "1":
                                CBO_Mukesaki.SelectedIndex = 0;
                                break;
                            case "2":
                                CBO_Mukesaki.SelectedIndex = 1;
                                break;
                            case "3":
                                CBO_Mukesaki.SelectedIndex = 2;
                                break;
                            case "4":
                                CBO_Mukesaki.SelectedIndex = 3;
                                break;
                            case "5":
                                CBO_Mukesaki.SelectedIndex = 4;
                                break;
                            case "6":
                                CBO_Mukesaki.SelectedIndex = 5;
                                break;
                            case "7":
                                CBO_Mukesaki.SelectedIndex = 6;
                                break;
                            case "8":
                                CBO_Mukesaki.SelectedIndex = 7;
                                break;
                            default:
                                CBO_Mukesaki.SelectedIndex = -1;
                                break;
                        }
                        switch (CKSI0130.G_FS_ICHIKBN)
                        {
                            case "1":
                                CBO_Syuko.SelectedIndex = 0;
                                break;
                            case "2":
                                CBO_Syuko.SelectedIndex = 1;
                                break;
                            case "3":
                                CBO_Syuko.SelectedIndex = 2;
                                break;
                            case "4":
                                CBO_Syuko.SelectedIndex = 3;
                                break;
                            case "5":
                                CBO_Syuko.SelectedIndex = 4;
                                break;
                            case "6":
                                CBO_Syuko.SelectedIndex = 5;
                                break;
                            case "7":
                                CBO_Syuko.SelectedIndex = 6;
                                break;
                            case "8":
                                CBO_Syuko.SelectedIndex = 7;
                                break;
                            default:
                                CBO_Syuko.SelectedIndex = -1;
                                break;
                        }
                    }
                    else
                    {
                        TXT_KensyuKbn.Text = "1";
                        TXT_KeiriKbn.Text = "1";
                    }
                    //プロテクト処理
                    if (OPT_Sakjyo.Checked == true)
                    {
                        SYORI_C();
                        CMD_Button[4].Focus();
                    }
                    else
                    {
                        SYORI_B();
                        TXT_HinNm.Focus();
                        TXT_HinNm_Enter(TXT_HinNm, new System.EventArgs());
                    }
                    break;

                case 1:

                    //閉じる            
                    this.Close();
                    break;

                case 2:
                    //クリア
                    Gamen_Clear();

                    //  13.07.03    ISV-TRUC    クリアボタンの押下時資材品目メンテ画面表示
                    if (CKSI0130.G_OfficeId.Equals("KOUBAI"))
                    {
                        SYORI_A();
                        TXT_HinCd.Focus();
                        TXT_HinCd_Enter(TXT_HinCd, new System.EventArgs());
                    }
                    else
                    {
                        //検収明細出力区分と経理報告区分へ "1"を設定する。
                        this.TXT_KensyuKbn.Text = "1";
                        this.TXT_KeiriKbn.Text = "1";
                        if (this.OPT_Sinki.Checked)
                        {
                            TXT_HinCd.Focus();
                            TXT_HinCd_Enter(TXT_HinCd, new System.EventArgs());
                        }
                        else
                        {
                            TXT_HinNm.Focus();
                            TXT_HinNm_Enter(TXT_HinNm, new System.EventArgs());
                        }
                    }
                    //  13.07.03    ISV-TRUC end
                    IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                    break;

                case 3:
                    //  13.07.03    ISV-TRUC    入力データをチェック
                    if (!CKSI0130.G_OfficeId.Equals("KOUBAI"))
                    {
                        if (IPPAN.C_Allspace(TXT_HinCd.Text) == 0)
                        {
                            TXT_HinCd.Focus();
                            TXT_HinCd_Enter(TXT_HinCd, new System.EventArgs());
                            //必須項目入力エラー
                            IPPAN.Error_Msg("E200", 0, " ");
                            IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                            return;
                        }
                        else if (IPPAN.Input_Check(TXT_HinCd.Text) == 1)
                        {
                            TXT_HinCd.Focus();
                            TXT_HinCd_Enter(TXT_HinCd, new System.EventArgs());
                            //入力エラー
                            IPPAN.Error_Msg("E202", 0, " ");
                            IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                            return;
                        }
                    }
                    // End 13.07.03    ISV-TRUC 

                    //単価設定
                    if (Gamen_Check() != 0)
                    {
                        IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                        return;
                    }

                    Set_HinmokuPara();
                    
                    this.TMR_Timer.Enabled = false;

                    //モーダル表示
                    FRM_CKSI0130S01 fm = new FRM_CKSI0130S01();
                    fm.HinmokuCD = TXT_HinCd.Text;
                    fm.HinmokuNM = TXT_HinNm.Text;

                    fm.ShowDialog(this);
                    fm.Dispose();
                    fm = null;

                    this.TMR_Timer.Enabled = true;

                    if (CKSI0130.CKSI0130S01_RET == 1)
                    {
                        TXT_HinCd.Text = "";
                        TXT_HinNm.Text = "";
                        TXT_Himoku.Text = "";
                        TXT_Utiwake.Text = "";
                        TXT_Tanaban.Text = "";
                        CBO_Tani.Text = "";
                        CBO_Tani.SelectedIndex = -1;
                        TXT_Syubetu.Text = "";
                        TXT_SuibunKbn.Text = "";
                        TXT_KensyuKbn.Text = "";
                        TXT_KeiriKbn.Text = "";
                        CBO_Mukesaki.SelectedIndex = -1;
                        CBO_Syuko.SelectedIndex = -1;
                        LBL_HimokuNm.Text = "";
                        LBL_UtiwakeNm.Text = "";
                        //  13.07.03    ISV-TRUC
                        if (CKSI0130.G_OfficeId.Equals("KOUBAI"))
                        {
                             SYORI_A();
                        }
                        // End 13.07.03    ISV-TRUC
                        OPT_Sinki.Checked = true;
                        IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                        TXT_HinCd.Focus();
                        TXT_HinCd_Enter(TXT_HinCd, new System.EventArgs());
                    }
                    break;
                case 4:
                    //  13.07.03    ISV-TRUC    入力データをチェック
                    if (!CKSI0130.G_OfficeId.Equals("KOUBAI"))
                    {
                        if (IPPAN.C_Allspace(TXT_HinCd.Text) == 0)
                        {
                            TXT_HinCd.Focus();
                            TXT_HinCd_Enter(TXT_HinCd, new System.EventArgs());
                            //必須項目入力エラー
                            IPPAN.Error_Msg("E200", 0, " ");
                            IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                            return;
                        }
                        else if (IPPAN.Input_Check(TXT_HinCd.Text) == 1)
                        {
                            TXT_HinCd.Focus();
                            TXT_HinCd_Enter(TXT_HinCd, new System.EventArgs());
                            //入力エラー
                            IPPAN.Error_Msg("E202", 0, " ");
                            IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                            return;
                        }
                    }
                    //  End 13.07.03    ISV-TRUC  

                    //入力確定
                    using (C_ODBC db = new C_ODBC())
                    {
                        try
                        {
                            //DB接続
                            db.Connect();
                            //トランザクション開始
                            db.BeginTrans();

                            if (OPT_Sakjyo.Checked == true)
                            {
                                //確認メッセージ
                                dlgRlt = IPPAN.Error_Msg("I014", 4, " ");
                                // [はい] ボタンを選択した場合
                                if (dlgRlt == DialogResult.Yes)
                                {
                                    IPPAN.G_SQL = "Delete FROM SIZAI_TANKA_MST ";
                                    IPPAN.G_SQL = IPPAN.G_SQL + "WHERE HINMOKUCD ='" + this.TXT_HinCd.Text + "'";

                                    //SQL実行
                                    db.ExecSQL(IPPAN.G_SQL);

                                    IPPAN.G_SQL = "Delete FROM SIZAI_HINMOKU_MST ";
                                    IPPAN.G_SQL = IPPAN.G_SQL + "WHERE HINMOKUCD ='" + TXT_HinCd.Text + "'";
                                }
                                else
                                {
                                    Gamen_Clear();
                                    if (CKSI0130.G_OfficeId.Equals("KOUBAI"))
                                    {
                                        SYORI_A();
                                    }
                                    TXT_HinCd.Focus();
                                    TXT_HinCd_Enter(TXT_HinCd, new System.EventArgs());
                                    IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                                    return;
                                }
                            }
                            else
                            {
                                if (Gamen_Check() != 0)
                                {
                                    IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                                    return;
                                }

                                //確認メッセージ
                                if (OPT_Sinki.Checked == true)
                                {
                                    //登録しますか
                                    dlgRlt = IPPAN.Error_Msg("I607", 4, " ");
                                }
                                else
                                {
                                    //変更しますか
                                    dlgRlt = IPPAN.Error_Msg("I608", 4, " ");
                                }
                                // [いいえ] ボタンを選択した場合
                                if (dlgRlt == DialogResult.No)
                                {
                                    SYORI_B();
                                    TXT_HinNm.Focus();
                                    TXT_HinNm_Enter(TXT_HinNm, new System.EventArgs());
                                    IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                                    return;
                                }

                                Set_HinmokuPara();

                                //ＳＱＬ文構築
                                if (OPT_Henko.Checked == true)
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
                                if (OPT_Sinki.Checked == true)
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
                            }
                            //排他チェック（ＯＫｸﾘｯｸ時と現在の UPDYMD を比較）
                            IPPAN.G_RET = CKSI0130.SIZAI_HINMOKU_CHECK(db, OPT_Sakjyo.Checked, OPT_Sinki.Checked, TXT_HinCd.Text);
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
                                TXT_HinCd.Text = "";
                                TXT_HinNm.Text = "";
                                TXT_Himoku.Text = "";
                                TXT_Utiwake.Text = "";
                                TXT_Tanaban.Text = "";
                                CBO_Tani.Text = "";
                                CBO_Tani.SelectedIndex = -1;
                                TXT_Syubetu.Text = "";
                                TXT_SuibunKbn.Text = "";
                                TXT_KensyuKbn.Text = "";
                                TXT_KeiriKbn.Text = "";
                                CBO_Mukesaki.SelectedIndex = -1;
                                CBO_Syuko.SelectedIndex = -1;
                                LBL_HimokuNm.Text = "";
                                LBL_UtiwakeNm.Text = "";

                                //コミット
                                db.Commit();

                                if (CKSI0130.G_OfficeId.Equals("KOUBAI"))
                                {
                                    SYORI_A();
                                }
                                IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                                TXT_HinCd.Focus();
                                TXT_HinCd_Enter(TXT_HinCd, new System.EventArgs());
                                return;
                            }

                            //SQL実行
                            db.ExecSQL(IPPAN.G_SQL);
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

                    //   13.07.03    ISV-TRUC      現行の登録処理を実行後、画面を閉じます。
                    if (CKSI0130.G_OfficeId.Equals("KOUBAI"))
                    {
                        TXT_HinCd.Text = "";
                        TXT_HinNm.Text = "";
                        TXT_Himoku.Text = "";
                        TXT_Utiwake.Text = "";
                        TXT_Tanaban.Text = "";
                        CBO_Tani.Text = "";
                        CBO_Tani.SelectedIndex = -1;
                        TXT_Syubetu.Text = "";
                        TXT_SuibunKbn.Text = "";
                        TXT_KensyuKbn.Text = "";
                        TXT_KeiriKbn.Text = "";
                        CBO_Mukesaki.SelectedIndex = -1;
                        CBO_Syuko.SelectedIndex = -1;
                        LBL_HimokuNm.Text = "";
                        LBL_UtiwakeNm.Text = "";
                        SYORI_A();
                        IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                        TXT_HinCd.Focus();
                        TXT_HinCd_Enter(TXT_HinCd, new System.EventArgs());
                    }
                    else
                    {

                        //  単位の値をリセットする。
                        CKSI0130.G_FS_TANI = "";
                        //  現行の登録処理を実行後、画面を閉じます。
                        HinmokuCD = TXT_HinCd.Text;
                        this.Close();
                    }
                    // End  13.07.03    ISV-TRUC
                    break;
            }

            IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
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

        private void FRM_CKSI0130M_Load(System.Object eventSender, System.EventArgs eventArgs)
        {
            string L_Search = null;
            string L_Command2 = null;

            //プロジェクト名を設定
            C_COMMON.G_COMMON_MSGTITLE = "CKSI0130";

            //コントロール配列にイベントを設定
            setEventCtrlAry();

            Show();

            //待機する秒数
            const short Waittime = 2;
            //起動時刻を待避
            System.DateTime now_old = DateAndTime.Now;
            
            //現在の時刻－起動時刻が待機する秒数になるまでループする
            while (!(DateTime.FromOADate(DateAndTime.Now.ToOADate() - Convert.ToDateTime(now_old).ToOADate()) >= DateAndTime.TimeSerial(0, 0, Waittime)))
            {
                System.Windows.Forms.Application.DoEvents();
            }

            //引数のチェック
            IPPAN.G_COMMAND = Interaction.Command();

            L_Search = ",";

            if (string.IsNullOrEmpty(IPPAN.G_COMMAND))
            {
                // コマンド ライン引数が指定されていないとき。
                //コマンド ライン引数が指定されていません
                IPPAN.Error_Msg("E507", 0, " ");
                System.Environment.Exit(0);
            }
            else
            {
                // コマンド ライン引数の受け渡し
                CKSI0130.G_UserId = Strings.Mid(IPPAN.G_COMMAND, 1, Strings.InStr(1, IPPAN.G_COMMAND, L_Search, CompareMethod.Binary) - 1);
                L_Command2 = Strings.Mid(IPPAN.G_COMMAND, Strings.InStr(1, IPPAN.G_COMMAND, L_Search, CompareMethod.Binary) + 1, 30);
                CKSI0130.G_OfficeId = Strings.Mid(L_Command2, 1, Strings.InStr(1, L_Command2, L_Search, CompareMethod.Binary) - 1);
                CKSI0130.G_SyokuiCd = Strings.Mid(L_Command2, Strings.InStr(1, L_Command2, L_Search, CompareMethod.Binary) + 1, 2);

                // 13.07.03    ISV-TRUC 
                ////購買以外の時は副資材ユーザかどうかの判断
                //if (CKSI0130.G_OfficeId != "KOUBAI")
                //{
                //    IPPAN.G_RET = FS_USER_CHECK.User_Check(CKSI0130.G_UserId);
                //    //ユーザーＩＤが副資材ユーザ以外かを見る
                //    if (IPPAN.G_RET != 0)
                //    {
                //        System.Environment.Exit(0);
                //    }
                //}
                //  13.07.03    ISV-TRUC end
                
                IPPAN.G_RET = IPPAN2.TIMER_SET(CKSI0130.G_UserId);
                if (IPPAN.G_RET != 0)
                {
                    System.Environment.Exit(0);
                }
            }
            
            SYORI_A();
            
            this.OPT_Sinki.Focus();
            
            if (Tani_Kensaku() != 0)
            {
                //単位マスタが見つかりません
                IPPAN.Error_Msg("E106", 0, " ");
                return;
            }
            
            // 13.07.03    ISV-TRUC    資材品目メンテ画面　初期表示
            if (!CKSI0130.G_OfficeId.Equals("KOUBAI"))
            {
                switch (ProcKBN)
                {
                    case CKSI0130.Mode.INSERT_MODE:
                        this.OPT_Sinki.Checked = true;
                        break;

                    case CKSI0130.Mode.UPDATE_MODE:
                        this.OPT_Henko.Checked = true;
                        break;
                }
                //Visible=Flase
                this.Frame2.Visible = false;

                //資材品目メンテ画面　プロパティ
                TXT_HinCd.Text = HinmokuCD;

                //ＯＫボタンのクリックイベント
                this.CMD_Button_Click(this._CMD_Button_0, null);

                if (this.OPT_Sinki.Checked)
                {
                    //品目CD Enable =true  をセット
                    this.TXT_HinCd.Enabled = true;
                    this.TXT_HinCd.Focus();
                }
            }
            else
            {
                TXT_HinCd.Location = new Point(484, 27);
                LBL_HinCd.Location = new Point(391, 29);
                Frame2.Controls.Add(TXT_HinCd);
                Frame2.Controls.Add(LBL_HinCd);
                TXT_HinCd.TabIndex = 1;
                this.TXT_HinCd.Focus();
            }
            //End  13.07.03    ISV-TRUC    
        }

        private void OPT_Henko_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            RadioButton rdBtn = (RadioButton)eventSender;
            if (rdBtn.Checked)
            {
                TXT_HinCd.Focus();
            }
        }

        private void OPT_Sakjyo_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            RadioButton rdBtn = (RadioButton)eventSender;
            if (rdBtn.Checked)
            {
                TXT_HinCd.Focus();
            }
        }

        private void OPT_Sinki_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            RadioButton rdBtn = (RadioButton)eventSender;
            if (rdBtn.Checked)
            {
                TXT_HinCd.Focus();
            }
        }

        private void TMR_TIMER_Tick(System.Object eventSender, System.EventArgs eventArgs)
        {
            IPPAN.Control_Init(this, "品目マスタメンテ", "CKSI0130", SO.SO_USERNAME, SO.SO_OFFICENAME, "1.00");
        }

        private void TXT_Himoku_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            WK_Himoku = TXT_Himoku.Text;
            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
        }

        private void TXT_Himoku_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (Strings.Trim(TXT_Himoku.Text) == WK_Himoku)
            {
                IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
                return;
            }
            if (IPPAN.C_Allspace(TXT_Himoku.Text) == 0)
            {
                LBL_HimokuNm.Text = "";
                TXT_Himoku.Focus();
                TXT_Himoku_Enter(TXT_Himoku, new System.EventArgs());
                //必須項目入力エラー
                IPPAN.Error_Msg("E200", 0, " ");
                return;
            }
            if (IPPAN.Input_Check(TXT_Himoku.Text) == 1)
            {
                LBL_HimokuNm.Text = "";
                TXT_Himoku.Focus();
                TXT_Himoku_Enter(TXT_Himoku, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                return;
            }
            IPPAN.G_RET = IPPAN.Numeric_Check2(TXT_Himoku.Text);
            if (IPPAN.G_RET == 1)
            {
                TXT_Himoku.Focus();
                TXT_Himoku_Enter(TXT_Himoku, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                return;
            }

            if (TXT_Syubetu.Text != "1")
            {
// 2015.02.25 nishi OBIC7仕訳連携 Step3対応 start.
                if (CKSI0130.Kamoku_Kensaku("110", TXT_Himoku.Text) == 1)
                {
                    LBL_HimokuNm.Text = "";
                    TXT_Himoku.Focus();
                    TXT_Himoku_Enter(TXT_Himoku, new System.EventArgs());
                    //科目マスタが見つかりません
                    IPPAN.Error_Msg("E520", 0, " ");
                    return;
                }
                LBL_HimokuNm.Text = CKSI0130.G_Kamokunm;
                //if (CKSI0130.Himoku_Kensaku(TXT_Himoku.Text) == 1)
                //{
                //    LBL_HimokuNm.Text = "";
                //    TXT_Himoku.Focus();
                //    TXT_Himoku_Enter(TXT_Himoku, new System.EventArgs());
                //    //費目マスタが見つかりません
                //    IPPAN.Error_Msg("E521", 0, " ");
                //    return;
                //}
                //LBL_HimokuNm.Text = CKSI0130.G_Himokunm;
// 2015.02.25 nishi OBIC7仕訳連携 Step3対応 end.
            }
            else
            {
                if (CKSI0130.Kamoku_Kensaku("111", TXT_Himoku.Text) == 1)
                {
                    LBL_HimokuNm.Text = "";
                    TXT_Himoku.Focus();
                    TXT_Himoku_Enter(TXT_Himoku, new System.EventArgs());
                    //科目マスタが見つかりません
                    IPPAN.Error_Msg("E520", 0, " ");
                    return;
                }
                LBL_HimokuNm.Text = CKSI0130.G_Kamokunm;
            }
            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
        }

        private void TXT_HinCd_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
        }

        private void TXT_HinCd_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {
            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
        }

        private void TXT_HinNm_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            WK_HinNm = TXT_HinNm.Text;
            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
        }

        private void TXT_HinNm_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (Strings.Trim(TXT_HinNm.Text) == WK_HinNm)
            {
                IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
                return;
            }
            if (IPPAN.C_Allspace(TXT_HinNm.Text) == 0)
            {
                TXT_HinNm.Focus();
                TXT_HinNm_Enter(TXT_HinNm, new System.EventArgs());
                //必須項目入力エラー
                IPPAN.Error_Msg("E200", 0, " ");
                return;
            }

            TXT_HinNm.Text = IPPAN2.NIBYTE_HENKAN(TXT_HinNm.Text, 20);
            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
        }

        private void TXT_KeiriKbn_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            WK_KeiriKbn = TXT_KeiriKbn.Text;
            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
        }

        private void TXT_KeiriKbn_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (Strings.Trim(TXT_KeiriKbn.Text) == WK_KeiriKbn)
            {
                IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
                return;
            }
            if (IPPAN.Input_Check(TXT_KeiriKbn.Text) == 1)
            {
                TXT_KeiriKbn.Focus();
                TXT_KeiriKbn_Enter(TXT_KeiriKbn, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                return;
            }
            if (TXT_KeiriKbn.Text == "1" || string.IsNullOrEmpty(TXT_KeiriKbn.Text) || TXT_KeiriKbn.Text == " ")
            {
            }
            else
            {
                TXT_KeiriKbn.Focus();
                TXT_KeiriKbn_Enter(TXT_KeiriKbn, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                return;
            }
            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
        }

        private void TXT_KensyuKbn_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            WK_KensyuKbn = TXT_KensyuKbn.Text;
            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
        }

        private void TXT_KensyuKbn_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (Strings.Trim(TXT_KensyuKbn.Text) == WK_KensyuKbn)
            {
                IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
                return;
            }
            if (IPPAN.Input_Check(TXT_KensyuKbn.Text) == 1)
            {
                TXT_KensyuKbn.Focus();
                TXT_KensyuKbn_Enter(TXT_KensyuKbn, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                return;
            }
            if (TXT_KensyuKbn.Text == "1" || string.IsNullOrEmpty(TXT_KensyuKbn.Text) || TXT_KensyuKbn.Text == " ")
            {
            }
            else
            {
                TXT_KensyuKbn.Focus();
                TXT_KensyuKbn_Enter(TXT_KensyuKbn, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                return;
            }
            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
        }

        private void TXT_SuibunKbn_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            WK_SuibunKbn = TXT_SuibunKbn.Text;
            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
        }

        private void TXT_SuibunKbn_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (Strings.Trim(TXT_SuibunKbn.Text) == WK_SuibunKbn)
            {
                IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
                return;
            }
            if (IPPAN.Input_Check(TXT_SuibunKbn.Text) == 1)
            {
                TXT_SuibunKbn.Focus();
                TXT_SuibunKbn_Enter(TXT_SuibunKbn, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                return;
            }
            if (TXT_SuibunKbn.Text == "1" || string.IsNullOrEmpty(TXT_SuibunKbn.Text) || TXT_SuibunKbn.Text == " ")
            {
            }
            else
            {
                TXT_SuibunKbn.Focus();
                TXT_SuibunKbn_Enter(TXT_SuibunKbn, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                return;
            }
            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
        }

        private void TXT_Syubetu_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            WK_Syubetu = TXT_Syubetu.Text;
            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
        }

        private void TXT_Syubetu_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (Strings.Trim(TXT_Syubetu.Text) == WK_Syubetu)
            {
                IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
                return;
            }
            if (IPPAN.C_Allspace(TXT_Syubetu.Text) == 0)
            {
                TXT_Syubetu.Focus();
                TXT_Syubetu_Enter(TXT_Syubetu, new System.EventArgs());
                //必須項目入力エラー
                IPPAN.Error_Msg("E200", 0, " ");
                return;
            }
            if (IPPAN.Input_Check(TXT_Syubetu.Text) == 1)
            {
                TXT_Syubetu.Focus();
                TXT_Syubetu_Enter(TXT_Syubetu, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                return;
            }
            IPPAN.G_RET = IPPAN.Numeric_Check2(TXT_Syubetu.Text);
            if (IPPAN.G_RET == 1)
            {
                TXT_Syubetu.Focus();
                TXT_Syubetu_Enter(TXT_Syubetu, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                return;
            }
            if (TXT_Syubetu.Text == "1" || TXT_Syubetu.Text == "2")
            {
            }
            else
            {
                TXT_Syubetu.Focus();
                TXT_Syubetu_Enter(TXT_Syubetu, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                return;
            }
            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
        }

        private void TXT_Tanaban_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            WK_Tanaban = TXT_Tanaban.Text;
            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
        }

        private void TXT_Tanaban_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (Strings.Trim(TXT_Tanaban.Text) == WK_Tanaban)
            {
                IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
                return;
            }
            if (IPPAN.C_Allspace(TXT_Tanaban.Text) == 0)
            {
                TXT_Tanaban.Focus();
                TXT_Tanaban_Enter(TXT_Tanaban, new System.EventArgs());
                //必須項目入力エラー
                IPPAN.Error_Msg("E200", 0, " ");
                return;
            }
            if (IPPAN.Input_Check(TXT_Tanaban.Text) == 1)
            {
                TXT_Tanaban.Focus();
                TXT_Tanaban_Enter(TXT_Tanaban, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                return;
            }
            IPPAN.G_RET = IPPAN.Numeric_Check2(TXT_Tanaban.Text);
            if (IPPAN.G_RET == 1)
            {
                TXT_Tanaban.Focus();
                TXT_Tanaban_Enter(TXT_Tanaban, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                return;
            }
            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
        }

        private void TXT_Utiwake_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            WK_Utiwake = TXT_Utiwake.Text;
            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
        }

        private void TXT_Utiwake_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (Strings.Trim(TXT_Utiwake.Text) == WK_Utiwake)
            {
                IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
                return;
            }
            if (IPPAN.C_Allspace(TXT_Utiwake.Text) == 0)
            {
                LBL_UtiwakeNm.Text = "";
                TXT_Utiwake.Focus();
                TXT_Utiwake_Enter(TXT_Utiwake, new System.EventArgs());
                //必須項目入力エラー
                IPPAN.Error_Msg("E200", 0, " ");
                return;
            }
            if (IPPAN.Input_Check(TXT_Utiwake.Text) == 1)
            {
                LBL_UtiwakeNm.Text = "";
                TXT_Utiwake.Focus();
                TXT_Utiwake_Enter(TXT_Utiwake, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                return;
            }

            IPPAN.G_RET = IPPAN.Numeric_Check2(TXT_Utiwake.Text);
            if (IPPAN.G_RET == 1)
            {
                LBL_UtiwakeNm.Text = "";
                TXT_Utiwake.Focus();
                TXT_Utiwake_Enter(TXT_Utiwake, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                return;
            }
            if (CKSI0130.Utiwake_Kensaku(TXT_Himoku.Text, TXT_Utiwake.Text) == 1)
            {
                LBL_UtiwakeNm.Text = "";
                TXT_Utiwake.Focus();
                TXT_Utiwake_Enter(TXT_Utiwake, new System.EventArgs());
                //製造費、費目内訳マスタが見つかりません
                IPPAN.Error_Msg("E522", 0, " ");
                return;
            }
            LBL_UtiwakeNm.Text = CKSI0130.G_Utiwakenm;
            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
        }

        private void Frame2_Leve(object sender, EventArgs e)
        {
            TXT_HinCd.Focus();
        }
    }
}

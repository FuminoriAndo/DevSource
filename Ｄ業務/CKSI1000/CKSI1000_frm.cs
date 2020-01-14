using Microsoft.VisualBasic;
using System;
using System.Windows.Forms;

namespace Project1
{
    internal partial class FRM_CKSI1000M : System.Windows.Forms.Form
    {
        //修正年月日   担当者      修正内容
        //99.04.20    NTIS        問合せ表示内容制限マスタ検索
        //99.04.22    NTIS        問合せ条件変更
        //99.05.07    NTIS        問合せマスタ検索キー変更
        //03.01.15    NTIS浅田　　Ｓ／Ｏサーバ更新の為、VB4.0→VB6.0SP5へのバージョンアップ対応
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FRM_CKSI1000M());
        }

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

        //画面入力不可
        private void Enabled_False()
        {
            TXT_HINCD.Enabled = false;
            LST_LIST.Enabled = true;
            CMD_BUTTON[0].Enabled = false;
        }

        //画面入力可
        private void Enabled_True()
        {
            TXT_HINCD.Enabled = true;
            LST_LIST.Enabled = false;
            CMD_BUTTON[0].Enabled = true;
        }

        private void CMD_Button_Click(System.Object eventSender, System.EventArgs eventArgs)
        {
            short Index = CMD_BUTTON.GetIndex((Button)eventSender);

            switch (Index)
            {
                case 0:
                    //検索
                    //ﾏｽｽﾎﾟｲﾝﾄの砂時計化
                    IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_WAIT);

                    //副資材棚卸トランの検索
                    IPPAN.G_RET = kensaku.Zaiko_Kensaku(TXT_HINCD.Text, LST_LIST);
                    TXT_HINCD.Focus();

                    //ﾏｳｽﾎﾟｲﾝﾄを元に戻す
                    IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                    if (IPPAN.G_RET == 1)
                    {
                        //画面入力可
                        Enabled_True();
                    }
                    else
                    {
                        //画面入力不可
                        Enabled_False();
                    }
                    //ﾏｳｽﾎﾟｲﾝﾄを元に戻す
                    IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                    break;

                case 1:
                    //クリア
                    //ﾏｽｽﾎﾟｲﾝﾄの砂時計化
                    IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_WAIT);
                    TXT_HINCD.Text = "";
                    LST_LIST.Items.Clear();
                    //画面入力可
                    Enabled_True();
                    //フォーカスセット
                    TXT_HINCD.Focus();
                    //ﾏｳｽﾎﾟｲﾝﾄを元に戻す
                    IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                    break;

                case 2:
                    //閉じる
                    this.Close();
                    break;
            }
        }

        private void setEventCtrlAry()
        {
            //コントロール配列にイベントを設定
            //CMD_Button
            for (short i = 0; i < this.CMD_BUTTON.Count(); i++)
            {
                this.CMD_BUTTON[i].Click += new EventHandler(CMD_Button_Click);
            }
        }

        private void FRM_CKSI1000M_Load(System.Object eventSender, System.EventArgs eventArgs)
        {
            //プロジェクト名を設定
            C_COMMON.G_COMMON_MSGTITLE = "CKSI1000";

            //コントロール配列にイベントを設定
            setEventCtrlAry();

            this.Show();

            //初期フォーカス設定
            this.TXT_HINCD.Focus();

            //待機する秒数
            const short Waittime = 2;
            //起動時刻を待避
            System.DateTime now_old = DateAndTime.Now;

            //現在の時刻－起動時刻が待機する秒数になるまでループする
            while (!(DateTime.FromOADate(DateAndTime.Now.ToOADate() - Convert.ToDateTime(now_old).ToOADate()) >= DateAndTime.TimeSerial(0, 0, Waittime)))
            {
                System.Windows.Forms.Application.DoEvents();
            }
        }

        private void TMR_Timer_Tick(System.Object eventSender, System.EventArgs eventArgs)
        {
            IPPAN.Control_Init(this, "在庫問合せ", "CKSI1000", SO.SO_USERNAME, SO.SO_OFFICENAME, "1.02");
        }

        private void TXT_HINCD_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
        }

        private void TXT_HINCD_KeyPress(System.Object eventSender, System.Windows.Forms.KeyPressEventArgs eventArgs)
        {
            int KeyAscii = Strings.Asc(eventArgs.KeyChar);
            if (KeyAscii == 13)
            {
                KeyAscii = 0;
                System.Windows.Forms.SendKeys.Send("{TAB}");
            }
            eventArgs.KeyChar = Strings.Chr(KeyAscii);
            if (KeyAscii == 0)
            {
                eventArgs.Handled = true;
            }
        }

        private void TXT_HINCD_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {
            kensaku.G_HINMOKU_CD = TXT_HINCD.Text;

            IPPAN.G_RET = IPPAN.C_Allspace(kensaku.G_HINMOKU_CD);

            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);

            if (IPPAN.G_RET == 0)
            {
                return;
            }

            //砂時計ポインタ
            IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_WAIT);
            IPPAN.G_RET = kensaku.Hinmoku_Kensaku(TXT_HINCD.Text);
            //標準ポインタ
            IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
            if (IPPAN.G_RET != 0)
            {
                TXT_HINCD.Focus();
                IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
                //副資材品目マスタが見つかりません。
                IPPAN.Error_Msg("E706", 0, " ");
                return;
            }
        }
    }
}

using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.Data.Odbc;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Project1
{
    internal partial class FRM_CKSI0060M : System.Windows.Forms.Form
    {
        //**************************************************************'
        //                                                              '
        //   プログラム名　　　購買要求申請画面処理                     '
        //                                                              '
        //   更新履歴
        //   更新日付    Rev     修正者      内容
        //   03.01.15           NTIS浅田　　Ｓ／Ｏサーバ更新の為、VB4.0→VB6.0SP5へのバージョンアップ対応
        //**************************************************************'
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FRM_CKSI0060M());
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

        private void CMD_Button_Click(System.Object eventSender, System.EventArgs eventArgs)
        {
            short Index = CMD_Button.GetIndex((Button)eventSender);
            DialogResult dlgRlt = DialogResult.None;

            switch (Index)
            {
                case 0:
                    //閉じる
                    this.Close();
                    break;
                case 1:
                    //実行
                    if (string.Compare(String.Format("{0:yyyyMM}", DateTime.Now), CKSI0060.G_SKYM) <= 0)
                    {
                        //購買検収データは既に作成済みです
                        IPPAN.Error_Msg("E704", 0, " ");
                        return;
                    }

                    //データを作成します。よろしいですか？
                    dlgRlt = IPPAN.Error_Msg("I701", 4, " ");
                    if (dlgRlt != DialogResult.Yes)
                    {
                        return;
                    }

                    CMD_Button[1].Enabled = false;
                    CMD_Button[0].Enabled = false;

                    IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_WAIT);

                    using (C_ODBC db = new C_ODBC())
                    {
                        try
                        {
                            //DB接続
                            db.Connect();
                            //トランザクション開始
                            db.BeginTrans();

                            LBL_MSG.Text = "購買検収データを作成しています．．．";
                            System.Windows.Forms.Application.DoEvents();
                            if (Kensyu_Kosin.Kensyu_Sakusei(db, this.LBL_Kensyuyy.Text, this.LBL_Kensyumm.Text) == 1)
                            {
                                //更新に失敗しました
                                IPPAN.Error_Msg("I503", 0, " ");
                                //SQLデータベース切断
                                db.Disconnect();
                                System.Environment.Exit(0);
                            }

                            LBL_MSG.Text = "コントロールマスタを更新しています．．．";
                            System.Windows.Forms.Application.DoEvents();
                            if (Control_Kosin(db) == 1)
                            {
                                //更新に失敗しました
                                IPPAN.Error_Msg("I503", 0, " ");
                                //SQLデータベース切断
                                db.Disconnect();
                                System.Environment.Exit(0);
                            }

                            //2017.01.14 yoshitake start
                            if (CKSI0060.G_START_SIZAI_TANAOROSI_SYSTEM.Equals("1"))
                            {
                                LBL_MSG.Text = "資材棚卸データを更新しています．．．";
                                System.Windows.Forms.Application.DoEvents();
                                //資材棚卸累積ワーク更新
                                if (Sizai_Tanaorosi_Ruiseki_Wrk_Insert(db) == 1)
                                {
                                    //更新に失敗しました
                                    IPPAN.Error_Msg("I503", 0, " ");
                                    //SQLデータベース切断
                                    db.Disconnect();
                                    System.Environment.Exit(0);
                                }
                                //資材棚卸累積ワーク2年前削除
                                if (Sizai_Tanaorosi_Ruiseki_Wrk_DELETE(db) == 1)
                                {
                                    //更新に失敗しました
                                    IPPAN.Error_Msg("I503", 0, " ");
                                    //SQLデータベース切断
                                    db.Disconnect();
                                    System.Environment.Exit(0);
                                }
                                //資材棚卸ワーク削除
                                if (Sizai_Tanaorosi_Wrk_DELETE(db) == 1)
                                {
                                    //更新に失敗しました
                                    IPPAN.Error_Msg("I503", 0, " ");
                                    //SQLデータベース切断
                                    db.Disconnect();
                                    System.Environment.Exit(0);
                                }
                                //資材棚卸立会い用ワーク削除
                                if (Sizai_Tanaorosi_Tatiai_Wrk_DELETE(db) == 1)
                                {
                                    //更新に失敗しました
                                    IPPAN.Error_Msg("I503", 0, " ");
                                    //SQLデータベース切断
                                    db.Disconnect();
                                    System.Environment.Exit(0);
                                }
                                //棚卸ログトランの不要なデータを削除
                                if (DeleteUnnesessaryData_Tanaorosi_Log_Trn(db) == 1)
                                {
                                    //更新に失敗しました
                                    IPPAN.Error_Msg("I503", 0, " ");
                                    //SQLデータベース切断
                                    db.Disconnect();
                                    System.Environment.Exit(0);
                                }
                                //棚卸詳細ログトランの不要なデータを削除
                                if (DeleteUnnesessaryData_Tanaorosi_Detail_Log_Trn(db) == 1)
                                {
                                    //更新に失敗しました
                                    IPPAN.Error_Msg("I503", 0, " ");
                                    //SQLデータベース切断
                                    db.Disconnect();
                                    System.Environment.Exit(0);
                                }
                                //棚卸進捗マスタ
                                if (Tanaorosi_Progress_MST_UPDATE(db) == 1)
                                {
                                    //更新に失敗しました
                                    IPPAN.Error_Msg("I503", 0, " ");
                                    //SQLデータベース切断
                                    db.Disconnect();
                                    System.Environment.Exit(0);
                                }
                                //棚卸会計期間マスタ
                                if (Tanaorosi_Kaikei_Kikan_MST_UPDATE(db) == 1)
                                {
                                    //更新に失敗しました
                                    IPPAN.Error_Msg("I503", 0, " ");
                                    //SQLデータベース切断
                                    db.Disconnect();
                                    System.Environment.Exit(0);
                                }

                            }
                            //2017.01.14 yoshitake end

                            //SQLトランザクション終了
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

                    //2013.08.27 DSK yoshida start
                    //IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                    //LBL_MSG.BackColor = System.Drawing.ColorTranslator.FromOle(Information.QBColor(9));
                    //LBL_MSG.ForeColor = System.Drawing.ColorTranslator.FromOle(Information.QBColor(15));
                    //LBL_MSG.Text = "処理は終了しました。閉じるボタンを押して下さい。";
                    //CMD_Button[0].Enabled = true;

                    //購買検収データ作成完了送信
                    try
                    {
                        System.Reflection.Assembly myAssembly = System.Reflection.Assembly.GetEntryAssembly();
                        string path = myAssembly.Location.Replace(myAssembly.ManifestModule.ToString(), string.Empty);
                        XDocument versionDoc = XDocument.Load(System.IO.Path.Combine(path, "CKSI0060.settings"));
                        IEnumerable<XElement> result = versionDoc.XPathSelectElements("hosts/host[@id]");

                        foreach (var item in result)
                        {
                            string ip = item.Element("ip").Value;
                            string port = item.Element("port").Value;

                            // TCPｸﾗｲｱﾝﾄを生成
                            System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient();   // TCPｸﾗｲｱﾝﾄ

                            // TCP/IP接続を行う
                            try
                            {
                                client.Connect(ip, int.Parse(port));
                            }
                            catch
                            {
                                throw new Exception(Properties.Resources.ERR_SEND_MSG);
                            }

                            // 通信ストリームの取得
                            System.Net.Sockets.NetworkStream stream = client.GetStream();

                            // サーバーへ送信
                            byte[] SendBuffer = System.Text.Encoding.Unicode.GetBytes("検収年月 " + this.LBL_Kensyuyy.Text + "年" + this.LBL_Kensyumm.Text + "月");
                            stream.Write(SendBuffer, 0, SendBuffer.Length);
                            stream.Flush(); // フラッシュ(強制書き出し)

                            // TCPｸﾗｲｱﾝﾄをｸﾛｰｽﾞ
                            client.Close();
                        }

                    }
                    catch (Exception ex)
                    {
                        // 接続できなかった場合
                        MessageBox.Show(ex.Message, Properties.Resources.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    finally
                    {
                        IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                        LBL_MSG.BackColor = System.Drawing.ColorTranslator.FromOle(Information.QBColor(9));
                        LBL_MSG.ForeColor = System.Drawing.ColorTranslator.FromOle(Information.QBColor(15));
                        LBL_MSG.Text = "処理は終了しました。閉じるボタンを押して下さい。";
                        CMD_Button[0].Enabled = true;
                    }
                    //2013.0812 DSK yoshida end

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
        }

        private void FRM_CKSI0060M_Load(System.Object eventSender, System.EventArgs eventArgs)
        {
            string L_Search = null;
            string L_Command2 = null;

            //プロジェクト名を設定
            C_COMMON.G_COMMON_MSGTITLE = "CKSI0060";

            //コントロール配列にイベントを設定
            setEventCtrlAry();

            //待機する秒数
            const short Waittime = 2;
            //起動時刻を待避
            System.DateTime now_old = DateTime.Now;

            //現在の時刻－起動時刻が待機する秒数になるまでループする
            while (!(DateTime.FromOADate(DateAndTime.Now.ToOADate() - Convert.ToDateTime(now_old).ToOADate()) >= DateAndTime.TimeSerial(0, 0, Waittime)))
            {
                System.Windows.Forms.Application.DoEvents();
            }

            Show();

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
// 2016.12.12 yoshitake 副資材棚卸システム再構築 start.
                string[] args = IPPAN.G_COMMAND.Split(',');
                CKSI0060.G_UserId = args[0];
                CKSI0060.G_OfficeId = args[1];
                CKSI0060.G_Syokuicd = args[2];

                if (args.Length >= 4)
                {
                    CKSI0060.G_START_SIZAI_TANAOROSI_SYSTEM = args[3];
                }

                //CKSI0060.G_UserId = Strings.Mid(IPPAN.G_COMMAND, 1, Strings.InStr(1, IPPAN.G_COMMAND, L_Search, CompareMethod.Binary) - 1);
                //L_Command2 = Strings.Mid(IPPAN.G_COMMAND, Strings.InStr(1, IPPAN.G_COMMAND, L_Search, CompareMethod.Binary) + 1, 30);
                //CKSI0060.G_OfficeId = Strings.Mid(L_Command2, 1, Strings.InStr(1, L_Command2, L_Search, CompareMethod.Binary) - 1);
                //CKSI0060.G_Syokuicd = Strings.Mid(L_Command2, Strings.InStr(1, L_Command2, L_Search, CompareMethod.Binary) + 1, 2);
// 2016.12.12 yoshitake 副資材棚卸システム再構築 end.
                
                //ユーザーＩＤが副資材ユーザ以外かを見る
                IPPAN.G_RET = FS_USER_CHECK.User_Check(CKSI0060.G_UserId);
                if (IPPAN.G_RET != 0)
                {
                    System.Environment.Exit(0);
                }
                else
                {
                    IPPAN.G_RET = IPPAN2.TIMER_SET(CKSI0060.G_UserId);
                    if (IPPAN.G_RET != 0)
                    {
                        System.Environment.Exit(0);
                    }
                }
            }
            if (CKSI0060.Control_Kensaku() == 1)
            {
                //副資材コントロールマスタが見つかりません
                IPPAN.Error_Msg("E700", 0, " ");
                System.Environment.Exit(0);
            }
            //2013.09.03 DSK yoshida start
            //LBL_Kensyuyy.Text = Strings.Mid(CKSI0060.G_SKYM, 3, 2);
            LBL_Kensyuyy.Text = Strings.Mid(CKSI0060.G_SKYM, 1, 4);
            //2013.09.03 DSK yoshida end
            LBL_Kensyumm.Text = Strings.Mid(CKSI0060.G_SKYM, 5, 2);

// 2016.12.12 yoshitake 副資材棚卸システム再構築 start.
            if (CKSI0060.G_START_SIZAI_TANAOROSI_SYSTEM.Equals("1"))
            {
                this._CMD_Button_0.Enabled = false;
            }
// 2016.12.12 yoshitake 副資材棚卸システム再構築 end.

        }

        private void TMR_TIMER_Tick(System.Object eventSender, System.EventArgs eventArgs)
        {
            IPPAN.Control_Init(this, "購買検収データ作成", "CKSI0060M", SO.SO_USERNAME, SO.SO_OFFICENAME, "2.00");
        }

        private short Control_Kosin(C_ODBC db)
        {
            short functionReturnValue = 1;
            string L_datetime = null;

            //コントロール年月のシフト
            CKSI0060.G_CONTROL_AREA[2] = CKSI0060.G_SKYM;
            if (Strings.Mid(CKSI0060.G_SKYM, 5, 2) == "12")
            {
                CKSI0060.G_CONTROL_AREA[1] = String.Format("{0:0000}", Convert.ToInt32(Strings.Mid(CKSI0060.G_SKYM, 1, 4)) + 1) + "01";
            }
            else
            {
                CKSI0060.G_CONTROL_AREA[1] = Strings.Mid(CKSI0060.G_SKYM, 1, 4) + String.Format("{0:00}", Convert.ToInt32(Strings.Mid(CKSI0060.G_SKYM, 5, 2)) + 1);
            }

            L_datetime = String.Format("{0:yyyy/MM/dd HH:mm:ss}", DateTime.Now);
            IPPAN.G_SQL = "update SIZAI_CONTROL_MST set ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SKYM = '" + CKSI0060.G_CONTROL_AREA[1] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",KBYM = '" + CKSI0060.G_CONTROL_AREA[2] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",SIWAKEFLG = ' ' ";
            IPPAN.G_SQL = IPPAN.G_SQL + ",UPDYMD = to_date('" + L_datetime + "' , 'YYYY/MM/DD HH24:MI:SS')";
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
// 2016.12.12 yoshitake 副資材棚卸システム再構築 start.
        /// <summary>
        /// 資材棚卸累積ワーク更新
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        private short Sizai_Tanaorosi_Ruiseki_Wrk_Insert(C_ODBC db)
        {
            short functionReturnValue = 1;

            //資材棚卸累積ワーク更新
            IPPAN.G_SQL = String.Empty;
            IPPAN.G_SQL = IPPAN.G_SQL + "INSERT INTO ";
            IPPAN.G_SQL = IPPAN.G_SQL + "  SIZAI_TANAOROSI_RUISEKI_WRK(";
            IPPAN.G_SQL = IPPAN.G_SQL + "                              TANAYM";
            IPPAN.G_SQL = IPPAN.G_SQL + "                             ,SIZAI_KBN";
            IPPAN.G_SQL = IPPAN.G_SQL + "                             ,WORK_KBN";
            IPPAN.G_SQL = IPPAN.G_SQL + "                             ,ITEM_ORDER";
            IPPAN.G_SQL = IPPAN.G_SQL + "                             ,HINMOKUCD";
            IPPAN.G_SQL = IPPAN.G_SQL + "                             ,GYOSYACD";
            IPPAN.G_SQL = IPPAN.G_SQL + "                             ,HINMOKUNM";
            IPPAN.G_SQL = IPPAN.G_SQL + "                             ,GYOSYANM";
            IPPAN.G_SQL = IPPAN.G_SQL + "                             ,TOGETU";
            IPPAN.G_SQL = IPPAN.G_SQL + "                             ,TOGETU_YOSOU";
            IPPAN.G_SQL = IPPAN.G_SQL + "                             ,NYUKO";
            IPPAN.G_SQL = IPPAN.G_SQL + "                             ,HARAI";
            IPPAN.G_SQL = IPPAN.G_SQL + "                             ,HENPIN";
            IPPAN.G_SQL = IPPAN.G_SQL + "                             ,ZSOUKO";
            IPPAN.G_SQL = IPPAN.G_SQL + "                             ,ZEF";
            IPPAN.G_SQL = IPPAN.G_SQL + "                             ,ZLF";
            IPPAN.G_SQL = IPPAN.G_SQL + "                             ,ZCC";
            IPPAN.G_SQL = IPPAN.G_SQL + "                             ,ZSONOTA";
            IPPAN.G_SQL = IPPAN.G_SQL + "                             ,ZMETER";
            IPPAN.G_SQL = IPPAN.G_SQL + "                             ,ZYOBI1";
            IPPAN.G_SQL = IPPAN.G_SQL + "                             ,ZYOBI2";
            IPPAN.G_SQL = IPPAN.G_SQL + "                             ,INSUSER";
            IPPAN.G_SQL = IPPAN.G_SQL + "                             ,INSYMD";
            IPPAN.G_SQL = IPPAN.G_SQL + "                             ,UPDUSER";
            IPPAN.G_SQL = IPPAN.G_SQL + "                             ,UPDYMD";
            IPPAN.G_SQL = IPPAN.G_SQL + "                             )";
            IPPAN.G_SQL = IPPAN.G_SQL + "                              SELECT";
            IPPAN.G_SQL = IPPAN.G_SQL + "                               (SELECT TANAYM ";
            IPPAN.G_SQL = IPPAN.G_SQL + "                                  FROM TANAOROSI_KAIKEI_KIKAN_MST";
            IPPAN.G_SQL = IPPAN.G_SQL + "                                 WHERE SYSTEM_CATEGORY = '1')";
            IPPAN.G_SQL = IPPAN.G_SQL + "                             ,SIZAI_KBN";
            IPPAN.G_SQL = IPPAN.G_SQL + "                             ,WORK_KBN";
            IPPAN.G_SQL = IPPAN.G_SQL + "                             ,ITEM_ORDER";
            IPPAN.G_SQL = IPPAN.G_SQL + "                             ,HINMOKUCD";
            IPPAN.G_SQL = IPPAN.G_SQL + "                             ,GYOSYACD";
            IPPAN.G_SQL = IPPAN.G_SQL + "                             ,HINMOKUNM";
            IPPAN.G_SQL = IPPAN.G_SQL + "                             ,GYOSYANM";
            IPPAN.G_SQL = IPPAN.G_SQL + "                             ,TOGETU";
            IPPAN.G_SQL = IPPAN.G_SQL + "                             ,TOGETU_YOSOU";
            IPPAN.G_SQL = IPPAN.G_SQL + "                             ,NYUKO";
            IPPAN.G_SQL = IPPAN.G_SQL + "                             ,HARAI";
            IPPAN.G_SQL = IPPAN.G_SQL + "                             ,HENPIN";
            IPPAN.G_SQL = IPPAN.G_SQL + "                             ,ZSOUKO";
            IPPAN.G_SQL = IPPAN.G_SQL + "                             ,ZEF";
            IPPAN.G_SQL = IPPAN.G_SQL + "                             ,ZLF";
            IPPAN.G_SQL = IPPAN.G_SQL + "                             ,ZCC";
            IPPAN.G_SQL = IPPAN.G_SQL + "                             ,ZSONOTA";
            IPPAN.G_SQL = IPPAN.G_SQL + "                             ,ZMETER";
            IPPAN.G_SQL = IPPAN.G_SQL + "                             ,ZYOBI1";
            IPPAN.G_SQL = IPPAN.G_SQL + "                             ,ZYOBI2";
            IPPAN.G_SQL = IPPAN.G_SQL + "                             ,INSUSER";
            IPPAN.G_SQL = IPPAN.G_SQL + "                             ,INSYMD";
            IPPAN.G_SQL = IPPAN.G_SQL + "                             ,UPDUSER";
            IPPAN.G_SQL = IPPAN.G_SQL + "                             ,UPDYMD";
            IPPAN.G_SQL = IPPAN.G_SQL + "                         FROM SIZAI_TANAOROSI_WRK ";
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
            catch (Exception)
            {
                db.Error();
            }

            functionReturnValue = 0;
            return functionReturnValue;
        }
        /// <summary>
        /// 資材棚卸累積ワーク削除
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        private short Sizai_Tanaorosi_Ruiseki_Wrk_DELETE(C_ODBC db)
        {
            short functionReturnValue = 1;

            //資材棚卸累積ワーク削除（２年前）
            IPPAN.G_SQL = String.Empty;
            IPPAN.G_SQL = IPPAN.G_SQL + "DELETE FROM SIZAI_TANAOROSI_RUISEKI_WRK";
            IPPAN.G_SQL = IPPAN.G_SQL + "      WHERE TANAYM <= (";
            IPPAN.G_SQL = IPPAN.G_SQL + "                       SELECT TANAYM - 200 ";
            IPPAN.G_SQL = IPPAN.G_SQL + "                         FROM TANAOROSI_KAIKEI_KIKAN_MST ";
            IPPAN.G_SQL = IPPAN.G_SQL + "                        WHERE SYSTEM_CATEGORY = '1') ";
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
            catch (Exception)
            {
                db.Error();
            }

            functionReturnValue = 0;
            return functionReturnValue;
        }
        /// <summary>
        /// 資材棚卸ワーク削除
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        private short Sizai_Tanaorosi_Wrk_DELETE(C_ODBC db)
        {
            short functionReturnValue = 1;

            IPPAN.G_SQL = " DELETE FROM SIZAI_TANAOROSI_WRK";
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
            catch (Exception)
            {
                db.Error();
            }

            functionReturnValue = 0;
            return functionReturnValue;
        }

        /// <summary>
        /// 資材棚卸立会い用ワーク削除
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        private short Sizai_Tanaorosi_Tatiai_Wrk_DELETE(C_ODBC db)
        {
            short functionReturnValue = 1;

            IPPAN.G_SQL = " DELETE FROM SIZAI_TANAOROSI_TATIAI_WRK";
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
            catch (Exception)
            {
                db.Error();
            }

            functionReturnValue = 0;
            return functionReturnValue;
        }
        /// <summary>
        /// 棚卸ログトランの不要なデータを削除
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        private short DeleteUnnesessaryData_Tanaorosi_Log_Trn(C_ODBC db)
        {
            short functionReturnValue = 1;

            //棚卸ログトラン削除（半年前）
            IPPAN.G_SQL = String.Empty;
            IPPAN.G_SQL = IPPAN.G_SQL + "DELETE FROM TANAOROSI_LOG_TRN";
            IPPAN.G_SQL = IPPAN.G_SQL + "      WHERE SUBSTR(OPERATION_YMD,1,6) <= (";
            IPPAN.G_SQL = IPPAN.G_SQL + "                       SELECT to_char(add_months(SYSDATE, -6),'yyyyMMdd') FROM DUAL) ";
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
            catch (Exception)
            {
                db.Error();
            }

            functionReturnValue = 0;
            return functionReturnValue;
        }
        /// <summary>
        /// 棚卸詳細ログトランの不要なデータを削除
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        private short DeleteUnnesessaryData_Tanaorosi_Detail_Log_Trn(C_ODBC db)
        {
            short functionReturnValue = 1;

            //棚卸詳細ログトラン削除（半年前）
            IPPAN.G_SQL = String.Empty;
            IPPAN.G_SQL = IPPAN.G_SQL + "DELETE FROM TANAOROSI_DETAIL_LOG_TRN";
            IPPAN.G_SQL = IPPAN.G_SQL + "      WHERE SUBSTR(OPERATION_YMD,1,6) <= (";
            IPPAN.G_SQL = IPPAN.G_SQL + "                       SELECT to_char(add_months(SYSDATE, -6),'yyyyMMdd') FROM DUAL) ";
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
            catch (Exception)
            {
                db.Error();
            }

            functionReturnValue = 0;
            return functionReturnValue;
        }
        /// <summary>
        /// 棚卸進捗状況マスタ更新
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        private short Tanaorosi_Progress_MST_UPDATE(C_ODBC db)
        {
            short functionReturnValue = 1;
            string L_datetime = null;

            IPPAN.G_SQL = String.Empty;
            IPPAN.G_SQL = IPPAN.G_SQL + "UPDATE TANAOROSI_PROGRESS_MST SET ";
            IPPAN.G_SQL = IPPAN.G_SQL + "       OPERATION_NO = '1'";
            IPPAN.G_SQL = IPPAN.G_SQL + "      ,OPERATION_CONDITION = '0'";
            IPPAN.G_SQL = IPPAN.G_SQL + "      ,UPDUSER = '" + CKSI0060.G_UserId + "' ";
            IPPAN.G_SQL = IPPAN.G_SQL + "      ,UPDYMD = SYSDATE";
// 2018.07.01 yoshitake 部品倉庫棚卸システム再構築 start.
            IPPAN.G_SQL = IPPAN.G_SQL + "  WHERE SYSTEM_CATEGORY = '1'";
// 2018.07.01 yoshitake 部品倉庫棚卸システム再構築 end.
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
            catch (Exception)
            {
                db.Error();
            }

            functionReturnValue = 0;
            return functionReturnValue;
        }

        /// <summary>
        /// 棚卸会計期間マスタ更新
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static short Tanaorosi_Kaikei_Kikan_MST_UPDATE(C_ODBC db)
        {
            short functionReturnValue = 1;
            DataTable tbl = null;
            string tanaym = String.Empty;
            int kaikei_nendo = 0;
            string L_datetime = null;

            //棚卸会計期間マスタ検索
            IPPAN.G_SQL = String.Empty;
            IPPAN.G_SQL = IPPAN.G_SQL + "SELECT ";
            IPPAN.G_SQL = IPPAN.G_SQL + "       TANAYM";
            IPPAN.G_SQL = IPPAN.G_SQL + "      ,KAIKEI_NENDO";
            IPPAN.G_SQL = IPPAN.G_SQL + "  FROM TANAOROSI_KAIKEI_KIKAN_MST";
            IPPAN.G_SQL = IPPAN.G_SQL + " WHERE SYSTEM_CATEGORY = '1'";
            Debug.Print(IPPAN.G_SQL);

            try
            {
                tbl = db.ExecSQL(IPPAN.G_SQL);
            }
            catch (OdbcException)
            {
                db.Error();
            }
            catch (Exception)
            {
                db.Error();
            }

            // 結果の列数を取得する
            if (tbl.Columns.Count < 1)
            {
                return functionReturnValue;
            }

            //データがない
            if (tbl.Rows.Count != 1)
            {
                return functionReturnValue;
            }

            //項目セット
            tanaym = tbl.Rows[0][0].ToString();
            kaikei_nendo = Convert.ToInt32(tbl.Rows[0][1]);

            //棚卸年月のシフト
            if (Strings.Mid(tanaym, 5, 2) == "12")
            {
                tanaym = String.Format("{0:0000}", Convert.ToInt32(Strings.Mid(tanaym, 1, 4)) + 1) + "01";
            }
            else
            {
                tanaym = Strings.Mid(tanaym, 1, 4) + String.Format("{0:00}", Convert.ToInt32(Strings.Mid(tanaym, 5, 2)) + 1);
            }

            //棚卸会計期間マスタ更新
            IPPAN.G_SQL = String.Empty;
            IPPAN.G_SQL = IPPAN.G_SQL + " UPDATE TANAOROSI_KAIKEI_KIKAN_MST SET";
            IPPAN.G_SQL = IPPAN.G_SQL + " TANAYM = '" + tanaym + "'";
            if (Strings.Mid(tanaym, 5, 2) == "04")
            {
                IPPAN.G_SQL = IPPAN.G_SQL + " ,KAIKEI_NENDO = " + ++kaikei_nendo;
            }
            if (Strings.Mid(tanaym, 5, 2) == "04")
            {
                IPPAN.G_SQL = IPPAN.G_SQL + " ,KAIKEI_KIKAN = '0'";
            }
            else if (Strings.Mid(tanaym, 5, 2) == "09")
            {
                IPPAN.G_SQL = IPPAN.G_SQL + " ,KAIKEI_KIKAN = '1'";
            }
            IPPAN.G_SQL = IPPAN.G_SQL + ",UPDYMD = SYSDATE";
// 2018.07.01 yoshitake 部品倉庫棚卸システム再構築 start.
            IPPAN.G_SQL = IPPAN.G_SQL + "  WHERE SYSTEM_CATEGORY = '1'";
// 2018.07.01 yoshitake 部品倉庫棚卸システム再構築 end.
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
            catch (Exception)
            {
                db.Error();
            }

            functionReturnValue = 0;
            return functionReturnValue;
        }
// 2016.12.12 yoshitake 副資材棚卸システム再構築 end.

    }

    class Soushin
    {
        // TCPｸﾗｲｱﾝﾄを生成
        System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient();   // TCPｸﾗｲｱﾝﾄ
    }
}

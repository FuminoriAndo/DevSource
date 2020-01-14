using Microsoft.VisualBasic;
using System;
using System.Windows.Forms;
using System.Data.Odbc;
using Excel = Microsoft.Office.Interop.Excel;
using System.Xml;
using System.Collections.Generic;
using System.Diagnostics;
using Common;

namespace Project1
{
	internal partial class FRM_CKSI0010M : System.Windows.Forms.Form
	{
		//**************************************************************************
		//
		//   プログラム名　　　資材班作業誌入力（Ｃｋｉｓ００１０）
		//
		//   更新履歴
		//   更新日付    Rev     修正者      内容
		//   03.01.15           NTIS浅田　　Ｓ／Ｏサーバ更新の為、VB4.0→VB6.0SP5へのバージョンアップ対応
		//  13.07.02            ISV-TRUC    作業日（年）の入力内容が、２桁の数値文字列だった場合は、先頭に "20"を付加する。
		//  13.07.02            ISV-TRUC    品名ＣＤのLeaveイベントで、区分の入力値が、9（削除）以外、向先のEnabledへTrueをセット。
		//**************************************************************************
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new FRM_CKSI0010M());
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

		//区分変更用ワーク
		string WK_Kbn;
		//品目コード変更用ワーク
		string WK_HinmokuCd;
		//業者コード変更用ワーク
		string WK_GyosyaCd;
		//向先変更用ワーク
		string WK_Mukesaki;
		//数量変更用ワーク
		string WK_Suryo;
		//水分引変更用ワーク
		string WK_Suibun;
		string G_UserId;
		string G_OfficeId;
		string G_SyokuiCd;
// 2016.12.12 yoshitake 副資材棚卸システム再構築 start.
        string G_EKISAN_PARAM = string.Empty;
        string G_KENSIN_DATA_FILE_PATH = string.Empty;
        string G_TANAOROSI_YEAR_MONTH = string.Empty;
// 2016.12.12 yoshitake 副資材棚卸システム再構築 end.
		//既にデータがあるときは”０”、ないときは”１”
		string G_flg;
		//入力された項目があるときは”０”、ないときは”１”
		string G_flg1;
		string G_flg2;

// 2016.12.12 yoshitake 副資材棚卸システム再構築 start.
// 2017.05.09 yoshitake 副資材棚卸システム不具合修正 start.

		// private readonly int SIZAI_IN_OUT_LIST_SHEET = 10;

        /// <summary>
        /// 副資材入出庫表のシート名の定義
        /// </summary>
        private string SIZAI_IN_OUT_LIST_SHEET_NAME = null;
// 2017.05.09 yoshitake 副資材棚卸システム不具合修正 end.

        /// <summary>
        /// 副資材入出庫表のカテゴリAの区分
        /// </summary>
        private readonly string KBN_CATEGORY_A = "1";

        /// <summary>
        /// 副資材入出庫表のカテゴリBの区分
        /// </summary>
        private readonly string KBN_CATEGORY_B = "2";

        /// <summary>
        /// 副資材入出庫表のカテゴリCの区分
        /// </summary>
        private readonly string KBN_CATEGORY_C = "3";

        /// <summary>
        /// 副資材入出庫表のデータ範囲の開始行
        /// </summary>
        private readonly int EXCEL_START_ROW = 8;

        /// <summary>
        /// 副資材入出庫表のデータ範囲の開始列
        /// </summary>
        private readonly int EXCEL_START_COL = 3;

        /// <summary>
        /// 副資材入出庫表のデータ範囲の終了列
        /// </summary>
        private readonly int EXCEL_END_COL = 11;

        /// <summary>
        /// 副資材入出庫表のカテゴリ
        /// </summary>
        private enum SIZAI_IN_OUT_CATEGORY : int
        {
            IN = 1,
            OUT = 2
        }

        /// <summary>
        /// 副資材入出庫表のデータ範囲の列のインデックス
        /// </summary>
        private enum EXCEL_COL_DEFINE : int
        {
            KBN = 2,
            HINMOKU_CODE = 3,
            HINMOKU_NAME = 4,
            TANI = 5,
            GYOSYA_CODE = 6,
            GYOSYA_NAME = 7,
            MUKESAKI = 8,
            SURYO = 9,
            SUIBUNHIKI = 10
        }

        /// <summary>
        /// 副資材入出庫表のファイルパス
        /// </summary>
        private string sizaiInOutFilePath = null;

        /// <summary>
        /// 検針データ(EXCEL)から読込んだデータ
        /// </summary>
        private IList<SizaiInOutItem> kensinExcelData = null;

        /// <summary>
        /// その他の資材区分で入力してはいけない品目の一覧
        /// </summary>
        private IList<string> othersNotInputHinmokuCode = null;
// 2016.12.12 yoshitake 副資材棚卸システム再構築 end.
// 2017.04.28 yoshitake 副資材棚卸システム再構築 追加対応 start.
        private bool IsShowExistSizaiTanaorosiMstMsg = false;
// 2017.04.28 yoshitake 副資材棚卸システム再構築 追加対応 end.

		public void Gamen_Set1()
		{
			short i = 0;

			for (i = 0; i <= 9; i += 1)
			{
				
// 2017.04.28 yoshitake 副資材棚卸システム再構築 追加対応 start.
                string approval = Strings.Trim(Cksi0010.G_SAGYOSI_AREA[(Convert.ToInt32(Cksi0010.G_Page) * 10) - 10  + i, 10]);
                if (approval == "1")
                {
                    // 区分
                    TXT_kbn[i].Enabled = false;
                    // 品目コード
                    TXT_HinCd[i].Enabled = false;
                    // 業者コード
                    TXT_GyoCd[i].Enabled = false;
                    // 向先
                    TXT_Mukesaki[i].Enabled = false;
                    // 数量
                    TXT_Suryo[i].Enabled = false;
                    // 水分引
                    TXT_Suibun[i].Enabled = false;
                }
                else
                {
                    // 区分
                    TXT_kbn[i].Enabled = true;
                    // 品目コード
                    TXT_HinCd[i].Enabled = true;
                    // 業者コード
                    TXT_GyoCd[i].Enabled = true;
                    // 向先
                    TXT_Mukesaki[i].Enabled = true;
                    // 数量
                    TXT_Suryo[i].Enabled = true;
                    // 水分引
                    TXT_Suibun[i].Enabled = true;
                }
// 2017.04.28 yoshitake 副資材棚卸システム再構築 追加対応 end.
                
                // 区分
				TXT_kbn[i].Text = Cksi0010.G_SAGYOSI_AREA[(Convert.ToInt32(Cksi0010.G_Page) * 10) - 10 + TXT_kbn.GetIndex(TXT_kbn[i]), 2];
				switch (Strings.Trim(TXT_kbn[i].Text))
				{
					//Start 13.07.02    ISV-TRUC 
					case "1":
					case "4":
					case "9":
						TXT_Mukesaki[i].Enabled = false;
						TXT_Mukesaki[i].Text = "";
						break;
					//End 13.07.02    ISV-TRUC 
					case "2":
						TXT_GyoCd[i].Enabled = false;
						TXT_GyoCd[i].Text = "";
						LBL_GyoNm[i].Text = "";
						break;
				}

				// 品目コード
				TXT_HinCd[i].Text = Cksi0010.G_SAGYOSI_AREA[(Convert.ToInt32(Cksi0010.G_Page) * 10) - 10 + TXT_HinCd.GetIndex(TXT_HinCd[i]), 3];
				//副資材品目マスタの検索
				if (Cksi0010.Hinmoku_Kensaku(TXT_HinCd[i].Text) == 0)
				{
					// 品目名
					LBL_HinNm[i].Text = Strings.Left(Cksi0010.G_HINMOKU_AREA[0], 15);
					if (Strings.Trim(Cksi0010.G_HINMOKU_AREA[1]) != "1")
					{
						TXT_Suibun[i].Enabled = false;
						TXT_Suibun[i].Text = "";
					}
					else
					{
						// 水分引
						TXT_Suibun[i].Text = IPPAN.Money_Hensyu(Strings.Trim(Cksi0010.G_SAGYOSI_AREA[(Convert.ToInt32(Cksi0010.G_Page) * 10) - 10 + TXT_Suibun.GetIndex(TXT_Suibun[i]), 9]), "#0.00");
						Cksi0010.G_Suibun[i] = IPPAN2.Numeric_Hensyu4(Strings.Trim(TXT_Suibun[Convert.ToInt16(Strings.Right(Convert.ToString(i), 1))].Text));
					}
				}
				else
				{
					LBL_HinNm[i].Text = "";
					TXT_Suibun[i].Enabled = false;
					TXT_Suibun[i].Text = "";
				}

				// 業者コード
				TXT_GyoCd[i].Text = Cksi0010.G_SAGYOSI_AREA[(Convert.ToInt32(Cksi0010.G_Page) * 10) - 10 + TXT_GyoCd.GetIndex(TXT_GyoCd[i]), 4];
				//業者マスタの検索
				if (Cksi0010.Gyosya_Kensaku(TXT_GyoCd[i].Text) == 0)
				{
					// 業者名
					LBL_GyoNm[i].Text = Strings.Left(Cksi0010.G_GYOSYA_AREA[0], 15);
				}
				else
				{
					LBL_GyoNm[i].Text = "";
				}

				// 向先
				TXT_Mukesaki[i].Text = Cksi0010.G_SAGYOSI_AREA[(Convert.ToInt32(Cksi0010.G_Page) * 10) - 10 + TXT_Mukesaki.GetIndex(TXT_Mukesaki[i]), 5];
				// 数量
				TXT_Suryo[i].Text = IPPAN.Money_Hensyu(Strings.Trim(Cksi0010.G_SAGYOSI_AREA[(Convert.ToInt32(Cksi0010.G_Page) * 10) - 10 + TXT_Suryo.GetIndex(TXT_Suryo[i]), 8]), "###,###,##0");
				Cksi0010.G_Suryo[i] = IPPAN2.Numeric_Hensyu3(Strings.Trim(TXT_Suryo[Convert.ToInt16(Strings.Right(Convert.ToString(i), 1))].Text));
			}
		}

		public short Gamen_Check()
		{
			short functionReturnValue = 0;
			short i = 0;
			functionReturnValue = 1;
			G_flg1 = "1";
			for (i = 0; i <= 9; i += 1)
			{
				if (IPPAN.C_Allspace(TXT_kbn[i].Text) != 0 || IPPAN.C_Allspace(TXT_HinCd[i].Text) != 0 || IPPAN.C_Allspace(TXT_GyoCd[i].Text) != 0 || IPPAN.C_Allspace(TXT_Mukesaki[i].Text) != 0 || IPPAN.C_Allspace(TXT_Suryo[i].Text) != 0 || IPPAN.C_Allspace(TXT_Suibun[i].Text) != 0)
				{
					// 区分
					if (IPPAN.C_Allspace(TXT_kbn[i].Text) != 0)
					{
						if (IPPAN.Input_Check(TXT_kbn[i].Text) != 0)
						{
							//フォーカスセット
							TXT_kbn[i].Focus();
							TXT_kbn_Enter(TXT_kbn[i], new System.EventArgs());
							//入力エラー
							IPPAN.Error_Msg("E202", 0, " ");
							return functionReturnValue;
						}
						IPPAN.G_RET = IPPAN.Numeric_Check2(TXT_kbn[i].Text);
						if (IPPAN.G_RET == 1)
						{
							TXT_kbn[i].Focus();
							TXT_kbn_Enter(TXT_kbn[i], new System.EventArgs());
							//入力エラー
							IPPAN.Error_Msg("E202", 0, " ");
							IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
							return functionReturnValue;
						}
						G_flg1 = "0";
					}
					else
					{
						//フォーカスセット
						TXT_kbn[i].Focus();
						TXT_kbn_Enter(TXT_kbn[i], new System.EventArgs());
						//必須項目入力エラー
						IPPAN.Error_Msg("E200", 0, "");
						return functionReturnValue;
					}
					// 品目コード
					if (TXT_kbn[i].Text == "1" || TXT_kbn[i].Text == "2" || TXT_kbn[i].Text == "3" || TXT_kbn[i].Text == "4")
					{
						if (IPPAN.C_Allspace(TXT_HinCd[i].Text) != 0)
						{
							if (IPPAN.Input_Check(TXT_HinCd[i].Text) != 0)
							{
								//フォーカスセット
								TXT_HinCd[i].Focus();
								TXT_HinCd_Enter(TXT_HinCd[i], new System.EventArgs());
								//入力エラー
								IPPAN.Error_Msg("E202", 0, " ");
								return functionReturnValue;
							}
							G_flg1 = "0";
						}
						else
						{
							//フォーカスセット
							TXT_HinCd[i].Focus();
							TXT_HinCd_Enter(TXT_HinCd[i], new System.EventArgs());
							//必須項目入力エラー
							IPPAN.Error_Msg("E200", 0, "");
							return functionReturnValue;
						}
					}
					// 業者コード
					if (TXT_kbn[i].Text == "1" || TXT_kbn[i].Text == "3" || TXT_kbn[i].Text == "4")
					{
						if (IPPAN.C_Allspace(TXT_GyoCd[i].Text) != 0)
						{
							if (IPPAN.Input_Check(TXT_GyoCd[i].Text) != 0)
							{
								//フォーカスセット
								TXT_GyoCd[i].Focus();
								TXT_GyoCd_Enter(TXT_GyoCd[i], new System.EventArgs());
								//入力エラー
								IPPAN.Error_Msg("E202", 0, " ");
								return functionReturnValue;
							}
							IPPAN.G_RET = IPPAN.Numeric_Check2(TXT_GyoCd[i].Text);
							if (IPPAN.G_RET == 1)
							{
								TXT_GyoCd[i].Focus();
								TXT_GyoCd_Enter(TXT_GyoCd[i], new System.EventArgs());
								//入力エラー
								IPPAN.Error_Msg("E202", 0, " ");
								IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
								return functionReturnValue;
							}
							G_flg1 = "0";
						}
						else
						{
							//フォーカスセット
							TXT_GyoCd[i].Focus();
							TXT_GyoCd_Enter(TXT_GyoCd[i], new System.EventArgs());
							//必須項目入力エラー
							IPPAN.Error_Msg("E200", 0, "");
							return functionReturnValue;
						}
					}
					// 向先
					if (TXT_kbn[i].Text == "2" || TXT_kbn[i].Text == "3")
					{
						if (IPPAN.C_Allspace(TXT_Mukesaki[i].Text) != 0)
						{
							if (IPPAN.Input_Check(TXT_Mukesaki[i].Text) != 0)
							{
								//フォーカスセット
								TXT_Mukesaki[i].Focus();
								TXT_Mukesaki_Enter(TXT_Mukesaki[i], new System.EventArgs());
								//入力エラー
								IPPAN.Error_Msg("E202", 0, " ");
								return functionReturnValue;
							}
							IPPAN.G_RET = IPPAN.Numeric_Check2(TXT_Mukesaki[i].Text);
							if (IPPAN.G_RET == 1)
							{
								TXT_Mukesaki[i].Focus();
								TXT_Mukesaki_Enter(TXT_Mukesaki[i], new System.EventArgs());
								//入力エラー
								IPPAN.Error_Msg("E202", 0, " ");
								IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
								return functionReturnValue;
							}
							G_flg1 = "0";
						}
						else
						{
							//フォーカスセット
							TXT_Mukesaki[i].Focus();
							TXT_Mukesaki_Enter(TXT_Mukesaki[i], new System.EventArgs());
							//必須項目入力エラー
							IPPAN.Error_Msg("E200", 0, "");
							return functionReturnValue;
						}
					}
// 2017.1.24 yoshitake 副資材棚卸システム再構築 start.
                    // 向先がその他で入力してはいけない品目CDが設定されていないか
                    if (((TXT_kbn[i].Text == "2") && (TXT_Mukesaki[i].Text == "4")) || ((TXT_kbn[i].Text == "3") && (TXT_Mukesaki[i].Text == "4")))
                    {
                        foreach(string hinmokuCode in othersNotInputHinmokuCode)
                        {
                            if (hinmokuCode.Equals(TXT_HinCd[i].Text))
                            {
                                //フォーカスセット
                                TXT_HinCd[i].Focus();
                                TXT_HinCd_Enter(TXT_HinCd[i], new System.EventArgs());
                                //入力エラー
                                IPPAN.Error_Msg("E673", 0, " ");
                                return functionReturnValue;
                            }
                        }
                    }
                    // 向先
                    if (TXT_Mukesaki[i].Text == "5" || TXT_Mukesaki[i].Text == "6" || TXT_Mukesaki[i].Text == "8")
                    {
                        //フォーカスセット
						TXT_Mukesaki[i].Focus();
						TXT_Mukesaki_Enter(TXT_Mukesaki[i], new System.EventArgs());
						//入力エラー
						IPPAN.Error_Msg("E672", 0, " ");
						return functionReturnValue;
                    }
// 2017.1.24 yoshitake 副資材棚卸システム再構築 end.
					// 数量
					if (TXT_kbn[i].Text == "1" || TXT_kbn[i].Text == "2" || TXT_kbn[i].Text == "3" || TXT_kbn[i].Text == "4")
					{
						if (IPPAN.C_Allspace(TXT_Suryo[i].Text) != 0)
						{
							if (IPPAN.Numeric_Check(TXT_Suryo[i].Text) == 1)
							{
								TXT_Suryo[i].Focus();
								TXT_Suryo_Enter(TXT_Suryo[i], new System.EventArgs());
								//入力エラー
								IPPAN.Error_Msg("E202", 0, " ");
								return functionReturnValue;
							}
							G_flg1 = "0";
						}
						else
						{
							//フォーカスセット
							TXT_Suryo[i].Focus();
							TXT_Suryo_Enter(TXT_Suryo[i], new System.EventArgs());
							//必須項目入力エラー
							IPPAN.Error_Msg("E200", 0, "");
							return functionReturnValue;
						}
					}
					// 水分引
					if (IPPAN.C_Allspace(TXT_Suibun[i].Text) != 0)
					{
						if (IPPAN.Numeric_Check(TXT_Suibun[i].Text) == 1)
						{
							TXT_Suibun[i].Focus();
							TXT_Suibun_Enter(TXT_Suibun[i], new System.EventArgs());
							//入力エラー
							IPPAN.Error_Msg("E202", 0, " ");
							return functionReturnValue;
						}
						G_flg1 = "0";
					}
// 2017.04.28 yoshitake 副資材棚卸システム再構築 追加対応 start.
                    if (G_EKISAN_PARAM == string.Empty)
                    {
                        if (TXT_kbn[i].Enabled)
                        {
                            if ((TXT_kbn[i].Text == "2") || (TXT_kbn[i].Text == "3"))
                            {
                                // 向先の値を資材棚卸しの区分値に変換する
                                IList<string> sizaiCategotyValue = convertToSizaiCategoryValue(TXT_Mukesaki[i].Text);
                                // 指定した品目コード、資材区分の品目データが資材棚卸マスタに存在するかチェックする
                                bool checkRet = existSizaiTanaorosiMst(TXT_HinCd[i].Text, sizaiCategotyValue);

                                if (!checkRet)
                                {
                                    //フォーカスセット
                                    //TXT_Mukesaki[i].Focus();
                                    //TXT_Mukesaki_Enter(TXT_Mukesaki[i], new System.EventArgs());
                                    // 確認を促すメッセージを表示する
                                    DialogResult result
                                        = MessageBox.Show("資材棚卸調査表に登録している向先以外の" + Environment.NewLine +
                                               "向先が設定されています。" + Environment.NewLine +
                                               "品目CD：" + TXT_HinCd[i].Text + Environment.NewLine +
                                               "向先：" + TXT_Mukesaki[i].Text + Environment.NewLine +
                                               "よろしいですか？"
                                               , "CKSI0010", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                                    if (result == DialogResult.No)
                                    {
                                        return functionReturnValue;
                                    }
                                    else
                                    {
                                        TXT_kbn[i].Focus();
                                        IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
                                        IsShowExistSizaiTanaorosiMstMsg = true;
                                    }
                                }
                            }
                        }
                    }
// 2017.04.28 yoshitake 副資材棚卸システム再構築 追加対応 end.

				}
			}
			functionReturnValue = 0;
			return functionReturnValue;
		}

// 2017.04.28 yoshitake 副資材棚卸システム再構築 追加対応 start.
        /// <summary>
        /// 向先の値を資材棚卸しの区分値に変換する
        /// </summary>
        /// <param name="mukesakiValue">向先の値</param>
        /// <returns>資材棚卸しの区分値</returns>
        private IList<string> convertToSizaiCategoryValue(string mukesakiValue)
        {
            IList<string> ret = new List<string>();

            if (!string.IsNullOrEmpty(mukesakiValue))
            {
                switch (mukesakiValue)
                {
                    // EF
                    case SizaiConstants.Mukesaki_EF_StringDefine:
                        ret.Add(String.Format("{0, -2}", SizaiConstants.SizaiCategory.EF.GetStringValue()));
                        break;

                    // LF、築炉、LD
                    case SizaiConstants.Mukesaki_LF_StringDefine:
                        ret.Add(String.Format("{0, -2}", SizaiConstants.SizaiCategory.LF.GetStringValue()));
                        ret.Add(String.Format("{0, -2}", SizaiConstants.SizaiCategory.Chikuro.GetStringValue()));
                        ret.Add(String.Format("{0, -2}", SizaiConstants.SizaiCategory.LD.GetStringValue()));
                        break;

                    // CC
                    case SizaiConstants.Mukesaki_CC_StringDefine:
                        ret.Add(String.Format("{0, -2}", SizaiConstants.SizaiCategory.CC.GetStringValue()));
                        break;

                    // TD
                    case SizaiConstants.Mukesaki_TD_StringDefine:
                        ret.Add(String.Format("{0, -2}", SizaiConstants.SizaiCategory.TD.GetStringValue()));
                        break;

                    // その他
                    case SizaiConstants.Mukesaki_Others_StringDefine:
                        ret.Add(String.Format("{0, -2}", SizaiConstants.SizaiCategory.Others.GetStringValue()));
                        break;

                    default:
                        break;

                }
            }

            return ret;
        }

        /// <summary>
        /// 指定した品目コード、資材区分の品目データが資材棚卸マスタに存在するかチェックする
        /// </summary>
        /// <param name="hinmokuCode">品目コード</param>
        /// <param name="sizaiCategory">資材区分</param>
        /// <returns>結果</returns>
        private bool existSizaiTanaorosiMst(string hinmokuCode, IList<string> sizaiCategory)
        {
            if(sizaiCategory.Count == 0) 
            {
                return false;
            }

            return Cksi0010.ExistSizaiTanaorosiMst(hinmokuCode, sizaiCategory);
        }
// 2017.04.28 yoshitake 副資材棚卸システム再構築 追加対応 end.

		public void Work_Set()
		{
			int i = 0;
			short shIdx = 0;
			for (i = (Convert.ToInt32(Cksi0010.G_Page) * 10) - 10; i <= (Convert.ToInt32(Cksi0010.G_Page) * 10) - 1; i += 1)
			{
				shIdx = Convert.ToInt16(Strings.Right(Convert.ToString(i), 1));
				// ワーク区分
				Cksi0010.G_SAGYOSI_AREA[i, 2] = TXT_kbn[TXT_kbn.GetIndex(TXT_kbn[shIdx])].Text;
				// ワーク品目コード
				Cksi0010.G_SAGYOSI_AREA[i, 3] = TXT_HinCd[TXT_HinCd.GetIndex(TXT_HinCd[shIdx])].Text;
				if (Cksi0010.Hinmoku_Kensaku(TXT_HinCd[TXT_HinCd.GetIndex(TXT_HinCd[shIdx])].Text) != 0)
				{
					Cksi0010.G_SAGYOSI_AREA[i, 6] = "";
				}
				else
				{
					Cksi0010.G_SAGYOSI_AREA[i, 6] = IPPAN.Space_Set(Strings.Trim(Strings.Mid(Cksi0010.G_HINMOKU_AREA[0], 1, 20)), 20, 2);
				}

				// ワーク業者コード
				Cksi0010.G_SAGYOSI_AREA[i, 4] = TXT_GyoCd[TXT_GyoCd.GetIndex(TXT_GyoCd[shIdx])].Text;
				if (Cksi0010.Gyosya_Kensaku(TXT_GyoCd[TXT_GyoCd.GetIndex(TXT_GyoCd[shIdx])].Text) != 0)
				{
					Cksi0010.G_SAGYOSI_AREA[i, 7] = IPPAN.Space_Set("", 20, 2);
				}
				else
				{
					Cksi0010.G_SAGYOSI_AREA[i, 7] = IPPAN.Space_Set(Strings.Trim(Strings.Mid(Cksi0010.G_GYOSYA_AREA[0], 1, 20)), 20, 2);
				}

				// ワーク向先
				Cksi0010.G_SAGYOSI_AREA[i, 5] = TXT_Mukesaki[TXT_Mukesaki.GetIndex(TXT_Mukesaki[shIdx])].Text;
				// ワーク数量
				Cksi0010.G_SAGYOSI_AREA[i, 8] = IPPAN.Numeric_Hensyu(TXT_Suryo[TXT_Suryo.GetIndex(TXT_Suryo[shIdx])].Text);

				if (IPPAN.C_Allspace(TXT_Suibun[TXT_Suibun.GetIndex(TXT_Suibun[shIdx])].Text) == 0)
				{
					Cksi0010.G_SAGYOSI_AREA[i, 9] = Convert.ToString(0);
				}
				else
				{
					// ワーク水分引
					Cksi0010.G_SAGYOSI_AREA[i, 9] = IPPAN2.Numeric_Hensyu4(TXT_Suibun[TXT_Suibun.GetIndex(TXT_Suibun[shIdx])].Text);
				}
			}
		}

		public void Gamen_Clear1()
		{
			for (short i = 0; i <= 9; i++)
			{
				// 区分
				TXT_kbn[i].Text = "";
				// 品目コード
				TXT_HinCd[i].Text = "";
				// 品目名
				LBL_HinNm[i].Text = "";
				// 業者コード
				TXT_GyoCd[i].Text = "";
				// 業者名
				LBL_GyoNm[i].Text = "";
				// 向先
				TXT_Mukesaki[i].Text = "";
				// 数量
				TXT_Suryo[i].Text = "";
				// 水分引
				TXT_Suibun[i].Text = "";
			}
		}

		public void Gamen_Set()
		{
			int i = 0;
			short shIdx = 0;

			for (i = (Convert.ToInt32(Cksi0010.G_Page) * 10) - 10; i <= (Convert.ToInt32(Cksi0010.G_Page) * 10) - 1; i += 1)
			{
				shIdx = Convert.ToInt16(Strings.Right(Convert.ToString(i), 1));
// 2017.04.28 yoshitake 副資材棚卸システム再構築 追加対応 start.
                string approval = Strings.Trim(Cksi0010.G_SAGYOSI_AREA[(Convert.ToInt32(Cksi0010.G_Page) * 10) - 10  + shIdx, 10]);
                if (approval == "1")
                {
                    // 区分
                    TXT_kbn[shIdx].Enabled = false;
                    // 品目コード
                    TXT_HinCd[shIdx].Enabled = false;
                    // 業者コード
                    TXT_GyoCd[shIdx].Enabled = false;
                    // 向先
                    TXT_Mukesaki[shIdx].Enabled = false;
                    // 数量
                    TXT_Suryo[shIdx].Enabled = false;
                    // 水分引
                    TXT_Suibun[shIdx].Enabled = false;
                }
                else
                {
                    // 区分
                    TXT_kbn[shIdx].Enabled = true;
                    // 品目コード
                    TXT_HinCd[shIdx].Enabled = true;
                    // 業者コード
                    TXT_GyoCd[shIdx].Enabled = true;
                    // 向先
                    TXT_Mukesaki[shIdx].Enabled = true;
                    // 数量
                    TXT_Suryo[shIdx].Enabled = true;
                    // 水分引
                    TXT_Suibun[shIdx].Enabled = true;
                }
// 2017.04.28 yoshitake 副資材棚卸システム再構築 追加対応 end.

				// 区分
				TXT_kbn[shIdx].Text = Strings.Trim(Cksi0010.G_SAGYOSI_AREA[(Convert.ToInt32(Cksi0010.G_Page) * 10) - 10 + TXT_kbn.GetIndex(TXT_kbn[shIdx]), 2]);
				switch (Strings.Trim(TXT_kbn[shIdx].Text))
				{
					//13.07.02    ISV-TRUC    品名ＣＤのLeaveイベントで、区分の入力値が、9（削除）以外、向先のEnabledへTrueをセット。
					case "1":
					case "4":
					case "9":
						TXT_Mukesaki[shIdx].Enabled = false;
						TXT_Mukesaki[shIdx].Text = "";
						break;
					//End 13.07.02    ISV-TRUC   
					case "2":
						TXT_GyoCd[shIdx].Enabled = false;
						TXT_GyoCd[shIdx].Text = "";
						LBL_GyoNm[shIdx].Text = "";
						break;
				}

				// 品目コード
				TXT_HinCd[shIdx].Text = Strings.Trim(Cksi0010.G_SAGYOSI_AREA[(Convert.ToInt32(Cksi0010.G_Page) * 10) - 10 + TXT_HinCd.GetIndex(TXT_HinCd[shIdx]), 3]);
				// 品目名
				LBL_HinNm[shIdx].Text = Strings.Trim(Strings.Mid(Cksi0010.G_SAGYOSI_AREA[(Convert.ToInt32(Cksi0010.G_Page) * 10) - 10 + LBL_HinNm.GetIndex(LBL_HinNm[shIdx]), 6], 1, 15));
				// 業者コード
				TXT_GyoCd[shIdx].Text = Strings.Trim(Cksi0010.G_SAGYOSI_AREA[(Convert.ToInt32(Cksi0010.G_Page) * 10) - 10 + TXT_GyoCd.GetIndex(TXT_GyoCd[shIdx]), 4]);
				// 業者名
				LBL_GyoNm[shIdx].Text = Strings.Trim(Strings.Mid(Cksi0010.G_SAGYOSI_AREA[(Convert.ToInt32(Cksi0010.G_Page) * 10) - 10 + LBL_GyoNm.GetIndex(LBL_GyoNm[shIdx]), 7], 1, 15));
				// 向先
				TXT_Mukesaki[shIdx].Text = Strings.Trim(Cksi0010.G_SAGYOSI_AREA[(Convert.ToInt32(Cksi0010.G_Page) * 10) - 10 + TXT_Mukesaki.GetIndex(TXT_Mukesaki[shIdx]), 5]);
				// 数量
				TXT_Suryo[shIdx].Text = IPPAN.Money_Hensyu(Strings.Trim(Cksi0010.G_SAGYOSI_AREA[(Convert.ToInt32(Cksi0010.G_Page) * 10) - 10 + TXT_Suryo.GetIndex(TXT_Suryo[shIdx]), 8]), "###,###,##0");
				Cksi0010.G_Suryo[Convert.ToInt32(shIdx)] = IPPAN2.Numeric_Hensyu3(Strings.Trim(TXT_Suryo[shIdx].Text));

				//副資材品目マスタの検索
				if (Cksi0010.Hinmoku_Kensaku(TXT_HinCd[shIdx].Text) == 0)
				{
					if (Strings.Trim(Cksi0010.G_HINMOKU_AREA[1]) != "1")
					{
						TXT_Suibun[shIdx].Enabled = false;
						TXT_Suibun[shIdx].Text = "";
					}
					else
					{
						// 水分引
						TXT_Suibun[shIdx].Text = IPPAN.Money_Hensyu(Strings.Trim(Cksi0010.G_SAGYOSI_AREA[(Convert.ToInt32(Cksi0010.G_Page) * 10) - 10 + TXT_Suibun.GetIndex(TXT_Suibun[shIdx]), 9]), "#0.00");
						Cksi0010.G_Suibun[Convert.ToInt32(shIdx)] = IPPAN2.Numeric_Hensyu4(Strings.Trim(TXT_Suibun[shIdx].Text));
					}
				}
				else
				{
					TXT_Suibun[shIdx].Text = "";
				}
			}
		}

		public void Gamen_Clear()
		{
			short i = 0;
			short j = 0;

			// 作業年
			TXT_YY.Text = "";
			// 作業月
			TXT_MM.Text = "";
			// 作業日
			TXT_DD.Text = "";

			for (j = 0; j <= 9; j++)
			{
				// 区分
				TXT_kbn[j].Text = "";
				// 品目コード
				TXT_HinCd[j].Text = "";
				// 品目名
				LBL_HinNm[j].Text = "";
				// 業者コード
				TXT_GyoCd[j].Text = "";
				// 業者名
				LBL_GyoNm[j].Text = "";
				// 向先
				TXT_Mukesaki[j].Text = "";
				// 数量
				TXT_Suryo[j].Text = "";
				// 水分引
				TXT_Suibun[j].Text = "";
			}
			for (i = 0; i <= (Cksi0010.G_Page1 + 1) * 10 - 1; i += 1)
			{
				// 区分
				Cksi0010.G_SAGYOSI_AREA[i, 2] = "";
				// 品目コード
				Cksi0010.G_SAGYOSI_AREA[i, 3] = "";
				// 品目名
				Cksi0010.G_SAGYOSI_AREA[i, 6] = "";
				// 業者コード
				Cksi0010.G_SAGYOSI_AREA[i, 4] = "";
				// 業者名
				Cksi0010.G_SAGYOSI_AREA[i, 7] = "";
				// 向先
				Cksi0010.G_SAGYOSI_AREA[i, 5] = "";
				// 数量
				Cksi0010.G_SAGYOSI_AREA[i, 8] = "";
				// 水分引
				Cksi0010.G_SAGYOSI_AREA[i, 9] = "";
// 2017.04.28 yoshitake 副資材棚卸システム再構築 追加対応 start.
                // 承認/未承認フラグ
                Cksi0010.G_SAGYOSI_AREA[i, 10] = "0";
// 2017.04.28 yoshitake 副資材棚卸システム再構築 追加対応 end.
			}

			Cksi0010.G_Page = "1";
			Cksi0010.G_Page1 = 0;
			G_flg2 = "";
			//ページ数欄
			LBL_Page.Text = "";
		}

		public short Hizuke_Check()
		{
			short functionReturnValue = 0;
			string L_YMD = null;

			functionReturnValue = 1;

			if (IPPAN.C_Allspace(TXT_YY.Text) != 0 || IPPAN.C_Allspace(TXT_MM.Text) != 0 || IPPAN.C_Allspace(TXT_DD.Text) != 0)
			{

				L_YMD = TXT_YY.Text + TXT_MM.Text + TXT_DD.Text;
				//   13.07.23    ISV-TRUC  
				//if (IPPAN.Date_Henkan(ref L_YMD) != 0)
				//{
				//    //フォーカスセット
				//    TXT_YY.Focus();
				//    TXT_YY_Enter(TXT_YY, new System.EventArgs());
				//    //日付入力エラー
				//    IPPAN.Error_Msg("E201", 0, "");
				//    return functionReturnValue;
				//}
				//   End 13.07.23    ISV-TRUC  
				if (IPPAN.Date_Check2(L_YMD) != 0)
				{
					//フォーカスセット
					TXT_YY.Focus();
					TXT_YY_Enter(TXT_YY, new System.EventArgs());
					//日付入力エラー
					IPPAN.Error_Msg("E201", 0, "");
					return functionReturnValue;
				}
			}
			else
			{
				//フォーカスセット
				TXT_YY.Focus();
				TXT_YY_Enter(TXT_YY, new System.EventArgs());
				//必須項目入力エラー
				IPPAN.Error_Msg("E200", 0, "");
				return functionReturnValue;
			}
			Cksi0010.G_YMD = Strings.Mid(L_YMD, 1, 4) + Strings.Mid(L_YMD, 5, 2) + Strings.Mid(L_YMD, 7, 2);

			functionReturnValue = 0;
			return functionReturnValue;
		}

		public void Enabled_False2()
		{
			// 作業年
			TXT_YY.Enabled = false;
			// 作業月
			TXT_MM.Enabled = false;
			// 作業日
			TXT_DD.Enabled = false;

			for (short i = 0; i <= 9; i++)
			{
				// 区分
				TXT_kbn[i].Enabled = true;
				// 品目コード
				TXT_HinCd[i].Enabled = true;
				// 業者コード
				TXT_GyoCd[i].Enabled = true;
				// 向先
				TXT_Mukesaki[i].Enabled = true;
				// 数量
				TXT_Suryo[i].Enabled = true;
				// 水分引
				TXT_Suibun[i].Enabled = true;
			}
			// 検索表示
			CMD_Button[0].Enabled = false;
			// 閉じる
			CMD_Button[1].Enabled = true;
			// 前へ
			CMD_Button[2].Enabled = true;
			// 次へ
			CMD_Button[3].Enabled = true;
			// 入力確定
			CMD_Button[4].Enabled = true;
			// クリア
			CMD_Button[5].Enabled = true;
		}

		public void Enabled_False1()
		{
			// 作業年
			TXT_YY.Enabled = true;
			// 作業月
			TXT_MM.Enabled = true;
			// 作業日
			TXT_DD.Enabled = true;

			for (short i = 0; i <= 9; i++)
			{
                // 区分
                TXT_kbn[i].Enabled = false;
                // 品目コード
                TXT_HinCd[i].Enabled = false;
                // 業者コード
                TXT_GyoCd[i].Enabled = false;
                // 向先
                TXT_Mukesaki[i].Enabled = false;
                // 数量
                TXT_Suryo[i].Enabled = false;
                // 水分引
                TXT_Suibun[i].Enabled = false;
			}
			// 検索表示
			CMD_Button[0].Enabled = true;
			// 閉じる
			CMD_Button[1].Enabled = true;
			// 前へ
			CMD_Button[2].Enabled = false;
			// 次へ
			CMD_Button[3].Enabled = false;
			// 入力確定
			CMD_Button[4].Enabled = false;
			// クリア
			CMD_Button[5].Enabled = true;
		}

		private void CMD_Button_Click(System.Object eventSender, System.EventArgs eventArgs)
		{
			short Index = CMD_Button.GetIndex((Button)eventSender);
			DialogResult dlgRlt = DialogResult.None;

			switch (Index)
			{
				case 0:
					//検索表示
					//入力された日付をチェック
					if (Hizuke_Check() != 0)
					{
						return;
					}
					//副資材コントロールトランの検索
					if (Cksi0010.Control_Kensaku("CKSI") != 0)
					{
						IPPAN.Error_Msg("E701", 0, " ");
						return;
					}
					if (Strings.Mid(Cksi0010.G_YMD, 1, 6) != Cksi0010.G_CONTROL_AREA[0])
					{
						//入力された年月を確認して下さい
						IPPAN.Error_Msg("I700", 0, "");
					}
                    Enabled_False2();
                    //副資材作業誌トランの検索
                    if (Cksi0010.Sagyosi_Kensaku(Cksi0010.G_YMD) == 0)
                    {
                        //データがある時は画面に表示する
                        Gamen_Set();
                        LBL_Page.Text = C_COMMON.FormatToNum(Cksi0010.G_Page, "00") + "/" + String.Format("{0:00}", Cksi0010.G_Page1);
                        G_flg = Convert.ToString(0);
                    }
                    else
                    {
                        Gamen_Clear1();
                        //データがなかったときは何も表示しない
                        Cksi0010.G_Page1 = Convert.ToInt16(String.Format("{0:00}", Cksi0010.G_N));
                        LBL_Page.Text = C_COMMON.FormatToNum(Cksi0010.G_Page, "00") + "/" + String.Format("{0:00}", Cksi0010.G_Page1 + 1);
                        G_flg = Convert.ToString(1);
                    }

					CMD_Button[2].Enabled = false;
// 2017.04.28 yoshitake 副資材棚卸システム再構築 追加対応 start.
                    if (TXT_kbn[0].Enabled)
                    {
                        TXT_kbn[0].Focus();
                        IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
                    }
                    //TXT_kbn[0].Focus();
                    //IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
// 2017.04.28 yoshitake 副資材棚卸システム再構築 追加対応 end.
					break;
				case 1:
					//閉じる
					this.Close();
					break;
				case 2:
					//前へ
					//ライン以下の入力内容をチェック
					if (Gamen_Check() != 0)
					{
						return;
					}
					if (G_flg1 == "0")
					{
						//画面に入力された内容をワークに送る
						Work_Set();
					}
					Cksi0010.G_Page = Convert.ToString(Convert.ToInt32(Cksi0010.G_Page) - 1);
					if (G_flg1 == "1")
					{
						Cksi0010.G_Page1 = Cksi0010.G_Page1 - 1;
					}
					Enabled_False2();
					if (Convert.ToDouble(G_flg) == 0)
					{
						if (Convert.ToInt32(Cksi0010.G_Page) > Cksi0010.G_Page1)
						{
							//画面にワークの内容をセットする
							Gamen_Set1();
						}
						else
						{
							//画面にデータベースの内容をセットする
							Gamen_Set();
						}
					}
					else
					{
						//画面にワークの内容をセットする
						Gamen_Set1();
					}
					LBL_Page.Text = C_COMMON.FormatToNum(Cksi0010.G_Page, "00") + "/" + String.Format("{0:00}", Cksi0010.G_Page1 + 1);

					//１ページ目のときは前へボタンを使用不可にする
					if (Convert.ToInt32(Cksi0010.G_Page) == 1)
					{
						CMD_Button[2].Enabled = false;
					}
// 2017.04.28 yoshitake 副資材棚卸システム再構築 追加対応 start.
                    if (TXT_kbn[0].Enabled)
                    {
                        TXT_kbn[0].Focus();
                        IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
                    }
                    //TXT_kbn[0].Focus();
                    //IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
// 2017.04.28 yoshitake 副資材棚卸システム再構築 追加対応 end.
                    if (G_EKISAN_PARAM.Equals("1") || G_EKISAN_PARAM.Equals("2"))
                    {
                        this._CMD_Button_1.Enabled = false;
                        this._CMD_Button_5.Enabled = false;
                    }
					break;
				case 3:
					//次へ
					//ライン以下の入力内容をチェック
                    if (Gamen_Check() != 0)
                    {
                        return;
                    }
                    if (G_flg1 == "0")
                    {
                        //画面に入力された内容をワークに送る
                        Work_Set();
                    }
                    Cksi0010.G_Page = Convert.ToString(Convert.ToInt32(Cksi0010.G_Page) + 1);
                    Enabled_False2();
                    if (Convert.ToDouble(G_flg) == 0)
                    {
                        if (Convert.ToInt32(Cksi0010.G_Page) > Cksi0010.G_Page1)
                        {
                            if (G_flg1 == "1")
                            {
                                //データがなかったときは何も表示しない
                                Gamen_Clear1();
                            }
                            else
                            {
                                //画面にワークの内容をセットする
                                Gamen_Set1();
                            }
                            if (Convert.ToInt32(Cksi0010.G_Page) > Cksi0010.G_Page1 + 1)
                            {
                                Cksi0010.G_Page1 = Cksi0010.G_Page1 + 1;
                            }
                            LBL_Page.Text = C_COMMON.FormatToNum(Cksi0010.G_Page, "00") + "/" + String.Format("{0:00}", Cksi0010.G_Page1 + 1);
                            G_flg2 = Convert.ToString(1);
                        }
                        else
                        {
                            Gamen_Set();
                            //画面にデータベースの内容をセットする
                            if (G_flg2 != "1")
                            {
                                LBL_Page.Text = C_COMMON.FormatToNum(Cksi0010.G_Page, "00") + "/" + String.Format("{0:00}", Cksi0010.G_Page1);
                            }
                            else
                            {
                                LBL_Page.Text = C_COMMON.FormatToNum(Cksi0010.G_Page, "00") + "/" + String.Format("{0:00}", Cksi0010.G_Page1 + 1);
                            }
                        }
                    }
                    else
                    {
                        if (G_flg1 == "1")
                        {
                            //データがなかったときは何も表示しない
                            Gamen_Clear1();
                        }
                        else
                        {
                            //画面にワークの内容をセットする
                            Gamen_Set1();
                        }
                        if (Convert.ToInt32(Cksi0010.G_Page) > Cksi0010.G_Page1 + 1)
                        {
                            Cksi0010.G_Page1 = Cksi0010.G_Page1 + 1;
                        }
                        LBL_Page.Text = C_COMMON.FormatToNum(Cksi0010.G_Page, "00") + "/" + String.Format("{0:00}", Cksi0010.G_Page1 + 1);
                    }

                    //１０ページ目のときは次へボタンを使用不可にする
                    if (Convert.ToInt32(Cksi0010.G_Page) == 10)
                    {
                        CMD_Button[3].Enabled = false;
                    }
// 2017.04.28 yoshitake 副資材棚卸システム再構築 追加対応 start.
                    if (TXT_kbn[0].Enabled)
                    {
                        TXT_kbn[0].Focus();
                        IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
                    }
                    //TXT_kbn[0].Focus();
                    //IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
// 2017.04.28 yoshitake 副資材棚卸システム再構築 追加対応 end.
                    if (G_EKISAN_PARAM.Equals("1") || G_EKISAN_PARAM.Equals("2"))
                    {
                        this._CMD_Button_1.Enabled = false;
                        this._CMD_Button_5.Enabled = false;
                    }
                    break;
				case 4:
					//入力確定
					//ライン以下の入力内容をチェック
					if (Gamen_Check() != 0)
					{
						return;
					}
					//登録しますか
					dlgRlt = IPPAN.Error_Msg("I607", 4, " ");
					// [いいえ] ボタンを選択した場合
					if (dlgRlt == DialogResult.No)
					{
						IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
						return;
					}
					if (G_flg1 == "0")
					{
						//画面に入力された内容をワークに送る
						Work_Set();
					}

					using (C_ODBC db = new C_ODBC())
					{
						try
						{
							//DB接続
							db.Connect();
							//トランザクション開始
							db.BeginTrans();

							if (Convert.ToInt32(G_flg) == 0)
							{
								if (Cksi0010.Del_Sagyosi(db, Cksi0010.G_YMD) == false)
								{
									return;
								}
							}
							if (Cksi0010.Sagyosi_Touroku(db, Cksi0010.G_YMD) == 1)
							{
								IPPAN.Error_Msg("I503", 0, " ");
								return;
							}

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

					//画面内容をクリアする
					Gamen_Clear();
					Enabled_False1();
					TXT_YY.Focus();
					IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
// 2016.12.12 yoshitake 副資材棚卸システム再構築 start.
                    if (G_EKISAN_PARAM.Equals("1") || G_EKISAN_PARAM.Equals("2"))
                    {
                        ProcessStartInfo processStartInfo = new ProcessStartInfo();
                        processStartInfo.FileName = "C:\\EXE\\資材システム\\CKSI0050.exe";

                        // コマンドライン引数を指定
                        // 社員コード
                        processStartInfo.Arguments += G_UserId;
                        // 所属コード 
                        processStartInfo.Arguments += "," + G_OfficeId;
                        // 職位コード
                        processStartInfo.Arguments += "," + G_SyokuiCd;
                        // 呼出し元が副資材棚卸システム
                        processStartInfo.Arguments += ",1";
                        // 棚卸年月日
                        processStartInfo.Arguments += "," + Cksi0010.G_YMD.Substring(0, 6);

                        // 起動
                        Process p = Process.Start(processStartInfo);

                        this.Close();
                    }
// 2016.12.12 yoshitake 副資材棚卸システム再構築 end.
					break;
				case 5:
					//クリア
					//画面内容をクリアする
					Gamen_Clear();
					Enabled_False1();
					TXT_YY.Focus();
					IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
					break;
			}
		}

		string static_TMR_TIMER_Tick_timeval1;
		string static_TMR_TIMER_Tick_timeval2;
		private void TMR_Timer_Tick(System.Object eventSender, System.EventArgs eventArgs)
		{
			static_TMR_TIMER_Tick_timeval2 = Convert.ToString(DateAndTime.TimeOfDay);
			if (static_TMR_TIMER_Tick_timeval1 != static_TMR_TIMER_Tick_timeval2)
			{
				IPPAN.Control_Init(this, "資材班作業入力", "CKSI0010", SO.SO_USERNAME, SO.SO_OFFICENAME, "1.00");
				static_TMR_TIMER_Tick_timeval1 = Convert.ToString(DateAndTime.TimeOfDay);
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
			//TXT_HinCd
			for (short i = 0; i < this.TXT_HinCd.Count(); i++)
			{
				this.TXT_HinCd[i].Enter += new EventHandler(TXT_HinCd_Enter);
				this.TXT_HinCd[i].Leave += new EventHandler(TXT_HinCd_Leave);
			}
			//TXT_Mukesaki
			for (short i = 0; i < this.TXT_Mukesaki.Count(); i++)
			{
				this.TXT_Mukesaki[i].Enter += new EventHandler(TXT_Mukesaki_Enter);
				this.TXT_Mukesaki[i].Leave += new EventHandler(TXT_Mukesaki_Leave);
			}
			//TXT_Suibun
			for (short i = 0; i < this.TXT_Suibun.Count(); i++)
			{
				this.TXT_Suibun[i].Enter += new EventHandler(TXT_Suibun_Enter);
				this.TXT_Suibun[i].Leave += new EventHandler(TXT_Suibun_Leave);
			}
			//TXT_Suryo
			for (short i = 0; i < this.TXT_Suryo.Count(); i++)
			{
				this.TXT_Suryo[i].Enter += new EventHandler(TXT_Suryo_Enter);
				this.TXT_Suryo[i].Leave += new EventHandler(TXT_Suryo_Leave);
			}
			//TXT_kbn
			for (short i = 0; i < this.TXT_kbn.Count(); i++)
			{
				this.TXT_kbn[i].Enter += new EventHandler(TXT_kbn_Enter);
				this.TXT_kbn[i].Leave += new EventHandler(TXT_kbn_Leave);
			}
		}

		private void FRM_CKSI0010M_Load(System.Object eventSender, System.EventArgs eventArgs)
		{
// 2016.12.12 yoshitake 副資材棚卸システム再構築 start.
            //string L_Search = null;
            //string L_Command2 = null;
// 2016.12.12 yoshitake 副資材棚卸システム再構築 end.

			//プロジェクト名を設定
			C_COMMON.G_COMMON_MSGTITLE = "CKSI0010";

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
// 2016.12.12 yoshitake 副資材棚卸システム再構築 start.
			//L_Search = ",";
// 2016.12.12 yoshitake 副資材棚卸システム再構築 end.

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
                G_UserId = args[0];
                G_OfficeId = args[1];
                G_SyokuiCd = args[2];

                if (args.Length >= 4)
                {
                    G_EKISAN_PARAM = args[3];
                    G_KENSIN_DATA_FILE_PATH = args[4];
                    G_TANAOROSI_YEAR_MONTH = args[5];
                }

                //G_UserId = Strings.Mid(IPPAN.G_COMMAND, 1, Strings.InStr(1, IPPAN.G_COMMAND, L_Search, CompareMethod.Binary) - 1);
                //L_Command2 = Strings.Mid(IPPAN.G_COMMAND, Strings.InStr(1, IPPAN.G_COMMAND, L_Search, CompareMethod.Binary) + 1, 30);
                //G_OfficeId = Strings.Mid(L_Command2, 1, Strings.InStr(1, L_Command2, L_Search, CompareMethod.Binary) - 1);
                //G_SyokuiCd = Strings.Mid(L_Command2, Strings.InStr(1, L_Command2, L_Search, CompareMethod.Binary) + 1, 2);
// 2016.12.12 yoshitake 副資材棚卸システム再構築 end.
                
				IPPAN.G_RET = FS_USER_CHECK.User_Check(G_UserId);

				//ユーザーＩＤが副資材ユーザ以外かを見る
				if (IPPAN.G_RET != 0)
				{
					System.Environment.Exit(0);
				}
				else
				{
                    IPPAN.G_RET = IPPAN2.TIMER_SET(G_UserId);
                    if (IPPAN.G_RET != 0)
                    {
                        System.Environment.Exit(0);
                    }
				}
			}
// 2016.12.12 yoshitake 副資材棚卸システム再構築 start.
            // その他の資材区分で入力してはいけない品目の一覧を取得する。
            othersNotInputHinmokuCode = Cksi0010.GetOthersNotInputHinmokuCode();

            if (G_EKISAN_PARAM.Equals("1") || G_EKISAN_PARAM.Equals("2"))
            {
                switch (Convert.ToInt32(G_EKISAN_PARAM))
                {
                    case (int)SIZAI_IN_OUT_CATEGORY.IN:
                        kensinExcelData = ReadSizaiInOutData(SIZAI_IN_OUT_CATEGORY.IN);
                        break;
                    case (int)SIZAI_IN_OUT_CATEGORY.OUT:
                        kensinExcelData = ReadSizaiInOutData(SIZAI_IN_OUT_CATEGORY.OUT);
                        break;
                    default:
                        break;
                }

                Cksi0010.G_YMD = GetYearMonthDay(G_TANAOROSI_YEAR_MONTH);
                // 作業年
                TXT_YY.Text = Cksi0010.G_YMD.Substring(0, 4);
                // 作業月
                TXT_MM.Text = Cksi0010.G_YMD.Substring(4, 2);
                // 作業日
                TXT_DD.Text = Cksi0010.G_YMD.Substring(6, 2);

                Cksi0010.G_Page = "1";

                Enabled_False2();

                //副資材作業誌トランの検索
                if (Cksi0010.Sagyosi_Kensaku(Cksi0010.G_YMD, G_EKISAN_PARAM, kensinExcelData) == 0)
                {
                    //データがある時は画面に表示する
                    Gamen_Set();
                    LBL_Page.Text = C_COMMON.FormatToNum(Cksi0010.G_Page, "00") + "/" + String.Format("{0:00}", Cksi0010.G_Page1);
                    G_flg = Convert.ToString(0);
                }
                else
                {
                    Gamen_Clear1();
                    //データがなかったときは何も表示しない
                    Cksi0010.G_Page1 = Convert.ToInt16(String.Format("{0:00}", Cksi0010.G_N));
                    LBL_Page.Text = C_COMMON.FormatToNum(Cksi0010.G_Page, "00") + "/" + String.Format("{0:00}", Cksi0010.G_Page1 + 1);
                    G_flg = Convert.ToString(1);
                }

                for (int i = 0; i < Cksi0010.G_Page1 - 1; i++)
                {
                    GoNextLogic();
                }

                if (G_EKISAN_PARAM.Equals("1") || G_EKISAN_PARAM.Equals("2"))
                {
                    this._CMD_Button_1.Enabled = false;
                    this._CMD_Button_5.Enabled = false;
                }
            }

            else
            {
                //画面入力不可
                Enabled_False1();
                //ページ数を１ページ目にセット
                Cksi0010.G_Page = "1";
                G_flg2 = "";
                TXT_YY.Focus();
                IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
            }
// 2016.12.12 yoshitake 副資材棚卸システム再構築 end.

		}

		private void TXT_DD_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
			if (Strings.Len(TXT_DD.Text) == 2)
			{
				CMD_Button[0].Focus();
			}
		}

		private void TXT_DD_Enter(System.Object eventSender, System.EventArgs eventArgs)
		{
			IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
		}

		private void TXT_DD_Leave(System.Object eventSender, System.EventArgs eventArgs)
		{
			TXT_DD.Text = C_COMMON.FormatToNum(TXT_DD.Text, "00");
			IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
		}

		private void TXT_GyoCd_Enter(System.Object eventSender, System.EventArgs eventArgs)
		{
			short Index = TXT_GyoCd.GetIndex((TextBox)eventSender);
			WK_GyosyaCd = TXT_GyoCd[Index].Text;
			IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
		}

		private void TXT_GyoCd_Leave(System.Object eventSender, System.EventArgs eventArgs)
		{
			short Index = TXT_GyoCd.GetIndex((TextBox)eventSender);
			if (TXT_GyoCd[Index].Text != WK_GyosyaCd)
			{
				if (IPPAN.C_Allspace(TXT_GyoCd[Index].Text) != 0)
				{
					if (IPPAN.Input_Check(TXT_GyoCd[Index].Text) != 0)
					{
						LBL_GyoNm[Index].Text = "";
						//フォーカスセット
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
					if (Cksi0010.Gyosya_Kensaku(Strings.Mid(TXT_GyoCd[Index].Text, 1, 4)) != 0)
					{
						LBL_GyoNm[Index].Text = "";
						IPPAN.Error_Msg("E503", 0, " ");
						//フォーカスセット
						TXT_GyoCd[Index].Focus();
						TXT_GyoCd_Enter(TXT_GyoCd[Index], new System.EventArgs());
						return;
					}
					else
					{
						LBL_GyoNm[Index].Text = Strings.Left(Strings.Trim(Cksi0010.G_GYOSYA_AREA[0]), 15);
					}
				}
				else
				{
					LBL_GyoNm[Index].Text = "";
					//フォーカスセット
					TXT_GyoCd[Index].Focus();
					TXT_GyoCd_Enter(TXT_GyoCd[Index], new System.EventArgs());
					//必須項目入力エラー
					IPPAN.Error_Msg("E200", 0, "");
					return;
				}
			}
			IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
		}

		private void TXT_HinCd_Enter(System.Object eventSender, System.EventArgs eventArgs)
		{
			short Index = TXT_HinCd.GetIndex((TextBox)eventSender);
			WK_HinmokuCd = TXT_HinCd[Index].Text;
			IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
		}

		private void TXT_HinCd_Leave(System.Object eventSender, System.EventArgs eventArgs)
		{
			short Index = TXT_HinCd.GetIndex((TextBox)eventSender);
			if (TXT_HinCd[Index].Text != WK_HinmokuCd)
			{
				if (IPPAN.C_Allspace(TXT_HinCd[Index].Text) != 0)
				{
					if (IPPAN.Input_Check(TXT_HinCd[Index].Text) != 0)
					{
						//フォーカスセット
						TXT_HinCd[Index].Focus();
						TXT_HinCd_Enter(TXT_HinCd[Index], new System.EventArgs());
						//入力エラー
						IPPAN.Error_Msg("E202", 0, " ");
						return;
					}
					if (Cksi0010.Hinmoku_Kensaku(Strings.Mid(TXT_HinCd[Index].Text, 1, 4)) != 0)
					{
						IPPAN.Error_Msg("E701", 0, " ");
						TXT_HinCd[Index].Focus();
						TXT_HinCd_Enter(TXT_HinCd[Index], new System.EventArgs());
						return;
					}
					else
					{
						LBL_HinNm[Index].Text = Strings.Left(Strings.Trim(Cksi0010.G_HINMOKU_AREA[0]), 15);
						if (Strings.Trim(Cksi0010.G_HINMOKU_AREA[1]) != "1")
						{
							TXT_Suibun[Index].Enabled = false;
							TXT_Suibun[Index].Text = "";
						}
						else
						{
							TXT_Suibun[Index].Enabled = true;
						}
						if (IPPAN.C_Allspace(Strings.Trim(Cksi0010.G_HINMOKU_AREA[2])) == 1)
						{
							//13.07.02    ISV-TRUC    品名ＣＤのLeaveイベントで、区分の入力値が、1,4,9（削除）以外、向先のEnabledへTrueをセット。
							//if (Strings.Trim(TXT_kbn[Index].Text) == "2" || Strings.Trim(TXT_kbn[Index].Text) == "3")
							//{
							//    TXT_Mukesaki[Index].Text = Strings.Trim(Cksi0010.G_HINMOKU_AREA[2]);
							//}
							//else
							//{
							//    TXT_Mukesaki[Index].Text = "";
							//}

							if ((Strings.Trim(TXT_kbn[Index].Text) == "1") || (Strings.Trim(TXT_kbn[Index].Text) == "4") || (Strings.Trim(TXT_kbn[Index].Text) == "9"))
							{
								// TXT_Mukesaki[Index].Text = Strings.Trim(Cksi0010.G_HINMOKU_AREA[2]);
								TXT_Mukesaki[Index].Text = "";
								TXT_Mukesaki[Index].Enabled = false;
							}
							else
							{
								// TXT_Mukesaki[Index].Text = "";
								TXT_Mukesaki[Index].Text = Strings.Trim(Cksi0010.G_HINMOKU_AREA[2]);
							}
							// End 13.07.02    ISV-TRUC  
						}
						else
						{
							TXT_Mukesaki[Index].Text = "";
						}

						if (TXT_GyoCd[Index].Enabled == true)
						{
							IPPAN.G_RET = Cksi0010.Syoki_Gyosya_Kensaku(TXT_HinCd[Index].Text);
							if (IPPAN.G_RET == 0)
							{
								TXT_GyoCd[Index].Text = Cksi0010.G_Syoki_Gyosya_AREA[2];
								if (Cksi0010.Gyosya_Kensaku(Strings.Mid(TXT_GyoCd[Index].Text, 1, 4)) != 0)
								{
									LBL_GyoNm[Index].Text = "";
									IPPAN.Error_Msg("E503", 0, " ");
									//フォーカスセット
									TXT_GyoCd[Index].Focus();
									TXT_GyoCd_Enter(TXT_GyoCd[Index], new System.EventArgs());
									return;
								}
								else
								{
									LBL_GyoNm[Index].Text = Strings.Left(Strings.Trim(Cksi0010.G_GYOSYA_AREA[0]), 15);
								}
							}
						}
					}
				}
				else
				{
					LBL_HinNm[Index].Text = "";
					//フォーカスセット
					TXT_HinCd[Index].Focus();
					TXT_HinCd_Enter(TXT_HinCd[Index], new System.EventArgs());
					//必須項目入力エラー
					IPPAN.Error_Msg("E200", 0, "");
					return;
				}
			}
			else
			{
				if (Strings.Trim(TXT_kbn[Index].Text) == "1" || Strings.Trim(TXT_kbn[Index].Text) == "2" || Strings.Trim(TXT_kbn[Index].Text) == "3" || Strings.Trim(TXT_kbn[Index].Text) == "4")
				{
					if (IPPAN.C_Allspace(TXT_HinCd[Index].Text) != 0)
					{
						if (Cksi0010.Hinmoku_Kensaku(Strings.Mid(TXT_HinCd[Index].Text, 1, 4)) != 0)
						{
							LBL_HinNm[Index].Text = "";

							IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
							return;
						}
						else
						{
							LBL_HinNm[Index].Text = Strings.Left(Strings.Trim(Cksi0010.G_HINMOKU_AREA[0]), 15);
							if (string.IsNullOrEmpty(Strings.Trim(Cksi0010.G_HINMOKU_AREA[1])))
							{
								TXT_Suibun[Index].Enabled = false;
								TXT_Suibun[Index].Text = "";
							}
						}
					}
				}
			}
			IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
		}

		private void TXT_kbn_Enter(System.Object eventSender, System.EventArgs eventArgs)
		{
			short Index = TXT_kbn.GetIndex((TextBox)eventSender);
			TXT_HinCd[Index].Enabled = true;
			TXT_GyoCd[Index].Enabled = true;
			TXT_Mukesaki[Index].Enabled = true;
			TXT_Suryo[Index].Enabled = true;
			TXT_Suibun[Index].Enabled = true;
			WK_Kbn = TXT_kbn[Index].Text;
			IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
		}

		private void TXT_kbn_Leave(System.Object eventSender, System.EventArgs eventArgs)
		{
			short Index = TXT_kbn.GetIndex((TextBox)eventSender);
            if (!IsShowExistSizaiTanaorosiMstMsg)
            {
                if (WK_Kbn != Strings.Trim(TXT_kbn[Index].Text))
                {
                    if (IPPAN.C_Allspace(TXT_kbn[Index].Text) != 0)
                    {
                        if (IPPAN.Input_Check(TXT_kbn[Index].Text) != 0)
                        {
                            //フォーカスセット
                            TXT_kbn[Index].Focus();
                            TXT_kbn_Enter(TXT_kbn[Index], new System.EventArgs());
                            //入力エラー
                            IPPAN.Error_Msg("E202", 0, " ");
                            return;
                        }
                        IPPAN.G_RET = IPPAN.Numeric_Check2(TXT_kbn[Index].Text);
                        if (IPPAN.G_RET == 1)
                        {
                            TXT_kbn[Index].Focus();
                            TXT_kbn_Enter(TXT_kbn[Index], new System.EventArgs());
                            //入力エラー
                            IPPAN.Error_Msg("E202", 0, " ");
                            IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                            return;
                        }
                        switch (Strings.Trim(TXT_kbn[Index].Text))
                        {
                            case "1":
                            case "4":
                                TXT_HinCd[Index].Enabled = true;
                                TXT_GyoCd[Index].Enabled = true;

                                TXT_Mukesaki[Index].Enabled = false;
                                TXT_Mukesaki[Index].Text = "";

                                TXT_Suryo[Index].Enabled = true;
                                TXT_Suibun[Index].Enabled = true;
                                break;
                            case "2":
                                TXT_HinCd[Index].Enabled = true;
                                TXT_GyoCd[Index].Enabled = false;
                                TXT_GyoCd[Index].Text = "";
                                LBL_GyoNm[Index].Text = "";
                                TXT_Mukesaki[Index].Enabled = true;
                                TXT_Suryo[Index].Enabled = true;
                                TXT_Suibun[Index].Enabled = true;
                                break;
                            case "3":
                                TXT_HinCd[Index].Enabled = true;
                                TXT_GyoCd[Index].Enabled = true;
                                TXT_Mukesaki[Index].Enabled = true;
                                TXT_Suryo[Index].Enabled = true;
                                TXT_Suibun[Index].Enabled = true;
                                break;
                            case "9":
                                TXT_HinCd[Index].Enabled = false;
                                TXT_HinCd[Index].Text = "";
                                LBL_HinNm[Index].Text = "";
                                TXT_GyoCd[Index].Enabled = false;
                                TXT_GyoCd[Index].Text = "";
                                LBL_GyoNm[Index].Text = "";
                                TXT_Mukesaki[Index].Enabled = false;
                                TXT_Mukesaki[Index].Text = "";
                                TXT_Suryo[Index].Enabled = false;
                                TXT_Suryo[Index].Text = "";
                                TXT_Suibun[Index].Enabled = false;
                                TXT_Suibun[Index].Text = "";

                                //  13.08.23    ISV-TRUC
                                int IndexAdd = Index + 1;
                                if (IndexAdd > 9)
                                {
                                    this.SelectNextControl(TXT_kbn[Index], true, true, true, true);
                                }
                                else
                                {
                                    short ShortIndexAdd = short.Parse(IndexAdd.ToString());
                                    TXT_kbn[ShortIndexAdd].Focus();
                                }

                                //  End 13.08.23    ISV-TRUC
                                break;
                            default:
                                //フォーカスセット
                                TXT_kbn[Index].Focus();
                                TXT_kbn_Enter(TXT_kbn[Index], new System.EventArgs());
                                //入力エラー
                                IPPAN.Error_Msg("E202", 0, " ");
                                return;
                        }
                    }
                    else
                    {
                        //フォーカスセット
                        TXT_kbn[Index].Focus();
                        TXT_kbn_Enter(TXT_kbn[Index], new System.EventArgs());
                        //必須項目入力エラー
                        IPPAN.Error_Msg("E200", 0, "");
                        return;
                    }
                }
                else
                {
                    switch (Strings.Trim(TXT_kbn[Index].Text))
                    {
                        case "1":
                        case "4":
                            TXT_HinCd[Index].Enabled = true;
                            TXT_GyoCd[Index].Enabled = true;
                            TXT_Mukesaki[Index].Enabled = false;
                            TXT_Mukesaki[Index].Text = "";
                            TXT_Suryo[Index].Enabled = true;
                            TXT_Suibun[Index].Enabled = true;
                            break;
                        case "2":
                            TXT_HinCd[Index].Enabled = true;
                            TXT_GyoCd[Index].Enabled = false;
                            TXT_GyoCd[Index].Text = "";
                            LBL_GyoNm[Index].Text = "";
                            TXT_Mukesaki[Index].Enabled = true;
                            TXT_Suryo[Index].Enabled = true;
                            TXT_Suibun[Index].Enabled = true;
                            break;
                        case "3":
                            TXT_HinCd[Index].Enabled = true;
                            TXT_GyoCd[Index].Enabled = true;
                            TXT_Mukesaki[Index].Enabled = true;
                            TXT_Suryo[Index].Enabled = true;
                            TXT_Suibun[Index].Enabled = true;
                            break;

                        //  13.07.02    ISV-TRUC    品名ＣＤのLeaveイベントで、区分の入力値が、9（削除）以外、向先のEnabledへTrueをセット。                       
                        case "9":
                            TXT_Mukesaki[Index].Enabled = false;
                            TXT_Mukesaki[Index].Text = "";
                            break;
                        // End 13.07.02    ISV-TRUC
                    }
                    if (Cksi0010.Hinmoku_Kensaku(Strings.Mid(TXT_HinCd[Index].Text, 1, 4)) != 0)
                    {
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(Strings.Trim(Cksi0010.G_HINMOKU_AREA[1])))
                        {
                            TXT_Suibun[Index].Enabled = false;
                            TXT_Suibun[Index].Text = "";
                        }
                    }
                }
            }
            IsShowExistSizaiTanaorosiMstMsg = false;
			IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
		}

		private void TXT_MM_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
			if (Strings.Len(TXT_MM.Text) == 2)
			{
				TXT_DD.Focus();
			}
		}

		private void TXT_MM_Enter(System.Object eventSender, System.EventArgs eventArgs)
		{
			IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
		}

		private void TXT_MM_Leave(System.Object eventSender, System.EventArgs eventArgs)
		{
			TXT_MM.Text = C_COMMON.FormatToNum(TXT_MM.Text, "00");
			IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
		}

		private void TXT_Mukesaki_Enter(System.Object eventSender, System.EventArgs eventArgs)
		{
			short Index = TXT_Mukesaki.GetIndex((TextBox)eventSender);
			WK_Mukesaki = TXT_Mukesaki[Index].Text;
			IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
		}

		private void TXT_Mukesaki_Leave(System.Object eventSender, System.EventArgs eventArgs)
		{
			short Index = TXT_Mukesaki.GetIndex((TextBox)eventSender);
			if (WK_Mukesaki != TXT_Mukesaki[Index].Text)
			{
				if (IPPAN.C_Allspace(TXT_Mukesaki[Index].Text) != 0)
				{
					if (IPPAN.Input_Check(TXT_Mukesaki[Index].Text) != 0)
					{
						//フォーカスセット
						TXT_Mukesaki[Index].Focus();
						TXT_Mukesaki_Enter(TXT_Mukesaki[Index], new System.EventArgs());
						//入力エラー
						IPPAN.Error_Msg("E202", 0, " ");
						return;
					}
					IPPAN.G_RET = IPPAN.Numeric_Check2(TXT_Mukesaki[Index].Text);
					if (IPPAN.G_RET == 1)
					{
						TXT_Mukesaki[Index].Focus();
						TXT_Mukesaki_Enter(TXT_Mukesaki[Index], new System.EventArgs());
						//入力エラー
						IPPAN.Error_Msg("E202", 0, " ");
						IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
						return;
					}
					switch (TXT_Mukesaki[Index].Text)
					{
						case "1":
						case "2":
						case "3":
						case "4":
						case "5":
						case "6":
						case "7":
						case "8":
							break;
						default:
							//フォーカスセット
							TXT_Mukesaki[Index].Focus();
							TXT_Mukesaki_Enter(TXT_Mukesaki[Index], new System.EventArgs());
							//入力エラー
							IPPAN.Error_Msg("E202", 0, " ");
							return;
					}
				}
				else
				{
					//フォーカスセット
					TXT_Mukesaki[Index].Focus();
					TXT_Mukesaki_Enter(TXT_Mukesaki[Index], new System.EventArgs());
					//必須項目入力エラー
					IPPAN.Error_Msg("E200", 0, "");
					return;
				}
			}
			IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
		}

		private void TXT_Suibun_Enter(System.Object eventSender, System.EventArgs eventArgs)
		{
			short Index = TXT_Suibun.GetIndex((TextBox)eventSender);

			IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
			if (IPPAN.C_Allspace(TXT_Suibun[Index].Text) == 0)
			{
				TXT_Suibun[Index].Text = "";
			}
			else
			{
				TXT_Suibun[Index].Text = "";
				if (Conversion.Val(Cksi0010.G_Suibun[Index]) != 0)
				{
					TXT_Suibun[Index].Text = Strings.Trim(Cksi0010.G_Suibun[Index]);
				}
			}
			WK_Suibun = Strings.Trim(TXT_Suibun[Index].Text);

		}

		private void TXT_Suibun_Leave(System.Object eventSender, System.EventArgs eventArgs)
		{
			short Index = TXT_Suibun.GetIndex((TextBox)eventSender);

			if (WK_Suibun != Strings.Trim(TXT_Suibun[Index].Text))
			{
				if (IPPAN.C_Allspace(TXT_Suibun[Index].Text) != 0)
				{
					if (IPPAN.Numeric_Check(TXT_Suibun[Index].Text) == 1)
					{
						TXT_Suibun[Index].Focus();
						TXT_Suibun_Enter(TXT_Suibun[Index], new System.EventArgs());
						//入力エラー
						IPPAN.Error_Msg("E202", 0, " ");
						return;
					}
					if (Conversion.Val(TXT_Suibun[Index].Text) >= 100)
					{
						TXT_Suibun[Index].Focus();
						TXT_Suibun_Enter(TXT_Suibun[Index], new System.EventArgs());
						//入力エラー
						IPPAN.Error_Msg("E202", 0, " ");
						return;
					}
					Cksi0010.G_Suibun[Index] = C_COMMON.FormatToNum(IPPAN2.Numeric_Hensyu4(Strings.Trim(TXT_Suibun[Index].Text)), "#0.00");
					if (Conversion.Val(Cksi0010.G_Suibun[Index]) < 0)
					{
						TXT_Suibun[Index].Focus();
						TXT_Suibun_Enter(TXT_Suibun[Index], new System.EventArgs());
						//入力エラー
						IPPAN.Error_Msg("E202", 0, " ");
						return;
					}
				}
				else
				{
					//フォーカスセット
					TXT_Suibun[Index].Focus();
					TXT_Suibun_Enter(TXT_Suibun[Index], new System.EventArgs());
					//必須項目入力エラー
					IPPAN.Error_Msg("E200", 0, "");
					return;
				}
			}
			TXT_Suibun[Index].Text = IPPAN.Money_Hensyu(Strings.Trim(TXT_Suibun[Index].Text), "#0.00");
			IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
		}

		private void TXT_Suryo_Enter(System.Object eventSender, System.EventArgs eventArgs)
		{
			short Index = TXT_Suryo.GetIndex((TextBox)eventSender);

			IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
			if (IPPAN.C_Allspace(TXT_Suryo[Index].Text) == 0)
			{
				TXT_Suryo[Index].Text = "";
			}
			else
			{
				TXT_Suryo[Index].Text = "";
				if (Conversion.Val(Cksi0010.G_Suryo[Index]) != 0)
				{
					TXT_Suryo[Index].Text = Strings.Trim(Cksi0010.G_Suryo[Index]);
				}
			}
			WK_Suryo = Strings.Trim(TXT_Suryo[Index].Text);
		}

		private void TXT_Suryo_Leave(System.Object eventSender, System.EventArgs eventArgs)
		{
			short Index = TXT_Suryo.GetIndex((TextBox)eventSender);
			if (WK_Suryo != Strings.Trim(TXT_Suryo[Index].Text))
			{
				if (IPPAN.C_Allspace(TXT_Suryo[Index].Text) != 0)
				{
					if (IPPAN2.Numeric_Hensyu3(TXT_Suryo[Index].Text) == "0")
					{
						TXT_Suryo[Index].Text = "";
						TXT_Suryo[Index].Focus();
						TXT_Suryo_Enter(TXT_Suryo[Index], new System.EventArgs());
						//入力エラー
						IPPAN.Error_Msg("E202", 0, " ");
						return;
					}
					if (Conversion.Val(TXT_Suryo[Index].Text) >= 1000000000)
					{
						TXT_Suryo[Index].Text = "";
						TXT_Suryo[Index].Focus();
						TXT_Suryo_Enter(TXT_Suryo[Index], new System.EventArgs());
						//入力エラー
						IPPAN.Error_Msg("E202", 0, " ");
						return;
					}
					Cksi0010.G_Suryo[Index] = C_COMMON.FormatToNum(IPPAN2.Numeric_Hensyu3(Strings.Trim(TXT_Suryo[Index].Text)), "########0");
					if (Conversion.Val(Cksi0010.G_Suryo[Index]) < 0)
					{
						TXT_Suryo[Index].Focus();
						TXT_Suryo_Enter(TXT_Suryo[Index], new System.EventArgs());
						//入力エラー
						IPPAN.Error_Msg("E202", 0, " ");
						return;
					}
				}
				else
				{
					//フォーカスセット
					TXT_Suryo[Index].Focus();
					TXT_Suryo_Enter(TXT_Suryo[Index], new System.EventArgs());
					//必須項目入力エラー
					IPPAN.Error_Msg("E200", 0, "");
					return;
				}
			}
			TXT_Suryo[Index].Text = IPPAN.Money_Hensyu(Strings.Trim(IPPAN2.Numeric_Hensyu3(TXT_Suryo[Index].Text)), "###,###,##0");
			IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
		}

		private void TXT_YY_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
			//13.07.02    ISV-TRUC    品名ＣＤのLeaveイベントで、区分の入力値が、9（削除）以外、向先のEnabledへTrueをセット。
			//if (Strings.Len(TXT_YY.Text) == 2)
			if (Strings.Len(TXT_YY.Text) == 4)
			{
				TXT_MM.Focus();
			}
		}

		private void TXT_YY_Enter(System.Object eventSender, System.EventArgs eventArgs)
		{
			IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
		}

		private void TXT_YY_Leave(System.Object eventSender, System.EventArgs eventArgs)
		{
			//  13.07.02    ISV-TRUC    作業日（年）の入力内容が、２桁の数値文字列だった場合は、先頭に "20"を付加する。
			//TXT_YY.Text = C_COMMON.FormatToNum(TXT_YY.Text, "00");  
			TXT_YY.Text = C_COMMON.DateYYChanged(TXT_YY.Text);
			// End 13.07.02    ISV-TRUC
			IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
		}

// 11/21 DSK tamai 副資材棚卸システム再構築 -start
        /// <summary>
        /// 副資材入出庫表のデータを読込む
        /// </summary>
        /// <param name="category">カテゴリ</param>
        /// <returns>引数で指定したカテゴリのデータ</returns>
        private IList<SizaiInOutItem> ReadSizaiInOutData(SIZAI_IN_OUT_CATEGORY category)
		{
            bool isTargetKbn = false;
            
            Excel.Application excelApp = new Excel.Application();

            // Excelの画面を非表示にする。
			excelApp.Visible = false;

            Excel.Workbook workBook = null;

            // 読込んだセルを保存するリスト
            IList<SizaiInOutItem> cell = new List<SizaiInOutItem>();

            try
            {
                workBook
                    = excelApp.Workbooks.Open(G_KENSIN_DATA_FILE_PATH,
                        Type.Missing, // ファイル内のリンクの更新方法を指定(引数省略可能)
                        Type.Missing, // ブックを読み取り専用モードで開くには、True を指定(引数省略可能)
                        Type.Missing, // Excelがテキストファイルを開くときに、項目の区切り文字を指定(引数省略可能)
                        Type.Missing, // パスワードで保護されたブックを開くのに必要なパスワード(省略可能)
                        Type.Missing, // 書き込み保護されたブックに書き込む場合に必要なパスワード(省略可能)
                        Type.Missing, /* [読み取り専用を推奨する] チェックボックスをオンにして保存されたブックを開くときでも、
								      読み取り専用を推奨するメッセージを非表示にする場合はTrue を指定(省略可能)*/
                        Type.Missing, // 指定したファイルがテキスト ファイルのときに、それがどのような形式のテキスト ファイルかを指定(省略可能)
                        Type.Missing, // 開こうとしているファイルがテキストファイルで引数Formatが6の場合の区切り文字を指定(省略可能)
                        Type.Missing, // 開こうとしているファイルがExcel4.0のアドインの場合、こん(省略可能)
                        Type.Missing, // ファイルが読み取り/書き込みモードで開けない場合に、ファイル通知リストに追加する場合はTrueを指定(省略可能) 
                        Type.Missing, // ファイルを開くとき最初に使用するファイルコンバーターのインデックス番号を指定(省略可能)
                        Type.Missing, // 最近使用したファイルの一覧にブックを追加する場合はTrueを指定(省略可能)
                        Type.Missing, // Excelの言語設定に合わせてファイルを保存する場合はTrueを指定(省略可能)
                        Type.Missing  // 使用できる定数はxlNormalLoad,xlRepairFile,xlExtraDataのいずれか(省略可能)
                        );

// 2017.05.09 yoshitake 副資材棚卸システム不具合修正 start.
                XMLOperator xmlOperator = XMLOperator.GetInstance();
                SIZAI_IN_OUT_LIST_SHEET_NAME = xmlOperator.GetSizaiInOutSheetName();
                // 検針データ.xlsの副資材入出庫表のシートを選択
                Excel.Worksheet sheet = SearchWorkSheet(workBook, SIZAI_IN_OUT_LIST_SHEET_NAME);
                //Excel.Worksheet sheet = (Excel.Worksheet)workBook.Sheets[SIZAI_IN_OUT_LIST_SHEET];
// 2017.05.09 yoshitake 副資材棚卸システム不具合修正 end.
                // セルまたはセル範囲を選択する(引数省略可能)
                sheet.Select(Type.Missing);

                // データの読み出し開始位置を設定する
                int startRow = EXCEL_START_ROW;
                int startCol = EXCEL_START_COL;
                int endCol = EXCEL_END_COL;

                // データがある最終行を取得する
                int lastRow = sheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing).Row;

                // 開始行から最終行までExcelを読み込む
                for (int i = startRow; i < lastRow; i++)
                {
                    // 副資材入出庫表の項目
                    SizaiInOutItem sizaiInOutItem = new SizaiInOutItem();

                    // データ取得開始列からデータ取得終了列まで読み込む。
                    for (int j = startCol; j < endCol; j++)
                    {
                        // セルの値を取得
                        Excel.Range range = (Excel.Range)sheet.Cells[i, j];

                        // 読み込むかどうか判定するために、区分列のセルを取得
                        Excel.Range rangeKbn = (Excel.Range)sheet.Cells[i, startCol - 1];

                        if (category == SIZAI_IN_OUT_CATEGORY.IN)
                        {
                            // 区分列の値がカテゴリAとカテゴリCの場合、
                            // リストに格納する。
                            if ((rangeKbn.Text.Equals(KBN_CATEGORY_A)) 
                                    || (rangeKbn.Text.Equals(KBN_CATEGORY_C)))
                            {
                                isTargetKbn = true;

                                switch(j)
                                {
                                    case (int)EXCEL_COL_DEFINE.HINMOKU_CODE:
                                        // 区分
                                        sizaiInOutItem.Kbn = (string)rangeKbn.Text;
                                        // 品目CD
                                        sizaiInOutItem.HinmokuCode = (string)range.Text;
                                        break;
                                    case (int)EXCEL_COL_DEFINE.HINMOKU_NAME:
                                        // 品目名
                                        sizaiInOutItem.HinmokuName = (string)range.Text;
                                        break;
                                    case (int)EXCEL_COL_DEFINE.TANI:
                                        // 単位
                                        sizaiInOutItem.Tani = (string)range.Text;
                                        break;
                                    case (int)EXCEL_COL_DEFINE.GYOSYA_CODE:
                                        // 業者CD
                                        sizaiInOutItem.GyosyaCode = (string)range.Text;
                                        break;
                                    case (int)EXCEL_COL_DEFINE.GYOSYA_NAME:
                                        // 業者名
                                        sizaiInOutItem.GyosyaName = (string)range.Text;
                                        break;
                                    case (int)EXCEL_COL_DEFINE.MUKESAKI:
                                        // 向先
                                        sizaiInOutItem.Mukesaki = (string)range.Text;
                                        break;
                                    case (int)EXCEL_COL_DEFINE.SURYO:
                                        // 数量
                                        sizaiInOutItem.Suryou = (string)range.Text;
                                        break;
                                    case (int)EXCEL_COL_DEFINE.SUIBUNHIKI:
                                        // 水分引
                                        sizaiInOutItem.Suibunhiki = (string)range.Text;
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                        else if (category == SIZAI_IN_OUT_CATEGORY.OUT)
                        {
                            // 区分列の値がカテゴリBの場合、リストに格納する。
                            if ((rangeKbn.Text.Equals(KBN_CATEGORY_B)))
                            {
                                isTargetKbn = true;
                                
                                switch (j)
                                {
                                    case (int)EXCEL_COL_DEFINE.HINMOKU_CODE:
                                        // 区分
                                        sizaiInOutItem.Kbn = (string)rangeKbn.Text;
                                        // 品目CD
                                        sizaiInOutItem.HinmokuCode = (string)range.Text;
                                        break;
                                    case (int)EXCEL_COL_DEFINE.HINMOKU_NAME:
                                        // 品目名
                                        sizaiInOutItem.HinmokuName = (string)range.Text;
                                        break;
                                    case (int)EXCEL_COL_DEFINE.TANI:
                                        // 単位
                                        sizaiInOutItem.Tani = (string)range.Text;
                                        break;
                                    case (int)EXCEL_COL_DEFINE.GYOSYA_CODE:
                                        // 業者CD
                                        sizaiInOutItem.GyosyaCode = (string)range.Text;
                                        break;
                                    case (int)EXCEL_COL_DEFINE.GYOSYA_NAME:
                                        // 業者名
                                        sizaiInOutItem.GyosyaName = (string)range.Text;
                                        break;
                                    case (int)EXCEL_COL_DEFINE.MUKESAKI:
                                        // 向先
                                        sizaiInOutItem.Mukesaki = (string)range.Text;
                                        break;
                                    case (int)EXCEL_COL_DEFINE.SURYO:
                                        // 数量
                                        sizaiInOutItem.Suryou = (string)range.Text;
                                        break;
                                    case (int)EXCEL_COL_DEFINE.SUIBUNHIKI:
                                        // 水分引
                                        sizaiInOutItem.Suibunhiki = (string)range.Text;
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }

                    if (isTargetKbn)
                    {
                        cell.Add(sizaiInOutItem);
                        isTargetKbn = false;
                    }
                }
            }

            // Equalsメソッドで例外が発生した場合
            catch(NullReferenceException ex)
            {
                C_COMMON.Msg(ex.Message);
                Close();
            }

            // Addメソッドで例外が発生した場合
            catch(NotSupportedException ex)
            {
                C_COMMON.Msg(ex.Message);
                Close();
            }

            // その他例外が発生した場合
            catch(Exception ex)
            {
                C_COMMON.Msg(ex.Message);
                Close();
            }

            finally
            {
                workBook.Close(false, // ブックに変更があり、開いている他のウィンドウにブックが表示されていない場合、変更を保存するかどうかを指定
                               sizaiInOutFilePath, // 変更後のブックのファイル名
                               false  // ブックを回覧するかどうか
                            );

                // Excelアプリケーション自体を閉じる
                excelApp.Quit();
			}

            // 取得した値を返す
            return cell;
		}

        private void GoNextLogic()
        {
            //ライン以下の入力内容をチェック
            if (Gamen_Check() != 0)
            {
                return;
            }
            if (G_flg1 == "0")
            {
                //画面に入力された内容をワークに送る
                Work_Set();
            }
            Cksi0010.G_Page = Convert.ToString(Convert.ToInt32(Cksi0010.G_Page) + 1);
            Enabled_False2();
            if (Convert.ToDouble(G_flg) == 0)
            {
                if (Convert.ToInt32(Cksi0010.G_Page) > Cksi0010.G_Page1)
                {
                    if (G_flg1 == "1")
                    {
                        //データがなかったときは何も表示しない
                        Gamen_Clear1();
                    }
                    else
                    {
                        //画面にワークの内容をセットする
                        Gamen_Set1();
                    }
                    if (Convert.ToInt32(Cksi0010.G_Page) > Cksi0010.G_Page1 + 1)
                    {
                        Cksi0010.G_Page1 = Cksi0010.G_Page1 + 1;
                    }
                    LBL_Page.Text = C_COMMON.FormatToNum(Cksi0010.G_Page, "00") + "/" + String.Format("{0:00}", Cksi0010.G_Page1 + 1);
                    G_flg2 = Convert.ToString(1);
                }
                else
                {
                    Gamen_Set();
                    //画面にデータベースの内容をセットする
                    if (G_flg2 != "1")
                    {
                        LBL_Page.Text = C_COMMON.FormatToNum(Cksi0010.G_Page, "00") + "/" + String.Format("{0:00}", Cksi0010.G_Page1);
                    }
                    else
                    {
                        LBL_Page.Text = C_COMMON.FormatToNum(Cksi0010.G_Page, "00") + "/" + String.Format("{0:00}", Cksi0010.G_Page1 + 1);
                    }
                }
            }
            else
            {
                if (G_flg1 == "1")
                {
                    //データがなかったときは何も表示しない
                    Gamen_Clear1();
                }
                else
                {
                    //画面にワークの内容をセットする
                    Gamen_Set1();
                }
                if (Convert.ToInt32(Cksi0010.G_Page) > Cksi0010.G_Page1 + 1)
                {
                    Cksi0010.G_Page1 = Cksi0010.G_Page1 + 1;
                }
                LBL_Page.Text = C_COMMON.FormatToNum(Cksi0010.G_Page, "00") + "/" + String.Format("{0:00}", Cksi0010.G_Page1 + 1);
            }

            //１０ページ目のときは次へボタンを使用不可にする
            if (Convert.ToInt32(Cksi0010.G_Page) == 10)
            {
                CMD_Button[3].Enabled = false;
            }
// 2017.04.28 yoshitake 副資材棚卸システム再構築 追加対応 start.
            if (TXT_kbn[0].Enabled)
            {
                TXT_kbn[0].Focus();
                IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
            }
            //TXT_kbn[0].Focus();
            //IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
// 2017.04.28 yoshitake 副資材棚卸システム再構築 追加対応 end.
        }
//12/12 DSK yoshitake 副資材棚卸システム再構築 -start
        /// <summary>
        /// 月末の年月日を取得する
        /// </summary>
        /// <returns>月末の年月日</returns>
        private string GetYearMonthDay(string yearMonth)
        {
            string thisMonth = yearMonth.Substring(0, 4) + "/" + yearMonth.Substring(4, 2);
            DateTime date = DateTime.Parse(Convert.ToString(thisMonth));
            int lastDay = DateTime.DaysInMonth(date.Year, date.Month);
            string thisLastDay = Convert.ToString(lastDay);

            return yearMonth + thisLastDay;
        }
//12/12 DSK yoshitake 副資材棚卸システム再構築 -end

        /// <summary>
        /// 指定したWorkSheetの検索
        /// </summary>
        /// <param name="workbook">WorkBook</param>
        /// <param name="sheetname">シート名</param>
        /// <returns>WorkSheet</returns>
        private static Excel.Worksheet SearchWorkSheet(Excel.Workbook workbook, string sheetname)
        {
            try
            {
                foreach (Excel.Worksheet sheet in workbook.Sheets)
                {
                    if (sheet.Name == sheetname)
                    {
                        return sheet;
                    }
                }

                return null;
            }

            catch (Exception)
            {
                throw;
            }
        }
	}
}

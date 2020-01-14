using Microsoft.VisualBasic;
using System;
using System.Drawing.Printing;
using System.Data;
using CKLib.Syohizei;
using CKLib.Syohizei.Types;

namespace Project1
{
    static class CKKR0010
    {
        //発行№
        public const short C_HKNO = 12;
        //経理年月日
        public const short C_KEIYMD = 6;
        //科目CD
        public const short C_KMCD = 3;
        //科目名
        public const short C_KMNM = 9;
        //金額
        public const short C_KIN = 15;
        //申請№
        public const short C_SINNO = 10;

        //フラグ１
        public const short C_FLG1 = 1;
        //フラグ０
        public const short C_FLG0 = 0;

        //再印刷
        public const short C_SAI = 0;
        //通常印刷
        public const short C_TUZYO = 1;

        //消費税データ存在フラグ
        public static bool G_SYOUHI;

        //リスト№
        public static string G_LISTNO;
        //件数
        public static string G_KENSU;
        //発行№(セーブ)
        public static string G_SV_HAKONO;
        //発行№(最新)
        public static string G_NW_HAKONO;

        //改頁用銀行コード（セーブ）
        public static string G_SV_GINKO;
        //改頁用銀行コード（最新）
        public static string G_NW_GINKO;

        //銀行別 ソート順チェック用
        public static bool G_GINKOCHK;

        //プリンタ集計用領域
        //通常の金額
        public static decimal G_KIN1;
        //消費税金額
        public static decimal G_KIN2;
        //合計金額
        public static decimal G_KIN3;
        //印字ページカウンタ
        public static decimal G_PAGE;
        //基準Ｘ座標
        public static float G_X;
        //現在の行数カウンタ
        public static int G_NW_LINE;
        //１ページ最大行数
        public static int G_GT_LINE;
        //小計金額
        public static decimal G_GINKEI;
        //総合計金額
        public static decimal G_GOKEI;

        //初回印刷フラグ
        public static Boolean G_SAISYO;
        //印刷正常・以上判定
        public static Boolean G_PRINT_CHECK;
        //印刷判定(0：再印刷 1：通常印刷)
        public static short G_PRINT_TYPE;
        //最終頁印刷判定
        public static Boolean G_PRINT_END;

// 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Add -start
        /// <summary>
        /// 消費税リゾルバー
        /// </summary>
        private static SyohizeiResolver syohizeiResolver = null;
// 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Add -end

        //プリンタ印字用領域
        public struct PR_ITEM_SET
        {
            //(1ﾊﾞｲﾄ)リスト番号
            public string LISTNO;
            //(2ﾊﾞｲﾄ)再出力
            public string SAI;
            //(1ﾊﾞｲﾄ)作成日時
            public string SAKUYMD;
            //(1ﾊﾞｲﾄ)ページ
            public string PG;
            //(1ﾊﾞｲﾄ)経理日
            public string KEIYMD;
            //(1ﾊﾞｲﾄ)伝票№1
            public string DENNO;
            //(1ﾊﾞｲﾄ)伝票№2
            public string DENNO2;
            //(1ﾊﾞｲﾄ)証憑№
            public string SYONO;
            //(1ﾊﾞｲﾄ)税区分1
            public string KU;
            //(1ﾊﾞｲﾄ)税区分2
            public string KU2;
            //(1ﾊﾞｲﾄ)科目コード(借)
            public string KRKAMO;
            //(1ﾊﾞｲﾄ)内容(借)
            public string KRNAIYO;
            //(2ﾊﾞｲﾄ)科目名(借)
            public string KRKAMONM;
            //(1ﾊﾞｲﾄ)科目コード(貸)
            public string KSKAMO;
            //(1ﾊﾞｲﾄ)内容(貸)
            public string KSNAIYO;
            //(2ﾊﾞｲﾄ)科目名(貸)
            public string KSKAMONM;
            //(1ﾊﾞｲﾄ)金額　仕様では金額(借)
            public string KIN;
            //(1ﾊﾞｲﾄ)科目コード(消費・借)
            public string SRKAMO;
            //(1ﾊﾞｲﾄ)内容(消費・借)
            public string SRNAIYO;
            //(2ﾊﾞｲﾄ)科目名(消費・借)
            public string SRKAMONM;
            //(1ﾊﾞｲﾄ)科目コード(消費・貸)
            public string SSKAMO;
            //(1ﾊﾞｲﾄ)内容(消費・貸)
            public string SSNAIYO;
            //(2ﾊﾞｲﾄ)科目名(消費・貸)
            public string SSKAMONM;
            //(1ﾊﾞｲﾄ)金額(消費)
            public string SKIN;
            //(1ﾊﾞｲﾄ)金額(合計)
            public string GKIN;
            //(2ﾊﾞｲﾄ)摘要
            public string TEKIYO;
            //(2ﾊﾞｲﾄ)摘要２
            public string TEKIYO2;
            //(1ﾊﾞｲﾄ)業者CD(月末)
            public string GEGYOUCD;
            //(1ﾊﾞｲﾄ)業者名(月末)
            public string GEGYOUNM;
            //(1ﾊﾞｲﾄ)銀行名(月末)
            public string GEGINKONM;
            //(1ﾊﾞｲﾄ)支店名(月末)
            public string GESITENNM;
            //(1ﾊﾞｲﾄ)口座番号(月末)
            public string GEKOUZA;
            //(1ﾊﾞｲﾄ)サイト(月末)
            public string SAITO;
            //(1ﾊﾞｲﾄ)手形№(月末)
            public string TEGNO;
            //(1ﾊﾞｲﾄ)申請№(月末)
            public string SINNO;
            //(2ﾊﾞｲﾄ)品名(購買)
            public string HINMEI;
            //9(9)V999 (1ﾊﾞｲﾄ)数量(購買)
            public string SURYO;
            //(2ﾊﾞｲﾄ)単位(購買)
            public string TANI;
            //9(8)V99 (1ﾊﾞｲﾄ)単価(購買)
            public string TANKA;
            //(1ﾊﾞｲﾄ)データ区分
            public string DK;
            //(1ﾊﾞｲﾄ)条件ＣＤ
            public string JOKENCD;
            //(2ﾊﾞｲﾄ)条件名
            public string JOKENNM;
            //(2ﾊﾞｲﾄ)借方１段目
            public string KARI1;
            //(2ﾊﾞｲﾄ)貸方１段目
            public string KASI1;
            //(2ﾊﾞｲﾄ)借方２段目
            public string KARI2;
            //(2ﾊﾞｲﾄ)貸方２段目
            public string KASI2;
            //(1ﾊﾞｲﾄ)借方３段目
            public string KARI3;
            //(1ﾊﾞｲﾄ)貸方３段目
            public string KASI3;
            //(1ﾊﾞｲﾄ)借方１段目ＣＤ
            public string KARICD1;
            //(1ﾊﾞｲﾄ)貸方１段目ＣＤ
            public string KASICD1;
            //(1ﾊﾞｲﾄ)借方２段目ＣＤ
            public string KARICD2;
            //(1ﾊﾞｲﾄ)貸方２段目ＣＤ
            public string KASICD2;
            //(1ﾊﾞｲﾄ)借方３段目ＣＤ→借方期日
            public string KARICD3;
            //(1ﾊﾞｲﾄ)貸方３段目ＣＤ→貸方期日
            public string KASICD3;
            //(1ﾊﾞｲﾄ)預金種別
            public string YSYUBETU;
            //(2ﾊﾞｲﾄ)預金内訳
            public string YUTI;
            //(1ﾊﾞｲﾄ)税率区分
            public string ZEIRITU;
// 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Add -start
            //税率補助区分
            public string ZEIRITU_HOJO_KBN;
// 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Add -end
            //(1ﾊﾞｲﾄ)借方貸借区分
            public string KRACKBN;
            //(1ﾊﾞｲﾄ)貸方貸借区分
            public string KSACKBN;
            //(1ﾊﾞｲﾄ)区切用線
            public string SEN;
            //(1ﾊﾞｲﾄ)予算№
            public string YOSANNO;
            //(1ﾊﾞｲﾄ)借方業者コード
            public string KRGYOSYACD;
            //(2ﾊﾞｲﾄ)借方業者名
            public string KRGYOSYANM;
            //(2ﾊﾞｲﾄ)承認者
            public string SYONM5;
            //(2ﾊﾞｲﾄ)承認者
            public string SYONM6;
            //(2ﾊﾞｲﾄ)承認者
            public string SYONM7;
            //(1ﾊﾞｲﾄ)予備フラグ１
            public string YBFLG1;
            //(1ﾊﾞｲﾄ)予備フラグ２
            public string YBFLG2;
            //(1ﾊﾞｲﾄ)予備フラグ３
            public string YBFLG3;
        }
        public static PR_ITEM_SET G_PR;

        public static int GetPrintRecord(DataTable tbl, int nCnt)
        {
            //データがないときは１を返す。
            int nRet = 1;

            //結果の列数を取得する
            if (tbl.Columns.Count < 1)
            {
                return nRet;
            }

            //データがない
            if (tbl.Rows.Count == 0)
            {
                //経理仕訳申請トランが見つかりません
                IPPAN.Error_Msg("E011", 0, "");
                return nRet;
            }

            //上限に達した
            if (tbl.Rows.Count <= nCnt)
            {
                return nRet;
            }

            for (int j = 0; j < tbl.Columns.Count; j++)
            {
                //項目セット
                //ヌル対応
                if (tbl.Rows[nCnt][j] == null)
                {
                    //項目セット
                    GYOMU.G_TRAN_AREA[j] = Strings.Space(1);
                }
                else
                {
                    //項目セット
                    GYOMU.G_TRAN_AREA[j] = tbl.Rows[nCnt][j].ToString();
                }
            }

            nRet = 0;
            return nRet;
        }

        public static void CLR_G_PR()
        {
            //リスト番号
            G_PR.LISTNO = Strings.Space(12);
            //再出力
            G_PR.SAI = Strings.StrConv(Strings.Space(3), VbStrConv.Wide, C_COMMON.G_CMN_LOCALID_JP);
            //作成日
            G_PR.SAKUYMD = Strings.Space(17);
            //ページ
            G_PR.PG = Strings.Space(6);
            //経理日
            G_PR.KEIYMD = Strings.Space(8);
            //伝票№1
            G_PR.DENNO = Strings.Space(4);
            //伝票№2
            G_PR.DENNO2 = Strings.Space(4);
            //証憑№
            G_PR.SYONO = Strings.Space(6);
            //税区分１
            G_PR.KU = Strings.Space(2);
            //税区分２
            G_PR.KU2 = Strings.Space(2);
            //科目コード(借)
            G_PR.KRKAMO = Strings.Space(3);
            //内容(借)
            G_PR.KRNAIYO = Strings.Space(10);
            //科目名(借)
            G_PR.KRKAMONM = Strings.StrConv(Strings.Space(14), VbStrConv.Wide, C_COMMON.G_CMN_LOCALID_JP);
            //科目コード(貸)
            G_PR.KSKAMO = Strings.Space(3);
            //内容(貸)
            G_PR.KSNAIYO = Strings.Space(10);
            //科目名(貸)
            G_PR.KSKAMONM = Strings.StrConv(Strings.Space(14), VbStrConv.Wide, C_COMMON.G_CMN_LOCALID_JP);
            //金額　仕様では金額(借)
            G_PR.KIN = Strings.Space(15);
            //科目コード(消費・借)
            G_PR.SRKAMO = Strings.Space(3);
            //内容(消費・借)
            G_PR.SRNAIYO = Strings.Space(10);
            //科目名(消費・借)
            G_PR.SRKAMONM = Strings.StrConv(Strings.Space(14), VbStrConv.Wide, C_COMMON.G_CMN_LOCALID_JP);
            //科目コード(消費・貸)
            G_PR.SSKAMO = Strings.Space(3);
            //内容(消費・貸)
            G_PR.SSNAIYO = Strings.Space(10);
            //科目名(消費・貸)
            G_PR.SSKAMONM = Strings.StrConv(Strings.Space(14), VbStrConv.Wide, C_COMMON.G_CMN_LOCALID_JP);
            //金額(消費)
            G_PR.SKIN = Strings.Space(15);
            //金額(合計)
            G_PR.GKIN = Strings.Space(15);
            //摘要
            G_PR.TEKIYO = Strings.StrConv(Strings.Space(30), VbStrConv.Wide, C_COMMON.G_CMN_LOCALID_JP);
            //摘要2
            G_PR.TEKIYO2 = Strings.StrConv(Strings.Space(60), VbStrConv.Wide, C_COMMON.G_CMN_LOCALID_JP);
            //業者CD(月末)
            G_PR.GEGYOUCD = Strings.Space(4);
            //業者名(月末)
            G_PR.GEGYOUNM = Strings.Space(8);
            //銀行名(月末)
            G_PR.GEGINKONM = Strings.Space(10);
            //支店名(月末)
            G_PR.GESITENNM = Strings.Space(10);
            //口座番号(月末)
            G_PR.GEKOUZA = Strings.Space(9);
            //サイト(月末)
            G_PR.SAITO = Strings.Space(3);
            //手形№(月末)
            G_PR.TEGNO = Strings.Space(10);
            //申請№(月末)
            G_PR.SINNO = Strings.Space(15);
            //品名(購買)
            G_PR.HINMEI = Strings.StrConv(Strings.Space(30), VbStrConv.Wide, C_COMMON.G_CMN_LOCALID_JP);
            //数量(購買)
            G_PR.SURYO = Strings.Space(15);
            //単位(購買)
            G_PR.TANI = Strings.StrConv(Strings.Space(1), VbStrConv.Wide, C_COMMON.G_CMN_LOCALID_JP);
            //単価(購買)
            G_PR.TANKA = Strings.Space(14);
            //(1ﾊﾞｲﾄ)データ区分
            G_PR.DK = Strings.Space(1);
            //(1ﾊﾞｲﾄ)条件ＣＤ
            G_PR.JOKENCD = Strings.Space(2);
            //(2ﾊﾞｲﾄ)条件名
            G_PR.JOKENNM = Strings.StrConv(Strings.Space(23), VbStrConv.Wide, C_COMMON.G_CMN_LOCALID_JP);
            //(2ﾊﾞｲﾄ)借方１段目
            G_PR.KARI1 = Strings.StrConv(Strings.Space(10), VbStrConv.Wide, C_COMMON.G_CMN_LOCALID_JP);
            //(2ﾊﾞｲﾄ)貸方１段目
            G_PR.KASI1 = Strings.StrConv(Strings.Space(10), VbStrConv.Wide, C_COMMON.G_CMN_LOCALID_JP);
            //(2ﾊﾞｲﾄ)借方２段目
            G_PR.KARI2 = Strings.StrConv(Strings.Space(10), VbStrConv.Wide, C_COMMON.G_CMN_LOCALID_JP);
            //(2ﾊﾞｲﾄ)貸方２段目
            G_PR.KASI2 = Strings.StrConv(Strings.Space(10), VbStrConv.Wide, C_COMMON.G_CMN_LOCALID_JP);
            //(1ﾊﾞｲﾄ)借方３段目
            G_PR.KARI3 = Strings.Space(10);
            //(1ﾊﾞｲﾄ)貸方３段目
            G_PR.KASI3 = Strings.Space(10);
            //(1ﾊﾞｲﾄ)借方１段目ＣＤ
            G_PR.KARICD1 = Strings.Space(3);
            //(1ﾊﾞｲﾄ)貸方１段目ＣＤ
            G_PR.KASICD1 = Strings.Space(3);
            //(1ﾊﾞｲﾄ)借方２段目ＣＤ
            G_PR.KARICD2 = Strings.Space(3);
            //(1ﾊﾞｲﾄ)貸方２段目ＣＤ
            G_PR.KASICD2 = Strings.Space(3);
            //(1ﾊﾞｲﾄ)借方３段目ＣＤ→借方期日
            G_PR.KARICD3 = Strings.Space(3);
            //(1ﾊﾞｲﾄ)貸方３段目ＣＤ→貸方期日
            G_PR.KASICD3 = Strings.Space(3);
            //(1ﾊﾞｲﾄ)預金種別
            G_PR.YSYUBETU = Strings.Space(1);
            //(2ﾊﾞｲﾄ)預金内訳
            G_PR.YUTI = Strings.StrConv(Strings.Space(7), VbStrConv.Wide, C_COMMON.G_CMN_LOCALID_JP);
            //(1ﾊﾞｲﾄ)税率区分
            G_PR.ZEIRITU = Strings.Space(2);
// 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Add -start
            G_PR.ZEIRITU_HOJO_KBN = Strings.Space(2);
// 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Add -end
            //(1ﾊﾞｲﾄ)借方貸借区分
            G_PR.KRACKBN = Strings.Space(1);
            //(1ﾊﾞｲﾄ)貸方貸借区分
            G_PR.KSACKBN = Strings.Space(1);
            //線
            G_PR.SEN = new string('-', 152);
            //(1ﾊﾞｲﾄ)予算№
            G_PR.YOSANNO = Strings.Space(17);
            //(1ﾊﾞｲﾄ)借方業者コード
            G_PR.KRGYOSYACD = Strings.Space(4);
            //(1ﾊﾞｲﾄ)借方業者名
            G_PR.KRGYOSYANM = Strings.StrConv(Strings.Space(8), VbStrConv.Wide, C_COMMON.G_CMN_LOCALID_JP);
            //(2ﾊﾞｲﾄ)承認者
            G_PR.SYONM5 = Strings.StrConv(Strings.Space(10), VbStrConv.Wide, C_COMMON.G_CMN_LOCALID_JP);
            //(2ﾊﾞｲﾄ)承認者
            G_PR.SYONM6 = Strings.StrConv(Strings.Space(10), VbStrConv.Wide, C_COMMON.G_CMN_LOCALID_JP);
            //(2ﾊﾞｲﾄ)承認者
            G_PR.SYONM7 = Strings.StrConv(Strings.Space(10), VbStrConv.Wide, C_COMMON.G_CMN_LOCALID_JP);
            //(1ﾊﾞｲﾄ)予備フラグ１
            G_PR.YBFLG1 = Strings.Space(1);
            //(1ﾊﾞｲﾄ)予備フラグ２
            G_PR.YBFLG2 = Strings.Space(1);
            //(1ﾊﾞｲﾄ)予備フラグ３
            G_PR.YBFLG3 = Strings.Space(1);
        }

        //
        //   総合計印刷
        //
        public static void GOKEI_INSATU(C_PRINT pd, PrintPageEventArgs e)
        {
            string l_sgoukei = "";
            float fNW_Line = 0f;
            const int nLen = 15;

            fNW_Line = (float)(G_NW_LINE + 1);

            pd.printer_print(e, "合　計　金　額", (G_X + 40f), fNW_Line);
            //総合計
            l_sgoukei = Strings.Left(C_COMMON.MIGIDUME(Conversion.Str(G_GOKEI), "###,###,###,##0", nLen), nLen);
            //総合計
            pd.printer_print(e, l_sgoukei, (G_X + 80f), fNW_Line);
        }

        //
        //   小計印刷
        //
        public static void SYOKEI_INSATU(C_PRINT pd, PrintPageEventArgs e)
        {
            string l_SGINKEI = "";
            float fNW_Line = 0f;
            const int nLen = 15;

            fNW_Line = (float)(G_GT_LINE + 1);

            pd.printer_print(e, "小　計　金　額", (G_X + 40f), fNW_Line);
            //小計
            l_SGINKEI = Strings.Left(C_COMMON.MIGIDUME(Conversion.Str(G_GINKEI), "###,###,###,##0", nLen), nLen);
            //小計
            pd.printer_print(e, l_SGINKEI, (G_X + 80f), fNW_Line);
        }

        //
        //   変数項目印刷（明細部）
        //
        public static void HENSU_INSATU(C_PRINT pd, PrintPageEventArgs e)
        {
            float fNW_Line = 0f;

            fNW_Line = (float)(G_NW_LINE + 1);
            //申請№(月末)
            pd.printer_print(e, G_PR.SINNO, (G_X + 9f), fNW_Line);
            //伝票№
            pd.printer_print(e, G_PR.DENNO, (G_X + 56f), fNW_Line);
            //伝票№2
            pd.printer_print(e, G_PR.DENNO2, (G_X + 62f), fNW_Line);
            //預金内訳
            pd.printer_print(e, G_PR.YUTI, (G_X + 104f), fNW_Line);

            //明細４行目
            fNW_Line = (float)(G_NW_LINE + 4);
            //経理日
            pd.printer_print(e, G_PR.KEIYMD, (G_X + 1f), fNW_Line);
            //税区分
            pd.printer_print(e, G_PR.KU, (G_X + 11f), fNW_Line);
            //税区分２
            pd.printer_print(e, G_PR.KU2, (G_X + 14f), fNW_Line);
// 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Mod -start
            //税率区分 + 税率補助区分
            pd.printer_print(e, G_PR.ZEIRITU + G_PR.ZEIRITU_HOJO_KBN, (G_X + 21f), fNW_Line);
            ////税率区分
            //pd.printer_print(e, G_PR.ZEIRITU, (G_X + 21f), fNW_Line);
// 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Mod -end
            //科目コード(借)
            pd.printer_print(e, G_PR.KRKAMO, (G_X + 28f), fNW_Line);
            //科目ACKBN(借)
            pd.printer_print(e, G_PR.KRACKBN, (G_X + 32f), fNW_Line);
            //内容(借)
            pd.printer_print(e, G_PR.KRNAIYO, (G_X + 35f), fNW_Line);
            //科目名(借)
            pd.printer_print(e, G_PR.KRKAMONM, (G_X + 48f), fNW_Line);
            //科目コード(貸)
            pd.printer_print(e, G_PR.KSKAMO, (G_X + 88f), fNW_Line);
            //科目ACKBN(貸)
            pd.printer_print(e, G_PR.KSACKBN, (G_X + 92f), fNW_Line);
            //内容(貸)
            pd.printer_print(e, G_PR.KSNAIYO, (G_X + 95f), fNW_Line);
            //科目名(貸)
            pd.printer_print(e, G_PR.KSKAMONM, (G_X + 108f), fNW_Line);
            //金額
            pd.printer_print(e, G_PR.KIN, (G_X + 160f), fNW_Line);

            //明細５行目
            fNW_Line = (float)(G_NW_LINE + 5);
            //科目コード(消費・借)
            pd.printer_print(e, G_PR.SRKAMO, (G_X + 28f), fNW_Line);
            //内容(消費・借)
            pd.printer_print(e, G_PR.SRNAIYO, (G_X + 35f), fNW_Line);
            //科目名(消費・借)
            pd.printer_print(e, G_PR.SRKAMONM, (G_X + 48f), fNW_Line);
            //科目コード(消費・貸)
            pd.printer_print(e, G_PR.SSKAMO, (G_X + 88f), fNW_Line);
            //内容(消費・貸)
            pd.printer_print(e, G_PR.SSNAIYO, (G_X + 95f), fNW_Line);
            //科目名(消費・貸)
            pd.printer_print(e, G_PR.SSKAMONM, (G_X + 108f), fNW_Line);
            //金額(消費)
            pd.printer_print(e, G_PR.SKIN, (G_X + 160f), fNW_Line);

            //明細６行目
            fNW_Line = (float)(G_NW_LINE + 6);
            //科目コード(消費・借)
            pd.printer_print(e, G_PR.KARICD1, (G_X + 28f), fNW_Line);
            //内容(消費・借)
            pd.printer_print(e, G_PR.KARI1, (G_X + 35f), fNW_Line);
            //科目コード(消費・貸)
            pd.printer_print(e, G_PR.KASICD1, (G_X + 88f), fNW_Line);
            //内容(消費・貸)
            pd.printer_print(e, G_PR.KASI1, (G_X + 95f), fNW_Line);
            //金額(合計)
            pd.printer_print(e, G_PR.GKIN, (G_X + 160f), fNW_Line);

            //明細７行目
            fNW_Line = (float)(G_NW_LINE + 7);
            //科目コード(消費・借)
            pd.printer_print(e, G_PR.KARICD2, (G_X + 28f), fNW_Line);
            //内容(消費・借)
            pd.printer_print(e, G_PR.KARI2, (G_X + 35f), fNW_Line);
            //科目コード(消費・貸)
            pd.printer_print(e, G_PR.KASICD2, (G_X + 88f), fNW_Line);
            //内容(消費・貸)
            pd.printer_print(e, G_PR.KASI2, (G_X + 95f), fNW_Line);

            //明細８行目
            fNW_Line = (float)(G_NW_LINE + 8);
            //科目コード(消費・借)
            pd.printer_print(e, G_PR.KARICD3, (G_X + 28f), fNW_Line);
            //内容(消費・借)
            pd.printer_print(e, G_PR.KARI3, (G_X + 35f), fNW_Line);
            //科目コード(消費・貸)
            pd.printer_print(e, G_PR.KASICD3, (G_X + 88f), fNW_Line);
            //内容(消費・貸)
            pd.printer_print(e, G_PR.KASI3, (G_X + 95f), fNW_Line);

            //明細９行目
            fNW_Line = (float)(G_NW_LINE + 9);
            //品名(購買)
            pd.printer_print(e, G_PR.HINMEI, (G_X + 27f), fNW_Line);
            //数量(購買)
            pd.printer_print(e, G_PR.SURYO, (G_X + 26f + 85f), fNW_Line);
            //単位(購買)
            pd.printer_print(e, G_PR.TANI, (G_X + 26f + 75f), fNW_Line);
            //単価(購買)
            pd.printer_print(e, G_PR.TANKA, (G_X + 26f + 110f), fNW_Line);

            //明細１０行目
            fNW_Line = (float)(G_NW_LINE + 10);
            //業者CD(月末)
            pd.printer_print(e, G_PR.GEGYOUCD, (G_X + 23f), fNW_Line);
            //業者名(月末)
            pd.printer_print(e, G_PR.GEGYOUNM, (G_X + 23f + 14f), fNW_Line);
            //銀行名(月末)
            pd.printer_print(e, G_PR.GEGINKONM, (G_X + 60f + 5f), fNW_Line);
            //支店名(月末)
            pd.printer_print(e, G_PR.GESITENNM, (G_X + 90f), fNW_Line);
            //預金種別(月末)
            pd.printer_print(e, G_PR.YSYUBETU, (G_X + 119f), fNW_Line);
            //口座番号(月末)
            pd.printer_print(e, G_PR.GEKOUZA, (G_X + 121f), fNW_Line);
            //サイト(月末)
            pd.printer_print(e, G_PR.SAITO, (G_X + 140f), fNW_Line);
            //手形№(月末)
            pd.printer_print(e, G_PR.TEGNO, (G_X + 164f), fNW_Line);

            //明細１１行目
            fNW_Line = (float)(G_NW_LINE + 11);
            //条件ＣＤ
            pd.printer_print(e, G_PR.JOKENCD, (G_X + 23f), fNW_Line);
            //条件名
            pd.printer_print(e, G_PR.JOKENNM, (G_X + 36f), fNW_Line);
            //借方業者CD
            pd.printer_print(e, G_PR.KRGYOSYACD, (G_X + 83f + 23f), fNW_Line);
            //借方業者名
            pd.printer_print(e, G_PR.KRGYOSYANM, (G_X + 83f + 23f + 14f), fNW_Line);

            //明細１２行目
            fNW_Line = (float)(G_NW_LINE + 12);
            //摘要
            pd.printer_print(e, G_PR.TEKIYO, (G_X + 10f), fNW_Line);
            //摘要
            pd.printer_print(e, Strings.Mid(G_PR.TEKIYO2, 1, 30), (G_X + 80f), fNW_Line);
            //予算№
            pd.printer_print(e, G_PR.YOSANNO, (G_X - 3f + 160f), fNW_Line);

            //明細１３行目
            fNW_Line = (float)(G_NW_LINE + 13);
            //摘要
            pd.printer_print(e, Strings.Mid(G_PR.TEKIYO2, 31, 60), (G_X + 4f), fNW_Line);
            //承認者
            pd.printer_print(e, G_PR.SYONM5, (G_X + 77f + 1f), fNW_Line);
            //承認者
            pd.printer_print(e, G_PR.SYONM6, (G_X + 100f + 1f), fNW_Line);
            //承認者
            pd.printer_print(e, G_PR.SYONM7, (G_X + 123f + 1f), fNW_Line);
            //証憑№
            pd.printer_print(e, Strings.Mid(G_PR.SYONO, 1, 2) + "-" + Strings.Mid(G_PR.SYONO, 3, 4), (G_X - 3f + 160f), fNW_Line);

            //明細１４行目
            fNW_Line = (float)(G_NW_LINE + 14);
            //摘要
            pd.printer_print(e, G_PR.SEN, (G_X + 1f), fNW_Line);
        }

        //
        //   固定項目印刷（明細部）
        //
        public static void KOTEI_INSATU(C_PRINT pd, PrintPageEventArgs e)
        {
            float fNW_Line = 0f;

            //見出し１行目
            fNW_Line = (float)(G_NW_LINE + 1);
            pd.printer_print(e, "申請№", (G_X + 1f), fNW_Line);
            pd.printer_print(e, "発行伝票№", (G_X + 43f), fNW_Line);
            pd.printer_print(e, "預金内訳", (G_X + 93f), fNW_Line);

            //見出し２行目
            fNW_Line = (float)(G_NW_LINE + 2);
            pd.printer_print(e, "借方", (G_X + 28f), fNW_Line);
            pd.printer_print(e, "貸方", (G_X + 88f), fNW_Line);
            
            //見出し３行目
            fNW_Line = (float)(G_NW_LINE + 3);
            //ページ
            pd.printer_print(e, "経理日", (G_X + 1f), fNW_Line);
            //ページ
            pd.printer_print(e, "税区分", (G_X + 11f), fNW_Line);
            //ページ
            pd.printer_print(e, "税率区分", (G_X + 18f), fNW_Line);
            //ページ
            pd.printer_print(e, "科目", (G_X + 28f), fNW_Line);
            //ページ
            pd.printer_print(e, "コード", (G_X + 35f), fNW_Line);
            //ページ
            pd.printer_print(e, "科目名", (G_X + 48f), fNW_Line);
            //ページ
            pd.printer_print(e, "科目", (G_X + 88f), fNW_Line);
            //ページ
            pd.printer_print(e, "コード", (G_X + 95f), fNW_Line);
            //ページ
            pd.printer_print(e, "科目名", (G_X + 108f), fNW_Line);
            //ページ
            pd.printer_print(e, "金額", (G_X + 170f), fNW_Line);

            //見出し９行目
            fNW_Line = (float)(G_NW_LINE + 9);
            pd.printer_print(e, "（購買）", (G_X + 7f), fNW_Line);
            pd.printer_print(e, "品名：件名", (G_X + 15f), fNW_Line);
            pd.printer_print(e, "単位", (G_X + 26f + 68f), fNW_Line);
            pd.printer_print(e, "数量", (G_X + 26f + 80f), fNW_Line);
            pd.printer_print(e, "単価", (G_X + 26f + 104f), fNW_Line);

            //見出し１０行目
            fNW_Line = (float)(G_NW_LINE + 10);
            pd.printer_print(e, "（支払）", (G_X + 7f), fNW_Line);
            pd.printer_print(e, "コード", (G_X + 15f), fNW_Line);
            pd.printer_print(e, "業者名", (G_X + 15 + 14f), fNW_Line);
            pd.printer_print(e, "銀行名", (G_X + 50 + 5f), fNW_Line);
            pd.printer_print(e, "支店名", (G_X + 80f), fNW_Line);
            pd.printer_print(e, "口座№", (G_X + 110f), fNW_Line);
            pd.printer_print(e, "サイト", (G_X + 132f), fNW_Line);
            pd.printer_print(e, "手形小切手№", (G_X + 150f), fNW_Line);

            //見出し１１行目
            fNW_Line = (float)(G_NW_LINE + 11);
            pd.printer_print(e, "（条件）", (G_X + 7f), fNW_Line);
            pd.printer_print(e, "コード", (G_X + 15f), fNW_Line);
            pd.printer_print(e, "条件名", (G_X + 28f), fNW_Line);
            pd.printer_print(e, "（借方業者）", (G_X + 79 + 7f), fNW_Line);
            pd.printer_print(e, "コード", (G_X + 80 + 15 + 4f), fNW_Line);
            pd.printer_print(e, "業者名", (G_X + 80 + 19 + 4 + 9f), fNW_Line);

            //見出し１２行目
            fNW_Line = (float)(G_NW_LINE + 12);
            pd.printer_print(e, "摘要", (G_X + 4f), fNW_Line);
            pd.printer_print(e, "予算№", (G_X + 149f), fNW_Line);

            //見出し１３行目
            fNW_Line = (float)(G_NW_LINE + 13);
            pd.printer_print(e, "承認者", (G_X + 70f + 1.4f), fNW_Line);
            pd.printer_print(e, "証憑№", (G_X + 149f), fNW_Line);
        }

        //
        //   見出し印刷
        //
        public static void MIDASI_INSATU(C_PRINT pd, PrintPageEventArgs e)
        {
            float fNW_Line = 0f;

            //印字領域の初期化
            CLR_G_PR();

            //仕訳申請トランのデータを印刷項目にセットする
            if (CKKR0010.G_PRINT_TYPE == 0)
            {
                //リスト番号
                G_PR.LISTNO = Strings.Mid(GYOMU.G_TRAN_AREA[0], 1, 2) + Strings.Mid(GYOMU.G_TRAN_AREA[0], 5, 8);
                //再出力
                G_PR.SAI = "再出力";
            }
            else
            {
                //リスト番号
                G_PR.LISTNO = Strings.Mid(GYOMU.G_SEQNO, 1, 2) + Strings.Mid(GYOMU.G_SEQNO, 5, 8);
                //通常出力
                G_PR.SAI = "　　　";
            }

            //作成日時
            G_PR.SAKUYMD = String.Format("{0:yy/MM/dd HH:mm:ss}", DateTime.Now);

            //ページカウント
            G_PAGE = G_PAGE + 1;
            G_PR.PG = Strings.Left(C_COMMON.MIGIDUME(Conversion.Str(G_PAGE), "#####0", Strings.Len(G_PR.PG)), 6);
            //見出し印字行
            G_NW_LINE = 4;

            //見出し１行目
            fNW_Line = (float)(G_NW_LINE + 1);
            //再出力
            pd.printer_print(e, "CKKR0010", (G_X + 1f), fNW_Line);
            pd.printer_print(e, G_PR.SAI, (G_X + 19f), fNW_Line);
            pd.printer_print(e, "＊＊　仕訳チェックリスト　＊＊", (G_X + 60f), fNW_Line);

            //リスト番号
            pd.printer_print(e, "リスト番号", (G_X + 100f + 1f), fNW_Line);
            pd.printer_print(e, ":", (G_X + 100f + 12f), fNW_Line);
            pd.printer_print(e, Strings.Mid(G_PR.LISTNO, 1, 2) + "-" + Strings.Mid(G_PR.LISTNO, 3, 8), (G_X + 100f + 13f), fNW_Line);

            //作成日
            pd.printer_print(e, "作成", (G_X + 131f), fNW_Line);
            pd.printer_print(e, G_PR.SAKUYMD, (G_X + 136f), fNW_Line);
            //ページ
            pd.printer_print(e, G_PR.PG, (G_X + 160f), fNW_Line);
            pd.printer_print(e, "ページ", (G_X + 167f), fNW_Line);

            //見出しは3行印字
            G_NW_LINE = 7;
            //消費税データが存在するか
            G_SYOUHI = false;
        }

        //
        //   印刷処理
        //
        public static void PR_START(C_PRINT pd, PrintPageEventArgs e)
        {
            //固定項目の位置あわせ
            KOTEI_INSATU(pd, e);

            //変数の位置あわせ
            HENSU_INSATU(pd, e);
        }

        //
        //   flg   -> 0 -> 再印刷  1 -> 通常印刷
        //
        public static void SET_PR_ITEM(short flg, bool bPage)
        {
            decimal L_KIN2 = default(decimal);
            int nLen = 0;

            //印字領域の初期化
            CLR_G_PR();

            //仕訳申請トランのデータを印刷項目にセットする
            if (flg == 0)
            {
                //リスト番号
                G_PR.LISTNO = Strings.Mid(GYOMU.G_TRAN_AREA[0], 1, 2) + Strings.Mid(GYOMU.G_TRAN_AREA[0], 5, 8);
                //再出力
                G_PR.SAI = "再出力";
            }
            else
            {
                //リスト番号
                G_PR.LISTNO = Strings.Mid(GYOMU.G_SEQNO, 1, 2) + Strings.Mid(GYOMU.G_SEQNO, 5, 8);
                //通常出力
                G_PR.SAI = "　　　";
            }

            //作成日時
            G_PR.SAKUYMD = String.Format("{0:yy/MM/dd HH:mm:ss}", DateTime.Now);
            //ページ
            G_PR.PG = Strings.Left(C_COMMON.MIGIDUME(Conversion.Str(G_PAGE), "#####0", Strings.Len(G_PR.PG)), 6);
            //経理日
            G_PR.KEIYMD = Strings.Mid(GYOMU.G_TRAN_AREA[1], 3, 2) + "/" + Strings.Mid(GYOMU.G_TRAN_AREA[1], 5, 2) + "/" + Strings.Mid(GYOMU.G_TRAN_AREA[1], 7, 2);
            //伝票№
            G_PR.DENNO = Strings.Mid(GYOMU.G_TRAN_AREA[2], 9, 4);
            //伝票№2
            G_PR.DENNO2 = Strings.Mid(GYOMU.G_TRAN_AREA[40], 9, 4);
            if (G_PR.DENNO == G_PR.DENNO2)
            {
                //伝票№2
                G_PR.DENNO2 = Strings.Space(4);
            }
            //証憑№
            G_PR.SYONO = Strings.Left(GYOMU.G_TRAN_AREA[3], Strings.Len(G_PR.SYONO));
            //税区分1
            G_PR.KU = Strings.Left(GYOMU.G_TRAN_AREA[26], Strings.Len(G_PR.KU));
            //税区分2
            G_PR.KU2 = Strings.Left(GYOMU.G_TRAN_AREA[28], Strings.Len(G_PR.KU2));
            //科目コード(借)
            G_PR.KRKAMO = Strings.Left(GYOMU.G_TRAN_AREA[5], Strings.Len(G_PR.KRKAMO));
            //内容(借)
            G_PR.KRNAIYO = Strings.Left(GYOMU.G_TRAN_AREA[6], Strings.Len(G_PR.KRNAIYO));
            //科目名(借)
            G_PR.KRKAMONM = Strings.Left(IPPAN.Space_Cut(GYOMU.G_TRAN_AREA[7]), Strings.Len(G_PR.KRKAMONM));
            //科目コード(貸)
            G_PR.KSKAMO = Strings.Left(GYOMU.G_TRAN_AREA[8], Strings.Len(G_PR.KSKAMO));
            //内容(貸)
            G_PR.KSNAIYO = Strings.Left(GYOMU.G_TRAN_AREA[9], Strings.Len(G_PR.KSNAIYO));
            //科目名(貸)
            G_PR.KSKAMONM = Strings.Left(IPPAN.Space_Cut(GYOMU.G_TRAN_AREA[10]), Strings.Len(G_PR.KSKAMONM));
            //通常金額(金額(借))
            G_KIN1 = Convert.ToDecimal(GYOMU.G_TRAN_AREA[11]);
            //金額
            nLen = Strings.Len(G_PR.KIN);
            G_PR.KIN = Strings.Left(C_COMMON.MIGIDUME(Conversion.Str(G_KIN1), "###,###,###,##0", nLen), nLen);
            //消費税データが存在するか
            G_SYOUHI = true;
            //科目コード(消費・借)
            G_PR.SRKAMO = Strings.Left(GYOMU.G_TRAN_AREA[29], Strings.Len(G_PR.SRKAMO));
            //内容(消費・借)
            G_PR.SRNAIYO = Strings.Left(GYOMU.G_TRAN_AREA[30], Strings.Len(G_PR.SRNAIYO));
            //科目名(消費・借)
            G_PR.SRKAMONM = Strings.Left(IPPAN.Space_Cut(GYOMU.G_TRAN_AREA[31]), Strings.Len(G_PR.SRKAMONM));
            //科目コード(消費・貸)
            G_PR.SSKAMO = Strings.Left(GYOMU.G_TRAN_AREA[32], Strings.Len(G_PR.SSKAMO));
            //内容(消費・貸)
            G_PR.SSNAIYO = Strings.Left(GYOMU.G_TRAN_AREA[33], Strings.Len(G_PR.SSNAIYO));
            //科目名(消費・貸)
            G_PR.SSKAMONM = Strings.Left(IPPAN.Space_Cut(GYOMU.G_TRAN_AREA[34]), Strings.Len(G_PR.SSKAMONM));
            //金額(消費)
            G_KIN2 = Convert.ToDecimal(GYOMU.G_TRAN_AREA[35]);
            nLen = Strings.Len(G_PR.SKIN);
            G_PR.SKIN = Strings.Left(C_COMMON.MIGIDUME(Conversion.Str(G_KIN2), "###,###,###,##0", nLen), nLen);
            //必ず集計 総合計計算
            G_KIN3 = Convert.ToDecimal(GYOMU.G_TRAN_AREA[41]);
            if (!bPage)
            {
                G_GOKEI = G_GOKEI + G_KIN3;
                //小計計算
                G_GINKEI = G_GINKEI + G_KIN3;
            }
            //金額(合計)
            nLen = Strings.Len(G_PR.GKIN);
            G_PR.GKIN = Strings.Left(C_COMMON.MIGIDUME(Conversion.Str(G_KIN3), "###,###,###,##0", nLen), nLen);
            //摘要
            G_PR.TEKIYO = Strings.Left(IPPAN.Space_Cut(GYOMU.G_TRAN_AREA[12]), Strings.Len(G_PR.TEKIYO));
            //摘要２
            G_PR.TEKIYO2 = Strings.Left(IPPAN.Space_Set(IPPAN.Space_Cut(GYOMU.G_TRAN_AREA[27]), 60, 2), Strings.Len(G_PR.TEKIYO2));

            if (IPPAN.C_Allspace(GYOMU.G_TRAN_AREA[61]) != 0)
            {
                //業者CD(月末)
                G_PR.GEGYOUCD = Strings.Left(IPPAN.Space_Cut(GYOMU.G_TRAN_AREA[61]), Strings.Len(G_PR.GEGYOUCD));
                //業者名(月末)
                G_PR.GEGYOUNM = Strings.Left(IPPAN.Space_Cut(GYOMU.G_TRAN_AREA[53]), Strings.Len(G_PR.GEGYOUNM));
                //銀行名(月末)
                G_PR.GEGINKONM = Strings.Left(IPPAN.Space_Cut(GYOMU.G_TRAN_AREA[54]), Strings.Len(G_PR.GEGINKONM));
                //支店名(月末)
                G_PR.GESITENNM = Strings.Left(IPPAN.Space_Cut(GYOMU.G_TRAN_AREA[55]), Strings.Len(G_PR.GESITENNM));
                //口座番号(月末)
                G_PR.GEKOUZA = Strings.Left(IPPAN.Space_Cut(GYOMU.G_TRAN_AREA[56]), Strings.Len(G_PR.GEKOUZA));
            }
            else
            {
                //業者CD(月末)
                G_PR.GEGYOUCD = Strings.Space(4);
                //業者名(月末)
                G_PR.GEGYOUNM = Strings.Space(10);
                //銀行名(月末)
                G_PR.GEGINKONM = Strings.Space(10);
                //支店名(月末)
                G_PR.GESITENNM = Strings.Space(10);
                //口座番号(月末)
                G_PR.GEKOUZA = Strings.Space(9);
            }

            //借方業者コードから名称取得
            if (IPPAN.C_Allspace(GYOMU.G_TRAN_AREA[66]) != 0)
            {
                //借方業者コード
                G_PR.KRGYOSYACD = Strings.Left(IPPAN.Space_Cut(GYOMU.G_TRAN_AREA[66]), Strings.Len(G_PR.KRGYOSYACD));
                //借方業者名
                G_PR.KRGYOSYANM = Strings.Left(IPPAN.Space_Cut(GYOMU.G_TRAN_AREA[67]), Strings.Len(G_PR.KRGYOSYANM));
            }
            else
            {
                //借方業者コード
                G_PR.KRGYOSYACD = Strings.Space(4);
                //借方業者名
                G_PR.KRGYOSYANM = Strings.StrConv(Strings.Space(8), VbStrConv.Wide, C_COMMON.G_CMN_LOCALID_JP);
            }
            //サイト(月末)
            G_PR.SAITO = Strings.Left(GYOMU.G_TRAN_AREA[21], Strings.Len(G_PR.SAITO));
            //手形№(月末)
            G_PR.TEGNO = Strings.Left(GYOMU.G_TRAN_AREA[22], Strings.Len(G_PR.TEGNO));
            //申請№(月末)
            G_PR.SINNO = Strings.Mid(GYOMU.G_TRAN_AREA[23], 1, 2) + Strings.Mid(GYOMU.G_TRAN_AREA[23], 5, 8);
            //品名(購買)
            G_PR.HINMEI = Strings.Left(IPPAN.Space_Cut(GYOMU.G_TRAN_AREA[13]), Strings.Len(G_PR.HINMEI));
            //数量(購買)
            L_KIN2 = Convert.ToDecimal(GYOMU.G_TRAN_AREA[14]);
            //数量(購買)
            nLen = Strings.Len(G_PR.SURYO);
            G_PR.SURYO = Strings.Left(C_COMMON.MIGIDUME(Conversion.Str(L_KIN2), "###,###,##0.000", nLen), nLen);
            //単位(購買)
            G_PR.TANI = Strings.Left(IPPAN.Space_Cut(GYOMU.G_TRAN_AREA[15]), Strings.Len(G_PR.TANI));
            //単価(購買)
            L_KIN2 = Convert.ToDecimal(GYOMU.G_TRAN_AREA[16]);

            //単価(購買)
            nLen = Strings.Len(G_PR.TANKA);
            G_PR.TANKA = Strings.Left(C_COMMON.MIGIDUME(Conversion.Str(L_KIN2), "##,###,##0.00", nLen), nLen);
            //条件ＣＤ
            if (IPPAN.C_Allspace(GYOMU.G_TRAN_AREA[44]) == 0)
            {
                //条件ＣＤ
                G_PR.JOKENCD = IPPAN.Space_Set("", 2, 1);
                //条件名
                G_PR.JOKENNM = IPPAN.Space_Set("", 23, 2);
            }
            else
            {
                //条件ＣＤ
                G_PR.JOKENCD = Strings.Left(IPPAN.Space_Cut(GYOMU.G_TRAN_AREA[44]), Strings.Len(G_PR.JOKENCD));
                //条件名
                G_PR.JOKENNM = Strings.Left(IPPAN.Space_Cut(GYOMU.G_TRAN_AREA[43]), Strings.Len(G_PR.JOKENNM));
            }

            //借方１段目
            G_PR.KARI1 = Strings.Left(IPPAN.Space_Cut(GYOMU.G_TRAN_AREA[36]), Strings.Len(G_PR.KARI1));
            //貸方１段目
            G_PR.KASI1 = Strings.Left(IPPAN.Space_Cut(GYOMU.G_TRAN_AREA[37]), Strings.Len(G_PR.KASI1));
            //借方２段目
            G_PR.KARI2 = Strings.Left(IPPAN.Space_Cut(GYOMU.G_TRAN_AREA[38]), Strings.Len(G_PR.KARI2));
            //貸方２段目
            G_PR.KASI2 = Strings.Left(IPPAN.Space_Cut(GYOMU.G_TRAN_AREA[39]), Strings.Len(G_PR.KASI2));
            //借方３段目
            G_PR.KARI3 = Strings.Left(IPPAN.Space_Cut(GYOMU.G_TRAN_AREA[42]), Strings.Len(G_PR.KARI3));
            //貸方３段目
            G_PR.KASI3 = Strings.Left(IPPAN.Space_Cut(GYOMU.G_TRAN_AREA[45]), Strings.Len(G_PR.KASI3));
            //借方１段目ＣＤ
            G_PR.KARICD1 = Strings.Left(GYOMU.G_TRAN_AREA[46], Strings.Len(G_PR.KARICD1));
            //貸方１段目ＣＤ
            G_PR.KASICD1 = Strings.Left(GYOMU.G_TRAN_AREA[47], Strings.Len(G_PR.KASICD1));
            //借方２段目ＣＤ
            G_PR.KARICD2 = Strings.Left(GYOMU.G_TRAN_AREA[48], Strings.Len(G_PR.KARICD2));
            //貸方２段目ＣＤ
            G_PR.KASICD2 = Strings.Left(GYOMU.G_TRAN_AREA[49], Strings.Len(G_PR.KASICD2));
            //借方３段目ＣＤ→借方期日
            G_PR.KARICD3 = Strings.Left(GYOMU.G_TRAN_AREA[50], Strings.Len(G_PR.KARICD3));
            //貸方３段目ＣＤ→貸方期日
            G_PR.KASICD3 = Strings.Left(GYOMU.G_TRAN_AREA[51], Strings.Len(G_PR.KASICD3));

            switch (Strings.Mid(GYOMU.G_TRAN_AREA[52], 1, 1))
            {
                case "1":
                    //預金種別
                    G_PR.YSYUBETU = "ﾌ";
                    break;
                case "2":
                    //預金種別
                    G_PR.YSYUBETU = "ﾄ";
                    break;
                default:
                    //預金種別
                    G_PR.YSYUBETU = " ";
                    break;
            }

            switch (Strings.Mid(GYOMU.G_TRAN_AREA[57], 1, 1))
            {
                case "1":
                    //預金内訳
                    G_PR.YUTI = "当座引き落とし";
                    break;
                case "2":
                    //預金内訳
                    G_PR.YUTI = "ＦＢ　　　　　";
                    break;
                case "3":
                    //預金内訳
                    G_PR.YUTI = "随時ＦＢ　　　";
                    break;
                case "4":
                    //預金内訳
                    G_PR.YUTI = "小切手　　　　";
                    break;
                case "5":
                    //預金内訳
                    G_PR.YUTI = "その他　　　　";
                    break;
                default:
                    //預金内訳
                    G_PR.YUTI = "　　　　　　　";
                    break;
            }
            //switch (Strings.Mid(GYOMU.G_TRAN_AREA[58], 1, 1))
            //{
            //    case "1":
            //        //税率区分
            //        G_PR.ZEIRITU = "5%";
            //        break;
            //    default:
            //        //税率区分
            //        G_PR.ZEIRITU = "3%";
            //        break;
            //}

// 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Mod -start
            G_PR.ZEIRITU_HOJO_KBN = GYOMU.G_TRAN_AREA[71];
            // 消費税リゾルバーのインスタンスの取得
            syohizeiResolver = SyohizeiResolver.GetInstance();
            var target = syohizeiResolver.GetSyohizeiDTO(SyohizeiSearchConditionType.InfoZeiKbn, Strings.Mid(GYOMU.G_TRAN_AREA[58], 1, 1));
            if (target != null)
            {
                G_PR.ZEIRITU = target.Zeiritu + "%";
            }
            var zeirituHojoName = syohizeiResolver.GetZeirituHojoName(SyohizeiSearchConditionType.InfoZeiKbn, Strings.Mid(GYOMU.G_TRAN_AREA[58], 1, 1), G_PR.ZEIRITU_HOJO_KBN);
            if (!string.IsNullOrEmpty(zeirituHojoName))
            {
                if (!string.IsNullOrEmpty(G_PR.ZEIRITU_HOJO_KBN))
                {
                    if (G_PR.ZEIRITU_HOJO_KBN != "0")
                    {
                        G_PR.ZEIRITU_HOJO_KBN = zeirituHojoName.Substring(0, 1);
                    }
                    else
                    {
                        G_PR.ZEIRITU_HOJO_KBN = string.Empty;
                    }
                }
            }
            ////13.08.27 tsunamoto 消費税対応 start
            //IPPAN2.ZeiInfo zeiinfo = IPPAN2.Get_Syohizei(2,Strings.Mid(GYOMU.G_TRAN_AREA[58], 1, 1) );
            //G_PR.ZEIRITU = zeiinfo.zeiritu + "%";
            ////13.08.27 tsunamoto 消費税対応 end
// 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Mod -end

            //借方貸借区分
            G_PR.KRACKBN = Strings.Left(GYOMU.G_TRAN_AREA[59], Strings.Len(G_PR.KRACKBN));
            //貸方貸借区分
            G_PR.KSACKBN = Strings.Left(GYOMU.G_TRAN_AREA[60], Strings.Len(G_PR.KSACKBN));
            //線
            G_PR.SEN = new string('-', 152);

            //(1ﾊﾞｲﾄ)予算№
            G_PR.YOSANNO = Strings.Left(GYOMU.G_TRAN_AREA[62], Strings.Len(G_PR.YOSANNO));
            //(2ﾊﾞｲﾄ)承認者
            G_PR.SYONM5 = Strings.Left(GYOMU.G_TRAN_AREA[68], Strings.Len(G_PR.SYONM5));
            //(2ﾊﾞｲﾄ)承認者
            G_PR.SYONM6 = Strings.Left(GYOMU.G_TRAN_AREA[69], Strings.Len(G_PR.SYONM6));
            //(2ﾊﾞｲﾄ)承認者
            G_PR.SYONM7 = Strings.Left(GYOMU.G_TRAN_AREA[70], Strings.Len(G_PR.SYONM7));
            //(1ﾊﾞｲﾄ)予備フラグ１
            G_PR.YBFLG1 = Strings.Left(GYOMU.G_TRAN_AREA[63], Strings.Len(G_PR.YBFLG1));
            //(1ﾊﾞｲﾄ)予備フラグ２
            G_PR.YBFLG2 = Strings.Left(GYOMU.G_TRAN_AREA[64], Strings.Len(G_PR.YBFLG2));
            //(1ﾊﾞｲﾄ)予備フラグ３
            G_PR.YBFLG3 = Strings.Left(GYOMU.G_TRAN_AREA[65], Strings.Len(G_PR.YBFLG3));
        }
    }
}

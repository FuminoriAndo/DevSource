//*************************************************************************************
//
//   カスタムファイルのリーダクラス(Text用)
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************
using System;
using FileManager.Text.Base;
using ProductionControlDailyExcelCreator.Exceptions;
using ProductionControlDailyExcelCreator.FileReader.Interface;
using Utility.Types;
using Utility.Core;
using Microsoft.VisualBasic;

namespace ProductionControlDailyExcelCreator.FileReader
{
    /// <summary>
    /// PMGQ010Bファイルのリーダクラス(Text用)
    /// </summary>
    public class PMGQ010BTextFileReader : TextFileReaderBase, IPMGQ010BReader
    {

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PMGQ010BTextFileReader() : base()
        {
        }

        #endregion

        #region メソッド

        /// <summary>
        /// 読込んだファイルの妥当性の検証
        /// </summary>
        public override void Validate()
        {
            if (document.RecordGroups.Count == 0)
                throw new FileNoDataException();
        }

        /// <summary>
        /// 受注№の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>受注№</returns>
        public string GetOrderNo(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "OrderNo1")
                     + document.GetStringValue("Detail", recordIndex, "OrderNoX")
                     + document.GetStringValue("Detail", recordIndex, "OrderNo2");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 需要家名の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>需要家名</returns>
        public string GetCustomerName(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "CustomerName");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 規格名の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>規格名</returns>
        public string GetStandardName(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "StandardName");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 製品サイズの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>製品サイズ</returns>
        public string GetProductSize(int recordIndex)
        {
            try
            {
                return (document.GetStringValue("Detail", recordIndex, "ProductAtu").TrimStartZeroWithConvertToInt()).ToString()
                     + Strings.Space(1)
                     + document.GetStringValue("Detail", recordIndex, "ProductX1")
                     + Strings.Space(1)
                     + (document.GetStringValue("Detail", recordIndex, "ProductHaba").TrimStartZeroWithConvertToInt()).ToString()
                     + Strings.Space(1)
                     + document.GetStringValue("Detail", recordIndex, "ProductX2")
                     + Strings.Space(1)
                     + (document.GetStringValue("Detail", recordIndex, "ProductNaga").TrimStartZeroWithConvertToInt()).ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 製品(厚)の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>製品(厚)</returns>
        public string GetProductAtu(int recordIndex)
        {
            try
            {
                var productAtu = (float)(document.GetIntegerValue("Detail", recordIndex, "ProductAtu"))/100;
                return productAtu.ToString("#0.00");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 製品(巾)の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>製品(巾)</returns>
        public string GetProductHaba(int recordIndex)
        {
            try
            {
                return (document.GetStringValue("Detail", recordIndex, "ProductHaba").TrimStartZeroWithConvertToInt()).ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 製品(長)の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>製品(長)</returns>
        public string GetProductNaga(int recordIndex)
        {
            try
            {
                return (document.GetStringValue("Detail", recordIndex, "ProductNaga").TrimStartZeroWithConvertToInt()).ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 定耳の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>定耳</returns>
        public string GetTM(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "TM");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 製品コードの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>製品コード</returns>
        public string GetProductCode(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "ProductCode");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ロールサイズの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ロールサイズ</returns>
        public string GetRollSize(int recordIndex)
        {
            try
            {
                return (document.GetStringValue("Detail", recordIndex, "RollHaba").TrimStartZeroWithConvertToInt()).ToString()
                     + Strings.Space(1)
                     + document.GetStringValue("Detail", recordIndex, "RollX")
                     + Strings.Space(1)
                     + (document.GetStringValue("Detail", recordIndex, "RollNaga").TrimStartZeroWithConvertToInt()).ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ロールサイズ(巾)の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ロールサイズ(巾)</returns>
        public string GetRollHaba(int recordIndex)
        {
            try
            {
                return (document.GetStringValue("Detail", recordIndex, "RollHaba").TrimStartZeroWithConvertToInt()).ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ロールサイズ(長)の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ロールサイズ(長)</returns>
        public string GetRollNaga(int recordIndex)
        {
            try
            {
                return (document.GetStringValue("Detail", recordIndex, "RollNaga").TrimStartZeroWithConvertToInt()).ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// TP区分の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>TP区分</returns>
        public string GetTPClassfication(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "TPClassfication");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ロットの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ロット</returns>
        public string GetLot(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "Lot1")
                     + document.GetStringValue("Detail", recordIndex, "LotX")
                     + document.GetStringValue("Detail", recordIndex, "Lot2");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ロット1の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ロット1</returns>
        public string GetLot1(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "Lot1");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ロット2の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ロット2</returns>
        public string GetLot2(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "Lot2");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 圧延予定の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>圧延予定</returns>
        public string GetAtuenYMD(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "AtuenYMD");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// チャージ№の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>チャージ№</returns>
        public string GetCHNo(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "CHNo");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// スラブ№の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>スラブ№</returns>
        public string GetSlabNo(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "CHNo")
                     + document.GetStringValue("Detail", recordIndex, "SlabX1")
                     + document.GetStringValue("Detail", recordIndex, "SlabNo1")
                     + document.GetStringValue("Detail", recordIndex, "SlabX2")
                     + document.GetStringValue("Detail", recordIndex, "SlabNo2");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ロール№の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ロール№</returns>
        public string GetRollNo(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "RollNo");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 抽出日の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>抽出日</returns>
        public string GetExtractionYMD(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "ExtractionYMD");
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }

    /// <summary>
    /// PMPA260Bファイルのリーダクラス(Text用)
    /// </summary>
    public class PMPA260BTextFileReader : TextFileReaderBase, IPMPA260BReader
    {

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PMPA260BTextFileReader() : base()
        {
        }

        #endregion

        #region メソッド

        /// <summary>
        /// 読込んだファイルの妥当性の検証
        /// </summary>
        public override void Validate()
        {
            if (document.RecordGroups.Count == 0)
                throw new FileNoDataException();
        }

        /// <summary>
        /// RCNOの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>RCNO</returns>
        public string GetRCNO(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "RCNO");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 勤務の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>勤務</returns>
        public string GetWork(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "Work");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 圧延順の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>圧延順</returns>
        public string GetAtuenOrder(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "AtuenOrder");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ロット№の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ロット№</returns>
        public string GetLotNo(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "Lot1")
                     + document.GetStringValue("Detail", recordIndex, "LotDash")
                     + document.GetStringValue("Detail", recordIndex, "Lot2");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ロット1の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ロット1</returns>
        public string GetLot1(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "Lot1");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ロット2の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ロット2</returns>
        public string GetLot2(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "Lot2");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// チャージ№の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>チャージ№</returns>
        public string GetCHNo(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "CHNo");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// チャージ("-")の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>チャージ("-")</returns>
        public string GetCHDash(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "CHDash");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 1次鋳片№の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>1次鋳片№</returns>
        public string GetSlab1No(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "Slab1No");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 2次鋳片№の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>2次鋳片№</returns>
        public string GetSlab2No(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "Slab2No");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 材料識別の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>材料識別</returns>
        public string GetMaterialsIdentification(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "MaterialsIdentification1")
                     + document.GetStringValue("Detail", recordIndex, "MaterialsIdentificationDash")
                     + document.GetStringValue("Detail", recordIndex, "MaterialsIdentification2");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 材料識別1の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>材料識別1</returns>
        public string GetMaterialsIdentification1(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "MaterialsIdentification1");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 材料識別2の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>材料識別2</returns>
        public string GetMaterialsIdentification2(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "MaterialsIdentification2");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ＴＰ管理の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ＴＰ管理</returns>
        public string GetTPManagement(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "TPManagement");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 厚テーバーの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>厚テーバー</returns>
        public string GetAtuTapera(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "AtuTapera");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 巾テーパーの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>巾テーパー</returns>
        public string GetHabaTapera(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "HabaTapera");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ＨＣ区分の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ＨＣ区分</returns>
        public string GetHCDivision(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "HCDivision");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 抽出時分の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>抽出時分</returns>
        public string GetExtractionHHMM(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "ExtractionHH")
                     + document.GetStringValue("Detail", recordIndex, "ExtractionKoron")
                     + document.GetStringValue("Detail", recordIndex, "ExtractionMM");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 抽出時の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>抽出時</returns>
        public string GetExtractionHH(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "ExtractionHH");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 抽出分の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>抽出分</returns>
        public string GetExtractionMM(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "ExtractionMM");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 規格の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>規格</returns>
        public string GetStandard(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "Standard");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ロールサイズの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ロールサイズ</returns>
        public string GetRollSize(int recordIndex)
        {
            try
            {
                return (document.GetStringValue("Detail", recordIndex, "RollAtu").TrimStartZeroWithConvertToInt()).ToString()
                     + Strings.Space(1)
                     + document.GetStringValue("Detail", recordIndex, "RollX1")
                     + Strings.Space(1)
                     + (document.GetStringValue("Detail", recordIndex, "RollHaba").TrimStartZeroWithConvertToInt()).ToString()
                     + Strings.Space(1)
                     + document.GetStringValue("Detail", recordIndex, "RollX2")
                     + Strings.Space(1)
                     + (document.GetStringValue("Detail", recordIndex, "RollNaga").TrimStartZeroWithConvertToInt()).ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ロール厚の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ロール厚</returns>
        public string GetRollAtu(int recordIndex)
        {
            try
            {
                var rollAtu = (float)(document.GetIntegerValue("Detail", recordIndex, "RollAtu")) / 100;
                return rollAtu.ToString("#0.00");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ロール巾の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ロール巾</returns>
        public string GetRollHaba(int recordIndex)
        {
            try
            {
                var rollHaba = document.GetStringValue("Detail", recordIndex, "RollHaba");
                return rollHaba.TrimStartZeroWithConvertToInt().ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ロール長の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ロール長</returns>
        public string GetRollNaga(int recordIndex)
        {
            try
            {
                var rollNaga = document.GetStringValue("Detail", recordIndex, "RollNaga");
                return rollNaga.TrimStartZeroWithConvertToInt().ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// スラブサイズの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>スラブサイズ</returns>
        public string GetSlabSize(int recordIndex)
        {
            try
            {
                return (document.GetStringValue("Detail", recordIndex, "SlabAtu").TrimStartZeroWithConvertToInt()).ToString()
                     + Strings.Space(1)
                     + document.GetStringValue("Detail", recordIndex, "SlabAtuX")
                     + Strings.Space(1)
                     + (document.GetStringValue("Detail", recordIndex, "SlabHaba").TrimStartZeroWithConvertToInt()).ToString()
                     + Strings.Space(1)
                     + document.GetStringValue("Detail", recordIndex, "SlabHabaX")
                     + Strings.Space(1)
                     + (document.GetStringValue("Detail", recordIndex, "SlabNaga").TrimStartZeroWithConvertToInt()).ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// スラブ厚の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>スラブ厚</returns>
        public string GetSlabAtu(int recordIndex)
        {
            try
            {
                return (document.GetStringValue("Detail", recordIndex, "SlabAtu").TrimStartZeroWithConvertToInt()).ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// スラブ巾の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>スラブ巾</returns>
        public string GetSlabHaba(int recordIndex)
        {
            try
            {
                return (document.GetStringValue("Detail", recordIndex, "SlabHaba").TrimStartZeroWithConvertToInt()).ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// スラブ長の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>スラブ長</returns>
        public string GetSlabNaga(int recordIndex)
        {
            try
            {
                return (document.GetStringValue("Detail", recordIndex, "SlabNaga").TrimStartZeroWithConvertToInt()).ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// スラブ重量の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>スラブ重量</returns>
        public string GetSlabWeight(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "SlabWeight");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 本数の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>本数</returns>
        public string GetSlabCount(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "SlabCount");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 再計画の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>再計画</returns>
        public string GetRePlan(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "RePlan");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 急ぎの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>急ぎ</returns>
        public string GetHaste(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "Haste");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 輸出の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>輸出</returns>
        public string GetExport(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "Export");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 抽出温度の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>抽出温度</returns>
        public string GetExtractionTemperature(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "ExtractionTemperature");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 試圧の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>試圧</returns>
        public string GetTestPressure(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "TestPressure");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 板巾の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>板巾</returns>
        public string GetPlateHaba(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "PlateHaba");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 歪の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>歪</returns>
        public string GetStrain(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "Strain");
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }

    /// <summary>
    /// PMPD330Bファイルのリーダクラス(Text用)
    /// </summary>
    public class PMPD330BTextFileReader : TextFileReaderBase, IPMPD330BReader
    {

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PMPD330BTextFileReader() : base()
        {
        }

        #endregion

        #region メソッド

        /// <summary>
        /// 読込んだファイルの妥当性の検証
        /// </summary>
        public override void Validate()
        {
            if(document.RecordGroups.Count == 0)
                throw new FileNoDataException();
        }

        /// <summary>
        /// 鋼種名の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>鋼種名</returns>
        public string GetSteelsName(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "SteelsName");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 鋼種別の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>鋼種別</returns>
        public string GetSteelsType(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "SteelsType");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 規格名の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>規格名</returns>
        public string GetStandardName(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "StandardName");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 製品サイズの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>製品サイズ</returns>
        public string GetProductSize(int recordIndex)
        {
            try
            {
                return (document.GetStringValue("Detail", recordIndex, "ProductAtu").TrimStartZeroWithConvertToInt()).ToString()
                     + Strings.Space(1)
                     + SymbolType.Multiple.GetValue()
                     + Strings.Space(1)
                     + (document.GetStringValue("Detail", recordIndex, "ProductHaba").TrimStartZeroWithConvertToInt()).ToString()
                     + Strings.Space(1)
                     + SymbolType.Multiple.GetValue()
                     + Strings.Space(1)
                     + (document.GetStringValue("Detail", recordIndex, "ProductNaga").TrimStartZeroWithConvertToInt()).ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 製品厚の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>製品厚</returns>
        public string GetProductAtu(int recordIndex)
        {
            try
            {
                var productAtu = (float)(document.GetIntegerValue("Detail", recordIndex, "ProductAtu")) / 100;
                return productAtu.ToString("#0.00");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 製品巾の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>製品巾</returns>
        public string GetProductHaba(int recordIndex)
        {
            try
            {
                return (document.GetStringValue("Detail", recordIndex, "ProductHaba").TrimStartZeroWithConvertToInt()).ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 製品長の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>製品長</returns>
        public string GetProductNaga(int recordIndex)
        {
            try
            {
                return (document.GetStringValue("Detail", recordIndex, "ProductNaga").TrimStartZeroWithConvertToInt()).ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 定耳の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>定耳</returns>
        public string GetTM(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "TM");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 製品コードの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>製品コード</returns>
        public string GetProductCode(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "ProductCode");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 受注№の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>受注№</returns>
        public string GetOrdersNo(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "OrdersNo");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 需要家名の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>需要家名</returns>
        public string GetCustomerName(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "CustomerName");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 納期月日の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>納期月日</returns>
        public string GetDeliveryDate(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "DeliveryDateMM")
                     + SymbolType.Slash.GetValue()
                     + document.GetStringValue("Detail", recordIndex, "DeliveryDateDD");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 納期ランクの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>納期ランク</returns>
        public string GetDeliveryDateRank(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "DeliveryDateRank");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 枚数の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>枚数</returns>
        public string GetMaisuu(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "Maisuu");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ロールサイズの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ロールサイズ</returns>
        public string GetRollSize(int recordIndex)
        {
            try
            {
                return (document.GetStringValue("Detail", recordIndex, "RollHaba").TrimStartZeroWithConvertToInt()).ToString()
                     + Strings.Space(1)
                     + SymbolType.Multiple.GetValue()
                     + Strings.Space(1)
                     + (document.GetStringValue("Detail", recordIndex, "RollNaga").TrimStartZeroWithConvertToInt()).ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>v
        /// ロール巾の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ロール巾</returns>
        public string GetRollHaba(int recordIndex)
        {
            try
            {
                return (document.GetStringValue("Detail", recordIndex, "RollHaba").TrimStartZeroWithConvertToInt()).ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ロール長の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ロール長</returns>
        public string GetRollNaga(int recordIndex)
        {
            try
            {
                return (document.GetStringValue("Detail", recordIndex, "RollNaga").TrimStartZeroWithConvertToInt()).ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 保温区分の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>保温区分</returns>
        public string GetHeatInsulation(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "HeatInsulation");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// スラブサイズの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>スラブサイズ</returns>
        public string GetSlabSize(int recordIndex)
        {
            try
            {
                return (document.GetStringValue("Detail", recordIndex, "SlabAtu").TrimStartZeroWithConvertToInt()).ToString()
                     + Strings.Space(1)
                     + SymbolType.Multiple.GetValue()
                     + Strings.Space(1)
                     + (document.GetStringValue("Detail", recordIndex, "SlabHaba").TrimStartZeroWithConvertToInt()).ToString()
                     + Strings.Space(1)
                     + SymbolType.Multiple.GetValue()
                     + Strings.Space(1)
                     + (document.GetStringValue("Detail", recordIndex, "SlabNaga").TrimStartZeroWithConvertToInt()).ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// スラブ厚の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>スラブ厚</returns>
        public string GetSlabAtu(int recordIndex)
        {
            try
            {
                return (document.GetStringValue("Detail", recordIndex, "SlabAtu").TrimStartZeroWithConvertToInt()).ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// スラブ巾の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>スラブ巾</returns>
        public string GetSlabHaba(int recordIndex)
        {
            try
            {
                return (document.GetStringValue("Detail", recordIndex, "SlabHaba").TrimStartZeroWithConvertToInt()).ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// スラブ長の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>スラブ長</returns>
        public string GetSlabNaga(int recordIndex)
        {
            try
            {
                return (document.GetStringValue("Detail", recordIndex, "SlabNaga").TrimStartZeroWithConvertToInt()).ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// スラブ重量の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>スラブ重量</returns>
        public string GetSlabWeight(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "SlabWeight");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// スラブ本数の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>スラブ本数</returns>
        public string GetSlabCount(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "SlabCount");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 急ぎ区分の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>急ぎ区分</returns>
        public string GetHasteDivision(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "HasteDivision");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 特殊区分の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>特殊区分</returns>
        public string GetSpecialDivision(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "SpecialDivision");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 輸出区分の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>輸出区分</returns>
        public string GetExportDivision(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "ExportDivision");
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }

    /// <summary>
    /// PMPF070Bファイルのリーダクラス(Text用)
    /// </summary>
    public class PMPF070BTextFileReader : TextFileReaderBase, IPMPF070BReader
    {

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PMPF070BTextFileReader() : base()
        {
        }

        #endregion

        #region メソッド

        /// <summary>
        /// 読込んだファイルの妥当性の検証
        /// </summary>
        public override void Validate()
        {
            if (document.RecordGroups.Count == 0)
                throw new FileNoDataException();
        }

        /// <summary>
        /// 受注№の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>受注№</returns>
        public string GetOrderNo(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "OrderNo1")
                     + document.GetStringValue("Detail", recordIndex, "OrderNoDash1")
                     + document.GetStringValue("Detail", recordIndex, "OrderNo2")
                     + document.GetStringValue("Detail", recordIndex, "OrderNoDash2")
                     + document.GetStringValue("Detail", recordIndex, "OrderNo3");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 受注№1の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>受注№1</returns>
        public string GetOrderNo1(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "OrderNo1");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 受注№2の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>受注№2</returns>
        public string GetOrderNo2(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "OrderNo2");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 受注№3の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>受注№3</returns>
        public string GetOrderNo3(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "OrderNo3");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 納期の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>納期</returns>
        public string GetDeliveryDate(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "DeliveryDateMM")
                     + document.GetStringValue("Detail", recordIndex, "DeliveryDateSlash")
                     + document.GetStringValue("Detail", recordIndex, "DeliveryDateDD");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 納期(MM)の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>納期(MM)</returns>
        public string GetDeliveryDateMM(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "DeliveryDateMM");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 納期(DD)の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>納期(DD)</returns>
        public string GetDeliveryDateDD(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "DeliveryDateDD");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 納期ランクの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>納期ランク</returns>
        public string GetDeliveryDateRank(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "DeliveryDateRank");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 需要家名の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>需要家名</returns>
        public string GetCustomerName(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "CustomerName");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ロットNoの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ロットNo</returns>
        public string GetLotNo(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "LotNo1") 
                     + document.GetStringValue("Detail", recordIndex, "LotNoDash")
                     + document.GetStringValue("Detail", recordIndex, "LotNo2");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ロット1の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ロット1</returns>
        public string GetLot1(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "LotNo1");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ロット2の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ロット2</returns>
        public string GetLot2(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "LotNo2");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ロール№の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ロール№</returns>
        public string GetRollNo(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "RollNo");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 未採取ロットの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>未採取ロット</returns>
        public string GetNotCollectedLot(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "NotCollectedLotNo1")
                     + document.GetStringValue("Detail", recordIndex, "NotCollectedLotNoDash")
                     + document.GetStringValue("Detail", recordIndex, "NotCollectedLotNo2");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 未採取ロット1の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>未採取ロット1</returns>
        public string GetNotCollectedLotNo1(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "NotCollectedLotNo1");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 未採取ロット2の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>未採取ロット2</returns>
        public string GetNotCollectedLotNo2(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "NotCollectedLotNo2");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 状態の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>状態</returns>
        public string GetStatus(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "Status");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 計画規格コードの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>計画規格コード</returns>
        public string GetPlanStandardCode(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "PlanStandardCode");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 計画サイズの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>計画サイズ</returns>
        public string GetPlanSize(int recordIndex)
        {
            try
            {
                return (document.GetStringValue("Detail", recordIndex, "PlanAtu").TrimStartZeroWithConvertToInt()).ToString()
                     + Strings.Space(1)
                     + document.GetStringValue("Detail", recordIndex, "PlanX1")
                     + Strings.Space(1)
                     + (document.GetStringValue("Detail", recordIndex, "PlanHaba").TrimStartZeroWithConvertToInt()).ToString()
                     + Strings.Space(1)
                     + document.GetStringValue("Detail", recordIndex, "PlanX2")
                     + Strings.Space(1)
                     + (document.GetStringValue("Detail", recordIndex, "PlanNaga").TrimStartZeroWithConvertToInt()).ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 計画厚の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>計画厚</returns>
        public string GetPlanAtu(int recordIndex)
        {
            try
            {
                var planAtu = (float)(document.GetIntegerValue("Detail", recordIndex, "PlanAtu")) / 100;
                return planAtu.ToString("#0.00");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 計画巾の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>計画巾</returns>
        public string GetPlanHaba(int recordIndex)
        {
            try
            {
                return (document.GetStringValue("Detail", recordIndex, "PlanHaba").TrimStartZeroWithConvertToInt()).ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 計画長の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>計画長</returns>
        public string GetPlanNaga(int recordIndex)
        {
            try
            {
                return (document.GetStringValue("Detail", recordIndex, "PlanNaga").TrimStartZeroWithConvertToInt()).ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 計画耳の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>計画耳</returns>
        public string GetPlanTM(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "PlanTM");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 計画製品コードの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>計画製品コード</returns>
        public string GetPlanProductCode(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "PlanProductCode");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 計画枚数の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>計画枚数</returns>
        public string GetPlanMaisuu(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "PlanMaisuu");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 実績規格コードの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>実績規格コード</returns>
        public string GetPerformanceStandardCode(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "PerformanceStandardCode");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 実績サイズの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>実績サイズ</returns>
        public string GetPerformanceSize(int recordIndex)
        {
            try
            {
                return (document.GetStringValue("Detail", recordIndex, "PerformanceAtu").TrimStartZeroWithConvertToInt()).ToString()
                     + Strings.Space(1)
                     + document.GetStringValue("Detail", recordIndex, "PerformanceX1")
                     + Strings.Space(1)
                     + (document.GetStringValue("Detail", recordIndex, "PerformanceHaba").TrimStartZeroWithConvertToInt()).ToString()
                     + Strings.Space(1)
                     + document.GetStringValue("Detail", recordIndex, "PerformanceX2")
                     + Strings.Space(1)
                     + (document.GetStringValue("Detail", recordIndex, "PerformanceNaga").TrimStartZeroWithConvertToInt()).ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 実績サイズ(厚)の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>実績サイズ(厚)</returns>
        public string GetPerformanceAtu(int recordIndex)
        {
            try
            {
                var performanceAtu = (float)(document.GetIntegerValue("Detail", recordIndex, "PerformanceAtu")) / 100;
                return performanceAtu.ToString("#0.00");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 実績サイズ(巾)の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>実績サイズ(巾)</returns>
        public string GetPerformanceHaba(int recordIndex)
        {
            try
            {
                return (document.GetStringValue("Detail", recordIndex, "PerformanceHaba").TrimStartZeroWithConvertToInt()).ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 実績サイズ(長)の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>実績サイズ(長)</returns>
        public string GetPerformanceNaga(int recordIndex)
        {
            try
            {
                return (document.GetStringValue("Detail", recordIndex, "PerformanceNaga").TrimStartZeroWithConvertToInt()).ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 実績耳の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>実績耳</returns>
        public string GetPerformanceTM(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "PerformanceTM");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 実績製品コードの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>実績製品コード</returns>
        public string GetPerformanceProdutCode(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "PerformanceProdutCode");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 実績枚数の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>実績枚数</returns>
        public string GetPerformanceMaisuu(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "PerformanceMaisuu");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 発生工程の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>発生工程</returns>
        public string GetGeneratingStep(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "GeneratingStep");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 特殊の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>特殊</returns>
        public string GetSpecial(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "Special");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 抽出日の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>抽出日</returns>
        public string GetExtractionYMD(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "ExtractionYMD");
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }

    /// <summary>
    /// PMPF090Bファイルのリーダクラス(Text用)
    /// </summary>
    public class PMPF090BTextFileReader : TextFileReaderBase, IPMPF090BReader
    {

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PMPF090BTextFileReader() : base()
        {
        }

        #endregion

        #region メソッド

        /// <summary>
        /// 読込んだファイルの妥当性の検証
        /// </summary>
        public override void Validate()
        {
            if (document.RecordGroups.Count == 0)
                throw new FileNoDataException();
        }

        /// <summary>
        /// 未採取区分の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>未採取区分</returns>
        public string GetNotCollectClassfication(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "NotCollectClassfication");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 受注№の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>受注№</returns>
        public string GetOrderNo(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "OrderNo1")
                     + document.GetStringValue("Detail", recordIndex, "OrderNoDash1")
                     + document.GetStringValue("Detail", recordIndex, "OrderNo2")
                     + document.GetStringValue("Detail", recordIndex, "OrderNoDash2")
                     + document.GetStringValue("Detail", recordIndex, "OrderNo3");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 受注№1の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>受注№1</returns>
        public string GetOrderNo1(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "OrderNo1");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 受注№2の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>受注№2</returns>
        public string GetOrderNo2(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "OrderNo2");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 受注№3の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>受注№3</returns>
        public string GetOrderNo3(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "OrderNo3");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 需要家名の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>需要家名</returns>
        public string GetCustomerName(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "CustomerName");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 未計画の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>未計画</returns>
        public string GetUnPlan(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "UnPlan");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 納期の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>納期</returns>
        public string GetDeliveryDate(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "DeliveryDate");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 納期ランクの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>納期ランク</returns>
        public　string GetDeliveryDateRank(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "DeliveryDateRank");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 規格名の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>規格名</returns>
        public string GetStandardName(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "StandardName");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 製品サイズの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>製品サイズ</returns>
        public string GetProductSize(int recordIndex)
        {
            try
            {
                return (document.GetStringValue("Detail", recordIndex, "ProductAtu").TrimStartZeroWithConvertToInt()).ToString()
                     + Strings.Space(1)
                     + document.GetStringValue("Detail", recordIndex, "ProductX1")
                     + Strings.Space(1)
                     + (document.GetStringValue("Detail", recordIndex, "ProductHaba").TrimStartZeroWithConvertToInt()).ToString()
                     + Strings.Space(1)
                     + document.GetStringValue("Detail", recordIndex, "ProductX2")
                     + Strings.Space(1)
                     + (document.GetStringValue("Detail", recordIndex, "ProductNaga").TrimStartZeroWithConvertToInt()).ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 製品(厚)の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>製品(厚)</returns>
        public string GetProductAtu(int recordIndex)
        {
            try
            {
                var ret = string.Empty;
                
                // 製品(厚)
                var productAtu = (document.GetStringValue("Detail", recordIndex, "ProductAtu").TrimStartZeroWithConvertToInt()).ToString();
                if(productAtu != NumericType.Zero.GetStringValue())
                {
                    var tmp = (float)(document.GetIntegerValue("Detail", recordIndex, "ProductAtu")) / 100;
                    ret = tmp.ToString("#0.00");
                }

                return ret;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 製品(巾)の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>製品(巾)</returns>
        public string GetProductHaba(int recordIndex)
        {
            try
            {
                // 製品(巾)
                var productHaba = (document.GetStringValue("Detail", recordIndex, "ProductHaba").TrimStartZeroWithConvertToInt()).ToString();
                return productHaba = (productHaba != NumericType.Zero.GetStringValue()) ? productHaba : string.Empty;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 製品(長)の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>製品(長)</returns>
        public string GetProductNaga(int recordIndex)
        {
            try
            {
                // 製品(長)
                var productNaga = (document.GetStringValue("Detail", recordIndex, "ProductNaga").TrimStartZeroWithConvertToInt()).ToString();
                return productNaga = (productNaga != NumericType.Zero.GetStringValue()) ? productNaga : string.Empty;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 定耳の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>定耳</returns>
        public string GetTM(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "TM");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 製品コードの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>製品コード</returns>
        public string GetProductCode(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "ProductCode");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 置場の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>置場</returns>
        public string GetYard(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "YardCode")
                     + document.GetStringValue("Detail", recordIndex, "YardDash1")
                     + document.GetStringValue("Detail", recordIndex, "YardX")
                     + document.GetStringValue("Detail", recordIndex, "YardDash2")
                     + document.GetStringValue("Detail", recordIndex, "YardY");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 置場コードの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>置場コード</returns>
        public string GetYardCode(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "YardCode");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 置場Xの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>置場X</returns>
        public string GetYardX(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "YardX");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 置場Yの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>置場Y</returns>
        public string GetYardY(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "YardY");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 枚数の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>枚数</returns>
        public string GetMaisuu(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "Maisuu");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 紐付枚数の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>紐付枚数</returns>
        public string GetTieMaisuu(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "TieMaisuu");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 予約区分の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>予約区分</returns>
        public string GetReservationClassfication(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "ReservationClassfication");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 発生品区分1の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>発生品区分1</returns>
        public string GetGeneratingProductClassfication1(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "GeneratingProductClassfication1");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 板№1の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>板№1</returns>
        public string GetItaNo1(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "ItaNo1");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 試験合否1の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>試験合否1</returns>
        public string GetTestAcceptance1(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "TestAcceptance1");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 積み上げ1の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>積み上げ1</returns>
        public string GetPileUp1(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "PileUp1");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 発生品区分2の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>発生品区分2</returns>
        public string GetGeneratingProductClassfication2(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "GeneratingProductClassfication2");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 板№2の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>板№2</returns>
        public string GetItaNo2(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "ItaNo2");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 試験合否2の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>試験合否2</returns>
        public string GetTestAcceptance2(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "TestAcceptance2");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 積み上げ2の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>積み上げ2</returns>
        public string GetPileUp2(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "PileUp2");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 抽出日の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>抽出日</returns>
        public string GetExtractionYMD(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "ExtractionYMD");
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }

    /// <summary>
    /// PQGA186Bファイルのリーダクラス(Text用)
    /// </summary>
    public class PQGA186BTextFileReader : TextFileReaderBase, IPQGA186BReader
    {

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PQGA186BTextFileReader() : base()
        {
        }

        #endregion

        #region メソッド

        /// <summary>
        /// 読込んだファイルの妥当性の検証
        /// </summary>
        public override void Validate()
        {
            if (document.RecordGroups.Count == 0)
                throw new FileNoDataException();
        }

        /// <summary>
        /// 発行№+日付の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>発行№+日付</returns>
        public string GetIssuNoAndDate(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "IssuNoAndDate");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// チャージ№の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>チャージ№</returns>
        public string GetCHNo(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "CHNo");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 規格名の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>規格名</returns>
        public string GetStandardName(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "StandardName");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// スラブサイズの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>スラブサイズ</returns>
        public string GetSlabSize(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "Atu")
                     + Strings.Space(1)
                     + document.GetStringValue("Detail", recordIndex, "X1")
                     + Strings.Space(1)
                     + document.GetStringValue("Detail", recordIndex, "Haba")
                     + Strings.Space(1)
                     + document.GetStringValue("Detail", recordIndex, "X2")
                     + Strings.Space(1)
                     + document.GetStringValue("Detail", recordIndex, "Naga");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// スラブサイズ(厚)の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>スラブサイズ(厚)</returns>
        public string GetSlabAtu(int recordIndex)
        {
            try
            {
                return (document.GetStringValue("Detail", recordIndex, "Atu").TrimStartZeroWithConvertToInt()).ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// スラブサイズ(巾)の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>スラブサイズ(巾)</returns>
        public string GetSlabHaba(int recordIndex)
        {
            try
            {
                return (document.GetStringValue("Detail", recordIndex, "Haba").TrimStartZeroWithConvertToInt()).ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// スラブサイズ(長)の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>スラブサイズ(長)</returns>
        public string GetSlabNaga(int recordIndex)
        {
            try
            {
                return (document.GetStringValue("Detail", recordIndex, "Naga").TrimStartZeroWithConvertToInt()).ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 単重の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>単重</returns>
        public string GetUnitWeight(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "UnitWeight");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 受注№の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>受注№</returns>
        public string GetOrderNo(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "OrderNo");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 計画№の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>計画№</returns>
        public string GetPlanNo(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "PlanNo");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 抽出日の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>抽出日</returns>
        public string GetExtractionYMD(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "ExtractionYMD");
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }

    /// <summary>
    /// PQGA380Bファイルのリーダクラス(Text用)
    /// </summary>
    public class PQGA380BTextFileReader : TextFileReaderBase, IPQGA380BReader
    {

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PQGA380BTextFileReader() : base()
        {
        }

        #endregion

        #region メソッド

        /// <summary>
        /// 読込んだファイルの妥当性の検証
        /// </summary>
        public override void Validate()
        {
            if (document.RecordGroups.Count == 0)
                throw new FileNoDataException();
        }

        /// <summary>
        /// 受注№の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>受注№</returns>
        public string GetOrderNo(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "OrderNo");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 需要家コードの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>需要家コード</returns>
        public string GetCustomerCode(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "CustomerCode");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 需要家名の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>需要家名</returns>
        public string GetCustomerName(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "CustomerName");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 商社コードの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>商社コード</returns>
        public string GetDisutributorCode(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "DisutributorCode");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 都市の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>都市</returns>
        public string GetCity(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "City");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 回収の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>回収</returns>
        public string GetCollectiion(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "Collectiion");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 品種の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>品種</returns>
        public string GetKind(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "Kind");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 規格コードの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>規格コード</returns>
        public string GetStandardCode(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "StandardCode");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 決済条件の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>決済条件</returns>
        public string GetSettlementCondition(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "SettlementCondition");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 商社名の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>商社名</returns>
        public string GetDisutributorName(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "DisutributorName");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 規格名の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>規格名</returns>
        public string GetStandardName(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "StandardName");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 製品サイズの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>製品サイズ</returns>
        public string GetProductSize(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "Atu")
                     + Strings.Space(1)
                     + document.GetStringValue("Detail", recordIndex, "AtuHabaX1")
                     + Strings.Space(1)
                     + document.GetStringValue("Detail", recordIndex, "Haba")
                     + Strings.Space(1)
                     + document.GetStringValue("Detail", recordIndex, "AtuHabaX2")
                     + Strings.Space(1)
                     + document.GetStringValue("Detail", recordIndex, "Naga");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 製品(厚)の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>製品(厚)</returns>
        public string GetProductAtu(int recordIndex)
        {
            try
            {
                return (document.GetStringValue("Detail", recordIndex, "Atu").TrimStartZeroWithConvertToInt()).ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 製品(巾)の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>製品(巾)</returns>
        public string GetProductHaba(int recordIndex)
        {
            try
            {
                return (document.GetStringValue("Detail", recordIndex, "Haba").TrimStartZeroWithConvertToInt()).ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 製品(長)の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>製品(長)</returns>
        public string GetProductNaga(int recordIndex)
        {
            try
            {
                return (document.GetStringValue("Detail", recordIndex, "Naga").TrimStartZeroWithConvertToInt()).ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 出荷本数の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>出荷本数</returns>
        public string GetShipmentHonsuu(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "ShipmentHonsuu");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 出荷重量の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>出荷重量</returns>
        public string GetShipmentWeight(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "ShipmentWeight");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 単価の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>単価</returns>
        public string GetUnitPrice(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "UnitPrice");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 受渡条件の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>受渡条件</returns>
        public string GetTransferCondition(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "TransferCondition");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// チャージ№の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>チャージ№</returns>
        public string GetCHNo(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "CHNo");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// １次№①の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>１次№①</returns>
        public string GetSlab1No1(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "Slab1No1");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// １次№②の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>１次№②</returns>
        public string GetSlab1No2(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "Slab1No2");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// １次№③の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>１次№③</returns>
        public string GetSlab1No3(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "Slab1No3");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// １次№④の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>１次№④</returns>
        public string GetSlab1No4(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "Slab1No4");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// １次№⑤の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>１次№⑤</returns>
        public string GetSlab1No5(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "Slab1No5");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// １次№⑥の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>１次№⑥</returns>
        public string GetSlab1No6(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "Slab1No6");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// １次№⑦の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>１次№⑦</returns>
        public string GetSlab1No7(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "Slab1No7");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// １次№⑧の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>１次№⑧</returns>
        public string GetSlab1No8(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "Slab1No8");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// １次№⑨の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>１次№⑨</returns>
        public string GetSlab1No9(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "Slab1No9");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// １次№⑩の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>１次№⑩</returns>
        public string GetSlab1No10(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "Slab1No10");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// チャージスラブ本数の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>チャージスラブ本数</returns>
        public string GetSlabHonsuu(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "SlabHonsuu");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// チャージスラブ重量の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>チャージスラブ重量</returns>
        public string GetSlabWeight(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "SlabWeight");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 抽出日の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>抽出日</returns>
        public string GetExtractionYMD(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "ExtractionYMD");
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }

    /// <summary>
    /// PQGA420Bファイルのリーダクラス(Text用)
    /// </summary>
    public class PQGA420BTextFileReader : TextFileReaderBase, IPQGA420BReader
    {

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PQGA420BTextFileReader() : base()
        {
        }

        #endregion

        #region メソッド

        /// <summary>
        /// 読込んだファイルの妥当性の検証
        /// </summary>
        public override void Validate()
        {
            if (document.RecordGroups.Count == 0)
                throw new FileNoDataException();
        }

        /// <summary>
        /// 受注№の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>受注№</returns>
        public string GetOrderNo(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "OrderNo");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 需要家コードの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>需要家コード</returns>
        public string GetCustomerCode(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "CustomerCode");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 需要家名の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>需要家名</returns>
        public string GetCustomerName(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "CustomerName");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 商社コードの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>商社コード</returns>
        public string GetDisutributorCode(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "DisutributorCode");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 都市の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>都市</returns>
        public string GetCity(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "City");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 回収の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>回収</returns>
        public string GetCollection(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "Collection");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 品種の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>品種</returns>
        public string GetKind(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "Kind");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 規格コードの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>規格コード</returns>
        public string GetStandardCode(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "StandardCode");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 決済条件の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>決済条件</returns>
        public string GetSettlementCondition(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "SettlementCondition");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 商社名の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>商社名</returns>
        public string GetDisutributorName(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "DisutributorName");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 規格名の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>規格名</returns>
        public string GetStandardName(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "StandardName");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// スラブサイズの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>スラブサイズ</returns>
        public string GetSlabSize(int recordIndex)
        {
            try
            {
                return (document.GetStringValue("Detail", recordIndex, "Atu").TrimStartZeroWithConvertToInt()).ToString()
                     + Strings.Space(1)
                     + document.GetStringValue("Detail", recordIndex, "EX1")
                     + Strings.Space(1)
                     + (document.GetStringValue("Detail", recordIndex, "Haba").TrimStartZeroWithConvertToInt()).ToString()
                     + Strings.Space(1)
                     + document.GetStringValue("Detail", recordIndex, "EX2")
                     + Strings.Space(1)
                     + (document.GetStringValue("Detail", recordIndex, "Naga").TrimStartZeroWithConvertToInt()).ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// スラブサイズ(厚)の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>スラブサイズ(厚)</returns>
        public string GetSlabAtu(int recordIndex)
        {
            try
            {
                return (document.GetStringValue("Detail", recordIndex, "Atu").TrimStartZeroWithConvertToInt()).ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// スラブサイズ(巾)の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>スラブサイズ(巾)</returns>
        public string GetSlabHaba(int recordIndex)
        {
            try
            {
                return (document.GetStringValue("Detail", recordIndex, "Haba").TrimStartZeroWithConvertToInt()).ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// スラブサイズ(長)の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>スラブサイズ(長)</returns>
        public string GetSlabNaga(int recordIndex)
        {
            try
            {
                return (document.GetStringValue("Detail", recordIndex, "Naga").TrimStartZeroWithConvertToInt()).ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 本数の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>本数</returns>
        public string GetHonsuu(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "Honsuu");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 重量の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>重量</returns>
        public string GetWeight(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "Weight");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 納期の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>納期</returns>
        public string GetDeliveryDate(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "DeliveryDate");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ランクの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ランク</returns>
        public string GetDeliveryDateRank(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "DeliveryDateRank");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 受渡し条件の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>受渡し条件</returns>
        public string GetTransferCondition(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "TransferCondition");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 抽出日の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>抽出日</returns>
        public string GetExtractionYMD(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "ExtractionYMD");
            }

            catch (Exception)
            {
                throw;
            }
        }


        #endregion
    }

    /// <summary>
    /// PQGA820Bファイルのリーダクラス(Text用)
    /// </summary>
    public class PQGA820BTextFileReader : TextFileReaderBase, IPQGA820BReader
    {

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PQGA820BTextFileReader() : base()
        {
        }

        #endregion

        #region メソッド

        /// <summary>
        /// 読込んだファイルの妥当性の検証
        /// </summary>
        public override void Validate()
        {
            if (document.RecordGroups.Count == 0)
                throw new FileNoDataException();
        }

        /// <summary>
        /// 出荷日の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>出荷日</returns>
        public string GetShipmentYYMMDD(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "ShipmentYYMMDD");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ZK081本数の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ZK081本数</returns>
        public string GetZK081Honsuu(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "ZK081Honsuu");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ZK081重量の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ZK081重量</returns>
        public string GetZK081Weight(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "ZK081Weight");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ZK085本数の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ZK085本数</returns>
        public string GetZK085Honsuu(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "ZK085Honsuu");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ZK085重量の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ZK085重量</returns>
        public string GetZK085Weight(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "ZK085Weight");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ZK121本数の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ZK121本数</returns>
        public string GetZK121Honsuu(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "ZK121Honsuu");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ZK121重量の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ZK121重量</returns>
        public string GetZK121Weight(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "ZK121Weight");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ZK172本数の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ZK172本数</returns>
        public string GetZK172Honsuu(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "ZK172Honsuu");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ZK172重量の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ZK172重量</returns>
        public string GetZK172Weight(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "ZK172Weight");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ＳＰＨＣ本数の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ＳＰＨＣ本数</returns>
        public string GetSPHCHonsuu(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "SPHCHonsuu");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ＳＰＨＣ重量の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ＳＰＨＣ重量</returns>
        public string GetSPHCWeight(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "SPHCWeight");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ＳＳ400本数の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ＳＳ400本数</returns>
        public string GetSS400Honsuu(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "SS400Honsuu");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ＳＳ400重量の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ＳＳ400重量</returns>
        public string GetSS400Weight(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "SS400Weight");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// その他本数の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>その他本数</returns>
        public string GetOtherHonsuu(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "OtherHonsuu");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// その他重量の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>その他重量</returns>
        public string GetOtherWeight(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "OtherWeight");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 合計本数の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>合計本数</returns>
        public string GetTotalHonsuu(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "TotalHonsuu");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 合計重量の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>合計重量</returns>
        public string GetTotalWeight(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "TotalWeight");
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }

    /// <summary>
    /// SSYM040Bファイルのリーダクラス(Text用)
    /// </summary>
    public class SSYM040BTextFileReader : TextFileReaderBase, ISSYM040BReader
    {

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SSYM040BTextFileReader() : base()
        {
        }

        #endregion

        #region メソッド

        /// <summary>
        /// 読込んだファイルの妥当性の検証
        /// </summary>
        public override void Validate()
        {
            if (document.RecordGroups.Count == 0)
                throw new FileNoDataException();
        }

        /// <summary>
        /// 年月日の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>年月日</returns>
        public string GetYYYYMMDD(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "Year")
                     + document.GetStringValue("Detail", recordIndex, "YearSlash")
                     + document.GetStringValue("Detail", recordIndex, "Month")
                     + document.GetStringValue("Detail", recordIndex, "MonthSlash")
                     + document.GetStringValue("Detail", recordIndex, "Day");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 中板屑の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>中板屑</returns>
        public string GetMiddlePlateWaste(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "MiddlePlateWaste");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 日清リターン屑の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>日清リターン屑</returns>
        public string GetNissinReturnWaste(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "NissinReturnWaste");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// トリーマ屑の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>トリーマ屑</returns>
        public string GetTrimmerWaste(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "TrimmerWaste");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 厚板ライン屑の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>厚板ライン屑</returns>
        public string GetAtuItaLineWaste(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "AtuItaLineWaste");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// プレーナ屑の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>プレーナ屑</returns>
        public string GetPlanerWaste(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "PlanerWaste");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// レーザー屑の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>レーザー屑</returns>
        public string GetLaserWaste(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "LaserWaste");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// プレーナ知多屑の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>プレーナ知多屑</returns>
        public string GetPlanerChitaWaste(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "PlanerChitaWaste");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ミスロール屑の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ミスロール屑</returns>
        public string GetMissRollWaste(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "MissRollWaste");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// コラム返品屑の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>コラム返品屑</returns>
        public string GetColumuReturnWaste(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "ColumuReturnWaste");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 合計の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>合計</returns>
        public string GetTotal(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "Total");
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }

    /// <summary>
    /// SSYM050Bファイルのリーダクラス(Text用)
    /// </summary>
    public class SSYM050BTextFileReader : TextFileReaderBase, ISSYM050BReader
    {

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SSYM050BTextFileReader() : base()
        {
        }

        #endregion

        #region メソッド

        /// <summary>
        /// 読込んだファイルの妥当性の検証
        /// </summary>
        public override void Validate()
        {
            if (document.RecordGroups.Count == 0)
                throw new FileNoDataException();
        }

        /// <summary>
        /// 年月日の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>年月日</returns>
        public string GetYYYYMMDD(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "Year")
                     + document.GetStringValue("Detail", recordIndex, "YearSlash")
                     + document.GetStringValue("Detail", recordIndex, "Month")
                     + document.GetStringValue("Detail", recordIndex, "MonthSlash")
                     + document.GetStringValue("Detail", recordIndex, "Day");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// エンドシャー屑の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>エンドシャー屑</returns>
        public string GetEndShearWaste(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "EndShearWaste");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// プレーナ屑の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>プレーナ屑</returns>
        public string GetPlanerWaste(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "PlanerWaste");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// トリーマ屑の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>トリーマ屑</returns>
        public string GetTrimmerWaste(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "TrimmerWaste");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// レーザー屑の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>レーザー屑</returns>
        public string GetLaserWaste(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "LaserWaste");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// その他屑の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>その他屑</returns>
        public string GetOtherWaste(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "OtherWaste");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 合計の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>合計</returns>
        public string GetTotal(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "Total");
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }

    /// <summary>
    /// SSZA040Bファイルのリーダクラス(Text用)
    /// </summary>
    public class SSZA040BTextFileReader : TextFileReaderBase, ISSZA040BReader
    {

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SSZA040BTextFileReader() : base()
        {
        }

        #endregion

        #region メソッド

        /// <summary>
        /// 読込んだファイルの妥当性の検証
        /// </summary>
        public override void Validate()
        {
            if (document.RecordGroups.Count == 0)
                throw new FileNoDataException();
        }

        /// <summary>
        /// 受注№の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>受注№</returns>
        public string GetOrderNo(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "OrderNo");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 規格名の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>規格名</returns>
        public string GetStandardName(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "StandardName");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 製品サイズの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>製品サイズ</returns>
        public string GetProductSize(int recordIndex)
        {
            try
            {
                return (document.GetStringValue("Detail", recordIndex, "ProductAtu").TrimStartZeroWithConvertToInt()).ToString()
                     + Strings.Space(1)
                     + document.GetStringValue("Detail", recordIndex, "ProductX1")
                     + Strings.Space(1)
                     + (document.GetStringValue("Detail", recordIndex, "ProductHaba").TrimStartZeroWithConvertToInt()).ToString()
                     + Strings.Space(1)
                     + document.GetStringValue("Detail", recordIndex, "ProductX2")
                     + Strings.Space(1)
                     + (document.GetStringValue("Detail", recordIndex, "ProductNaga").TrimStartZeroWithConvertToInt()).ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 製品(厚)の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>製品(厚)</returns>
        public string GetProductAtu(int recordIndex)
        {
            try
            {
                var rollAtu = (float)(document.GetIntegerValue("Detail", recordIndex, "ProductAtu")) / 100;
                return rollAtu.ToString("#0.00");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 製品(巾)の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>製品(巾)</returns>
        public string GetProductHaba(int recordIndex)
        {
            try
            {
                return (document.GetStringValue("Detail", recordIndex, "ProductHaba").TrimStartZeroWithConvertToInt()).ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 製品(長)の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>製品(長)</returns>
        public string GetProductNaga(int recordIndex)
        {
            try
            {
                return (document.GetStringValue("Detail", recordIndex, "ProductNaga").TrimStartZeroWithConvertToInt()).ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 定耳の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>定耳</returns>
        public string GetTM(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "TM");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 製品コードの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>製品コード</returns>
        public string GetProductCode(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "ProductCode");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 需要家名の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>需要家名</returns>
        public string GetCustomerName(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "CustomerName");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 受注枚数の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>受注枚数</returns>
        public string GetOrderCount(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "OrderCount");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 受注重量の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>受注重量</returns>
        public string GetOrderWeight(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "OrderWeight");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 出荷枚数の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>出荷枚数</returns>
        public string GetShipmentCount(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "ShipmentCount");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 受渡場所名の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>受渡場所名</returns>
        public string GetTransferPlaceName(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "TransferPlaceName");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 受渡条件の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>受渡条件</returns>
        public string GetTransferCondition(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "TransferCondition");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 納期の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>納期</returns>
        public string GetDeliveryDate(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "DeliveryDate");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 納期ランクの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>納期ランク</returns>
        public string GetDeliveryDateRank(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "DeliveryDateRank");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 引当の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>引当</returns>
        public string GetProvisionClassfication(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "ProvisionClassfication");
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 抽出日の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>抽出日</returns>
        public string GetExtractionYMD(int recordIndex)
        {
            try
            {
                return document.GetStringValue("Detail", recordIndex, "ExtractionYMD");
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
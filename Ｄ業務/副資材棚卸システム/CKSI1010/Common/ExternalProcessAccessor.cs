using CKSI1010.Shared;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.XPath;
using static CKSI1010.Common.Constants;
//*************************************************************************************
//
//   外部プロセスアクセッサー
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************
namespace CKSI1010.Common
{
    /// <summary>
    /// 外部プロセスアクセッサー
    /// </summary>
    internal class ExternalProcessAccessor
    {
        /// <summary>
        /// 資材棚卸システム
        /// </summary>
        private const string SIZAI_TANAOROSI_SYSTEM = "1";

        /// <summary>
        /// 資材ユーザ
        /// </summary>
        private const string SIZAI_USER = "SIZA";

        /// <summary>
        /// 資材作業誌入力(入庫)
        /// </summary>
        private const string SAGYOSI_NYUKO = "1";

        /// <summary>
        /// 資材作業誌入力(出庫)
        /// </summary>
        private const string SAGYOSI_SHUKKO = "2";

        /// <summary>
        /// 資材班作業誌入力のプロセス名
        /// </summary>
        private const string SIZAI_SAGYOSI_NYURYOKU_PROCESS_NAME = "資材班作業誌入力";

        /// <summary>
        /// 経理報告データ作成のプロセス名
        /// </summary>
        private const string KEIRI_HOUKOKU_DATA_PROCESS_NAME = "経理報告データ作成";

        /// <summary>
        /// 検収明細書作成のプロセス名
        /// </summary>
        private const string KENSYU_MEISAISHO_PROCESS_NAME = "検収明細書作成";

        /// <summary>
        /// 購買検収データ作成のプロセス名
        /// </summary>
        private const string KOBAI_KENSYU_DATA_SAKUSEI_PROCESS_NAME = "購買検収データ作成";

        /// <summary>
        /// 資材班作業誌入力を起動する
        /// </summary>
        /// <param name="dataType">検針データ(EXCEL)のデータの種類</param> 
        /// <param name="filePath">検針データ(EXCEL)のファイルパス</param>
        internal static void StartSizaiSagyosiNyuryoku(KensinDataType dataType, string kensinExcelFilePath)
        {
            string path = string.Empty;
            string name = SIZAI_SAGYOSI_NYURYOKU_PROCESS_NAME;

            try
            {
                var settings = Path.Combine(Path.GetDirectoryName(
                        Assembly.GetExecutingAssembly().Location) ?? string.Empty, "Settings.xml");
                var doc = XDocument.Load(settings);

                var define = doc.XPathSelectElement($"/Settings/ApplicationDefinitions/ApplicationDefinition[@Name='{name}']");
                if (define != null)
                {
                    path = define.Attribute("Path").Value;

                    ProcessStartInfo processStartInfo = new ProcessStartInfo();
                    processStartInfo.FileName = path;
                    processStartInfo.WindowStyle = ProcessWindowStyle.Maximized;

                    // コマンドライン引数を指定
                    // 社員コード
                    processStartInfo.Arguments += SIZAI_USER;
                    // 所属コード 
                    processStartInfo.Arguments += "," + SharedModel.Instance.StartupArgs.PositionCode;
                    // 職位コード
                    processStartInfo.Arguments += "," + SharedModel.Instance.StartupArgs.BelongingCode;
                    // 液酸入庫 / 出庫パラメータ
                    var param = dataType.Equals(KensinDataType.Nyuko) ? SAGYOSI_NYUKO : SAGYOSI_SHUKKO;
                    processStartInfo.Arguments += "," + param;
                    // 検針データ(EXCEL)のファイルパス
                    processStartInfo.Arguments += "," + kensinExcelFilePath;
                    // 棚卸実施年月
                    processStartInfo.Arguments += "," + SharedViewModel.Instance.OperationYearMonth.YearMonth;

                    // 起動
                    Process p = Process.Start(processStartInfo);
                    WindowOperator.ForegroundWindow(p);
                    p.WaitForExit();

                }
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 経理報告データ作成を起動する
        /// </summary>
        internal static void StartKeiriHoukokuData()
        {
            string path = string.Empty;
            string fileName = KEIRI_HOUKOKU_DATA_PROCESS_NAME;

            try
            {
                var settings = Path.Combine(Path.GetDirectoryName(
                              Assembly.GetExecutingAssembly().Location) ?? string.Empty, "Settings.xml");
                var doc = XDocument.Load(settings);

                var define = doc.XPathSelectElement($"/Settings/ApplicationDefinitions/ApplicationDefinition[@Name='{fileName}']");
                if (define != null)
                {
                    path = define.Attribute("Path").Value;

                    ProcessStartInfo processStartInfo = new ProcessStartInfo();
                    processStartInfo.FileName = path;
                    processStartInfo.WindowStyle = ProcessWindowStyle.Maximized;

                    // コマンドライン引数を指定
                    // 社員コード
                    processStartInfo.Arguments += SIZAI_USER;
                    // 所属コード 
                    processStartInfo.Arguments += "," + SharedModel.Instance.StartupArgs.PositionCode;
                    // 職位コード
                    processStartInfo.Arguments += "," + SharedModel.Instance.StartupArgs.BelongingCode;
                    // 呼出し元が副資材棚卸システム
                    processStartInfo.Arguments += "," + SIZAI_TANAOROSI_SYSTEM;
                    // 棚卸実施年月
                    processStartInfo.Arguments += "," + SharedViewModel.Instance.OperationYearMonth.YearMonth;

                    // 起動
                    Process p = Process.Start(processStartInfo);
                    WindowOperator.ForegroundWindow(p);
                    p.WaitForExit();

                }
            }

            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// 検収明細書作成を起動する
        /// </summary>
        internal static void StartKenshuMeisaishoSakusei()
        {
            string path = string.Empty;
            string fileName = KENSYU_MEISAISHO_PROCESS_NAME;

            try
            {
                var settings = Path.Combine(Path.GetDirectoryName(
                              Assembly.GetExecutingAssembly().Location) ?? string.Empty, "Settings.xml");
                var doc = XDocument.Load(settings);

                var define = doc.XPathSelectElement($"/Settings/ApplicationDefinitions/ApplicationDefinition[@Name='{fileName}']");
                if (define != null)
                {
                    path = define.Attribute("Path").Value;

                    ProcessStartInfo processStartInfo = new ProcessStartInfo();
                    processStartInfo.FileName = path;
                    processStartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                    // コマンドライン引数を指定

                    // 社員コード
                    processStartInfo.Arguments += SIZAI_USER;
                    // 所属コード 
                    processStartInfo.Arguments += "," + SharedModel.Instance.StartupArgs.PositionCode;
                    // 職位コード
                    processStartInfo.Arguments += "," + SharedModel.Instance.StartupArgs.BelongingCode;
                    // 呼出し元が副資材棚卸システム
                    processStartInfo.Arguments += "," + SIZAI_TANAOROSI_SYSTEM;

                    // 起動
                    Process p = Process.Start(processStartInfo);
                    WindowOperator.ForegroundWindow(p);
                    p.WaitForExit();

                }
            }

            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// 購買検収データ作成を起動する
        /// </summary>
        internal static void StartKobaiKenshuDataSakusei()
        {
            string path = string.Empty;
            string fileName = KOBAI_KENSYU_DATA_SAKUSEI_PROCESS_NAME;

            try
            {
                var settings = Path.Combine(Path.GetDirectoryName(
                              Assembly.GetExecutingAssembly().Location) ?? string.Empty, "Settings.xml");
                var doc = XDocument.Load(settings);

                var define = doc.XPathSelectElement($"/Settings/ApplicationDefinitions/ApplicationDefinition[@Name='{fileName}']");
                if (define != null)
                {
                    path = define.Attribute("Path").Value;

                    ProcessStartInfo processStartInfo = new ProcessStartInfo();
                    processStartInfo.FileName = path;
                    processStartInfo.WindowStyle = ProcessWindowStyle.Maximized;

                    // コマンドライン引数を指定
                    // 社員コード
                    processStartInfo.Arguments += SIZAI_USER;
                    // 所属コード 
                    processStartInfo.Arguments += "," + SharedModel.Instance.StartupArgs.PositionCode;
                    // 職位コード
                    processStartInfo.Arguments += "," + SharedModel.Instance.StartupArgs.BelongingCode;
                    // 呼出し元が副資材棚卸システム
                    processStartInfo.Arguments += "," + SIZAI_TANAOROSI_SYSTEM;

                    // 起動
                    Process p = Process.Start(processStartInfo);
                    WindowOperator.ForegroundWindow(p);
                    p.WaitForExit();

                }
            }

            catch (Exception)
            {
                throw;
            }

        }
    }
}

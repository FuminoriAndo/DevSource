using CKSI1010.Core;
using CKSI1010.Shared;
using Common;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using static CKSI1010.Common.Constants;
using static Common.MessageManager;
//*************************************************************************************
//
//   ユーティリティークラス
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
    /// ユーティリティークラス
    /// </summary>
    internal class CommonUtility
    {
        #region メソッド

        /// <summary>
        /// 確定状態にする
        /// </summary>
        /// <param name="operateName">操作名</param>
        internal static void ExcuteFixStatus(string operateName)
        {
            try
            {
                int index = SharedViewModel.Instance.Operations.FirstOrDefault(item => (item.Title == operateName)).Index;
                SharedViewModel.Instance.Operations[index].IsFixed = true;
                SharedViewModel.Instance.Operations[index].IsModified = false;
            }

            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// 修正状態にする
        /// </summary>
        /// <param name="operateName">操作名</param>
        internal static void ExcuteModifyStatus(string operateName)
        {
            try
            {
                int index = SharedViewModel.Instance.Operations.FirstOrDefault(item => (item.Title == operateName)).Index;
                SharedViewModel.Instance.Operations[index].IsFixed = false;
                SharedViewModel.Instance.Operations[index].IsModified = true;

                // 以降の作業を未完了状態にする
                foreach (var operation in SharedViewModel.Instance.Operations)
                {
                    if (index < operation.Index)
                    {
                        SharedViewModel.Instance.Operations[operation.Index].IsFixed = false;
                        SharedViewModel.Instance.Operations[operation.Index].IsModified = true;
                    }
                }
            }

            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// 操作コードを取得する
        /// </summary>
        /// <param name="operateName">操作名</param>
        internal static int GetOperationCode(string operateName)
        {
            int result = 0;

            try
            {
                int index = SharedViewModel.Instance.Operations.FirstOrDefault(item => (item.Title == operateName)).Index;
                result = SharedViewModel.Instance.Operations[index].Code;
            }

            catch (Exception)
            {
                throw;
            }

            return result;

        }

        /// <summary>
        /// 操作順を取得する
        /// </summary>
        /// <param name="operateName">操作名</param>
        internal static int GetOperationIndex(string operateName)
        {
            int result = 0;

            try
            {
                result = SharedViewModel.Instance.Operations.FirstOrDefault(item => (item.Title == operateName)).Index + 1;
            }

            catch (Exception)
            {
                throw;
            }

            return result;

        }

        /// <summary>
        /// 現在の操作状態を取得する
        /// </summary>
        /// <returns>操作の状態</returns>
        internal static OperationCondition GetCurrentOperationStatus()
        {
            OperationCondition result = OperationCondition.Modify;

            try
            {
                bool isFixed = SharedViewModel.Instance.CurrentOperation.IsFixed;
                if (isFixed) result = OperationCondition.Fix;
            }

            catch (Exception)
            {
                throw;
            }

            return result;
        }

        /// <summary>
        /// 現在の操作状態の値を取得する
        /// </summary>
        /// <returns>操作状態の値</returns>
        internal static string GetCurrentOperationStatusValue()
        {
            string result = OperationCondition.Modify.GetStringValue();

            try
            {
                bool isFixed = SharedViewModel.Instance.CurrentOperation.IsFixed;
                if (isFixed) result = OperationCondition.Fix.GetStringValue();
            }

            catch (Exception)
            {
                throw;
            }

            return result;
        }

        /// <summary>
        /// 現在の操作が確定状態か
        /// </summary>
        /// <returns>結果</returns>
        internal static bool IsCurrentOperationFixed()
        {
            bool result = false;

            try
            {
                result = SharedViewModel.Instance.CurrentOperation.IsFixed;
            }

            catch (Exception)
            {
                throw;
            }

            return result;
        }

        /// <summary>
        /// 操作の確定状態を取得する
        /// </summary>
        /// <param name="operateName">操作名</param>
        internal static bool GetOperationFixedStatus(string operateName)
        {
            bool result = false;

            try
            {
                int index = SharedViewModel.Instance.Operations.FirstOrDefault(item => (item.Title == operateName)).Index;
                result = SharedViewModel.Instance.Operations[index].IsFixed;
            }

            catch (Exception)
            {
                throw;
            }

            return result;

        }

        /// <summary>
        /// 現在の操作を確定状態にする
        /// </summary>
        internal static void ExcuteCurrentOperaionFixed()
        {
            try
            {
                int index = SharedViewModel.Instance.CurrentOperation.Index;
                SharedViewModel.Instance.Operations[index].IsFixed = true;
                SharedViewModel.Instance.Operations[index].IsModified = false;
            }

            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// 現在の操作を修正状態にする
        /// </summary>
        internal static void ExcuteCurrentOperaionModify()
        {
            try
            {
                int index = SharedViewModel.Instance.CurrentOperation.Index;
                SharedViewModel.Instance.Operations[index].IsFixed = false;
                SharedViewModel.Instance.Operations[index].IsModified = true;

                // 以降の作業を未完了状態にする
                foreach (var operation in SharedViewModel.Instance.Operations)
                {
                    if (index < operation.Index)
                    {
                        SharedViewModel.Instance.Operations[operation.Index].IsFixed = false;
                        SharedViewModel.Instance.Operations[operation.Index].IsModified = true;
                    }
                }
            }

            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// 現在の操作の操作順を取得する
        /// </summary>
        internal static int GetCurrentOperaionOrder()
        {
            int resuult = -1;

            try
            {
                resuult = SharedViewModel.Instance.CurrentOperation.Index + 1;
            }

            catch (Exception)
            {
                throw;
            }

            return resuult;
        }

        /// <summary>
        /// 現在の操作の作業区分を取得する
        /// </summary>
        internal static string GetCurrentOperationWorkKbn()
        {
            string result = string.Empty;

            try
            {
                result = SharedViewModel.Instance.CurrentOperation.WorkType;
            }

            catch (Exception)
            {
                throw;
            }

            return result;
        }

        /// <summary>
        /// 作業区分を取得する
        /// </summary>
        /// <param name="operateName">操作名</param>
        internal static string GetWorkKbn(string operateName)
        {
            string result = null;

            try
            {
                int index = SharedViewModel.Instance.Operations.FirstOrDefault(item => (item.Title == operateName)).Index;
                result = SharedViewModel.Instance.Operations[index].WorkType;
            }

            catch (Exception)
            {
                throw;
            }

            return result;

        }

        /// <summary>
        /// 現在の操作を操作不可にする
        /// </summary>
        internal static void ExcuteCanNotOperation()
        {
            try
            {
                int index = SharedViewModel.Instance.CurrentOperation.Index;
                SharedViewModel.Instance.Operations[index].CanOperate = false;
            }

            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// 指定のバイト数になるまで末尾を空白文字で埋める
        /// </summary>
        /// <param name="str">対象文字列</param>
        /// <param name="length">バイト数</param>
        /// <returns>末尾を空白文字で埋めた後の文字列</returns>
        internal static string PadRightSJIS(string str, int byteLength)
        {
            var enc = Encoding.GetEncoding("Shift_JIS");
            var strLen = enc.GetByteCount(str);
            if (strLen < byteLength)
            {
                return str + new string(' ', byteLength - strLen);
            }
            return str;
        }

        /// <summary>
        /// SQL操作で例外発生時のSQLを出力する
        /// </summary>
        /// <param name="timeStamp">タイムスタンプ</param>
        /// <param name="sql">SQL</param>
        /// <param name="exception">例外内容</param>
        /// <param name="errorCode">エラーコード</param>
        /// <param name="sqlParam">SQLパラメータ</param>
        internal static void WriteLogExceptionSQL(string timeStamp, string sql, string exception, string errorCode, string sqlParam)
        {
            string filePath = null;
            string fileName = null;

            try
            {
                // XMLファイルのパスの取得
                string xmlPath = Path.Combine(Path.GetDirectoryName(
                Assembly.GetExecutingAssembly().Location) ?? string.Empty, "Settings.xml");

                // XMLファイルを読込む
                var doc = XDocument.Load(xmlPath);
                var elem = doc.XPathSelectElement($"/Settings/ExceptionSQL/File");
                filePath = (string)elem.Attribute("Path");
                fileName = (string)elem.Attribute("Name");

                Encoding sjisEnc = Encoding.GetEncoding("Shift_JIS");
                StreamWriter writer =
                new StreamWriter(@filePath + "/" + fileName, true, sjisEnc);
                // タイムスタンプ
                // 例外内容
                // エラーコード
                // SQL
                writer.WriteLine(timeStamp + " エラー内容：" + exception);
                writer.WriteLine(timeStamp + " SQL：" + sql);
                writer.WriteLine(timeStamp + " パラメータ：" + sqlParam);
                writer.Close();

                MessageManager.ShowError(SystemID.CKSI1010, "HasDBError", @filePath + "\\" + fileName);
            }

            catch (Exception)
            {
                throw;
            }
            
        }

        /// <summary>
        /// 共有データで保持している印刷対象外受払い情報の初期化
        /// </summary>
        internal static void InitializeExcludeInputPaymentInfo()
        {
            try
            {
                // 共有データで保持している印刷対象外の使用高払い受払い情報のクリア
                foreach (var info in SharedModel.Instance.ExcludeUsePaymentInfo)
                {
                    info.Value.Clear();
                }

                SharedModel.Instance.IsUsePaymentSelectInitialized = true;
                SharedModel.Instance.IsUsePaymentSelectAll = true;

                // 共有データで保持している印刷対象外の入庫払い受払い情報のクリア
                foreach (var info in SharedModel.Instance.ExcludeInputPaymentInfo)
                {
                    info.Value.Clear();
                }

                SharedModel.Instance.IsInputPaymentSelectInitialized = true;
                SharedModel.Instance.IsInputPaymentSelectAll = true;
            }

            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// 棚卸ログ用のフォルダを作成する
        /// </summary>
        internal static void CreateTanaorosiLogFolder()
        {
            string path = string.Empty;

            try
            {
                // XMLファイルのパスの取得
                string xmlPath = Path.Combine(Path.GetDirectoryName(
                Assembly.GetExecutingAssembly().Location) ?? string.Empty, "Settings.xml");

                // XMLファイルを読込む
                var doc = XDocument.Load(xmlPath);
                var elem = doc.XPathSelectElement($"/Settings/ExceptionSQL/File");
                path = (string)elem.Attribute("Path");

                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 棚卸ログ用のフォルダの存在チェック
        /// </summary>
        /// <returns>結果</returns>
        internal static bool ExistTanaorosiLogFolder()
        {
            bool ret = false;
            string path = string.Empty;

            try
            {
                // XMLファイルのパスの取得
                string xmlPath = Path.Combine(Path.GetDirectoryName(
                Assembly.GetExecutingAssembly().Location) ?? string.Empty, "Settings.xml");

                // XMLファイルを読込む
                var doc = XDocument.Load(xmlPath);
                var elem = doc.XPathSelectElement($"/Settings/ExceptionSQL/File");
                path = (string)elem.Attribute("Path");

                if (Directory.Exists(path)) ret = true;
                return ret;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 資材棚卸Exceブックのパスの取得
        /// </summary>
        /// <returns>結果</returns>
        internal static  string GetSizaiTanaorosiExcelBookPath()
        {
            string filePath = string.Empty;
            string fileName = string.Empty; 

            try
            {
                // XMLファイルのパスの取得
                string xmlPath = Path.Combine(Path.GetDirectoryName(
                Assembly.GetExecutingAssembly().Location) ?? string.Empty, "Settings.xml");

                // XMLファイルを読込む
                var doc = XDocument.Load(xmlPath);
                var elem = doc.XPathSelectElement($"/Settings/Excel/Template");
                filePath = (string)elem.Attribute("Path");
                fileName = (string)elem.Attribute("Name");

                return filePath + fileName;
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

namespace Project1
{
    //*************************************************************************************
    //
    //
    //   プログラム名　　　まいと～くCSV連携クラス
    //
    //
    //   修正履歴
    //
    //   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
    //   13.07.19             HIT桑島     新規作成
    //
    //*************************************************************************************
    class C_FAXCSV
    {
        /// <summary>
        /// Faxデータクラス
        /// </summary>
        public class FaxData
        {
            public enum InterFaxFlagValue
            {
                True, 
                False
            }

            public string GenkoFileName = string.Empty;
            public string FaxNo = string.Empty;
            public string Name = string.Empty;
            public string Keisyo = string.Empty;
            public string Kaisyamei = string.Empty;
            public string Syozoku = string.Empty;
            public string Yakusyoku = string.Empty;
            public string Tel = string.Empty;
            public string FCode = string.Empty;
            public string MailAdress = string.Empty;
            public InterFaxFlagValue InterFaxFlag = InterFaxFlagValue.False;
            public string MailSubject = string.Empty;
            public string MailBody = string.Empty;
            public string Soufujyo = string.Empty;
            public string SoufuMemo = string.Empty;
            public string Port = string.Empty;
            public string UserData = string.Empty;
        }

        private ArrayList _faxDatas = new ArrayList();
        private string _DataPath;
        private string _CsvPath;
        private string _ClassId;
        private const string VERSIONINFO = "V200";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="csvPath"></param>
        public C_FAXCSV(string classid, string csvPath)
        {
            this._ClassId = classid;
            this._CsvPath = csvPath;
        }

        /// <summary>
        /// 添付ファイルの保存フォルダ
        /// </summary>
        public string DataPath
        {
            set { this._DataPath = value; }
        }

        /// <summary>
        /// 送信データ追加する
        /// </summary>
        /// <param name="faxData"></param>
        public void AddFaxData(FaxData faxData)
        {
            this._faxDatas.Add(faxData);
        }

        /// <summary>
        /// CSV連携によるFAX送信を実行
        /// </summary>
        /// <returns></returns>
        public bool FaxSend()
        {
            if (this._faxDatas.Count == 0)
            {
                return true;
            }

            try
            {
                string tmpFile = this._CsvPath + "\\" + this._ClassId + "_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".tmp";
                string csvFile = this._CsvPath + "\\" + this._ClassId + "_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".csv";
                System.Text.Encoding enc = System.Text.Encoding.GetEncoding("Shift_JIS");
                StreamWriter sr = new StreamWriter(tmpFile, false, enc);

                foreach (FaxData fd in this._faxDatas)
                {
                    StringBuilder rec = new StringBuilder();
                    rec.Append("\"" + VERSIONINFO + "\",");
                    rec.Append("\"" + fd.GenkoFileName + "\",");
                    rec.Append("\"" + fd.FaxNo + "\",");
                    rec.Append("\"" + fd.Name + "\",");
                    rec.Append("\"" + fd.Keisyo + "\",");
                    rec.Append("\"" + fd.Kaisyamei + "\",");
                    rec.Append("\"" + fd.Syozoku + "\",");
                    rec.Append("\"" + fd.Yakusyoku + "\",");
                    rec.Append("\"" + fd.Tel + "\",");
                    rec.Append("\"" + fd.FCode + "\",");
                    rec.Append("\"" + fd.MailAdress + "\",");
                    if (fd.InterFaxFlag == FaxData.InterFaxFlagValue.False)
                    {
                        rec.Append("\"0\",");
                    }
                    else
                    {
                        rec.Append("\"1\",");
                    }
                    rec.Append("\"" + fd.MailSubject + "\",");
                    rec.Append("\"" + fd.MailBody + "\",");
                    rec.Append("\"" + fd.Soufujyo + "\",");
                    rec.Append("\"" + fd.SoufuMemo + "\",");
                    rec.Append("\"" + fd.Port + "\",");
                    rec.Append("\"" + fd.UserData + "\"");
                    rec.Append("\r\n");
                    sr.Write(rec.ToString());
                }
                sr.Close();

                //ファイルができたら拡張子をリネームする。
                System.IO.File.Move(tmpFile, csvFile);

                return true;
            }
            catch
            {
                return false;
            }
        }
         
    }
}

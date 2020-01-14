using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//↓2013.08.21 kuwajima 追加要望対応
using CrystalDecisions.CrystalReports.Engine;
using System.Windows.Forms;
using Microsoft.VisualBasic.PowerPacks.Printing.Compatibility.VB6;
//↑2013.08.21 kuwajima 追加要望対応

namespace Project1
{
    //Statusコード
    enum FaxSendStatus
    {
        NONE,       //値未設定
        APPROVING,  //承認待ち
        PERMIT,     //承認済み
        WAITING,    //送信待機中
        SENDING,    //送信中
        NORMAL,     //送信正常終了
        ERROR,      //送信異常終了
        REJECTION   //却下
    };


    //内部使用クラス
    class FaxInfo
    {
        //Fax送信情報
        public int ReceiptNumber;
        public FaxSendStatus Status
        {
            get
            {
                string status = _FaxClient.GetString(
                    _FaxClient.GetSendFaxStatus(this.ReceiptNumber),
                    2);
                FaxSendStatus result;

                if (status == "WAITING")
                {
                    //送信待機中
                    result = FaxSendStatus.WAITING;
                }
                else if (status == "SENDING")
                {
                    //送信中
                    result = FaxSendStatus.SENDING;
                }
                else if (status == "NORMAL")
                {
                    //送信正常終了
                    result = FaxSendStatus.NORMAL;
                }
                else if (status == "ERROR")
                {
                    //送信異常終了
                    result = FaxSendStatus.ERROR;
                }
                else if (status == "APPROVING")
                {
                    //承認待ち
                    result = FaxSendStatus.APPROVING;
                }
                else if (status == "PERMIT")
                {
                    //承認済み
                    result = FaxSendStatus.PERMIT;
                }
                else if (status == "REJECTION")
                {
                    //却下
                    result = FaxSendStatus.REJECTION;
                }
                else
                {
                    result = FaxSendStatus.NONE;
                }

                return result;
            }
        }


        //送信先情報（内部まいと～くあて先設定用）
        private int _CountryCode = 81;  //国コード
        private string _FaxNumber
        {
            set
            {
                if (value != null && value.Length > 3)
                {
                    _FaxAreaNumber = value.Substring(0, 3);
                    _FaxLocalNumber = value.Substring(3, value.Length - 3);
                }
                else
                {
                    _FaxAreaNumber = string.Empty;
                    _FaxLocalNumber = string.Empty;
                }
            }
            get
            {
                return _FaxAreaNumber + _FaxLocalNumber;
            }
        }    //Fax番号
        private string _FaxAreaNumber;  //Fax市外局番
        private string _FaxLocalNumber; //Fax市内局番
        private string _MailAddress;    //E-Mailアドレス
        private bool _InternetFax;      //インターネットファックスを使用
        private string _Name;           //担当者名
        private string _Keisho;         //敬称
        private string _Company;        //会社名
        private string _Section;        //所属名
        private string _Position;       //役職
        private string _PhoneNumber
        {
            set
            {
                _PhoneAreaNumber = value.Substring(0, 3);
                _PhoneLocalNumber = value.Substring(3, value.Length);
            }
            get
            {
                return _FaxAreaNumber + _FaxLocalNumber;
            }
        } //電話番号
        private string _PhoneAreaNumber;//電話市外局番
        private string _PhoneLocalNumber;//電話市内局番
        private string _Fcode;          //Fコード


        //PrintDocumentオブジェクト
        private System.Drawing.Printing.PrintDocument _Document;
        //↓2013.08.21 kuwajima 追加要望対応
        private ReportDocument _CrystalRep = null;
        //↑2013.08.21 kuwajima 追加要望対応

        private AxMCREMOTELib.AxMcRemote _FaxClient;

        /// <summary>
        /// ファックス送信情報
        /// </summary>
        /// <param name="faxClient">まいと～くFaxクライアントオブジェクト</param>
        /// <param name="document">PrintDocumentオブジェクト</param>
        public FaxInfo(AxMCREMOTELib.AxMcRemote faxClient, System.Drawing.Printing.PrintDocument document)
        {
            _FaxClient = faxClient;
            _Document = document;
        }

        //↓2013.08.21 kuwajima 追加要望対応
        /// <summary>
        /// ファックス送信情報
        /// </summary>
        /// <param name="faxClient"></param>
        /// <param name="crystalRep"></param>
        public FaxInfo(AxMCREMOTELib.AxMcRemote faxClient, ReportDocument crystalRep)
        {
            _FaxClient = faxClient;
            _CrystalRep = crystalRep;
        }
        //↑2013.08.21 kuwajima 追加要望対応

        public void SendFax()
        {
            //あて先設定
            this.ReceiptNumber = _FaxClient.SendToV7(_CountryCode, _FaxAreaNumber, _FaxLocalNumber, _MailAddress, _InternetFax,
                _Name, _Keisho, _Company, _Section, _Position, _PhoneAreaNumber, _PhoneLocalNumber, _Fcode);

            _FaxClient.CoverPage(""); //送付状なし


            //↓2013.08.21 kuwajima 追加要望対応
            //_Document.Print();
            if (_CrystalRep == null)
            {
                _Document.Print();
            }
            else
            {
                _CrystalRep.PrintToPrinter(1, false, 0, 0);
            }
            //↑2013.08.21 kuwajima 追加要望対応

            //while (this.Status == FaxSendStatus.NORMAL ||
            //    this.Status == FaxSendStatus.ERROR ||
            //    this.Status == FaxSendStatus.REJECTION)
            //{
                System.Windows.Forms.Application.DoEvents();
            //}

            _FaxClient.ClearSendTo(this.ReceiptNumber);
        }

        public void SetFaxNumber(string companyName, string name, string faxNumber)
        {
            _FaxNumber = faxNumber;
            _Company = companyName;
            _Name = name;
        }

        /// <summary>
        /// メール送信設定
        /// </summary>
        /// <param name="companyName"></param>
        /// <param name="mailAddress"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <param name="attatchFileName"></param>
        public void SetMailAddress(string companyName, string name, string mailAddress, string subject, string message, string attatchFileName)
        {
            _MailAddress = mailAddress;
            _Company = companyName;
            _Name = name;
            _InternetFax = true;

            _FaxClient.InetFaxMessage(subject, message);
            _FaxClient.InetFaxAttachment(attatchFileName, 3);
        }

    }

    /// <summary>
    /// ファックスコントローラークラス
    /// このクラスを作成してFaxを送信する
    /// </summary>
    class FaxController
    {
        private AxMCREMOTELib.AxMcRemote faxClient;
        private List<FaxInfo> faxInfoList;
        private int sentCount = 0;

        /// <summary>
        /// ファックス送信コントローラーインスタンスを作成する
        /// コントローラーに対してSendFax、SendMailを実行する
        /// </summary>
        /// <param name="McRemote1">まいと～くActiveXオブジェクト</param>
        public FaxController(AxMCREMOTELib.AxMcRemote McRemote1)
        {
            faxClient = McRemote1;  //UI側で開いておく
            faxInfoList = new List<FaxInfo>();
            faxClient.ControlPrintStatus += new AxMCREMOTELib._DMcRemoteEvents_ControlPrintStatusEventHandler(faxClient_ControlPrintStatus);
        }

        /// <summary>
        /// ファックス送信
        /// </summary>
        /// <param name="faxNumber">ファックス番号</param>
        /// <param name="companyName">会社名</param>
        /// <param name="name">氏名</param>
        /// <param name="document">PrintDocumentオブジェクト</param>
        public void SendFax(string faxNumber, string companyName, string name, System.Drawing.Printing.PrintDocument document)
        {
            document.PrinterSettings.PrinterName = faxClient.GetPrinterName();

            FaxInfo info = new FaxInfo(this.faxClient, document);
            info.SetFaxNumber(companyName, name, faxNumber);
            faxInfoList.Add(info);

            if (faxInfoList.Count - 1 == sentCount)
            {
                CheckFax();
            }
        }

        //↓2013.08.13 kuwajima 追加要望対応
        /// <summary>
        /// ファックス送信
        /// </summary>
        /// <param name="faxNumber">ファックス番号</param>
        /// <param name="companyName">会社名</param>
        /// <param name="name">氏名</param>
        /// <param name="document">PrintDocumentオブジェクト</param>
        public void SendFax(string faxNumber, string companyName, string name, ReportDocument crystalRep)
        {
            crystalRep.PrintOptions.PrinterName = faxClient.GetPrinterName();

            FaxInfo info = new FaxInfo(this.faxClient, crystalRep);
            info.SetFaxNumber(companyName, name, faxNumber);
            faxInfoList.Add(info);

            if (faxInfoList.Count - 1 == sentCount)
            {
                CheckFax();
            }
        }
        //↑2013.08.13 kuwajima 追加要望対応

        /// <summary>
        /// メール送信
        /// </summary>
        /// <param name="mailAddress"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <param name="attatchFileName"></param>
        /// <param name="companyName"></param>
        /// <param name="document"></param>
        public void SendMail(string mailAddress,
                             string subject,
                             string message,
                             string attatchFileName,
                             string companyName,
                             string name,
                             System.Drawing.Printing.PrintDocument document)
        {
            document.PrinterSettings.PrinterName = faxClient.GetPrinterName();

            FaxInfo info = new FaxInfo(this.faxClient, document);
            info.SetMailAddress(companyName, name, mailAddress, subject, message, attatchFileName);
            faxInfoList.Add(info);

            if (faxInfoList.Count - 1 == sentCount)
            {
                CheckFax();
            }
        }

        //Faxが送信中かどうか調べる
        private void CheckFax()
        {
            if (!faxClient.IsPrinting())
            {
                faxInfoList.ElementAt(sentCount).SendFax();
            }
        }

        //プリント状態を調べるイベントハンドラ
        private void faxClient_ControlPrintStatus(object sender, AxMCREMOTELib._DMcRemoteEvents_ControlPrintStatusEvent e)
        {
            string statusText = e.lpszPrintStatus.Length >= 6 ? e.lpszPrintStatus.Substring(0, 6) : String.Empty;

            if (statusText == "EndDoc")
            {
                faxInfoList.ElementAt(sentCount++);
            }
        }
    }
    class FaxCommon
    {
        /// <summary>
        /// FAXを接続する
        /// </summary>
        /// <param name="McRemote1"></param>
        /// <returns></returns>
        public static bool FAX_CHK(AxMCREMOTELib.AxMcRemote McRemote1)
        {
            bool bFAX = true;
            do
            {
                //FAXサーバー接続
                if (McRemote1.Connect() != 0)
                {
                    //接続成功
                    break;
                }
                else
                {
                    //接続失敗
                    string stMsg = string.Empty;
                    stMsg += "ＦＡＸサーバーへのログインに失敗しました。\n";
                    stMsg += "ＦＡＸサーバーの電源が入っているか確認してください。\n";
                    stMsg += "確認後’はい’ボタンを押してください。\n";
                    stMsg += "’いいえ’でプリンタに出力します。\n";
                    DialogResult dlgRlt = DialogResult.None;
                    dlgRlt = C_COMMON.Msg(stMsg, MessageBoxButtons.YesNo);
                    if (dlgRlt != DialogResult.Yes)
                    {
                        //プリンタ出力
                        bFAX = false;
                        break;
                    }
                }

            } while (true);
            return bFAX;
        }
    }
}

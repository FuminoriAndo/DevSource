using System.Data;
using System.Drawing;
using System.Drawing.Printing;

namespace Project1
{
    class C_PRINT
    {
        /// <summary>
        /// PrintDocumentオブジェクト
        /// </summary>
        private PrintDocument _pd = null;

        /// <summary>
        /// フォント
        /// </summary>
        private Font _font = null;

        /// <summary>
        /// Ｘ座標
        /// </summary>
        private float _fXpitch = 0f;

        /// <summary>
        /// Ｙ座標
        /// </summary>
        private float _fYpitch = 0f;

        public string DocumentName
        {
            get { return this._pd.DocumentName; }
            set { this._pd.DocumentName = value; }
        }

        /// <summary>
        /// 汎用カウンター
        /// </summary>
        private int _nLineCount = 0;
        public int Count
        {
            get { return this._nLineCount; }
            set { this._nLineCount = value; }
        }

        /// <summary>
        /// 汎用DataTable
        /// </summary>
        private DataTable _dtDataTable = null;
        public DataTable Records
        {
            get { return this._dtDataTable; }
            set { this._dtDataTable = value; }
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
            if (this._pd == null)
            {
                this._pd = new PrintDocument();
                this._pd.PrintController = new System.Drawing.Printing.StandardPrintController();  // 2013.04.04 miura mori
            }
            this._nLineCount = 0;
            this._dtDataTable = null;
            this._fXpitch = 1f / 13f;
            this._fYpitch = 1f / 6f;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="sPrtNm">プリンタ名</param>
        /// <param name="sFontNm">フォント名</param>
        /// <param name="nFontSz">フォントサイズ</param>
        /// <param name="nPpSz">用紙サイズ</param>
        /// <param name="nType">印刷方向</param>
        public void Init(string sPrtNm, string sFontNm, int nFontSz, int nPpSz, int nType)
        {
            if (this._pd == null)
            {
                this._pd = new PrintDocument();
                this._pd.PrintController = new System.Drawing.Printing.StandardPrintController();  // 2013.04.04 miura mori
                this.setPrinter(sPrtNm);
                this.setFont(sFontNm, nFontSz);
                this.setPaperSize(nPpSz);
                this.setLandscape(nType);
            }
            this._nLineCount = 0;
            this._dtDataTable = null;
            this._fXpitch = 1f / 13f;
            this._fYpitch = 1f / 6f;
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public void Dispose()
        {
            if (this._pd != null)
            {
                this._pd.Dispose();
                this._pd = null;
            }
            this._nLineCount = 0;
            this._dtDataTable = null;
            this._fXpitch = 0f;
            this._fYpitch = 0f;
        }

        /// <summary>
        /// PrintDocumentオブジェクト取得
        /// </summary>
        /// <returns>PrintDocumentオブジェクト</returns>
        public PrintDocument getObject()
        {
            return this._pd;
        }

        /// <summary>
        /// 印刷開始
        /// </summary>
        public void Print()
        {
            if (this._pd == null)
            {
                return;
            }
            this._pd.Print();
        }
        
        /// <summary>
        /// イベントハンドラ登録
        /// </summary>
        public void setPrintEvent(PrintPageEventHandler hdr)
        {
            if (this._pd == null)
            {
                return;
            }
            this._pd.PrintPage -= new PrintPageEventHandler(hdr);
            this._pd.PrintPage += new PrintPageEventHandler(hdr);
        }

        /// <summary>
        /// 印刷プリンタ名設定
        /// </summary>
        public void setPrinter(string sName)
        {
            // インストール済のプリンタと比較し、登録されているか判定する
            foreach (string sPrinterName in PrinterSettings.InstalledPrinters)
            {
                if (sPrinterName == sName)
                {
                    // 指定したプリンタをシステムの通常使うプリンタに設定する
                    this._pd.PrinterSettings.PrinterName = sName;
                    break;
                }
            }
        }

        /// <summary>
        /// 印刷フォント設定
        /// </summary>
        public void setFont(string sName, int nSize)
        {
            this._font = new Font(sName, nSize);
        }

        /// <summary>
        /// 印刷方向設定
        /// </summary>
        public void setLandscape(int nType)
        {
            if (nType == 2)
            {
                // 2:横
                this._pd.DefaultPageSettings.Landscape = true;
            }
            else
            {
                // 1:縦
                this._pd.DefaultPageSettings.Landscape = false;
            }
        }

        /// <summary>
        /// 印刷用紙設定
        /// </summary>
        public void setPaperSize(int nKind)
        {
            PaperKind pkSetSize = cnvPaperKindPRTtoPRTDC(nKind);

            foreach (PaperSize pkSize in _pd.PrinterSettings.PaperSizes)
            {
                if (pkSize.Kind == pkSetSize)
                {
                    this._pd.DefaultPageSettings.PaperSize = pkSize;
                    break;
                }
            }
        }

        /// <summary>
        /// 用紙種類変換
        /// </summary>
        /// <param name="nKind">Printerの用紙種類</param>
        /// <returns>PrintDocumentの用紙種類</returns>
        private PaperKind cnvPaperKindPRTtoPRTDC(int nKind)
        {
            PaperKind nRet = PaperKind.A4;

            switch (nKind)
            {
                case 1://レター (8.5 x 11 インチ)
                    nRet = PaperKind.Letter;
                    break;
                case 2://レター小 (8.5 x 11 インチ)
                    nRet = PaperKind.LetterSmall;
                    break;
                case 3://タブロイド紙 (11 x 17 インチ)
                    nRet = PaperKind.Tabloid;
                    break;
                case 4://レジャー (17 x 11 インチ)
                    nRet = PaperKind.Ledger;
                    break;
                case 5://リーガル (8.5 x 14 インチ)
                    nRet = PaperKind.Legal;
                    break;
                case 6://申告用紙 (5.5 x 8.5 インチ)
                    nRet = PaperKind.Statement;
                    break;
                case 7://エグゼクティブ (7.5 x 10.5 インチ)
                    nRet = PaperKind.Executive;
                    break;
                case 8://A3 (297 x 420 mm)
                    nRet = PaperKind.A3;
                    break;
                case 9://A4 (210 x 297 mm)
                    nRet = PaperKind.A4;
                    break;
                case 10://A4 小 (210 x 297 mm)
                    nRet = PaperKind.A4Small;
                    break;
                case 11://A5 (148 x 210 mm)
                    nRet = PaperKind.A5;
                    break;
                case 12://B4 (250 x 354 mm)
                    nRet = PaperKind.B4;
                    break;
                case 13://B5 (182 x 257 mm)
                    nRet = PaperKind.B5;
                    break;
                case 14://二つ折り (8.5 x 13 インチ)
                    nRet = PaperKind.Folio;
                    break;
                case 15://四つ折り (215 x 275 mm)
                    nRet = PaperKind.Quarto;
                    break;
                case 16://10 x 14 インチ
                    nRet = PaperKind.Standard10x14;
                    break;
                case 17://11 x 17 インチ
                    nRet = PaperKind.Standard11x17;
                    break;
                case 18://ノート用紙 (8.5 x 11 インチ)
                    nRet = PaperKind.Note;
                    break;
                case 19://#9 封筒 (3.875 x 8.875 インチ)
                    nRet = PaperKind.Number9Envelope;
                    break;
                case 20://#10 封筒 (4.125 x 9.5 インチ)
                    nRet = PaperKind.Number10Envelope;
                    break;
                case 21://#11 封筒 (4.5 x 10.375 インチ)
                    nRet = PaperKind.Number11Envelope;
                    break;
                case 22://#12 封筒 (4.5 x 11 インチ)
                    nRet = PaperKind.Number12Envelope;
                    break;
                case 23://#14 封筒 (5 x 11.5 インチ)
                    nRet = PaperKind.Number14Envelope;
                    break;
                case 24://C サイズ用紙
                    nRet = PaperKind.CSheet;
                    break;
                case 25://D サイズ用紙
                    nRet = PaperKind.DSheet;
                    break;
                case 26://E サイズ用紙
                    nRet = PaperKind.ESheet;
                    break;
                case 27://DL 封筒 (110 x 220 mm)
                    nRet = PaperKind.DLEnvelope;
                    break;
                case 29://C3 封筒 (324 x 458 mm)
                    nRet = PaperKind.C3Envelope;
                    break;
                case 30://C4 封筒 (229 x 324 mm)
                    nRet = PaperKind.C4Envelope;
                    break;
                case 28://C5 封筒 (162 x 229 mm)
                    nRet = PaperKind.C5Envelope;
                    break;
                case 31://C6 封筒 (114 x 162 mm)
                    nRet = PaperKind.C6Envelope;
                    break;
                case 32://C65 封筒 (114 x 229 mm)
                    nRet = PaperKind.C65Envelope;
                    break;
                case 33://B4 封筒 (250 x 353 mm)
                    nRet = PaperKind.B4Envelope;
                    break;
                case 34://B5 封筒 (176 x 250 mm)
                    nRet = PaperKind.B5Envelope;
                    break;
                case 35://B6 封筒 (176 x 125 mm)
                    nRet = PaperKind.B6Envelope;
                    break;
                case 36://封筒 (110 x 230 mm)
                    nRet = PaperKind.ItalyEnvelope;
                    break;
                case 37://最高級封筒 (3.875 x 7.5 インチ)
                    nRet = PaperKind.MonarchEnvelope;
                    break;
                case 38://封筒 (3.625 x 6.5 インチ)
                    nRet = PaperKind.PersonalEnvelope;
                    break;
                case 39://米国標準ファンフォールド (14.875 x 11 インチ)
                    nRet = PaperKind.USStandardFanfold;
                    break;
                case 40://ドイツ標準ファンフォールド (8.5 x 12 インチ)
                    nRet = PaperKind.GermanStandardFanfold;
                    break;
                case 41://ドイツ リーガル ファンフォールド (8.5 x 13 インチ)
                    nRet = PaperKind.GermanLegalFanfold;
                    break;
                case 256://ユーザー定義
                    nRet = PaperKind.Custom;
                    break;
                default:
                    break;
            }

            return nRet;
        }

        /// <summary>
        /// 印刷処理
        /// </summary>
        public void printer_print(PrintPageEventArgs e, string sPrtMsg, float fPosX, float fPosY)
        {
            float fSetPosX;
            float fSetPosY;

            // Ｘ座標位置
            fSetPosX = ((fPosX - 1f) * this._fXpitch) * 100f;
            // Ｙ座標位置
            fSetPosY = ((fPosY - 1f) * this._fYpitch) * 100f;

            // 黒字で印刷
            e.Graphics.DrawString(sPrtMsg, this._font, Brushes.Black, fSetPosX, fSetPosY);
        }
    }
}

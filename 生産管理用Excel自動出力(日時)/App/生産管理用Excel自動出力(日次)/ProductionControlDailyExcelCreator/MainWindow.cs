//*************************************************************************************
//
//   生産管理用Excel自動出力(日次)画面のコードビハインド
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Cleaner.Log4Net;
using ExcelCreatorBase;
using log4net;
using Model;
using ProductionControlDailyExcelCreator.Common;
using ProductionControlDailyExcelCreator.Core;
using ProductionControlDailyExcelCreator.Exceptions;
using ProductionControlDailyExcelCreator.Log;
using ProductionControlDailyExcelCreator.Operation;
using ProductionControlDailyExcelCreator.Setting;
using ProductionControlDailyExcelCreator.Shared;
using ProductionControlDailyExcelCreator.Types;
using Utility.Core;
using Utility.Network;
using Utility.Types;
using Utility.Common.Network;
using ProductionControlDailyExcelCreator.Common.Container;

namespace ProductionControlDailyExcelCreator
{
    /// <summary>
    /// 生産管理用Excel自動出力(日次)画面のコードビハインド
    /// </summary>
    public partial class MainWindow : BaseWindow, IDocumentOperationContainer
    {
        #region フィールド

        /// <summary>
        /// XML操作オブジェクト
        /// </summary>
        private WrapXmlOperator xmlOperator = null;

        /// <summary>
        /// 出力Excelのファイルパス
        /// </summary>
        private string outputExcelFilePath = null;

        /// <summary>
        /// カスタムログのマネージャ
        /// </summary>
        private readonly CustomLogManager logManager = null;

        /// <summary>
        /// アセンブリの格納位置
        /// </summary>
        private readonly string assemblyLocation = null;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
            this.AllowTransparency = true;
            this.WindowState = FormWindowState.Minimized;
            this.Opacity = (double)0;

            // XML操作(Utility)のラッパーのインスタンスの取得
            this.xmlOperator = WrapXmlOperator.GetInstance();
            // アセンブリの格納位置の取得
            assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var customLogFolderPath = this.xmlOperator.GetCustomLogFolderPath();
            var customLogFileName = this.xmlOperator.GetCustomLogFileName();
            AppSharedModel.Instance.LogManager 
                = CustomLogManager.GetInstance(customLogFolderPath, customLogFileName);
            this.logManager = AppSharedModel.Instance.LogManager;
            this.logManager.CleanUp();
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            new Log4NetCleaner().CleanUp();
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 年
        /// </summary>
        internal string Year { get; set; }

        /// <summary>
        /// 月
        /// </summary>
        internal string Month { get; set; }

        /// <summary>
        /// アセンブリの格納位置
        /// </summary>
        internal string AssemblyLocation
        {
            get { return assemblyLocation; }
        }

        /// <summary>
        /// 読込みフォルダ
        /// </summary>
        internal string InputFolder { get; set; }

        /// <summary>
        /// 読込みファイル情報一覧
        /// </summary>
        internal IList<ReadFileSetting> ReadFileSettings { get; set; }

        /// <summary>
        /// Excel情報の一覧
        /// </summary>
        internal IList<ExcelSetting> ExcelSettings { get; set; }

        /// <summary>
        /// Ping情報
        /// </summary>
        internal PingSetting PingSetting { get; set; }

        #endregion

        #region メソッド

        /// <summary>
        /// 初期処理
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベントデータ</param>
        protected override bool Initialize(object sender, EventArgs e)
        {
            try
            {
                // 年月
                var tmpYearMonth = DateTime.Now;
                // 年
                Year = tmpYearMonth.Year.ToString().PadLeft(4, '0');
                AppSharedModel.Instance.Year = Year;
                // 月
                Month = tmpYearMonth.Month.ToString().PadLeft(2, '0');
                AppSharedModel.Instance.Month = Month;
                // 読込みファイル情報一覧の取得
                ReadFileSettings = xmlOperator.GetReadFileSettings();
                // 読込みフォルダの取得
                InputFolder = xmlOperator.GetReadFolderPath();
                // Excel情報の一覧の取得
                ExcelSettings = xmlOperator.GetExcelProperties();
                // Ping情報の取得
                PingSetting = xmlOperator.GetPingProperty();
                return true;
            }

            catch (Exception ex)
            {
                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(ex.ToString());
                MessageManager.ShowError(MessageManager.SystemID.ProductionControlDailyExcelCreator,
                                        "Exception", ex.Message.ToString());
                return false;
            }

        }

        /// <summary>
        /// 終了処理
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベントデータ</param>
        protected override bool Terminate(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.logManager.Terminate();
                return true;
            }

            catch (Exception ex)
            {
                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(ex.ToString());
                MessageManager.ShowError(MessageManager.SystemID.ProductionControlDailyExcelCreator,
                                        "Exception", ex.Message.ToString());
                return false;
            }
        }

        /// <summary>
        /// バックグラウンド処理
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベントデータ</param>
        protected override void DoInBackground(BackgroundWorker bgWorker, DoWorkEventArgs e)
        {
            try
            {
                ///////////////////////////////////
                int a = 0;
                var b = 1 / a;
                ///////////////////////////////////
                bgWorker.ReportProgress(40,
                    MessageManager.GetMessage(MessageManager.SystemID.ProductionControlDailyExcelCreator,
                    "ReadTargetTextFile"));

                // ファイルの読込み
                if (ReadFile(ExtensionType.TEXT))
                {
                    bgWorker.ReportProgress(80,
                        MessageManager.GetMessage(MessageManager.SystemID.ProductionControlDailyExcelCreator,
                        "WriteTargetExcelFile"));

                    var targetData = DateTime.Now.ToString("yyyyMMdd");
                    foreach (var modelType in EnumExtensions<ModelType>.Enumerate())
                    {
                        bool isDestConnected = false;
                        var item = ExcelSettings.FirstOrDefault(x => x.ModelType == modelType);
                        if (item != null)
                        {
                            isDestConnected = NetworkUtil.ExcutePing(PingSetting.IPAddress, PingSetting.TimeoutPeriod, PingSetting.RetryCount);
                            if (isDestConnected)
                            {
                                if (isExistTargetDateExcelFile(item.OutputBookFolder, targetData) && modelType != ModelType.PMPF090F)
                                {
                                    // 何もしない
                                }
                                else
                                {
                                    // Excelの出力
                                    OperationUtil.OutputExcel(item.ModelType, item.TemplateBookPath,
                                                              item.OutputBookFolder + item.OutputBookName, this);
                                    if (outputExcelFilePath != null)
                                    {
                                        // 出力ファイルのバックアップ
                                        var target = item.BackupFolder + Path.GetFileName(outputExcelFilePath);
                                        File.Copy(outputExcelFilePath, target);
                                        outputExcelFilePath = null;
                                    }
                                }
                            }
                            else
                            {
                                // Excelの出力
                                OperationUtil.OutputExcel(item.ModelType, item.TemplateBookPath,
                                                          item.BackupFolder + item.OutputBookName, this);
                            }
                        }
                    }
                }

                bgWorker.ReportProgress(100,
                    MessageManager.GetMessage(MessageManager.SystemID.ProductionControlDailyExcelCreator,
                    "ProcessCompleted"));
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// バックグラウンド処理完了後に行う処理
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベントデータ</param>
        protected override void OnPostExecute(BackgroundWorker bgWorker, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                // キャンセル
                MessageManager.ShowInformation(MessageManager.SystemID.ProductionControlDailyExcelCreator, "ProcessCanceled");
            }
            else if (e.Error == null)
            {
                // アプリケーションの終了
                this.Close();
            }
            else
            {
                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.Error.GetType().ToString() + " " + e.Error.StackTrace.ToString());
                MessageManager.ShowError(MessageManager.SystemID.ProductionControlDailyExcelCreator,
                                        "Exception", e.Error.Message);
                MessageManager.ShowError(MessageManager.SystemID.ProductionControlDailyExcelCreator, "ProcessAborted");
                // アプリケーションの終了
                this.Close();
            }
        }

        /// <summary>
        /// ファイルの読込み
        /// </summary>
        /// <param name="extensionType">拡張子の種類</param>
        /// <returns>結果</returns>
        private bool ReadFile(ExtensionType extension)
        {
            // 読込みファイルパス
            string readFilePath = null;
            // 読込み用設定ファイルパス
            string readSettingFilePath = null;
            // ファイル1行あたりの文字数
            int lengthPerLine = default(int);
            // ログメッセージ出力用ビルダー
            StringBuilder logMessageStringBuilder = null;
            // 読込みファイル名
            string readFileName = null;

            try
            {
                AppSharedModel.Instance.PMGQ010BModels.Clear();
                AppSharedModel.Instance.PMPA260BModels.Clear();
                AppSharedModel.Instance.PMPD330BModels.Clear();
                AppSharedModel.Instance.PMPF070BModels.Clear();
                AppSharedModel.Instance.PMPF090BModels.Clear();
                AppSharedModel.Instance.PQGA186BModels.Clear();
                AppSharedModel.Instance.PQGA380BModels.Clear();
                AppSharedModel.Instance.PQGA420BModels.Clear();
                AppSharedModel.Instance.PQGA820BModels.Clear();
                AppSharedModel.Instance.PQGA820BModels.Clear();
                AppSharedModel.Instance.SSYM050BModels.Clear();
                AppSharedModel.Instance.SSZA040BModels.Clear();

                foreach (var modelType in EnumExtensions<ModelType>.Enumerate())
                {
                    var items = ReadFileSettings.Where(x => x.ModelType == modelType);
                    foreach (var item in items)
                    {
                        readFilePath = InputFolder + item.FileName;
                        readSettingFilePath = AssemblyLocation + "\\" + item.SettingFileName;
                        lengthPerLine = item.LengthPerLine;
                        readFileName = item.FileName;

                        // ファイルの読込み
                        switch (modelType)
                        {
                            case ModelType.PMGQ010F:
                                if (File.Exists(readFilePath))
                                {
                                    try
                                    {
                                        logMessageStringBuilder
                                          = new StringBuilder()
                                           .Append("ファイル読込み開始(")
                                           .Append("ファイル名:" + readFileName + ")");
                                        this.logManager.Write(logMessageStringBuilder.ToString());
                                        AppSharedModel.Instance.PMGQ010BModels
                                            .AddRange(OperationUtil.ReadFile<PMGQ010BModel>(modelType, extension, readFilePath, readSettingFilePath));
                                        var recordCount = AppSharedModel.Instance.PMGQ010BModels.Count();
                                        logMessageStringBuilder
                                          = new StringBuilder()
                                           .Append("ファイル読込み終了(")
                                           .Append("ファイル名:" + readFileName + ",")
                                           .Append("レコード数：" + recordCount.ToString() + ")");
                                        this.logManager.Write(logMessageStringBuilder.ToString());
                                    }
                                    catch (FileNoDataException)
                                    {
                                        logMessageStringBuilder
                                           = new StringBuilder()
                                            .Append("ファイル読込み終了(")
                                            .Append("ファイル名:" + readFileName + ",")
                                            .Append("レコード数：0)");
                                        this.logManager.Write(logMessageStringBuilder.ToString());
                                    }
                                }
                                break;
                            case ModelType.PMPA280F:
                                if (File.Exists(readFilePath))
                                {
                                    try
                                    {
                                        logMessageStringBuilder
                                          = new StringBuilder()
                                           .Append("ファイル読込み開始(")
                                           .Append("ファイル名:" + readFileName + ")");
                                        this.logManager.Write(logMessageStringBuilder.ToString());
                                        AppSharedModel.Instance.PMPA260BModels
                                            .AddRange(OperationUtil.ReadFile<PMPA260BModel>(modelType, extension, readFilePath, readSettingFilePath));
                                        var recordCount = AppSharedModel.Instance.PMPA260BModels.Count();
                                        logMessageStringBuilder
                                          = new StringBuilder()
                                           .Append("ファイル読込み終了(")
                                           .Append("ファイル名:" + readFileName + ",")
                                           .Append("レコード数：" + recordCount.ToString() + ")");
                                        this.logManager.Write(logMessageStringBuilder.ToString());
                                    }
                                    catch (FileNoDataException)
                                    {
                                        logMessageStringBuilder
                                           = new StringBuilder()
                                            .Append("ファイル読込み終了(")
                                            .Append("ファイル名:" + readFileName + ",")
                                            .Append("レコード数：0)");
                                        this.logManager.Write(logMessageStringBuilder.ToString());
                                    }
                                }
                                break;
                            case ModelType.PMPD610F:
                                if (File.Exists(readFilePath))
                                {
                                    try
                                    {
                                        logMessageStringBuilder
                                          = new StringBuilder()
                                           .Append("ファイル読込み開始(")
                                           .Append("ファイル名:" + readFileName + ")");
                                        this.logManager.Write(logMessageStringBuilder.ToString());
                                        AppSharedModel.Instance.PMPD330BModels
                                            .AddRange(OperationUtil.ReadFile<PMPD330BModel>(modelType, extension, readFilePath, readSettingFilePath));
                                        var recordCount = AppSharedModel.Instance.PMPD330BModels.Count();
                                        logMessageStringBuilder
                                          = new StringBuilder()
                                           .Append("ファイル読込み終了(")
                                           .Append("ファイル名:" + readFileName + ",")
                                           .Append("レコード数：" + recordCount.ToString() + ")");
                                        this.logManager.Write(logMessageStringBuilder.ToString());
                                    }
                                    catch (FileNoDataException)
                                    {
                                        logMessageStringBuilder
                                           = new StringBuilder()
                                            .Append("ファイル読込み終了(")
                                            .Append("ファイル名:" + readFileName + ",")
                                            .Append("レコード数：0)");
                                        this.logManager.Write(logMessageStringBuilder.ToString());
                                    }
                                }
                                break;
                            case ModelType.PMPF070F:
                                if (File.Exists(readFilePath))
                                {
                                    try
                                    {
                                        logMessageStringBuilder
                                          = new StringBuilder()
                                           .Append("ファイル読込み開始(")
                                           .Append("ファイル名:" + readFileName + ")");
                                        this.logManager.Write(logMessageStringBuilder.ToString());
                                        AppSharedModel.Instance.PMPF070BModels
                                            .AddRange(OperationUtil.ReadFile<PMPF070BModel>(modelType, extension, readFilePath, readSettingFilePath));
                                        var recordCount = AppSharedModel.Instance.PMPF070BModels.Count();
                                        logMessageStringBuilder
                                          = new StringBuilder()
                                           .Append("ファイル読込み終了(")
                                           .Append("ファイル名:" + readFileName + ",")
                                           .Append("レコード数：" + recordCount.ToString() + ")");
                                        this.logManager.Write(logMessageStringBuilder.ToString());
                                    }
                                    catch (FileNoDataException)
                                    {
                                        logMessageStringBuilder
                                           = new StringBuilder()
                                            .Append("ファイル読込み終了(")
                                            .Append("ファイル名:" + readFileName + ",")
                                            .Append("レコード数：0)");
                                        this.logManager.Write(logMessageStringBuilder.ToString());
                                    }
                                }
                                break;
                            case ModelType.PMPF090F:
                                if (File.Exists(readFilePath))
                                {
                                    try
                                    {
                                        logMessageStringBuilder
                                          = new StringBuilder()
                                           .Append("ファイル読込み開始(")
                                           .Append("ファイル名:" + readFileName + ")");
                                        this.logManager.Write(logMessageStringBuilder.ToString());
                                        AppSharedModel.Instance.PMPF090BModels
                                            .AddRange(OperationUtil.ReadFile<PMPF090BModel>(modelType, extension, readFilePath, readSettingFilePath));
                                        var recordCount = AppSharedModel.Instance.PMPF090BModels.Count();
                                        logMessageStringBuilder
                                          = new StringBuilder()
                                           .Append("ファイル読込み終了(")
                                           .Append("ファイル名:" + readFileName + ",")
                                           .Append("レコード数：" + recordCount.ToString() + ")");
                                        this.logManager.Write(logMessageStringBuilder.ToString());
                                    }
                                    catch (FileNoDataException)
                                    {
                                        logMessageStringBuilder
                                           = new StringBuilder()
                                            .Append("ファイル読込み終了(")
                                            .Append("ファイル名:" + readFileName + ",")
                                            .Append("レコード数：0)");
                                        this.logManager.Write(logMessageStringBuilder.ToString());
                                    }
                                }
                                break;
                            case ModelType.PQGA186F:
                                if (File.Exists(readFilePath))
                                {
                                    try
                                    {
                                        logMessageStringBuilder
                                          = new StringBuilder()
                                           .Append("ファイル読込み開始(")
                                           .Append("ファイル名:" + readFileName + ")");
                                        this.logManager.Write(logMessageStringBuilder.ToString());
                                        AppSharedModel.Instance.PQGA186BModels
                                            .AddRange(OperationUtil.ReadFile<PQGA186BModel>(modelType, extension, readFilePath, readSettingFilePath));
                                        var recordCount = AppSharedModel.Instance.PQGA186BModels.Count();
                                        logMessageStringBuilder
                                          = new StringBuilder()
                                           .Append("ファイル読込み終了(")
                                           .Append("ファイル名:" + readFileName + ",")
                                           .Append("レコード数：" + recordCount.ToString() + ")");
                                        this.logManager.Write(logMessageStringBuilder.ToString());
                                    }
                                    catch (FileNoDataException)
                                    {
                                        logMessageStringBuilder
                                           = new StringBuilder()
                                            .Append("ファイル読込み終了(")
                                            .Append("ファイル名:" + readFileName + ",")
                                            .Append("レコード数：0)");
                                        this.logManager.Write(logMessageStringBuilder.ToString());
                                    }
                                }
                                break;
                            case ModelType.PQGA380F:
                                if (File.Exists(readFilePath))
                                {
                                    try
                                    {
                                        logMessageStringBuilder
                                          = new StringBuilder()
                                           .Append("ファイル読込み開始(")
                                           .Append("ファイル名:" + readFileName + ")");
                                        this.logManager.Write(logMessageStringBuilder.ToString());
                                        AppSharedModel.Instance.PQGA380BModels
                                            .AddRange(OperationUtil.ReadFile<PQGA380BModel>(modelType, extension, readFilePath, readSettingFilePath));
                                        var recordCount = AppSharedModel.Instance.PQGA380BModels.Count();
                                        logMessageStringBuilder
                                          = new StringBuilder()
                                           .Append("ファイル読込み終了(")
                                           .Append("ファイル名:" + readFileName + ",")
                                           .Append("レコード数：" + recordCount.ToString() + ")");
                                        this.logManager.Write(logMessageStringBuilder.ToString());
                                    }
                                    catch (FileNoDataException)
                                    {
                                        logMessageStringBuilder
                                           = new StringBuilder()
                                            .Append("ファイル読込み終了(")
                                            .Append("ファイル名:" + readFileName + ",")
                                            .Append("レコード数：0)");
                                        this.logManager.Write(logMessageStringBuilder.ToString());
                                    }
                                }
                                break;
                            case ModelType.PQGA420F:
                                if (File.Exists(readFilePath))
                                {
                                    try
                                    {
                                        logMessageStringBuilder
                                          = new StringBuilder()
                                           .Append("ファイル読込み開始(")
                                           .Append("ファイル名:" + readFileName + ")");
                                        this.logManager.Write(logMessageStringBuilder.ToString());
                                        AppSharedModel.Instance.PQGA420BModels
                                            .AddRange(OperationUtil.ReadFile<PQGA420BModel>(modelType, extension, readFilePath, readSettingFilePath));
                                        var recordCount = AppSharedModel.Instance.PQGA420BModels.Count();
                                        logMessageStringBuilder
                                          = new StringBuilder()
                                           .Append("ファイル読込み終了(")
                                           .Append("ファイル名:" + readFileName + ",")
                                           .Append("レコード数：" + recordCount.ToString() + ")");
                                        this.logManager.Write(logMessageStringBuilder.ToString());
                                    }
                                    catch (FileNoDataException)
                                    {
                                        logMessageStringBuilder
                                           = new StringBuilder()
                                            .Append("ファイル読込み終了(")
                                            .Append("ファイル名:" + readFileName + ",")
                                            .Append("レコード数：0)");
                                        this.logManager.Write(logMessageStringBuilder.ToString());
                                    }
                                }
                                break;
                            case ModelType.PQGA820F:
                                if (File.Exists(readFilePath))
                                {
                                    try
                                    {
                                        logMessageStringBuilder
                                          = new StringBuilder()
                                           .Append("ファイル読込み開始(")
                                           .Append("ファイル名:" + readFileName + ")");
                                        this.logManager.Write(logMessageStringBuilder.ToString());
                                        AppSharedModel.Instance.PQGA820BModels
                                            .AddRange(OperationUtil.ReadFile<PQGA820BModel>(modelType, extension, readFilePath, readSettingFilePath));
                                        var recordCount = AppSharedModel.Instance.PQGA820BModels.Count();
                                        logMessageStringBuilder
                                          = new StringBuilder()
                                           .Append("ファイル読込み終了(")
                                           .Append("ファイル名:" + readFileName + ",")
                                           .Append("レコード数：" + recordCount.ToString() + ")");
                                        this.logManager.Write(logMessageStringBuilder.ToString());
                                    }
                                    catch (FileNoDataException)
                                    {
                                        logMessageStringBuilder
                                           = new StringBuilder()
                                            .Append("ファイル読込み終了(")
                                            .Append("ファイル名:" + item.FileName + ",")
                                            .Append("レコード数：0)");
                                        this.logManager.Write(logMessageStringBuilder.ToString());
                                    }
                                }
                                break;
                            case ModelType.SSYM040F:
                                if (File.Exists(readFilePath))
                                {
                                    try
                                    {
                                        logMessageStringBuilder
                                          = new StringBuilder()
                                           .Append("ファイル読込み開始(")
                                           .Append("ファイル名:" + readFileName + ")");
                                        this.logManager.Write(logMessageStringBuilder.ToString());
                                        AppSharedModel.Instance.SSYM040BModels
                                            .AddRange(OperationUtil.ReadFile<SSYM040BModel>(modelType, extension, readFilePath, readSettingFilePath));
                                        var recordCount = AppSharedModel.Instance.SSYM040BModels.Count();
                                        logMessageStringBuilder
                                          = new StringBuilder()
                                           .Append("ファイル読込み終了(")
                                           .Append("ファイル名:" + readFileName + ",")
                                           .Append("レコード数：" + recordCount.ToString() + ")");
                                        this.logManager.Write(logMessageStringBuilder.ToString());
                                    }
                                    catch (FileNoDataException)
                                    {
                                        logMessageStringBuilder
                                           = new StringBuilder()
                                            .Append("ファイル読込み終了(")
                                            .Append("ファイル名:" + readFileName + ",")
                                            .Append("レコード数：0)");
                                        this.logManager.Write(logMessageStringBuilder.ToString());
                                    }
                                }
                                break;
                            case ModelType.SSYM050F:
                                if (File.Exists(readFilePath))
                                {
                                    try
                                    {
                                        logMessageStringBuilder
                                          = new StringBuilder()
                                           .Append("ファイル読込み開始(")
                                           .Append("ファイル名:" + readFileName + ")");
                                        this.logManager.Write(logMessageStringBuilder.ToString());
                                        AppSharedModel.Instance.SSYM050BModels
                                            .AddRange(OperationUtil.ReadFile<SSYM050BModel>(modelType, extension, readFilePath, readSettingFilePath));
                                        var recordCount = AppSharedModel.Instance.SSYM050BModels.Count();
                                        logMessageStringBuilder
                                          = new StringBuilder()
                                           .Append("ファイル読込み終了(")
                                           .Append("ファイル名:" + readFileName + ",")
                                           .Append("レコード数：" + recordCount.ToString() + ")");
                                        this.logManager.Write(logMessageStringBuilder.ToString());
                                    }
                                    catch (FileNoDataException)
                                    {
                                        logMessageStringBuilder
                                           = new StringBuilder()
                                            .Append("ファイル読込み終了(")
                                            .Append("ファイル名:" + readFileName + ",")
                                            .Append("レコード数：0)");
                                        this.logManager.Write(logMessageStringBuilder.ToString());
                                    }
                                }
                                break;
                            case ModelType.SSZA400B:
                                if (File.Exists(readFilePath))
                                {
                                    try
                                    {
                                        logMessageStringBuilder
                                          = new StringBuilder()
                                           .Append("ファイル読込み開始(")
                                           .Append("ファイル名:" + readFileName + ")");
                                        this.logManager.Write(logMessageStringBuilder.ToString());
                                        AppSharedModel.Instance.SSZA040BModels
                                            .AddRange(OperationUtil.ReadFile<SSZA040BModel>(modelType, extension, readFilePath, readSettingFilePath));
                                        var recordCount = AppSharedModel.Instance.SSZA040BModels.Count();
                                        logMessageStringBuilder
                                          = new StringBuilder()
                                           .Append("ファイル読込み終了(")
                                           .Append("ファイル名:" + readFileName + ",")
                                           .Append("レコード数：" + recordCount.ToString() + ")");
                                        this.logManager.Write(logMessageStringBuilder.ToString());
                                    }
                                    catch (FileNoDataException)
                                    {
                                        logMessageStringBuilder
                                           = new StringBuilder()
                                            .Append("ファイル読込み終了(")
                                            .Append("ファイル名:" + readFileName + ",")
                                            .Append("レコード数：0)");
                                        this.logManager.Write(logMessageStringBuilder.ToString());
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }

                return true;
            }

            catch (Exception ex)
            {
                logMessageStringBuilder
                   = new StringBuilder()
                    .Append("ファイル読込み異常(")
                    .Append("ファイル名:" + readFileName + ",")
                    .Append("エラーメッセージ:" + ex.Message + ")");
                this.logManager.Write(logMessageStringBuilder.ToString());
                throw;
            }
        }

        /// <summary>
        /// 対象日付のExcelファイルが存在するか
        /// </summary>
        /// <param name="targetFolder">対象フォルダ</param>
        /// <param name="targetDate">対象日付</param>
        /// <returns>結果</returns>
        internal bool isExistTargetDateExcelFile(string targetFolder, string targetDate)
        {
            try
            {
                // 出力ファイルのバックアップ
                string[] files
                    = Directory.GetFiles(targetFolder, "*", SearchOption.TopDirectoryOnly);

                foreach (var file in files)
                {
                    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
                    var timeStamp = fileNameWithoutExtension.SubstringRight(12);
                    if (targetDate == timeStamp.Substring(0, 8))
                        return true;
                }

                return false;
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region インターフェース

        /// <summary>
        /// Excel作成完了の通知
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        public void NotifyOutputExcel(string filePath)
        {
            outputExcelFilePath = filePath;
        }

        #endregion

    }
}


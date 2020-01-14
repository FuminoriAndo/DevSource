//*************************************************************************************
//
//   設定画面のビューモデル
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   18.07.01              DSK吉武   　 新規作成
//
//*************************************************************************************
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Xml.Linq;
using System.Xml.XPath;
using CKSI1010.Common;
using CKSI1010.Core;
using CKSI1010.Operation.Common.ViewModel;
using CKSI1010.Operation.SettingDisplay.Setting;
using CKSI1010.Shared;
using CKSI1010.Shared.Model;
using Common;
using GalaSoft.MvvmLight.Command;
using log4net;
using static Common.MessageManager;

namespace CKSI1010.Operation.SettingDisplay.ViewModel
{
    /// <summary>
    /// 設定画面のビューモデル
    /// </summary>
    public class SettingDisplayViewModel : OperationViewModelBase
    {
        #region フィールド

        /// <summary>
        /// XMLファイルのパス
        /// </summary>
        private readonly string xmlFilePath = null;

        /// <summary>
        /// XDocumentオブジェクト
        /// </summary>
        private readonly XDocument xDocument = null;

        /// <summary>
        /// XML操作オブジェクト
        /// </summary>
        private XMLOperator xmlOperator = null;

        /// <summary>
        /// 副資材棚卸.xlsxのディレクトリ
        /// </summary>
        private string inventoryExcelFileDirectory = string.Empty;

        /// <summary>
        /// 副資材棚卸.xlsx(テンプレートファイル)のディレクトリ
        /// </summary>
        private string inventoryTemplateExcelFileDirectory = string.Empty;

        /// <summary>
        /// 月末検針データ(Excel)のディレクトリ
        /// </summary>
        private string kensinExcelFileDirectory = string.Empty;

        /// <summary>
        /// 向先の設定情報
        /// </summary>
        internal IList<BaseSetting> MukesakiSettings { get; set; } = new List<BaseSetting>();

        /// <summary>
        /// 対象の向先の設定情報
        /// </summary>
        public ObservableCollection<BaseSetting> TargetMukesakiSettings { get; } = new ObservableCollection<BaseSetting>();

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dataService">データサービス</param>
        public SettingDisplayViewModel(IDataService dataService) : base(dataService)
        {
            // XMLファイルのパスの取得
            xmlFilePath
                = Path.Combine(Path.GetDirectoryName(
                    Assembly.GetExecutingAssembly().Location) ?? string.Empty, "Settings.xml");

            // XMLファイルを読込む
            xDocument = XDocument.Load(xmlFilePath);

            // XML操作オブジェクトのインスタンスの取得
            xmlOperator = XMLOperator.GetInstance();
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 副資材棚卸.xlsxのディレクトリ
        /// </summary>
        public string InventoryExcelFileDirectory
        {
            get { return inventoryExcelFileDirectory; }
            set { Set(ref inventoryExcelFileDirectory, value); }
        }

        /// <summary>
        /// 副資材棚卸.xlsx(テンプレートファイル)のディレクトリ
        /// </summary>
        public string InventoryTemplateExcelFileDirectory
        {
            get { return inventoryTemplateExcelFileDirectory; }
            set { Set(ref inventoryTemplateExcelFileDirectory, value); }
        }

        /// <summary>
        /// 月末検針データ(Excel)のディレクトリ
        /// </summary>
        public string KensinExcelFileDirectory
        {
            get { return kensinExcelFileDirectory; }
            set { Set(ref kensinExcelFileDirectory, value); }
        }

        #endregion

        #region メソッド

        /// <summary>
        /// 初期化
        /// </summary>
        /// <returns>非同期タスク</returns>
        internal override async Task InitializeAsync()
        {
            try
            {
                // 編集可能
                var elemExcelOutput = xDocument.XPathSelectElement($"/Settings/Excel/Output");
                InventoryExcelFileDirectory = (string)elemExcelOutput.Attribute("Path");
                var elemKensinNyuko = xDocument.XPathSelectElement($"/Settings/Kensin/File");
                KensinExcelFileDirectory = (string)elemKensinNyuko.Attribute("Path");

                // 編集不可
                var elemExcelTemplate = xDocument.XPathSelectElement($"/Settings/Excel/Template");
                InventoryTemplateExcelFileDirectory = (string)elemExcelTemplate.Attribute("Path");

                // 向先の設定情報の取得
                SharedViewModel.Instance.Mukesaki = this.xmlOperator.GetMukesakiType();
                MukesakiSettings.Clear();
                foreach (var item in SharedViewModel.Instance.Mukesaki)
                {
                    var setting = new BaseSetting();
                    setting.Key = item.Key;
                    setting.Value = item.Value;
                    MukesakiSettings.Add(setting);
                }
                // 対象の向先の設定情報
                TargetMukesakiSettings.Clear();
                TargetMukesakiSettings.AddRange(MukesakiSettings.ToObservableCollection());

                await base.InitializeAsync();
            }

            catch (Exception e)
            {
                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.ToString());
                MessageManager.ShowError(SystemID.CKSI1010, "FatalError");
            }
        }

        #endregion

        #region コマンド

        /// <summary>
        /// 「・・・」ボタン押下
        /// </summary>
        public ICommand FileSelectInventoryExcelFileDirectory => new RelayCommand(() =>
        {
            try
            {
                using (var ofd = new FolderBrowserDialog())
                {
                    try
                    {
                        ofd.SelectedPath = InventoryExcelFileDirectory;
                        ofd.Description = "副資材棚卸.xlsxを出力するフォルダを指定してください。";

                        // フォルダダイアログを表示する
                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            InventoryExcelFileDirectory = ofd.SelectedPath + "\\";
                        }
                    }

                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

            catch (Exception e)
            {
                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.ToString());
                MessageManager.ShowError(SystemID.CKSI1010, "FatalError");
            }
        });

        /// <summary>
        /// 「・・・」ボタン押下
        /// </summary>
        public ICommand FileSelectKensinExcelFileDirectory => new RelayCommand(() =>
        {
            try
            {
                using (var ofd = new FolderBrowserDialog())
                {
                    try
                    {
                        ofd.SelectedPath = KensinExcelFileDirectory;
                        ofd.Description = "月末検針データ(EXCEL)の格納場所を指定してください。";

                        // フォルダダイアログを表示する
                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            KensinExcelFileDirectory = ofd.SelectedPath + "\\";
                        }
                    }

                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

            catch (Exception e)
            {
                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.ToString());
                MessageManager.ShowError(SystemID.CKSI1010, "FatalError");
            }
        });

        /// <summary>
        /// 「保存」ボタン押下
        /// </summary>
        public ICommand FireSave => new RelayCommand(() =>
        {
            try
            {
                var elemOutputExcel = xDocument.XPathSelectElement($"/Settings/Excel/Output");
                elemOutputExcel.Attribute("Path").SetValue(InventoryExcelFileDirectory);
                xDocument.Save(xmlFilePath);
                var elemKensinExcel = xDocument.XPathSelectElement($"/Settings/Kensin/File");
                elemKensinExcel.Attribute("Path").SetValue(KensinExcelFileDirectory);
                xDocument.Save(xmlFilePath);
                MessageManager.ShowInformation(SystemID.CKSI1010, "SaveCompleted");
            }

            catch (Exception e)
            {
                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.ToString());
                MessageManager.ShowError(SystemID.CKSI1010, "FatalError");
            }
        });

        /// <summary>
        /// 「閉じる」ボタン押下
        /// </summary>
        public ICommand FireClose => new RelayCommand(() =>
        {
            try
            {
                // モーダル画面を閉じる
                CloseModalWindow();
                // クローズイベントを通知する
                CloseEvent(new SettingDisplayCloseEventArgs(true));
                // クローズした後は、呼び出し元から登録されたイベントハンドラを削除する
                CloseEvent -= CloseEvent;
            }

            catch (Exception e)
            {
                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.ToString());
                MessageManager.ShowError(SystemID.CKSI1010, "FatalError");
            }
        });

        #endregion

        #region イベントハンドラー

        /// <summary>
        /// 「閉じる」ボタン押下時のイベントハンドラーのデリゲート
        /// </summary>
        /// <param name="e">イベント引数</param>
        public delegate void CloseEventHandler(SettingDisplayCloseEventArgs e);

        /// <summary>
        /// 「閉じる」ボタン押下時のイベントハンドラー
        /// </summary>
        public event CloseEventHandler CloseEvent;

        #endregion
    }

    #region 内部クラス

    /// <summary>
    /// 「閉じる」のイベント引数クラス
    /// </summary>
    public class SettingDisplayCloseEventArgs : EventArgs
    {
        #region フィールド

        /// <summary>
        /// キャンセルしたかどうか
        /// </summary>
        private readonly bool isCancel;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="cancel"></param>
        public SettingDisplayCloseEventArgs(bool cancel)
        {
            isCancel = cancel;
        }

        #endregion

        #region キャンセルしたかどうか

        /// <summary>
        /// キャンセルしたかどうか
        /// </summary>
        public bool Cancel { get { return isCancel; } }

        #endregion
    }

    #endregion
}


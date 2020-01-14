using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using CKSI1020.ViewModel;
using Common;
using Core;
using System.IO;
using System.Windows.Input;

//*************************************************************************************
//
//   MainWindow.xaml の相互作用ロジック
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   17.02.01              DSK   　     新規作成
//
//*************************************************************************************
namespace CKSI1020
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        #region フィールド

        /// <summary>
        /// XML操作オブジェクト
        /// </summary>
        private XMLOperator _xmlOperator = null;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindow()
        {
            this._xmlOperator = XMLOperator.GetInstance();
            InitializeComponent();

            // メニューボタン生成
            CreateMenuButton();

            this.Loaded += delegate { Keyboard.Focus(this.TanaorosiSystemCategory); };
            
        }

        #endregion

        #region メソッド

        /// <summary>
        /// メニューボタン生成
        /// </summary>
        private void CreateMenuButton()
        {
            try
            {
                // メニューボタン設定情報取得
                MenuButtonSettingInfo menuButtonSettingInfo = this._xmlOperator.GetMenuButtonSettingInfo();

                // メニュー設定情報取得
                IList<MenuSettingInfo> menuSettingInformations = this._xmlOperator.GetMenuSettingInfo();

                foreach (MenuSettingInfo menuSettingInfo in menuSettingInformations)
                {
                    if (this.TanaorosiSystemCategory.Name.Equals(menuSettingInfo.ButtonName))
                    {
                        SettingMenuButton(this.TanaorosiSystemCategory, menuButtonSettingInfo, menuSettingInfo);
                    }
                    else if (this.TanaorosiGroupKBN.Name.Equals(menuSettingInfo.ButtonName))
                    {
                        SettingMenuButton(this.TanaorosiGroupKBN, menuButtonSettingInfo, menuSettingInfo);
                    }
                    else if (this.TanaorosiOperation.Name.Equals(menuSettingInfo.ButtonName))
                    {
                        SettingMenuButton(this.TanaorosiOperation, menuButtonSettingInfo, menuSettingInfo);
                    }
                    else if (this.TanaorosiOperationGroup.Name.Equals(menuSettingInfo.ButtonName))
                    {
                        SettingMenuButton(this.TanaorosiOperationGroup, menuButtonSettingInfo, menuSettingInfo);
                    }
                    else if (this.TanaorosiOperationMenu.Name.Equals(menuSettingInfo.ButtonName))
                    {
                        SettingMenuButton(this.TanaorosiOperationMenu, menuButtonSettingInfo, menuSettingInfo);
                    }
                }
            }

            catch (Exception)
            {
                //システムエラー
                MessageManager.ShowError(MessageManager.SystemID.CKSI1020, "SystemError");
            }
        }

        /// <summary>
        /// メニューボタン設定
        /// </summary>
        private void SettingMenuButton(Button button, MenuButtonSettingInfo menuButtonSettingInfo, MenuSettingInfo menuSettingInfo)
        {
            //幅
            button.Width = double.Parse(menuButtonSettingInfo.Width);
            //高さ
            button.Height = double.Parse(menuButtonSettingInfo.Height);
            //余白
            Thickness margin = new Thickness(double.Parse(menuButtonSettingInfo.Margin));
            button.Margin = margin;

            //コンテント
            button.Content = menuSettingInfo.MenuName;
            //パラメータ
            button.CommandParameter = menuSettingInfo.Execution;
        }

        /// <summary>
        /// 閉じる「×」ボタン押下時のイベント
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベントデータ</param>
        private void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            if (MessageManager.ShowQuestion(MessageManager.SystemID.CKSI1020, "ConfirmExit") == MessageManager.ResultType.No)
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// ボタンクリックイベント
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベントデータ</param>
        private void OnButtonClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = (Button)sender;
                string execution = button.CommandParameter.ToString();
                
                // プロセスが起動可能かどうか確認する。
                if (WindowOperator.CanStartupWindow(execution))
                {
                    // 対象機能を起動する。
                    WindowOperator.StartupWindow(Path.GetDirectoryName(execution), Path.GetFileName(execution), "");
                }
            }
            catch (Exception)
            {
                //システムエラー
                MessageManager.ShowError(MessageManager.SystemID.CKSI1020, "SystemError");
            }
        }

        #endregion

    }
}





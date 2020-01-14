using CKSI1010.Common;
using CKSI1010.Core;
using CKSI1010.Operation.Common.ViewModel;
using CKSI1010.Shared.Model;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Windows.Input;
using static CKSI1010.Common.Constants;
//*************************************************************************************
//
//   入力作業の選択画面のビューモデル
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************
namespace CKSI1010.Operation.SelectInputWork.ViewModel
{
    /// <summary>
    /// イベント引数
    /// </summary>
    public class InputWorkSelectedEventArgs : EventArgs
    {
        public String WorkKbn { get; set; } = string.Empty;
    }

    /// <summary>
    /// ボタン選択時のデリゲート
    /// </summary>
    /// <param name="sender">送信者</param>
    /// <param name="e">イベント引数</param>
    public delegate void InputWorkSelected(object sender, InputWorkSelectedEventArgs e);

    /// <summary>
    /// 入力選択ビューモデル
    /// (選択結果をInputWorkSelectedで発火する)
    /// </summary>
    public class SelectInputWorkViewModel : OperationViewModelBase
    {
        public SelectInputWorkViewModel(IDataService dataService) : base(dataService)
        {
        }

        public event InputWorkSelected InputWorkSelected = null;

        protected void OnInputWorkSelected(InputWorkSelectedEventArgs e)
        {
            CloseModalWindow();
            InputWorkSelected?.Invoke(this, e);
        }

        /// <summary>
        /// 棚卸(作業区分1)
        /// </summary>
        public ICommand SelectTanaorosi => new RelayCommand(() =>
        {
            OnInputWorkSelected(new InputWorkSelectedEventArgs() { WorkKbn = SizaiWorkCategory.TanaoroshiWork.GetStringValue() });
        });

        /// <summary>
        /// 検針(作業区分2)
        /// </summary>
        public ICommand SelectMeterReading => new RelayCommand(() =>
        {
            OnInputWorkSelected(new InputWorkSelectedEventArgs() { WorkKbn = SizaiWorkCategory.KensinWork.GetStringValue() });
        });
    }
}


using CKSI1010.Operation.InputInventoryActual.ViewModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
//*************************************************************************************
//
//   棚卸実績値入力画面(検針データ入力作業)のビュー
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1010.Operation.InputInventoryActual.View
{
    /// <summary>
    /// InputInventoryActualMeterDataView.xaml の相互作用ロジック
    /// </summary>
    public partial class InputInventoryActualMeterDataView
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public InputInventoryActualMeterDataView()
        {
            InitializeComponent();

            this.Loaded += InputInventoryActualMeterDataView_Loaded;
        }

        #endregion

        #region メソッド
        private void textboxTogetu_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            bool isInput = false;

            //数字以外は入力不可にする
            if (System.Text.RegularExpressions.Regex.IsMatch(e.Text, @"[0-9]"))
            {
                isInput = true;
            }
            // 更新したい場合は false, 更新したくない場合は true
            e.Handled = !isInput;
        }

        private DataGrid GetActiveDataGrid()
        {
            var dataContext = this.DataContext as InputInventoryActualMeterDataViewModel;

            if (dataContext == null) return null;

            switch (dataContext.SelectShizaiKbn)
            {
                case Types.ShizaiKbnTypes.SK:
                    return skDataGrid;
                case Types.ShizaiKbnTypes.EF:
                    return efDataGrid;
                case Types.ShizaiKbnTypes.LF:
                    return lfDataGrid;
                case Types.ShizaiKbnTypes.Building:
                    return buildingDataGrid;
                case Types.ShizaiKbnTypes.LD:
                    return ldDataGrid;
                case Types.ShizaiKbnTypes.TD:
                    return tdDataGrid;
                case Types.ShizaiKbnTypes.CC:
                    return ccDataGrid;
                case Types.ShizaiKbnTypes.Others:
                    return othersDataGrid;
                default:
                    return null;
            }
        }

        private void textboxTogetu_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Down || e.Key == Key.Up || e.Key == Key.Tab)
            {
                var dataGrid = GetActiveDataGrid();
                if (dataGrid == null) return;

                var index = dataGrid.SelectedIndex;

                if (e.Key == Key.Enter || e.Key == Key.Tab)
                {
                    if (Keyboard.Modifiers == ModifierKeys.Shift)
                    {
                        index--;
                    }
                    else
                    {
                        index++;

                    }
                }
                else if (e.Key == Key.Down)
                {
                    index++;
                }
                else if (e.Key == Key.Up)
                {
                    index--;
                }

                if (dataGrid.Items.Count - 1 <= index)
                {
                    index = dataGrid.Items.Count - 1;
                }
                else if (0 > index)
                {
                    index = 0;
                }

                SelectDataGridRow(dataGrid, index);
                e.Handled = true;
            }
        }

        private void textboxTogetu_GotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).Text = ((TextBox)sender).Text.Replace(",", "");
            ((TextBox)sender).SelectAll();
        }

        private void textboxTogetu_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            string val = ((TextBox)sender).Text;
            if (!string.IsNullOrEmpty(val))
            {
                long num = long.Parse(val.Replace(",", ""));
                ((TextBox)sender).Text = num.ToString("#,0");
            }
        }

        private void textboxTogetu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void dataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
            {
                ((DataGrid)sender).BeginEdit();
            }

        }

        private void InputInventoryActualMeterDataView_Loaded(object sender, RoutedEventArgs e)
        {
            var dataContext = this.DataContext as InputInventoryActualMeterDataViewModel;
            if (dataContext != null)
            {
                dataContext.PropertyChanged += DataContext_PropertyChanged;
                SetFocusToDataGridViewFirstRow(GetActiveDataGrid());
            }
        }

        private void DataContext_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "SelectShizaiKbn") return;
            SetFocusToDataGridViewFirstRow(GetActiveDataGrid());
        }

        private void SetFocusToDataGridViewFirstRow(DataGrid dataGrid)
        {
            if (dataGrid == null) return;

            if (dataGrid.Items.Count > 0)
            {
                SelectDataGridRow(dataGrid, 0);
            }
        }

        private void SelectDataGridRow(DataGrid dataGrid, int index)
        {
            var dataContext = this.DataContext as InputInventoryActualMeterDataViewModel;
            if (dataContext == null) return;

            dataContext.SelectIndex = index;
            if (dataGrid.SelectedItem == null) return;
            dataGrid.ScrollIntoView(dataGrid.SelectedItem);
            dataGrid.UpdateLayout();

            var firstRow = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(index);
            if (firstRow != null)
            {
                FocusManager.SetIsFocusScope(firstRow, true);
                FocusManager.SetFocusedElement(firstRow, firstRow);
                firstRow.Focus();
            }

            var firstCell = dataGrid.Columns[4].GetCellContent(dataGrid.SelectedItem)?.Parent as DataGridCell;
            if (firstCell != null)
            {
                FocusManager.SetIsFocusScope(firstCell, true);
                FocusManager.SetFocusedElement(firstCell, firstCell);
                firstCell.Focus();

                if (!dataContext.IsFixed)
                {
                    dataGrid.BeginEdit();
                    firstCell.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }
            }
        }

        #endregion
    }
}
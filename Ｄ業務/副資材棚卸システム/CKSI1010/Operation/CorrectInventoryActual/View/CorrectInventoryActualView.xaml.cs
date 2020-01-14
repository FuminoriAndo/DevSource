using CKSI1010.Operation.CorrectInventoryActual.ViewModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
//*************************************************************************************
//
//   当月量修正画面のビュー
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1010.Operation.CorrectInventoryActual.View
{
    /// <summary>
    /// CorrectInventoryActualView.xaml の相互作用ロジック
    /// </summary>
    public partial class CorrectInventoryActualView
    {
        public CorrectInventoryActualView()
        {
            InitializeComponent();
            this.Loaded += CorrectInventoryActualView_Loaded;
        }

        private void CorrectInventoryActualView_Loaded(object sender, RoutedEventArgs e)
        {
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(dataGrid, (s, ev) => SetFocusToDataGridViewFirstRow(dataGrid));
                SetFocusToDataGridViewFirstRow(dataGrid);
            }
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
            var dataContext = this.DataContext as CorrectInventoryActualViewModel;
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

            var firstCell = dataGrid.Columns[5].GetCellContent(dataGrid.SelectedItem)?.Parent as DataGridCell;
            if (firstCell != null)
            {
                FocusManager.SetIsFocusScope(firstCell, true);
                FocusManager.SetFocusedElement(firstCell, firstCell);
                dataGrid.BeginEdit();
                firstCell.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }

        private void textboxTogetu_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
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

        private void textboxTogetu_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Down || e.Key == Key.Up || e.Key == Key.Tab)
            {
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
    }
}


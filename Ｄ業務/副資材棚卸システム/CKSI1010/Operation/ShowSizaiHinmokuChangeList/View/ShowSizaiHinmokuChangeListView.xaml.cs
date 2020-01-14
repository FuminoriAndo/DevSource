//*************************************************************************************
//
//   資材品目の変更履歴画面のビュー
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   17.04.28              DSK   　     新規作成
//
//*************************************************************************************
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace CKSI1010.Operation.ShowSizaiHinmokuChangeList.View
{
    /// <summary>
    /// 資材品目の変更履歴画面のビュー
    /// </summary>
    public partial class ShowSizaiHinmokuChangeListView
    {
        public ShowSizaiHinmokuChangeListView()
        {
            InitializeComponent();
            this.Loaded += ShowSizaiHinmokuChangeListVieww_Loaded;
        }

        private void ShowSizaiHinmokuChangeListVieww_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(dataGrid, (s, ev) => SetFocusToDataGridViewFirstRow(dataGrid));
                SetFocusToDataGridViewFirstRow(dataGrid);
            }
        }

        private static void SetFocusToDataGridViewFirstRow(DataGrid dataGrid)
        {
            if (dataGrid == null) return;

            if (dataGrid.Items.Count > 0)
            {
                var item = dataGrid.Items[0];
                dataGrid.ScrollIntoView(item);
                dataGrid.UpdateLayout();
                dataGrid.SelectedItem = item;
                var firstCell = dataGrid.Columns[0].GetCellContent(item);
                if (firstCell != null)
                {
                    FocusManager.SetIsFocusScope(firstCell, true);
                    FocusManager.SetFocusedElement(firstCell, firstCell);

                    Keyboard.Focus(firstCell);
                }
            }
        }
    }
}

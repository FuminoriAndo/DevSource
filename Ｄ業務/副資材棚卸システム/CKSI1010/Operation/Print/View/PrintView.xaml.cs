using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using System;

namespace CKSI1010.Operation.Print.View
{
    /// <summary>
    /// PrintView.xaml の相互作用ロジック
    /// </summary>
    public partial class PrintView
    {
        public PrintView()
        {
            InitializeComponent();
            this.Loaded += PrintView_Loaded;
        }

        private void PrintView_Loaded(object sender, System.Windows.RoutedEventArgs e)
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

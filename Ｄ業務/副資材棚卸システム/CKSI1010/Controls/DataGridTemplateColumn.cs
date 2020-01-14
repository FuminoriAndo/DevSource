using System.Windows;
using System.Windows.Input;
//*************************************************************************************
//
//   拡張DataGridTemplateColumn
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1010.Controls
{
    /// <summary>
    /// 拡張DataGridTemplateColumn
    /// </summary>
    public class DataGridTemplateColumn : System.Windows.Controls.DataGridTemplateColumn
    {
        #region メソッド
        protected override object PrepareCellForEdit(FrameworkElement editingElement, RoutedEventArgs editingEventArgs)
        {
            editingElement.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
            return base.PrepareCellForEdit(editingElement, editingEventArgs);
        }
        #endregion
    }
}

//*************************************************************************************
//
//   Excel操作の拡張メソッド
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************
using System;
using System.Drawing;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;

namespace Document.Excel
{
    /// <summary>
    /// Excel操作の拡張メソッド
    /// </summary>
    public static class ExcelExtensions
    {
        #region メソッド

        /// <summary>
        /// オートシェープの追加
        /// </summary>
        /// <param name="workSheet">対象のワークシート</param>
        /// <param name="autoShapeType">オートシェープの種類(MsoAutoShapeType)</param>
        /// <param name="left">左位置</param>
        /// <param name="top">上位置</param>
        /// <param name="width">横</param>
        /// <param name="height">縦</param>
        /// <returns>オートシェープの名前</returns>
        public static string AddAutoShape(this Worksheet workSheet,
                                            MsoAutoShapeType autoShapeType,
                                            float left, float top,
                                            float width, float height)
        {
            try
            {
                return workSheet.Shapes.AddShape(autoShapeType, left, top, width, height).Name;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// オートシェープ(塗りつぶしなし)の追加
        /// </summary>
        /// <param name="workSheet">対象のワークシート</param>
        /// <param name="autoShapeType">オートシェープの種類(MsoAutoShapeType)</param>
        /// <param name="left">左位置</param>
        /// <param name="top">上位置</param>
        /// <param name="width">横</param>
        /// <param name="height">縦</param>
        /// <returns>オートシェープの名前</returns>
        public static string AddAutoShapeWithoutFill(this Worksheet workSheet,
                                                     MsoAutoShapeType autoShapeType,
                                                     float left, float top,
                                                     float width, float height)
                                         
        {
            try
            {
                string name = null;

                name = workSheet.Shapes.AddShape(autoShapeType, left, top, width, height).Name;
                workSheet.Shapes.Item(name).Fill.Visible = MsoTriState.msoFalse;
                workSheet.Shapes.Item(name).Line.Visible = MsoTriState.msoTrue;
                workSheet.Shapes.Item(name).Line.Style = MsoLineStyle.msoLineSingle;
                workSheet.Shapes.Item(name).Line.ForeColor.RGB = (int)XlRgbColor.rgbBlack;
                workSheet.Shapes.Item(name).Line.Transparency = 0;
                workSheet.Shapes.Item(name).Line.Weight = 2.0f;

                return name;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// MsoAutoShapeTypeへの変換
        /// </summary>
        /// <param name="autoShapeType">オートシェープの種類(文字列)</param>
        /// <returns>MsoAutoShapeType</returns>
        public static MsoAutoShapeType ConvertMsoAutoShapeType(this string autoShapeType)
        {
            MsoAutoShapeType ret = default(MsoAutoShapeType);
            
            switch(autoShapeType)
            {
                case "RoundedRectangle":
                    ret = MsoAutoShapeType.msoShapeRoundedRectangle;
                    break;
                case "ShapeDownArrow":
                    ret = MsoAutoShapeType.msoShapeDownArrow;
                    break;
                default:
                    break;
            }

            return ret;
        }

        /// <summary>
        /// レンジの取得
        /// </summary>
        /// <param name="workSheet">対象のWorksheet</param>
        /// <param name="cell">対象のセル</param>
        /// <returns>レンジ</returns>
        public static Range Range(this Worksheet workSheet, object cell)

        {
            try
            {
                return workSheet.get_Range(cell, cell);
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// レンジの取得
        /// </summary>
        /// <param name="workSheet">対象のWorksheet</param>
        /// <param name="cell1">対象のセル(From)</param>
        /// <param name="cell2">対象のセル(To)</param>
        /// <returns>レンジ</returns>
        public static Range Range(this Worksheet workSheet, object cell1, object cell2)

        {
            try
            {
                return workSheet.get_Range(cell1, cell2);
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 背景色の設定
        /// </summary>
        /// <param name="workSheet">対象のWorksheet</param>
        /// <param name="cell1">対象のセル(From)</param>
        /// <param name="cell2">対象のセル(To)</param>
        /// <param name="color">背景色</param>
        public static void InteriorColor(this Worksheet workSheet, object cell1, object cell2, XlRgbColor color)

        {
            try
            {
                workSheet.Range(cell1, cell2).Interior.Color = (int)color;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 背景色の設定
        /// </summary>
        /// <param name="workSheet">対象のWorksheet</param>
        /// <param name="row">行番号</param>
        /// <param name="columns">列番号</param>
        /// <param name="color">背景色</param>
        public static void InteriorColor(this Worksheet workSheet, int row, int columns, Color color)
        {
            try
            {
                if (color == Color.Transparent)
                    workSheet.Range(row, columns).Interior.ColorIndex 
                        = Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexNone;
                else
                    workSheet.Range(row, columns).Interior.Color 
                        = ColorTranslator.ToOle(Color.FromArgb(color.R, color.G, color.B));
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 条件付き背景色の設定
        /// </summary>
        /// <param name="workSheet">対象のWorksheet</param>
        /// <param name="cell1">対象のセル(From)</param>
        /// <param name="cell2">対象のセル(To)</param>
        /// <param name="color">背景色</param>
        /// <param name="func">条件(bool型を返却するファンクション)</param>
        public static void InteriorColor(this Worksheet workSheet, object cell1, object cell2, XlRgbColor color, 
                                           Func<bool> func)

        {
            try
            {
                if (func()) workSheet.InteriorColor(cell1, cell2, color);
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 条件付き背景色の設定
        /// </summary>
        /// <typeparam name="T">任意のオブジェクトの型</typeparam>
        /// <param name="workSheet">対象のWorksheet</param>
        /// <param name="cell1">対象のセル(From)</param>
        /// <param name="cell2">対象のセル(To)</param>
        /// <param name="color">背景色</param>
        /// <param name="func">条件(bool型を返却するファンクション)</param>
        /// <param name="param">ファンクションに設定する引数</param>
        public static void InteriorColor<T>(this Worksheet workSheet, object cell1, object cell2, XlRgbColor color, 
                                            Func<T,bool> func, T param)

        {
            try
            {
                if(func(param)) workSheet.InteriorColor(cell1, cell2, color);
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// セルへのデータの設定
        /// </summary>
        /// <param name="workSheet">対象のWorksheet</param>
        /// <param name="row">行番号</param>
        /// <param name="columns">列番号</param>
        /// <param name="data">データ</param>
        public static void SetCellData(this Worksheet workSheet, int row, int columns, string data)
        {
            try
            {
                workSheet.Range(row, columns).Value2 = data;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// フォント色の設定
        /// </summary>
        /// <param name="workSheet">対象のWorksheet</param>
        /// <param name="row">行番号</param>
        /// <param name="columns">列番号</param>
        /// <param name="color">背景色</param>
        public static void FontColor(this Worksheet workSheet, int row, int columns, Color color)
        {
            try
            {
                workSheet.Range(row, columns).Font.Color 
                    = ColorTranslator.ToOle(Color.FromArgb(color.R, color.G, color.B));
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 取消線の設定
        /// </summary>
        /// <param name="workSheet">対象のWorksheet</param>
        /// <param name="row">行番号</param>
        /// <param name="columns">列番号</param>
        /// <param name="isStrikethrough">true:取消/false:取消しない</param>
        public static void StrikeThrough(this Worksheet workSheet, int row, int columns, bool isStrikethrough)
        {
            try
            {
                workSheet.Range(row, columns).Font.Strikethrough = isStrikethrough;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// セルの結合
        /// </summary>
        /// <param name="workSheet">対象のWorksheet</param>
        /// <param name="cell1">対象のセル(From)</param>
        /// <param name="cell2">対象のセル(To)</param>
        public static void CellMerge(this Worksheet workSheet, object cell1, object cell2)

        {
            try
            {
                workSheet.Range(cell1, cell2).Cells.Merge(true);
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 条件付きセルの結合
        /// </summary>
        /// <param name="workSheet">対象のWorksheet</param>
        /// <param name="cell1">対象のセル(From)</param>
        /// <param name="cell2">対象のセル(To)</param>
        /// <param name="func">条件(bool型を返却するファンクション)</param>
        public static void CellMerge(this Worksheet workSheet, object cell1, object cell2, Func<bool> func)

        {
            try
            {
                if (func()) workSheet.Range(cell1, cell2).Cells.Merge(true);
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 条件付きセルの結合
        /// </summary>
        /// <typeparam name="T">任意のオブジェクトの型</typeparam>
        /// <param name="workSheet">対象のWorksheet</param>
        /// <param name="cell1">対象のセル(From)</param>
        /// <param name="cell2">対象のセル(To)</param>
        /// <param name="func">条件(bool型を返却するファンクション)</param>
        /// <param name="param">ファンクションに設定する引数</param>
        public static void CellMerge<T>(this Worksheet workSheet, object cell1, object cell2, 
                                          Func<T, bool> func, T param)

        {
            try
            {
                if (func(param)) workSheet.Range(cell1, cell2).Cells.Merge(true);
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// セルの結合解除
        /// </summary>
        /// <param name="workSheet">対象のWorksheet</param>
        /// <param name="cell1">対象のセル(From)</param>
        /// <param name="cell2">対象のセル(To)</param>
        public static void CellUnMerge(this Worksheet workSheet, object cell1, object cell2)

        {
            try
            {
                workSheet.Range(cell1, cell2).Cells.UnMerge();
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 条件付きセルの結合解除
        /// </summary>
        /// <param name="workSheet">対象のWorksheet</param>
        /// <param name="cell1">対象のセル(From)</param>
        /// <param name="cell2">対象のセル(To)</param>
        /// <param name="func">条件(bool型を返却するファンクション)</param>
        public static void CellUnMerge(this Worksheet workSheet, object cell1, object cell2, Func<bool> func)

        {
            try
            {
                if (func()) workSheet.Range(cell1, cell2).Cells.UnMerge();
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 条件付きセルの結合解除
        /// </summary>
        /// <param name="workSheet">対象のWorksheet</param>
        /// <param name="cell1">対象のセル(From)</param>
        /// <param name="cell2">対象のセル(To)</param>
        /// <param name="func">条件(bool型を返却するファンクション)</param>
        /// <param name="param">ファンクションに設定する引数</param>
        public static void CellUnMerge<T>(this Worksheet workSheet, object cell1, object cell2, 
                                            Func<T, bool> func, T param)

        {
            try
            {
                if (func(param)) workSheet.Range(cell1, cell2).Cells.UnMerge();
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ワークブックの保存
        /// </summary>
        /// <param name="workbook">対象のworkbook</param>
        /// <param name="folder">対象のフォルダ</param>
        /// <param name="bookName">対象のブック名</param>
        public static void Save(this Workbook workbook, string folder, string bookName)
        {
            try
            {
                workbook.SaveAs(folder + bookName,
                               Type.Missing, Type.Missing, Type.Missing,
                               Type.Missing, Type.Missing, XlSaveAsAccessMode.xlNoChange,
                               Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
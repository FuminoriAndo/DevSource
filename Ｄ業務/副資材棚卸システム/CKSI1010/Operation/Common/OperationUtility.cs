using CKSI1010.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
//*************************************************************************************
//
//   操作のユーティリティークラス
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   17.04.28              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1010.Operation.Common
{
    /// <summary>
    /// 操作のユーティリティークラス
    /// </summary>
    internal class OperationUtility
    {
        #region メソッド

        /// <summary>
        /// 立会い用棚卸表印刷表のデータを整形する
        /// </summary>
        /// <param name="src">立会い用棚卸表印刷表のモデル</param>
        /// <returns>サブトータルを含む立会い用棚卸表印刷表のデータ</returns>
        internal static IList<InventoryWitness> ShapeWitnessInventoryData
                                                            (IList<InventoryWitness> src)
        {
            try
            {
                var shapeData = from x in src
                                group x by new { x.Himoku, x.Utiwake, x.Tanaban, x.Tani }
                                  into g
                                orderby g.Key.Himoku, g.Key.Utiwake, g.Key.Tanaban
                                select new InventoryWitness()
                                {
                                    Himoku = g.Key.Himoku,
                                    Utiwake = g.Key.Utiwake,
                                    Tanaban = g.Key.Tanaban,
                                    ZSK = g.Sum(p => p.ZSK),
                                    GenbaSuryo = g.Sum(p => p.GenbaSuryo),
                                    Total = g.Sum(p => p.Total),
                                    HinmokuCD = "????",
                                    Tani = g.Key.Tani.Trim()
                                };

                var sortedList = src.Concat(shapeData)
                                    .OrderBy(t => t.Himoku).ThenBy(t => t.Utiwake).ThenBy(t => t.Tanaban);

                return sortedList.ToList();
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
using CKSI1010.Common;
using CKSI1010.Core;
using CKSI1010.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CKSI1010.Common.Constants;
//*************************************************************************************
//
//   棚卸実績値入力画面(棚卸データ入力作業)のデータ用のビューモデル
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1010.Operation.InputInventoryActual.ViewModel
{
    /// <summary>
    /// 棚卸実績値入力画面(棚卸データ入力作業)のデータ用のビューモデル
    /// </summary>
    public class InputInventoryActualInventoryDataViewModel : InputInventoryActualBaseViewModel
    {
        #region　フィールド

        /// <summary>
        /// Excelアクセッサー
        /// </summary>
        private ExcelAccessor excelAccessor = null;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public InputInventoryActualInventoryDataViewModel(IDataService dataService) : base(dataService)
        {
        }

        #endregion

        #region メソッド

        /// <summary>
        /// 棚卸ワークデータの取得
        /// </summary>
        /// <returns></returns>
        protected override async Task<IEnumerable<InventoryWork>> GetInventoryWork()
        {

            try
            {
                return await DataService.GetInventoriesWorkAsync
                    (SizaiWorkCategory.TanaoroshiWork.GetStringValue());
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 棚卸ワークデータの取得(棚卸マスタのデータをもとにする)
        /// </summary>
        /// <returns>棚卸ワークデータ</returns>
        protected override Task<IEnumerable<InventoryWork>> GetInventoryWorkFromMst()
        {
            return null;
        }

        /// <summary>
        /// 単価入力チェック
        /// </summary>
        protected override async Task<bool> CheckTankaInput()
        {

            bool ret = true;
            var list = await DataService.CheckNotEnteredTanka();

            // 単価未入力の品目が存在するか
            if (list.Count() > 0)
            {
                // Excelアクセッサーのインスタンスの取得
                excelAccessor = ExcelAccessor.GetInstance();
                // 単価未入力リストを出力する
                await excelAccessor.OutputUnInputTankaList(list.ToList());
                ret = false;
            }

            return ret;

        }

        #endregion
    }
}
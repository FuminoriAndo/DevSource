using CKSI1010.Types;
//*************************************************************************************
//
//   資材区分と置場との紐付けの定義
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1010.Common
{
    /// <summary>
    /// 資材区分と置場との紐付けの定義
    /// </summary>
    public class SizaiCategoryTiePlaceDefinition
    {
        #region プロパティ

        /// <summary>
        /// 資材区分
        /// </summary>
        public ShizaiKbnTypes ShizaiKbnTypes { get; set; }

        /// <summary>
        /// 資材区分
        /// </summary>
        public string ShizaiKbnType { get; set; } = string.Empty;

        /// <summary>
        /// 置場
        /// </summary>
        public string PlaceCode { get; set; } = string.Empty;

        #endregion

    }
}

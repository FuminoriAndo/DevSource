//*************************************************************************************
//
//   副資材入出庫表の項目
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.11.16              DSK   　     新規作成
//
//*************************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project1
{
    /// <summary>
    /// 副資材入出庫表の項目
    /// </summary>
    internal class SizaiInOutItem
    {
        #region プロパティ
        /// <summary>
        /// 区分
        /// </summary>
        public string Kbn { get; set; }
        /// <summary>
        /// 品目CD
        /// </summary>
        public string HinmokuCode { get; set; }
        /// <summary>
        /// 品目名
        /// </summary>
        public string HinmokuName { get; set; }
        /// <summary>
        /// 単位
        /// </summary>
        public string Tani { get; set; }
        /// <summary>
        /// 業者コード
        /// </summary>
        public string GyosyaCode { get; set; }
        /// <summary>
        /// 業者名
        /// </summary>
        public string GyosyaName { get; set; }
        /// <summary>
        /// 向先
        /// </summary>
        public string Mukesaki { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public string Suryou { get; set; }
        /// <summary>
        /// 水分引
        /// </summary>
        public string Suibunhiki { get; set; }
        #endregion
    }
}

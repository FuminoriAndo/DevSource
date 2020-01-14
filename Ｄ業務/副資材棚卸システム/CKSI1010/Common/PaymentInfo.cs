//*************************************************************************************
//
//   受払い情報
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
    /// 受払い情報
    /// </summary>
    public class PaymentInfo
    {
        #region プロパティ

        /// <summary>
        /// 費目
        /// </summary>
        public string Himoku { get; set; } = string.Empty;

        /// <summary>
        /// 内訳
        /// </summary>
        public string Utiwake { get; set; } = string.Empty;

        /// <summary>
        /// 棚番
        /// </summary>
        public string Tanaban { get; set; } = string.Empty;

        /// <summary>
        /// 品目CD
        /// </summary>
        public string HinmokuCode { get; set; } = string.Empty;

        /// <summary>
        /// 口座名
        /// </summary>
        public string KouzaName { get; set; } = string.Empty;

        #endregion
    }
}
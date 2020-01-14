using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Linq;
//*************************************************************************************
//
//   実施年月
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   17.04.28              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1090.Shared.ViewModel
{
    /// <summary>
    /// 実施年月
    /// </summary>
    public class OperationYearMonth : ViewModelBase
    {
        #region フィールド

        /// <summary>
        /// 実施年
        /// </summary>
        private int year;

        /// <summary>
        /// 実施月
        /// </summary>
        private int month;

        /// <summary>
        /// 期数
        /// </summary>
        private int term;

        /// <summary>
        /// 上期/下期
        /// </summary>
        private int half;

        /// <summary>
        /// 期末か
        /// </summary>
        private bool termEnd;

        #endregion

        #region プロパティ

        /// <summary>
        /// 期末月一覧
        /// </summary>
        internal List<int> TermEndMonths { get; } = new List<int>();

        /// <summary>
        /// 実施年
        /// </summary>
        public int Year
        {
            get { return year; }
            set { Set(ref year, value); }
        }

        /// <summary>
        /// 実施月
        /// </summary>
        public int Month
        {
            get { return month; }
            set
            {
                if (Set(ref month, value))
                {
                    TermEnd = TermEndMonths.Any(month => month == this.month);
                }
            }
        }

        /// <summary>
        /// 年月
        /// </summary>
        public int YearMonth
        {
            get
            {
                return Year * 100 + Month;
            }
            set
            {
                Month = value % 100;
                Year = value / 100;
            }
        }

        /// <summary>
        /// 期数
        /// </summary>
        public int Term
        {
            get { return term; }
            set { Set(ref term, value); }
        }

        /// <summary>
        /// 上期/下期
        /// </summary>
        public int Half
        {
            get { return half; }
            set { Set(ref half, value); }
        }

        /// <summary>
        /// 期末か
        /// </summary>
        public bool TermEnd
        {
            get { return termEnd; }
            set { Set(ref termEnd, value); }
        }

        #endregion
    }
}

using CKSI1010.Common;
using CKSI1010.Shared.Model;
using Core;
using System.Collections.Generic;
using static CKSI1010.Common.Constants;
//*************************************************************************************
//
//   共有データ
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1010.Shared
{
    /// <summary>
    /// 共有データ
    /// </summary>
    public class SharedModel : Singleton<SharedModel>
    {
        #region プロパティ

        /// <summary>
        /// 業者一覧
        /// </summary>
        public List<Supplier> Suppliers { get; } = new List<Supplier>();

        /// <summary>
        /// 品目一覧
        /// </summary>
        public List<Material> Items { get; } = new List<Material>();

        /// <summary>
        /// 先月棚卸データ
        /// </summary>
        public List<Inventory> LastInventories { get; } = new List<Inventory>();

        /// <summary>
        /// 入庫データ
        /// </summary>
        public List<WorkNoteItem> Receivings { get; } = new List<WorkNoteItem>();

        /// <summary>
        /// 出庫データ
        /// </summary>
        public List<OutWorkNoteItem> Leavings { get; } = new List<OutWorkNoteItem>();

        /// <summary>
        /// 出庫データ
        /// </summary>
        public List<OutWorkNoteItem> DirectDeliverys { get; } = new List<OutWorkNoteItem>();

        /// <summary>
        /// 返品データ
        /// </summary>
        public List<WorkNoteItem> Returns { get; } = new List<WorkNoteItem>();

        /// <summary>
        /// 期末確定棚卸データ
        /// </summary>
        public List<Material> ItemsOfEndTerm { get; } = new List<Material>();

        /// <summary>
        /// 期末提出項目(費目内訳棚番単位)
        /// </summary>
        public List<SubmitItem> SubmitItemsOfEndTerm { get; } = new List<SubmitItem>();

        /// <summary>
        /// 作業誌データ（液酸入出庫）
        /// </summary>
        public List<MeterDataWorkNoteItem> MeterDataWorkNoteData { get; } = new List<MeterDataWorkNoteItem>();

        /// 起動引数
        /// </summary>
        public StartupArgs StartupArgs { get; } = new StartupArgs();

        /// <summary>
        /// 作業区分
        /// </summary>
        public SizaiWorkCategory WorkKbn { get; set; }

        /// <summary>
        /// 印刷対象外の受払い(使用高払い)情報
        /// </summary>
        public IDictionary<int, IList<PaymentInfo>> ExcludeUsePaymentInfo { get; set; } = new Dictionary<int, IList<PaymentInfo>>();

        /// <summary>
        /// 印刷対象外の受払い(入庫払い)情報
        /// </summary>
        public IDictionary<int, IList<PaymentInfo>> ExcludeInputPaymentInfo { get; set; } = new Dictionary<int, IList<PaymentInfo>>();

        /// <summary>
        /// 全選択されているか(使用高払い)
        /// </summary>
        public bool IsUsePaymentSelectAll { get; set; } = false;

        /// <summary>
        /// 選択状態の初期化がされたか(使用高払い)
        /// </summary>
        public bool IsUsePaymentSelectInitialized { get; set; } = false;

        /// <summary>
        /// 全選択されているか(入庫払い)
        /// </summary>
        public bool IsInputPaymentSelectAll { get; set; } = false;

        /// <summary>
        /// 選択状態の初期化がされたか(入庫払い)
        /// </summary>
        public bool IsInputPaymentSelectInitialized { get; set; } = false;

        #endregion

    }
}

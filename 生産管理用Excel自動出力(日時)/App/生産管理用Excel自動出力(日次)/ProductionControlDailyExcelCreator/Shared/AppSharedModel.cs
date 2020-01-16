//*************************************************************************************
//
//   共有データ
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************
using System.Collections.Generic;
using Model;
using ProductionControlDailyExcelCreator.Shared.Base;
using ProductionControlDailyExcelCreator.Log;

namespace ProductionControlDailyExcelCreator.Shared
{
    /// <summary>
    /// 共有データ
    /// </summary>
    public class AppSharedModel : SingletonModelBase<AppSharedModel>
    {
        #region プロパティ

        /// <summary>
        /// アプリケーションタイトル
        /// </summary>
        internal string ApplicationTitle { get; set; }

        /// <summary>
        /// 設定ファイルのパス
        /// </summary>
        internal string SettingFilePath { get; set; }

        /// <summary>
        /// メッセージ文言ファイルのパス
        /// </summary>
        internal string MessageFilePath { get; set; }

        /// <summary>
        /// PMGQ010Bのモデル一覧
        /// </summary>
        internal IList<PMGQ010BModel> PMGQ010BModels = new List<PMGQ010BModel>();

        /// <summary>
        /// PMPA260Bのモデル一覧
        /// </summary>
        internal IList<PMPA260BModel> PMPA260BModels = new List<PMPA260BModel>();

        /// <summary>
        /// PMPD330Bのモデル一覧
        /// </summary>
        internal IList<PMPD330BModel> PMPD330BModels = new List<PMPD330BModel>();

        /// <summary>
        /// PMPF070Bのモデル一覧
        /// </summary>
        internal IList<PMPF070BModel> PMPF070BModels = new List<PMPF070BModel>();

        /// <summary>
        /// PMPF090Bのモデル一覧
        /// </summary>
        internal IList<PMPF090BModel> PMPF090BModels = new List<PMPF090BModel>();

        /// <summary>
        /// PQGA186Bのモデル一覧
        /// </summary>
        internal IList<PQGA186BModel> PQGA186BModels = new List<PQGA186BModel>();

        /// <summary>
        /// PQGA380Bのモデル一覧
        /// </summary>
        internal IList<PQGA380BModel> PQGA380BModels = new List<PQGA380BModel>();

        /// <summary>
        /// PQGA420Bのモデル一覧
        /// </summary>
        internal IList<PQGA420BModel> PQGA420BModels = new List<PQGA420BModel>();

        /// <summary>
        /// PQGA820Bのモデル一覧
        /// </summary>
        internal IList<PQGA820BModel> PQGA820BModels = new List<PQGA820BModel>();

        /// <summary>
        /// SSYM040Bのモデル一覧
        /// </summary>
        internal IList<SSYM040BModel> SSYM040BModels = new List<SSYM040BModel>();

        /// <summary>
        /// SSYM050Bのモデル一覧
        /// </summary>
        internal IList<SSYM050BModel> SSYM050BModels = new List<SSYM050BModel>();

        /// <summary>
        /// SSZA040Bのモデル一覧
        /// </summary>
        internal IList<SSZA040BModel> SSZA040BModels = new List<SSZA040BModel>();

        /// <summary>
        /// 対象年
        /// </summary>
        internal string Year { get; set; }

        /// <summary>
        /// 対象月
        /// </summary>
        internal string Month { get; set; }

        /// <summary>
        /// カスタムログのマネージャ
        /// </summary>
        internal CustomLogManager LogManager { get; set; }

        #endregion
    }
}

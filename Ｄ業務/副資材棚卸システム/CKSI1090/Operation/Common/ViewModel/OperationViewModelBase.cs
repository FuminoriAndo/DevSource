using AutoMapper;
using CKSI1090.Shared;
using CKSI1090.Shared.Model;
using CKSI1090.Shared.ViewModel;
using GalaSoft.MvvmLight;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Controls;
//*************************************************************************************
//
//   各棚卸操作の基底となるビューモデル
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   17.04.28              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1090.Operation.Common.ViewModel
{
    /// <summary>
    /// 各棚卸操作の基底となるビューモデル
    /// </summary>
    public abstract class OperationViewModelBase : ViewModelBase
    {

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dataService">データサービス</param>
        internal OperationViewModelBase(IDataService dataService)
        {
            DataService = dataService;
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// データサービス
        /// </summary>
        protected IDataService DataService { get; private set; }

        /// <summary>
        /// ビジー状態
        /// </summary>
        public BusyStatus BusyStatus => SharedViewModel.Instance.BusyStatus;

        #endregion

        #region メソッド

        /// <summary>
        /// 初期化
        /// </summary>
        /// <returns>非同期タスク</returns>
        internal virtual Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// ×ボタン
        /// </summary>
        /// <returns>非同期タスク</returns>
        internal virtual Task<bool> CloseAsync()
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// プロパティの移送
        /// </summary>
        /// <typeparam name="TSource">対象となるオブジェクト</typeparam>
        /// <typeparam name="TDistination">対象となるオブジェクト</typeparam>
        /// <param name="source">コピー元</param>
        /// <param name="distination">コピー先</param>
        internal bool CopyProperties<TSource, TDistination>(TSource source, out TDistination distination)
        {
            try
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<TSource, TDistination>();
                });

                var mapper = config.CreateMapper();
                distination = mapper.Map<TDistination>(source);

            }

            catch (Exception)
            {
                throw;
            }

            return true;
        }

        /// <summary>
        /// 画面を生成する
        /// </summary>
        public UserControl CreateWindow(string windowName)
        {
            var tempControl = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(
                eachType => eachType.Name == windowName);

            var userControl = (UserControl)Activator.CreateInstance(tempControl);
            return userControl;
        }

        #endregion
    }
}
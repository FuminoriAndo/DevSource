//*************************************************************************************
//
//   シングルトン
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ   担当者    修正内容
//   18.07.10             DSK吉武   新規作成
//
//*************************************************************************************

namespace ProductionControlDailyExcelCreator.Shared.Base
{
    /// <summary>
    /// シングルトン
    /// </summary>
    /// <typeparam name="T">シングルトン化するクラスの型</typeparam>
    public class SingletonModelBase<T> where T : class, new()
    {
        #region フィールド

        /// <summary>
        /// インスタンス
        /// </summary>
        private static volatile T instance;

        /// <summary>
        /// ロック用
        /// </summary>
        private static readonly object locked = new object();

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        protected SingletonModelBase()
        {
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// インスタンス
        /// </summary>
        public static T Instance
        {
            get
            {
                if (instance != null)
                {
                    return instance;
                }

                lock (Locked)
                {
                    return instance ?? (instance = new T());
                }
            }
        }

        /// <summary>
        /// ロック用
        /// </summary>
        public static object Locked
        {
            get
            {
                return locked;
            }
        }

        #endregion

    }
}

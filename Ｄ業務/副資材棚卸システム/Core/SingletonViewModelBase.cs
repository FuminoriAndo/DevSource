using GalaSoft.MvvmLight;
//*************************************************************************************
//
//   シングルトンビューモデル
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************

namespace Core
{
    /// <summary>
    /// シングルトンビューモデル
    /// </summary>
    /// <typeparam name="T">シングルトン化するクラスの型</typeparam>
    public class SingletonViewModelBase<T> : ViewModelBase where T : class, new()
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
        protected SingletonViewModelBase()
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

                lock (locked)
                {
                    return instance ?? (instance = new T());
                }
            }
        }

        #endregion

    }
}

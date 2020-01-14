using GalaSoft.MvvmLight;
//*************************************************************************************
//
//   �V���O���g���r���[���f��
//
//   �C������
//
//   �C���N����  �q����  �@�S���ҁ@�@ �@�C�����e
//   16.12.12              DSK   �@     �V�K�쐬
//
//*************************************************************************************

namespace Core
{
    /// <summary>
    /// �V���O���g���r���[���f��
    /// </summary>
    /// <typeparam name="T">�V���O���g��������N���X�̌^</typeparam>
    public class SingletonViewModelBase<T> : ViewModelBase where T : class, new()
    {
        #region �t�B�[���h

        /// <summary>
        /// �C���X�^���X
        /// </summary>
        private static volatile T instance;

        /// <summary>
        /// ���b�N�p
        /// </summary>
        private static readonly object locked = new object();

        #endregion

        #region �R���X�g���N�^

        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        protected SingletonViewModelBase()
        {
        }

        #endregion

        #region �v���p�e�B

        /// <summary>
        /// �C���X�^���X
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

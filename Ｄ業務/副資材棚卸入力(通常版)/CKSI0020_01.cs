namespace Project1
{
    static class CKSI0020_01
    {
        public static string[] G_AREA = new string[11];
        //棚卸年月
        public static string G_NENGETU;

        //棚卸入力用ワークエリア数
        public const short G_WRK_MAX = 2000;
        public struct TANAOROSI_WORK
        {
            //棚卸入力用ワークエリア
            //削除フラグ
            public string SFLG;
            //品目コード
            public string HINMOKUCD;
            //業者コード
            public string GYOSYACD;
            //品目名
            public string HINMOKUNM;
            //業者名
            public string GYOSYANM;
            //在庫量（倉庫）
            public string ZSOUKO;
            //在庫量（ＥＦ）
            public string ZEF;
            //在庫量（ＬＦ）
            public string ZLF;
            //在庫量（ＣＣ）
            public string ZCC;
            //在庫量（その他）
            public string ZSONOTA;
            //在庫量（メータ）
            public string ZMETER;
        }
        public static TANAOROSI_WORK[] TANAOROSI = new TANAOROSI_WORK[G_WRK_MAX + 1];

        public struct OLD_KEY
        {
            //棚卸削除用ワークエリア
            //品目コード
            public string HINMOKUCD;
            //業者コード
            public string GYOSYACD;
        }

        public static OLD_KEY[] ST_OLD_KEY = new OLD_KEY[10];

        public static string G_USERID;
        public static string G_OFFICEID;
        public static string G_SYOKUICD;

        public static string G_CD;
        public static string G_SYUBETU;
        public static string G_GYOSYANM;
        public static string[] G_HINMOKUNM = new string[11];
        public static short G_Flg;

        //   13.07.02   ISV-TRUC    品目ＣＤに入力により、向先項目にフォーカスをセットする。
        public static string G_MUKESAKI;
         //   13.07.02   ISV-TRUC end
    }
}

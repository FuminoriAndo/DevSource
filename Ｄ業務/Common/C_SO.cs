
namespace Project1
{
    static class SO
    {
        //*************************************************************************************
        //
        //
        //   プログラム名　　　ＳＯ関数
        //
        //
        //   修正履歴
        //
        //   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
        //   97.08.22             NTIS大和    プロダクト更新項目追加（SO_ProductAssign）
        //   97.09.22             NTIS田中    SO初期化処理追加（SO_Common）
        //                                    各ルーチンの初期化処理削除
        //   98.05.07             NTIS伊東    件名が20文字を超えた場合は文字カット（SO_WorkflowAssign）
        //*************************************************************************************
        //-------------------------------------------------------------
        //プログラム全般変数宣言
        //-------------------------------------------------------------
        //        1         2         3         4         5         6         7         8
        //2345678901234567890123456789012345678901234567890123456789012345678901234567890


        //StarOfficeセッションコントロール用共用変数
        //StarOfficeユーザID
        public static string SO_USERID = string.Empty;
        //StarOfficeユーザ名
        public static string SO_USERNAME = string.Empty;
        //StarOfficeオフィスID
        public static string SO_OFFICEID = string.Empty;
        //StarOfficeオフィス名
        public static string SO_OFFICENAME = string.Empty;
        //StarOffice作業オフィスでの職位コード
        public static string SO_USERRANK = string.Empty;
        //StarOfficeリターンコード
        public static string SO_STATUS = string.Empty;

        //StarOffice作業コントロール用共用変数
        //フローインスタンスID
        public static string SO_INSTANCEID = string.Empty;
        //フローインスタンス名（件名）
        public static string SO_INSTANCENAME = string.Empty;
        //到着状態
        public static string SO_RECIEVEDSTATE = string.Empty;
        //作業期限
        public static string SO_TIMELIMIT = string.Empty;
        //作業状態
        public static string SO_WORKSTATE = string.Empty;
        //重要度
        public static string SO_IMPORTANCE = string.Empty;
        //差業種別
        public static string SO_WORKTYPE = string.Empty;
        //作業名
        public static string SO_ACTIVITYNAME = string.Empty;
        //コメント
        public static string SO_COMMENT = string.Empty;

        //StarOfficeプロダクトコントロール共用変数
        //入力者番号
        public static string SOPR_USERNO = string.Empty;
        //入力オフィス番号
        public static string SOPR_OFFICENO = string.Empty;
        //最終承認職位コード
        public static string SOPR_ENDRANK = string.Empty;
        //起票番号
        public static string SOPR_NO = string.Empty;
        //完了フラグ
        public static short SOPR_ENDFLG = 0;
        //入力オフィス名
        public static string SOPR_OFFICENAME = string.Empty;
        //入力者名
        public static string SOPR_USERNAME = string.Empty;
        //汎用プロダクト
        public static object SOPR_TEMP = null;
        //97-08-06 ins
        //オフィス最終職位
        public static short SOPR_OFFICEMUSTER = 0;
        //完了フラグ２
        public static short SOPR_ENDFLG2 = 0;
        //支払番号
        public static short SOPR_SEINO = 0;


        //StarOfficeAPIオブジェクト
        //StarOfficeセッションコントロール
        public static object Sofs = null;
        //StarOffice作業コントロール
        public static object SOFW = null;
        //StarOfficeプロダクトコントロール
        public static object SOFP = null;
        //StarOffice一覧コントロール
        public static object SOFL = null;

        //その他共通変数
        //確定フラグ
        public static string CU_CheckFlg = string.Empty;
        //
        public static string CU_CustProductName = string.Empty;


        // StarOffice用
        #if SO

        public static void SO_LoginGet()
        {
            //-------------------------------------------------------------
            //メインフォームロード処理
            //-------------------------------------------------------------
            //        1         2         3         4         5         6         7         8
            //2345678901234567890123456789012345678901234567890123456789012345678901234567890

            //Set Sofs = CreateObject("SOFAPI.SOFSession")

            Sofs.StartSession();
            if (Sofs.Status == 0)
            {

                SO_USERID = Sofs.UserID;
                //ログインユーザID取得
                SO_USERNAME = Sofs.UserName;
                //ログインユーザ名取得
                SO_OFFICEID = Sofs.OFFICEID;
                //ログインユーザのオフィスID取得
                SO_OFFICENAME = Sofs.OfficeName;
                //ログインユーザのオフィス名取得
                SO_USERRANK = Sofs.userrank;
                //ログインユーザの職位コード
            }
            else
            {

                SO_Error(ref Sofs.Status);
                TM_End();
                Sofs.Endsession();
                System.Environment.Exit(0);

            }

            Sofs.Endsession();

        }

        public static void SO_WorkflowGet()
        {
            //-------------------------------------------------------------
            //StarOffice作業情報取得処理
            //-------------------------------------------------------------
            //        1         2         3         4         5         6         7         8
            //2345678901234567890123456789012345678901234567890123456789012345678901234567890

            Sofs.StartSession();
            //StarOfficeサーバとのセッション開始


            if (Sofs.Status == 0)
            {
                SOFW.SessionHandle = Sofs.SessionHandle;
                SOFW.GetAttribute();


                if (SOFW.Status == 0)
                {
                    SO_WORKSTATE = SOFW.WorkState;
                    //作業状態取得

                    SO_INSTANCEID = SOFW.InstanceID;
                    //インスタンスID取得
                    SO_INSTANCENAME = SOFW.InstanceName;
                    //件名取得
                    SO_RECIEVEDSTATE = SOFW.ReceivedState;
                    //到着状態取得
                    SO_TIMELIMIT = SOFW.TimeLimit;
                    //作業期限取得
                    SO_IMPORTANCE = SOFW.Importance;
                    //重要度取得
                    SO_WORKTYPE = SOFW.WorkType;
                    //作業種別取得
                    SO_ACTIVITYNAME = SOFW.ActivityName;
                    //作業名取得
                    SO_COMMENT = SOFW.Comment;
                    //コメント取得


                }
                else
                {
                    SO_Error(ref SOFW.Status);
                    TM_End();
                    Sofs.Endsession();
                    System.Environment.Exit(0);

                }


            }
            else
            {
                SO_Error(ref Sofs.Status);
                //StarOfficeエラー処理の呼出し
                TM_End();
                //AP異常終了処理
                Sofs.Endsession();
                System.Environment.Exit(0);

            }

            Sofs.Endsession();
            //StarOfficeサーバとのセッション終了

        }

        public static void SO_ProductGet()
        {
            //-------------------------------------------------------------
            //StarOfficeプロダクト情報取得処理
            //-------------------------------------------------------------
            //
            //このStarOfficeプロダクト情報の取得処理は、フロー毎にプロダクトが変わる
            //ため、フロー毎に個別のプロダクトがある場合は、ここ追記する事。
            //
            //        1         2         3         4         5         6         7         8
            //2345678901234567890123456789012345678901234567890123456789012345678901234567890

            Sofs.StartSession();
            //StarOfficeサーバとのセッション開始


            if (Sofs.Status == 0)
            {
                SOFP.SessionHandle = Sofs.SessionHandle;

                //プロダクト起票番号を取得
                SOFP.ProductName = "起票番号";
                SOFP.GetData();
                if (SOFP.Status == 0)
                {
                    SOPR_NO = SOFP.productvalue;
                }
                else
                {
                    SO_Error(ref Sofs.Status);
                    TM_End();
                    Sofs.Endsession();
                    System.Environment.Exit(0);
                }

                //プロダクト入力オフィス番号を取得
                SOFP.ProductName = "入力オフィス番号";
                SOFP.GetData();
                if (SOFP.Status == 0)
                {
                    SOPR_OFFICENO = SOFP.productvalue;
                }
                else
                {
                    SO_Error(ref Sofs.Status);
                    TM_End();
                    Sofs.Endsession();
                    System.Environment.Exit(0);
                }

                //プロダクト入力者番号を取得
                SOFP.ProductName = "入力者番号";
                SOFP.GetData();
                if (SOFP.Status == 0)
                {
                    SOPR_USERNO = SOFP.productvalue;
                }
                else
                {
                    SO_Error(ref Sofs.Status);
                    TM_End();
                    Sofs.Endsession();
                    System.Environment.Exit(0);
                }

                //プロダクト入力オフィス名を取得
                SOFP.ProductName = "入力オフィス名";
                SOFP.GetData();
                if (SOFP.Status == 0)
                {
                    SOPR_OFFICENAME = SOFP.productvalue;
                }
                else
                {
                    SO_Error(ref Sofs.Status);
                    TM_End();
                    Sofs.Endsession();
                    System.Environment.Exit(0);
                }

                //プロダクト入力者名を取得
                SOFP.ProductName = "入力者名";
                SOFP.GetData();
                if (SOFP.Status == 0)
                {
                    SOPR_USERNAME = SOFP.productvalue;
                }
                else
                {
                    SO_Error(ref Sofs.Status);
                    TM_End();
                    Sofs.Endsession();
                    System.Environment.Exit(0);
                }

                //プロダクト最終承認職位コードを取得
                SOFP.ProductName = "最終承認者職位コード";
                SOFP.GetData();
                if (SOFP.Status == 0)
                {
                    SOPR_ENDRANK = SOFP.productvalue;
                }
                else
                {
                    SO_Error(ref Sofs.Status);
                    TM_End();
                    Sofs.Endsession();
                    System.Environment.Exit(0);
                }

                //プロダクト完了フラグを取得
                SOFP.ProductName = "完了フラグ";
                SOFP.GetData();
                if (SOFP.Status == 0)
                {
                    SOPR_ENDFLG = SOFP.productvalue;
                }
                else
                {
                    SO_Error(ref Sofs.Status);
                    TM_End();
                    Sofs.Endsession();
                    System.Environment.Exit(0);
                }

                //オフィス最終職位を取得
                SOFP.ProductName = "オフィス最終職位";
                SOFP.GetData();
                if (SOFP.Status == 0)
                {
                    SOPR_OFFICEMUSTER = SOFP.productvalue;
                }
                else
                {
                    SO_Error(ref Sofs.Status);
                    TM_End();
                    Sofs.Endsession();
                    System.Environment.Exit(0);
                }

                //完了フラグ２を取得
                SOFP.ProductName = "完了フラグ二";
                SOFP.GetData();
                if (SOFP.Status == 0)
                {
                    SOPR_ENDFLG2 = SOFP.productvalue;
                }
                else
                {
                    SO_Error(ref Sofs.Status);
                    TM_End();
                    Sofs.Endsession();
                    System.Environment.Exit(0);
                }

                //支払番号を取得
                SOFP.ProductName = "支払番号";
                SOFP.GetData();
                if (SOFP.Status == 0)
                {
                    SOPR_SEINO = SOFP.productvalue;
                }
                else
                {
                    SO_Error(ref Sofs.Status);
                    TM_End();
                    Sofs.Endsession();
                    System.Environment.Exit(0);
                }


            }
            else
            {
                SO_Error(ref Sofs.Status);
                TM_End();
                Sofs.Endsession();
                System.Environment.Exit(0);

            }

            Sofs.Endsession();
            //StarOfficeサーバとのセッション終了

        }

        public static void SO_ProductAssign()
        {
            //-------------------------------------------------------------
            //StarOfficeプロダクト情報登録処理
            //-------------------------------------------------------------
            //
            //このStarOfficeプロダクト情報の取得処理は、フロー毎にプロダクトが変わる
            //ため、フロー毎に個別のプロダクトがある場合は、ここ追記する事。
            //
            //        1         2         3         4         5         6         7         8
            //2345678901234567890123456789012345678901234567890123456789012345678901234567890

            Sofs.StartSession();


            if (Sofs.Status == 0)
            {
                SOFP.SessionHandle = Sofs.SessionHandle;

                //プロダクトを登録
                SOFP.ProductName = "入力者番号";
                SOFP.productvalue = SOPR_USERNO;
                SOFP.ASSIGNData();

                SOFP.ProductName = "入力オフィス番号";
                SOFP.productvalue = SOPR_OFFICENO;
                SOFP.ASSIGNData();

                SOFP.ProductName = "最終承認者職位コード";
                SOFP.productvalue = SOPR_ENDRANK;
                SOFP.ASSIGNData();

                SOFP.ProductName = "起票番号";
                SOFP.productvalue = SOPR_NO;
                SOFP.ASSIGNData();

                SOFP.ProductName = "入力オフィス名";
                SOFP.productvalue = SOPR_OFFICENAME;
                SOFP.ASSIGNData();

                SOFP.ProductName = "入力者名";
                SOFP.productvalue = SOPR_USERNAME;
                SOFP.ASSIGNData();

                SOFP.ProductName = "完了フラグ";
                SOFP.productvalue = SOPR_ENDFLG;
                SOFP.ASSIGNData();
                SOFP.ProductName = "オフィス最終職位";
                SOFP.productvalue = SOPR_OFFICEMUSTER;
                SOFP.ASSIGNData();
                SOFP.ProductName = "完了フラグ二";
                SOFP.productvalue = SOPR_ENDFLG2;
                SOFP.ASSIGNData();
                SOFP.ProductName = "支払番号";
                SOFP.productvalue = SOPR_SEINO;
                SOFP.ASSIGNData();

                if (SOFP.Status != 0)
                {
                    SO_Error(ref SOFP.Status);
                    TM_End();
                    Sofs.Endsession();
                    System.Environment.Exit(0);
                }


            }
            else
            {
                SO_Error(ref Sofs.Status);
                TM_End();
                Sofs.Endsession();
                System.Environment.Exit(0);

            }

            Sofs.Endsession();

        }

        public static void SO_WorkflowAssign()
        {
            //-------------------------------------------------------------
            //StarOffice作業情報登録処理
            //-------------------------------------------------------------
            //        1         2         3         4         5         6         7         8
            //2345678901234567890123456789012345678901234567890123456789012345678901234567890

            Sofs.StartSession();
            //StarOfficeサーバとのセッション開始


            if (Sofs.Status == 0)
            {
                SOFW.SessionHandle = Sofs.SessionHandle;
                //StarOffice制御引き渡し

                SOFW.GetAttribute();
                //StarOffice作業情報取得

                if (SOFW.Status == 0)
                {
                    //20文字を超えたら文字カット
                    if (Strings.Len(SO_INSTANCENAME) > 20)
                    {
                        SOFW.InstanceName = Strings.Mid(SO_INSTANCENAME, 1, 20);
                        //件名登録
                    }
                    else
                    {
                        SOFW.InstanceName = SO_INSTANCENAME;
                        //件名登録
                    }
                    //        SOFW.Importance = SO_INPORTANCE            '重要度取得
                    //        SOFW.Comment = SO_COMMENT                  'コメント取得
                    SOFW.AssignAttribute();
                    //StarOffice作業情報登録
                }
                else
                {
                    SO_Error(ref SOFW.Status);
                    TM_End();
                    Sofs.Endsession();
                    System.Environment.Exit(0);
                }


            }
            else
            {
                SO_Error(ref Sofs.Status);
                //StarOfficeエラー処理の呼出し
                TM_End();
                //AP異常終了処理
                Sofs.Endsession();
                System.Environment.Exit(0);

            }

            Sofs.Endsession();
            //StarOfficeサーバとのセッション終了

        }

        public static void TM_EndRank()
        {
            //-------------------------------------------------------------
            //最終承認職位判定処理
            //-------------------------------------------------------------
            //        1         2         3         4         5         6         7         8
            //2345678901234567890123456789012345678901234567890123456789012345678901234567890


        }

        public static void SO_Error(ref short ER_Status)
        {
            //-------------------------------------------------------------
            //StarOfficeエラー処理
            //-------------------------------------------------------------
            //        1         2         3         4         5         6         7         8
            //2345678901234567890123456789012345678901234567890123456789012345678901234567890

            short Res = 0;
            string Msg = null;
            short Style = 0;
            string Title = null;

            Msg = "エラー番号" + ER_Status + Strings.Chr(13);
            Msg = Msg + "エラーが起きています";
            Style = 16;
            Title = "StarOfficeエラーメッセージ";

            Res = Interaction.MsgBox(Msg, Style, Title);

        }

        public static void TM_End()
        {
            //-------------------------------------------------------------
            //アプリケーション終了処理
            //-------------------------------------------------------------
            //        1         2         3         4         5         6         7         8
            //2345678901234567890123456789012345678901234567890123456789012345678901234567890

            short Res = 0;
            string Msg = null;
            short Style = 0;
            string Title = null;

            Msg = "アプリケーションに異常が発生しました。" + Strings.Chr(13);
            Msg = Msg + "アプリケーションを終了させます。";
            Style = 16;
            Title = "アプリケーション異常終了メッセージ";

            Res = Interaction.MsgBox(Msg, Style, Title);

        }

        public static object SO_CustProductGet(ref string CU_ProductName, ref object SOPR_TEMP)
        {
            object functionReturnValue = null;

            //-------------------------------------------------------------
            //StarOfficeプロダクト情報取得処理（カスタム）
            //-------------------------------------------------------------
            //
            //このStarOfficeプロダクト情報の取得処理は、フロー毎にプロダクトが変わる
            //ため、フロー毎に個別のプロダクトがある場合は、この部品を使用する事。
            //（プロダクトは引数にて呼出し元から引き継がれる）
            //このFunction名へプロダクト値を返す
            //        1         2         3         4         5         6         7         8
            //2345678901234567890123456789012345678901234567890123456789012345678901234567890

            Sofs.StartSession();


            if (Sofs.Status == 0)
            {
                SOFP.SessionHandle = Sofs.SessionHandle;

                //プロダクトを取得
                SOFP.ProductName = CU_ProductName;
                SOFP.GetData();
                if (SOFP.Status == 0)
                {
                    functionReturnValue = SOFP.productvalue;
                }
                else
                {
                    SO_Error(ref SOFP.Status);
                    TM_End();
                    Sofs.Endsession();
                    System.Environment.Exit(0);
                }
            }
            else
            {
                SO_Error(ref Sofs.Status);
                TM_End();
                Sofs.Endsession();
                System.Environment.Exit(0);
            }

            Sofs.Endsession();
            return functionReturnValue;

        }


        public static void SO_CustProductAssign(ref string CU_ProductName, ref object SOPR_TEMP)
        {
            //-------------------------------------------------------------
            //StarOfficeプロダクト情報登録処理（カスタム）
            //-------------------------------------------------------------
            //
            //このStarOfficeプロダクト情報の取得処理は、フロー毎にプロダクトが変わる
            //ため、フロー毎に個別のプロダクトがある場合は、この部品を使用する事。
            //（プロダクトは引数にて呼出し元から引き継がれる）
            //        1         2         3         4         5         6         7         8
            //2345678901234567890123456789012345678901234567890123456789012345678901234567890

            Sofs.StartSession();


            if (Sofs.Status == 0)
            {
                SOFP.SessionHandle = Sofs.SessionHandle;

                //プロダクトを登録
                SOFP.ProductName = CU_ProductName;
                SOFP.productvalue = SOPR_TEMP;
                SOFP.ASSIGNData();

                if (SOFP.Status != 0)
                {
                    SO_Error(ref SOFP.Status);
                    TM_End();
                    Sofs.Endsession();
                    System.Environment.Exit(0);
                }
            }
            else
            {
                SO_Error(ref SOFP.Status);
                TM_End();
                Sofs.Endsession();
                System.Environment.Exit(0);
            }

            Sofs.Endsession();

        }

        public static void SO_Complete()
        {
            //-------------------------------------------------------------
            //StarOffice確定処理
            //-------------------------------------------------------------
            //
            //        1         2         3         4         5         6         7         8
            //2345678901234567890123456789012345678901234567890123456789012345678901234567890

            Sofs.StartSession();
            //StarOfficeサーバとのセッション開始


            if (Sofs.Status == 0)
            {
                SOFW.SessionHandle = Sofs.SessionHandle;
                //StarOffice制御引き渡し

                SOFW.CompleteWork();
                //StarOffice作業完了

                if (SOFW.Status != 0)
                {
                    SO_Error(ref SOFW.Status);
                    TM_End();
                    Sofs.Endsession();
                    System.Environment.Exit(0);
                }


            }
            else
            {
                SO_Error(ref Sofs.Status);
                //StarOfficeエラー処理の呼出し
                TM_End();
                //AP異常終了処理
                Sofs.Endsession();
                System.Environment.Exit(0);

            }

            Sofs.Endsession();
            //StarOfficeサーバとのセッション終了

        }

        public static void SO_Approve()
        {
            //-------------------------------------------------------------
            //StarOffice承認処理
            //-------------------------------------------------------------
            //
            //        1         2         3         4         5         6         7         8
            //2345678901234567890123456789012345678901234567890123456789012345678901234567890

            Sofs.StartSession();
            //StarOfficeサーバとのセッション開始


            if (Sofs.Status == 0)
            {
                SOFW.SessionHandle = Sofs.SessionHandle;
                //StarOffice制御引き渡し

                SOFW.ApproveWork();
                //StarOffice作業承認

                if (SOFW.Status != 0)
                {
                    SO_Error(ref SOFW.Status);
                    TM_End();
                    Sofs.Endsession();
                    System.Environment.Exit(0);
                }


            }
            else
            {
                SO_Error(ref Sofs.Status);
                //StarOfficeエラー処理の呼出し
                TM_End();
                //AP異常終了処理
                Sofs.Endsession();
                System.Environment.Exit(0);

            }

            Sofs.Endsession();
            //StarOfficeサーバとのセッション終了

        }

        public static void SO_Disapprove()
        {
            //-------------------------------------------------------------
            //StarOffice否認処理
            //-------------------------------------------------------------
            //
            //        1         2         3         4         5         6         7         8
            //2345678901234567890123456789012345678901234567890123456789012345678901234567890

            Sofs.StartSession();
            //StarOfficeサーバとのセッション開始


            if (Sofs.Status == 0)
            {
                SOFW.SessionHandle = Sofs.SessionHandle;
                //StarOffice制御引き渡し

                SOFW.DisapproveWork();
                //StarOffice作業否認

                if (SOFW.Status != 0)
                {
                    SO_Error(ref SOFW.Status);
                    TM_End();
                    Sofs.Endsession();
                    System.Environment.Exit(0);
                }


            }
            else
            {
                SO_Error(ref Sofs.Status);
                //StarOfficeエラー処理の呼出し
                TM_End();
                //AP異常終了処理
                Sofs.Endsession();
                System.Environment.Exit(0);

            }

            Sofs.Endsession();
            //StarOfficeサーバとのセッション終了

        }

        public static void SO_Common()
        {
            //-------------------------------------------------------------
            //StarOfficeAPIオブジェクト設定処理
            //-------------------------------------------------------------
            //
            //        1         2         3         4         5         6         7         8
            //2345678901234567890123456789012345678901234567890123456789012345678901234567890

            Sofs = Interaction.CreateObject("SOFAPI.SOFSession");
            SOFW = Interaction.CreateObject("SOFAPI.SOFWorkItem");
            SOFP = Interaction.CreateObject("SOFAPI.SOFProductData");

        }
        #endif
    }
}

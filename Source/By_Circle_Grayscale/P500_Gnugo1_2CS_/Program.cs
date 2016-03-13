/**
 * C# Arrange 2.0 of GNUGO 1.2
 * 
 * modified by Muzudho
 * last modified 2015-11-30
 *  
 * main.c -> Program.cs
 */
/*
                 GNUGO - the game of Go (Wei-Chi)
                Version 1.2   last revised 10-31-95
           Copyright (C) Free Software Foundation, Inc.
                      written by Man L. Li
                      modified by Wayne Iba
        modified by Frank Pursel <fpp%minor.UUCP@dragon.com>
                    documented by Bob Webber
*/
/*
This program is free software; you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation - version 2.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License in file COPYING for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.

Please report any bug/fix, modification, suggestion to

           manli@cs.uh.edu
*/
using Grayscale.GPL.P___160_Collection_.L500_Collection;
using Grayscale.GPL.P___170_GameState__.L500_Struct;
using Grayscale.GPL.P___190_Board______.L063_Word;
using Grayscale.GPL.P___300_Taikyoku___.L500_Taikyoku;
using Grayscale.GPL.P___480_Print______.L500_Print;
using Grayscale.GPL.P160____Collection_.L500_Collection;
using Grayscale.GPL.P190____Board______.L125_WorkingBoard;
using Grayscale.GPL.P190____Board______.L250_Board;
using Grayscale.GPL.P300____Taikyoku___.L500_Taikyoku;
using Grayscale.GPL.P350____RemoveStone.L500_RemoveStones;
using Grayscale.GPL.P407____CompBestMov.L500_BestMove;
using Grayscale.GPL.P409____ComputerB__.L500_Computer;
using Grayscale.GPL.P460____SaveLoad___.L500_SaveLoad;
using Grayscale.GPL.P470____KeyInput___.L500_CommandDriven;
using Grayscale.GPL.P480____Print______.L500_Print;
using Grayscale.GPL.P900____Scenes_____.L500_Scenes;
using System;
using System.IO;

namespace Grayscale.GPL.P999_Gnugo1_2CS_
{
    /// <summary>
    /// C#あれんじ GNUの碁1.2 のメイン・プログラム
    /// </summary>
    class Program
    {
        /// <summary>
        /// C#あれんじ GNUの碁1.2 のメイン・プログラム
        /// 
        /// Gnugo1.2 では、main.c の main関数。
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        static int Main(string[] args)
        {

            bool isContinueGame = false;// Gnugo1.2 では、cont という変数名。continueの略か。

            // タイトル画面
            new Scene_01_TitleImpl().DoScene();

            // 操作説明画面
            new Scene_02_InstructionImpl().DoScene();

            Taikyoku taikyoku;
            if (File.Exists(TaikyokuImpl.SAVE_FILE_NAME_19ZIBAN))
            {
                // セーブファイルが作られていれば、古いゲームを続けます。

                //----------------------------------------
                // 19路盤のゲームを再開
                //----------------------------------------
                isContinueGame = true;

                // 古い対局データの読み込み
                Util_Load.Load(out taikyoku, 19);

                // ファイルを削除します。
                File.Delete(TaikyokuImpl.SAVE_FILE_NAME_19ZIBAN);
            }
            else if (File.Exists(TaikyokuImpl.SAVE_FILE_NAME_9ZIBAN))
            {
                // セーブファイルが作られていれば、古いゲームを続けます。

                //----------------------------------------
                // 9路盤のゲームを再開
                //----------------------------------------
                isContinueGame = true;

                // 古い対局データの読み込み
                Util_Load.Load(out taikyoku, 9);

                // ファイルを削除します。
                File.Delete(TaikyokuImpl.SAVE_FILE_NAME_9ZIBAN);
            }
            else
            {
                //----------------------------------------
                // 9路盤 or 19路盤の選択
                //----------------------------------------
                {
                    // 盤のサイズが決まっていないうちは、対局データを初期設定できません。
                    Scene_03_BoardSizeImpl scene_03 = new Scene_03_BoardSizeImpl();
                    scene_03.DoScene();

                    // TODO: 盤サイズ（仮）。 盤サイズを指定してから決めたい。暫定で、19路盤サイズとします。
                    int boardSize_temp = scene_03.BoardSize;

                    BoardPrinterB boardPrinterB;
                    switch (boardSize_temp)
                    {
                        case 9: boardPrinterB = new N9zibanPrinterImpl(); break;
                        default: boardPrinterB = new N19zibanPrinterImpl(); break;
                    }

                    // C#化にあたって、グローバル変数はこちらに移動しました。
                    taikyoku = new TaikyokuImpl(
                        boardSize_temp,
                        new ComputerPlayerBImpl(),
                        new BoardImpl(boardSize_temp),
                        new MarkingBoardImpl(boardSize_temp),
                        new CountedBoardImpl(boardSize_temp),
                        boardPrinterB
                        );


                    taikyoku.GobanBounds.BoardSize = scene_03.BoardSize;
                    switch (taikyoku.GobanBounds.BoardSize)
                    {
                        case 9:
                            {
                                taikyoku.BoardPrinter = new N9zibanPrinterImpl();
                            }
                            break;
                        default:
                            {
                                taikyoku.BoardPrinter = new N19zibanPrinterImpl();
                            }
                            break;
                    }
                }

                // ハンディーキャップを選択したところで、ゲーム盤を表示します。
                ((BoardPrinterB)taikyoku.BoardPrinter).ShowBoard(taikyoku);


                // 序盤定跡フラグを ON にします。
                for (int index = 0; index < 9; index++)
                {
                    taikyoku.OpeningZyosekiFlag[index] = true;
                }
                taikyoku.OpeningZyosekiFlag[4] = false;// [4]のフラグだけ OFF にします。

                if(taikyoku.GobanBounds.BoardSize==9)
                {
                    // 序盤定石のほとんどは、１９路盤専用です。
                    taikyoku.OpeningZyosekiFlag[1] = false;
                    taikyoku.OpeningZyosekiFlag[2] = false;
                    taikyoku.OpeningZyosekiFlag[3] = false;
                    taikyoku.OpeningZyosekiFlag[5] = false;
                    taikyoku.OpeningZyosekiFlag[6] = false;
                    taikyoku.OpeningZyosekiFlag[7] = false;
                    taikyoku.OpeningZyosekiFlag[8] = false;
                }

                // 盤上の石を全て取り払います。
                for (int i = 0; i < taikyoku.GobanBounds.BoardSize; i++)
                {
                    for (int j = 0; j < taikyoku.GobanBounds.BoardSize; j++)
                    {
                        taikyoku.Goban.Put(new GobanPointImpl(i, j), StoneColor.Empty);
                    }
                }

                // お互いの取られた石数もクリアーします。
                taikyoku.Count_MyCaptured = 0;
                taikyoku.Count_YourCaptured = 0;
            }

            taikyoku.PlayState = GameState.Playing;//ゲーム開始
            taikyoku.Pass = 0;//パス0回
            taikyoku.MyKo.MoveToVanish();// お互いの取った石（コウになるかもしれない）の位置をクリアー
            taikyoku.YourKo.MoveToVanish();

            taikyoku.Random = new Random(
                //0 // 毎回同じランダム値を出すには、種を固定します。
                DateTime.Now.Millisecond // 毎回ランダムにするには、種を時刻などによって変更します。
                );

            if (!isContinueGame)  // 新ゲーム
            {
                //----------------------------------------
                // ハンディーキャップの選択
                //----------------------------------------
                Scene_04_HandicapImpl scene_04 = new Scene_04_HandicapImpl();
                scene_04.DoScene(taikyoku);

                //----------------------------------------
                // 色の選択
                //----------------------------------------
                for (; ; )//無限ループ
                {
                    // 色を尋ねます。
                    Console.Write("\nChoose side(b or w)? ");
                    string command_str = Console.ReadLine();
                    //scanf("%c",ans);

                    if (command_str.StartsWith("b"))
                    {
                        //----------------------------------------
                        // 人間が黒手番（先手）を選んだとき
                        //----------------------------------------
                        taikyoku.MyColor = StoneColor.White; // コンピューターが白
                        taikyoku.YourColor = StoneColor.Black;  // 人間が黒
                        if (scene_04.Handicap != 0)
                        {
                            // 人間がハンディキャップを取ったときは、
                            // 初手として、コンピューターが石を置きます。
                            GobanPoint bestMove;                            
                            Util_BestMove.Generate_BestMove(out bestMove, taikyoku);
                            taikyoku.Goban.Put(bestMove, taikyoku.MyColor);
                        }
                        break;
                    }
                    else if (command_str.StartsWith("w"))
                    {
                        //----------------------------------------
                        // 人間が白手番（後手）を選んだとき
                        //----------------------------------------
                        taikyoku.MyColor = StoneColor.Black; // コンピューターが黒
                        taikyoku.YourColor = StoneColor.White;  // コンピューターが白
                        if (scene_04.Handicap == 0)
                        {
                            // 人間がハンディキャップを取らないときは、
                            // 初手として、コンピューターが石を置きます。
                            GobanPoint bestMove;                            
                            Util_BestMove.Generate_BestMove(out bestMove, taikyoku);
                            taikyoku.Goban.Put(bestMove, taikyoku.MyColor);
                        }
                        // 人間がハンディキャップを取っているときは、コンピューターは初手の石を置かず、
                        // 人間の手番から始まります。
                        break;
                    }

                    // それ以外の入力は、無限ループ
                }
            }

            // 人間の先後番を選んだところで、碁番を再表示します。
            ((BoardPrinterB)taikyoku.BoardPrinter).ShowBoard(taikyoku);

            // メインループ
            while (0 < taikyoku.PlayState)
            {
                // 人間側の指し手を入力してください。
                // 列を示すアルファベットに続いて、行を示す番号を入力してください。
                Console.Write("your move? ");

                // Gnugo1.2 では、move配列。コマンドラインから サイズ10のchar型配列に入れていたが、C#化にあたり、やり方を変えた。
                string command_str = Console.ReadLine();
                //scanf("%s", move);

                GobanPoint yourLocation; // 人間の指し手
                Util_CommandDriven.DoCommand(command_str, out yourLocation, taikyoku);  // 人間の指し手を読込
                if (GameState.Playing == taikyoku.PlayState)
                {
                    if (!yourLocation.IsPass()) // パスでないなら
                    {
                        // 碁番に石を置きます。
                        taikyoku.Goban.Put(yourLocation, taikyoku.YourColor);
                        // コンピューター側の囲われた石を削除します。コウにならないか、注意します。
                        Util_RemoveStones_Surrounded.RemoveStones_Surrounded(taikyoku.MyColor, taikyoku);
                    }

                    if (taikyoku.Pass != 2)
                    {
                        GobanPoint myLocation; // コンピューターの指し手
                        Util_BestMove.Generate_BestMove(out myLocation, taikyoku);
                        if (!myLocation.IsPass()) // パスでなければ
                        {
                            // 碁番に石を置きます。
                            taikyoku.Goban.Put(myLocation, taikyoku.MyColor);
                            // 人間側の囲われた石を削除します。コウにならないか、注意します。
                            Util_RemoveStones_Surrounded.RemoveStones_Surrounded(taikyoku.YourColor, taikyoku);
                        }
                    }
                    ((BoardPrinterB)taikyoku.BoardPrinter).ShowBoard(taikyoku);
                }

                if (taikyoku.Pass == 2)
                {
                    taikyoku.PlayState = GameState.Stop;   // お互いがパスをしたら、ゲーム終了です。
                }
            }

            if (taikyoku.PlayState == GameState.Stop)
            {
                // ゲームを終了します。
                // 負けがわかっている人向けにか、結果画面は飛ばすことができます。
                //getchar();
                Console.Write("Do you want to count score (y or n)? ");
                string command_str = Console.ReadLine();
                //scanf("%c",ans);

                if (command_str.StartsWith("y"))
                {
                    new Scene_99_EndOfGameImpl().DoScene(taikyoku);
                }
            }

            // アプリケーションを正常終了します。
            return 0;
        }
    }
}

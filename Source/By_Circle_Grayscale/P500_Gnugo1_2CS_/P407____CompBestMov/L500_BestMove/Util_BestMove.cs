/**
 * C# Arrange of GNUGO 1.2
 * 
 * modified by Muzudho
 * last modified 2015-11-28
 *  
 * genmove.c -> Util_BestMove.cs
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
using Grayscale.GPL.P___190_Board______.L063_Word;
using Grayscale.GPL.P___300_Taikyoku___.L500_Taikyoku;
using Grayscale.GPL.P160____Collection_.L500_Collection;
using Grayscale.GPL.P340____Liberty____.L250_Liberty;
using Grayscale.GPL.P340____Liberty____.L500_LibertyAll;
using Grayscale.GPL.P403____CompThink__.L075_OwnEye;
using Grayscale.GPL.P403____CompThink__.L250_SasiteSaver;
using Grayscale.GPL.P403____CompThink__.L500_SasiteWinner;
using Grayscale.GPL.P405____CompZyoseki.L500_FindOpeningZyoseki;
using System;

namespace Grayscale.GPL.P407____CompBestMov.L500_BestMove
{
    /// <summary>
    /// コンピューターの次の動きを作ります。
    /// </summary>
    public abstract class Util_BestMove
    {
        /// <summary>
        /// 最善手を探す試行最大回数。
        /// </summary>
        public const int MAXTRY = 400;

        /// <summary>
        /// コンピューターの動きを作ります。
        /// 
        /// Gnugo1.2 では genmove関数。
        /// </summary>
        /// <param name="out_bestMove">コンピューターが選んだ、一番いい石の置き場所</param>
        /// <param name="taikyoku"></param>
        public static void Generate_BestMove
        (
            out GobanPoint out_bestMove,
            Taikyoku taikyoku
        )
        {
            GobanPoint tryLocation; // Gnugo1.2 では、 ti, tj という変数名。
            int bestScore;// Gnugo1.2 では val変数。評価値。
            int tryScore; // Gnugo1.2 では tval変数。試して算出した評価値。
            int try_ = 0;   // トライの数

            // ムーブと評価値を初期化します。
            out_bestMove = new GobanPointImpl(-1, -1);
            bestScore = -1;

            // カレント色の石のリバティー（四方の石を置けるところ）
            // 
            // Gnugo1.2 では、l という名前のグローバル変数。liberty の略だろうか？
            // eval で内容が設定され、その内容は exambord、findsavr、findwinr、suicideで使用されます。
            int[,] libertyOfPiece_eachPoint;

            // 相手（人間）のそれぞれのピースのリバティーを再び数えます。
            Util_CountLibertyAll.Count_LibertyOfPiece_EachPoint(out libertyOfPiece_eachPoint, taikyoku.YourColor, taikyoku);

            // 相手のピースを取ったり、攻めたりする手を探します。
            if (Util_FindWinner.FindSasite(out tryLocation, out tryScore, libertyOfPiece_eachPoint, taikyoku))
            {
                if (bestScore < tryScore) // 新しい最善手を見つけたなら
                {
                    bestScore = tryScore; // 最善の評価値と置き場所を、更新します。
                    out_bestMove.SetLocation(tryLocation);
                }
            }

            // もし脅かされていれば、幾つかのピースを守ります。
            if (Util_SasiteSaver.FindSasite(out tryLocation, out tryScore, libertyOfPiece_eachPoint, taikyoku))
            {
                if (bestScore < tryScore) // 新しい最善手を見つけたなら
                {
                    bestScore = tryScore; // 最善の評価値と置き場所を、更新します。
                    out_bestMove.SetLocation(tryLocation);
                }
            }

            // 新しい動きのためのローカル・プレー・パターンに一致するか試します。
            if (Util_FindOpeningZyoseki.FindPattern(out tryLocation, out tryScore, taikyoku))
            {
                if (bestScore < tryScore) // 新しい最善手を見つけたなら
                {
                    bestScore = tryScore; // 最善の評価値と置き場所を、更新します。
                    out_bestMove.SetLocation(tryLocation);
                }
            }

            // いい手が無ければ、ランダムに打ちます。
            if (bestScore < 0)
            {
                int count_libertyOfPiece;// Gnugo1.2 では、静的グローバル変数 lib でした。

                // くり返し。
                do
                {
                    // 行は 盤幅でランダム。
                    out_bestMove.I = taikyoku.Random.Next() % taikyoku.GobanBounds.BoardSize;

                    // 低いラインを避ける、中央の領域
                    if
                    (
                        out_bestMove.OutOfI(2, 16)//0～1行、または 17～18行だ。
                        ||
                        out_bestMove.ContainsI(6, 12)//6～12行のどこかにある。
                    )
                    {
                        // 振りなおし
                        out_bestMove.I = taikyoku.Random.Next() % taikyoku.GobanBounds.BoardSize;
                        if (out_bestMove.OutOfI(2, 16))//0～1行、または 17～18行だ。
                        {
                            // 振りなおし
                            out_bestMove.I = taikyoku.Random.Next() % taikyoku.GobanBounds.BoardSize;
                        }
                    }

                    // 列は 盤幅 でランダム。
                    out_bestMove.J = taikyoku.Random.Next() % taikyoku.GobanBounds.BoardSize;

                    // 低いラインを避ける、中央の領域
                    if
                    (
                        out_bestMove.OutOfJ(2,16)//0～1列、または 17～18列だ。
                        || // または、
                        out_bestMove.ContainsJ(6,12)//6～12列のどこかにある。
                    )
                    {
                        // 振りなおし
                        out_bestMove.J = taikyoku.Random.Next() % taikyoku.GobanBounds.BoardSize;

                        if (out_bestMove.OutOfJ(2, 16))//0～1列、または 17～18列だ。
                        {
                            // 振りなおし
                            out_bestMove.J = taikyoku.Random.Next() % taikyoku.GobanBounds.BoardSize;
                        }
                    }

                    // リバティーを数えなおし。
                    Util_CountLiberty.Count_LibertyOfPiece(out count_libertyOfPiece, out_bestMove, taikyoku.MyColor, taikyoku);
                    ++try_;
                }
                while
                (
                    try_ < MAXTRY // まだ次もトライできます。
                    &&
                    (
                        // 次の３つの条件のどれかを満たすようなら、再トライします。
                        // （１）ベストムーブか空っぽだ。非合法手かも。
                        // （２）ピースのリバティーが 0～1 しかない。
                        // （３）自分の目を埋める手なら。
                        taikyoku.Goban.LookColor(out_bestMove) != StoneColor.Empty
                        ||
                        count_libertyOfPiece < 2
                        ||
                        Util_OwnEye.IsOwnEye(out_bestMove, taikyoku)
                    )
                );
            }

            if (MAXTRY <= try_)  // 最大試行回数を超えていたら、コンピューターはパス。
            {
                taikyoku.Pass++;

                //----------------------------------------
                // コンピューターの指し手を表示します。
                //----------------------------------------
                Console.WriteLine("I pass.");
                out_bestMove.SetPass();
            }
            else   // 妥当な指し手を指します。
            {
                taikyoku.Pass = 0;

                //----------------------------------------
                // コンピューターの指し手を表示します。
                //----------------------------------------
                Console.Write("my move: ");
                // 座標の列番号を、アスキー文字コードに変換します。
                char a;
                if (out_bestMove.J < 8)
                {
                    a = (char)(out_bestMove.J + 65);
                }
                else
                {
                    a = (char)(out_bestMove.J + 66);
                }            
                Console.Write(a);

                int ii; // Gnugo1.2 では、行i を反転させた変数名が ii。
                ii = taikyoku.GobanBounds.BoardSize - out_bestMove.I;
                if (ii < 10)
                {
                    Console.WriteLine("{0,1}", ii);
                }
                else
                {
                    Console.WriteLine("{0,2}", ii);
                }
            }
        }
    }
}
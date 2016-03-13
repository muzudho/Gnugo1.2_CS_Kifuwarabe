/**
 * C# Arrange of GNUGO 1.2
 * 
 * modified by Muzudho
 * last modified 2015-11-28
 *  
 * findwinr.c -> Util_FindWinner.cs
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
using Grayscale.GPL.P403____CompThink__.L037_FindOpen;
using System.Collections.Generic;

namespace Grayscale.GPL.P403____CompThink__.L500_SasiteWinner
{

    /// <summary>
    /// 相手の石たちを攻撃するコンピューターの次の動きを探します。
    /// 
    /// コンピューターの思考中に使われます。
    /// </summary>
    public abstract class Util_FindWinner
    {
        /// <summary>
        /// 取るか、攻撃する相手のピースを探します。
        /// 
        /// Gnugo1.2 では findwinner 関数。
        /// </summary>
        /// <param name="out_location">次の動きの   行、列番号</param>
        /// <param name="out_score">Gnugo1.2 では *val 引数。評価値</param>
        /// <param name="libertyOfPiece_eachPoint">
        /// カレント色の石のリバティー（四方の石を置けるところ）
        /// 
        /// Gnugo1.2 では、l という名前のグローバル変数。liberty の略だろうか？
        /// eval で内容が設定され、その内容は exambord、findsavr、findwinr、suicideで使用されます。
        /// </param>
        /// <param name="taikyoku"></param>
        /// <returns></returns>
        public static bool FindSasite
        (
            out GobanPoint out_location,
            out int out_score,
            int[,] libertyOfPiece_eachPoint,
            Taikyoku taikyoku
        )
        {
            // 要素数 3 以下のリスト。
            List<GobanPoint> trayLocations = new List<GobanPoint>(3);// Gnugo1.2 では、それぞれ要素数[3]の ti配列、tj配列。

            int tryScore;// Gnugo1.2 では tval 変数。

            out_location = new GobanPointImpl(-1, -1);// 位置i,j
            out_score = -1;  // 評価値

            //
            // リバティー（四方の置けるところ）が３つ以下の相手（人間）の石を探します。
            // つまり、つながっている石（色が異なる石とつながっている場合もあり）か、端にある石ということです。
            //
            for (int m = 0; m < taikyoku.GobanBounds.BoardSize; m++)
            {
                for (int n = 0; n < taikyoku.GobanBounds.BoardSize; n++)
                {
                    GobanPoint location = new GobanPointImpl(m, n);
                    if (
                        // 相手（人間）の石が置いてあり。
                        taikyoku.Goban.At(location) == taikyoku.YourColor
                        &&
                        // リバティーが 3以下。
                        libertyOfPiece_eachPoint[m, n] < 4
                    )
                    {
                        // これからフラグを立てていくためにクリアーします。
                        taikyoku.MarkingBoard.Initmark(taikyoku.GobanBounds.BoardSize);

                        int ct = 0; // 再帰するときに使います。初期値 0 を与えておけば構いません。
                        if
                        (
                            Util_FindOpen.FindOpenLocations(trayLocations, location, taikyoku.YourColor, libertyOfPiece_eachPoint[m, n], taikyoku)
                        )
                        {
                            if (libertyOfPiece_eachPoint[m,n] == 1)
                            {
                                // リバティーが 1 の相手（人間）の石を見つけました。
                                if (out_score < 120) // 評価値が 120 未満なら
                                {
                                    // 評価値120の指し手として、ベストムーブの位置を更新します。
                                    out_score = 120;
                                    out_location.SetLocation(trayLocations[0]);
                                }
                            }
                            else
                            {
                                // リバティーの数の２重ループで、
                                // 0,1 や、0,2 や、 1,2 など、異なるペア u,v を作ります。
                                for (int u = 0; u < libertyOfPiece_eachPoint[m,n]; u++)//自分（コンピューター）という想定
                                {
                                    for (int v = 0; v < libertyOfPiece_eachPoint[m,n]; v++)//相手（人間）という想定
                                    {
                                        if (u != v)
                                        {
                                            int libertyOfPiece_a; // Gnugo1.2 では、グローバル変数 lib = 0 でした。
                                            Util_CountLiberty.Count(out libertyOfPiece_a, trayLocations[u], taikyoku.MyColor, taikyoku);
                                            if (0 < libertyOfPiece_a)    // 妥当な動き
                                            {
                                                // コンピューターの色の石を（試しに）置きます。
                                                taikyoku.Goban.Put(trayLocations[u], taikyoku.MyColor);
                                        
                                                // look ahead opponent move
                                                int libertyOfPiece_b; // Gnugo1.2 では、グローバル変数 lib = 0 でした。
                                                Util_CountLiberty.Count(out libertyOfPiece_b, trayLocations[v], taikyoku.YourColor, taikyoku);
                                                if
                                                (
                                                    1 == libertyOfPiece_a
                                                    &&
                                                    0 < libertyOfPiece_b
                                                )
                                                {
                                                    tryScore = 0;
                                                }
                                                else
                                                {
                                                    tryScore = 120 - 20 * libertyOfPiece_b;
                                                }
                                            
                                                if (out_score < tryScore)
                                                {
                                                    // より評価値の高い指し手を見つけました。ベストムーブの位置を更新します。
                                                    out_score = tryScore;
                                                    out_location.SetLocation(trayLocations[u]);
                                                }

                                                // （試しに置いた）石を取り除きます。盤上を元に戻すだけです。
                                                taikyoku.Goban.Put(trayLocations[u], StoneColor.Empty);
                                            }
                                        }
                                    }//v
                                }//u
                            }
                        }
                    }
                }//n
            }//m

            if (out_score > 0)   // 指し手を見つけた。
            {
                return true;
            }
            else    // 勝者の指し手を見つけるのには失敗した。
            {
                return false;
            }
        }
    }
}